using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Interfaces;

namespace NIST.CVP.Pools.Services
{
    public class PoolMongoRepositoryFactory : IPoolRepositoryFactory
    {
        private readonly IMongoDbFactory _mongoDbFactory;
        private readonly IMongoPoolObjectFactory _mongoPoolObjectFactory;
        private readonly IBsonConverter _bsonConverter;
        private readonly IOptions<PoolConfig> _poolConfig;

        public PoolMongoRepositoryFactory(
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

        public IPoolRepository<TResult> GetRepository<TResult>() where TResult : IResult
        {
            return new PoolMongoRepository<TResult>(
                _mongoDbFactory, 
                _mongoPoolObjectFactory, 
                _bsonConverter,
                _poolConfig
            );
        }
    }
}