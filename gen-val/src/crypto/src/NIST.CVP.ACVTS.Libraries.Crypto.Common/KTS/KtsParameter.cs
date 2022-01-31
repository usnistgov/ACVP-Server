using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS
{
    public class KtsParameter
    {
        [JsonIgnore]
        public KasHashAlg KtsHashAlg { get; set; }
        [JsonIgnore]
        public string AssociatedDataPattern { get; set; }
        [JsonIgnore]
        public FixedInfoEncoding Encoding { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Context { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString AlgorithmId { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Label { get; set; }

        /// <summary>
        /// Only serialize if at least one of the serializable properties is not null.
        /// </summary>
        [JsonIgnore]
        public bool ShouldSerialize => Context != null || AlgorithmId != null || Label != null;
    }
}
