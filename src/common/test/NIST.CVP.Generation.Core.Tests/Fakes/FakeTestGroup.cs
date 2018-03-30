using System.Collections.Generic;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeTestGroup : ITestGroup
    {
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

        public int TestGroupId { get; set; }
        public string TestType { get; }
        public List<ITestCase> Tests { get; }

        public int KeyLength { get; }
    }
}