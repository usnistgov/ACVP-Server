using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.KeyGen
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed => true;
        public bool Deferred => true;
        public TestGroup ParentGroup { get; set; }

        [JsonIgnore] public XecdhKeyPair KeyPair { get; set; } = new XecdhKeyPair();
        [JsonProperty(PropertyName = "privateKey", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString PrivateKey
        {
            get => KeyPair?.PrivateKey?.PadToModulusMsb(BitString.BITSINBYTE);
            set => KeyPair.PrivateKey = value;
        }

        [JsonProperty(PropertyName = "publicKey", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString PublicKey
        {
            get => KeyPair?.PublicKey?.PadToModulusMsb(BitString.BITSINBYTE);
            set => KeyPair.PublicKey = value;
        }
    }
}
