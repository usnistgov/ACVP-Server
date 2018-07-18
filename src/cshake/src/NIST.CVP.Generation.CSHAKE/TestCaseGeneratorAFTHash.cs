using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.CSHAKE
{
    public class TestCaseGeneratorAFTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _testCasesToGenerate = 512;
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;
        private int _currentTestCase = 0;
        private int _customizationLength = 1;
        private int _digestLength = 0;
        private int _capacity = 0;

        private string[] VALID_FUNCTION_NAMES = new string[] { "KMAC", "TupleHash", "ParallelHash", "" };

        private readonly IRandom800_90 _random800_90;
        private readonly ICSHAKE _algo;
        
        public int NumberOfTestCasesToGenerate => _testCasesToGenerate;
        public List<int> TestCaseSizes { get; } = new List<int>() { 0 };                 // Primarily for testing purposes

        public TestCaseGeneratorAFTHash(IRandom800_90 random800_90, ICSHAKE algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var unitSize = group.BitOrientedInput ? 1 : 8;
            var rate = 1600 - group.DigestSize * 2;

            // Only do this logic once
            if (_capacity == 0)
            {
                TestCaseSizes.Clear();
                DetermineLengths(group.OutputLength);
                _capacity = 2 * group.DigestSize;
            }

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

            _testCasesToGenerate = numSmallCases + numLargeCases;

            _digestLength = TestCaseSizes[_currentTestCase++ % TestCaseSizes.Count];

            var functionName = "";
            var customization = "";

            var message = new BitString(0);
            if (_currentSmallCase <= numSmallCases)
            {
                message = _random800_90.GetRandomBitString(unitSize * _currentSmallCase);
                customization = _random800_90.GetRandomString(_customizationLength);
                _customizationLength = (_customizationLength + 1) % 100;
                _currentSmallCase++;
            }
            else
            {
                if (_customizationLength * _currentLargeCase < 2000)
                {
                    customization = _random800_90.GetRandomString(_customizationLength++ * _currentLargeCase);
                    functionName = VALID_FUNCTION_NAMES[_currentLargeCase % 4];
                }
                message = _random800_90.GetRandomBitString(rate + _currentLargeCase * (rate + unitSize));
                _currentLargeCase++;
            }

            var testCase = new TestCase
            {
                Message = message,
                FunctionName = functionName,
                Customization = customization,
                Deferred = false,
                DigestLength = _digestLength
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
                    Capacity = _capacity,
                    DigestLength = testCase.DigestLength,
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

        private void DetermineLengths(MathDomain domain)
        {
            domain.SetRangeOptions(RangeDomainSegmentOptions.Random);
            var minMax = domain.GetDomainMinMax();

            var values = domain.GetValues(1000).OrderBy(o => Guid.NewGuid()).Take(1000);
            int repetitions;

            if (values.Count() == 0)
            {
                repetitions = 999;
            }
            else if (values.Count() > 999)
            {
                repetitions = 1;
            }
            else
            {
                repetitions = 1000 / values.Count() + (1000 % values.Count() > 0 ? 1 : 0);
            }

            foreach (var value in values)
            {
                for (var i = 0; i < repetitions; i++)
                {
                    TestCaseSizes.Add(value);
                }
            }

            // Make sure min and max appear in the list
            TestCaseSizes.Add(minMax.Minimum);
            TestCaseSizes.Add(minMax.Maximum);

            TestCaseSizes.Sort();
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
