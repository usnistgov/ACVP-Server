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

namespace NIST.CVP.Pools
{
    public class PoolManager
    {
        public readonly List<IPool> Pools = new List<IPool>();

        private PoolProperties[] _properties;
        private string _poolDirectory;

        private readonly IList<JsonConverter> _jsonConverters = new List<JsonConverter>
        {
            new BitstringConverter(),
            new DomainConverter(),
            new BigIntegerConverter(),
            new StringEnumConverter()
        };

        public PoolManager(string configFile, string poolDirectory)
        {
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

        private void LoadPools(string configFile, string poolDirectory)
        {
            _poolDirectory = poolDirectory;

            var fullConfigFile = Path.Combine(_poolDirectory, configFile);
            _properties = JsonConvert.DeserializeObject<PoolProperties[]>(File.ReadAllText(fullConfigFile));

            foreach (var poolProperty in _properties)
            {
                var filePath = Path.Combine(_poolDirectory, poolProperty.FilePath);
                var param = poolProperty.PoolType.Parameters;

                switch (poolProperty.PoolType.Type)
                {
                    case PoolTypes.SHA:
                        var shaPool = new ShaPool(param as ShaParameters, filePath, _jsonConverters);
                        Pools.Add(shaPool);
                        break;

                    case PoolTypes.AES:
                        var aesPool = new AesPool(param as AesParameters, filePath, _jsonConverters);
                        Pools.Add(aesPool);
                        break;

                    case PoolTypes.SHA_MCT:
                        var shaMctPool = new ShaMctPool(param as ShaParameters, filePath, _jsonConverters);
                        Pools.Add(shaMctPool);
                        break;

                    default:
                        throw new Exception("No pool found");
                }
            }
        }
    }
}
