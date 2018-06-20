using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CCM
{
    public class TestCaseGeneratorEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAeadModeBlockCipher _algo;

        private BitString _key = null;
        private BitString _nonce = null;
        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGeneratorEncrypt(IRandom800_90 random800_90, IAeadModeBlockCipherFactory cipherFactory, IBlockCipherEngineFactory engineFactory)
        {
            _random800_90 = random800_90;
            _algo = cipherFactory.GetAeadCipher(engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), BlockCipherModesOfOperation.Ccm);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, bool isSample)
        {
            // In instances like 2^16 aadLength, we only want to do a single test case.
            if (group.AADLength > 32 * 8)
            {
                NumberOfTestCasesToGenerate = 1;
            }

            var key = GetReusableInput(ref _key, group.GroupReusesKeyForTestCases, group.KeyLength);
            var iv = GetReusableInput(ref _nonce, group.GroupReusesNonceForTestCases, group.IVLength);
            var plainText = _random800_90.GetRandomBitString(group.PTLength);
            var aad = _random800_90.GetRandomBitString(group.AADLength);
            var testCase = new TestCase
            {
                Key = key,
                IV = iv,
                AAD = aad,
                PlainText = plainText,
                Deferred = false,
                TestPassed = true
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, TestCase testCase)
        {
            SymmetricCipherResult encryptionResult = null;
            try
            {
                var param = new AeadModeBlockCipherParameters(
                    BlockCipherDirections.Encrypt,
                    testCase.IV,
                    testCase.Key,
                    testCase.PlainText,
                    testCase.AAD,
                    group.TagLength
                );

                encryptionResult = _algo.ProcessPayload(param);
                if (!encryptionResult.Success)
                {
                    ThisLogger.Warn(encryptionResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(encryptionResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
                }
            }

            testCase.CipherText = encryptionResult.Result;
            return  new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private BitString GetReusableInput(ref BitString holdInstance, bool isReusable, int lengthToGenerate)
        {
            if (!isReusable)
            {
                holdInstance = null;
            }

            if (holdInstance == null)
            {
                holdInstance = _random800_90.GetRandomBitString(lengthToGenerate);
            }

            return holdInstance;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
