using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SHAWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.HMAC
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        private const int _MESSAGE_LENGTH = 128;
        private const string _TEST_TYPE = "AFT";
        private const int _MAX_MAC_LENGTHS_TO_TEST = 4;

        public int[] KeyLens { get; private set; }
        public int[] MacLens { get; private set; }

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();

            return CreateGroups(parameters, testGroups);
        }

        private IEnumerable<ITestGroup> CreateGroups(Parameters parameters, List<ITestGroup> testGroups)
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
            List<int> keyLengths = new List<int>();
            
            var keyLengthMinMax = parameters.KeyLen.GetDomainMinMax();
            parameters.KeyLen.SetRangeOptions(RangeDomainSegmentOptions.Random);

            // key < block
            keyLengths.AddRangeIfNotNullOrEmpty(parameters.KeyLen.GetValues(keyLengthMinMax.Minimum, blockSize - 1, 2));

            // key = block
            keyLengths.AddRangeIfNotNullOrEmpty(parameters.KeyLen.GetValues(blockSize, blockSize, 1));

            // key > block
            keyLengths.AddRangeIfNotNullOrEmpty(parameters.KeyLen.GetValues(blockSize + 1, blockSize + 1024, 2));

            KeyLens = keyLengths.ToArray();
        }

        private void GetMacLengths(Parameters parameters)
        {
            parameters.MacLen.SetRangeOptions(RangeDomainSegmentOptions.Random);

            var macMinMax = parameters.MacLen.GetDomainMinMaxAsEnumerable();

            var randomMacLengths = parameters.MacLen.GetValues(4).Where(w => !macMinMax.Contains(w)).Take(2);

            List<int> macLens = new List<int>();
            macLens.AddRangeIfNotNullOrEmpty(macMinMax);
            macLens.AddRangeIfNotNullOrEmpty(randomMacLengths);

            MacLens = macLens.OrderBy(ob => ob).ToArray();
        }
    }
}