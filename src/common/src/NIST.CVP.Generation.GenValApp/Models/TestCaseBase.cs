using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.GenValApp.Models
{
    internal class TestCaseBase : ITestCase<TestGroupBase, TestCaseBase>
    {
        public int TestCaseId { get; set; }
        public TestGroupBase ParentGroup { get; set; }
        public bool? TestPassed { get; }
        public bool Deferred { get; }
    }
}