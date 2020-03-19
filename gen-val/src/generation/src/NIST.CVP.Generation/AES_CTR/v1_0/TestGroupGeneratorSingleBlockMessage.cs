using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR.v1_0
{
    public class TestGroupGeneratorSingleBlockMessage : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string LABEL = "singleblock";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var direction in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    var testGroup = new TestGroup
                    {
                        Direction = direction,
                        KeyLength = keyLength,

                        InternalTestType = LABEL
                    };

                    testGroups.Add(testGroup);
                }
            }

            return Task.FromResult(testGroups.AsEnumerable());
        }
    }
}
