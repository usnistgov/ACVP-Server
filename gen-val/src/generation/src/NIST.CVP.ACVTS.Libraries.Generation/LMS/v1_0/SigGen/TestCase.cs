using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed => true;
        public bool Deferred => true;
        public TestGroup ParentGroup { get; set; }

        [JsonProperty(PropertyName = "message")]
        public BitString Message { get; set; }

        [JsonProperty(PropertyName = "signature")]
        public BitString Signature { get; set; }

        [JsonProperty(PropertyName = "seed")]
        public BitString Seed { get; set; }

        [JsonProperty(PropertyName = "rootI")]
        public BitString RootI { get; set; }

        public List<AlgoArrayResponse> ResultsArray { get; set; }
    }
}
