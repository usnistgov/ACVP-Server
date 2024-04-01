using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;

public class TestGroupGeneratorGdt : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
{
    private readonly IOracle _oracle;

    public TestGroupGeneratorGdt(IOracle oracle)
    {
        _oracle = oracle;
    }
    
    public async Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
    {
        var testGroups = new List<TestGroup>();
        var map = new Dictionary<TestGroup, Task<MLDSAKeyPairResult>>();
        
        foreach (var parameterSet in parameters.ParameterSets.Distinct())
        {
            foreach (var deterministicOption in parameters.Deterministic.Distinct())
            {
                var testGroup = new TestGroup
                {
                    TestType = "GDT",
                    ParameterSet = parameterSet,
                    Deterministic = deterministicOption
                };

                var param = new MLDSAKeyGenParameters { ParameterSet = parameterSet };
                map.Add(testGroup, _oracle.GetMLDSAKeyCaseAsync(param));
            }
        }

        await Task.WhenAll(map.Values);
        foreach (var keyValuePair in map)
        {
            var group = keyValuePair.Key;
            var key = keyValuePair.Value.Result;
            group.PublicKey = key.PublicKey;
            group.PrivateKey = key.PrivateKey;
            
            testGroups.Add(group);
        }
        
        return testGroups;
    }
}
