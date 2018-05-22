using System.Collections.Generic;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeTestVectorSet : ITestVectorSet<FakeTestGroup, FakeTestCase>
    {
        public string Algorithm { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public bool IsSample { get; set; } = false;
        public List<FakeTestGroup> TestGroups { get; set; } = new List<FakeTestGroup>();
    }
}
