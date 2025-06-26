using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128;

public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
{
    public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
    {
        var testGroups = new List<TestGroup>();
        
        foreach (var direction in parameters.Direction.Distinct())
        {
            foreach (var mask in parameters.SupportsNonceMasking.Distinct())
            {
                TestGroup tg = new TestGroup
                {
                    TestType = "AFT", 
                    Direction = direction, 
                    PlaintextLength = parameters.PayloadLen, 
                    ADLength = parameters.AadLen,
                    TruncationLength = parameters.TagLen,
                    NonceMasking = mask,
                    TestCaseExpectationProvider = new AEADExpectationProvider()
                };

                testGroups.Add(tg);
            }
        }

        return Task.FromResult(testGroups);
    }
}
