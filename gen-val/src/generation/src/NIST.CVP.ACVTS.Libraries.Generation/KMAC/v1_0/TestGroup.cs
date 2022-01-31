using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KMAC.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }

        [JsonProperty(PropertyName = "keyLens")]
        public MathDomain KeyLengths { get; set; }

        [JsonProperty(PropertyName = "xof")]
        public bool XOF { get; set; }

        [JsonIgnore]
        public int DigestSize { get; set; }

        [JsonProperty(PropertyName = "msgLen")]
        public int MessageLength { get; set; }

        [JsonProperty(PropertyName = "msgLens")]
        public MathDomain MsgLengths { get; set; }

        [JsonProperty(PropertyName = "macLen")]
        public MathDomain MacLengths { get; set; }

        [JsonProperty(PropertyName = "hexCustomization")]
        public bool HexCustomization { get; set; } = false;

        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}
