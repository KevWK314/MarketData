namespace MarketData.MarketStack.Model;

public record MarketStackError
{
    public string? Code { get; init; }
    public string? Message { get; init; }
    public MarketStackErrorContext? Context { get; init; }
}
