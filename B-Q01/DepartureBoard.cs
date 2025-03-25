using B_Q01.Converters;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace B_Q01
{
    public class Product
    {
        [property: JsonPropertyName("name")]
        public string Name { get; set; }

        [property: JsonPropertyName("displayNumber")]
        public string DisplayNumber { get; set; }

        [property: JsonPropertyName("line")]
        public string Line { get; set; }

        [property: JsonPropertyName("catOut")]
        public string Type { get; set; }
    }

    public class Departure 
    {
        public int Id { get; set; }

        [property: JsonPropertyName("ProductAtStop")]
        public Product Product { get; set; }

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
                return Product.Type.Equals(other.Product.Type) &&
                    Product.Line.Equals(other.Product.Line) &&
                    Stop.Equals(other.Stop) &&
                    Direction.Equals(other.Direction) &&
                    Date.Equals(other.Date) &&
                    Time.Equals(other.Time);
            }
        }

        public override int GetHashCode()
        {
            var stringToHash = string.Join("-", Product.Type, Product.Line, Stop, Direction, Date, Time);
            return stringToHash.GetHashCode();
        }
    }
}
