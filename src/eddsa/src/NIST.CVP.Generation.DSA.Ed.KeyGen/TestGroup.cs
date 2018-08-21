using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.DSA.Ed.KeyGen
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        [JsonProperty(PropertyName = "curve")]
        public Curve Curve { get; set; }
        [JsonProperty(PropertyName = "secretGenerationMode")]
        public SecretGenerationMode SecretGenerationMode { get; set; }
    }
}
