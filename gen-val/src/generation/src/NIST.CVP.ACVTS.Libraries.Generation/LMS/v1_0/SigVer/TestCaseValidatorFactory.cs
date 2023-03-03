using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigVer;

public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
{
    public List<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
    {
        var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

        foreach (var group in testVectorSet.TestGroups.Select(g => g))
        {
            foreach (var test in group.Tests.Select(t => t))
            {
                list.Add(new TestCaseValidatorAft(test));
            }
        }

        return list;
    }
}
