using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CBC_CTS.v1_0
{
    public class TestGroupGeneratorMultiBlockMessageFullBlock : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string MMT_TYPE_LABEL = "AFT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var function in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    var testGroup = new TestGroup
                    {
                        Function = function,
                        KeyLength = keyLength,
                        TestType = MMT_TYPE_LABEL,
                        InternalTestType = "MMT",
                        IsPartialBlockGroup = false,
                        PayloadLen = parameters.PayloadLen.GetDeepCopy(),
                        AlgoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision)
                    };
                    testGroups.Add(testGroup);
                }
            }
            return testGroups;
        }
    }
}
