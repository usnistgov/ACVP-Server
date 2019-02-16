using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Interfaces;

namespace NIST.CVP.Pools.Services
{
    public class PoolSqlRepositoryFactory : IPoolRepositoryFactory
    {
        private readonly IDbConnectionStringFactory _connectionStringFactory;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IList<JsonConverter> _jsonConverters;

        public PoolSqlRepositoryFactory(
            IDbConnectionStringFactory connectionStringFactory,
            IDbConnectionFactory connectionFactory,
            IJsonConverterProvider jsonConverterProvider
        )
        {
            _connectionStringFactory = connectionStringFactory;
            _connectionFactory = connectionFactory;
            _jsonConverters = jsonConverterProvider.GetJsonConverters();
        }

        public IPoolRepository<TResult> GetRepository<TResult>() where TResult : IResult
        {
            return new PoolSqlRepository<TResult>(
                _connectionStringFactory, 
                _connectionFactory,
                _jsonConverters
            );
        }
    }
}