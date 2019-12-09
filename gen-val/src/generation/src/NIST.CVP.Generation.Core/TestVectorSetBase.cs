using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    public class TestVectorSetBase : ITestVectorSet<TestGroupBase, TestCaseBase>
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public List<TestGroupBase> TestGroups { get; set; }
    }
}