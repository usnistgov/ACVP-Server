using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.FFC.Helpers;
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

            var serverKas = _kasBuilder
                .WithAssurances(KasAssurance.None)
                .WithScheme(group.Scheme)
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithHashFunction(group.HashAlg)
                )
                .WithParameterSet(group.ParmSet)
                .WithPartyId(SpecificationMapping.ServerId)
                .WithKeyAgreementRole(serverRole)
                .BuildNoKdfNoKc()
                .Build();

            serverKas.SetDomainParameters(new FfcDomainParameters(group.P, group.Q, group.G));
            var serverPublicInfo = serverKas.ReturnPublicInfoThisParty();

            testCase.StaticPrivateKeyServer = serverKas.Scheme.StaticKeyPair?.PrivateKeyX ?? 0;
            testCase.StaticPublicKeyServer = serverKas.Scheme.StaticKeyPair?.PublicKeyY ?? 0;

            testCase.EphemeralPrivateKeyServer = serverKas.Scheme.EphemeralKeyPair?.PrivateKeyX ?? 0;
            testCase.EphemeralPublicKeyServer = serverKas.Scheme.EphemeralKeyPair?.PublicKeyY ?? 0;

            // For sample, we need to generate everything up front so that something's available
            // in the answer files
            if (isSample)
            {
                testCase.Deferred = false;

                var iutKas = _kasBuilder
                    .WithAssurances(KasAssurance.None)
                    .WithScheme(group.Scheme)
                    .WithSchemeBuilder(
                        _schemeBuilder
                            .WithHashFunction(group.HashAlg)
                    )
                    .WithParameterSet(group.ParmSet)
                    .WithPartyId(SpecificationMapping.IutId)
                    .WithKeyAgreementRole(group.KasRole)
                    .BuildNoKdfNoKc()
                    .Build();

                var result = iutKas.ComputeResult(serverPublicInfo);

                TestCaseDispositionHelper.SetTestCaseInformationFromKasResults(group, testCase, serverKas, iutKas, result);
            }

            return Generate(@group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            return new TestCaseGenerateResponse(testCase);
        }
    }
}