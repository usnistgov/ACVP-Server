namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeTestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }
        public bool Merge(ITestCase otherTest)
        {
            return true;
        }
    }
}
