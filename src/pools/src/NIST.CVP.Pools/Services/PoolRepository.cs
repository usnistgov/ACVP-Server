using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Interfaces;
using NIST.CVP.Pools.Models;
using NIST.CVP.Common.ExtensionMethods;

namespace NIST.CVP.Pools.Services
{
    public class PoolRepository<TResult> : IPoolRepository<TResult>
        where TResult : IResult
    {
        private readonly IMongoDbFactory _mongoDbFactory;
        private readonly IMongoPoolObjectFactory _mongoPoolObjectFactory;
        private readonly IBsonConverter _bsonConverter;
        private readonly IOptions<PoolConfig> _poolConfig;

        public PoolRepository(
            IMongoDbFactory mongoDbFactory, 
            IMongoPoolObjectFactory mongoPoolObjectFactory, 
            IBsonConverter bsonConverter,
            IOptions<PoolConfig> poolConfig
        )
        {
            _mongoDbFactory = mongoDbFactory;
            _mongoPoolObjectFactory = mongoPoolObjectFactory;
            _bsonConverter = bsonConverter;
            _poolConfig = poolConfig;
        }

        public void AddResultToPool(string poolName, TResult value)
        {
            var collection = GetDbConnection().GetCollection<MongoPoolObject<string>>(poolName);
            
            var bson = _mongoPoolObjectFactory.WrapResult(_bsonConverter.ToBson(value));
            collection.InsertOne(bson);
            
            //var collection = GetDbConnection().GetCollection<MongoPoolObject<TResult>>(poolName);
            //collection.InsertOne(_mongoPoolObjectFactory.WrapResult(value));
        }

        public void CleanPool(string poolName)
        {
            var collection = GetDbConnection().GetCollection<MongoPoolObject<string>>(poolName);
            collection.DeleteMany(new FilterDefinitionBuilder<MongoPoolObject<string>>().Empty);

            //var collection = GetDbConnection().GetCollection<MongoPoolObject<TResult>>(poolName);
            //collection.DeleteMany(new FilterDefinitionBuilder<MongoPoolObject<TResult>>().Empty);
        }

        public long GetPoolCount(string poolName)
        {
            // TODO REMOVE THIS - for debugging
            MixStagingPoolIntoPool($"staging-{poolName}", poolName);

            var collection = GetDbConnection().GetCollection<MongoPoolObject<string>>(poolName);
            var count = collection.CountDocuments(new FilterDefinitionBuilder<MongoPoolObject<string>>().Empty);
            return count;

            //var collection = GetDbConnection().GetCollection<MongoPoolObject<TResult>>(poolName);
            //return collection.CountDocuments(new FilterDefinitionBuilder<MongoPoolObject<TResult>>().Empty);
        }

        public TResult GetResultFromPool(string poolName)
        {
            var collection = GetDbConnection().GetCollection<MongoPoolObject<string>>(poolName);
            var dbResult = collection.FindOneAndDelete(new FilterDefinitionBuilder<MongoPoolObject<string>>().Empty);

            if (dbResult == null)
            {
                return default;
            }

            if (string.IsNullOrEmpty(dbResult.Value))
            {
                return default;
            }
            
            var result = _bsonConverter.FromBson<TResult>(dbResult.Value);
            return result;

            //var collection = GetDbConnection().GetCollection<MongoPoolObject<TResult>>(poolName);
            //var result = collection.FindOneAndDelete(new FilterDefinitionBuilder<MongoPoolObject<TResult>>().Empty);

            //// TODO REMOVE THIS - for debugging
            //MixStagingPoolIntoPool($"staging-{poolName}", poolName);

            //if (result == null)
            //{
            //    return default;
            //}

            //if (result.Value == null)
            //{
            //    return default;
            //}

            //return result.Value;
        }

        public void MixStagingPoolIntoPool(string stagingPoolName, string poolName)
        {
            var dbConnection = GetDbConnection();

            var stagingCollection = dbConnection.GetCollection<MongoPoolObject<string>>(stagingPoolName);
            var stagingItems = stagingCollection
                .Find(new FilterDefinitionBuilder<MongoPoolObject<string>>().Empty)
                .ToList()
                .Shuffle();
            stagingCollection.DeleteMany(new FilterDefinitionBuilder<MongoPoolObject<string>>().Empty);

            if (stagingItems.Count > 0)
            {
                var collection = dbConnection.GetCollection<MongoPoolObject<string>>(poolName);
                collection.InsertMany(stagingItems);
            }

            //var stagingCollection = dbConnection.GetCollection<MongoPoolObject<TResult>>(stagingPoolName);
            //var stagingItems = stagingCollection
            //    .Find(new FilterDefinitionBuilder<MongoPoolObject<TResult>>().Empty)
            //    .ToList()
            //    .Shuffle();
            //stagingCollection.DeleteMany(new FilterDefinitionBuilder<MongoPoolObject<TResult>>().Empty);

            //if (stagingItems.Count > 0)
            //{
            //    var collection = dbConnection.GetCollection<MongoPoolObject<TResult>>(poolName);
            //    collection.InsertMany(stagingItems);
            //}
        }

        private IMongoDatabase GetDbConnection()
        {
            return _mongoDbFactory
                .GetMongoDbClient(_poolConfig.Value.ConnectionString)
                .GetDatabase(_poolConfig.Value.DatabaseName);
        }
    }
}
