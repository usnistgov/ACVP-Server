using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.AES_GCM_SIV
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; } = "AES";
        public string Mode { get; set; } = "GCM-SIV";
        public bool IsSample { get; set; }
        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();
    }
}
