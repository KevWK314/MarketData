namespace MarketData.Model.Model;

public record Exchange
{
    public required string Name { get; init; }
    public required string Mic { get; init; }
    public required string ShortName { get; init; }
    public required string CountryCode { get; init; }
    public required string City { get; init; }
    public required string SourceSymbol { get; init; }
}