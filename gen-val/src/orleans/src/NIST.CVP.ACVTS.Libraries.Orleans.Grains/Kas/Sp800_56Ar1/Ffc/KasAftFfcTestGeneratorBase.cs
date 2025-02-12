﻿using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1.Ffc.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1.Ffc
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

        protected override SchemeKeyNonceGenRequirement GetPartyNonceKeyGenRequirements(KasAftParametersFfc param, KeyAgreementRole partyKeyAgreementRole, KeyConfirmationRole partyKeyConfirmationRole)
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
