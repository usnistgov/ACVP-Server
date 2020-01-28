using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    public class TestGroupBase : ITestGroup<TestGroupBase, TestCaseBase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TestCaseBase> Tests { get; set; }
    }
}