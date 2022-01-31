using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigGen
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
