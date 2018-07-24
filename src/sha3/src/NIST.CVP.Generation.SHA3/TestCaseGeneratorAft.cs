using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.SHA3
{
    public class TestCaseGeneratorAft : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;

        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 1;

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var param = new Sha3Parameters
            {
                HashFunction = new HashFunction(group.DigestSize, group.DigestSize * 2, group.Function.ToLower().Equals("shake", StringComparison.OrdinalIgnoreCase)),
                MessageLength = DetermineMessageLength(group.BitOrientedInput, group.IncludeNull, group.DigestSize)
            };

            Common.Oracle.ResultTypes.HashResult oracleResult = null;
            try
            {
                oracleResult = _oracle.GetSha3Case(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                Message = oracleResult.Message,
                Digest = oracleResult.Digest
            });
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
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

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

