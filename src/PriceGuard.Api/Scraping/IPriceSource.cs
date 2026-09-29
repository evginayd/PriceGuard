namespace PriceGuard.Api.Scraping;

public interface IPriceSource
{
    string SourceName { get; }

    bool CanHandle(Uri productUrl);

    Task<PriceSourceResult> GetPriceAsync(
        Uri productUrl,
        CancellationToken cancellationToken);
}