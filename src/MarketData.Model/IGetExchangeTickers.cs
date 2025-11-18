using MarketData.Model.Model;

namespace MarketData.Model;

public interface IGetExchangeTickers
{
    Task<IEnumerable<Instrument>> GetExchangeTickers(string exchangeSourceSymbol);
}