using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Interfaces;

namespace NIST.CVP.Pools.Services
{
    public class PoolRepositoryFactory : IPoolRepositoryFactory
    {
        private readonly IMongoDbFactory _mongoDbFactory;
        private readonly IOptions<PoolConfig> _poolConfig;

        public PoolRepositoryFactory(IMongoDbFactory mongoDbFactory, IOptions<PoolConfig> poolConfig)
        {
            _mongoDbFactory = mongoDbFactory;
            _poolConfig = poolConfig;
        }

        public IPoolRepository<TResult> GetRepository<TResult>() where TResult : IResult
        {
            return new PoolRepository<TResult>(_mongoDbFactory, _poolConfig);
        }
    }
}