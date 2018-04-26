using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TestVectorSet: ITestVectorSet<TestGroup, TestCase>
    {
        public string Algorithm { get; set; } = "TDES";
        public string Mode { get; set; } = "ECB";
        public bool IsSample { get; set; }
        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();
    }
}
