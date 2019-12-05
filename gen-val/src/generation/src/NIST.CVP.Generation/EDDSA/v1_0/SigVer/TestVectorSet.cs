using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.EDDSA.v1_0.SigVer
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; } = "EDDSA";
        public string Mode { get; set; } = "SigVer";
        public string Revision { get; set; }
        public bool IsSample { get; set; }

        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();
    }
}
