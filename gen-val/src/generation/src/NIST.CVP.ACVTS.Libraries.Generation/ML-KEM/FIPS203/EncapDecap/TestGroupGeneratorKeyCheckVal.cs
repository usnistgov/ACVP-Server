using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.MLKEM;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap.TestCaseExpectations;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestGroupGeneratorKeyCheckVal : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
{
    public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
    {
        var testGroups = new List<TestGroup>();

        foreach (var parameterSet in parameters.ParameterSets.Distinct())
        {
            if (parameters.Functions.Contains(MLKEMFunction.DecapsulationKeyCheck))
            {
                testGroups.Add(new TestGroup
                {
                    TestType = "VAL",
                    Function = MLKEMFunction.DecapsulationKeyCheck,
                    ParameterSet = parameterSet,
                    DecapsulationKeyExpectationProvider = new DecapsulationKeyExpectationProvider()
                });
            }

            if (parameters.Functions.Contains(MLKEMFunction.EncapsulationKeyCheck))
            {
                testGroups.Add(new TestGroup
                {
                    TestType = "VAL",
                    Function = MLKEMFunction.EncapsulationKeyCheck,
                    ParameterSet = parameterSet,
                    EncapsulationKeyExpectationProvider = new EncapsulationKeyExpectationProvider()
                });
            }
        }
        
        return Task.FromResult(testGroups);
    }
}
