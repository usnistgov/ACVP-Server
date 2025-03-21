using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.KeyGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, XecdhKeyPairGenerateResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<XecdhKeyPairGenerateResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var param = new XecdhKeyParameters
            {
                Curve = serverTestGroup.Curve
            };

            var fullParam = new XecdhKeyResult
            {
                Key = iutTestCase.KeyPair
            };

            try
            {
                var result = await _oracle.CompleteDeferredXecdhKeyAsync(param, fullParam);

                return new XecdhKeyPairGenerateResult(result.Key);
            }
            catch (Exception ex)
            {
                return new XecdhKeyPairGenerateResult(ex.Message);
            }
        }
    }
}
