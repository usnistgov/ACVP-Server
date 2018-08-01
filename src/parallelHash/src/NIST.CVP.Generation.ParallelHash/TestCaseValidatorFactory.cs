using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.ParallelHash
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
    {
        public IEnumerable<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

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
