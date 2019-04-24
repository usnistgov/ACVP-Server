using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math.Domain;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES_CBC_CTS.v1_0
{
    public class TestCaseGeneratorMmtFullBlock : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        private bool _sizesSet = false;
        private List<int> _validSizes = new List<int>();
        private int _currentIndex = 0;

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
                Mode = group.BlockCipherModeOfOperation,
                DataLength = GetDataLength(),
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

        private int GetDataLength()
        {
            var valueToReturn = _validSizes[_currentIndex];
            _currentIndex++;

            if (_validSizes.Count == _currentIndex)
            {
                _currentIndex = 0;
            }

            return valueToReturn;
        }

        private List<int> GetValidSizes(MathDomain dataLength)
        {
            _sizesSet = true;

            List<int> values = new List<int>();
            // Use larger numbers only when the "smaller" values don't exist.
            values.AddRangeIfNotNullOrEmpty(dataLength.GetValues(a => a > 128 && a < 1280 && a % 128 == 0, 128, true));
            values.AddRangeIfNotNullOrEmpty(dataLength.GetValues(a => a % 128 == 0, 128, true));

            return values.Take(NumberOfTestCasesToGenerate).ToList();
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
