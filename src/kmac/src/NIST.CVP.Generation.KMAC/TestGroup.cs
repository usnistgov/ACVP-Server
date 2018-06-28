using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KMAC
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }

        public string TestType { get; set; }

        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }

        [JsonProperty(PropertyName = "msgLen")]
        public int MessageLength { get; set; }

        [JsonProperty(PropertyName = "macLen")]
        public int MacLength { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        [JsonProperty(PropertyName = "xof")]
        public bool XOF { get; set; }

        [JsonProperty(PropertyName = "digestSize")]
        public int DigestSize { get; set; }

        [JsonProperty(PropertyName = "customization")]
        public string Customization { get; set; }
    }
}
