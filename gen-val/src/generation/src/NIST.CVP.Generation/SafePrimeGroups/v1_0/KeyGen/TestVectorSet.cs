using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SafePrimeGroups.v1_0.KeyGen
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; } = "SafePrimeGroups";
        public string Mode { get; set; } = "KeyGen";
        public string Revision { get; set; } = "1.0";
        public bool IsSample { get; set; }

        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();
    }
}