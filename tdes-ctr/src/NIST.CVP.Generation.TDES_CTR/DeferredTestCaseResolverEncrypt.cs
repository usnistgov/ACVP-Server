using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.CTR;
using NIST.CVP.Crypto.CTR.Enums;
using NIST.CVP.Crypto.TDES_CTR;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class DeferredTestCaseResolverEncrypt : IDeferredTestCaseResolver<TestGroup, TestCase, CounterEncryptionResult>
    {
        private readonly ITdesCtr _algo;

        public DeferredTestCaseResolverEncrypt(ITdesCtr algo)
        {
            _algo = algo;
        }

        public CounterEncryptionResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var counter = new TestableCounter(Cipher.TDES, iutTestCase.Ivs);
            return _algo.Encrypt(serverTestCase.Key, serverTestCase.PlainText, counter);
        }
    }
}
