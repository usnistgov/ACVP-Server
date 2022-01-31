using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.SigGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, FfcVerificationResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<FfcVerificationResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var iutTestGroup = iutTestCase.ParentGroup;
            if (iutTestGroup.DomainParams == null)
            {
                return new FfcVerificationResult("Could not find p, q or g");
            }

            var param = new DsaSignatureParameters
            {
                HashAlg = serverTestGroup.HashAlg,
                DomainParameters = iutTestGroup.DomainParams,
                IsMessageRandomized = serverTestGroup.IsMessageRandomized
            };

            var fullParam = new DsaSignatureResult
            {
                Key = iutTestGroup.Key,
                Message = serverTestCase.Message,
                RandomValue = iutTestCase.RandomValue?.GetMostSignificantBits(iutTestCase.RandomValueLen),
                Signature = iutTestCase.Signature
            };

            var result = await _oracle.CompleteDeferredDsaSignatureAsync(param, fullParam);
            return result.Result ? new FfcVerificationResult() : new FfcVerificationResult("Failed to verify");
        }
    }
}
