namespace PriceGuard.Api.Scraping;

public sealed class FakeStorePriceSource : IPriceSource
{
    public string SourceName => "Fake Store";

    public bool CanHandle(Uri productUrl)
    {
        return productUrl.Host.Equals(
            "fakestore.local",
            StringComparison.OrdinalIgnoreCase);
    }

    public async Task<PriceSourceResult> GetPriceAsync(
        Uri productUrl,
        CancellationToken cancellationToken)
    {
        if (!CanHandle(productUrl))
        {
            return PriceSourceResult.Failed(
                errorCode: "unsupported_url",
                errorMessage: "Fake Store cannot process this URL.");
        }

        // Gerçek bir HTTP isteğinin gecikmesini simüle ediyoruz.
        await Task.Delay(100, cancellationToken);

        // Scraper'ın HTML değişikliği nedeniyle bozulmasını simüle eder.
        if (productUrl.AbsolutePath.Contains(
                "broken",
                StringComparison.OrdinalIgnoreCase))
        {
            return PriceSourceResult.Failed(
                errorCode: "selector_not_found",
                errorMessage: "The product price selector could not be found.");
        }

        if (productUrl.AbsolutePath.Contains(
                "keyboard",
                StringComparison.OrdinalIgnoreCase))
        {
            return PriceSourceResult.Succeeded(
                productTitle: "Fake Mechanical Keyboard",
                price: 899.90m,
                currency: "TRY",
                isInStock: true);
        }

        return PriceSourceResult.Succeeded(
            productTitle: "Fake Wireless Headphones",
            price: 1499.90m,
            currency: "TRY",
            isInStock: true);
    }
}