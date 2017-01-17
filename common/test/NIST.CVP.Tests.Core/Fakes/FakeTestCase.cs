using NIST.CVP.Generation.Core;

namespace NIST.CVP.Tests.Core.Fakes
{
    public class FakeTestCase : ITestCase
    {
        public int TestCaseId { get { return 1; } }
        public bool FailureTest { get { return false; } }
        public bool Deferred { get { return false; } }
        public bool Merge(ITestCase otherTest)
        {
            return true;
        }
    }
}
