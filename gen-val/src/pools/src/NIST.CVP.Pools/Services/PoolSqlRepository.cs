using System;
using System.Collections.Generic;
using System.Data;
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

        public void AddResultToPool(string poolName, bool useStagingPool, PoolObject<TResult> value)
        {
            using (var conn = _connectionFactory.Get(_connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
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

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CleanPool(string poolName)
        {
            using (var conn = _connectionFactory.Get(_connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "ClearPool";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.AddParameter("poolName", poolName);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public long GetPoolCount(string poolName, bool useStagingPool)
        {
            using (var conn = _connectionFactory.Get(_connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "GetPoolCount";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.AddParameter("poolName", poolName);
                    cmd.AddParameter("isStagingValue", useStagingPool);

                    using (var reader = cmd.ExecuteReader())
                    {
                        // There should only be one record
                        while (reader.Read())
                        {
                            return reader.GetInt64(reader.GetOrdinal("poolCount"));
                        }
                    }

                    return 0;
                }
            }
        }

        public PoolObject<TResult> GetResultFromPool(string poolName)
        {
            using (var conn = _connectionFactory.Get(_connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "GetPoolValue";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.AddParameter("poolName", poolName);

                    using (var reader = cmd.ExecuteReader())
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
            }
        }

        public void MixStagingPoolIntoPool(string poolName)
        {
            using (var conn = _connectionFactory.Get(_connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "MixStagingValuesIntoPool";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.AddParameter("poolName", poolName);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        public Dictionary<string, long> GetAllPoolCounts()
        {
            var results = new Dictionary<string, long>();
            
            using (var conn = _connectionFactory.Get(_connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "GetPoolCounts";
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(
                                reader.GetString(reader.GetOrdinal("poolName")), 
                                reader.GetInt64(reader.GetOrdinal("poolCount")));
                        }
                    }
                }
            }

            return results;
        }
    }
}