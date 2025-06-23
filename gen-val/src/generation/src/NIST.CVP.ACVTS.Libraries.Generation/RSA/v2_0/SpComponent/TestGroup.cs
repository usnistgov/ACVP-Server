using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v2_0.SpComponent.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v2_0.SpComponent
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public int Modulo { get; set; }
        public string TestType { get; set; }
        
        [JsonProperty(PropertyName = "keyMode")]
        public PrivateKeyModes KeyMode { get; set; }
        
        [JsonIgnore]
        public PublicExponentModes PublicExponentMode { get; set; }

        [JsonIgnore]
        public BitString PublicExponent { get; set; }
        
        [JsonIgnore]
        // Used internally to build test cases for the group
        public RsaSignaturePrimitiveExpectationProvider TestCaseExpectationProvider { get; set; }
        
        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}
