using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        public IEnumerable<ITestCaseValidator<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestGroup, TestCase>>();

            foreach(var group in testVectorSet.TestGroups)
            {
                foreach(var test in group.Tests)
                {
                    if(group.TestType.ToLower() == "gdt")
                    {
                        list.Add(new TestCaseValidatorGDT(test));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorNull("Could not determine TestType for TestCase", test.TestCaseId));
                    }
                }
            }

            return list;
        }
    }
}
