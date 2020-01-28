using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_FFX.v1_0.Base
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {

        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        [JsonIgnore] public bool Deferred => false;
        [JsonIgnore] public bool? TestPassed => true;
        [JsonProperty(PropertyName = "tweak")]
        public BitString Tweak { get; set; }
        public int TweakLen => Tweak?.BitLength ?? 0;
        [JsonProperty(PropertyName = "pt")]
        public string PlainText { get; set; }
        [JsonProperty(PropertyName = "key")]
        public BitString Key { get; set; }
        [JsonProperty(PropertyName = "ct")]
        public string CipherText { get; set; }
    }
}
