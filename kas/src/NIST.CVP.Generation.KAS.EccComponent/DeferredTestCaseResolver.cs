using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.EccComponent
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolver<TestGroup, TestCase, SharedSecretResponse>
    {
        private readonly IEccCurveFactory _curveFactory;
        private readonly IEccDhComponent _eccDhComponent;

        public DeferredTestCaseResolver(IEccCurveFactory curveFactory, IEccDhComponent eccDhComponent)
        {
            _curveFactory = curveFactory;
            _eccDhComponent = eccDhComponent;
        }

        public SharedSecretResponse CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var domainParameters = new EccDomainParameters(_curveFactory.GetCurve(testGroup.CurveName));
            return _eccDhComponent.GenerateSharedSecret(
                domainParameters,
                serverTestCase.KeyPairPartyServer,
                iutTestCase.KeyPairPartyIut
            );
        }
    }
}