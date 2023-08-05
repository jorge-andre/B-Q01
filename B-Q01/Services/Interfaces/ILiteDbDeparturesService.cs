namespace B_Q01.Services.Interfaces
{
    public interface ILiteDbDeparturesService
    {
        public IList<Departure> GetAll();
        public bool AddOrUpdate(Departure departure);
        public List<Departure> FindNext(int quantity = 1);
        public bool Delete(int id);
        public int DeletePastDepartures();
    }
}
