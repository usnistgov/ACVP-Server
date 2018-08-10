using Newtonsoft.Json;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.PoolModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NIST.CVP.Pools
{
    public class PoolManager
    {
        public readonly List<ShaPool> ShaPools = new List<ShaPool>();
        public readonly List<AesPool> AesPools = new List<AesPool>();

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

        public int GetPoolCount(IParameters param)
        {
            switch (param)
            {
                case AesParameters aesParam:
                    return AesPools.First(pool => pool.WaterType.Equals(aesParam)).WaterLevel;
                case ShaParameters shaParam:
                    return ShaPools.First(pool => pool.WaterType.Equals(shaParam)).WaterLevel;
            }

            return 0;
        }

        public object GetResultFromPool(IParameters param)
        {
            switch (param)
            {
                case AesParameters aesParam:
                    return AesPools.First(pool => pool.WaterType.Equals(aesParam)).GetNext();
                case ShaParameters shaParam:
                    return ShaPools.First(pool => pool.WaterType.Equals(shaParam)).GetNext();
            }

            return null;
        }

        private void LoadPools(string configFile, string poolDirectory)
        {
            var config = JsonConvert.DeserializeObject<PoolProperties[]>(File.ReadAllText(configFile));

            foreach (var poolProperty in config)
            {
                var filePath = Path.Combine(poolDirectory, poolProperty.FilePath);
                var param = poolProperty.PoolType.Parameters;

                // TODO need a case for each one?
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
                }
            }
        }
    }
}
