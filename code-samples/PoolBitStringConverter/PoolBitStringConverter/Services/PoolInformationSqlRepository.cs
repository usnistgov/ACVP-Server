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
                        WHERE   poolName IN (
                            'aes-cfbbit-128-encrypt',
                            'aes-cfbbit-128-decrypt',
                            'aes-cfbbit-192-encrypt',
                            'aes-cfbbit-192-decrypt',
                            'aes-cfbbit-256-encrypt',
                            'aes-cfbbit-256-decrypt',
                            'aes-cbc-cs1-128-encrypt-len521',
                            'aes-cbc-cs2-128-encrypt-len521',
                            'aes-cbc-cs3-128-encrypt-len521',
                            'aes-cbc-cs1-128-decrypt-len521',
                            'aes-cbc-cs2-128-decrypt-len521',
                            'aes-cbc-cs3-128-decrypt-len521',
                            'aes-cbc-cs1-192-encrypt-len521',
                            'aes-cbc-cs2-192-encrypt-len521',
                            'aes-cbc-cs3-192-encrypt-len521',
                            'aes-cbc-cs1-192-decrypt-len521',
                            'aes-cbc-cs2-192-decrypt-len521',
                            'aes-cbc-cs3-192-decrypt-len521',
                            'aes-cbc-cs1-256-encrypt-len521',
                            'aes-cbc-cs2-256-encrypt-len521',
                            'aes-cbc-cs3-256-encrypt-len521',
                            'aes-cbc-cs1-256-decrypt-len521',
                            'aes-cbc-cs2-256-decrypt-len521',
                            'aes-cbc-cs3-256-decrypt-len521',
                            'aes-cbc-cs1-128-encrypt-len1337',
                            'aes-cbc-cs2-128-encrypt-len1337',
                            'aes-cbc-cs3-128-encrypt-len1337',
                            'aes-cbc-cs1-128-decrypt-len1337',
                            'aes-cbc-cs2-128-decrypt-len1337',
                            'aes-cbc-cs3-128-decrypt-len1337',
                            'aes-cbc-cs1-192-encrypt-len1337',
                            'aes-cbc-cs2-192-encrypt-len1337',
                            'aes-cbc-cs3-192-encrypt-len1337',
                            'aes-cbc-cs1-192-decrypt-len1337',
                            'aes-cbc-cs2-192-decrypt-len1337',
                            'aes-cbc-cs3-192-decrypt-len1337',
                            'aes-cbc-cs1-256-encrypt-len1337',
                            'aes-cbc-cs2-256-encrypt-len1337',
                            'aes-cbc-cs3-256-encrypt-len1337',
                            'aes-cbc-cs1-256-decrypt-len1337',
                            'aes-cbc-cs2-256-decrypt-len1337',
                            'aes-cbc-cs3-256-decrypt-len1337',
                            'tdes-cfbbit-encrypt-keyingoption-1',
                            'tdes-cfbbit-decrypt-keyingoption-1',
                            'tdes-cfbbit-encrypt-keyingoption-2',
                            'tdes-cfbbit-decrypt-keyingoption-2',
                            'tdes-cfbpbit-encrypt-keyingoption-1',
                            'tdes-cfbpbit-decrypt-keyingoption-1',
                            'tdes-cfbpbit-encrypt-keyingoption-2',
                            'tdes-cfbpbit-decrypt-keyingoption-2'
                        )
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