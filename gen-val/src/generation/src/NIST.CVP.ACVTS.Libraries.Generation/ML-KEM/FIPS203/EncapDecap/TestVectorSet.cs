using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
{
    public int VectorSetId { get; set; }
    public string Algorithm { get; set; } = "ML-KEM";
    public string Mode { get; set; } = "EncapDecap";
    public string Revision { get; set; } = "FIPS203";
    public bool IsSample { get; set; }
    public List<TestGroup> TestGroups { get; set; } = new();
}
