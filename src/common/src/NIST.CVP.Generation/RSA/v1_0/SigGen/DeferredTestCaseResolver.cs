using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.RSA.v1_0.SigGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, VerifyResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<VerifyResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var iutTestGroup = iutTestCase.ParentGroup;

            var param = new RsaSignatureParameters
            {
                HashAlg = serverTestGroup.HashAlg,
                Key = iutTestGroup.Key,
                Modulo = serverTestGroup.Modulo,
                PaddingScheme = serverTestGroup.Mode,
                SaltLength = serverTestGroup.SaltLen
            };

            var fullParam = new RsaSignatureResult
            {
                Message = serverTestCase.Message,
                Salt = serverTestCase.Salt,
                Signature = iutTestCase.Signature
            };

            var result = await _oracle.CompleteDeferredRsaSignatureAsync(param, fullParam);

            return result.Result ? new VerifyResult() : new VerifyResult("Failed to verify.");
        }
    }
}
