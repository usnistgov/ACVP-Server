using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.SigGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, VerifyResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<VerifyResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var iutTestGroup = iutTestCase.ParentGroup;

            var param = new RsaSignatureParameters
            {
                HashAlg = serverTestGroup.HashAlg,
                Key = iutTestGroup.Key,
                Modulo = serverTestGroup.Modulo,
                PaddingScheme = serverTestGroup.Mode,
                SaltLength = serverTestGroup.SaltLen,
                MaskFunction = serverTestGroup.MaskFunction,
                IsMessageRandomized = iutTestGroup.IsMessageRandomized
            };

            var fullParam = new RsaSignatureResult
            {
                Message = serverTestCase.Message,
                RandomValue = iutTestCase.RandomValue?.GetMostSignificantBits(iutTestCase.RandomValueLen),
                Salt = serverTestCase.Salt,
                Signature = iutTestCase.Signature
            };

            var result = await _oracle.CompleteDeferredRsaSignatureAsync(param, fullParam);

            return result.Result ? new VerifyResult() : new VerifyResult("Failed to verify.");
        }
    }
}
