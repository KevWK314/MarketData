namespace MarketData.MarketStack.Model;

public record MarketStackExchangeTickers
{
    public required string Name { get; init; }
    public required string Acronym { get; init; }
    public required string Mic { get; init; }
    public required string Country { get; init; }
    public required string City { get; init; }
    public IEnumerable<MarketStackInstrument> Tickers { get; init; } = [];
}