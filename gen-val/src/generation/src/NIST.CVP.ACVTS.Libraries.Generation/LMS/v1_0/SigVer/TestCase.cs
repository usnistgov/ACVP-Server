using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigVer
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        [JsonIgnore]
        public bool Deferred => true;
        public TestGroup ParentGroup { get; set; }

        [JsonProperty(PropertyName = "message")]
        public BitString Message { get; set; }

        [JsonProperty(PropertyName = "signature")]
        public BitString Signature { get; set; }

        [JsonProperty(PropertyName = "publicKey")]
        public BitString PublicKey { get; set; }

        [JsonProperty(PropertyName = "reason")]
        public LmsSignatureDisposition Reason { get; set; }
    }
}
