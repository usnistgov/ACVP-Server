using Newtonsoft.Json;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Pools.PoolTypes;
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

        public PoolManager(string configFile)
        {
            LoadPools(configFile);
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

        private void LoadPools(string filename)
        {
            var config = JsonConvert.DeserializeObject<PoolProperties[]>(File.ReadAllText(filename));

            foreach (var poolProperty in config)
            {
                var param = poolProperty.Parameters.Parameters;
                var filePath = Path.Combine(new FileInfo(filename).DirectoryName, poolProperty.FilePath);

                // TODO need a case for each one? Should use switch with an ID, or just try to cast the param?
                switch (poolProperty.Parameters.TypeId)
                {
                    case 1:
                        var shaPool = new ShaPool(param as ShaParameters, filePath, _jsonConverters);
                        ShaPools.Add(shaPool);
                        break;

                    case 2:
                        var aesPool = new AesPool(param as AesParameters, filePath, _jsonConverters);
                        AesPools.Add(aesPool);
                        break;
                }
            }
        }
    }
}
