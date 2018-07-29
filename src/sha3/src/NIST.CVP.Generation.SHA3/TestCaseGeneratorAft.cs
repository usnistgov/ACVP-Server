using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Generation.Core;
using NLog;
using System;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.SHA3
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;

        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 1;

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            var param = new Sha3Parameters
            {
                HashFunction = new HashFunction(group.DigestSize, group.DigestSize * 2, group.Function.ToLower().Equals("shake", StringComparison.OrdinalIgnoreCase)),
                MessageLength = DetermineMessageLength(group.BitOrientedInput, group.IncludeNull, group.DigestSize)
            };

            try
            {
                var oracleResult = await _oracle.GetSha3CaseAsync(param);

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

        private int DetermineMessageLength(bool bitOriented, bool includeNull, int digestSize)
        {
            var unitSize = bitOriented ? 1 : 8;
            var rate = 1600 - digestSize * 2;

            var numSmallCases = rate / unitSize;
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
                numSmallCases = rate / unitSize + 1;
            }

            NumberOfTestCasesToGenerate = numSmallCases + numLargeCases;

            var messageLength = 0;
            if (_currentSmallCase <= numSmallCases)
            {
                messageLength = unitSize * _currentSmallCase++;
            }
            else
            {
                messageLength = rate + _currentLargeCase++ * (rate + unitSize);
            }

            return messageLength;
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

