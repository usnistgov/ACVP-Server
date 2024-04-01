using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
{
    private readonly IOracle _oracle;

    public TestCaseValidatorFactory(IOracle oracle)
    {
        _oracle = oracle;
    }

    public List<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
    {
        var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

        foreach (var group in testVectorSet.TestGroups.Select(g => g))
        {
            foreach (var test in group.Tests.Select(t => t))
            {
                switch (group.TestType.ToUpper())
                {
                    case "VAL":
                        list.Add(new TestCaseValidatorVal(test));
                        break;
                    
                    case "AFT":
                        list.Add(new TestCaseValidatorAft(test, group, new DeferredAftTestCaseResolver(_oracle)));
                        break;
                }
            }
        }

        return list;
    }
}
