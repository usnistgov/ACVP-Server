using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TupleHash
{
    public class TestCaseGeneratorAFTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _numberOfCases = 512;
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;
        private int _customizationLength = 0;

        private readonly IRandom800_90 _random800_90;
        private readonly ITupleHash _algo;

        public int NumberOfTestCasesToGenerate => _numberOfCases;

        public TestCaseGeneratorAFTHash(IRandom800_90 random800_90, ITupleHash algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
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
            
            var customization = "";
            customization = _random800_90.GetRandomString(_customizationLength++);

            var tuple = new List<BitString>();
            if (_currentSmallCase <= numSmallCases)
            {
                tuple.Add(_random800_90.GetRandomBitString(unitSize * _currentSmallCase));
                _currentSmallCase++;
            }
            else
            {
                tuple.Add(_random800_90.GetRandomBitString(rate + _currentLargeCase * (rate + unitSize)));
                _currentLargeCase++;
            }

            var testCase = new TestCase
            {
                Tuple = tuple,
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
                    XOF = group.XOF,
                    Customization = testCase.Customization
                };

                hashResult = _algo.HashMessage(hashFunction, testCase.Tuple);
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
