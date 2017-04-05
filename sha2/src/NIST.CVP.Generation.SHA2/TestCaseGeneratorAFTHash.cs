using System;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.SHA2
{
    public class TestCaseGeneratorAFTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _numberOfCases = 512;
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;
        private int _currentCase = 0;

        private readonly IRandom800_90 _random800_90;
        private readonly ISHA _algo;

        public int NumberOfTestCasesToGenerate { get { return _numberOfCases; } }

        public TestCaseGeneratorAFTHash(IRandom800_90 random800_90, ISHA algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var unitSize = group.BitOriented ? 1 : 8;
            var blockSize = SHAEnumHelpers.DetermineBlockSize(group.DigestSize);

            var numSmallCases = blockSize / unitSize;
            var numLargeCases = blockSize / unitSize;

            if (!group.IncludeNull)
            {
                if (_currentSmallCase == 0)
                {
                    _currentSmallCase = 1;
                }
            }
            else
            {
                numSmallCases = blockSize / unitSize + 1;
            }

            _numberOfCases = numSmallCases + numLargeCases;

            var message = new BitString(0);
            if (_currentSmallCase <= numSmallCases)
            {
                message = _random800_90.GetRandomBitString(unitSize * _currentSmallCase);
                _currentSmallCase++;
            }
            else
            {
                message = _random800_90.GetRandomBitString(blockSize + (unitSize * 99 * _currentLargeCase));
                _currentLargeCase++;
            }

            var testCase = new TestCase
            {
                Message = message,
                Deferred = false
            };

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

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
