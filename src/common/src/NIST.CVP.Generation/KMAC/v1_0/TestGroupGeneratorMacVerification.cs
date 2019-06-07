using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.KMAC.v1_0
{
    public class TestGroupGeneratorMacVerification : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "MVT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digestSize in parameters.DigestSizes)
            {
                foreach (var xof in parameters.XOF)
                {
                    var testGroup = new TestGroup
                    {
                        DigestSize = digestSize,
                        TestType = TEST_TYPE,
                        KeyLengths = parameters.KeyLen.GetDeepCopy(),
                        MacLengths = parameters.MacLen.GetDeepCopy(),
                        MessageLength = digestSize,
                        HexCustomization = parameters.HexCustomization,
                        XOF = xof
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
