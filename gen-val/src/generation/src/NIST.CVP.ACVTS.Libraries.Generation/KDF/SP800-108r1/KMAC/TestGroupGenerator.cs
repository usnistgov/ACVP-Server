using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF.SP800_108r1.KMAC
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var macMode in parameters.MacMode)
            {
                var testGroup = new TestGroup
                {
                    MacMode = macMode,
                    KeyDerivationKeyLength = parameters.KeyDerivationKeyLength,
                    ContextLength = parameters.ContextLength,
                    LabelLength = parameters.LabelLength,
                    DerivedKeyLength = parameters.DerivedKeyLength,
                    TestType = "AFT"
                };
                
                testGroups.Add(testGroup);
            }

            return Task.FromResult(testGroups);
        }
    }
}
