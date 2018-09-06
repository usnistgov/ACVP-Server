using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES_CCM
{
    public class TestCaseGeneratorEcmaVadt : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        private int _offset = 0;
        private int _maxCasesToGenerate = 1;

        public int NumberOfTestCasesToGenerate => _maxCasesToGenerate;       // 0-18 bytes of additional adata

        public TestCaseGeneratorEcmaVadt(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            _maxCasesToGenerate = group.AADLengths.Length;

            var param = new AeadParameters
            {
                KeyLength = group.KeyLength,
                AadLength = group.AADLengths[_offset++],
                IvLength = group.IVLength,
                DataLength = group.PTLength,
                TagLength = group.TagLength
            };

            try
            {
                var oracleResult = await _oracle.GetEcmaCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    AAD = oracleResult.Aad,
                    CipherText = oracleResult.CipherText,
                    IV = oracleResult.Iv,
                    Key = oracleResult.Key,
                    PlainText = oracleResult.PlainText,
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
