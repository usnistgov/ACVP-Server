using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA2
{
    public class MCTTestGroupFactory : IMonteCarloTestGroupFactory<Parameters, TestGroup>
    {
        public const string _MCT_TEST_TYPE_LABEL = "montecarlo";

        public IEnumerable<TestGroup> BuildMCTTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digSize in parameters.DigestSize)
            {
                var testGroup = new TestGroup
                {
                    Function = GetMode(digSize),
                    DigestSize = GetDigestSize(digSize),
                    TestType = _MCT_TEST_TYPE_LABEL,
                    BitOriented = false
                };
                testGroups.Add(testGroup);
            }

            return testGroups;
        }

        private ModeValues GetMode(string digSize)
        {
            if (digSize.Contains("160"))
            {
                return ModeValues.SHA1;
            }
            else
            {
                return ModeValues.SHA2;
            }
        }

        private DigestSizes GetDigestSize(string digSize)
        {
            if (digSize.Contains("512t256"))
            {
                return DigestSizes.d512t256;
            }
            else if (digSize.Contains("512t224"))
            {
                return DigestSizes.d512t224;
            }
            else if (digSize.Contains("512"))
            {
                return DigestSizes.d512;
            }
            else if (digSize.Contains("384"))
            {
                return DigestSizes.d384;
            }
            else if (digSize.Contains("256"))
            {
                return DigestSizes.d256;
            }
            else if (digSize.Contains("224"))
            {
                return DigestSizes.d224;
            }
            else
            {
                return DigestSizes.d160;
            }
        }
    }
}