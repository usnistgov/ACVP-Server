using System;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigGen;

public class TestCaseGeneratorFactory : ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>
{
    private readonly IOracle _oracle;

    public TestCaseGeneratorFactory(IOracle oracle)
    {
        _oracle = oracle;
    }

    public ITestCaseGeneratorAsync<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
    {
        return testGroup.TestType switch
        {
            TestGroupGenerator.ALGORITHM_FUNCTIONAL_TEST => new TestCaseGeneratorAft(_oracle),
             _ => throw new Exception("Invalid test type")
        };
    }
}
