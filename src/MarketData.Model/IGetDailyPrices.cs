using MarketData.Model.Model;

namespace MarketData.Model;

public interface IGetDailyPrices
{
    Task<IEnumerable<DailyPrice>> GetDailyPrices(string tickerSourceSymbol, DateOnly from, DateOnly to);
}