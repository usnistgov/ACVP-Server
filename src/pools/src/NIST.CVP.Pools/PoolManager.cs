using Newtonsoft.Json;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.PoolTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace NIST.CVP.Pools
{
    public class PoolManager
    {
        public Collection<Pool<Parameters, Result>> Pools = new Collection<Pool<Parameters, Result>>();
        public List<Pool<ShaParameters, HashResult>> _shaPools = new List<Pool<ShaParameters, HashResult>>();
        public List<Pool<AesParameters, AesResult>> _aesPools = new List<Pool<AesParameters, AesResult>>();

        public PoolManager(string configFile)
        {
            LoadPools(configFile);
        }

        private void LoadPools(string filename)
        {
            var config = JsonConvert.DeserializeObject<PoolProperties[]>(File.ReadAllText(filename));

            foreach (var poolProperty in config)
            {
                var param = poolProperty.Parameters.Parameters;
                foreach (var pool in Pools)
                {
                    if (pool.PoolIsOfType(param))
                    {
                        throw new Exception("Pool type already exists!");
                    }
                    else
                    {
                    }
                }

                switch (poolProperty.Parameters.TypeId)
                {
                    case 1:
                        var shaPool = new Pool<ShaParameters, HashResult>(param as ShaParameters, poolProperty.FilePath);
                        _shaPools.Add(shaPool);
                        break;

                    case 2:
                        var aesPool = new AesPool(param as AesParameters, poolProperty.FilePath);
                        _aesPools.Add(aesPool);
                        break;
                }
            }
        }
    }
}
