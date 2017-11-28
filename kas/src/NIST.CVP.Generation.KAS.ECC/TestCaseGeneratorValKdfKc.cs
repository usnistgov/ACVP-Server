using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KAS.Scheme.Ecc;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.KAS.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.ECC
{
    public class TestCaseGeneratorValKdfKc : TestCaseGeneratorValBaseEcc
    {
        public TestCaseGeneratorValKdfKc(
            IEccCurveFactory curveFactory, 
            IKasBuilder<
                KasDsaAlgoAttributesEcc, 
                OtherPartySharedInformation<
                    EccDomainParameters, 
                    EccKeyPair
                >, 
                EccDomainParameters, 
                EccKeyPair
            > kasBuilder, 
            ISchemeBuilder<
                KasDsaAlgoAttributesEcc, 
                OtherPartySharedInformation<
                    EccDomainParameters, 
                    EccKeyPair
                >, 
                EccDomainParameters, 
                EccKeyPair
            > schemeBuilder, 
            IShaFactory shaFactory, 
            IEntropyProviderFactory entropyProviderFactory, 
            IMacParametersBuilder macParametersBuilder, 
            IKdfFactory kdfFactory, 
            IKeyConfirmationFactory keyConfirmationFactory, 
            INoKeyConfirmationFactory noKeyConfirmationFactory, 
            TestCaseDispositionOption intendedDisposition
        ) : base(
            curveFactory, 
            kasBuilder, 
            schemeBuilder, 
            shaFactory, 
            entropyProviderFactory, 
            macParametersBuilder, 
            kdfFactory, 
            keyConfirmationFactory, 
            noKeyConfirmationFactory, 
            intendedDisposition
        ) { }

        protected override IKas<
            KasDsaAlgoAttributesEcc, 
            OtherPartySharedInformation<
                EccDomainParameters, 
                EccKeyPair
            >, 
            EccDomainParameters, 
            EccKeyPair
        > GetKasInstance(
            KeyAgreementRole partyRole, 
            KeyConfirmationRole partyKcRole, 
            MacParameters macParameters,
            TestGroup @group, 
            TestCase testCase, 
            BitString partyId, 
            IKdfFactory kdfFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory, 
            IKeyConfirmationFactory keyConfirmationFactory
        )
        {
            return _kasBuilder
                .WithPartyId(partyId)
                .WithKeyAgreementRole(partyRole)
                .WithKasDsaAlgoAttributes(group.KasDsaAlgoAttributes)
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithHashFunction(group.HashAlg)
                        .WithKdfFactory(kdfFactory)
                        .WithKeyConfirmationFactory(keyConfirmationFactory)
                )
                .BuildKdfKc()
                .WithKeyLength(group.KeyLen)
                .WithMacParameters(macParameters)
                .WithOtherInfoPattern(group.OiPattern)
                .WithKeyConfirmationRole(partyKcRole)
                .WithKeyConfirmationDirection(group.KcType)
                .Build();
        }
    }
}