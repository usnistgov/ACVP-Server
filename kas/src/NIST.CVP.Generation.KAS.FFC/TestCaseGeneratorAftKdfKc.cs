using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.FFC.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseGeneratorAftKdfKc : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IKasBuilder _kasBuilder;
        private readonly ISchemeBuilder _schemeBuilder;
        private readonly IEntropyProviderFactory _entropyProviderFactory;
        private readonly IMacParametersBuilder _macParametersBuilder;

        public TestCaseGeneratorAftKdfKc(IKasBuilder kasBuilder, ISchemeBuilder schemeBuilder, IEntropyProviderFactory entropyProviderFactory, IMacParametersBuilder macParametersBuilder)
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
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

            KeyAgreementRole serverRole =
                KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(group.KasRole);
            KeyConfirmationRole serverKcRole =
                KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(group.KcRole);

            // TODO validate this can be done at below todo
            var serverKeyNonceRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                group.Scheme,
                group.KasMode,
                serverRole,
                serverKcRole,
                group.KcType
            );

            if (serverKeyNonceRequirements.GeneratesEphemeralNonce)
            {
                var parameterSetAttributes = FfcParameterSetDetails.GetDetailsForParameterSet(group.ParmSet);
                testCase.EphemeralNonceServer = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                    .GetEntropy(parameterSetAttributes.pLength);
                var ephemeralNonceEntropyProvider = _entropyProviderFactory
                    .GetEntropyProvider(EntropyProviderTypes.Testable);
                ephemeralNonceEntropyProvider.AddEntropy(testCase.EphemeralNonceServer.GetDeepCopy());
                _schemeBuilder.WithEntropyProvider(ephemeralNonceEntropyProvider);
            }

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
                )
                .WithParameterSet(group.ParmSet)
                .WithPartyId(SpecificationMapping.ServerId)
                .WithKeyAgreementRole(serverRole)
                .BuildKdfKc()
                .WithKeyLength(group.KeyLen)
                .WithMacParameters(
                    macParameters
                )
                .WithOtherInfoPattern(group.OiPattern)
                .WithKeyConfirmationRole(serverKcRole)
                .WithKeyConfirmationDirection(group.KcType)
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
                _schemeBuilder.WithEntropyProvider(
                    _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                );

                var iutKeyNonceRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                    group.Scheme,
                    group.KasMode,
                    group.KasRole,
                    group.KcRole,
                    group.KcType
                );

                if (iutKeyNonceRequirements.GeneratesEphemeralNonce)
                {
                    var parameterSetAttributes = FfcParameterSetDetails.GetDetailsForParameterSet(group.ParmSet);
                    testCase.EphemeralNonceIut = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)
                        .GetEntropy(parameterSetAttributes.pLength);
                    var ephemeralNonceEntropyProvider = _entropyProviderFactory
                        .GetEntropyProvider(EntropyProviderTypes.Testable);
                    ephemeralNonceEntropyProvider.AddEntropy(testCase.EphemeralNonceServer.GetDeepCopy());
                    _schemeBuilder.WithEntropyProvider(ephemeralNonceEntropyProvider);
                }

                if (group.AesCcmNonceLen != 0)
                {
                    testCase.NonceAesCcm = macParameters.CcmNonce.GetDeepCopy();
                }
                testCase.IdIut = SpecificationMapping.IutId;
                testCase.IdIutLen = testCase.IdIut.BitLength;

                var iutKas = _kasBuilder
                    .WithAssurances(KasAssurance.None)
                    .WithScheme(group.Scheme)
                    .WithSchemeBuilder(
                        _schemeBuilder
                            .WithHashFunction(group.HashAlg)
                    )
                    .WithParameterSet(group.ParmSet)
                    .WithPartyId(testCase.IdIut)
                    .WithKeyAgreementRole(group.KasRole)
                    .BuildKdfKc()
                    .WithKeyLength(group.KeyLen)
                    .WithOtherInfoPattern(group.OiPattern)
                    .WithMacParameters(macParameters)
                    .WithKeyConfirmationRole(group.KcRole)
                    .WithKeyConfirmationDirection(group.KcType)
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