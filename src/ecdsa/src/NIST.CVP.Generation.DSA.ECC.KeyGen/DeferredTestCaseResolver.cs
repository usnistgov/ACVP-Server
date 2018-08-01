using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using System;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, EccKeyPairGenerateResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<EccKeyPairGenerateResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var param = new EcdsaKeyParameters
            {
                Curve = serverTestGroup.Curve
            };

            var fullParam = new EcdsaKeyResult
            {
                Key = iutTestCase.KeyPair
            };

            try
            {
                var result = await _oracle.CompleteDeferredEcdsaKeyAsync(param, fullParam);

                return new EccKeyPairGenerateResult(result.Key);
            }
            catch (Exception ex)
            {
                return new EccKeyPairGenerateResult(ex.Message);
            }            
        }
    }
}
