using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_FFX.v1_0.Base
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        private const int LENGTH_MULTIPLIER = 16;
        private const int BITS_IN_BYTE = 8;

        private int _lenGenIteration = 1;

        public int NumberOfTestCasesToGenerate => 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            var param = new AesFfParameters
            {
                AlgoMode = group.AlgoMode,
                Direction = group.Function,
                KeyLength = group.KeyLength,
                MinDataLength = group.Capability.MinLength,
                MaxDataLength = group.Capability.MaxLength
            };

            try
            {
                var oracleResult = await _oracle.GetAesFfCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Key = oracleResult.Key,
                    IV = oracleResult.Iv,
                    PlainText = NumeralString.ToAlphabetString(group.Alphabet, group.Radix, NumeralString.ToNumeralString(oracleResult.PlainText)),
                    CipherText = NumeralString.ToAlphabetString(group.Alphabet, group.Radix, NumeralString.ToNumeralString(oracleResult.CipherText))
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
