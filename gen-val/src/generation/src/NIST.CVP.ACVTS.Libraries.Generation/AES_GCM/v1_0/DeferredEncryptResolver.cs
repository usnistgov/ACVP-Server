using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_GCM.v1_0
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
