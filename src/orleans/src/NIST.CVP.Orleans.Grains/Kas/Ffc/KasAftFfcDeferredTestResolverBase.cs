using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Kas.Ffc
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