using NIST.CVP.Common.Interfaces;
using NIST.CVP.Pools;
using PoolBitStringConverter.Models;
using System.Collections.Generic;
using System.Data;

namespace PoolBitStringConverter.Services
{
    internal class PoolInformationSqlRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly string _connectionString;

        public PoolInformationSqlRepository(
            IDbConnectionStringFactory connectionStringFactory,
            IDbConnectionFactory connectionFactory
        )
        {
            _connectionString = connectionStringFactory.GetConnectionString(Constants.AcvpPoolsConnString);
            _connectionFactory = connectionFactory;
        }

        public List<PoolType> GetPoolTypes()
        {
            List<PoolType> list = new List<PoolType>();

            using (var conn = _connectionFactory.Get(_connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT  id, poolName
                        FROM    [dbo].[PoolInformation]
                    ";
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new PoolType()
                            {
                                Id = reader.GetInt64(reader.GetOrdinal(nameof(PoolType.Id))),
                                PoolName = reader.GetString(reader.GetOrdinal(nameof(PoolType.PoolName)))
                            });
                        }
                    }

                    return list;
                }
            }
        }
    }
}