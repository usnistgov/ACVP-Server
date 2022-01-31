using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.SigGen
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
                NonceProviderType = NonceProviderTypes.Random
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
