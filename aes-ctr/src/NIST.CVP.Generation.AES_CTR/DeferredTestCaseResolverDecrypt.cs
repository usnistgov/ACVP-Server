using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class DeferredTestCaseResolverDecrypt : IDeferredTestCaseResolver<TestGroup, TestCase, DecryptionResult>
    {
        private readonly IAesCtr _algo;

        public DeferredTestCaseResolverDecrypt(IAesCtr algo)
        {
            _algo = algo;
        }

        public DecryptionResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            return _algo.DecryptBlock(serverTestCase.Key, serverTestCase.CipherText, iutTestCase.IV);
        }
    }
}
