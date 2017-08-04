using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseValidatorFactory<TTestVectorSet, TTestGroup, TTestCase> : ITestCaseValidatorFactory<TTestVectorSet, TTestCase>
        where TTestVectorSet : TestVectorSetBase<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestCase>, new()
        where TTestCase : TestCaseBase, new()
    {
        public IEnumerable<ITestCaseValidator<TTestCase>> GetValidators(TTestVectorSet testVectorSet, IEnumerable<TTestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TTestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TTestGroup)g))
            {
                foreach (var test in group.Tests.Select(t => (TTestCase)t))
                {
                    var workingTest = test;
                    if (group.Direction.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                    {
                        list.Add(new TestCaseValidatorEncrypt<TTestCase>(workingTest));
                    }
                    if (group.Direction.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                    {
                        list.Add(new TestCaseValidatorDecrypt<TTestCase>(workingTest));
                    }
                }
            }

            return list;
        }
    }
}
