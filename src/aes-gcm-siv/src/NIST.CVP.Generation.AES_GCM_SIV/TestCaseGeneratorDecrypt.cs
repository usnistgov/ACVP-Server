using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES_GCM_SIV
{
    public class TestCaseGeneratorDecrypt : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        //private readonly IOracle _oracle;
        private readonly IRandom800_90 _rand;
        private readonly IAeadModeBlockCipherFactory _aeadCipherFactory;
        private readonly IBlockCipherEngineFactory _engineFactory;

        private readonly double FAIL_RATE = 0.25;

        public int NumberOfTestCasesToGenerate => 15;

        public TestCaseGeneratorDecrypt(IRandom800_90 rand, IAeadModeBlockCipherFactory aeadCipherFactory, IBlockCipherEngineFactory engineFactory)
        {
            _rand = rand;
            _aeadCipherFactory = aeadCipherFactory;
            _engineFactory = engineFactory;

            //_oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            var param = new AeadParameters
            {
                PayloadLength = group.PayloadLength,
                KeyLength = group.KeyLength,
                AadLength = group.AadLength,
                CouldFail = true
            };

            try
            {
                var oracleResult = GetAesGcmSivCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    AAD = oracleResult.Aad,
                    Ciphertext = oracleResult.CipherText,
                    IV = oracleResult.Iv,
                    Key = oracleResult.Key,
                    Plaintext = oracleResult.PlainText,
                    TestPassed = oracleResult.TestPassed
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private AeadResult GetAesGcmSivCaseAsync(AeadParameters param)
        {
            var aes = _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes);
            var aead = _aeadCipherFactory.GetAeadCipher(aes, BlockCipherModesOfOperation.GcmSiv);

            var plaintext = _rand.GetRandomBitString(param.PayloadLength);
            var aad = _rand.GetRandomBitString(param.AadLength);
            var key = _rand.GetRandomBitString(param.KeyLength);
            var iv = _rand.GetRandomBitString(96);

            var fullParam = new AeadModeBlockCipherParameters(BlockCipherDirections.Encrypt, iv, key, plaintext, aad, 0);

            var result = aead.ProcessPayload(fullParam);

            if (param.CouldFail)
            {
                // Should Fail at certain ratio, 25%
                var upperBound = (int)(1.0 / FAIL_RATE);
                var shouldFail = _rand.GetRandomInt(0, upperBound) == 0;

                if (shouldFail)
                {
                    result = new SymmetricCipherAeadResult(_rand.GetDifferentBitStringOfSameSize(result.Result), false);
                }
            }

            return new AeadResult
            {
                Aad = aad,
                PlainText = plaintext,
                Key = key,
                Iv = iv,
                CipherText = result.Result,
                TestPassed = result.TestPassed
            };
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
