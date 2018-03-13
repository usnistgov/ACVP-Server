using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class TestGroupGeneratorAlgorithmFunctionalTest : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "AFT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            foreach (var digestSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    Function = SHAEnumHelpers.StringToMode(parameters.Algorithm),
                    DigestSize = SHAEnumHelpers.StringToDigest(digestSize),
                    TestType = TEST_TYPE,
                    IncludeNull = parameters.IncludeNull,
                    BitOriented = parameters.BitOriented
                };
                testGroups.Add(testGroup);
            }

            return testGroups;
        }
    }
}
