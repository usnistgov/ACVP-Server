using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet, IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach(var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                foreach(var test in group.Tests.Select(t => (TestCase)t))
                {
                    if(group.TestType.ToLower() == "gdt")
                    {
                        list.Add(new TestCaseValidatorGDT(test));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorNull($"Could not determine TestType for TestCase", test.TestCaseId));
                    }
                }
            }

            return list;
        }
    }
}
