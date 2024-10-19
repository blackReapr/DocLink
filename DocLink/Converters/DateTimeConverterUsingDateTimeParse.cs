using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DocLink.Presentation.Converters;

public class DateTimeConverterUsingDateTimeParse : JsonConverter<DateTime>
{
    // Updated format to include hours and minutes (24-hour format)
    private readonly string _format = "dd-MM-yyyy HH:mm"; // The date format with hour and minute

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? dateString = reader.GetString();
        if (string.IsNullOrEmpty(dateString))
        {
            throw new JsonException("Invalid date string");
        }

        return DateTime.ParseExact(dateString, _format, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_format));
    }
}
