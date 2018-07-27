using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_XPN
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCipherAeadResult>
    {
        private readonly IAeadModeBlockCipher _algo;

        public DeferredTestCaseResolver(IAeadModeBlockCipher algo)
        {
            _algo = algo;
        }

        public SymmetricCipherAeadResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var iv = serverTestCase.IV.GetDeepCopy();
            if (testGroup.IVGeneration.Equals("internal", StringComparison.OrdinalIgnoreCase))
            {
                iv = iutTestCase.IV.GetDeepCopy();
            }

            var salt = serverTestCase.Salt;
            if (testGroup.SaltGen.Equals("internal", StringComparison.OrdinalIgnoreCase))
            {
                salt = iutTestCase.Salt.GetDeepCopy();
            }

            var ivXorSalt = salt.XOR(iv);

            var param = new AeadModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                ivXorSalt,
                serverTestCase.Key,
                serverTestCase.PlainText,
                serverTestCase.AAD,
                testGroup.TagLength
            );

            return _algo.ProcessPayload(param);
        }
    }
}