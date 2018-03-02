using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Generation.KAS.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseGeneratorValKdfKc : TestCaseGeneratorValBaseFfc
    {
        
        public TestCaseGeneratorValKdfKc(
            IKasBuilder<
                KasDsaAlgoAttributesFfc, 
                OtherPartySharedInformation<
                    FfcDomainParameters, 
                    FfcKeyPair
                >, 
                FfcDomainParameters, 
                FfcKeyPair
            > kasBuilder,
            ISchemeBuilder<
                KasDsaAlgoAttributesFfc, 
                OtherPartySharedInformation<
                    FfcDomainParameters, 
                    FfcKeyPair
                >, 
                FfcDomainParameters, 
                FfcKeyPair
            > schemeBuilder,
            IShaFactory shaFactory,
            IEntropyProviderFactory entropyProviderFactory,
            IMacParametersBuilder macParametersBuilder,
            IKdfFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            List<TestCaseDispositionOption> dispositionList
        )
            : base(
                  kasBuilder, 
                  schemeBuilder, 
                  shaFactory, 
                  entropyProviderFactory, 
                  macParametersBuilder, 
                  kdfFactory, 
                  keyConfirmationFactory, 
                  noKeyConfirmationFactory,
                  dispositionList
              ) { }

        protected override IKas<
            KasDsaAlgoAttributesFfc, 
            OtherPartySharedInformation<
                FfcDomainParameters, 
                FfcKeyPair
            >, 
            FfcDomainParameters, 
            FfcKeyPair
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