using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestGroupGeneratorAft : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
{
    public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
    {
        var testGroups = new List<TestGroup>();
        
        foreach (var parameterSet in parameters.ParameterSets.Distinct())
        {
            if (parameters.Functions.Contains(KyberFunction.Encapsulation))
            {
                testGroups.Add(new TestGroup
                {
                    TestType = "AFT",
                    Function = KyberFunction.Encapsulation,
                    ParameterSet = parameterSet
                });    
            }
        }

        return Task.FromResult(testGroups);
    }
}
