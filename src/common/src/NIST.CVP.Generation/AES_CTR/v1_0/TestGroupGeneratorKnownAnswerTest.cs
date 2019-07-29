using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR.v1_0
{
    public class TestGroupGeneratorKnownAnswerTest : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
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
                        var testGroup = new TestGroup
                        {
                            Direction = direction,
                            KeyLength = keyLength,
                            InternalTestType = katTest
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return testGroups;
        }
    }
}
