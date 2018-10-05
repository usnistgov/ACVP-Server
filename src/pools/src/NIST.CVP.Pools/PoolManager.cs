using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.PoolModels;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;

namespace NIST.CVP.Pools
{
    public class PoolManager
    {
        public readonly List<IPool> Pools = new List<IPool>();

        private readonly IOptions<PoolConfig> _poolConfig;
        private PoolProperties[] _properties;
        private string _poolDirectory;
        
        private readonly IList<JsonConverter> _jsonConverters = new List<JsonConverter>
        {
            new BitstringConverter(),
            new DomainConverter(),
            new BigIntegerConverter(),
            new StringEnumConverter()
        };

        public PoolManager(IOptions<PoolConfig> poolConfig, string configFile, string poolDirectory)
        {
            _poolConfig = poolConfig;
            LoadPools(configFile, poolDirectory);
        }

        public PoolInformation GetPoolStatus(ParameterHolder paramHolder)
        {
            if (Pools.TryFirst(pool => pool.Param.Equals(paramHolder.Parameters), out var result))
            {
                return new PoolInformation {FillLevel = result.WaterLevel};
            }

            return new PoolInformation {PoolExists = false};
        }

        public bool AddResultToPool(ParameterHolder paramHolder)
        {
            if (Pools.TryFirst(pool => pool.Param.Equals(paramHolder.Parameters), out var result))
            {
                return result.AddWater(paramHolder.Result);
            }

            return false;
        }

        public PoolResult<IResult> GetResultFromPool(ParameterHolder paramHolder)
        {
            if (Pools.TryFirst(pool => pool.Param.Equals(paramHolder.Parameters), out var result))
            {
                return result.GetNextUntyped();
            }

            return new PoolResult<IResult> { PoolEmpty = true };
        }

        public List<ParameterHolder> GetPoolInformation()
        {
            var list = new List<ParameterHolder>();
            Pools.ForEach(fe =>
            {
                list.Add(new ParameterHolder
                {
                    Parameters = fe.Param,
                    Type = fe.DeclaredType
                });
            });

            return list;
        }

        public bool SavePools()
        {
            foreach (var pool in Pools)
            {
                if (_properties.TryFirst(prop => pool.Param.Equals(prop.PoolType.Parameters), out var properties))
                {
                    var filePath = Path.Combine(_poolDirectory, properties.FilePath);
                    if (!pool.SavePoolToFile(filePath))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public bool CleanPools()
        {
            foreach (var pool in Pools)
            {
                pool.CleanPool();
            }

            return true;
        }

        private void LoadPools(string configFile, string poolDirectory)
        {
            _poolDirectory = poolDirectory;

            var fullConfigFile = Path.Combine(_poolDirectory, configFile);
            _properties = JsonConvert.DeserializeObject<PoolProperties[]>
            (
                File.ReadAllText(fullConfigFile), 
                new JsonSerializerSettings
                {
                    Converters = _jsonConverters
                }
            );

            foreach (var poolProperty in _properties)
            {
                var filePath = Path.Combine(_poolDirectory, poolProperty.FilePath);
                var param = poolProperty.PoolType.Parameters;

                IPool pool = null;
                switch (poolProperty.PoolType.Type)
                {
                    case PoolTypes.SHA:
                        pool = new ShaPool(_poolConfig, param as ShaParameters, filePath, _jsonConverters);
                        break;

                    case PoolTypes.AES:
                        pool = new AesPool(_poolConfig, param as AesParameters, filePath, _jsonConverters);
                        break;

                    case PoolTypes.SHA_MCT:
                        pool = new ShaMctPool(_poolConfig, param as ShaParameters, filePath, _jsonConverters);
                        break;

                    case PoolTypes.AES_MCT:
                        pool = new AesMctPool(_poolConfig, param as AesParameters, filePath, _jsonConverters);
                        break;

                    case PoolTypes.TDES_MCT:
                        pool = new TdesMctPool(_poolConfig, param as TdesParameters, filePath, _jsonConverters);
                        break;

                    case PoolTypes.SHA3_MCT:
                        pool = new Sha3MctPool(_poolConfig, param as Sha3Parameters, filePath, _jsonConverters);
                        break;

                    case PoolTypes.CSHAKE_MCT:
                        pool = new CShakeMctPool(_poolConfig, param as CShakeParameters, filePath, _jsonConverters);
                        break;

                    case PoolTypes.PARALLEL_HASH_MCT:
                        pool = new ParallelHashMctPool(_poolConfig, param as ParallelHashParameters, filePath, _jsonConverters);
                        break;

                    case PoolTypes.TUPLE_HASH_MCT:
                        pool = new TupleHashMctPool(_poolConfig, param as TupleHashParameters, filePath, _jsonConverters);
                        break;

                    case PoolTypes.DSA_PQG:
                        pool = new DsaPqgPool(_poolConfig, param as DsaDomainParametersParameters, filePath, _jsonConverters);
                        break;

                    case PoolTypes.ECDSA_KEY:
                        pool = new EcdsaKeyPool(_poolConfig, param as EcdsaKeyParameters, filePath, _jsonConverters);
                        break;

                    case PoolTypes.RSA_KEY:
                        pool = new RsaKeyPool(_poolConfig, param as RsaKeyParameters, filePath, _jsonConverters);
                        break;

                    default:
                        throw new Exception("No pool model found");
                }

                Pools.Add(pool);
            }
        }
    }
}
