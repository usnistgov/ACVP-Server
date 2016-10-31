using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestGroup : ITestGroup
    {
        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }
        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }
        [JsonProperty(PropertyName = "keylen")]
        public int KeyLength { get; set; }
        [JsonProperty(PropertyName = "ivlen")]
        public int IVLength { get; set; }
        [JsonProperty(PropertyName = "ivgen")]
        public string IVGeneration { get; set; }
        [JsonProperty(PropertyName = "ivgenmode")]
        public string IVGenerationMode { get; set; }
        [JsonProperty(PropertyName = "ptlen")]
        public int PTLength { get; set; }
        [JsonProperty(PropertyName = "aadlen")]
        public int AADLength { get; set; }
        [JsonProperty(PropertyName = "taglen")]
        public int TagLength { get; set; }
        public List<ITestCase> Tests { get; set; }
    }
}
