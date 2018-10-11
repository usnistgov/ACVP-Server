using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.Models;
using NIST.CVP.Pools.PoolModels;
using System;
using System.Collections.Generic;
using System.IO;

namespace NIST.CVP.Pools
{
    public class PoolManager
    {
        public readonly List<IPool> Pools = new List<IPool>();
        private readonly IOptions<PoolConfig> _poolConfig;
        private readonly string _poolDirectory;
        
        private PoolProperties[] _properties;
        
        private readonly IList<JsonConverter> _jsonConverters = new List<JsonConverter>
        {
            new BitstringConverter(),
            new DomainConverter(),
            new BigIntegerConverter(),
            new StringEnumConverter()
        };

        public PoolManager(IOptions<PoolConfig> poolConfig, string configFile, string poolDirectory)
        {
            _poolDirectory = poolDirectory;
            _poolConfig = poolConfig;
            LoadPools(configFile);
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

        public bool EditPoolProperties(PoolProperties poolProps)
        {
            if (_properties.TryFirst(
                properties => properties.FilePath.Equals(poolProps.FilePath, StringComparison.OrdinalIgnoreCase),
                out var result))
            {
                result.MaxCapacity = poolProps.MaxCapacity;
                result.MaxWaterReuse = poolProps.MaxWaterReuse;
                result.MonitorFrequency = poolProps.MonitorFrequency;
            }

            return true;
        }

        public List<PoolProperties> GetPoolProperties()
        {
            return new List<PoolProperties>(_properties);
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

        private void LoadPools(string configFile)
        {
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
                var fullPoolLocation = Path.Combine(_poolDirectory, poolProperty.FilePath);
                var param = poolProperty.PoolType.Parameters;

                IPool pool = null;
                switch (poolProperty.PoolType.Type)
                {
                    case PoolTypes.SHA:
                        pool = new ShaPool(GetConstructionParameters(param as ShaParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.AES:
                        pool = new AesPool(GetConstructionParameters(param as AesParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.SHA_MCT:
                        pool = new ShaMctPool(GetConstructionParameters(param as ShaParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.AES_MCT:
                        pool = new AesMctPool(GetConstructionParameters(param as AesParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.TDES_MCT:
                        pool = new TdesMctPool(GetConstructionParameters(param as TdesParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.SHA3_MCT:
                        pool = new Sha3MctPool(GetConstructionParameters(param as Sha3Parameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.CSHAKE_MCT:
                        pool = new CShakeMctPool(GetConstructionParameters(param as CShakeParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.PARALLEL_HASH_MCT:
                        pool = new ParallelHashMctPool(GetConstructionParameters(param as ParallelHashParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.TUPLE_HASH_MCT:
                        pool = new TupleHashMctPool(GetConstructionParameters(param as TupleHashParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.DSA_PQG:
                        pool = new DsaPqgPool(GetConstructionParameters(param as DsaDomainParametersParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.ECDSA_KEY:
                        pool = new EcdsaKeyPool(GetConstructionParameters(param as EcdsaKeyParameters, poolProperty, fullPoolLocation));
                        break;

                    case PoolTypes.RSA_KEY:
                        pool = new RsaKeyPool(GetConstructionParameters(param as RsaKeyParameters, poolProperty, fullPoolLocation));
                        break;

                    default:
                        throw new Exception("No pool model found");
                }

                Pools.Add(pool);
            }
        }

        private PoolConstructionParameters<TParam> GetConstructionParameters<TParam>(TParam param, PoolProperties poolProperties, string fullPoolLocation)
            where TParam : IParameters
        {
            return new PoolConstructionParameters<TParam>()
            {
                JsonConverters = _jsonConverters,
                PoolConfig = _poolConfig,
                PoolProperties = poolProperties,
                WaterType = param,
                FullPoolLocation = fullPoolLocation
            };
        }
    }
}
