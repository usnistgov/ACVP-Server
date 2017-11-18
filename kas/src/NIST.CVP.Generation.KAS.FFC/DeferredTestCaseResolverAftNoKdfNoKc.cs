using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class DeferredTestCaseResolverAftNoKdfNoKc : IDeferredTestCaseResolver<TestGroup, TestCase, KasResult>
    {
        private readonly IKasBuilder<FfcParameterSet, FfcScheme, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> _kasBuilder;
        private readonly ISchemeBuilder<FfcParameterSet, FfcScheme, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> _schemeBuilder;

        public DeferredTestCaseResolverAftNoKdfNoKc(
            IKasBuilder<FfcParameterSet, FfcScheme, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> kasBuilder, 
            ISchemeBuilder<FfcParameterSet, FfcScheme, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> schemeBuilder)
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
        }

        public KasResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            KeyAgreementRole serverRole =
                KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(testGroup.KasRole);

            var serverKeyRequirements =
                    KeyGenerationRequirementsHelper.GetFfcKeyGenerationOptionsForSchemeAndRole(
                        testGroup.Scheme,
                        testGroup.KasMode,
                        serverRole,
                        testGroup.KcRole,
                        testGroup.KcType
                    );

            FfcDomainParameters domainParameters = new FfcDomainParameters(testGroup.P, testGroup.Q, testGroup.G);
            OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair> iutPublicInfo = 
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(
                    domainParameters,
                    iutTestCase.IdIut ?? testGroup.IdIut,
                    new FfcKeyPair(iutTestCase.StaticPublicKeyIut),
                    new FfcKeyPair(iutTestCase.EphemeralPublicKeyIut),
                    null,
                    null,
                    null
                );

            var serverKas = _kasBuilder
                .WithKeyAgreementRole(
                    serverKeyRequirements.ThisPartyKasRole
                )
                .WithPartyId(testGroup.IdServer)
                .WithParameterSet(testGroup.ParmSet)
                .WithScheme(testGroup.Scheme)
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithHashFunction(testGroup.HashAlg)
                )
                .BuildNoKdfNoKc()
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