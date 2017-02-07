using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.SHA;
using NIST.CVP.Math;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA2
{
    public class TestCaseGeneratorMonteCarloHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int NUMBER_OF_ITERATIONS = 10000;
        private const int NUMBER_OF_DIGESTS_TO_SAVE = 3;
        private const int NUMBER_OF_CASES = 100;
        private const int NUMBER_OF_SAMPLE_CASES = 3;

        private readonly IRandom800_90 _random800_90;
        private readonly ISHA _algo;

        private int _testCaseId;
        private TestCase _previousCase = null;
        public bool IsSample { get; } = false;
        private readonly List<BitString> _lastDigests = new List<BitString>();
        private TestCase _seedCaseForTest = null;

        public TestCaseGeneratorMonteCarloHash(IRandom800_90 random800_90, ISHA algo, bool isSample)
        {
            _random800_90 = random800_90;
            _algo = algo;
            IsSample = isSample;
        }
        
        public TestCaseGeneratorMonteCarloHash(IRandom800_90 random800_90, ISHA algo, TestCase seedCase)
        {
            _random800_90 = random800_90;
            _algo = algo;
            _seedCaseForTest = seedCase;
        }

        public int NumberOfTestCasesToGenerate
        {
            get
            {
                if (IsSample)
                {
                    return NUMBER_OF_SAMPLE_CASES;
                }

                return NUMBER_OF_CASES;
            }
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var seedCase = GetSeedCase(group);

            var result = Generate(group, seedCase);
            if (result.Success)
            {
                _previousCase = (TestCase)result.TestCase;
            }

            return result;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase seedCase)
        {
            TestCase tempTestCase = seedCase;
            HashFunction hashFunction = new HashFunction
            {
                Mode = group.Function,
                DigestSize = group.DigestSize
            };

            try
            {
                HashResult hashResult = null;
                for(var i = 0; i < NUMBER_OF_ITERATIONS; i++)
                {
                    hashResult = _algo.HashMessage(hashFunction, tempTestCase.Message);

                    if (!hashResult.Success)
                    {
                        ThisLogger.Warn(hashResult.ErrorMessage);
                        {
                            return new TestCaseGenerateResponse(hashResult.ErrorMessage);
                        }
                    }

                    SaveDigestForMessageMixing(hashResult.Digest);
                    tempTestCase.Message = MixMessages(hashResult.Digest);
                }

                var testCase = new TestCase { Message = seedCase.Message, Digest = hashResult.Digest, TestCaseId = _testCaseId };
                return new TestCaseGenerateResponse(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse(ex.Message);
                }
            }
        }

        private TestCase GetSeedCase(TestGroup group)
        {
            if(_previousCase == null)
            {
                return GetInitialSeedCase(group);
            }

            var newMessage = MixMessages(_previousCase.Digest);
            return new TestCase { Message = newMessage };
        }

        private BitString MixMessages(BitString message)
        {
            // If we don't have enough digests, just keep adding the given message until we do
            // Logically this only happens once
            while(_lastDigests.Count < 3)
            {
                SaveDigestForMessageMixing(message);
            }

            return BitString.ConcatenateBits(_lastDigests[0], BitString.ConcatenateBits(_lastDigests[1], _lastDigests[2]));
        }

        private TestCase GetInitialSeedCase(TestGroup group)
        {
            if(_seedCaseForTest != null)
            {
                return _seedCaseForTest;
            }

            var digestSize = 0;
            switch (group.DigestSize)
            {
                case DigestSizes.d160:
                    digestSize = 160;
                    break;
                case DigestSizes.d224:
                case DigestSizes.d512t224:
                    digestSize = 224;
                    break;
                case DigestSizes.d256:
                case DigestSizes.d512t256:
                    digestSize = 256;
                    break;
                case DigestSizes.d384:
                    digestSize = 384;
                    break;
                case DigestSizes.d512:
                    digestSize = 512;
                    break;
            }

            var seed = _random800_90.GetRandomBitString(digestSize);
            var message = MixMessages(seed);

            return new TestCase { Message = message };
        }

        private void SaveDigestForMessageMixing(BitString digest)
        {
            _lastDigests.Insert(0, digest);
            if(_lastDigests.Count > NUMBER_OF_DIGESTS_TO_SAVE)
            {
                _lastDigests.RemoveRange(NUMBER_OF_DIGESTS_TO_SAVE, _lastDigests.Count - NUMBER_OF_DIGESTS_TO_SAVE);
            }
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
