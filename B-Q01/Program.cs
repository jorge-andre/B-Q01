// See https://aka.ms/new-console-template for more information
using B_Q01;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

var res = await client.GetStringAsync("http://xmlopen.rejseplanen.dk/bin/rest.exe/departureBoard?id=2753&date=10.07.23&offsetTime=1&format=json");
var board = JsonNode.Parse(res)?["DepartureBoard"]?["Departure"]?.ToString();

IList<Departure> departures = new List<Departure>();
if (board != null)
    departures = JsonSerializer.Deserialize<List<Departure>>(board)!;

List<Departure> tracking = new List<Departure>();

foreach (var departure in departures)
{
    if (!tracking.Any(x => x.TimeUtc.Equals(departure.TimeUtc)))
    {
        tracking.Add(departure);
    }
    var trkDeparture = tracking.Find(x => x.TimeUtc.Equals(departure.TimeUtc))!;
    if (!departure.Equals(trkDeparture))
    {
        tracking[tracking.FindIndex(d => d.Equals(trkDeparture))] = departure;
    }
}

