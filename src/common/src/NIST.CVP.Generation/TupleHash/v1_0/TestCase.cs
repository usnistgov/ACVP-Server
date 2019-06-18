using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TupleHash.v1_0
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        
        [JsonIgnore]
        public bool? TestPassed { get; set; }
        
        [JsonIgnore]
        public bool Deferred { get; set; }

        [JsonProperty(PropertyName = "tuple")]
        public List<BitString> Tuple { get; set; }

        [JsonProperty(PropertyName = "len")]
        public int[] MessageLength {
            get
            {

                if (Tuple == null) return new int[] { 0 };
                var lengths = new int[Tuple.Count];
                for (int i = 0; i < Tuple.Count; i++)
                {
                    lengths[i] = Tuple.ElementAt(i).BitLength;
                }
                return lengths;
            }
        }

        [JsonProperty(PropertyName = "tupleLen")]
        public int TupleLength => Tuple?.Count ?? 0;

        [JsonProperty(PropertyName = "customization")]
        public string Customization { get; set; } = "";

        [JsonProperty(PropertyName = "customizationHex")]
        public BitString CustomizationHex { get; set; }

        [JsonProperty(PropertyName = "md")]
        public BitString Digest { get; set; }

        [JsonProperty(PropertyName = "outLen")]
        public int DigestLength { get; set; }

        public List<AlgoArrayResponse> ResultsArray { get; set; }
    }
}
