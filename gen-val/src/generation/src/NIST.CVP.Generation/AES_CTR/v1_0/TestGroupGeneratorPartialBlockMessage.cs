using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR.v1_0
{
    public class TestGroupGeneratorPartialBlockMessage : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string LABEL = "partialblock";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            if (parameters.PayloadLen.ContainsValueOtherThan(128))
            {
                foreach (var direction in parameters.Direction)
                {
                    foreach (var keyLength in parameters.KeyLen)
                    {
                        var testGroup = new TestGroup
                        {
                            Direction = direction,
                            KeyLength = keyLength,

                            // Only test case generator that cares about this information
                            PayloadLength = parameters.PayloadLen,

                            InternalTestType = LABEL
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }
            
            return Task.FromResult(testGroups.AsEnumerable());
        }
    }
}
