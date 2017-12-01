using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KAS.Scheme.Ffc;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.Fakes;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class DeferredTestCaseResolverAftKdfKc 
        : DeferredTestCaseResolverBaseFfc
    {
        
        public DeferredTestCaseResolverAftKdfKc(
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
                .BuildKdfKc()
                .WithKeyLength(testGroup.KeyLen)
                .WithMacParameters(macParameters)
                .WithOtherInfoPattern(testGroup.OiPattern)
                .WithKeyLength(testGroup.KeyLen)
                .WithKeyConfirmationRole(serverKcRole)
                .WithKeyConfirmationDirection(testGroup.KcType)
                .Build();
        }
    }
}