using System.Text.Json.Serialization;

namespace MarketData.MarketStack.Model;

public record MarketStackExchange
{
    public required string Name { get; init; }
    public required string Acronym { get; init; }
    public required string Mic { get; init; }
    public required string Country { get; init; }
    [JsonPropertyName("country_code")]
    public required string CountryCode { get; init; }
    public required string City { get; init; }
}
