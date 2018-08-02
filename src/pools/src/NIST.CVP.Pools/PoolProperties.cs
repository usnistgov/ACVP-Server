using Newtonsoft.Json;
using NIST.CVP.Common.Oracle;

namespace NIST.CVP.Pools
{
    [JsonConverter(typeof(ParametersConverter))]
    public class PoolProperties
    {
        public ParameterHolder Parameters { get; set; }
        public string FilePath { get; set; }
        public int MaxCapacity { get; set; }
        public int MonitorFrequency { get; set; }
    }
}
