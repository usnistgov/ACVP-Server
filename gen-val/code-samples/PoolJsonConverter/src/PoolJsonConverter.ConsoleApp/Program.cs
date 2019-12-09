using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.ExtensionMethods;
using NIST.CVP.Pools.Models;
using NIST.CVP.Pools.PoolModels;
using NIST.CVP.Pools.Services;

namespace PoolJsonConverter.ConsoleApp
{
    class Program
    {
        private static readonly IList<JsonConverter> _jsonConverters = new JsonConverterProvider().GetJsonConverters();
        private static List<PoolProperties> _properties;

        private const string POOL_CONFIG_FILE_NAME = "poolConfig.json";

        static void Main(string[] args)
        {
            Console.WriteLine("This application is used to convert all the existing json file pool values into the database");
            Console.WriteLine(
                "Ensure that the current poolConfig.json is present, " + 
                "that the 'PoolName' property exists, " + 
                "and that the PoolNames do not contain the .json extension within their value."
            );
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Enter the folder where the current json files containing pool values can be found:");
            var folder = Console.ReadLine();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);

            if (!Directory.Exists(folder))
            {
                Console.WriteLine("Folder not found.");
                Console.ReadKey();

                return;
            }

            Console.WriteLine("Folder found, proceeding with parsing and insertion into database.");

            LoadPoolConfig(folder);

            using (var conn = new SqlDbConnectionFactory().Get(
                "Server=localhost\\SQLEXPRESS;Database=AcvpPools;Integrated Security=true;")
            )
            {
                conn.Open();
                var createdDate = DateTime.Now;

                foreach (var file in Directory.GetFiles(folder))
                {
                    // Check that the file is not the config file.
                    if (file.EndsWith(POOL_CONFIG_FILE_NAME, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var poolName = Path.GetFileNameWithoutExtension(file);
                    Console.WriteLine($"Processing file \"{file}\" for pool \"{poolName}\"");

                    var poolContents = GetPoolContents(file, poolName);

                    // Pool is empty
                    if (poolContents == null || poolContents.Length == 0)
                    {
                        continue;
                    }

                    // For every value in the pool, insert it into the database
                    foreach (var result in poolContents)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "AddValueToPool";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.AddParameter("poolName", poolName);
                            cmd.AddParameter(
                                "value", 
                                JsonConvert.SerializeObject(
                                    result, new JsonSerializerSettings()
                                    {
                                        Converters = _jsonConverters
                                    }
                                )
                            );
                            cmd.AddParameter("isStagingValue", false);
                            cmd.AddParameter("dateCreated", createdDate);
                            cmd.AddParameter("dateLastUsed", null);
                            cmd.AddParameter("timesValueUsed", 0);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private static void LoadPoolConfig(string folder)
        {
            var fullConfigFile = Path.Combine(folder, POOL_CONFIG_FILE_NAME);
            _properties = JsonConvert.DeserializeObject<PoolProperties[]>
            (
                File.ReadAllText(fullConfigFile), 
                new JsonSerializerSettings
                {
                    Converters = _jsonConverters
                }
            ).ToList();
        }

        private static IResult[] GetPoolContents(string fullFileWithPath, string poolName)
        {
            var poolConfig = _properties
                .FirstOrDefault(w => w.PoolName.Equals(poolName, StringComparison.OrdinalIgnoreCase));

            if (poolConfig == null)
            {
                return null;
            }

            var poolType = poolConfig.PoolType.Type;

            switch (poolType)
            {
                case PoolTypes.SHA_MCT:
                    return JsonConvert.DeserializeObject<MctResult<HashResult>[]>(
                        File.ReadAllText(fullFileWithPath),
                        new JsonSerializerSettings
                        {
                            Converters = _jsonConverters
                        }
                    );

                case PoolTypes.AES_MCT:
                    return JsonConvert.DeserializeObject<MctResult<AesResult>[]>(
                        File.ReadAllText(fullFileWithPath),
                        new JsonSerializerSettings
                        {
                            Converters = _jsonConverters
                        }
                    );

                case PoolTypes.TDES_MCT:
                    return JsonConvert.DeserializeObject<MctResult<TdesResult>[]>(
                        File.ReadAllText(fullFileWithPath),
                        new JsonSerializerSettings
                        {
                            Converters = _jsonConverters
                        }
                    );

                case PoolTypes.SHA3_MCT:
                    return JsonConvert.DeserializeObject<MctResult<HashResult>[]>(
                        File.ReadAllText(fullFileWithPath),
                        new JsonSerializerSettings
                        {
                            Converters = _jsonConverters
                        }
                    );

                case PoolTypes.CSHAKE_MCT:
                    return JsonConvert.DeserializeObject<MctResult<CShakeResult>[]>(
                        File.ReadAllText(fullFileWithPath),
                        new JsonSerializerSettings
                        {
                            Converters = _jsonConverters
                        }
                    );

                case PoolTypes.PARALLEL_HASH_MCT:
                    return JsonConvert.DeserializeObject<MctResult<ParallelHashResult>[]>(
                        File.ReadAllText(fullFileWithPath),
                        new JsonSerializerSettings
                        {
                            Converters = _jsonConverters
                        }
                    );

                case PoolTypes.TUPLE_HASH_MCT:
                    return JsonConvert.DeserializeObject<MctResult<TupleHashResult>[]>(
                        File.ReadAllText(fullFileWithPath),
                        new JsonSerializerSettings
                        {
                            Converters = _jsonConverters
                        }
                    );

                case PoolTypes.DSA_PQG:
                    return JsonConvert.DeserializeObject<DsaDomainParametersResult[]>(
                        File.ReadAllText(fullFileWithPath),
                        new JsonSerializerSettings
                        {
                            Converters = _jsonConverters
                        }
                    );

                case PoolTypes.ECDSA_KEY:
                    return JsonConvert.DeserializeObject<EcdsaKeyResult[]>(
                        File.ReadAllText(fullFileWithPath),
                        new JsonSerializerSettings
                        {
                            Converters = _jsonConverters
                        }
                    );

                case PoolTypes.RSA_KEY:
                    return JsonConvert.DeserializeObject<RsaKeyResult[]>(
                        File.ReadAllText(fullFileWithPath),
                        new JsonSerializerSettings
                        {
                            Converters = _jsonConverters
                        }
                    );

                default:
                    return null;
            }
        }
    }
}
