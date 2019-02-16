using MongoDB.Driver;

namespace NIST.CVP.Pools.Interfaces
{
    public interface IMongoDbFactory
    {
        IMongoClient GetMongoDbClient(string connectionString);
    }
}