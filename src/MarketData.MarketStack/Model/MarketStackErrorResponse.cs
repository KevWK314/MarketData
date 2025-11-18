namespace MarketData.MarketStack.Model;

public record MarketStackErrorResponse
{
    public MarketStackError? Error { get; init; }
}
