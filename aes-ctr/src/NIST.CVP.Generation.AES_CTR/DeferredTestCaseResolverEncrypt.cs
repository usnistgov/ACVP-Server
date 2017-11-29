using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class DeferredTestCaseResolverEncrypt : IDeferredTestCaseResolver<TestGroup, TestCase, EncryptionResult>
    {
        private readonly IAesCtr _algo;

        public DeferredTestCaseResolverEncrypt(IAesCtr algo)
        {
            _algo = algo;
        }

        public EncryptionResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            return _algo.EncryptBlock(serverTestCase.Key, serverTestCase.PlainText, iutTestCase.IV);
        }
    }
}
