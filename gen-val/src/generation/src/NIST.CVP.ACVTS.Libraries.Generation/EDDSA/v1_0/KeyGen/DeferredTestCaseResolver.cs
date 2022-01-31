using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.KeyGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, EdKeyPairGenerateResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<EdKeyPairGenerateResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var param = new EddsaKeyParameters
            {
                Curve = serverTestGroup.Curve
            };

            var fullParam = new EddsaKeyResult
            {
                Key = iutTestCase.KeyPair
            };

            try
            {
                var result = await _oracle.CompleteDeferredEddsaKeyAsync(param, fullParam);

                return new EdKeyPairGenerateResult(result.Key);
            }
            catch (Exception ex)
            {
                return new EdKeyPairGenerateResult(ex.Message);
            }
        }
    }
}
