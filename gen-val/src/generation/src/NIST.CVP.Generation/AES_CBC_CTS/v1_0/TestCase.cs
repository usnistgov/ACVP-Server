using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Generation.AES_CBC_CTS.v1_0
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        [JsonIgnore] public bool Deferred => false;
        [JsonIgnore] public bool? TestPassed => true;
        [JsonProperty(PropertyName = "iv")]
        public BitString IV { get; set; }
        [JsonProperty(PropertyName = "pt")]
        public BitString PlainText { get; set; }
        [JsonProperty(PropertyName = "payloadLen")]
        public int PayloadLen { get; set; }
        [JsonProperty(PropertyName = "key")]
        public BitString Key { get; set; }
        [JsonProperty(PropertyName = "ct")]
        public BitString CipherText { get; set; }
        [JsonProperty(PropertyName = "resultsArray")]
        public List<AlgoArrayResponse> ResultsArray { get; set; }
    }
}
