using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.SafePrimeGroups.v1_0.KeyGen
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
            var param = new DsaKeyParameters
            {
                DomainParameters = await _oracle.GetSafePrimeGroupsDomainParameterAsync(serverTestGroup.SafePrimeGroup)
            };

            var fullParam = new DsaKeyResult
            {
                Key = iutTestCase.Key
            };

            try
            {
                var result = await _oracle.CompleteDeferredDsaKeyAsync(param, fullParam);

                return result.Result ? new FfcKeyPairValidateResult() : new FfcKeyPairValidateResult("Fail");
            }
            catch (Exception ex)
            {
                return new FfcKeyPairValidateResult(ex.Message);
            }            
        }
    }
}