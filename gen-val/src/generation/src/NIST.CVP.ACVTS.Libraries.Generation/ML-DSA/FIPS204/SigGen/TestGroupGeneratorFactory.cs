using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;

public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
{
    private readonly IOracle _oracle;

    public TestGroupGeneratorFactory(IOracle oracle)
    {
        _oracle = oracle;
    }
    
    public IEnumerable<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
    {
        var list = new HashSet<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>>
        {
            new TestGroupGeneratorAft()
        };

        return list;
    }
}
