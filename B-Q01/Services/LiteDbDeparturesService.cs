using B_Q01.Data.Context;
using B_Q01.Services.Interfaces;
using LiteDB;

namespace B_Q01.Services
{
    public class LiteDbDeparturesService : ILiteDbDeparturesService
    {
        private LiteDatabase db;

        public LiteDbDeparturesService(ILiteDbContext context)
        {
            db = context.Database;
        }

        public IList<Departure> GetAll()
        {
            return db.GetCollection<Departure>("Departure").FindAll().ToList();
        }

        public int AddOrUpdate(Departure departure)
        {
            return db.GetCollection<Departure>("Departure").Insert(departure);
        }
    }
}
