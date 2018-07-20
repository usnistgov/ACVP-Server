using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.KMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.KMAC
{
    public class TestCaseGeneratorAFT : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IKmac _algo;
        private readonly IRandom800_90 _random800_90;

        private int _capacity = 0;
        private int _macLength = 0;
        private int _keyLength = 0;
        private int _numberOfCases = 512;
        private int _currentTestCase = 0;
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;
        private int _customizationLength = 0;

        public int NumberOfTestCasesToGenerate => _numberOfCases;
        public List<int> TestCaseSizes { get; } = new List<int>();                 // Primarily for testing purposes
        public List<int> KeySizes { get; } = new List<int>();                 // Primarily for testing purposes

        public TestCaseGeneratorAFT(IRandom800_90 random800_90, IKmac algo)
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
                KeySizes.Clear();
                DetermineLengths(group.KeyLengths, true);
                DetermineLengths(group.MacLengths, false);
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

            _numberOfCases = numSmallCases + numLargeCases;

            _macLength = TestCaseSizes[_currentTestCase % TestCaseSizes.Count];

            _keyLength = KeySizes[_currentTestCase++ % KeySizes.Count];

            var key = _random800_90.GetRandomBitString(_keyLength);

            var message = new BitString(0);
            var customization = "";
            var customizationHex = new BitString(0);
            if (_currentSmallCase <= numSmallCases)
            {
                message = _random800_90.GetRandomBitString(unitSize * _currentSmallCase);
                if (group.HexCustomization)
                {
                    customizationHex = _random800_90.GetRandomBitString(_customizationLength * 8);  // always byte oriented... for now?
                }
                else
                {
                    customization = _random800_90.GetRandomString(_customizationLength);
                }
                _customizationLength = (_customizationLength + 1) % 100;
                _currentSmallCase++;
            }
            else
            {
                if (_customizationLength * _currentLargeCase < 2000)
                {
                    if (group.HexCustomization)
                    {
                        customizationHex = _random800_90.GetRandomBitString(_customizationLength++ * _currentLargeCase * 8);  // always byte oriented... for now?
                    }
                    else
                    {
                        customization = _random800_90.GetRandomString(_customizationLength++ * _currentLargeCase);
                    }
                }
                message = _random800_90.GetRandomBitString(rate + _currentLargeCase * (rate + unitSize));
                _currentLargeCase++;
            }

            TestCase testCase = null;
            if (group.HexCustomization)
            {
                testCase = new TestCase
                {
                    Key = key,
                    Message = message,
                    CustomizationHex = customizationHex,
                    MacLength = _macLength
                };
            }
            else
            {
                testCase = new TestCase
                {
                    Key = key,
                    Message = message,
                    Customization = customization,
                    MacLength = _macLength
                };
            }
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            MacResult genResult = null;
            try
            {
                if (group.HexCustomization)
                {
                    genResult = _algo.Generate(testCase.Key, testCase.Message, testCase.Customization, testCase.MacLength);
                }
                else
                {
                    genResult = _algo.Generate(testCase.Key, testCase.Message, testCase.Customization, testCase.MacLength);
                }
                
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

        private void DetermineLengths(MathDomain domain, bool key)
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
                    if (key)
                    {
                        KeySizes.Add(value);
                    }
                    else
                    {
                        TestCaseSizes.Add(value);
                    }
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
