using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KMAC
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const int _MESSAGE_LENGTH = 128;
        private const string TEST_TYPE = "aft";

        public int[] KeyLens { get; private set; }
        public int[] MacLens { get; private set; }

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digestSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    DigestSize = digestSize,
                    BitOrientedInput = parameters.BitOrientedInput,
                    BitOrientedOutput = parameters.BitOrientedOutput,
                    TestType = TEST_TYPE,
                    KeyLength = parameters.KeyLen.GetDomainMinMax().Minimum,    // temporary fix later
                    MacLength = parameters.MacLen.GetDomainMinMax().Minimum,    // temporary fix later
                    MessageLength = _MESSAGE_LENGTH,
                    XOF = false
                };

                testGroups.Add(testGroup);

                if (parameters.XOF)
                {
                    var testGroupXOF = new TestGroup
                    {
                        DigestSize = digestSize,
                        BitOrientedInput = parameters.BitOrientedInput,
                        BitOrientedOutput = parameters.BitOrientedOutput,
                        TestType = TEST_TYPE,
                        KeyLength = parameters.KeyLen.GetDomainMinMax().Minimum,    // temporary fix later
                        MacLength = parameters.MacLen.GetDomainMinMax().Minimum,    // temporary fix later
                        MessageLength = _MESSAGE_LENGTH,
                        XOF = true
                    };

                    testGroups.Add(testGroupXOF);
                }
            }

            return testGroups;
        }
    }
}
