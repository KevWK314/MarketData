using MarketData.MarketStack.Model;
using MarketData.MarketStack.Model.Converters;
using MarketData.Model;
using MarketData.Model.Model;
using System.Net.Http.Json;
using System.Text.Json;

namespace MarketData.MarketStack;

public interface IMarketStackClient :
    IGetExchanges,
    IGetExchangeTickers,
    IGetDailyPrices
{
}

public class MarketStackClient : IMarketStackClient
{
    private readonly HttpClient _httpClient;
    private readonly MarketStackClientConfig _config;

    public static JsonSerializerOptions SerializerOptions = MarketStackSerializerOptions();

    public MarketStackClient(HttpClient httpClient, MarketStackClientConfig config)
    {
        _httpClient = httpClient;
        _config = config;

        if (_httpClient.BaseAddress == null)
            _httpClient.BaseAddress = _config.BaseAddress;
    }

    public async Task<IEnumerable<Exchange>> GetExchanges()
    {
        var exchanges = await Get<MarketStackExchange[], MarketStackExchange>("v2/exchanges", x => x);
        return exchanges.Select(x =>
            new Exchange
            {
                Name = x.Name,
                Mic = x.Mic,
                CountryCode = x.CountryCode,
                City = x.City,
                ShortName = x.Acronym,
                SourceSymbol = x.Mic
            });
    }

    public async Task<IEnumerable<Instrument>> GetExchangeTickers(string exchangeSourceSymbol)
    {
        var exchanges = await Get<MarketStackExchangeTickers, MarketStackInstrument>(
            $"v2/exchanges/{exchangeSourceSymbol}/tickers",
            x => x.Tickers);
        return exchanges.Select(x =>
            new Instrument
            {
                Name = x.Name,
                Ticker = x.Symbol,
                SourceSymbol = x.Symbol
            });
    }

    public async Task<IEnumerable<DailyPrice>> GetDailyPrices(string tickerSourceSymbol, DateOnly from, DateOnly to)
    {
        var prices = await Get<MarketStackDailyPrice[], MarketStackDailyPrice>(
            $"v2/eod",
            new Dictionary<string, string>
            {
                { "symbols", tickerSourceSymbol },
                { "date_from", $"{from:yyyy-MM-dd}" },
                { "date_to", $"{to:yyyy-MM-dd}" }
            },
            x => x);

        return prices.Select(x =>
            new DailyPrice
            {
                Date = DateOnly.FromDateTime(x.Date.Date),
                Open = x.Open,
                Close = x.Close,
                High = x.High,
                Low = x.Low,
                Volume = x.Volume
            });
    }

    private Task<IEnumerable<TData>> Get<TResponse, TData>(string uri, Func<TResponse, IEnumerable<TData>> getData)
    {
        return Get<TResponse, TData>(uri, new Dictionary<string, string>(), getData);
    }

    private async Task<IEnumerable<TData>> Get<TResponse, TData>(string uri, IDictionary<string, string> input, Func<TResponse, IEnumerable<TData>> getData)
    {
        var data = new List<TData>();
        await foreach (var response in Request<TResponse>(uri, input))
        {
            if (response.Data is null)
                throw new InvalidOperationException("There was no data where data was expected");

            data.AddRange(getData(response.Data));
        }

        return data;
    }

    private async IAsyncEnumerable<MarketDataResponse<TData>> Request<TData>(string uri, IDictionary<string, string> input)
    {
        var offset = 0;
        while (offset >= 0)
        {
            var fullUri = $"{uri}?access_key={_config.AccessToken}&offset={offset}&limit={_config.RequestLimit}";
            if (input?.Any() ?? false)
                fullUri += "&" + string.Join("&", input.Select(x => $"{x.Key}={x.Value}"));

            using var response = await _httpClient.GetAsync(fullUri);
            await ValidateResponse(response);

            var marketDataResponse = await response.Content.ReadFromJsonAsync<MarketDataResponse<TData>>(SerializerOptions)
                ?? throw new InvalidOperationException("Unable to deserialize");
            yield return marketDataResponse;

            offset += _config.RequestLimit;
            if (offset >= marketDataResponse.Pagination?.Total)
                break;
        }
    }

    private static async Task ValidateResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        var error = await response.Content.ReadFromJsonAsync<MarketStackError>() ??
            throw new InvalidOperationException("Unable to deserialise");
        throw new MarketStackException(error);
    }

    private static JsonSerializerOptions MarketStackSerializerOptions()
    {
        var settings = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        settings.Converters.Add(new CustomDateTimeOffsetConverter());
        return settings;
    }
}
