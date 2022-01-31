using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.CSHAKE.v1_0
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
        public int MessageLength
        {
            get
            {
                if (Message == null) return 0;
                return Message.BitLength;
            }
        }

        [JsonProperty(PropertyName = "functionName")]
        public string FunctionName { get; set; } = "";

        [JsonProperty(PropertyName = "customization")]
        public string Customization { get; set; } = "";

        [JsonProperty(PropertyName = "customizationHex")]
        public BitString CustomizationHex { get; set; }

        [JsonProperty(PropertyName = "md")]
        public BitString Digest { get; set; }

        [JsonProperty(PropertyName = "outLen")]
        public int DigestLength { get; set; }

        public List<AlgoArrayResponseWithCustomization> ResultsArray { get; set; }
    }
}
