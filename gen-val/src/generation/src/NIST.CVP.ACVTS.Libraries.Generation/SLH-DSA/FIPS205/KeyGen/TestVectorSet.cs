using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.KeyGen;

public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
{
    public int VectorSetId { get; set; }
    public string Algorithm { get; set; } = "SLH-DSA";
    public string Mode { get; set; } = "keyGen";
    public string Revision { get; set; } = "FIPS205";
    public bool IsSample { get; set; }
    public List<TestGroup> TestGroups { get; set; } = new();
}
