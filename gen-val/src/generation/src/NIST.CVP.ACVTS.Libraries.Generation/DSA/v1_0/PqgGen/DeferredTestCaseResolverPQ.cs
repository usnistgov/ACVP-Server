using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgGen
{
    public class DeferredTestCaseResolverPQ : IDeferredTestCaseResolverAsync<TestGroup, TestCase, PQValidateResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolverPQ(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<PQValidateResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var param = new DsaDomainParametersParameters
            {
                HashAlg = serverTestGroup.HashAlg,
                PQGenMode = serverTestGroup.PQGenMode,
                L = serverTestGroup.L,
                N = serverTestGroup.N
            };

            var fullParam = new DsaDomainParametersResult
            {
                P = iutTestCase.P,
                Q = iutTestCase.Q,
                Seed = iutTestCase.Seed,
                Counter = iutTestCase.Counter
            };

            var result = await _oracle.GetDsaPQVerifyAsync(param, fullParam);

            return result.Result ? new PQValidateResult() : new PQValidateResult("Failed to validate");
        }
    }
}
