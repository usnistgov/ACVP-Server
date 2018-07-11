using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using System;

namespace NIST.CVP.Generation.AES_XTS
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 50;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 10;
            }

            var param = new AesXtsParameters
            {
                Mode = BlockCipherModesOfOperation.Xts,
                Direction = group.Direction,
                DataLength = group.PtLen,
                KeyLength = group.KeyLen,
                TweakMode = group.TweakMode
            };

            AesXtsResult oracleResult = null;
            try
            {
                oracleResult = _oracle.GetAesXtsCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                CipherText = oracleResult.CipherText,
                I = oracleResult.Iv,
                Key = oracleResult.Key,
                PlainText = oracleResult.PlainText,
                SequenceNumber = oracleResult.SequenceNumber,
                XtsKey = new XtsKey(oracleResult.Key)
            });
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            throw new NotImplementedException();
        }
    }
}
