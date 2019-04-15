using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.EDDSA.v1_0.SigGen
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed => true;
        public bool Deferred => true;
        public TestGroup ParentGroup { get; set; }

        [JsonProperty(PropertyName = "message")]
        public BitString Message { get; set; }

        [JsonProperty(PropertyName = "context")]
        public BitString Context { get; set; } = null;

        [JsonIgnore] public EdSignature Signature { get; set; } = new EdSignature();

        [JsonProperty(PropertyName = "signature", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Sig
        {
            get => Signature?.Sig;
            set => Signature.Sig = value;
        }
    }
}
