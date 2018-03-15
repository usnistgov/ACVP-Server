using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        public string Algorithm { get; set; } = "DSA";
        public string Mode { get; set; } = "KeyGen";
        public bool IsSample { get; set; }
        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();
    }
}
