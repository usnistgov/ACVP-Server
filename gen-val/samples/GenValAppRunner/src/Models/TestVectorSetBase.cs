using NIST.CVP.ACVTS.Libraries.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.ACVTS.Generation.GenValApp.Models
{
    internal class TestVectorSetBase : ITestVectorSet<TestGroupBase, TestCaseBase>
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public List<TestGroupBase> TestGroups { get; set; }
    }
}