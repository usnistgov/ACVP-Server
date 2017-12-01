using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Crypto.CTR;
using NIST.CVP.Crypto.CTR.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class DeferredTestCaseResolverEncrypt : IDeferredTestCaseResolver<TestGroup, TestCase, CounterEncryptionResult>
    {
        private readonly IAesCtr _algo;

        public DeferredTestCaseResolverEncrypt(IAesCtr algo)
        {
            _algo = algo;
        }

        public CounterEncryptionResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var counter = new TestableCounter(Cipher.AES, iutTestCase.IVs);
            return _algo.Encrypt(serverTestCase.Key, serverTestCase.PlainText, counter);
        }
    }
}
