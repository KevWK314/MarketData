using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MarketData.MarketStack.Model.Converters;

public class CustomDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    private static readonly Regex OffsetNoColon = new Regex(@"(?<prefix>.*)(?<tz>[\+\-]\d{4})$", RegexOptions.Compiled);

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException();

        var s = reader.GetString() ?? throw new JsonException();

        // If timezone looks like +0000 or -0130, insert a colon -> +00:00 or -01:30
        var m = OffsetNoColon.Match(s);
        if (m.Success)
        {
            var tz = m.Groups["tz"].Value; // e.g. +0000
            var fixedTz = tz[..3] + ":" + tz[3..]; // +00:00
            s = m.Groups["prefix"].Value + fixedTz;
        }

        if (DateTimeOffset.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dto))
            return dto.UtcDateTime;

        if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dt))
            return dt;

        throw new JsonException($"Unable to parse datetime '{s}'.");
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
    }
}