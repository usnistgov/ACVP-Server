using System;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.SHA2
{
    public class TestCaseGeneratorMCTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISHA_MCT _algo;

        public bool IsSample { get; set; } = false;
        private TestCase _seedCaseForTest = null;

        public int NumberOfTestCasesToGenerate { get { return 1; } }

        public TestCaseGeneratorMCTHash(IRandom800_90 random800_90, ISHA_MCT algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }
        
        public TestCaseGeneratorMCTHash(IRandom800_90 random800_90, ISHA_MCT algo, TestCase seedCase)
        {
            _random800_90 = random800_90;
            _algo = algo;
            _seedCaseForTest = seedCase;
        }
        
        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            IsSample = isSample;
            var seedCase = GetSeedCase(group);
            return Generate(group, seedCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            var hashFunction = new HashFunction
            {
                Mode = group.Function,
                DigestSize = group.DigestSize
            };

            MCTResult<AlgoArrayResponse> hashResult = null;
            try
            {
                hashResult = _algo.MCTHash(hashFunction, testCase.Message, IsSample);
                if (!hashResult.Success)
                {
                    ThisLogger.Warn(hashResult.ErrorMessage);
                    return new TestCaseGenerateResponse(hashResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse(ex.Message);
            }

            testCase.ResultsArray = hashResult.Response;
            return new TestCaseGenerateResponse(testCase);
        }

        private TestCase GetSeedCase(TestGroup group)
        {
            if (_seedCaseForTest != null)
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
            return new TestCase { Message = seed };
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}

