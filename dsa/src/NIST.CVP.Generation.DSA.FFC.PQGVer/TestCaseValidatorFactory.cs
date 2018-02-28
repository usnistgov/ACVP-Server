using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase)t))
                {
                    if (group.TestType.ToLower() == "gdt")
                    {
                        list.Add(new TestCaseValidator(test));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorNull("Could not find TestType for group", test.TestCaseId));
                    }
                }
            }

            return list;
        }
    }
}
