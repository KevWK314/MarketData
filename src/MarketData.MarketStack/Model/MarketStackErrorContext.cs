namespace MarketData.MarketStack.Model;

public record MarketStackErrorContext
{
    public IEnumerable<MarketStackErrorContextSymbol>? Symbols { get; init; }
}
