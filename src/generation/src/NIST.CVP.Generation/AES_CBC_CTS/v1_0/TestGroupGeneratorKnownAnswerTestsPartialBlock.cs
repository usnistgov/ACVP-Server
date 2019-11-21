using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CBC_CTS.v1_0
{
    public class TestGroupGeneratorKnownAnswerTestsPartialBlock : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";
        private readonly string[] _katTests =
        {
            "GFSBox",
            "KeySBox",
            "VarTxt",
            "VarKey"
        };

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var direction in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    foreach (var katTest in _katTests)
                    {
                        var testGroup = new TestGroup()
                        {
                            Function = direction,
                            KeyLength = keyLength,
                            TestType = TEST_TYPE,
                            InternalTestType = katTest,
                            IsPartialBlockGroup = true,
                            PayloadLen = parameters.PayloadLen.GetDeepCopy(),
                            AlgoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision)
                        };
                        testGroups.Add(testGroup);
                    }
                }
            }

            return testGroups;
        }
    }
}
