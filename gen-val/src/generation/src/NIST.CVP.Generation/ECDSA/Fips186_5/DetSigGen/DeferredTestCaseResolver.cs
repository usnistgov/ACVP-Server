using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.ECDSA.v1_0.SigGen;

namespace NIST.CVP.Generation.ECDSA.Fips186_5.DetSigGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, EccVerificationResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<EccVerificationResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var iutTestGroup = iutTestCase.ParentGroup;
            if (iutTestGroup.KeyPair == null)
            {
                return new EccVerificationResult("Could not find Q");
            }

            var param = new EcdsaSignatureParameters
            {
                Curve = serverTestGroup.Curve,
                HashAlg = serverTestGroup.HashAlg,
                PreHashedMessage = serverTestGroup.ComponentTest,
                Key = iutTestGroup.KeyPair,
                IsMessageRandomized = serverTestGroup.IsMessageRandomized,
                NonceProviderType = NonceProviderTypes.Deterministic
            };

            var fullParam = new EcdsaSignatureResult
            {
                Message = serverTestCase.Message,
                RandomValue = iutTestCase.RandomValue?.GetMostSignificantBits(iutTestCase.RandomValueLen),
                Signature = iutTestCase.Signature
            };

            var result = await _oracle.CompleteDeferredEcdsaSignatureAsync(param, fullParam);
            return result.Result ? new EccVerificationResult() : new EccVerificationResult("Failed to verify");
        }
    }
}