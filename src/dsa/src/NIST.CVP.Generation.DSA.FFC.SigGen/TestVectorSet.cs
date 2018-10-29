using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; } = "DSA";
        public string Mode { get; set; } = "SigGen";
        public bool IsSample { get; set; }
        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();
    }
}
