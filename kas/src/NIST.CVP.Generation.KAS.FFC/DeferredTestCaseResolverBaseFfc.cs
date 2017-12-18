using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KAS.Scheme.Ffc;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public abstract class DeferredTestCaseResolverBaseFfc : DeferredTestCaseResolverBase<
        TestGroup,
        TestCase,
        KasDsaAlgoAttributesFfc,
        FfcDomainParameters,
        FfcKeyPair,
        FfcScheme
    >
    {

        protected DeferredTestCaseResolverBaseFfc(
            IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> kasBuilder,
            ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> schemeBuilder,
            IMacParametersBuilder macParametersBuilder,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(kasBuilder, schemeBuilder, macParametersBuilder, entropyProviderFactory) { }

        /// <inheritdoc />
        protected override SchemeKeyNonceGenRequirement<FfcScheme> GetServerNonceKeyGenRequirements(
            TestGroup testGroup,
            KeyAgreementRole serverKeyAgreementRole,
            KeyConfirmationRole serverKeyConfirmationRole)
        {
            return KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                testGroup.Scheme,
                testGroup.KasMode,
                serverKeyAgreementRole,
                serverKeyConfirmationRole,
                testGroup.KcType
            );
        }

        /// <inheritdoc />
        protected override FfcDomainParameters GetDomainParameters(TestGroup testGroup)
        {
            return new FfcDomainParameters(testGroup.P, testGroup.Q, testGroup.G);
        }

        /// <inheritdoc />
        protected override OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>
            GetIutSharedInformation(
                TestGroup testGroup,
                TestCase serverTestCase,
                TestCase iutTestCase,
                FfcDomainParameters domainParameters
            )
        {
            return new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(
                domainParameters,
                iutTestCase.IdIut ?? testGroup.IdIut,
                new FfcKeyPair(iutTestCase.StaticPublicKeyIut),
                new FfcKeyPair(iutTestCase.EphemeralPublicKeyIut),
                iutTestCase.DkmNonceIut,
                iutTestCase.EphemeralNonceIut,
                serverTestCase.NonceNoKc
            );
        }

        /// <inheritdoc />
        protected override void UpdateKasInstanceWithTestCaseInformation(
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> serverKas,
            SchemeKeyNonceGenRequirement<FfcScheme> serverKeyRequirements,
            TestCase serverTestCase
        )
        {
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
        }
    }
}