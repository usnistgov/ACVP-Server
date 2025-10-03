using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128;

public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
{
    public int VectorSetId { get; set; }
    public string Algorithm { get; set; } = "Ascon";
    public string Mode { get; set; } = "AEAD128";
    public string Revision { get; set; } = "SP800-232";
    public bool IsSample { get; set; }
    public List<TestGroup> TestGroups { get; set; } = new();
}
