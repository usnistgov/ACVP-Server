using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class DeferredTestCaseResolverAftNoKdfNoKc : IDeferredTestCaseResolver<TestGroup, TestCase, KasResult>
    {
        private readonly IKasBuilder _kasBuilder;

        public DeferredTestCaseResolverAftNoKdfNoKc(IKasBuilder kasBuilder)
        {
            _kasBuilder = kasBuilder;
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
                iutTestCase.IdIut ?? testGroup.IdIut,
                iutTestCase.StaticPublicKeyIut,
                iutTestCase.EphemeralPublicKeyIut,
                null,
                null,
                null
            );

            var serverKas = _kasBuilder
                .WithKeyAgreementRole(
                    serverKeyRequirements.thisPartyKasRole
                )
                .WithPartyId(testGroup.IdServer)
                .WithParameterSet(testGroup.ParmSet)
                .WithScheme(testGroup.Scheme)
                .BuildNoKdfNoKc()
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