using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseGeneratorDecrypt<TTestGroup, TTestCase> : ITestCaseGeneratorAsync<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        protected readonly IOracle _oracle;
        
        public TestCaseGeneratorDecrypt(IOracle oracle)
        {
            _oracle = oracle;
        }

        public int NumberOfTestCasesToGenerate => 100;

        public async Task<TestCaseGenerateResponse<TTestGroup, TTestCase>> GenerateAsync(TTestGroup group, bool isSample)
        {
            var param = new KeyWrapParameters
            {
                DataLength = group.PtLen,
                Direction = group.Direction,
                KeyLength = group.KeyLength,
                KeyWrapType = group.KeyWrapType,
                WithInverseCipher = group.UseInverseCipher,
                CouldFail = true
            };

            try
            {
                var oracleResult = await _oracle.GetKeyWrapCaseAsync(param);

                return new TestCaseGenerateResponse<TTestGroup, TTestCase>(new TTestCase()
                {
                    Key = oracleResult.Key,
                    PlainText = oracleResult.Plaintext,
                    CipherText = oracleResult.Ciphertext,
                    TestPassed = oracleResult.TestPassed
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TTestGroup, TTestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}