using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Interfaces;

namespace NIST.CVP.Pools.Services
{
    public class PoolRepository<TResult> : IPoolRepository<TResult>
        where TResult : IResult
    {
        private readonly IMongoDbFactory _mongoDbFactory;
        private readonly IOptions<PoolConfig> _poolConfig;

        public PoolRepository(IMongoDbFactory mongoDbFactory, IOptions<PoolConfig> poolConfig)
        {
            _mongoDbFactory = mongoDbFactory;
            _poolConfig = poolConfig;
        }

        public void AddResultToPool(string poolName, TResult value)
        {
            throw new NotImplementedException();
        }

        public TResult GetResultFromPool(string poolName)
        {
            throw new NotImplementedException();
        }

        public void MixStagingPoolIntoPool(string stagingPoolName, string poolName)
        {
            throw new NotImplementedException();
        }
    }
}
