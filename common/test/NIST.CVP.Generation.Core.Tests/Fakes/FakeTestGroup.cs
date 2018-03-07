using System.Collections.Generic;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeTestGroup : ITestGroup<FakeTestGroup, FakeTestCase>
    {
        public FakeTestGroup()
        {
            Tests = new List<FakeTestCase>();
            var fakeTestCase = new FakeTestCase
            {
                TestCaseId = 1,
                Deferred = false
            };
            Tests.Add(fakeTestCase);

            KeyLength = 1;
        }

        public int TestGroupId { get; set; }
        public string TestType { get; }
        public List<FakeTestCase> Tests { get; set; }

        public int KeyLength { get; }
    }
}