using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.Blake2;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.BLAKE2.v1_0
{
    public class TestGroupGeneratorAft : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TestType = "AFT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digestLength in parameters.DigestLengths)
            {
                var hashFunction = new Blake2HashFunction(Blake2Variant.Blake2b, digestLength);
                testGroups.Add(new TestGroup
                {
                    TestType = TestType,
                    DigestLength = digestLength,
                    MessageLength = parameters.MessageLength.GetDeepCopy(),
                    KeyLength = parameters.KeyLength?.GetDeepCopy(),
                    HashFunction = hashFunction
                });
            }

            return Task.FromResult(testGroups);
        }
    }
}
