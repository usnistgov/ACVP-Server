using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES_FFX.v1_0.Base
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string AFT_TYPE_LABEL = "AFT";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            var algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);

            foreach (var function in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    foreach (var capability in parameters.Capabilities)
                    {
                        var testGroup = new TestGroup
                        {
                            AlgoMode = algoMode,
                            Function = function.ToLower() == "encrypt" ? BlockCipherDirections.Encrypt : BlockCipherDirections.Decrypt,
                            KeyLength = keyLength,
                            Capability = capability,
                            TweakLen = parameters.TweakLen,
                            TestType = AFT_TYPE_LABEL
                        };
                        testGroups.Add(testGroup);
                    }
                }
            }
            return Task.FromResult(testGroups.AsEnumerable());
        }
    }
}
