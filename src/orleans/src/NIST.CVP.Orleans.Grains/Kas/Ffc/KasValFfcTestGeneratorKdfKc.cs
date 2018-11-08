using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.KAS.Builders.Ffc;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Kas.Ffc
{
    internal class KasValFfcTestGeneratorKdfKc : KasValFfcTestGeneratorBase
    {
        public KasValFfcTestGeneratorKdfKc(
            IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > kasBuilder, 
            ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > schemeBuilder,
            IEntropyProviderFactory entropyProviderFactory,
            IMacParametersBuilder macParametersBuilder,
            IKdfFactory kdfFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            IShaFactory shaFactory
        ) : base(kasBuilder, schemeBuilder, entropyProviderFactory, macParametersBuilder, kdfFactory, noKeyConfirmationFactory, keyConfirmationFactory, shaFactory)
        {
        }

        protected override IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> 
            GetKasInstance(KeyAgreementRole partyRole, KeyConfirmationRole partyKcRole, MacParameters macParameters, KasValParametersFfc param, KasValResultFfc result, BitString partyId, IKdfFactory kdfFactory, INoKeyConfirmationFactory noKeyConfirmationFactory, IKeyConfirmationFactory keyConfirmationFactory)
        {
            return new KasBuilderFfc(SchemeBuilder)
                .WithPartyId(partyId)
                .WithKeyAgreementRole(partyRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesFfc(
                    param.FfcScheme, param.FfcParameterSet
                ))
                .WithSchemeBuilder(
                    SchemeBuilder
                        .WithHashFunction(param.HashFunction)
                        .WithKdfFactory(kdfFactory)
                        .WithKeyConfirmationFactory(keyConfirmationFactory)
                )
                .BuildKdfKc()
                .WithKeyLength(param.KeyLen)
                .WithMacParameters(macParameters)
                .WithOtherInfoPattern(param.OiPattern)
                .WithKeyConfirmationRole(partyKcRole)
                .WithKeyConfirmationDirection(param.KeyConfirmationDirection)
                .Build();
        }
    }
}