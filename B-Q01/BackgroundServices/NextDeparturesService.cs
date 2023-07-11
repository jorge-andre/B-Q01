using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace B_Q01.BackgroundServices
{
    public class NextDeparturesService : BackgroundService
    {
        private readonly ILogger<NextDeparturesService> logger;

        public NextDeparturesService(ILogger<NextDeparturesService> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await DoWork();

            using PeriodicTimer timer = new(TimeSpan.FromMinutes(1));
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
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var currentDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, 
                TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"));
            var dateString = string.Join(".", currentDate.ToString("dd"), currentDate.ToString("MM"), currentDate.ToString("yy"));

            var res = await client.GetStringAsync("http://xmlopen.rejseplanen.dk/bin/rest.exe/departureBoard?id=2753&date="+dateString+"&offsetTime=0&format=json");
            var board = JsonNode.Parse(res)?["DepartureBoard"]?["Departure"]?.ToString();

            IList<Departure> departures = new List<Departure>();
            if (board != null)
                departures = JsonSerializer.Deserialize<List<Departure>>(board)!;

            Console.WriteLine("Next bus: " + departures.First().ToString());
        }
    }
}
