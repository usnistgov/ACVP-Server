using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap.TDES
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet, IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase)t))
                {
                    var workingTest = test;
                    if (group.Direction.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                    {
                        list.Add(new TestCaseValidatorEncrypt(workingTest));
                    }
                    if (group.Direction.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                    {
                        list.Add(new TestCaseValidatorDecrypt(workingTest));
                    }
                }
            }

            return list;
        }
    }
}
