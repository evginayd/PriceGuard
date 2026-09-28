using Microsoft.EntityFrameworkCore;
using PriceGuard.Api.Data;
using PriceGuard.Api.Data.Entities;

namespace PriceGuard.Api.Features.Stores;

public static class StoreEndpoints
{
    public static IEndpointRouteBuilder MapStoreEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/stores")
            .WithTags("Stores");

        group.MapGet("", GetStores);
        group.MapPost("", CreateStore);

        return endpoints;
    }

    private static async Task<IResult> GetStores(
        PriceGuardDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var stores = await dbContext.Stores
            .AsNoTracking()
            .OrderBy(store => store.Name)
            .Select(store => new StoreResponse(
                store.Id,
                store.Name,
                store.Domain,
                store.IsActive,
                store.CreatedAt))
            .ToListAsync(cancellationToken);

        return Results.Ok(stores);
    }

    private static async Task<IResult> CreateStore(
        CreateStoreRequest request,
        PriceGuardDbContext dbContext,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["name"] = ["Store name is required."]
            });
        }

        if (string.IsNullOrWhiteSpace(request.Domain))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["domain"] = ["Store domain is required."]
            });
        }

        var normalizedDomain = request.Domain
            .Trim()
            .ToLowerInvariant();

        if (Uri.CheckHostName(normalizedDomain)
            == UriHostNameType.Unknown)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["domain"] = ["Enter a valid domain without http or https."]
            });
        }

        var alreadyExists = await dbContext.Stores
            .AnyAsync(
                store => store.Domain == normalizedDomain,
                cancellationToken);

        if (alreadyExists)
        {
            return Results.Conflict(new
            {
                message = "A store with this domain already exists."
            });
        }

        var store = new Store
        {
            Name = request.Name.Trim(),
            Domain = normalizedDomain
        };

        dbContext.Stores.Add(store);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new StoreResponse(
            store.Id,
            store.Name,
            store.Domain,
            store.IsActive,
            store.CreatedAt);

        return Results.Created(
            $"/api/stores/{store.Id}",
            response);
    }
}

public sealed record CreateStoreRequest(
    string Name,
    string Domain);

public sealed record StoreResponse(
    Guid Id,
    string Name,
    string Domain,
    bool IsActive,
    DateTimeOffset CreatedAt);