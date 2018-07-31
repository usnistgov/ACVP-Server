using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.TupleHash
{
    public class TestCaseGeneratorAft : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _testCasesToGenerate = 512;
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;
        private int _currentEmptyCase = 1;
        private int _currentSemiEmptyCase = 1;
        private int _currentLongTupleCase = 1;
        private int _currentTestCase = 0;
        private int _customizationLength = 1;
        private int _digestLength = 0;
        private int _capacity = 0;

        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => _testCasesToGenerate;
        public List<int> TestCaseSizes { get; } = new List<int>() { 0 };                 // Primarily for testing purposes

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
                DetermineLengths(group.OutputLength);
                _capacity = 2 * group.DigestSize;
            }

            _digestLength = TestCaseSizes[_currentTestCase++ % TestCaseSizes.Count];

            var param = DetermineParameters(group.BitOrientedInput, group.IncludeNull, group.DigestSize, group.HexCustomization, group.XOF);

            Common.Oracle.ResultTypes.HashResultTupleHash oracleResult = null;
            try
            {
                oracleResult = _oracle.GetTupleHashCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                Tuple = oracleResult.Tuple,
                Digest = oracleResult.Digest,
                Customization = oracleResult.Customization,
                CustomizationHex = oracleResult.CustomizationHex,
                Deferred = false,
                DigestLength = _digestLength
            });
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
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

        private TupleHashParameters DetermineParameters(bool bitOriented, bool includeNull, int digestSize, bool hexCustomization, bool xof)
        {
            var unitSize = bitOriented ? 1 : 8;
            var rate = 1600 - digestSize * 2;

            var numSmallCases = (rate / unitSize) * 2;
            var numLargeCases = 100;
            var numEmptyCases = includeNull ? 10 : 0;
            var numSemiEmptyCases = includeNull ? 30 : 0;
            var numLongTupleCases = 25;

            _testCasesToGenerate = numSmallCases + numLargeCases + numEmptyCases + numSemiEmptyCases;

            var customizationLen = 0;
            var tupleSize = 0;
            var messageLen = 0;
            var semiEmpty = false;
            var longRandom = false;
            if (_currentEmptyCase <= numEmptyCases)
            {
                tupleSize = _currentEmptyCase - 1;
                messageLen = 0;
                customizationLen = 0;
                _currentEmptyCase++;
            }
            else if (_currentSemiEmptyCase <= numSemiEmptyCases)
            {
                tupleSize = ((_currentSemiEmptyCase - 1) + 6) / 3;
                semiEmpty = true;
                customizationLen = 0;
                _currentSemiEmptyCase++;
            }
            else if (_currentSmallCase <= numSmallCases)
            {
                tupleSize = (_currentSmallCase % 3) + 1;
                messageLen = unitSize * _currentSmallCase;
                if (hexCustomization)
                {
                    customizationLen = _customizationLength * 8;  // always byte oriented... for now?
                }
                else
                {
                    customizationLen = _customizationLength;
                }
                _customizationLength = (_customizationLength + 1) % 100;
                _currentSmallCase++;
            }
            else if (_currentLongTupleCase <= numLongTupleCases)
            {
                if (_currentLongTupleCase <= 20)
                {
                    tupleSize = _currentLongTupleCase;
                }
                else
                {
                    tupleSize = _currentLongTupleCase * 5;
                }
                longRandom = true;
                _currentLongTupleCase++;
            }
            else
            {
                if (_customizationLength * _currentLargeCase < 2000)
                {
                    if (hexCustomization)
                    {
                        customizationLen = _customizationLength++ * _currentLargeCase * 8;  // always byte oriented... for now?
                    }
                    else
                    {
                        customizationLen = _customizationLength++ * _currentLargeCase;
                    }
                }
                customizationLen = 0;
                messageLen = rate + _currentLargeCase * (rate + unitSize);
                tupleSize = 1;
                _currentLargeCase++;
            }

            return new TupleHashParameters
            {
                HashFunction = new HashFunction(_digestLength, _capacity, xof),
                BitOrientedInput = bitOriented,
                CustomizationLength = customizationLen,
                HexCustomization = hexCustomization,
                LongRandomCase = longRandom,
                SemiEmptyCase = semiEmpty,
                MessageLength = messageLen,
                TupleSize = tupleSize
            };
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
