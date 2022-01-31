using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0
{
    public class TestGroupGeneratorPartialBlockMessage : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string LABEL = "partialblock";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
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

                            PayloadLength = parameters.PayloadLen.GetDeepCopy(),

                            InternalTestType = LABEL
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return Task.FromResult(testGroups);
        }
    }
}
