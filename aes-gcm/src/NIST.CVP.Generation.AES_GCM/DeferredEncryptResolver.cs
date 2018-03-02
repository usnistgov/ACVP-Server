using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public class DeferredEncryptResolver : IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCipherAeadResult>
    {
        private readonly IAES_GCM _algo;

        public DeferredEncryptResolver(IAES_GCM algo)
        {
            _algo = algo;
        }

        public SymmetricCipherAeadResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            return _algo.BlockEncrypt(
                serverTestCase.Key, 
                serverTestCase.PlainText, 
                iutTestCase.IV, 
                serverTestCase.AAD,
                testGroup.TagLength
            );
        }
    }
}