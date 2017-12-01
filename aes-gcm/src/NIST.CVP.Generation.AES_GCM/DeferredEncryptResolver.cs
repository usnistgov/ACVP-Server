using NIST.CVP.Crypto.AES_GCM;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public class DeferredEncryptResolver : IDeferredTestCaseResolver<TestGroup, TestCase, EncryptionResult>
    {
        private readonly IAES_GCM _algo;

        public DeferredEncryptResolver(IAES_GCM algo)
        {
            _algo = algo;
        }

        public EncryptionResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
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