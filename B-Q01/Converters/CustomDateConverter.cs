using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace B_Q01.Converters
{
    public class CustomDateConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                DateTimeOffset.ParseExact(reader.GetString()!,
                    "yyyy-MM-dd", new CultureInfo("en-DK"));

        public override void Write(
            Utf8JsonWriter writer,
            DateTimeOffset dateTimeValue,
            JsonSerializerOptions options) => writer.WriteStringValue(dateTimeValue);
    }
}
