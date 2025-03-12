using B_Q01.Data.Context;
using B_Q01.Services.Interfaces;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace B_Q01.Services
{
    public class LiteDbDeparturesService : ILiteDbDeparturesService
    {
        private LiteDatabase db;
        private readonly ILogger<LiteDbDeparturesService> logger;

        public LiteDbDeparturesService(
            ILiteDbContext context,
            ILogger<LiteDbDeparturesService> logger)
        {
            db = context.Database;
            this.logger = logger;
        }

        public IList<Departure> GetAll()
        {
            return db.GetCollection<Departure>("Departure").FindAll().ToList();
        }

        public bool AddOrUpdateDeparture(Departure departure)
        {
            var col = db.GetCollection<Departure>("Departure");
            var dep = col.FindOne(x => x.Type == departure.Type
                && x.Line == departure.Line
                && x.Direction == departure.Direction
                && x.Date == departure.Date
                && x.Time == departure.Time);

            if (dep != null)
            {
                logger.LogInformation("Found departure, ID: {id}. Updating realtime from {old} to {new}", dep.Id, dep.RealTime.GetValueOrDefault(), departure.RealTime.GetValueOrDefault());
                departure.Id = dep.Id;
                return col.Update(departure);
            }
            logger.LogInformation("Adding new departure");
            return col.Insert(departure).IsNumber;
        }

        public List<Departure> FindNext(int qnt = 1)
        {
            var items = db.GetCollection<Departure>("Departure").Query();
            var ticksNow = DateTime.UtcNow.Ticks;

            return items.Where(x => x.RealTimeTicks > ticksNow).OrderBy(x => x.RealTimeTicks).Offset(qnt).ToList();
        }

        public bool DeleteDeparture(int id)
        {
            logger.LogInformation("Deleting departure ID: {id}", id);
            return db.GetCollection<Departure>("Departure").Delete(id);
        }

        public int DeletePastDepartures()
        {
            var ticksNow = DateTime.UtcNow.Ticks;
            return db.GetCollection<Departure>("Departure").DeleteMany(x => x.RealTimeTicks <= ticksNow);
        }

        public TrackedStop AddTrackedStop(string stopName, int stopId)
        {
            var col = db.GetCollection<TrackedStop>("TrackedStop");
            var stop = col.FindOne(x => x.StopName == stopName
            && x.StopId == stopId);

            if (stop != null)
            {
                logger.LogInformation("Stop already tracked, ID: {id}", stop.StopId);
                return stop;
            }
            var newStop = new TrackedStop { StopName = stopName, StopId = stopId };
            col.Insert(newStop);
            logger.LogInformation("Added new tracked stop, ID: {id}", newStop.StopId);
            return newStop;
        }
    }
}
