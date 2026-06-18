using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.BLAKE2.v1_0
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        [JsonProperty(PropertyName = "digestLen")]
        public List<int> DigestLengths { get; set; }

        [JsonProperty(PropertyName = "msgLen")]
        public MathDomain MessageLength { get; set; }

        [JsonProperty(PropertyName = "keyLen")]
        public MathDomain KeyLength { get; set; }
    }
}
