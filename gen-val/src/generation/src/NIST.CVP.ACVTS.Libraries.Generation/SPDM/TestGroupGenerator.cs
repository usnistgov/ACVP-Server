using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SPDM;

public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
{
    public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
    {
        var testGroups = new List<TestGroup>();

        foreach (var version in parameters.SPDMVersion.Distinct())
        {
            foreach (var psk in parameters.UsesPSK.Distinct())
            {
                foreach (var mode in parameters.HashAlgs.Distinct())
                {
                    TestGroup tg = new TestGroup
                    {
                        TestType = "AFT",
                        KeyLength = parameters.KeyLen,
                        THLength = parameters.THLen,
                        HashFunction = mode,
                        Version = version,
                        UsesPSK = psk,
                    };

                    testGroups.Add(tg);
                }
            }
        }        

        return Task.FromResult(testGroups);
    }
}
