using System.Collections.Generic;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KAS.Scheme.Ffc;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.Enums;
using NIST.CVP.Generation.KAS.FFC.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseGeneratorValNoKdfNoKc : TestCaseGeneratorValBaseFfc
    {

        public TestCaseGeneratorValNoKdfNoKc(
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
             )
        { }

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
                )
                .BuildNoKdfNoKc()
                .Build();
        }
    }
}