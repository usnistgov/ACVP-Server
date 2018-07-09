using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using System;

namespace NIST.CVP.Generation.AES_ECB
{
    public class TestCaseGeneratorMmt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        private const int LENGTH_MULTIPLIER = 16;
        private const int BITS_IN_BYTE = 8;

        private int _lenGenIteration = 1;

        public int NumberOfTestCasesToGenerate => 10;

        public TestCaseGeneratorMmt(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var param = new AesParameters
            {
                Mode = BlockCipherModesOfOperation.Ecb,
                DataLength = _lenGenIteration++ * LENGTH_MULTIPLIER * BITS_IN_BYTE,
                Direction = group.Function,
                KeyLength = group.KeyLength
            };

            AesResult oracleResult = null;
            try
            {
                oracleResult = _oracle.GetAesCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                Key = oracleResult.Key,
                PlainText = oracleResult.PlainText,
                CipherText = oracleResult.CipherText
            });
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, TestCase testCase)
        {
            return null;
        }
    }
}
