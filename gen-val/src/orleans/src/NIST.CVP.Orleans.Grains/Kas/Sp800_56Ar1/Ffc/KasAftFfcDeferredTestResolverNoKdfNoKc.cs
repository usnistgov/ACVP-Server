﻿using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar1.Ffc
{
    internal class KasAftFfcDeferredTestResolverNoKdfNoKc : KasAftFfcDeferredTestResolverBase
    {
        public KasAftFfcDeferredTestResolverNoKdfNoKc(
            IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > kasBuilder,
            ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair
            > schemeBuilder,
            IMacParametersBuilder macParametersBuilder,
            IEntropyProviderFactory entropyProviderFactory)
            : base (kasBuilder, schemeBuilder, macParametersBuilder, entropyProviderFactory)
        {
        }

        protected override IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> GetServerKas(SchemeKeyNonceGenRequirement serverKeyRequirements, KeyAgreementRole serverRole, KeyConfirmationRole serverKcRole, MacParameters macParameters, KasAftDeferredParametersFfc param)
        {
            return _kasBuilder
                .WithKeyAgreementRole(serverKeyRequirements.ThisPartyKasRole)
                .WithPartyId(param.IdServer)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesFfc(
                    param.FfcScheme, param.FfcParameterSet
                ))
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithHashFunction(param.HashFunction)
                )
                .BuildNoKdfNoKc()
                .Build();
        }
    }
}