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

    [Fact]
    public void Des()
    {
        var settings = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        settings.Converters.Add(new CustomDateTimeOffsetConverter());
        var text = """
            {
                "open": 1190.0,
                "high": 1190.0,
                "low": 1170.0,
                "close": 1170.0,
                "volume": 434.0,
                "adj_high": null,
                "adj_low": null,
                "adj_close": 1170.0,
                "adj_open": null,
                "adj_volume": null,
                "split_factor": 1.0,
                "dividend": 0.0,
                "name": "L\u00e5n \u0026amp; Spar Bank A\/S",
                "exchange_code": null,
                "asset_type": null,
                "price_currency": "DKK",
                "symbol": "LASP.CO",
                "exchange": "XCSE",
                "date": "2025-10-31T00:00:00+0000"
            }
            """;

        var price = JsonSerializer.Deserialize<MarketStackDailyPrice>(text, settings);
    }
}
