using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CFB1
{
    public class KATTestGroupFactory : IKATTestGroupFactory<Parameters, IEnumerable<TestGroup>>
    {
        private readonly string[] _katTests = new string[]
        {
            "GFSBox",
            "KeySBox",
            "VarTxt",
            "VarKey"
        };

        public IEnumerable<TestGroup> BuildKATTestGroups(Parameters parameters)
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
