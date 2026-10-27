using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigGen;

public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
{
    public int VectorSetId { get; set; }
    public string Algorithm { get; set; } = "XMSS";
    public string Mode { get; set; } = "sigGen";
    public string Revision { get; set; } = "SP800-208";
    public bool IsSample { get; set; }

    public List<TestGroup> TestGroups { get; set; } = new();
}
