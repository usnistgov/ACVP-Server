using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseValidatorFactoryTdes : ITestCaseValidatorFactory<TestVectorSetTdes, TestCaseTdes>
    {
        public IEnumerable<ITestCaseValidator<TestCaseTdes>> GetValidators(TestVectorSetTdes testVectorSet, IEnumerable<TestCaseTdes> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCaseTdes>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroupTdes)g))
            {
                foreach (var test in group.Tests.Select(t => (TestCaseTdes)t))
                {
                    var workingTest = test;
                    if (group.Direction.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                    {
                        list.Add(new TestCaseValidatorEncryptTdes(workingTest));
                    }
                    if (group.Direction.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                    {
                        list.Add(new TestCaseValidatorDecryptTdes(workingTest));
                    }
                }
            }

            return list;
        }
    }
}
