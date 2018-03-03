using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class TestGroupGeneratorMonteCarloTest : ITestGroupGenerator<Parameters>
    {
        public const string TEST_TYPE = "MCT";

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    Function = SHAEnumHelpers.StringToMode(parameters.Algorithm),
                    DigestSize = SHAEnumHelpers.StringToDigest(digSize),
                    TestType = TEST_TYPE
                };

                testGroups.Add(testGroup);
            }
            
            return testGroups;
        }
    }
}

