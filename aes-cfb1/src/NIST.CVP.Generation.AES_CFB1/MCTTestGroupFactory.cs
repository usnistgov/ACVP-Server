using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CFB1
{
    public class MCTTestGroupFactory : IMCTTestGroupFactory<Parameters, IEnumerable<TestGroup>>
    {
        public const string _MCT_TEST_TYPE_LABEL = "MCT";

        public IEnumerable<TestGroup> BuildMCTTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var direction in parameters.Mode)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    var testGroup = new TestGroup()
                    {
                        Function = direction,
                        KeyLength = keyLength,
                        TestType = _MCT_TEST_TYPE_LABEL
                    };
                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
