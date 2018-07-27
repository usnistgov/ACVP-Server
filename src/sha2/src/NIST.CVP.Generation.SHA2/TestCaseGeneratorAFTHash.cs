using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.SHA2
{
    public class TestCaseGeneratorAFTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;

        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 512;

        public TestCaseGeneratorAFTHash(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var param = new ShaParameters
            {
                HashFunction = new HashFunction(group.Function, group.DigestSize),
                MessageLength = DetermineMessageLength(group.BitOriented, group.IncludeNull, SHAEnumHelpers.DetermineBlockSize(group.DigestSize))
            };

            Common.Oracle.ResultTypes.HashResult oracleResult = null;
            try
            {
                oracleResult = _oracle.GetShaCase(param);
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

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
