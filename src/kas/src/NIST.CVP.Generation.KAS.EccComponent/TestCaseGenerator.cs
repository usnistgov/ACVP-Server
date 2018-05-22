using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.EccComponent
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IEccCurve _curve;
        private readonly IDsaEcc _dsa;
        private readonly IEccDhComponent _eccDhComponent;
        private readonly EccDomainParameters _domainParameters;

        public TestCaseGenerator(IEccCurve curve, IDsaEcc dsa, IEccDhComponent eccDhComponent)
        {
            _curve = curve;
            _dsa = dsa;
            _eccDhComponent = eccDhComponent;
            _domainParameters = new EccDomainParameters(_curve);
        }

        public int NumberOfTestCasesToGenerate => 25;

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var testCase = new TestCase();

            // When sample, generate the IUT portion as well
            if (isSample)
            {
                testCase.Deferred = false;
                var iutKeyPair = _dsa.GenerateKeyPair(_domainParameters);

                testCase.PrivateKeyIut = iutKeyPair.KeyPair.PrivateD;
                testCase.PublicKeyIutX = iutKeyPair.KeyPair.PublicQ.X;
                testCase.PublicKeyIutY = iutKeyPair.KeyPair.PublicQ.Y;
            }

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            var serverKeyPair = _dsa.GenerateKeyPair(_domainParameters);

            testCase.PrivateKeyServer = serverKeyPair.KeyPair.PrivateD;
            testCase.PublicKeyServerX = serverKeyPair.KeyPair.PublicQ.X;
            testCase.PublicKeyServerY = serverKeyPair.KeyPair.PublicQ.Y;

            // Sample tests aren't deferred, calculate the shared secret
            if (!testCase.Deferred)
            {
                var sharedSecret = _eccDhComponent.GenerateSharedSecret(
                    _domainParameters, 
                    testCase.KeyPairPartyServer,
                    testCase.KeyPairPartyIut
                );

                testCase.Z = sharedSecret.SharedSecretZ;
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }
    }
}
