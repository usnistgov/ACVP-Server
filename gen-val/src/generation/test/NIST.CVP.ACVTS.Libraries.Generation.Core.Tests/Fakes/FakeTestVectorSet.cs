using System.Collections.Generic;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.Fakes
{
    public class FakeTestVectorSet : ITestVectorSet<FakeTestGroup, FakeTestCase>
    {
        public int VectorSetId { get; set; } = 42;
        public string Algorithm { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public string Revision { get; set; } = "v1.0.0";
        public bool IsSample { get; set; } = false;
        public List<FakeTestGroup> TestGroups { get; set; } = new List<FakeTestGroup>();
    }
}
