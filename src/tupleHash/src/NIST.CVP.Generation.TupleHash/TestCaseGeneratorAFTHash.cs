using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.TupleHash
{
    public class TestCaseGeneratorAFTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _testCasesToGenerate = 512;
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;
        private int _currentEmptyCase = 1;
        private int _currentSemiEmptyCase = 1;
        private int _currentMiddleCase = 1;
        private int _currentTestCase = 0;
        private int _customizationLength = 1;
        private int _digestSize = 0;
        private int _capacity = 0;

        private readonly IRandom800_90 _random800_90;
        private readonly ITupleHash _algo;

        public int NumberOfTestCasesToGenerate => _testCasesToGenerate;
        public List<int> TestCaseSizes { get; } = new List<int>() { 0 };                 // Primarily for testing purposes

        public TestCaseGeneratorAFTHash(IRandom800_90 random800_90, ITupleHash algo)
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
            var numEmptyCases = group.IncludeNull ? 10 : 0;
            var numSemiEmptyCases = group.IncludeNull ? 30 : 0;
            var numMiddleCases = 25;

            _testCasesToGenerate = numSmallCases + numLargeCases + numEmptyCases + numSemiEmptyCases;

            _digestSize = TestCaseSizes[_currentTestCase++ % TestCaseSizes.Count];

            var customization = "";

            var tuple = new List<BitString>();
            var tupleSize = 0;
            if (_currentEmptyCase <= numEmptyCases)
            {
                tupleSize = _currentEmptyCase - 1;
                for (int i = 0; i < tupleSize; i++)
                {
                    tuple.Add(new BitString(""));
                }
                _currentEmptyCase++;
            }
            else if (_currentSemiEmptyCase <= numSemiEmptyCases)
            {
                tupleSize = ((_currentSemiEmptyCase - 1) + 6) / 3;
                for (int i = 0; i < tupleSize; i++)
                {
                    if (_random800_90.GetRandomInt(0, 2) == 1)  // either 1 or 0
                    {
                        tuple.Add(_random800_90.GetRandomBitString(GetRandomValidLength(group.BitOrientedInput)));
                    }
                    else
                    {
                        tuple.Add(new BitString(""));
                    }
                }
                _currentSemiEmptyCase++;
            }
            else if (_currentSmallCase <= numSmallCases)
            {
                tupleSize = (_currentSmallCase % 3) + 1;
                for (int i = 0; i < tupleSize; i++)
                {
                    tuple.Add(_random800_90.GetRandomBitString(unitSize * _currentSmallCase));
                }
                customization = _random800_90.GetRandomString(_customizationLength);
                _customizationLength = (_customizationLength + 1) % 100;
                _currentSmallCase++;
            }
            else if (_currentMiddleCase <= numMiddleCases)
            {
                if (_currentMiddleCase <= 20)
                {
                    tupleSize = _currentMiddleCase;
                }
                else
                {
                    tupleSize = _currentMiddleCase * 5;
                }
                for (int i = 0; i < tupleSize; i++)
                {
                    tuple.Add(_random800_90.GetRandomBitString(GetRandomValidLength(group.BitOrientedInput)));
                }
                _currentMiddleCase++;
            }
            else
            {
                tuple.Add(_random800_90.GetRandomBitString(GetRandomValidLength(group.BitOrientedInput)));
                if (_customizationLength * _currentLargeCase < 2000)
                {
                    customization = _random800_90.GetRandomString(_customizationLength++ * _currentLargeCase);
                }
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

        private int GetRandomValidLength(bool bitOriented)
        {
            var length = _random800_90.GetRandomInt(1, 2049);
            if (!bitOriented)
            {
                while (length % 8 != 0)
                {
                    length++;
                }
            }
            return length;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
