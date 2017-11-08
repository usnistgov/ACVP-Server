using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.FFC.Fakes;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    internal class DeferredTestCaseResolverAftKdfKc : IDeferredTestCaseResolver<TestGroup, TestCase, KasResult>
    {
        private readonly IKasBuilder _kasBuilder;
        private readonly IMacParametersBuilder _macParametersBuilder;
        private readonly ISchemeBuilder _schemeBuilder;
        private readonly IEntropyProviderFactory _entropyProviderFactory;

        public DeferredTestCaseResolverAftKdfKc(IKasBuilder kasBuilder, IMacParametersBuilder macParametersBuilder, ISchemeBuilder schemeBuilder, IEntropyProviderFactory entropyProviderFactory)
        {
            _kasBuilder = kasBuilder;
            _macParametersBuilder = macParametersBuilder;
            _schemeBuilder = schemeBuilder;
            _entropyProviderFactory = entropyProviderFactory;
        }

        public KasResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            KeyAgreementRole serverRole =
                KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(testGroup.KasRole);

            var serverKeyRequirements =
                KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                    testGroup.Scheme,
                    testGroup.KasMode,
                    serverRole,
                    testGroup.KcRole,
                    testGroup.KcType
                );

            FfcDomainParameters domainParameters = new FfcDomainParameters(testGroup.P, testGroup.Q, testGroup.G);
            FfcSharedInformation iutPublicInfo = new FfcSharedInformation(
                domainParameters,
                iutTestCase.IdIut ?? testGroup.IdIut,
                iutTestCase.StaticPublicKeyIut,
                iutTestCase.EphemeralPublicKeyIut,
                iutTestCase.EphemeralNonceIut,
                null,
                null
            );

            var macParameters = _macParametersBuilder
                .WithKeyAgreementMacType(testGroup.MacType)
                .WithMacLength(testGroup.MacLen)
                .WithNonce(iutTestCase.NonceAesCcm ?? serverTestCase.NonceAesCcm)
                .Build();

            if (serverKeyRequirements.GeneratesEphemeralNonce)
            {
                var entropyProvider = _entropyProviderFactory
                    .GetEntropyProvider(EntropyProviderTypes.Testable);
                entropyProvider.AddEntropy(serverTestCase.EphemeralNonceServer);

                _schemeBuilder.WithEntropyProvider(entropyProvider);
            }

            var serverKas = _kasBuilder
                .WithKeyAgreementRole(
                    serverKeyRequirements.ThisPartyKasRole
                )
                .WithParameterSet(testGroup.ParmSet)
                .WithScheme(testGroup.Scheme)
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithOtherInfoFactory(new FakeOtherInfoFactory(iutTestCase.OtherInfo))
                        .WithHashFunction(testGroup.HashAlg)
                )
                .WithKeyAgreementRole(serverRole)
                .WithPartyId(testGroup.IdServer)
                .BuildKdfNoKc()
                .WithKeyLength(testGroup.KeyLen)
                .WithMacParameters(macParameters)
                .WithOtherInfoPattern(testGroup.OiPattern)
                .Build();

            serverKas.SetDomainParameters(domainParameters);
            serverKas.ReturnPublicInfoThisParty();

            if (serverKeyRequirements.GeneratesStaticKeyPair)
            {
                serverKas.Scheme.StaticKeyPair.PrivateKeyX = serverTestCase.StaticPrivateKeyServer;
                serverKas.Scheme.StaticKeyPair.PublicKeyY = serverTestCase.StaticPublicKeyServer;
            }

            if (serverKeyRequirements.GeneratesEphemeralKeyPair)
            {
                serverKas.Scheme.EphemeralKeyPair.PrivateKeyX = serverTestCase.EphemeralPrivateKeyServer;
                serverKas.Scheme.EphemeralKeyPair.PublicKeyY = serverTestCase.EphemeralPublicKeyServer;
            }

            var serverResult = serverKas.ComputeResult(iutPublicInfo);
            return serverResult;
        }
    }
}