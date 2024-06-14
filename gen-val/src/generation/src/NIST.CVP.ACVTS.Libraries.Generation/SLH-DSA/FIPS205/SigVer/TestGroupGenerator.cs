using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigVer.TestCaseExpectations;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigVer;

public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
{
    public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
    {
        var testGroups = new List<TestGroup>();

        foreach (var capability in parameters.Capabilities)
        {
            // each capability has an array of parameter sets
            foreach (var parameterSet in capability.ParameterSets.Distinct())
            {
                testGroups.Add( new TestGroup
                {
                    TestType = "AFT",
                    ParameterSet = parameterSet,
                    TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample),
                    MessageLengths = capability.MessageLength.GetDeepCopy()
                });    
            }
        }            

        return Task.FromResult(testGroups);
    }   
}
