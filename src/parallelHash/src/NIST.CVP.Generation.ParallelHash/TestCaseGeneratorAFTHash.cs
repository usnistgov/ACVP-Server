using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.ParallelHash
{
    public class TestCaseGeneratorAFTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _testCasesToGenerate = 512;
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;
        private int _currentTestCase = 0;
        private int _customizationLength = 1;
        private int _digestSize = 0;
        private int _capacity = 0;
        private int _blockSize = 8;

        private readonly IRandom800_90 _random800_90;
        private readonly IParallelHash _algo;

        public int NumberOfTestCasesToGenerate => _testCasesToGenerate;
        public List<int> TestCaseSizes { get; } = new List<int>() { 0 };                 // Primarily for testing purposes

        public TestCaseGeneratorAFTHash(IRandom800_90 random800_90, IParallelHash algo)
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

            _digestSize = TestCaseSizes[_currentTestCase++ % TestCaseSizes.Count];

            var customization = "";

            var message = new BitString(0);
            if (_currentSmallCase <= numSmallCases)
            {
                message = _random800_90.GetRandomBitString(unitSize * _currentSmallCase);
                customization = _random800_90.GetRandomString(_customizationLength);
                _customizationLength = (_customizationLength + 1) % 100;
                _blockSize = 8;
                _currentSmallCase++;
            }
            else
            {
                if (_customizationLength * _currentLargeCase < 2000)
                {
                    customization = _random800_90.GetRandomString(_customizationLength++ * _currentLargeCase);
                }
                message = _random800_90.GetRandomBitString(rate + _currentLargeCase * (rate + unitSize));
                if (_currentLargeCase < 33)
                {
                    _blockSize = _currentLargeCase;
                }
                else
                {
                    _blockSize = _currentLargeCase < 56 ? 64 : (_currentLargeCase < 79 ? 128 : 256);    // 33 to 55 is 64, 56 to 78 is 128, 79 to 100 is 256
                }
                _currentLargeCase++;
            }

            var testCase = new TestCase
            {
                Message = message,
                Customization = customization,
                BlockSize = _blockSize,
                Deferred = false,
                DigestLength = _digestSize
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
                    DigestLength = testCase.DigestLength,
                    BlockSize = testCase.BlockSize,
                    XOF = group.XOF,
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
