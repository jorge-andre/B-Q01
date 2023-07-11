// See https://aka.ms/new-console-template for more information
using B_Q01;
using B_Q01.BackgroundServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

var builder = new HostBuilder();

builder.ConfigureServices(services =>
    {
        services.AddHostedService<NextDeparturesService>();
        //services.AddHostedService<DepartureAlertService>();

        services.AddLogging();
    });
builder.ConfigureLogging(configLogging =>
    {
        configLogging.AddConsole();
        configLogging.AddDebug();
    });

await builder.RunConsoleAsync();

//List<Departure> tracking = new();

//foreach (var departure in departures)
//{
//    if (!tracking.Any(x => x.TimeUtc.Equals(departure.TimeUtc)))
//    {
//        tracking.Add(departure);
//    }
//    var trkDeparture = tracking.Find(x => x.TimeUtc.Equals(departure.TimeUtc))!;
//    if (!departure.Equals(trkDeparture))
//    {
//        tracking[tracking.FindIndex(d => d.Equals(trkDeparture))] = departure;
//    }
//}

//var nextArrival = tracking.First();
//if ((nextArrival.RealTimeUtc - DateTimeOffset.UtcNow).Minutes < 10)
//{
//    Console.WriteLine("Bus arriving soon!");
//    Console.WriteLine(nextArrival);
//}
//else
//{
//    Console.WriteLine("Ya got time");
//}