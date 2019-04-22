using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.EDDSA.v1_0.KeyGen
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed => true;
        public bool Deferred => true;
        public TestGroup ParentGroup { get; set; }

        [JsonIgnore] public EdKeyPair KeyPair { get; set; } = new EdKeyPair();
        [JsonProperty(PropertyName = "d", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString D
        {
            get => KeyPair.PrivateD;
            set => KeyPair.PrivateD = value;
        }

        [JsonProperty(PropertyName = "q", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Q
        {
            get => KeyPair.PublicQ;
            set => KeyPair.PublicQ = value;
        }
    }
}
