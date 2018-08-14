using Newtonsoft.Json;
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

        private readonly IList<JsonConverter> _jsonConverters = new List<JsonConverter>
        {
            new BitstringConverter(),
            new DomainConverter(),
            new BigIntegerConverter()
        };

        public PoolManager(string configFile, string poolDirectory)
        {
            LoadPools(configFile, poolDirectory);
        }

        public int GetPoolCount(ParameterHolder paramHolder)
        {
            if (Pools.TryFirst(pool => pool.Param.Equals(paramHolder.Parameters), out var result))
            {
                return result.WaterLevel;
            }

            return 0;
        }

        public bool AddResultToPool(ParameterHolder paramHolder)
        {
            if (Pools.TryFirst(pool => pool.Param.Equals(paramHolder.Parameters), out var result))
            {
                return result.AddWater(paramHolder.Result);
            }

            return false;
        }

        public object GetResultFromPool(ParameterHolder paramHolder)
        {
            if (Pools.TryFirst(pool => pool.Param.Equals(paramHolder.Parameters), out var result))
            {
                return result.GetNextUntyped();
            }

            return new PoolResult<IResult> { PoolEmpty = true };
        }

        public bool SavePools()
        {
            return true;
        }

        private void LoadPools(string configFile, string poolDirectory)
        {
            _properties = JsonConvert.DeserializeObject<PoolProperties[]>(File.ReadAllText(configFile));

            foreach (var poolProperty in _properties)
            {
                var filePath = Path.Combine(poolDirectory, poolProperty.FilePath);
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

                    default:
                        throw new Exception("No pool found");
                }
            }
        }
    }
}
