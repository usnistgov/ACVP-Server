using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA1
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet, IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                foreach(var test in group.Tests.Select(t => (TestCase)t))
                {
                    var workingTest = test;
                    if(group.TestType.ToLower() == "mct")
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
