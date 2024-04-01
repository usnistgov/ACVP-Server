using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.KeyGen;

public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
{
    public List<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
    {
        var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

        foreach (var group in testVectorSet.TestGroups.Select(g => g))
        {
            list.AddRange(group.Tests.Select(t => t).Select(test => new TestCaseValidator(test)));
        }

        return list;
    }
}
