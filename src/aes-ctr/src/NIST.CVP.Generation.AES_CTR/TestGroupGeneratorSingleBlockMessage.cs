using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestGroupGeneratorSingleBlockMessage : ITestGroupGenerator<Parameters>
    {
        public const string LABEL = "singleblock";

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var direction in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    var testGroup = new TestGroup
                    {
                        Direction = direction,
                        KeyLength = keyLength,

                        TestType = LABEL
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
