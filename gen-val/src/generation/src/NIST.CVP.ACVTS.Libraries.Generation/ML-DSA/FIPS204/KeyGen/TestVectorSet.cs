using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.KeyGen;

public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
{
    public int VectorSetId { get; set; }
    public string Algorithm { get; set; } = "ML-DSA";
    public string Mode { get; set; } = "KeyGen";
    public string Revision { get; set; } = "FIPS204";
    public bool IsSample { get; set; }
    public List<TestGroup> TestGroups { get; set; } = new();
}
