using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.SHA2.v1_0
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        [JsonProperty(PropertyName = "digestSize")]
        public List<string> DigestSizes { get; set; }
        
        //[JsonProperty(PropertyName = "inBit")]
        //public bool BitOriented { get; set; } = false;

        //[JsonProperty(PropertyName = "inEmpty")]
        //public bool IncludeNull { get; set; } = false;

        [JsonProperty(PropertyName = "messageLength")]
        public MathDomain MessageLength { get; set; }
    }
}
