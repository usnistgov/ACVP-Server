using System.Collections.Generic;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeTestGroup : ITestGroup
    {
        public List<ITestCase> Tests { get; }
        public bool MergeTests(List<ITestCase> testsToMerge)
        {
            throw new System.NotImplementedException();
        }

        public int KeyLength { get; }

        public FakeTestGroup()
        {
            Tests = new List<ITestCase>();
            var fakeTestCase = new FakeTestCase
            {
                TestCaseId = 1,
                Deferred = false,
                FailureTest = false
            };
            Tests.Add(fakeTestCase);

            KeyLength = 1;
        }

        public string TestType { get; }
    }
}