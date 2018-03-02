using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Generation.KAS.Fakes;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class DeferredTestCaseResolverAftKdfNoKc
        : DeferredTestCaseResolverBaseFfc
    {
        
        public DeferredTestCaseResolverAftKdfNoKc(
            IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> kasBuilder,
            ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> schemeBuilder,
            IMacParametersBuilder macParametersBuilder,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(kasBuilder, schemeBuilder, macParametersBuilder, entropyProviderFactory) { }

        /// <inheritdoc />
        protected override IKas<
            KasDsaAlgoAttributesFfc,
            OtherPartySharedInformation<
                FfcDomainParameters,
                FfcKeyPair
            >,
            FfcDomainParameters,
            FfcKeyPair
        > GetServerKas(
            SchemeKeyNonceGenRequirement<FfcScheme> serverKeyRequirements,
            KeyAgreementRole serverRole,
            KeyConfirmationRole serverKcRole,
            MacParameters macParameters,
            TestGroup testGroup,
            TestCase iutTestCase
        )
        {
            return _kasBuilder
                .WithKeyAgreementRole(serverKeyRequirements.ThisPartyKasRole)
                .WithKasDsaAlgoAttributes(testGroup.KasDsaAlgoAttributes)
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithOtherInfoFactory(new FakeOtherInfoFactory(iutTestCase.OtherInfo))
                        .WithHashFunction(testGroup.HashAlg)
                )
                .WithKeyAgreementRole(serverRole)
                .WithPartyId(testGroup.IdServer)
                .BuildKdfNoKc()
                .WithKeyLength(testGroup.KeyLen)
                .WithMacParameters(macParameters)
                .WithOtherInfoPattern(testGroup.OiPattern)
                .Build();
        }
    }
}