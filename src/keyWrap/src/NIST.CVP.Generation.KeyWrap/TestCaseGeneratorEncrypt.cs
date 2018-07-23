using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseGeneratorEncrypt<TTestGroup, TTestCase> : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        private readonly IOracle _oracle;

        public TestCaseGeneratorEncrypt(IOracle oracle)
        {
            _oracle = oracle;
        }

        public int NumberOfTestCasesToGenerate => 100;

        public TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup group, bool isSample)
        {
            var param = new KeyWrapParameters
            {
                DataLength = group.PtLen,
                Direction = group.Direction,
                KeyLength = group.KeyLength,
                KeyWrapType = group.KeyWrapType,
                WithInverseCipher = group.UseInverseCipher,
                CouldFail = false
            };

            try
            {
                var oracleResult = _oracle.GetKeyWrapCase(param);

                return new TestCaseGenerateResponse<TTestGroup, TTestCase>(new TTestCase()
                {
                    Key = oracleResult.Key,
                    PlainText = oracleResult.Plaintext,
                    CipherText = oracleResult.Ciphertext
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TTestGroup, TTestCase>($"Failed to generate. {ex.Message}");
            }
        }

        public TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup group, TTestCase testCase)
        {
            throw new NotImplementedException();
        }

        private static Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}