using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.HMAC.v1_0
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const int _MESSAGE_LENGTH = 128;
        private const string _TEST_TYPE = "AFT";

        public int[] KeyLens { get; private set; }
        public int[] MacLens { get; private set; }

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            return CreateGroups(parameters, testGroups);
        }

        private IEnumerable<TestGroup> CreateGroups(Parameters parameters, List<TestGroup> testGroups)
        {
            var result = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(parameters.Algorithm);
            
            DetermineLengths(parameters, result.shaMode, result.shaDigestSize);

            foreach (var keyLen in KeyLens)
            {
                foreach (var macLen in MacLens)
                {
                    TestGroup tg = new TestGroup()
                    {
                        ShaMode = result.shaMode,
                        TestType = _TEST_TYPE,
                        ShaDigestSize = result.shaDigestSize,
                        KeyLength = keyLen,
                        MessageLength = _MESSAGE_LENGTH,
                        MacLength = macLen
                    };

                    testGroups.Add(tg);
                }
            }

            return testGroups;
        }

        private void DetermineLengths(Parameters parameters, ModeValues shaMode, DigestSizes shaDigestSize)
        {
            var blockSize = ShaAttributes.GetShaAttributes(shaMode, shaDigestSize).blockSize;

            GetKeyLengths(parameters, blockSize);
            GetMacLengths(parameters);
        }

        private void GetKeyLengths(Parameters parameters, int blockSize)
        {
            // Differing algo logic depending on keyLen < blockSize, keyLen = blockSize, keyLen > blockSize
            // Ensure we grab values from each section of logic (where applicable)
            var keyLengths = new List<int>();
            
            parameters.KeyLen.SetRangeOptions(RangeDomainSegmentOptions.Random);

            keyLengths.AddRangeIfNotNullOrEmpty(parameters.KeyLen.GetValues(x => x < blockSize, 2, false));
            keyLengths.AddRangeIfNotNullOrEmpty(parameters.KeyLen.GetValues(x => x == blockSize, 1, false));
            keyLengths.AddRangeIfNotNullOrEmpty(parameters.KeyLen.GetValues(x => x > blockSize, 2, false));

            KeyLens = keyLengths.ToArray();
        }

        private void GetMacLengths(Parameters parameters)
        {
            parameters.MacLen.SetRangeOptions(RangeDomainSegmentOptions.Random);

            var macMinMax = parameters.MacLen.GetDomainMinMaxAsEnumerable();

            var randomMacLengths = parameters.MacLen.GetValues(x => !macMinMax.Contains(x), 2, false);

            var macLens = new List<int>();
            macLens.AddRangeIfNotNullOrEmpty(macMinMax);
            macLens.AddRangeIfNotNullOrEmpty(randomMacLengths);

            MacLens = macLens.OrderBy(ob => ob).ToArray();
        }
    }
}