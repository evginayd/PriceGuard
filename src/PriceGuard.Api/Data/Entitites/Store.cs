namespace PriceGuard.Api.Data.Entities;

public sealed class Store
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string Domain { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }
        = DateTimeOffset.UtcNow;
}