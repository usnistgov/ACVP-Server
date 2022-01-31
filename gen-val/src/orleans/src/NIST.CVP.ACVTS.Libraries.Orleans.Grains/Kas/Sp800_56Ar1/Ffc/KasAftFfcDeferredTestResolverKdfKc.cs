using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Fakes;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1.Ffc
{
    internal class KasAftFfcDeferredTestResolverKdfKc : KasAftFfcDeferredTestResolverBase
    {
        public KasAftFfcDeferredTestResolverKdfKc(
            IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > kasBuilder,
            ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > schemeBuilder,
            IMacParametersBuilder macParametersBuilder,
            IEntropyProviderFactory entropyProviderFactory)
            : base(kasBuilder, schemeBuilder, macParametersBuilder, entropyProviderFactory)
        {
        }

        protected override IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> GetServerKas(SchemeKeyNonceGenRequirement serverKeyRequirements, KeyAgreementRole serverRole, KeyConfirmationRole serverKcRole, MacParameters macParameters, KasAftDeferredParametersFfc param)
        {
            return _kasBuilder
                .WithKeyAgreementRole(serverKeyRequirements.ThisPartyKasRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesFfc(
                    param.FfcScheme, param.FfcParameterSet
                ))
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithOtherInfoFactory(
                            new FakeOtherInfoFactory(param.OtherInfo)
                        )
                        .WithHashFunction(param.HashFunction)
                )
                .WithKeyAgreementRole(serverRole)
                .WithPartyId(param.IdServer)
                .BuildKdfKc()
                .WithKeyLength(param.KeyLen)
                .WithMacParameters(macParameters)
                .WithOtherInfoPattern(param.OiPattern)
                .WithKeyLength(param.KeyLen)
                .WithKeyConfirmationRole(serverKcRole)
                .WithKeyConfirmationDirection(param.KeyConfirmationDirection)
                .Build();
        }
    }
}
