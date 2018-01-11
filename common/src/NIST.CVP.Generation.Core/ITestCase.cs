namespace NIST.CVP.Generation.Core
{
    public interface ITestCase
    {
        int TestCaseId { get; set; }
        bool FailureTest { get; }
        bool Deferred { get; }
        bool Merge(ITestCase otherTest);
    }
}