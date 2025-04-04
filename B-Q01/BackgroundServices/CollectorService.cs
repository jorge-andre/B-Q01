using B_Q01.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace B_Q01.BackgroundServices
{

    public class CollectorService : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<CollectorService> logger;

        public CollectorService(
            IServiceScopeFactory scopeFactory,
            ILogger<CollectorService> logger)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await DoWork();

            using PeriodicTimer timer = new(TimeSpan.FromSeconds(60));
            try
            {
                while (await timer.WaitForNextTickAsync(cancellationToken))
                {
                    await DoWork();
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("NextDepartures hosted service is stopping.");
            }
        }

        private async Task DoWork()
        {
            using var scope = scopeFactory.CreateScope();
            var departuresService = scope.ServiceProvider.GetRequiredService<ILiteDbDeparturesService>();

            Console.WriteLine("Departures in db: " + departuresService.GetAll().Count);

            using HttpClient client = CreateClient();

            var currentDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"));
            var dateString = string.Join("-", currentDate.ToString("yyyy"), currentDate.ToString("MM"), currentDate.ToString("dd"));

            var stopId = 28002; //Get ID from stops table

            var res = await client.GetStringAsync($"departureBoard?id={stopId}&date={dateString}");
            if (res == null || res.Equals(string.Empty))
            {
                logger.LogWarning("API response null");
                return;
            }
            var board = JsonNode.Parse(res)?["Departure"]?.ToString();

            List<Departure> nextDepartures = new List<Departure>();
            if (board != null)
                nextDepartures = JsonSerializer.Deserialize<List<Departure>>(board)!;

            foreach (var departure in nextDepartures)
            {
                departuresService.AddOrUpdateDeparture(departure);
            }

            var delCount = departuresService.DeletePastDepartures();
            logger.LogInformation("Removed {count} past departures", delCount);
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "5067f9fa-acd4-4161-954f-e49db5fa5620");
            client.BaseAddress = new Uri("http://rejseplanen.dk/api/");

            return client;
        }
    }
}
