namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.Fakes
{
    public class FakeTestCase : ITestCase<FakeTestGroup, FakeTestCase>
    {
        public int TestCaseId { get; set; }
        public FakeTestGroup ParentGroup { get; set; }
        public bool Deferred { get; set; }
        public bool? TestPassed => true;
    }
}
