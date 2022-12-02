using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.TwoStep
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public bool IsSample { get; set; }
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TwoStepConfiguration KdfConfiguration { get; set; }
        public int ZLength { get; set; }
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? UsesHybridSharedSecret { get; set; }
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int AuxSharedSecretLen { get; set; }
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? MultiExpansion { get; set; }
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TwoStepMultiExpansionConfiguration KdfMultiExpansionConfiguration { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}
