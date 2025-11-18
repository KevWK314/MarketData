using System.Text.Json.Serialization;

namespace MarketData.MarketStack.Model;

public record MarketStackDailyPrice
{
    public required string Symbol { get; init; }
    public required string Exchange { get; init; }
    public DateOnly Date { get; init; }
    public decimal Open { get; init; }
    public decimal High { get; init; }
    public decimal Low { get; init; }
    public decimal Close { get; init; }
    public double Volume { get; init; }
    [JsonPropertyName("adj_high")]
    public decimal AdjHigh { get; init; }
    [JsonPropertyName("adj_low")]
    public decimal AdjLow { get; init; }
    [JsonPropertyName("adj_close")]
    public decimal AdjClose { get; init; }
    [JsonPropertyName("adj_open")]
    public decimal AdjOpen { get; init; }
    [JsonPropertyName("adj_volume")]
    public decimal AdjVolume { get; init; }
    [JsonPropertyName("split_factor")]
    public decimal SplitFactor { get; init; }
    public decimal Dividend { get; init; }
}