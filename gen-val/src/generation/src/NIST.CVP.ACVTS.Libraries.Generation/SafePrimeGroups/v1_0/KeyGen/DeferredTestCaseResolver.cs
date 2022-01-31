using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.SafePrimeGroups.v1_0.KeyGen
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
                DomainParameters = SafePrimesFactory.GetDomainParameters(serverTestGroup.SafePrimeGroup)
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
