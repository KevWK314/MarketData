using MarketData.Model;

namespace MarketData.MarketStack.Tests;

public class GetExchangesTests
{
    [Fact]
    public async Task GetExchanges()
    {
        var messageHandler = new PlaybackMessageHandler("GetExchanges");
        var query = new MarketStackClient(
            new HttpClient(messageHandler) { BaseAddress = new Uri("http://api.marketstack.com/") },
            new MarketStackClientConfig { AccessToken = Settings.AccessToken }) as IGetExchanges;

        var exchanges = await query.GetExchanges();
        await Verify(exchanges);
    }
}
