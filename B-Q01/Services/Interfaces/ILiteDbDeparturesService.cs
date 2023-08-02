namespace B_Q01.Services.Interfaces
{
    public interface ILiteDbDeparturesService
    {
        public IList<Departure> GetAll();
        public int AddOrUpdate(Departure departure);
    }
}
