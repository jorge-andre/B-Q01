using B_Q01.Kafka;
using B_Q01.Services.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace B_Q01.BackgroundServices
{
    public class DepartureAlertService : BackgroundService
    {
        private readonly KafkaDependentProducer producer;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<DepartureAlertService> logger;
        private Departure? lastSentDeparture = null;

        public DepartureAlertService(
            KafkaDependentProducer producer,
            IServiceScopeFactory scopeFactory,
            ILogger<DepartureAlertService> logger)
        {
            this.producer = producer;
            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await DoWork();

            using PeriodicTimer timer = new(TimeSpan.FromSeconds(10));
            try
            {
                while (await timer.WaitForNextTickAsync(cancellationToken))
                {
                    await DoWork();
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("DepartureAlert hosted service is stopping.");
            }
        }

        private async Task DoWork()
        {
            using var scope = scopeFactory.CreateScope();
            var departuresService = scope.ServiceProvider.GetRequiredService<ILiteDbDeparturesService>();

            var stops = departuresService.GetAllTrackedStops();

            foreach (var stop in stops)
            {
                var nextDeparture = departuresService.FindNext(stop.StopId).FirstOrDefault();
                if (nextDeparture == null)
                {
                    Console.WriteLine("No departures found");
                    logger.LogInformation("No departures found");
                    return;
                }
                if (nextDeparture.Equals(lastSentDeparture))
                {
                    Console.WriteLine("No changes on next departure");
                    return;
                }
                var message = new Message<string, string>
                {
                    Key = nextDeparture.Stop,
                    Value = JsonSerializer.Serialize(nextDeparture)
                };

                var topic = stop.KafkaTopic;

                var result = await producer.ProduceAsync(topic, message);

                lastSentDeparture = nextDeparture;
                Console.WriteLine(result.Value);
            }
        }
    }
}
