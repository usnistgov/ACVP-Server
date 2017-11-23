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
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class DeferredTestCaseResolverAftKdfNoKc
        : DeferredTestCaseResolverBaseFfc
    {
        
        public DeferredTestCaseResolverAftKdfNoKc(
            IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> kasBuilder, 
            IMacParametersBuilder macParametersBuilder, 
            ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> schemeBuilder, 
            IEntropyProviderFactory entropyProviderFactory
        ) : base(kasBuilder, macParametersBuilder, schemeBuilder, entropyProviderFactory) { }

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
                .WithKeyAgreementRole(
                    serverKeyRequirements.ThisPartyKasRole
                )
                .WithKasDsaAlgoAttributes(testGroup.KasDsaAlgoAttributes)
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithOtherInfoFactory(
                            new FakeOtherInfoFactory<
                                OtherPartySharedInformation<
                                    FfcDomainParameters,
                                    FfcKeyPair
                                >,
                                FfcDomainParameters,
                                FfcKeyPair
                            >(iutTestCase.OtherInfo)
                        )
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