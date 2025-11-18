using MarketData.Model;

namespace MarketData.MarketStack.Tests;

public class GetExchangeTickersTests
{
    [Fact]
    public async Task GetExchangeTickers()
    {
        var messageHandler = new PlaybackMessageHandler("GetExchangeTickers");
        var query = new MarketStackClient(
            new HttpClient(messageHandler) { BaseAddress = new Uri("http://api.marketstack.com/") },
            new MarketStackClientConfig { AccessToken = Settings.AccessToken, RequestLimit = 300 }) as IGetExchangeTickers;

        var exchangeTickers = await query.GetExchangeTickers("XCSE"); // Copenhagen
        await Verify(exchangeTickers);
    }
}
