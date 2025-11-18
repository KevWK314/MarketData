using MarketData.Model;

namespace MarketData.MarketStack.Tests;

public class GetDailyPricesTests
{
    [Fact]
    public async Task GetDailyPrices()
    {
        var messageHandler = new PlaybackMessageHandler("GetDailyPrices");
        var query = new MarketStackClient(
            new HttpClient(messageHandler) { BaseAddress = new Uri("http://api.marketstack.com/") },
            new MarketStackClientConfig { AccessToken = Settings.AccessToken, RequestLimit = 1000 }) as IGetDailyPrices;

        var dailyPrices = await query.GetDailyPrices("NOVO-B.XCSE", new DateOnly(2024, 11, 1), new DateOnly(2025, 11, 1));

        await Verify(dailyPrices);
    }
}
