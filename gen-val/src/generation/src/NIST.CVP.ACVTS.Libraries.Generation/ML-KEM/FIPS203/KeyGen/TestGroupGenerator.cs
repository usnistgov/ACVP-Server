using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.KeyGen;

public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
{
    public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
    {
        var testGroups = new List<TestGroup>();
        
        foreach (var parameterSet in parameters.ParameterSets.Distinct())
        {
            testGroups.Add(new TestGroup
            {
                TestType = "AFT",
                ParameterSet = parameterSet
            });
        }

        return Task.FromResult(testGroups);
    }
}
