namespace B_Q01.Services.Interfaces
{
    public interface ILiteDbDeparturesService
    {
        public IList<Departure> GetAll();
        public bool AddOrUpdateDeparture(Departure departure);
        public List<Departure> FindNext(int quantity = 1);
        public bool DeleteDeparture(int id);
        public int DeletePastDepartures();
        public TrackedStop AddTrackedStop(string stopName, int stopId)
    }
}
