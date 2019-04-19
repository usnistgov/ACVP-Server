using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.AES_CBC_CTS.v1_0
{
    public class TestCaseGeneratorMmtFullBlock : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        private const int LENGTH_MULTIPLIER = 16;
        private const int BITS_IN_BYTE = 8;

        private int _lenGenIteration = 1;
        private bool _sizesSet = false;
        private List<int> _validSizes = new List<int>();

        public int NumberOfTestCasesToGenerate => 10;

        public TestCaseGeneratorMmtFullBlock(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            if (!_sizesSet)
            {
                _validSizes = GetValidSizes(group.PayloadLen);
            }

            var param = new AesParameters
            {
                Mode = BlockCipherModesOfOperation.CbcCts,
                DataLength = _lenGenIteration++ * LENGTH_MULTIPLIER * BITS_IN_BYTE,
                Direction = group.Function,
                KeyLength = group.KeyLength
            };

            try
            {
                var oracleResult = await _oracle.GetAesCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Key = oracleResult.Key,
                    IV = oracleResult.Iv,
                    PlainText = oracleResult.PlainText,
                    CipherText = oracleResult.CipherText
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private List<int> GetValidSizes(MathDomain dataLength)
        {
            _sizesSet = true;

            return dataLength.GetValues(a => a % 128 == 0, NumberOfTestCasesToGenerate, true).ToList();
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
