using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1.Ffc
{
    public abstract class KasAftFfcDeferredTestResolverBase : KasAftDeferredTestResolverBase<
        KasAftDeferredParametersFfc, KasAftDeferredResult,
        KasDsaAlgoAttributesFfc, FfcDomainParameters, FfcKeyPair, FfcScheme>
    {
        protected KasAftFfcDeferredTestResolverBase(
            IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > kasBuilder,
            ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > schemeBuilder,
            IMacParametersBuilder macParametersBuilder,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(kasBuilder, macParametersBuilder, schemeBuilder, entropyProviderFactory)
        {
        }

        protected override FfcDomainParameters GetDomainParameters(KasAftDeferredParametersFfc param)
        {
            return new FfcDomainParameters(param.P, param.Q, param.G);
        }

        protected override OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair> GetIutSharedInformation(KasAftDeferredParametersFfc param, FfcDomainParameters domainParameters)
        {
            return new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(
                domainParameters,
                param.IdIut,
                new FfcKeyPair(param.StaticPublicKeyIut),
                new FfcKeyPair(param.EphemeralPublicKeyIut),
                param.DkmNonceIut,
                param.EphemeralNonceIut,
                param.NonceNoKc
            );
        }

        protected override SchemeKeyNonceGenRequirement GetServerNonceKeyGenRequirements(KasAftDeferredParametersFfc param, KeyAgreementRole serverKeyAgreementRole, KeyConfirmationRole serverKeyConfirmationRole)
        {
            return KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                param.FfcScheme,
                param.KasMode,
                serverKeyAgreementRole,
                serverKeyConfirmationRole,
                param.KeyConfirmationDirection
            );
        }

        protected override void UpdateKasInstanceWithTestCaseInformation(IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> serverKas, SchemeKeyNonceGenRequirement serverKeyRequirements, KasAftDeferredParametersFfc param)
        {
            if (serverKeyRequirements.GeneratesStaticKeyPair)
            {
                serverKas.Scheme.StaticKeyPair.PrivateKeyX = param.StaticPrivateKeyServer;
                serverKas.Scheme.StaticKeyPair.PublicKeyY = param.StaticPublicKeyServer;
            }

            if (serverKeyRequirements.GeneratesEphemeralKeyPair)
            {
                serverKas.Scheme.EphemeralKeyPair.PrivateKeyX = param.EphemeralPrivateKeyServer;
                serverKas.Scheme.EphemeralKeyPair.PublicKeyY = param.EphemeralPublicKeyServer;
            }
        }
    }
}
