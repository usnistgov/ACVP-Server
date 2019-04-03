using NIST.CVP.Generation.Core.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NIST.CVP.Generation.AES_GCM_SIV
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
    {
        public IEnumerable<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => g))
            {
                foreach (var test in group.Tests.Select(t => t))
                {
                    if (group.Function == "encrypt")
                    {
                        list.Add(new TestCaseValidatorEncrypt(test));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorDecrypt(test));
                    }
                }
            }

            return list;
        }
    }
}
