namespace MarketData.Model.Model;

public record Instrument
{
    public required string Name { get; init; }
    public required string Ticker { get; init; }
    public required string SourceSymbol { get; init; }
}