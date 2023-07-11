using System.Globalization;
using System.Text.Json.Serialization;

namespace B_Q01
{
    public record class DepartureBoard(
        [property: JsonPropertyName("Departure")] List<Departure> Departures);

    public record class Departure(
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("line")] string Line,
        [property: JsonPropertyName("stop")] string Stop,
        [property: JsonPropertyName("direction")] string Direction,
        [property: JsonPropertyName("time")] string Time,
        [property: JsonPropertyName("rtTime")] string RealTime,
        [property: JsonPropertyName("date")] string Date,
        [property: JsonPropertyName("ref")] Uri DetailRef)
    {
        public DateTimeOffset TimeUtc
        {
            get
            {
                var parsedDate = DateTime.ParseExact(String.Join(" ", Date, Time), "dd.MM.yy HH:mm", new CultureInfo("en-DK"));
                return new DateTimeOffset(parsedDate);
            }
        }

        public DateTimeOffset RealTimeUtc
        {
            get
            {
                if (String.IsNullOrEmpty(RealTime))
                    return TimeUtc;
                var parsedDate = DateTime.ParseExact(String.Join(" ", Date, RealTime), "dd.MM.yy HH:mm", new CultureInfo("en-DK"));
                return new DateTimeOffset(parsedDate);
            }
            set
            {
                RealTimeUtc = value;
            }
        }

        public TimeSpan ScheduleOffset
        {
            get
            {
                return RealTimeUtc.Subtract(TimeUtc);
            }
        }

    }
}
