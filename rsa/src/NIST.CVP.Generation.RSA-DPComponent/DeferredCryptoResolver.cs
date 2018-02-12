using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class DeferredCryptoResolver : IDeferredTestCaseResolver<TestGroup, TestCase, EncryptionResult>
    {
        private readonly IRsa _rsa;

        public DeferredCryptoResolver(IRsa rsa)
        {
            _rsa = rsa;
        }

        public EncryptionResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            return _rsa.Encrypt(iutTestCase.PlainText.ToPositiveBigInteger(), iutTestCase.Key.PubKey);
        }
    }
}
