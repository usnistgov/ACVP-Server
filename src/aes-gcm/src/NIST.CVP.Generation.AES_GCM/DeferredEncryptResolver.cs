using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public class DeferredEncryptResolver : IDeferredTestCaseResolver<TestGroup, TestCase, AeadResult>
    {
        private readonly IOracle _oracle;

        public DeferredEncryptResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public AeadResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var param = new AeadParameters
            {
                TagLength = testGroup.TagLength
            };

            var fullParam = new AeadResult
            {
                Iv = iutTestCase.IV,
                Key = serverTestCase.Key,
                PlainText = serverTestCase.PlainText,
                Aad = serverTestCase.AAD
            };
            
            return _oracle.CompleteDeferredAesGcmCase(param, fullParam);
        }
    }
}