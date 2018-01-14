using System;
using NIST.CVP.Crypto.AES_GCM;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_XPN
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCipherAeadResult>
    {
        private readonly IAES_GCM _algo;

        public DeferredTestCaseResolver(IAES_GCM algo)
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

            return _algo.BlockEncrypt(
                serverTestCase.Key,
                serverTestCase.PlainText,
                ivXorSalt,
                serverTestCase.AAD,
                testGroup.TagLength
            );
        }
    }
}