using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgGen
{
    public class DeferredTestCaseResolverG : IDeferredTestCaseResolverAsync<TestGroup, TestCase, GValidateResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolverG(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<GValidateResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var param = new DsaDomainParametersParameters
            {
                HashAlg = serverTestGroup.HashAlg,
                GGenMode = serverTestGroup.GGenMode,
                L = serverTestGroup.L,
                N = serverTestGroup.N
            };

            var fullParam = new DsaDomainParametersResult
            {
                P = serverTestCase.P,
                Q = serverTestCase.Q,
                Seed = serverTestCase.Seed,
                Index = serverTestCase.Index,
                G = iutTestCase.G
            };

            var result = await _oracle.GetDsaGVerifyAsync(param, fullParam);

            return result.Result ? new GValidateResult() : new GValidateResult("Failed to validate");
        }
    }
}
