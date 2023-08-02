using LiteDB;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_Q01.Data.Context
{
    public class LiteDbContext : ILiteDbContext
    {
        public LiteDatabase Database { get; }

        public LiteDbContext()
        {
            Database = new LiteDatabase(":memory:");
        }
    }
}
