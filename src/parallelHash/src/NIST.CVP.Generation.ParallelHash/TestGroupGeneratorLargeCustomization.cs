using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.ParallelHash
{
    public class TestGroupGeneratorLargeCustomization : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "lct";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digestSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    Function = parameters.Algorithm,
                    DigestSize = digestSize,
                    IncludeNull = parameters.IncludeNull,
                    BitOrientedInput = parameters.BitOrientedInput,
                    BitOrientedOutput = parameters.BitOrientedOutput,
                    TestType = TEST_TYPE,
                    XOF = false
                };

                testGroups.Add(testGroup);

                if (parameters.XOF)
                {
                    var testGroupXOF = new TestGroup
                    {
                        Function = parameters.Algorithm,
                        DigestSize = digestSize,
                        IncludeNull = parameters.IncludeNull,
                        BitOrientedInput = parameters.BitOrientedInput,
                        BitOrientedOutput = parameters.BitOrientedOutput,
                        TestType = TEST_TYPE,
                        XOF = true
                    };

                    testGroups.Add(testGroupXOF);
                }
            }

            return testGroups;
        }
    }
}
