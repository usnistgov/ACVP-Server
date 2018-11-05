using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Kas.Ffc.Helpers;

namespace NIST.CVP.Orleans.Grains.Kas.Ffc
{
    public abstract class KasAftFfcTestGeneratorBase : KasAftTestGeneratorBase<
        KasAftParametersFfc, KasAftResultFfc,
        KasDsaAlgoAttributesFfc, FfcDomainParameters, FfcKeyPair, FfcScheme>
    {
        protected KasAftFfcTestGeneratorBase(
            IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > kasBuilder, 
            ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > schemeBuilder,
            IEntropyProviderFactory entropyProviderFactory,
            IMacParametersBuilder macParametersBuilder
        ) : base(kasBuilder, schemeBuilder, entropyProviderFactory, macParametersBuilder)
        {
        }

        protected override int GetDkmLengthRequirement(KasAftParametersFfc param)
        {
            return ParameterSetDetails.GetDetailsForFfcParameterSet(param.FfcParameterSet).qLength / 2;
        }

        protected override int GetEphemeralLengthRequirement(KasAftParametersFfc param)
        {
            return ParameterSetDetails.GetDetailsForFfcParameterSet(param.FfcParameterSet).pLength;
        }

        protected override FfcDomainParameters GetGroupDomainParameters(KasAftParametersFfc param)
        {
            return new FfcDomainParameters(param.P, param.Q, param.G);
        }

        protected override SchemeKeyNonceGenRequirement<FfcScheme> GetPartyNonceKeyGenRequirements(KasAftParametersFfc param, KeyAgreementRole partyKeyAgreementRole, KeyConfirmationRole partyKeyConfirmationRole)
        {
            return KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                param.FfcScheme,
                param.KasMode,
                partyKeyAgreementRole,
                partyKeyConfirmationRole,
                param.KeyConfirmationDirection
            );
        }

        protected override void SetTestResultInformationFromKasResult(KasAftParametersFfc param, KasAftResultFfc result, IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> serverKas, IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> iutKas, KasResult iutResult)
        {
            TestDispositionHelperFfc.SetResultInformationFromKasProcessing(
                param,
                result,
                serverKas,
                iutKas,
                iutResult
            );
        }
    }
}