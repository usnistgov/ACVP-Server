using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_GCM_SIV.v1_0
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        [JsonProperty(PropertyName = "pt")]
        public BitString Plaintext { get; set; }

        [JsonProperty(PropertyName = "key")]
        public BitString Key { get; set; }

        [JsonProperty(PropertyName = "aad")]
        public BitString AAD { get; set; }

        [JsonProperty(PropertyName = "iv")]
        public BitString IV { get; set; }

        [JsonProperty(PropertyName = "ct")]
        public BitString Ciphertext { get; set; }


    }
}
