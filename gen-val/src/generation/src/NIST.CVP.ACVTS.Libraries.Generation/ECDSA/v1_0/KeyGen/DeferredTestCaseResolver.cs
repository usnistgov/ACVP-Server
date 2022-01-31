using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.KeyGen
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
