using MarketData.Model.Model;

namespace MarketData.Model;

public interface IGetExchanges
{
    Task<IEnumerable<Exchange>> GetExchanges();
}
