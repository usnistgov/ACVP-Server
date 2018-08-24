using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.Ed.SigVer
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }
        public TestGroup ParentGroup { get; set; }

        [JsonProperty(PropertyName = "reason")]
        public EddsaSignatureDisposition Reason { get; set; }
        [JsonProperty(PropertyName = "message")]
        public BitString Message { get; set; }

        [JsonProperty(PropertyName = "context")]
        public BitString Context { get; set; }

        [JsonIgnore] public EdSignature Signature { get; set; } = new EdSignature();
        [JsonProperty(PropertyName = "sig", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Sig
        {
            get => Signature.Sig;
            set => Signature.Sig = value;
        }
        public EdKeyPair KeyPair = new EdKeyPair();
    }
}
