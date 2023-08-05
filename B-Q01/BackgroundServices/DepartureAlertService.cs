using B_Q01.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace B_Q01.BackgroundServices
{
    public class DepartureAlertService : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<DepartureAlertService> logger;

        public DepartureAlertService(
            IServiceScopeFactory scopeFactory,
            ILogger<DepartureAlertService> logger)
        {
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

            var nextDeparture = departuresService.FindNext().FirstOrDefault();
            if (nextDeparture == null)
            {
                Console.WriteLine("No arriving buses found");
                return;
            }
            TimeSpan arrival = nextDeparture.RealTime?.Subtract(DateTime.UtcNow) ?? nextDeparture.Time.Subtract(DateTime.UtcNow);

            switch (arrival.Minutes)
            {
                case < 5:
                    Console.WriteLine("Bus about to arrive");
                    break;
                case < 10:
                    Console.WriteLine("Bus arriving soon");
                    break;
                default:
                    Console.WriteLine("Some time until bus");
                    break;
            }
            var departureTime = TimeZoneInfo.ConvertTime(nextDeparture.RealTime ?? nextDeparture.Time,
                TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"));
            var supposedDep = TimeZoneInfo.ConvertTime(nextDeparture.Time,
                TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"));
            Console.WriteLine($"Line: {nextDeparture.Line} / Direction: {nextDeparture.Direction} / Arrival: {departureTime}, in {arrival.Minutes} minutes / Supposed Arrival: {supposedDep}");
        }
    }
}
