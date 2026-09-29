using PriceGuard.Api.Scraping;

namespace PriceGuard.Tests.Scraping;

public sealed class FakeStorePriceSourceTests
{
    private readonly FakeStorePriceSource _source = new();

    [Theory]
    [InlineData("https://fakestore.local/products/headphones", true)]
    [InlineData("https://FAKESTORE.LOCAL/products/headphones", true)]
    [InlineData("https://example.com/products/headphones", false)]
    [InlineData("https://fakestore.local.attacker.com/product", false)]
    public void CanHandle_ShouldCheckExactDomain(
        string url,
        bool expected)
    {
        var result = _source.CanHandle(new Uri(url));

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetPriceAsync_ValidUrl_ShouldReturnPrice()
    {
        var url = new Uri(
            "https://fakestore.local/products/headphones");

        var result = await _source.GetPriceAsync(
            url,
            CancellationToken.None);

        Assert.True(result.Success);
        Assert.Equal("Fake Wireless Headphones", result.ProductTitle);
        Assert.Equal(1499.90m, result.Price);
        Assert.Equal("TRY", result.Currency);
        Assert.Null(result.ErrorCode);
    }

    [Fact]
    public async Task GetPriceAsync_BrokenPage_ShouldReturnFailure()
    {
        var url = new Uri(
            "https://fakestore.local/products/broken");

        var result = await _source.GetPriceAsync(
            url,
            CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal("selector_not_found", result.ErrorCode);
        Assert.Null(result.Price);
    }
}