// See https://aka.ms/new-console-template for more information
using B_Q01.BackgroundServices;
using B_Q01.Data.Context;
using B_Q01.Services;
using B_Q01.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<ILiteDbContext, LiteDbContext>();
builder.Services.AddTransient<ILiteDbDeparturesService, LiteDbDeparturesService>();
builder.Services.AddHostedService<NextDeparturesService>();
builder.Services.AddHostedService<DepartureAlertService>();

builder.Services.AddLogging();

var host = builder.Build();

host.Run();

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