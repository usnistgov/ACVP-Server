using System;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.CSHAKE
{
    public class TestCaseGeneratorLCTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _numberOfCases = 512;
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;
        private int _customizationLength = 1;

        private readonly IRandom800_90 _random800_90;
        private readonly ICSHAKE _algo;

        public int NumberOfTestCasesToGenerate => _numberOfCases;

        public TestCaseGeneratorLCTHash(IRandom800_90 random800_90, ICSHAKE algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var unitSize = group.BitOrientedInput ? 1 : 8;
            var rate = 1600 - group.DigestSize * 2;

            var numSmallCases = 30;
            var numLargeCases = 30;

            _numberOfCases = numSmallCases + numLargeCases;

            var functionName = "";

            var message = new BitString(0);
            var customization = "";
            if (_currentSmallCase <= numSmallCases)
            {
                message = _random800_90.GetRandomBitString(group.DigestSize);
                customization = _random800_90.GetRandomString(_customizationLength++);
                _currentSmallCase++;
            }
            else
            {
                message = _random800_90.GetRandomBitString(group.DigestSize);
                customization = _random800_90.GetRandomString(_customizationLength++ * _currentLargeCase * _currentLargeCase);
                _currentLargeCase++;
            }

            var testCase = new TestCase
            {
                Message = message,
                FunctionName = functionName,
                Customization = customization,
                Deferred = false
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            HashResult hashResult = null;

            try
            {
                var hashFunction = new HashFunction
                {
                    Capacity = group.DigestSize * 2,
                    DigestSize = group.DigestSize,
                    FunctionName = testCase.FunctionName,
                    Customization = testCase.Customization
                };

                hashResult = _algo.HashMessage(hashFunction, testCase.Message);
                if (!hashResult.Success)
                {
                    ThisLogger.Warn(hashResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(hashResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
                }
            }

            testCase.Digest = hashResult.Digest;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
