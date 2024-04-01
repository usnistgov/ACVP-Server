using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigGen
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

        [JsonProperty(PropertyName = "contextLength")]
        public int ContextLength { get; set; }

        [JsonIgnore] public EdSignature Signature { get; set; } = new EdSignature();

        [JsonProperty(PropertyName = "signature", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Sig
        {
            get => Signature?.Sig?.PadToModulusMsb(BitString.BITSINBYTE);
            set => Signature.Sig = value;
        }
    }
}
