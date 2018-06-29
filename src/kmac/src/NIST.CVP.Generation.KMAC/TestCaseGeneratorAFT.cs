using System;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.KMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.KMAC
{
    public class TestCaseGeneratorAFT : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IKmac _algo;
        private readonly IRandom800_90 _random800_90;

        private const int CUSTOMIZATION_LENGTH = 15;
        private int _numberOfCases = 512;
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;
        private int _customizationLength = 10;

        public int NumberOfTestCasesToGenerate => _numberOfCases;

        public TestCaseGeneratorAFT(IRandom800_90 random800_90, IKmac algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var unitSize = group.BitOrientedInput ? 1 : 8;
            var rate = 1600 - group.DigestSize * 2;

            var numSmallCases = isSample ? 5 : (rate / unitSize) * 2;
            var numLargeCases = isSample ? 3 : 100;

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

            var key = _random800_90.GetRandomBitString(group.KeyLength);

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

            var customization = _random800_90.GetRandomString(10);

            var testCase = new TestCase
            {
                Key = key,
                Message = message,
                Customization = customization
            };
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            MacResult genResult = null;
            try
            {
                genResult = _algo.Generate(testCase.Key, testCase.Message, testCase.Customization, group.MacLength);
                if (!genResult.Success)
                {
                    ThisLogger.Warn(genResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(genResult.ErrorMessage);
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
            testCase.Mac = genResult.Mac;

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
