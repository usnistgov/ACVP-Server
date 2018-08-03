using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.AES_GCM
{
    public class DeferredEncryptResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, AeadResult>
    {
        private readonly IOracle _oracle;

        public DeferredEncryptResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<AeadResult> CompleteDeferredCryptoAsync(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
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
            
            return await _oracle.CompleteDeferredAesGcmCaseAsync(param, fullParam);
        }
    }
}