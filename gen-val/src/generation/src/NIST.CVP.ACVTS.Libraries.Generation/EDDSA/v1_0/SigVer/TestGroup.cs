using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigVer
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
