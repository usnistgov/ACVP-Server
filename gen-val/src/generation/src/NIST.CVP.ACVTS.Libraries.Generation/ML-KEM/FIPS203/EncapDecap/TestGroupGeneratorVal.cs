using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_KEM;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestGroupGeneratorVal : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
{
    private readonly IOracle _oracle;

    public TestGroupGeneratorVal(IOracle oracle)
    {
        _oracle = oracle;
    }
    
    public async Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
    {
        var testGroups = new List<TestGroup>();
        var map = new Dictionary<TestGroup, Task<MLKEMKeyPairResult>>();
        
        foreach (var parameterSet in parameters.ParameterSets.Distinct())
        {
            if (parameters.Functions.Contains(KyberFunction.Decapsulation))
            {
                var testGroup = new TestGroup
                {
                    TestType = "VAL",
                    Function = KyberFunction.Decapsulation,
                    ParameterSet = parameterSet,
                    TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample)
                };
            
                var param = new MLKEMKeyGenParameters { ParameterSet = parameterSet };
                map.Add(testGroup, _oracle.GetMLKEMKeyCaseAsync(param));
            }
        }

        await Task.WhenAll(map.Values);
        foreach (var keyValuePair in map)
        {
            var group = keyValuePair.Key;
            var key = keyValuePair.Value.Result;
            group.EncapsulationKey = key.EncapsulationKey;
            group.DecapsulationKey = key.DecapsulationKey;
            
            testGroups.Add(group);
        }
        
        return testGroups;
    }
}
