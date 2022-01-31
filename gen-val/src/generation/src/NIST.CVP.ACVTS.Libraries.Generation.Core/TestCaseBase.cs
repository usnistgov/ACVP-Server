namespace NIST.CVP.ACVTS.Libraries.Generation.Core
{
    public class TestCaseBase : ITestCase<TestGroupBase, TestCaseBase>
    {
        public int TestCaseId { get; set; }
        public TestGroupBase ParentGroup { get; set; }
        public bool? TestPassed { get; }
        public bool Deferred { get; }
    }
}
