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
    public class TestCaseGeneratorMmtPartialBlock : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private List<int> _validSizes = new List<int>();

        public int NumberOfTestCasesToGenerate => 25;

        public TestCaseGeneratorMmtPartialBlock(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var dataLength = group.PayloadLen.GetDeepCopy();
            var minMax = dataLength.GetDomainMinMax();
            
            // Use larger numbers only when the "smaller" values don't exist.
            //_validSizes.Add(minMax.Minimum);
            _validSizes.AddRangeIfNotNullOrEmpty(dataLength.GetValues(a => a > 128 && a < 1280 && a % 128 != 0, 128, true));
            _validSizes.AddRangeIfNotNullOrEmpty(dataLength.GetValues(a => a % 128 != 0, 128, true));
            //_validSizes.Add(minMax.Maximum);
            
            _validSizes = _validSizes.Shuffle().Take(NumberOfTestCasesToGenerate).ToList();
            
            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new AesParameters
            {
                Mode = group.BlockCipherModeOfOperation,
                DataLength = _validSizes[caseNo % _validSizes.Count],
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
                    CipherText = oracleResult.CipherText,
                    PayloadLen = param.DataLength
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
