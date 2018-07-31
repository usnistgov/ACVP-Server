using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.KMAC
{
    public class TestCaseGeneratorAft : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

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

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            // Only do this logic once
            if (_capacity == 0)
            {
                TestCaseSizes.Clear();
                KeySizes.Clear();
                DetermineLengths(group.KeyLengths, true);
                DetermineLengths(group.MacLengths, false);
                _capacity = 2 * group.DigestSize;
            }

            var param = DetermineParameters(group.BitOrientedInput, group.DigestSize, group.IncludeNull, group.HexCustomization, group.XOF);

            try
            {
                var oracleResult = _oracle.GetKmacCase(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Key = oracleResult.Key,
                    Message = oracleResult.Message,
                    Mac = oracleResult.Tag,
                    Customization = oracleResult.Customization,
                    CustomizationHex = oracleResult.CustomizationHex,
                    MacLength = oracleResult.Tag.BitLength
                });
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            throw new NotImplementedException();
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

        private KmacParameters DetermineParameters(bool bitOriented, int digestSize, bool includeNull, bool hexCustomization, bool xof)
        {
            var unitSize = bitOriented ? 1 : 8;
            var rate = 1600 - digestSize * 2;

            var numSmallCases = (rate / unitSize) * 2;
            var numLargeCases = 100;

            if (!includeNull)
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

            var messageLength = 0;
            var customizationLength = 0;
            if (_currentSmallCase <= numSmallCases)
            {
                messageLength = unitSize * _currentSmallCase;
                if (hexCustomization)
                {
                    customizationLength = _customizationLength * 8;  // always byte oriented... for now?
                }
                else
                {
                    customizationLength = _customizationLength;
                }
                _customizationLength = (_customizationLength + 1) % 100;
                _currentSmallCase++;
            }
            else
            {
                if (_customizationLength * _currentLargeCase < 2000)
                {
                    if (hexCustomization)
                    {
                        customizationLength = _customizationLength++ * _currentLargeCase * 8;  // always byte oriented... for now?
                    }
                    else
                    {
                        customizationLength = _customizationLength++ * _currentLargeCase;
                    }
                }
                messageLength = rate + _currentLargeCase * (rate + unitSize);
                _currentLargeCase++;
            }

            return new KmacParameters()
            {
                CouldFail = false,
                CustomizationLength = customizationLength,
                DigestSize = digestSize,
                HexCustomization = hexCustomization,
                KeyLength = _keyLength,
                MacLength = _macLength,
                MessageLength = messageLength,
                XOF = xof
            };
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
