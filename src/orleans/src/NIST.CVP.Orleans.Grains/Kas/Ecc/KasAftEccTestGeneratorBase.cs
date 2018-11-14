using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Helpers;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Kas.Ecc.Helpers;

namespace NIST.CVP.Orleans.Grains.Kas.Ecc
{
    public abstract class KasAftEccTestGeneratorBase : KasAftTestGeneratorBase<
        KasAftParametersEcc, KasAftResultEcc,
        KasDsaAlgoAttributesEcc, EccDomainParameters, EccKeyPair, EccScheme>
    {
        private readonly IEccCurveFactory _curveFactory;

        protected KasAftEccTestGeneratorBase(
            IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair
            > kasBuilder, 
            ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair
            > schemeBuilder,
            IEntropyProviderFactory entropyProviderFactory,
            IMacParametersBuilder macParametersBuilder,
            IEccCurveFactory curveFactory
        ) : base(kasBuilder, schemeBuilder, entropyProviderFactory, macParametersBuilder)
        {
            _curveFactory = curveFactory;
        }

        protected override int GetDkmLengthRequirement(KasAftParametersEcc param)
        {
            var curveAttributes = CurveAttributesHelper.GetCurveAttribute(param.Curve);
            return curveAttributes.LengthN / 2;
        }

        protected override int GetEphemeralLengthRequirement(KasAftParametersEcc param)
        {
            var curveAttributes = CurveAttributesHelper.GetCurveAttribute(param.Curve);
            return curveAttributes.LengthN * 2;
        }

        protected override EccDomainParameters GetGroupDomainParameters(KasAftParametersEcc param)
        {
            return new EccDomainParameters(_curveFactory.GetCurve(param.Curve));
        }

        protected override SchemeKeyNonceGenRequirement GetPartyNonceKeyGenRequirements(KasAftParametersEcc param, KeyAgreementRole partyKeyAgreementRole, KeyConfirmationRole partyKeyConfirmationRole)
        {
            return KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                param.EccScheme,
                param.KasMode,
                partyKeyAgreementRole,
                partyKeyConfirmationRole,
                param.KeyConfirmationDirection
            );
        }

        protected override void SetTestResultInformationFromKasResult(KasAftParametersEcc param, KasAftResultEcc result, IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> serverKas, IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> iutKas, KasResult iutResult)
        {
            TestDispositionHelperEcc.SetResultInformationFromKasProcessing(
                param,
                result,
                serverKas,
                iutKas,
                iutResult
            );
        }
    }
}