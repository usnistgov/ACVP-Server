using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.ParallelHash
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private int _testCasesToGenerate = 512;
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;
        private int _currentTestCase = 0;
        private int _customizationLength = 1;
        private int _digestLength = 0;
        private int _capacity = 0;
        private int _blockSize = 8;

        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => _testCasesToGenerate;
        public List<int> TestCaseSizes { get; } = new List<int>() { 0 };                 // Primarily for testing purposes

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
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

            try
            {
                var oracleResult = await _oracle.GetParallelHashCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = oracleResult.Message,
                    Digest = oracleResult.Digest,
                    Customization = oracleResult.Customization,
                    CustomizationHex = oracleResult.CustomizationHex,
                    BlockSize = _blockSize,
                    Deferred = false,
                    DigestLength = _digestLength
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
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

        private ParallelHashParameters DetermineParameters(bool bitOriented, bool includeNull, int digestSize, bool hexCustomization, bool xof)
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

            _testCasesToGenerate = numSmallCases + numLargeCases;

            var customizationLength = 0;

            var messageLength = 0;
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
                _blockSize = 64;
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
                if (_currentLargeCase < 33)
                {
                    _blockSize = _currentLargeCase * 8;
                }
                else
                {
                    _blockSize = _currentLargeCase < 56 ? 512 : (_currentLargeCase < 79 ? 1024 : 2048);    // 33 to 55 is 64, 56 to 78 is 128, 79 to 100 is 256
                }
                _currentLargeCase++;
            }

            return new ParallelHashParameters
            {
                HashFunction = new HashFunction(_digestLength, digestSize * 2, xof),
                CustomizationLength = customizationLength,
                HexCustomization = hexCustomization,
                BlockSize = _blockSize / 8,         // needs to be in bytes for algo
                MessageLength = messageLength
            };
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
