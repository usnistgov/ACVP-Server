using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CSHAKE
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        public IEnumerable<ITestCaseValidator<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups)
            {
                foreach (var test in group.Tests)
                {
                    var workingTest = test;
                    if (group.TestType.ToLower() == "mct")
                    {
                        list.Add(new TestCaseValidatorMCTHash(workingTest));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorHash(workingTest));
                    }
                }
            }

            return list;
        }
    }
}
