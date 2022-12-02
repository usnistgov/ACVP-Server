using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Sp800_56Br2.DpComponent
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public int Modulo { get; set; }
        public string TestType { get; set; }
        
        [JsonProperty(PropertyName = "keyMode")]
        public PrivateKeyModes KeyMode { get; set; }
        
        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}
