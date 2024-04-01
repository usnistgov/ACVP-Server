using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;

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

        foreach (var group in testVectorSet.TestGroups)
        {
            switch (group.TestType.ToUpper())
            {
                case "AFT":
                    list.AddRange(group.Tests.Select(test => new TestCaseValidatorAft(test)));
                    break;
                
                case "GDT":
                    list.AddRange(group.Tests.Select(test => new TestCaseValidatorGdt(test, group, new DeferredTestCaseResolver(_oracle))));
                    break;
            }
        }

        return list;
    }
}
