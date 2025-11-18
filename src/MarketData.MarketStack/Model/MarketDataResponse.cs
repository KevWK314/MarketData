namespace MarketData.MarketStack.Model;

public record MarketDataResponse<TData>
{
    public MarketDataResponsePagination? Pagination { get; init; }
    public TData? Data { get; init; }
}
