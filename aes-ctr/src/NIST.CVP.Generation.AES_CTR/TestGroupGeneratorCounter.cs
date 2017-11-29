using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestGroupGeneratorCounter : ITestGroupGenerator<Parameters>
    {
        public const string LABEL = "counter";
        public const string OVERFLOW_LABEL = "overflowcounter";

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var direction in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    if (parameters.OverflowCounter)
                    {
                        var overflowGroup = new TestGroup
                        {
                            Direction = direction,
                            KeyLength = keyLength,
                            IncrementalCounter = parameters.IncrementalCounter,

                            StaticGroupOfTests = false,
                            TestType = OVERFLOW_LABEL
                        };

                        testGroups.Add(overflowGroup);
                    }

                    var testGroup = new TestGroup
                    {
                        Direction = direction,
                        KeyLength = keyLength,
                        IncrementalCounter = parameters.IncrementalCounter,

                        StaticGroupOfTests = false,
                        TestType = LABEL
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
