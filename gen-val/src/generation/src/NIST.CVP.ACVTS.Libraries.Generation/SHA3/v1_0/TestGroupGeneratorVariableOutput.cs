using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0
{
    public class TestGroupGeneratorVariableOutput : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "VOT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            var function = ShaAttributes.GetHashFunctionFromName(parameters.Algorithm);

            // VOT tests are only valid for shake
            if (!parameters.Algorithm.ToLower().Contains("shake"))
            {
                return Task.FromResult(testGroups);
            }

            foreach (var digestSize in parameters.DigestSizes)        // Only ever a single digest size
            {
                var testGroup = new TestGroup
                {
                    Function = function.Mode,
                    DigestSize = function.DigestSize,
                    IncludeNull = parameters.IncludeNull,
                    BitOrientedInput = parameters.BitOrientedInput,
                    BitOrientedOutput = parameters.BitOrientedOutput,
                    OutputLength = parameters.OutputLength.GetDeepCopy(),
                    TestType = TEST_TYPE
                };

                testGroups.Add(testGroup);
            }

            return Task.FromResult(testGroups);
        }
    }
}
