using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Kas.Ecc
{
    public abstract class KasAftEccDeferredTestResolverBase : KasAftDeferredTestResolverBase<
        KasAftDeferredParametersEcc, KasAftDeferredResult,
        KasDsaAlgoAttributesEcc, EccDomainParameters, EccKeyPair, EccScheme>
    {
        protected readonly IEccCurveFactory CurveFactory;

        protected KasAftEccDeferredTestResolverBase(
            IEccCurveFactory curveFactory,
            IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair
            > kasBuilder,
            ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair
            > schemeBuilder,
            IMacParametersBuilder macParametersBuilder,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(kasBuilder, macParametersBuilder, schemeBuilder, entropyProviderFactory)
        {
            CurveFactory = curveFactory;
        }

        protected override EccDomainParameters GetDomainParameters(KasAftDeferredParametersEcc param)
        {
            return new EccDomainParameters(CurveFactory.GetCurve(param.Curve));
        }

        protected override OtherPartySharedInformation<EccDomainParameters, EccKeyPair> GetIutSharedInformation(KasAftDeferredParametersEcc param, EccDomainParameters domainParameters)
        {
            return new OtherPartySharedInformation<EccDomainParameters, EccKeyPair>(
                domainParameters,
                param.IdIut,
                new EccKeyPair(new EccPoint(param.StaticPublicKeyIutX, param.StaticPublicKeyIutY)),
                new EccKeyPair(new EccPoint(param.EphemeralPublicKeyIutX, param.EphemeralPublicKeyIutY)),
                param.DkmNonceIut,
                param.EphemeralNonceIut,
                param.NonceNoKc
            );
        }
        
        protected override SchemeKeyNonceGenRequirement GetServerNonceKeyGenRequirements(KasAftDeferredParametersEcc param, KeyAgreementRole serverKeyAgreementRole, KeyConfirmationRole serverKeyConfirmationRole)
        {
            return KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                param.EccScheme,
                param.KasMode,
                serverKeyAgreementRole,
                serverKeyConfirmationRole,
                param.KeyConfirmationDirection
            );
        }

        protected override void UpdateKasInstanceWithTestCaseInformation(IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> serverKas, SchemeKeyNonceGenRequirement serverKeyRequirements, KasAftDeferredParametersEcc param)
        {
            if (serverKeyRequirements.GeneratesStaticKeyPair)
            {
                serverKas.Scheme.StaticKeyPair.PrivateD = param.StaticPrivateKeyServer;
                serverKas.Scheme.StaticKeyPair.PublicQ = new EccPoint(
                    param.StaticPublicKeyServerX,
                    param.StaticPublicKeyServerY
                );
            }

            if (serverKeyRequirements.GeneratesEphemeralKeyPair)
            {
                serverKas.Scheme.EphemeralKeyPair.PrivateD = param.EphemeralPrivateKeyServer;
                serverKas.Scheme.EphemeralKeyPair.PublicQ = new EccPoint(
                    param.EphemeralPublicKeyServerX,
                    param.EphemeralPublicKeyServerY
                );
            }
        }
    }
}