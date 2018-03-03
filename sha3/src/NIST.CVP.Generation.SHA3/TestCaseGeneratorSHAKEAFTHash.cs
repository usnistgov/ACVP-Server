using System;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.SHA3
{
    public class TestCaseGeneratorSHAKEAFTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _numberOfCases = 512;
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;

        private readonly IRandom800_90 _random800_90;
        private readonly ISHA3 _algo;

        public int NumberOfTestCasesToGenerate { get { return _numberOfCases; } }

        public TestCaseGeneratorSHAKEAFTHash(IRandom800_90 random800_90, ISHA3 algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var unitSize = group.BitOrientedInput ? 1 : 8;
            var rate = 1600 - group.DigestSize * 2;

            var numSmallCases = (rate / unitSize) * 2;
            var numLargeCases = 100;

            if (!group.IncludeNull)
            {
                if (_currentSmallCase == 0)
                {
                    _currentSmallCase = 1;
                }
            }
            else
            {
                numSmallCases = (rate / unitSize) * 2 + 1;
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
                message = _random800_90.GetRandomBitString(rate + _currentLargeCase * (rate + unitSize));
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
                    Capacity = group.DigestSize * 2,
                    DigestSize = group.DigestSize,
                    XOF = true
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
