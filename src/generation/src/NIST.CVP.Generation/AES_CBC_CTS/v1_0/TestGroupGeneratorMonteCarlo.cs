using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CBC_CTS.v1_0
{
    public class TestGroupGeneratorMonteCarlo : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string _MCT_TEST_TYPE_LABEL = "MCT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var direction in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    var testGroup = new TestGroup()
                    {
                        Function = direction,
                        KeyLength = keyLength,
                        TestType = _MCT_TEST_TYPE_LABEL,
                        IsPartialBlockGroup = true,
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
