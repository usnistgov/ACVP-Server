using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC_Component;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.Sp800_56Ar3.Ecc_Component
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; } = "KAS-ECC";
        public string Mode { get; set; } = "CDH-Component";
        public string Revision { get; set; } = "Sp800-56Ar3";
        public bool IsSample { get; set; }

        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();
    }
}
