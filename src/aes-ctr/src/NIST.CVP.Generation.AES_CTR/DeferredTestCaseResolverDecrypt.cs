using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.CTR;
using NIST.CVP.Generation.Core;
using Cipher = NIST.CVP.Crypto.CTR.Enums.Cipher;

namespace NIST.CVP.Generation.AES_CTR
{
    public class DeferredTestCaseResolverDecrypt : IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCounterResult>
    {
        private readonly IAesCtr _algo;

        public DeferredTestCaseResolverDecrypt(IAesCtr algo)
        {
            _algo = algo;
        }

        public SymmetricCounterResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var counter = new TestableCounter(Cipher.AES, iutTestCase.IVs);
            return _algo.Decrypt(serverTestCase.Key, serverTestCase.CipherText, counter);
        }
    }
}
