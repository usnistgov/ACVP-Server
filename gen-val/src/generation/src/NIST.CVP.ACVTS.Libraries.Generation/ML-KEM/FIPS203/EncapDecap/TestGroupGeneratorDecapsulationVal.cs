using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_KEM;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestGroupGeneratorDecapsulationVal : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
{
public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
    {
        var testGroups = new List<TestGroup>();
        
        foreach (var parameterSet in parameters.ParameterSets.Distinct())
        {
            if (parameters.Functions.Contains(KyberFunction.Decapsulation))
            {
                var testGroup = new TestGroup
                {
                    TestType = "VAL",
                    Function = KyberFunction.Decapsulation,
                    ParameterSet = parameterSet,
                    DecapsulationExpectationProvider = new DecapsulationExpectationProvider()
                };

                testGroups.Add(testGroup);
            }
        }

        return Task.FromResult(testGroups);
    }
}
