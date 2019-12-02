using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.EDDSA.v1_0.SigGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, EdVerificationResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<EdVerificationResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var iutTestGroup = iutTestCase.ParentGroup;
            if (iutTestGroup.KeyPair == null)
            {
                return new EdVerificationResult("Could not find Q");
            }

            var param = new EddsaSignatureParameters
            {
                Curve = serverTestGroup.Curve,
                PreHash = serverTestGroup.PreHash,
                Key = iutTestGroup.KeyPair
            };

            var fullParam = new EddsaSignatureResult
            {
                Message = serverTestCase.Message,
                Context = serverTestCase.Context,
                Signature = iutTestCase.Signature
            };

            var result = await _oracle.CompleteDeferredEddsaSignatureAsync(param, fullParam);
            return result.Result ? new EdVerificationResult() : new EdVerificationResult("Failed to verify");
        }
    }
}
