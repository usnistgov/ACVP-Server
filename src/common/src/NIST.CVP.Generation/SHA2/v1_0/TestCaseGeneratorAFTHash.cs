using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math.Domain;
using NIST.CVP.Math.Helpers;
using NLog;

namespace NIST.CVP.Generation.SHA2.v1_0
{
    public class TestCaseGeneratorAFTHash : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private int _currentSmallCase = 0;
        private int _currentSmallCaseSize = 0;

        private int _currentLargeCase = 0;
        private int[] _largeCases;

        private int _currentSpecialCase = 0;

        private readonly IOracle _oracle;

        // Initial values don't matter here (just less than blockSize), just gotta make sure they are run
        private int _maxSmallCases = 1;
        private int _maxLargeCases = 1;

        private int[] _specialCases = new int[2];
        public int NumberOfTestCasesToGenerate { get; private set; } = 512;

        public TestCaseGeneratorAFTHash(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            var minMax = group.MessageLength.GetDomainMinMax();
            _specialCases[0] = minMax.Minimum;
            _specialCases[1] = minMax.Maximum;

            var blockSize = SHAEnumHelpers.DetermineBlockSize(group.DigestSize);

            // Small cases handle [0, blockSize] and we can hit all of them
            _maxSmallCases = blockSize + 1;

            // Large cases handle [blockSize, 65535] and we can't hit all of them (blockSize is either 512 or 1024)
            // If nothing above the blockSize is supported, skip the large tests
            _maxLargeCases = minMax.Maximum > blockSize ? blockSize : 0;

            NumberOfTestCasesToGenerate = _maxSmallCases + _maxLargeCases + _specialCases.Length;

            var param = new ShaParameters
            {
                HashFunction = new HashFunction(group.Function, group.DigestSize),
                MessageLength = DetermineMessageLength(group.MessageLength, blockSize)
            };

            try
            {
                var oracleResult = await _oracle.GetShaCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = oracleResult.Message,
                    Digest = oracleResult.Digest
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private int DetermineMessageLength(MathDomain messageLength, int blockSize)
        {
            if (_currentSmallCase < _maxSmallCases)
            {
                // Small cases
                return DetermineSmallMessageLength(messageLength, blockSize);
            }
            else
            {
                if (_currentLargeCase < _maxLargeCases)
                {
                    // Large cases
                    // Find the large case sizes by grabbing a whole bunch and picking as many as we need
                    if (_largeCases == null)
                    {
                        _largeCases = messageLength.GetValues(x => (x > blockSize), ParameterValidator.MAX_MESSAGE_LENGTH, true).Take(_maxLargeCases).ToArray();
                    }

                    // Adjust the length if needed
                    _maxLargeCases = _largeCases.Length;

                    var current = _currentLargeCase;
                    _currentLargeCase++;
                    return _largeCases[current];
                }
                else
                {
                    // Special cases
                    var current = _currentSpecialCase;
                    _currentSpecialCase++;
                    return _specialCases[current];
                }
            }
        }

        private int DetermineSmallMessageLength(MathDomain messageLength, int blockSize)
        {
            // Keep trying sizes until we find one that works
            while (_currentSmallCase <= _maxSmallCases)
            {
                // Found one that works
                if (messageLength.IsWithinDomain(_currentSmallCaseSize))
                {
                    // Increment number of cases and increment the size
                    var sizeSelected = _currentSmallCaseSize;
                    _currentSmallCaseSize = _currentSmallCaseSize.IncrementOrReset(ParameterValidator.MIN_MESSAGE_LENGTH, blockSize);

                    _currentSmallCase++;
                    return sizeSelected;
                }

                // Increment, and if we exceed blockSize, reset to 0
                _currentSmallCaseSize = _currentSmallCaseSize.IncrementOrReset(ParameterValidator.MIN_MESSAGE_LENGTH, blockSize);
            }

            // Should never hit this point
            throw new Exception("Should never get here. Too many small tests generated!");
        }

        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
