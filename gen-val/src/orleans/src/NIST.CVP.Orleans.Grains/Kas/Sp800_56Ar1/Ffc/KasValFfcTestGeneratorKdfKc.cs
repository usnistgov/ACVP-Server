﻿using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.KAS.Builders.Ffc;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar1.Ffc
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
            IKdfOneStepFactory kdfFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            IShaFactory shaFactory
        ) : base(kasBuilder, schemeBuilder, entropyProviderFactory, macParametersBuilder, kdfFactory, noKeyConfirmationFactory, keyConfirmationFactory, shaFactory)
        {
        }

        protected override IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> 
            GetKasInstance(KeyAgreementRole partyRole, KeyConfirmationRole partyKcRole, MacParameters macParameters, KasValParametersFfc param, KasValResultFfc result, BitString partyId, IKdfOneStepFactory kdfFactory, INoKeyConfirmationFactory noKeyConfirmationFactory, IKeyConfirmationFactory keyConfirmationFactory)
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