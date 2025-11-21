using MarketData.MarketStack.Model;
using MarketData.MarketStack.Model.Converters;
using MarketData.Model;
using System.Text.Json;

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

        var dailyPrices = await query.GetDailyPrices("LASP.CO", new DateOnly(2024, 11, 1), new DateOnly(2025, 11, 1));

        await Verify(dailyPrices);
    }

    [Fact(Skip = "Manual")]
    public async Task GetDailyPrices_Manual()
    {
        var query = new MarketStackClient(
            new HttpClient() { BaseAddress = new Uri("http://api.marketstack.com/") },
            new MarketStackClientConfig { AccessToken = Settings.AccessToken, RequestLimit = 1000 }) as IGetDailyPrices;

        var dailyPrices = await query.GetDailyPrices("VZLA", new DateOnly(2020, 11, 18), new DateOnly(2025, 11, 18));

        var json = JsonSerializer.Serialize(dailyPrices);
    }
}
