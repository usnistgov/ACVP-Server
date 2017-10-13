using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseGeneratorAftNoKdfNoKc : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IKasBuilder _kasBuilder;
        private readonly ISchemeBuilder _schemeBuilder;
        private readonly IDsaFfcFactory _dsaFactory;
        private readonly IShaFactory _shaFactory;

        public TestCaseGeneratorAftNoKdfNoKc(IKasBuilder kasBuilder, ISchemeBuilder schemeBuilder, IDsaFfcFactory dsaFactory, IShaFactory shaFactory)
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _dsaFactory = dsaFactory;
            _shaFactory = shaFactory;
        }

        public int NumberOfTestCasesToGenerate => 10;
        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            var testCase = new TestCase()
            {
                Deferred = true
            };

            var serverRole = group.KasRole == KeyAgreementRole.InitiatorPartyU
                ? KeyAgreementRole.ResponderPartyV
                : KeyAgreementRole.InitiatorPartyU;

            var party = _kasBuilder
                .WithAssurances(KasAssurance.None)
                .WithScheme(group.Scheme)
                .WithParameterSet(group.ParmSet)
                .WithPartyId(group.IdServer)
                .WithKeyAgreementRole(serverRole)
                .BuildNoKdfNoKc()
                .Build();

            party.SetDomainParameters(new FfcDomainParameters(group.P, group.Q, group.G));
            party.ReturnPublicInfoThisParty();

            testCase.StaticPrivateKeyServer = party.Scheme.StaticKeyPair?.PrivateKeyX ?? 0;
            testCase.StaticPublicKeyServer = party.Scheme.StaticKeyPair?.PublicKeyY ?? 0;

            testCase.EphemeralPrivateKeyServer = party.Scheme.EphemeralKeyPair?.PrivateKeyX ?? 0;
            testCase.EphemeralPublicKeyServer = party.Scheme.EphemeralKeyPair?.PublicKeyY ?? 0;

            return Generate(@group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            return new TestCaseGenerateResponse(testCase);
        }
    }
}