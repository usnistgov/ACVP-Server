﻿using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.KAS.Fakes;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar1.Ecc
{
    internal class KasAftEccDeferredTestResolverKdfNoKc : KasAftEccDeferredTestResolverBase
    {
        public KasAftEccDeferredTestResolverKdfNoKc(
            IEccCurveFactory curveFactory,
            IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair
            > kasBuilder,
            ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair
            > schemeBuilder,
            IMacParametersBuilder macParametersBuilder,
            IEntropyProviderFactory entropyProviderFactory)
            : base (curveFactory, kasBuilder, schemeBuilder, macParametersBuilder, entropyProviderFactory)
        {
        }

        protected override IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> GetServerKas(SchemeKeyNonceGenRequirement serverKeyRequirements, KeyAgreementRole serverRole, KeyConfirmationRole serverKcRole, MacParameters macParameters, KasAftDeferredParametersEcc param)
        {
            return _kasBuilder
                .WithKeyAgreementRole(serverKeyRequirements.ThisPartyKasRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(
                    param.EccScheme, param.EccParameterSet, param.Curve
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
                .BuildKdfNoKc()
                .WithKeyLength(param.KeyLen)
                .WithMacParameters(macParameters)
                .WithOtherInfoPattern(param.OiPattern)
                .Build();
        }
    }
}