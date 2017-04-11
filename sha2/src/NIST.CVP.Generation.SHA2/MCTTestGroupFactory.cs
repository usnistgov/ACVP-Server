using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.SHA2;

namespace NIST.CVP.Generation.SHA2
{
    public class MCTTestGroupFactory : IMonteCarloTestGroupFactory<Parameters, TestGroup>
    {
        public const string _MCT_TEST_TYPE_LABEL = "mct";

        public IEnumerable<TestGroup> BuildMCTTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digSize in parameters.DigestSize)
            {
                var testGroup = new TestGroup
                {
                    Function = SHAEnumHelpers.DigestSizeToMode(digSize),
                    DigestSize =  SHAEnumHelpers.StringToDigest(digSize),
                    TestType = _MCT_TEST_TYPE_LABEL,
                    BitOriented = false
                };
                testGroups.Add(testGroup);
            }

            return testGroups;
        }
    }
}