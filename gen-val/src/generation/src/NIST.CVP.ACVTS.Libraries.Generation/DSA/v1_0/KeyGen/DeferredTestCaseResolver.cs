using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.KeyGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, FfcKeyPairValidateResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<FfcKeyPairValidateResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var iutTestGroup = iutTestCase.ParentGroup;
            if (iutTestGroup.DomainParams.P == 0 || iutTestGroup.DomainParams.Q == 0 || iutTestGroup.DomainParams.G == 0)
            {
                return new FfcKeyPairValidateResult("Could not find p, q, or g");
            }

            var param = new DsaKeyParameters
            {
                DomainParameters = iutTestGroup.DomainParams
            };

            var fullParam = new DsaKeyResult
            {
                Key = iutTestCase.Key
            };

            var result = await _oracle.CompleteDeferredDsaKeyAsync(param, fullParam);

            return result.Result ? new FfcKeyPairValidateResult() : new FfcKeyPairValidateResult("Fail");
        }
    }
}
