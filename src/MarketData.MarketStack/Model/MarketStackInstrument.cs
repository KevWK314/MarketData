using System.Text.Json.Serialization;

namespace MarketData.MarketStack.Model;

public record MarketStackInstrument
{
    public required string Name { get; init; }
    public required string Symbol { get; init; }
    [JsonPropertyName("has_intraday")]
    public bool HasIntraday { get; init; }
    [JsonPropertyName("has_eod")]
    public bool HasEod { get; init; }
}