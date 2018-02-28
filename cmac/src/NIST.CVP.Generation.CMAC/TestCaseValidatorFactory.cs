using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CMAC
{
    public class TestCaseValidatorFactory<TTestVectorSet, TTestGroup, TTestCase> : ITestCaseValidatorFactory<TTestVectorSet, TTestCase>
        where TTestVectorSet : TestVectorSetBase<TTestGroup>
        where TTestGroup : TestGroupBase, new()
        where TTestCase : TestCaseBase, new()
    {
        public IEnumerable<ITestCaseValidator<TTestCase>> GetValidators(TTestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TTestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TTestGroup)g))
            {
                foreach (var test in group.Tests.Select(t => (TTestCase)t))
                {
                    var workingTest = test;
                    if (group.Function.ToLower() == "gen")
                    {
                        list.Add(new TestCaseValidatorGen<TTestCase>(workingTest));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorVer<TTestCase>(workingTest));
                    }
                }
            }

            return list;
        }
    }
}
