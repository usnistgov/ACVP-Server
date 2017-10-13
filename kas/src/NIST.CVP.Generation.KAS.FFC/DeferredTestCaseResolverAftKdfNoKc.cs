using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.FFC.Fakes;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class DeferredTestCaseResolverAftKdfNoKc : IDeferredTestCaseResolver<TestGroup, TestCase, KasResult>
    {
        private readonly IKasBuilder _kasBuilder;
        private readonly IMacParametersBuilder _macParametersBuilder;
        private readonly ISchemeBuilder _schemeBuilder;
        private readonly IEntropyProviderFactory _entropyProviderFactory;
        
        public DeferredTestCaseResolverAftKdfNoKc(IKasBuilder kasBuilder, IMacParametersBuilder macParametersBuilder, ISchemeBuilder schemeBuilder, IEntropyProviderFactory entropyProviderFactory)
        {
            _kasBuilder = kasBuilder;
            _macParametersBuilder = macParametersBuilder;
            _schemeBuilder = schemeBuilder;
            _entropyProviderFactory = entropyProviderFactory;
        }

        public KasResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            (FfcScheme scheme, KeyAgreementRole thisPartyKasRole, bool generatesStaticKeyPair, bool
                generatesEphemeralKeyPair) serverKeyRequirements =
                    SpecificationMapping.GetKeyGenerationOptionsForSchemeAndRole(testGroup.Scheme,
                        testGroup.KasRole == KeyAgreementRole.InitiatorPartyU
                            ? KeyAgreementRole.ResponderPartyV
                            : KeyAgreementRole.InitiatorPartyU);

            FfcDomainParameters domainParameters = new FfcDomainParameters(testGroup.P, testGroup.Q, testGroup.G);
            FfcSharedInformation iutPublicInfo = new FfcSharedInformation(
                domainParameters,
                iutTestCase.IdIut,
                iutTestCase.StaticPublicKeyServer,
                iutTestCase.EphemeralPublicKeyServer,
                null,
                null,
                serverTestCase.NonceNoKc
            );

            var macParameters = _macParametersBuilder
                .WithKeyAgreementMacType(testGroup.MacType)
                .WithMacLength(testGroup.MacLen)
                .WithNonce(iutTestCase.NonceAesCcm)
                .Build();

            KeyAgreementRole serverRole = testGroup.KasRole == KeyAgreementRole.InitiatorPartyU
                ? KeyAgreementRole.ResponderPartyV
                : KeyAgreementRole.InitiatorPartyU;

            // inject specific entropy for nonceNoKc when the server is the initiator
            // when the server is not the initiator, nonceNoKc is provided via the other party's public info
            if (serverRole == KeyAgreementRole.InitiatorPartyU)
            {
                var entropyProvider = _entropyProviderFactory
                    .GetEntropyProvider(EntropyProviderTypes.Testable);
                entropyProvider.AddEntropy(serverTestCase.NonceNoKc);

                _schemeBuilder.WithEntropyProvider(entropyProvider);
            }

            var serverKas = _kasBuilder
                .WithKeyAgreementRole(
                    serverKeyRequirements.thisPartyKasRole
                )
                .WithParameterSet(testGroup.ParmSet)
                .WithScheme(testGroup.Scheme)
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithOtherInfoFactory(new FakeOtherInfoFactory(iutTestCase.OtherInfo))
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

            if (serverKeyRequirements.generatesStaticKeyPair)
            {
                serverKas.Scheme.StaticKeyPair.PrivateKeyX = serverTestCase.StaticPrivateKeyServer;
                serverKas.Scheme.StaticKeyPair.PublicKeyY = serverTestCase.StaticPublicKeyServer;
            }

            if (serverKeyRequirements.generatesEphemeralKeyPair)
            {
                serverKas.Scheme.EphemeralKeyPair.PrivateKeyX = serverTestCase.EphemeralPrivateKeyServer;
                serverKas.Scheme.EphemeralKeyPair.PublicKeyY = serverTestCase.EphemeralPublicKeyServer;
            }

            var serverResult = serverKas.ComputeResult(iutPublicInfo);
            return serverResult;
        }
    }
}