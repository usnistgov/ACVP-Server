using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Generation.DSA.Ed.SigVer
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }

        [JsonProperty(PropertyName = "curve")]
        public Curve Curve { get; set; }

        [JsonProperty(PropertyName = "preHash")]
        public bool PreHash { get; set; }

        [JsonIgnore] public ITestCaseExpectationProvider<EddsaSignatureDisposition> TestCaseExpectationProvider { get; set; }
        
        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}
