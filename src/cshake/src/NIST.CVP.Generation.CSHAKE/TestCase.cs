using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CSHAKE
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        [JsonProperty(PropertyName = "msg")]
        public BitString Message { get; set; }

        [JsonProperty(PropertyName = "len")]
        public int MessageLength {
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
