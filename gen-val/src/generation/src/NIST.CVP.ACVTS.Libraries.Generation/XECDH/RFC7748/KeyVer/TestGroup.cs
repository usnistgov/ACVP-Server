using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.KeyVer
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        [JsonProperty(PropertyName = "curve")]
        public Curve Curve { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        // Used internally to build test cases with particular error cases
        [JsonIgnore] public ITestCaseExpectationProvider<XecdhKeyDisposition> TestCaseExpectationProvider { get; set; }
    }
}
