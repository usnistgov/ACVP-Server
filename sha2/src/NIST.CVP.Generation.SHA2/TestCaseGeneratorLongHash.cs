using System;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.SHA2
{
    public class TestCaseGeneratorLongHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _numberOfCases = 512;
        private int _currentCase = 1;

        private readonly IRandom800_90 _random800_90;
        private readonly ISHA _algo;

        public int NumberOfTestCasesToGenerate { get { return _numberOfCases; } }

        public TestCaseGeneratorLongHash(IRandom800_90 random800_90, ISHA algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var blockSize = DetermineBlockSize(group.DigestSize);
            var unitSize = (group.BitOriented ? 1 : 8);
            _numberOfCases = blockSize / unitSize;

            var message = _random800_90.GetRandomBitString(blockSize + (unitSize * 99 * _currentCase));
            var testCase = new TestCase
            {
                Message = message,
                Deferred = false
            };

            _currentCase++;

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            HashResult hashResult = null;

            try
            {
                var hashFunction = new HashFunction
                {
                    Mode = group.Function,
                    DigestSize = group.DigestSize
                };

                hashResult = _algo.HashMessage(hashFunction, testCase.Message);
                if (!hashResult.Success)
                {
                    ThisLogger.Warn(hashResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse(hashResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse(ex.Message);
                }
            }

            testCase.Digest = hashResult.Digest;
            return new TestCaseGenerateResponse(testCase);
        }

        private int DetermineBlockSize(DigestSizes digestSize)
        {
            switch (digestSize)
            {
                case DigestSizes.d160:
                case DigestSizes.d224:
                case DigestSizes.d256:
                    return 512;

                case DigestSizes.d384:
                case DigestSizes.d512:
                case DigestSizes.d512t224:
                case DigestSizes.d512t256:
                    return 1024;
            }

            throw new Exception("Invalid block size in TestCaseGenerator");
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
