using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolver<TestGroup, TestCase, FfcVerificationResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public FfcVerificationResult CompleteDeferredCrypto(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
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
                Key = iutTestCase.Key,
                Message = serverTestCase.Message,
                Signature = iutTestCase.Signature
            };

            var result = _oracle.CompleteDeferredDsaSignature(param, fullParam);
            return result.Result ? new FfcVerificationResult() : new FfcVerificationResult("Failed to verify"); 
        }
    }
}
