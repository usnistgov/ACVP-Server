using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128;

public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
{
    public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
    {
        var testGroups = new List<TestGroup>();
        
        foreach (var direction in parameters.Directions.Distinct())
        {
            foreach (var mask in parameters.SupportsNonceMasking.Distinct())
            {
                TestGroup tg = new TestGroup();
                tg.TestType = "AFT";
                tg.Direction = direction;
                tg.PlaintextLength = parameters.PayloadLength;
                tg.ADLength = parameters.ADLength;
                tg.TruncationLength = parameters.TagLength;
                tg.NonceMasking = mask;
                testGroups.Add(tg);
            }
            
        }

        return Task.FromResult(testGroups);
    }
}
