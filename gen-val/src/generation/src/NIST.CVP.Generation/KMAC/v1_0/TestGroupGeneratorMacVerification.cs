using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.KMAC.v1_0
{
    public class TestGroupGeneratorMacVerification : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "MVT";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digestSize in parameters.DigestSizes)
            {
                foreach (var xof in parameters.XOF)
                {
                    var testGroup = new TestGroup
                    {
                        DigestSize = digestSize,
                        TestType = TEST_TYPE,
                        KeyLengths = parameters.KeyLen.GetDeepCopy(),
                        MacLengths = parameters.MacLen.GetDeepCopy(),
                        MsgLengths = parameters.MsgLen.GetDeepCopy(),
                        HexCustomization = parameters.HexCustomization,
                        XOF = xof
                    };

                    testGroups.Add(testGroup);
                }
            }

            return Task.FromResult(testGroups.AsEnumerable());
        }
    }
}
