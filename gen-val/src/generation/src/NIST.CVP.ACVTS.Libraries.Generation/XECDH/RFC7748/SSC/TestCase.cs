using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.SSC
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }

        public bool? TestPassed => true;

        public bool Deferred { get; set; } = true;

        public TestGroup ParentGroup { get; set; }

        [JsonIgnore] public XecdhKeyPair KeyPairPartyServer { get; set; } = new XecdhKeyPair();

        [JsonProperty(PropertyName = "privateServer")]
        public BitString PrivateKeyServer
        {
            get => KeyPairPartyServer.PrivateKey;
            set => KeyPairPartyServer.PrivateKey = value;
        }

        [JsonProperty(PropertyName = "publicServer")]
        public BitString PublicKeyServer
        {
            get => KeyPairPartyServer.PublicKey;
            set => KeyPairPartyServer.PublicKey = value;
        }


        [JsonIgnore]
        public XecdhKeyPair KeyPairPartyIut { get; set; } = new XecdhKeyPair();

        [JsonProperty(PropertyName = "privateIut")]
        public BitString PrivateKeyIut
        {
            get => KeyPairPartyIut.PrivateKey;
            set => KeyPairPartyIut.PrivateKey = value;
        }

        [JsonProperty(PropertyName = "publicIut")]
        public BitString PublicKeyIut
        {
            get => KeyPairPartyIut.PublicKey;
            set => KeyPairPartyIut.PublicKey = value;
        }

        public BitString Z { get; set; }
    }
}
