using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.Core;
using System;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolver<TestGroup, TestCase, EccKeyPairGenerateResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public EccKeyPairGenerateResult CompleteDeferredCrypto(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var param = new EcdsaKeyParameters
            {
                Curve = serverTestGroup.Curve
            };

            var fullParam = new EcdsaKeyResult
            {
                Key = iutTestCase.KeyPair
            };

            EcdsaKeyResult result = null;
            try
            {
                result = _oracle.CompleteDeferredEcdsaKey(param, fullParam);
            }
            catch (Exception ex)
            {
                return new EccKeyPairGenerateResult(ex.Message);
            }            

            return new EccKeyPairGenerateResult(result.Key);
        }
    }
}
