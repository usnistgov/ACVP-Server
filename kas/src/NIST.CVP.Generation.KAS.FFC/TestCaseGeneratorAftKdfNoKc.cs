using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.FFC.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseGeneratorAftKdfNoKc : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IKasBuilder _kasBuilder;
        private readonly ISchemeBuilder _schemeBuilder;
        private readonly IDsaFfcFactory _dsaFactory;
        private readonly IShaFactory _shaFactory;
        private readonly IEntropyProviderFactory _entropyProviderFactory;
        private readonly IMacParametersBuilder _macParametersBuilder;

        public TestCaseGeneratorAftKdfNoKc(IKasBuilder kasBuilder, ISchemeBuilder schemeBuilder, IDsaFfcFactory dsaFactory, IShaFactory shaFactory, IEntropyProviderFactory entropyProviderFactory, IMacParametersBuilder macParametersBuilder)
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _dsaFactory = dsaFactory;
            _shaFactory = shaFactory;
            _entropyProviderFactory = entropyProviderFactory;
            _macParametersBuilder = macParametersBuilder;
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

            testCase.NonceNoKc = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random).GetEntropy(128);
            var noKcEntropyProvider = _entropyProviderFactory
                .GetEntropyProvider(EntropyProviderTypes.Testable);
            noKcEntropyProvider.AddEntropy(testCase.NonceNoKc.GetDeepCopy());

            BitString aesCcmNonce = null;
            if ((serverRole == KeyAgreementRole.InitiatorPartyU && group.MacType == KeyAgreementMacType.AesCcm) || isSample)
            {
                aesCcmNonce = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                    .GetEntropy(group.AesCcmNonceLen);

                testCase.NonceAesCcm = aesCcmNonce;
            }

            MacParameters macParameters = _macParametersBuilder
                .WithKeyAgreementMacType(group.MacType)
                .WithMacLength(group.MacLen)
                .WithNonce(aesCcmNonce)
                .Build();

            var serverKas = _kasBuilder
                .WithAssurances(KasAssurance.None)
                .WithScheme(group.Scheme)
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithHashFunction(group.HashAlg)
                        .WithEntropyProvider(noKcEntropyProvider)
                )
                .WithParameterSet(group.ParmSet)
                .WithPartyId(SpecificationMapping.ServerId)
                .WithKeyAgreementRole(serverRole)
                .BuildKdfNoKc()
                .WithKeyLength(group.KeyLen)
                .WithMacParameters(
                    macParameters
                )
                .WithOtherInfoPattern(group.OiPattern)
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

                if (group.AesCcmNonceLen != 0)
                {
                    testCase.NonceAesCcm = macParameters.CcmNonce.GetDeepCopy();
                }
                testCase.IdIut = SpecificationMapping.IutId;
                testCase.IdIutLen = testCase.IdIut.BitLength;

                noKcEntropyProvider = _entropyProviderFactory
                    .GetEntropyProvider(EntropyProviderTypes.Testable);
                noKcEntropyProvider.AddEntropy(testCase.NonceNoKc.GetDeepCopy());

                var iutKas = _kasBuilder
                    .WithAssurances(KasAssurance.None)
                    .WithScheme(group.Scheme)
                    .WithSchemeBuilder(
                        _schemeBuilder
                            .WithHashFunction(group.HashAlg)
                            .WithEntropyProvider(noKcEntropyProvider)
                    )
                    .WithParameterSet(group.ParmSet)
                    .WithPartyId(testCase.IdIut)
                    .WithKeyAgreementRole(group.KasRole)
                    .BuildKdfNoKc()
                    .WithKeyLength(group.KeyLen)
                    .WithOtherInfoPattern(group.OiPattern)
                    .WithMacParameters(macParameters)
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