using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_FFX.v1_0.Base
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string AFT_TYPE_LABEL = "AFT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
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
                            TweakLen = parameters.TweakLen.GetDeepCopy(),
                            TestType = AFT_TYPE_LABEL
                        };
                        testGroups.Add(testGroup);
                    }
                }
            }
            return Task.FromResult(testGroups);
        }
    }
}
