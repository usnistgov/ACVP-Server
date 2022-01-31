using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1.Ecc.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1.Ecc
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
            return curveAttributes.DegreeOfPolynomial.ValueToMod(BitString.BITSINBYTE);
        }

        protected override int GetEphemeralLengthRequirement(KasAftParametersEcc param)
        {
            var curveAttributes = CurveAttributesHelper.GetCurveAttribute(param.Curve);
            return curveAttributes.DegreeOfPolynomial.ValueToMod(BitString.BITSINBYTE);
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
