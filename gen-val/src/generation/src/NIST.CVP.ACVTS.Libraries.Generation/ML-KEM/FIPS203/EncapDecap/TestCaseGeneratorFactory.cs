using System;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestCaseGeneratorFactory : ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>
{
    private readonly IOracle _oracle;
        
    public TestCaseGeneratorFactory(IOracle oracle)
    {
        _oracle = oracle;
    }
        
    public ITestCaseGeneratorAsync<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
    {
        return testGroup.TestType.ToUpper() switch
        {
            "AFT" => new TestCaseGeneratorEncapsulationAft(_oracle),
            "VAL" => new TestCaseGeneratorDecapsulationVal(_oracle),
            _ => throw new ArgumentException("Invalid test type")
        };
    }
}
