using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.PoolModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NIST.CVP.Pools
{
    public class PoolManager
    {
        public readonly List<ShaPool> ShaPools = new List<ShaPool>();
        public readonly List<AesPool> AesPools = new List<AesPool>();

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
            switch (paramHolder.Type)
            {
                case PoolTypes.AES:
                    return AesPools.First(pool => pool.WaterType.Equals(paramHolder.Parameters)).WaterLevel;
                case PoolTypes.SHA:
                    return ShaPools.First(pool => pool.WaterType.Equals(paramHolder.Parameters)).WaterLevel;
                default:
                    return 0;
            }
        }

        public bool AddResultToPool(ParameterHolder paramHolder)
        {
            switch (paramHolder.Type)
            {
                case PoolTypes.AES:
                    return AesPools.First(pool => pool.WaterType.Equals(paramHolder.Parameters)).AddWater(paramHolder.Result as AesResult);
                case PoolTypes.SHA:
                    return ShaPools.First(pool => pool.WaterType.Equals(paramHolder.Parameters)).AddWater(paramHolder.Result as HashResult);
                default:
                    return false;
            }
        }

        public object GetResultFromPool(ParameterHolder paramHolder)
        {
            switch (paramHolder.Type)
            {
                case PoolTypes.AES:
                    return AesPools.First(pool => pool.WaterType.Equals(paramHolder.Parameters)).GetNext();
                case PoolTypes.SHA:
                    return ShaPools.First(pool => pool.WaterType.Equals(paramHolder.Parameters)).GetNext();
                default:
                    return new PoolResult<IResult> { PoolEmpty = true };
            }
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
                        ShaPools.Add(shaPool);
                        break;

                    case PoolTypes.AES:
                        var aesPool = new AesPool(param as AesParameters, filePath, _jsonConverters);
                        AesPools.Add(aesPool);
                        break;

                    default:
                        throw new Exception("No pool found");
                }
            }
        }
    }
}
