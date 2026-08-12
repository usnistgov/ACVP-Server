using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.MLKEM;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.tr1.EncapDecap.TestCaseExpectations;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.tr1.EncapDecap;

public class TestGroupGeneratorKeyCheckVal : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
{
    public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
    {
        var testGroups = new List<TestGroup>();

        foreach (var parameterSet in parameters.ParameterSets.Distinct())
        {
            // Only test DecapsulationKeyCheck with an expanded key
            if (parameters.KeyFormats.Contains(PrivateKeyFormat.Expanded))
            {
                if (parameters.Functions.Contains(MLKEMFunction.DecapsulationKeyCheck))
                {
                    testGroups.Add(new TestGroup
                    {
                        TestType = "VAL",
                        Function = MLKEMFunction.DecapsulationKeyCheck,
                        ParameterSet = parameterSet,
                        KeyFormat = PrivateKeyFormat.Expanded,
                        DecapsulationKeyExpectationProvider = new DecapsulationKeyExpectationProvider()
                    });
                } 
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
