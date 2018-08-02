using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Collections.Generic;
using System.IO;

namespace NIST.CVP.Pools
{
    public class PoolManager
    {
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
                switch (poolProperty.Parameters.TypeId)
                {
                    case 1:
                        var pool1 = new Pool<ShaParameters, HashResult>(param as ShaParameters, poolProperty.FilePath)
                        {
                            MaxWaterLevel = poolProperty.MaxCapacity,
                            MonitorFrequency = poolProperty.MonitorFrequency
                        };

                        _shaPools.Add(pool1);
                        break;

                    case 2:
                        var pool2 = new Pool<AesParameters, AesResult>(param as AesParameters, poolProperty.FilePath)
                        {
                            MaxWaterLevel = poolProperty.MaxCapacity,
                            MonitorFrequency = poolProperty.MonitorFrequency
                        };

                        _aesPools.Add(pool2);
                        break;
                }
            }
        }
    }
}
