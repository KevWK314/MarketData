using MarketData.MarketStack.Model;

namespace MarketData.MarketStack;

public class MarketStackException : Exception
{
	public MarketStackException(MarketStackError marketStackError)
        : base($"{marketStackError.Code ?? "NoCode"} - {marketStackError.Message ?? "No message"}")
	{
        MarketStackError = marketStackError;
    }

    public MarketStackError MarketStackError { get; }
}
