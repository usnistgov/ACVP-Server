using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Fakes;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Kas.Ecc
{
    internal class KasAftEccDeferredTestResolverKdfKc : KasAftEccDeferredTestResolverBase
    {
        public KasAftEccDeferredTestResolverKdfKc(
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

        protected override IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> GetServerKas(SchemeKeyNonceGenRequirement<EccScheme> serverKeyRequirements, KeyAgreementRole serverRole, KeyConfirmationRole serverKcRole, MacParameters macParameters, KasAftDeferredParametersEcc param)
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