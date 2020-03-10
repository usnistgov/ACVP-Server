using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Common.Interfaces;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.ExtensionMethods;
using NIST.CVP.Pools.Interfaces;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.Pools.Services
{
    public class PoolSqlRepository<TResult> : IPoolRepository<TResult>
        where TResult : IResult
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IList<JsonConverter> _jsonConverters;

        private readonly string _connectionString;

        public PoolSqlRepository(
            IDbConnectionStringFactory connectionStringFactory,
            IDbConnectionFactory connectionFactory,
            IList<JsonConverter> jsonConverters
        )
        {
            _connectionString = connectionStringFactory.GetConnectionString(Constants.AcvpPoolsConnString);
            _connectionFactory = connectionFactory;
            _jsonConverters = jsonConverters;
        }

        public async Task AddResultToPool(string poolName, bool useStagingPool, PoolObject<TResult> value)
        {
            await using var conn = _connectionFactory.Get(_connectionString);
            conn.Open();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "AddValueToPool";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.AddParameter("poolName", poolName);
            cmd.AddParameter(
                "value", 
                JsonConvert.SerializeObject(
                    value.Value, new JsonSerializerSettings()
                    {
                        Converters = _jsonConverters
                    }
                )
            );
            cmd.AddParameter("isStagingValue", useStagingPool);
            cmd.AddParameter("dateCreated", value.DateCreated);
            cmd.AddParameter("dateLastUsed", value.DateLastUsed);
            cmd.AddParameter("timesValueUsed", value.TimesUsed);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task CleanPool(string poolName)
        {
            await using var conn = _connectionFactory.Get(_connectionString);
            conn.Open();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "ClearPool";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.AddParameter("poolName", poolName);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<long> GetPoolCount(string poolName, bool useStagingPool)
        {
            await using var conn = _connectionFactory.Get(_connectionString);
            conn.Open();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "GetPoolCount";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.AddParameter("poolName", poolName);
            cmd.AddParameter("isStagingValue", useStagingPool);

            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                // There should only be one record
                while (reader.Read())
                {
                    return reader.GetInt64(reader.GetOrdinal("poolCount"));
                }
            }

            return 0;
        }

        public async Task<PoolObject<TResult>> GetResultFromPool(string poolName)
        {
            await using var conn = _connectionFactory.Get(_connectionString);
            conn.Open();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "GetPoolValue";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.AddParameter("poolName", poolName);

            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                // There should be a max of one record.
                while (reader.Read())
                {
                    return new PoolObject<TResult>()
                    {
                        DateCreated = reader.GetDateTime(reader.GetOrdinal("dateCreated")),
                        DateLastUsed = reader.GetNullableDateTime("dateLastUsed"),
                        TimesUsed = reader.GetInt64(reader.GetOrdinal("timesUsed")),
                        Value = JsonConvert.DeserializeObject<TResult>(
                            reader.GetString(reader.GetOrdinal("value")),
                            new JsonSerializerSettings()
                            {
                                Converters = _jsonConverters
                            }
                        )
                    };
                }
            }

            return null;
        }

        public async Task MixStagingPoolIntoPool(string poolName)
        {
            await using var conn = _connectionFactory.Get(_connectionString);
            conn.Open();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "MixStagingValuesIntoPool";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.AddParameter("poolName", poolName);

            await cmd.ExecuteNonQueryAsync();
        }
        
        public async Task<Dictionary<string, long>> GetAllPoolCounts()
        {
            var results = new Dictionary<string, long>();

            await using var conn = _connectionFactory.Get(_connectionString);
            conn.Open();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "GetPoolCounts";
            cmd.CommandType = CommandType.StoredProcedure;

            await using var reader = await cmd.ExecuteReaderAsync();
            while (reader.Read())
            {
                results.Add(
                    reader.GetString(reader.GetOrdinal("poolName")), 
                    reader.GetInt64(reader.GetOrdinal("poolCount")));
            }

            return results;
        }
    }
}