using System;
using System.Data;
using Newtonsoft.Json;
using NIST.CVP.Common.Interfaces;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.ExtensionMethods;
using NIST.CVP.Pools.Interfaces;

namespace NIST.CVP.Pools.Services
{
    public class PoolLogSqlRepository : IPoolLogRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly string _connectionString;

        public PoolLogSqlRepository(
            IDbConnectionStringFactory connectionStringFactory,
            IDbConnectionFactory connectionFactory
        )
        {
            _connectionString = connectionStringFactory.GetConnectionString(Constants.AcvpPoolsConnString);
            _connectionFactory = connectionFactory;
        }

        public void WriteLog(LogTypes logType, string poolName, DateTime dateStart, DateTime? dateEnd, string msg)
        {
            using (var conn = _connectionFactory.Get(_connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "AddPoolLogEntry";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.AddParameter("logTypeName", logType.ToString());
                    cmd.AddParameter("poolName", poolName);
                    cmd.AddParameter("dateStart", dateStart);
                    cmd.AddParameter("dateEnd", dateEnd);
                    cmd.AddParameter("msg", msg);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}