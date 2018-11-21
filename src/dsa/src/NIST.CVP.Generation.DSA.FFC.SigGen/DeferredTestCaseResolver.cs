using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
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
                DomainParameters = iutTestGroup.DomainParams
            };

            var fullParam = new DsaSignatureResult
            {
                Key = iutTestGroup.Key,
                Message = serverTestCase.Message,
                Signature = iutTestCase.Signature
            };

            var result = await _oracle.CompleteDeferredDsaSignatureAsync(param, fullParam);
            return result.Result ? new FfcVerificationResult() : new FfcVerificationResult("Failed to verify"); 
        }
    }
}
