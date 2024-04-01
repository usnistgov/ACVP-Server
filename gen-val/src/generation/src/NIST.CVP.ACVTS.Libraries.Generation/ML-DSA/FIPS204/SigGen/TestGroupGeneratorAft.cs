using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;

public class TestGroupGeneratorAft : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
{
    public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
    {
        var testGroups = new List<TestGroup>();
        
        foreach (var parameterSet in parameters.ParameterSets.Distinct())
        {
            foreach (var deterministicOption in parameters.Deterministic.Distinct())
            {
                var testGroup = new TestGroup
                {
                    TestType = "AFT",
                    ParameterSet = parameterSet,
                    Deterministic = deterministicOption
                };

                testGroups.Add(testGroup);
            }
        }
        
        return Task.FromResult(testGroups);
    }
}
