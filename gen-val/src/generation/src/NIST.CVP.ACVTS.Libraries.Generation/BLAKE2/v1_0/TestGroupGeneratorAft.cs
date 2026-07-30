using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.BLAKE2.v1_0
{
    public class TestGroupGeneratorAft : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TestType = "AFT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            testGroups.Add(new TestGroup
            {
                TestType = TestType,
                DigestLength = parameters.DigestLength.GetDeepCopy(),
                MessageLength = parameters.MessageLength.GetDeepCopy(),
                KeyLength = parameters.KeyLength?.GetDeepCopy()
            });

            return Task.FromResult(testGroups);
        }
    }
}
