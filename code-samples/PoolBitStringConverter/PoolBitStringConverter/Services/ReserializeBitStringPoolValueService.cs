using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NIST.CVP.Common.Interfaces;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Math;
using NIST.CVP.Math.JsonConverters;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.Models;
using PoolBitStringConverter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PoolBitStringConverter.Services
{
    internal class ReserializeBitStringPoolValueService
    {
        /// <summary>
        /// Number of pool value records to work with at a time.
        /// </summary>
        private const int ChunkSize = 1000;

        private readonly IList<JsonConverter> _originalJsonConverters = new List<JsonConverter>()
        {
            new BitstringConverter(),
            new DomainConverter(),
            new BigIntegerConverter(),
            new StringEnumConverter()
        };
        private readonly IList<JsonConverter> _newJsonConverters = new List<JsonConverter>()
        {
            new BitstringBitLengthConverter(),
            new DomainConverter(),
            new BigIntegerConverter(),
            new StringEnumConverter()
        };

        private readonly IDbConnectionStringFactory _dbConnectionStringFactory;
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly List<PoolProperties> _properties;

        private readonly PoolInformationSqlRepository _poolInfoRepo;
        private readonly PoolValueSqlRepository _poolValueRepo;


        public ReserializeBitStringPoolValueService(IServiceProvider serviceProvider, string poolConfigLocation)
        {
            _dbConnectionStringFactory = serviceProvider.GetService<IDbConnectionStringFactory>();
            _dbConnectionFactory = serviceProvider.GetService<IDbConnectionFactory>();

            _properties = JsonConvert.DeserializeObject<PoolProperties[]>
            (
                File.ReadAllText(poolConfigLocation),
                new JsonSerializerSettings
                {
                    Converters = _originalJsonConverters
                }
            ).ToList();

            _poolInfoRepo = new PoolInformationSqlRepository(_dbConnectionStringFactory, _dbConnectionFactory);
            _poolValueRepo = new PoolValueSqlRepository(_dbConnectionStringFactory, _dbConnectionFactory);
        }

        public void Execute()
        {
            try
            {
                var startTime = DateTime.Now;
                Console.WriteLine($"Starting conversion process at {startTime:yyyy-MM-dd HH:mm:ss}");

                var poolTypes = GetPoolTypes();
                UpdatePoolValuesPerPoolType(poolTypes);

                var endTime = DateTime.Now;

                Console.WriteLine(Environment.NewLine);
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine($"Conversion process completed at {endTime:yyyy-MM-dd HH:mm:ss}");
                var ts = endTime - startTime;
                Console.WriteLine($"Elapsed time of {ts:g}.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Get each pool type that exists and return as a <see cref="List{T}" /> of <see cref="PoolType"/>.
        /// </summary>
        /// <returns></returns>
        private List<PoolType> GetPoolTypes()
        {
            Console.WriteLine("Getting PoolTypes");
            var poolTypes = _poolInfoRepo.GetPoolTypes();
            Console.WriteLine($"Got {poolTypes.Count} PoolTypes");

            return poolTypes;
        }

        /// <summary>
        /// For every pool type, we want to update the values for that type.
        /// </summary>
        /// <param name="poolTypes">The poolTypes to enumerate.</param>
        private void UpdatePoolValuesPerPoolType(IEnumerable<PoolType> poolTypes)
        {
            foreach (var poolType in poolTypes)
            {
                var startTime = DateTime.Now;
                Console.WriteLine($"Starting on PoolType {poolType.PoolName} at {startTime:yyyy-MM-dd HH:mm:ss}");

                UpdatePoolValues(poolType);

                var endTime = DateTime.Now;
                var ts = endTime - startTime;
                Console.WriteLine($"PoolType {poolType.PoolName} conversion completed {endTime:yyyy-MM-dd HH:mm:ss}. Elapsed time of {ts:g}.");

            }
        }

        /// <summary>
        /// Update an individual poolType's values
        /// </summary>
        /// <param name="poolType"></param>
        private void UpdatePoolValues(PoolType poolType)
        {
            while (true)
            {
                var poolValues = GetValuesForPool(poolType);
                SaveValuesForPool(poolType, poolValues.Values);

                if (poolValues.HasAdditionalValues)
                {
                    continue;
                }

                break;
            }
        }

        private PoolValues GetValuesForPool(PoolType poolType)
        {
            return _poolValueRepo.GetPoolValues(poolType.Id, ChunkSize);
        }

        private void SaveValuesForPool(PoolType poolType, List<PoolValue> poolValues)
        {
            // Deserialize the values to the appropriate type
            var deserializedValues = GetPoolContents(poolType.PoolName, poolValues);

            // Serialize the values using the new converter, note specific pool types require a differing from default BitLength
            var serializedValues = ReserializeValuesUnderNewConverter(poolType.PoolName, deserializedValues);

            PersistNewlySerializedValues(serializedValues);
        }

        private Dictionary<PoolValue, IResult> GetPoolContents(string poolName, List<PoolValue> poolValues)
        {
            Dictionary<PoolValue, IResult> list = new Dictionary<PoolValue, IResult>();

            var poolConfig = _properties
                .FirstOrDefault(w => w.PoolName.Equals(poolName, StringComparison.OrdinalIgnoreCase));

            if (poolConfig == null)
            {
                return list;
            }

            var poolType = poolConfig.PoolType.Type;

            poolValues.ForEach(fe =>
            {
                list.Add(fe, DeserializeJsonIntoAppropriateType(poolName, poolType, fe.Value));
            });

            return list;
        }

        private IResult DeserializeJsonIntoAppropriateType(string poolName, PoolTypes poolType, string json)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
            {
                Converters = _originalJsonConverters,
                NullValueHandling = NullValueHandling.Ignore
            };

            switch (poolType)
            {
                case PoolTypes.SHA_MCT:
                    return JsonConvert.DeserializeObject<MctResult<HashResult>>(
                        json, jsonSerializerSettings
                    );

                case PoolTypes.AES_MCT:
                    var potentialAesSpecialCase = JsonConvert.DeserializeObject<MctResult<AesResult>>(
                        json, jsonSerializerSettings

                    );

                    var newBitLen = 0;

                    switch (poolName.ToLower())
                    {
                        case "aes-cfbbit-128-encrypt":
                        case "aes-cfbbit-128-decrypt":
                        case "aes-cfbbit-192-encrypt":
                        case "aes-cfbbit-192-decrypt":
                        case "aes-cfbbit-256-encrypt":
                        case "aes-cfbbit-256-decrypt":
                            newBitLen = 1;

                            potentialAesSpecialCase.Seed.PlainText = GetRelevantBitsFromBitString(potentialAesSpecialCase.Seed.PlainText, newBitLen);
                            potentialAesSpecialCase.Seed.CipherText = GetRelevantBitsFromBitString(potentialAesSpecialCase.Seed.CipherText, newBitLen);
                            potentialAesSpecialCase.Results.ForEach(fe =>
                                {
                                    fe.PlainText = GetRelevantBitsFromBitString(fe.PlainText, newBitLen);
                                    fe.CipherText = GetRelevantBitsFromBitString(fe.CipherText, newBitLen);
                                });
                            break;
                        case "aes-cbc-cs1-128-encrypt-len521":
                        case "aes-cbc-cs2-128-encrypt-len521":
                        case "aes-cbc-cs3-128-encrypt-len521":
                        case "aes-cbc-cs1-128-decrypt-len521":
                        case "aes-cbc-cs2-128-decrypt-len521":
                        case "aes-cbc-cs3-128-decrypt-len521":
                        case "aes-cbc-cs1-192-encrypt-len521":
                        case "aes-cbc-cs2-192-encrypt-len521":
                        case "aes-cbc-cs3-192-encrypt-len521":
                        case "aes-cbc-cs1-192-decrypt-len521":
                        case "aes-cbc-cs2-192-decrypt-len521":
                        case "aes-cbc-cs3-192-decrypt-len521":
                        case "aes-cbc-cs1-256-encrypt-len521":
                        case "aes-cbc-cs2-256-encrypt-len521":
                        case "aes-cbc-cs3-256-encrypt-len521":
                        case "aes-cbc-cs1-256-decrypt-len521":
                        case "aes-cbc-cs2-256-decrypt-len521":
                        case "aes-cbc-cs3-256-decrypt-len521":
                            newBitLen = 521;

                            potentialAesSpecialCase.Seed.PlainText = GetRelevantBitsFromBitString(potentialAesSpecialCase.Seed.PlainText, newBitLen);
                            potentialAesSpecialCase.Seed.CipherText = GetRelevantBitsFromBitString(potentialAesSpecialCase.Seed.CipherText, newBitLen);
                            potentialAesSpecialCase.Results.ForEach(fe =>
                            {
                                fe.PlainText = GetRelevantBitsFromBitString(fe.PlainText, newBitLen);
                                fe.CipherText = GetRelevantBitsFromBitString(fe.CipherText, newBitLen);
                            });
                            break;

                        case "aes-cbc-cs1-128-encrypt-len1337":
                        case "aes-cbc-cs2-128-encrypt-len1337":
                        case "aes-cbc-cs3-128-encrypt-len1337":
                        case "aes-cbc-cs1-128-decrypt-len1337":
                        case "aes-cbc-cs2-128-decrypt-len1337":
                        case "aes-cbc-cs3-128-decrypt-len1337":
                        case "aes-cbc-cs1-192-encrypt-len1337":
                        case "aes-cbc-cs2-192-encrypt-len1337":
                        case "aes-cbc-cs3-192-encrypt-len1337":
                        case "aes-cbc-cs1-192-decrypt-len1337":
                        case "aes-cbc-cs2-192-decrypt-len1337":
                        case "aes-cbc-cs3-192-decrypt-len1337":
                        case "aes-cbc-cs1-256-encrypt-len1337":
                        case "aes-cbc-cs2-256-encrypt-len1337":
                        case "aes-cbc-cs3-256-encrypt-len1337":
                        case "aes-cbc-cs1-256-decrypt-len1337":
                        case "aes-cbc-cs2-256-decrypt-len1337":
                        case "aes-cbc-cs3-256-decrypt-len1337":
                            newBitLen = 521;

                            potentialAesSpecialCase.Seed.PlainText = GetRelevantBitsFromBitString(potentialAesSpecialCase.Seed.PlainText, newBitLen);
                            potentialAesSpecialCase.Seed.CipherText = GetRelevantBitsFromBitString(potentialAesSpecialCase.Seed.CipherText, newBitLen);
                            potentialAesSpecialCase.Results.ForEach(fe =>
                            {
                                fe.PlainText = GetRelevantBitsFromBitString(fe.PlainText, newBitLen);
                                fe.CipherText = GetRelevantBitsFromBitString(fe.CipherText, newBitLen);
                            });
                            break;
                    }

                    return potentialAesSpecialCase;

                case PoolTypes.TDES_MCT:
                    var potentialTdesSpecialCase = JsonConvert.DeserializeObject<MctResult<TdesResult>>(
                        json, jsonSerializerSettings

                    );

                    switch (poolName.ToLower())
                    {
                        case "tdes-cfbbit-encrypt-keyingoption-1":
                        case "tdes-cfbbit-decrypt-keyingoption-1":
                        case "tdes-cfbbit-encrypt-keyingoption-2":
                        case "tdes-cfbbit-decrypt-keyingoption-2":
                        case "tdes-cfbpbit-encrypt-keyingoption-1":
                        case "tdes-cfbpbit-decrypt-keyingoption-1":
                        case "tdes-cfbpbit-encrypt-keyingoption-2":
                        case "tdes-cfbpbit-decrypt-keyingoption-2":
                            potentialTdesSpecialCase.Seed.PlainText = GetRelevantBitsFromBitString(potentialTdesSpecialCase.Seed.PlainText, 1);
                            potentialTdesSpecialCase.Seed.CipherText = GetRelevantBitsFromBitString(potentialTdesSpecialCase.Seed.CipherText, 1);
                            potentialTdesSpecialCase.Results.ForEach(fe =>
                            {
                                fe.PlainText = GetRelevantBitsFromBitString(fe.PlainText, 1);
                                fe.CipherText = GetRelevantBitsFromBitString(fe.CipherText, 1);
                            });
                            break;
                    }

                    return potentialTdesSpecialCase;

                case PoolTypes.SHA3_MCT:
                    return JsonConvert.DeserializeObject<MctResult<HashResult>>(
                        json, jsonSerializerSettings

                    );

                case PoolTypes.CSHAKE_MCT:
                    return JsonConvert.DeserializeObject<MctResult<CShakeResult>>(
                        json, jsonSerializerSettings

                    );

                case PoolTypes.PARALLEL_HASH_MCT:
                    return JsonConvert.DeserializeObject<MctResult<ParallelHashResult>>(
                        json, jsonSerializerSettings

                    );

                case PoolTypes.TUPLE_HASH_MCT:
                    return JsonConvert.DeserializeObject<MctResult<TupleHashResult>>(
                        json, jsonSerializerSettings

                    );

                case PoolTypes.DSA_PQG:
                    return JsonConvert.DeserializeObject<DsaDomainParametersResult>(
                        json, jsonSerializerSettings

                    );

                case PoolTypes.ECDSA_KEY:
                    return JsonConvert.DeserializeObject<EcdsaKeyResult>(
                        json, jsonSerializerSettings

                    );

                case PoolTypes.RSA_KEY:
                    return JsonConvert.DeserializeObject<RsaKeyResult>(
                        json, jsonSerializerSettings

                    );

                default:
                    return null;
            }
        }

        private BitString GetRelevantBitsFromBitString(BitString bitString, int numberOfBits)
        {
            if (bitString == null)
            {
                return null;
            }

            if (bitString.BitLength == 0)
            {
                return new BitString(0);
            }

            return bitString.GetMostSignificantBits(numberOfBits);
        }

        private List<PoolValue> ReserializeValuesUnderNewConverter(string poolName, Dictionary<PoolValue, IResult> deserializedValues)
        {
            List<PoolValue> list = new List<PoolValue>();

            foreach (var item in deserializedValues)
            {
                list.Add(new PoolValue()
                {
                    Id = item.Key.Id,
                    Value = JsonConvert.SerializeObject(
                        item.Value, new JsonSerializerSettings()
                        {
                            Converters = _newJsonConverters
                        }
                    )
                });
            }

            return list;
        }

        private void PersistNewlySerializedValues(List<PoolValue> serializedValues)
        {
            _poolValueRepo.SavePoolValues(serializedValues);
        }
    }

}
