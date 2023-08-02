using LiteDB;

namespace B_Q01.Data.Context
{
    public interface ILiteDbContext
    {
        public LiteDatabase Database { get; }
    }
}
