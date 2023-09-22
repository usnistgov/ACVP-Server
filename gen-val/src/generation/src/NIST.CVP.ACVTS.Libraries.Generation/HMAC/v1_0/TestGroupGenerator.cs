using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.HMAC.v1_0
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const int _MESSAGE_LENGTH = 128;
        private const string _TEST_TYPE = "AFT";

        public ShuffleQueue<int> KeyLens { get; private set; }
        public ShuffleQueue<int> MacLens { get; private set; }

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            return Task.FromResult(CreateGroups(parameters, testGroups));
        }

        private List<TestGroup> CreateGroups(Parameters parameters, List<TestGroup> testGroups)
        {
            var result = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(parameters.Algorithm);

            DetermineLengths(parameters, result.shaMode, result.shaDigestSize);

            var maxLen = new[] { KeyLens.OriginalListCount, MacLens.OriginalListCount }.Max();

            for (var i = 0; i < maxLen; i++)
            {
                testGroups.Add(new TestGroup()
                {
                    ShaMode = result.shaMode,
                    TestType = _TEST_TYPE,
                    ShaDigestSize = result.shaDigestSize,
                    KeyLength = KeyLens.Pop(),
                    MessageLength = _MESSAGE_LENGTH,
                    MacLength = MacLens.Pop()
                });
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
            
            keyLengths.AddRangeIfNotNullOrEmpty(parameters.KeyLen.GetDomainMinMaxAsEnumerable());
            keyLengths.AddRangeIfNotNullOrEmpty(parameters.KeyLen.GetRandomValues(x => x < blockSize, 5));
            keyLengths.AddRangeIfNotNullOrEmpty(parameters.KeyLen.GetSequentialValues(x => x == blockSize, 1));
            keyLengths.AddRangeIfNotNullOrEmpty(parameters.KeyLen.GetRandomValues(x => x > blockSize, 5));

            KeyLens = new ShuffleQueue<int>(keyLengths);
        }

        private void GetMacLengths(Parameters parameters)
        {
            var macMinMax = parameters.MacLen.GetDomainMinMaxAsEnumerable();

            var randomMacLengths = parameters.MacLen.GetSequentialValues(x => !macMinMax.Contains(x), 2);

            var macLens = new List<int>();
            macLens.AddRangeIfNotNullOrEmpty(macMinMax);
            macLens.AddRangeIfNotNullOrEmpty(randomMacLengths);

            MacLens = new ShuffleQueue<int>(macLens);
        }
    }
}
