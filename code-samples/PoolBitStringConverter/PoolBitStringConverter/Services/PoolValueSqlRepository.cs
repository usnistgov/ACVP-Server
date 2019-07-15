using NIST.CVP.Common.Interfaces;
using NIST.CVP.Pools;
using NIST.CVP.Pools.ExtensionMethods;
using PoolBitStringConverter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace PoolBitStringConverter.Services
{
    internal class PoolValueSqlRepository
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

        public PoolValues GetPoolValues(long poolId, int numberOfValuesToPull)
        {
            var poolValues = new PoolValues
            {
                Values = new List<PoolValue>()
            };
            var valuesRead = 0;

            // Get the pool values
            using (var conn = _connectionFactory.Get(_connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"
                        SELECT  TOP {numberOfValuesToPull} id, value
                        FROM    [dbo].[PoolValues]
                        WHERE   hasNewlySerializedBitStrings = 0
                            AND poolId = @poolId
                    ";
                    cmd.CommandType = CommandType.Text;
                    cmd.AddParameter(nameof(poolId), poolId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            poolValues.Values.Add(new PoolValue()
                            {
                                Id = reader.GetInt64(reader.GetOrdinal(nameof(PoolValue.Id))),
                                Value = reader.GetString(reader.GetOrdinal(nameof(PoolValue.Value)))
                            });

                            valuesRead++;
                        }
                    }
                }

                // Determine if there are more pool values to get
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT  COUNT_BIG(1) as remainingToSerialize
                        FROM    [dbo].[PoolValues]
                        WHERE   hasNewlySerializedBitStrings = 0
                            AND poolId = @poolId
                    ";
                    cmd.CommandType = CommandType.Text;
                    cmd.AddParameter(nameof(poolId), poolId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        // Should only be a single row
                        while (reader.Read())
                        {
                            poolValues.HasAdditionalValues =
                                reader.GetInt64(reader.GetOrdinal("remainingToSerialize")) - valuesRead > 0;
                        }
                    }
                }
            }

            return poolValues;
        }

        public void SavePoolValues(List<PoolValue> list)
        {
            DataTable dt = new DataTable("MyTable");
            dt = ConvertToDataTable(list);

            using (var conn = _connectionFactory.Get(_connectionString))
            {
                conn.Open();

                // Create a temp table to hold the PoolValue Id and new value
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE #TmpTable(id BIGINT, value VARCHAR(MAX))";
                    cmd.ExecuteNonQuery();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)conn))
                    {
                        bulkCopy.BulkCopyTimeout = 1000;
                        bulkCopy.DestinationTableName = "#TmpTable";
                        bulkCopy.WriteToServer(dt);
                        bulkCopy.Close();
                    }

                    cmd.CommandTimeout = 1000;
                    cmd.CommandText = @"
                        UPDATE T 
                        SET hasNewlySerializedBitStrings = 1,
                            value = Temp.value
                        FROM [dbo].[PoolValues] T 
                        INNER JOIN #TmpTable Temp ON T.id = Temp.id; 

                        DROP TABLE #TmpTable;";

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }
}