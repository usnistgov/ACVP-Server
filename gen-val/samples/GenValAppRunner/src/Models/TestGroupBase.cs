using NIST.CVP.ACVTS.Libraries.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.ACVTS.Generation.GenValApp.Models
{
    internal class TestGroupBase : ITestGroup<TestGroupBase, TestCaseBase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TestCaseBase> Tests { get; set; }
    }
}