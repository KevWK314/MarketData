namespace MarketData.MarketStack.Model;

public record MarketStackErrorContextSymbol
{
    public string? Key { get; init; }
    public string? Message { get; init; }
}