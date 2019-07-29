using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.EDDSA.v1_0.SigVer
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        [JsonIgnore]
        public bool Deferred { get; set; }
        public TestGroup ParentGroup { get; set; }

        [JsonProperty(PropertyName = "reason")]
        public EddsaSignatureDisposition Reason { get; set; }
        [JsonProperty(PropertyName = "message")]
        public BitString Message { get; set; }

        [JsonProperty(PropertyName = "context")]
        public BitString Context { get; set; }

        [JsonIgnore] public EdKeyPair KeyPair { get; set; } = new EdKeyPair();
        [JsonProperty(PropertyName = "d")]
        public BitString D
        {
            get => KeyPair?.PrivateD?.PadToModulusMsb(BitString.BITSINBYTE);
            set => KeyPair.PrivateD = value;
        }

        [JsonProperty(PropertyName = "q")]
        public BitString Q
        {
            get => KeyPair?.PublicQ?.PadToModulusMsb(BitString.BITSINBYTE);
            set => KeyPair.PublicQ = value;
        }

        [JsonIgnore] public EdSignature Signature { get; set; } = new EdSignature();
        [JsonProperty(PropertyName = "signature")]
        public BitString Sig
        {
            get => Signature?.Sig?.PadToModulusMsb(BitString.BITSINBYTE);
            set => Signature.Sig = value;
        }
    }
}
