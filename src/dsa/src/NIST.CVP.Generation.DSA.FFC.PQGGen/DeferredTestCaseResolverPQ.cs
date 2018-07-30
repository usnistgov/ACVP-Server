using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class DeferredTestCaseResolverPQ : IDeferredTestCaseResolver<TestGroup, TestCase, PQValidateResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolverPQ(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public PQValidateResult CompleteDeferredCrypto(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
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
                Seed = serverTestCase.Seed,
                Counter = iutTestCase.Counter
            };

            var result = _oracle.GetDsaPQVerify(param, fullParam);

            return result.Result ? new PQValidateResult() : new PQValidateResult("Failed to validate");
        }
    }
}
