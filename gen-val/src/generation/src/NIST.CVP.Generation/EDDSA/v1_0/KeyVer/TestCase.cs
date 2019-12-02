using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.EDDSA.v1_0.KeyVer
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        [JsonIgnore]
        public bool Deferred => false;
        public TestGroup ParentGroup { get; set; }
        public EddsaKeyDisposition Reason { get; set; }

        [JsonIgnore] public EdKeyPair KeyPair { get; set; } = new EdKeyPair();
        [JsonProperty(PropertyName = "d", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString D
        {
            get => KeyPair?.PrivateD?.PadToModulusMsb(BitString.BITSINBYTE);
            set => KeyPair.PrivateD = value;
        }

        [JsonProperty(PropertyName = "q", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Q
        {
            get => KeyPair?.PublicQ?.PadToModulusMsb(BitString.BITSINBYTE);
            set => KeyPair.PublicQ = value;
        }
    }
}
