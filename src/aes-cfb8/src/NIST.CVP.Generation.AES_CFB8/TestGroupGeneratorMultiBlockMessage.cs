using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CFB8
{
    public class TestGroupGeneratorMultiBlockMessage : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string MMT_TYPE_LABEL = "MMT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            var algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);

            foreach (var function in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    var testGroup = new TestGroup
                    {
                        AlgoMode = algoMode,
                        Function = function,
                        KeyLength = keyLength,
                        InternalTestType = MMT_TYPE_LABEL
                    };
                    testGroups.Add(testGroup);
                }
            }
            return testGroups;
        }
    }
}
