using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.PoolTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Common.Oracle;

namespace NIST.CVP.Pools
{
    public class PoolManager
    {
        public List<ShaPool> _shaPools = new List<ShaPool>();
        public List<AesPool> _aesPools = new List<AesPool>();

        public PoolManager(string configFile)
        {
            LoadPools(configFile);
        }

        public string GetResultFromPool(IParameters param)
        {
            // Cast param to specific type
            // Get result from pool
            // Serialize result into json and return that
            return "testtest";
        }

        public PoolResult<AesResult> GetAesResultFromPool(AesParameters param)
        {
            // TODO need an Equals call for each parameter type?
            try
            {
                return _aesPools.First(pool => pool.WaterType.Equals(param)).GetNext();
            }
            catch (Exception)
            {
                return new PoolResult<AesResult> {PoolEmpty = true};
            }
        }

        public PoolResult<HashResult> GetHashResultFromPool(ShaParameters param)
        {
            return _shaPools.First(pool => pool.WaterType.Equals(param)).GetNext();
        }

        private void LoadPools(string filename)
        {
            var config = JsonConvert.DeserializeObject<PoolProperties[]>(File.ReadAllText(filename));

            foreach (var poolProperty in config)
            {
                var param = poolProperty.Parameters.Parameters;
                var filePath = Path.Combine(Path.GetDirectoryName(filename), @"..\Pools\", poolProperty.FilePath);

                // TODO need a case for each one? Should use switch with an ID, or just try to cast the param?
                switch (poolProperty.Parameters.TypeId)
                {
                    case 1:
                        var shaPool = new ShaPool(param as ShaParameters, filePath);
                        _shaPools.Add(shaPool);
                        break;

                    case 2:
                        var aesPool = new AesPool(param as AesParameters, filePath);
                        _aesPools.Add(aesPool);
                        break;
                }
            }
        }
    }
}
