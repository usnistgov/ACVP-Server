using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.KeyVer.TestCaseExpectations;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.KeyVer
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        
        [JsonProperty(PropertyName = "curve")]
        public Curve Curve { get; set; }

        // Used internally to build test cases with particular error cases
        [JsonIgnore] 
        public KeyExpectationProvider TestCaseExpectationProvider { get; set; }

        public string TestType { get; set; }
        
        public List<TestCase> Tests { get; set; } = [];
    }
}
