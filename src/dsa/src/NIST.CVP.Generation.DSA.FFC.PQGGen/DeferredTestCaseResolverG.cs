using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class DeferredTestCaseResolverG : IDeferredTestCaseResolver<TestGroup, TestCase, GValidateResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolverG(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public GValidateResult CompleteDeferredCrypto(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
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
                P = serverTestCase.P,
                Q = serverTestCase.Q,
                Seed = serverTestCase.Seed,
                Index = serverTestCase.Index,
                G = iutTestCase.G
            };

            var result = _oracle.GetDsaGVerify(param, fullParam);

            return result.Result ? new GValidateResult() : new GValidateResult("Failed to validate");
        }
    }
}
