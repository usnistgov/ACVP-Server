using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA3.v1_0
{
    public class TestGroupGeneratorMonteCarlo : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "MCT";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digSize in parameters.DigestSizes)
            {
                // Must make sure we actually have a domain. SHAKE will, SHA3 will not.
                MathDomain domain = null;
                if (parameters.OutputLength != null)
                {
                    domain = parameters.OutputLength.GetDeepCopy();
                }

                var testGroup = new TestGroup
                {
                    Function = parameters.Algorithm.ToLower().Contains("shake") ? "SHAKE" : "SHA3",
                    DigestSize = digSize,
                    BitOrientedInput = parameters.BitOrientedInput,
                    BitOrientedOutput = parameters.BitOrientedOutput,
                    OutputLength = domain,
                    TestType = TEST_TYPE
                };

                testGroups.Add(testGroup);
            }

            return Task.FromResult(testGroups.AsEnumerable());
        }
    }
}
