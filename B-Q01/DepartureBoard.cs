using B_Q01.Converters;
using System.Text.Json.Serialization;

namespace B_Q01
{
    public class DepartureBoard 
    {
        [property: JsonPropertyName("Departure")]
        public List<Departure>? Departures { get; set; }
    }

    public class Departure 
    {
        public int Id { get; set; }

        [property: JsonPropertyName("type")]
        public string Type { get; set; }

        [property: JsonPropertyName("line")]
        public string Line { get; set; }

        [property: JsonPropertyName("stop")]
        public string Stop { get; set; }

        [property: JsonPropertyName("direction")]
        public string Direction { get; set; }

        [property: JsonPropertyName("time")]
        [JsonConverter(typeof(CustomTimeConverter))]
        public DateTimeOffset Time { get; set; }

        [property: JsonPropertyName("rtTime")]
        [JsonConverter(typeof(CustomTimeConverter))]
        public DateTimeOffset? RealTime { get; set; }

        [property: JsonPropertyName("date")]
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTimeOffset Date { get; set; }

        [property: JsonPropertyName("realTimeTicks")]
        public long RealTimeTicks
        {
            get
            {
                return RealTime?.UtcTicks ?? Time.UtcTicks;
            }
        }

        [property: JsonPropertyName("scheduleOffset")]
        public TimeSpan ScheduleOffset
        {
            get
            {
                return RealTime?.Subtract(Time) ?? TimeSpan.Zero;
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !this.GetType().Equals(obj.GetType())) return false;
            else
            {
                Departure other = (Departure)obj;
                return Type.Equals(other.Type) &&
                    Line.Equals(other.Line) &&
                    Stop.Equals(other.Stop) &&
                    Direction.Equals(other.Direction) &&
                    Date.Equals(other.Date) &&
                    Time.Equals(other.Time);
            }
        }

        public override int GetHashCode()
        {
            var stringToHash = string.Join("-", Type, Line, Stop, Direction, Date, Time);
            return stringToHash.GetHashCode();
        }
    }
}
