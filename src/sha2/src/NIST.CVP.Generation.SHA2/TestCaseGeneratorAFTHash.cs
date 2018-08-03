using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Generation.Core;
using NLog;
using System;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.SHA2
{
    public class TestCaseGeneratorAFTHash : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;

        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 512;

        public TestCaseGeneratorAFTHash(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            var param = new ShaParameters
            {
                HashFunction = new HashFunction(group.Function, group.DigestSize),
                MessageLength = DetermineMessageLength(group.BitOriented, group.IncludeNull, SHAEnumHelpers.DetermineBlockSize(group.DigestSize))
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

        private int DetermineMessageLength(bool bitOriented, bool includeNull, int blockSize)
        {
            var unitSize = bitOriented ? 1 : 8;

            var numSmallCases = blockSize / unitSize;
            var numLargeCases = blockSize / unitSize;

            if (includeNull)
            {
                if (_currentSmallCase == 0)
                {
                    _currentSmallCase = 1;
                }
            }
            else
            {
                numSmallCases = blockSize / unitSize + 1;
            }

            NumberOfTestCasesToGenerate = numSmallCases + numLargeCases;

            if (_currentSmallCase <= numSmallCases)
            {
                return unitSize * _currentSmallCase++;
            }
            else
            {
                return blockSize + unitSize * 99 * _currentLargeCase++;
            }
        }

        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
