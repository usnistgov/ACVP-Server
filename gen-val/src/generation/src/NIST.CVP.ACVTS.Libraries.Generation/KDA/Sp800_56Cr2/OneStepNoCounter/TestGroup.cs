using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.KDA.Shared;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.OneStepNoCounter
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public bool IsSample { get; set; }
        public OneStepNoCounterConfiguration KdfConfiguration { get; set; }
        public int ZLength { get; set; }
        
        [JsonIgnore]
        public KdaExpectationProvider KdaExpectationProvider { get; set; }
        
        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}
