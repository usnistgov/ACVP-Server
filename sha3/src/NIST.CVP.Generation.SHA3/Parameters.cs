using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        
        [JsonProperty(PropertyName = "digestSizes")]
        public int[] DigestSizes { get; set; }

        [JsonProperty(PropertyName = "inBit")]
        public bool BitOrientedInput { get; set; } = false;

        [JsonProperty(PropertyName = "outBit")]
        public bool BitOrientedOutput { get; set; } = false;

        [JsonProperty(PropertyName = "inEmpty")]
        public bool IncludeNull { get; set; } = false;

        [JsonProperty(PropertyName = "outLengthMin")]
        public int MinOutputLength { get; set; } = 0;

        [JsonProperty(PropertyName = "outLengthMax")]
        public int MaxOutputLength { get; set; } = 0;
    }
}
