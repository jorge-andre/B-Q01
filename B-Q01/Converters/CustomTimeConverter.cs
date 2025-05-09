﻿using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace B_Q01.Converters
{
    public class CustomTimeConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                DateTimeOffset.ParseExact(reader.GetString()!,
                    "HH:mm:ss", new CultureInfo("en-DK"));

        public override void Write(
            Utf8JsonWriter writer,
            DateTimeOffset dateTimeValue,
            JsonSerializerOptions options) => writer.WriteStringValue(dateTimeValue);
    }
}
