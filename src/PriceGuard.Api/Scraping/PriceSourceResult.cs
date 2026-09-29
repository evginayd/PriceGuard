namespace PriceGuard.Api.Scraping;

public sealed record PriceSourceResult(
    bool Success,
    string? ProductTitle,
    decimal? Price,
    string Currency,
    bool IsInStock,
    string? ErrorCode,
    string? ErrorMessage)
{
    public static PriceSourceResult Succeeded(
        string productTitle,
        decimal price,
        string currency,
        bool isInStock)
    {
        return new PriceSourceResult(
            Success: true,
            ProductTitle: productTitle,
            Price: price,
            Currency: currency,
            IsInStock: isInStock,
            ErrorCode: null,
            ErrorMessage: null);
    }

    public static PriceSourceResult Failed(
        string errorCode,
        string errorMessage)
    {
        return new PriceSourceResult(
            Success: false,
            ProductTitle: null,
            Price: null,
            Currency: "TRY",
            IsInStock: false,
            ErrorCode: errorCode,
            ErrorMessage: errorMessage);
    }
}