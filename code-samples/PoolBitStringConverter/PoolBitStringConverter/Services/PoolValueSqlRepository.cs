using NIST.CVP.Common.Interfaces;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools;

namespace PoolBitStringConverter.Services
{
    internal class PoolValueSqlRepository<TResult>
        where TResult : IResult
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly string _connectionString;

        public PoolValueSqlRepository(
            IDbConnectionStringFactory connectionStringFactory,
            IDbConnectionFactory connectionFactory
        )
        {
            _connectionString = connectionStringFactory.GetConnectionString(Constants.AcvpPoolsConnString);
            _connectionFactory = connectionFactory;
        }
    }
}