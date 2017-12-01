using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Crypto.CTR;
using NIST.CVP.Crypto.CTR.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class DeferredTestCaseResolverDecrypt : IDeferredTestCaseResolver<TestGroup, TestCase, CounterDecryptionResult>
    {
        private readonly IAesCtr _algo;

        public DeferredTestCaseResolverDecrypt(IAesCtr algo)
        {
            _algo = algo;
        }

        public CounterDecryptionResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var counter = new TestableCounter(Cipher.AES, iutTestCase.IVs);
            return _algo.Decrypt(serverTestCase.Key, serverTestCase.CipherText, counter);
        }
    }
}
