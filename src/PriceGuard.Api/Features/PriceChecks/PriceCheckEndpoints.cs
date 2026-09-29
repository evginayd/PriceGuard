using PriceGuard.Api.Scraping;

namespace PriceGuard.Api.Features.PriceChecks;

public static class PriceCheckEndpoints
{
    public static IEndpointRouteBuilder MapPriceCheckEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/price-checks")
            .WithTags("Price Checks");

        group.MapPost("/preview", PreviewPrice)
            .WithName("PreviewPrice")
            .WithSummary("Checks a product price without saving it.")
            .WithDescription(
                "Finds a compatible price source and returns " +
                "the current product information.");

        return endpoints;
    }

    private static async Task<IResult> PreviewPrice(
        PreviewPriceRequest request,
        IEnumerable<IPriceSource> priceSources,
        CancellationToken cancellationToken)
    {
        if (!Uri.TryCreate(
                request.Url,
                UriKind.Absolute,
                out var productUrl))
        {
            return Results.ValidationProblem(
                new Dictionary<string, string[]>
                {
                    ["url"] = ["Enter a valid absolute product URL."]
                });
        }

        if (productUrl.Scheme is not ("http" or "https"))
        {
            return Results.ValidationProblem(
                new Dictionary<string, string[]>
                {
                    ["url"] = ["Only HTTP and HTTPS URLs are supported."]
                });
        }

        var priceSource = priceSources.FirstOrDefault(
            source => source.CanHandle(productUrl));

        if (priceSource is null)
        {
            return Results.BadRequest(new
            {
                message = "No price source supports this URL."
            });
        }

        var result = await priceSource.GetPriceAsync(
            productUrl,
            cancellationToken);

        if (!result.Success)
        {
            return Results.UnprocessableEntity(result);
        }

        return Results.Ok(new PricePreviewResponse(
            priceSource.SourceName,
            productUrl.ToString(),
            result.ProductTitle!,
            result.Price!.Value,
            result.Currency,
            result.IsInStock));
    }
}

public sealed record PreviewPriceRequest(string Url);

public sealed record PricePreviewResponse(
    string Source,
    string Url,
    string ProductTitle,
    decimal Price,
    string Currency,
    bool IsInStock);