using MongoDB.Driver;
using NIST.CVP.Pools.Interfaces;

namespace NIST.CVP.Pools.Services
{
    public class MongoDbFactory : IMongoDbFactory
    {
        public IMongoClient GetMongoDbClient(string connectionString)
        {
            return new MongoClient(connectionString);
        }
    }
}
