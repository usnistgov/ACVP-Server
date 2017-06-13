using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB
{
    public class TestGroupGeneratorKnownAnswerTests : ITestGroupGenerator<Parameters>
    {
        private readonly string[] _katTests = new string[]
        {
            "GFSBox",
            "KeySBox",
            "VarTxt",
            "VarKey"
        };

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var direction in parameters.Mode)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    foreach (var katTest in _katTests)
                    {
                        var testGroup = new TestGroup()
                        {
                            Function = direction,
                            KeyLength = keyLength,
                            TestType = katTest,
                            StaticGroupOfTests = true
                        };
                        testGroups.Add(testGroup);
                    }
                }
            }

            return testGroups;
        }
    }
}
