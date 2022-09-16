using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF.SP800_108r1.KMAC
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; } = "KDF";
        public string Mode { get; set; } = "KMAC";
        public string Revision { get; set; } = "Sp800-108r1";
        public bool IsSample { get; set; }
        public List<TestGroup> TestGroups { get; set; } = new();
    }
}
