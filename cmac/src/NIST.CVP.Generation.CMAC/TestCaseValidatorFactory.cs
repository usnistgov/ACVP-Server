using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CMAC
{
    public class TestCaseValidatorFactory<TTestVectorSet, TTestGroup, TTestCase> : ITestCaseValidatorFactory<TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : TestVectorSetBase<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>
    {
        public IEnumerable<ITestCaseValidator<TTestGroup, TTestCase>> GetValidators(TTestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TTestGroup, TTestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => g))
            {
                foreach (var test in group.Tests.Select(t => t))
                {
                    var workingTest = test;
                    if (group.Function.ToLower() == "gen")
                    {
                        list.Add(new TestCaseValidatorGen<TTestGroup, TTestCase>(workingTest));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorVer<TTestGroup, TTestCase>(workingTest));
                    }
                }
            }

            return list;
        }
    }
}
