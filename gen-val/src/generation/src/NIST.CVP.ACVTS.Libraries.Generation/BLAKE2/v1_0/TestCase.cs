using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.BLAKE2.v1_0
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }

        [JsonIgnore]
        public bool? TestPassed { get; set; }

        [JsonIgnore]
        public bool Deferred { get; set; }

        [JsonProperty(PropertyName = "msg")]
        public BitString Message { get; set; }

        [JsonProperty(PropertyName = "len")]
        public int MessageLength => Message?.BitLength ?? 0;

        [JsonProperty(PropertyName = "key")]
        public BitString Key { get; set; }

        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength => Key?.BitLength ?? 0;

        [JsonProperty(PropertyName = "digestLen")]
        public int DigestLength { get; set; }

        [JsonProperty(PropertyName = "md")]
        public BitString Digest { get; set; }
    }
}
