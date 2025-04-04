// See https://aka.ms/new-console-template for more information
using B_Q01.BackgroundServices;
using B_Q01.Data.Context;
using B_Q01.Kafka;
using B_Q01.Services;
using B_Q01.Services.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<KafkaClientHandle>();
builder.Services.AddSingleton<KafkaDependentProducer>();
builder.Services.AddSingleton<ILiteDbContext, LiteDbContext>();
builder.Services.AddTransient<ILiteDbDeparturesService, LiteDbDeparturesService>();
builder.Services.AddHostedService<CollectorService>();
builder.Services.AddHostedService<DepartureAlertService>();

builder.Services.AddLogging();
builder.Logging.AddConsole();

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("./appsettings.json")
    .Build();

var host = builder.Build();

host.Run();