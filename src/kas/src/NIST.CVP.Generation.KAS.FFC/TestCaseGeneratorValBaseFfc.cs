using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Generation.KAS.Enums;
using NIST.CVP.Generation.KAS.FFC.Helpers;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public abstract class TestCaseGeneratorValBaseFfc : TestCaseGeneratorValBase<
        TestGroup,
        TestCase,
        KasDsaAlgoAttributesFfc,
        FfcDomainParameters,
        FfcKeyPair
    >
    {
        protected TestCaseGeneratorValBaseFfc(
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
            List<TestCaseDispositionOption> dispositionList)
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

        /// <inheritdoc />
        public override FfcDomainParameters GetDomainParameters(TestGroup testGroup)
        {
            return new FfcDomainParameters(testGroup.P, testGroup.Q, testGroup.G);
        }

        /// <inheritdoc />
        protected override void MangleKeys(
            TestCase testCase, 
            TestCaseDispositionOption intendedDisposition, 
            IKas<
                KasDsaAlgoAttributesFfc, 
                OtherPartySharedInformation<
                    FfcDomainParameters, 
                    FfcKeyPair
                >, 
                FfcDomainParameters, 
                FfcKeyPair
            > serverKas, 
            IKas<
                KasDsaAlgoAttributesFfc, 
                OtherPartySharedInformation<
                    FfcDomainParameters, 
                    FfcKeyPair
                >, 
                FfcDomainParameters, 
                FfcKeyPair
            > iutKas)
        {
            TestCaseDispositionHelper.MangleKeys(
                testCase,
                intendedDisposition,
                serverKas,
                iutKas
            );
        }

        /// <inheritdoc />
        protected override void SetTestCaseInformationFromKasResult(
            TestGroup @group, 
            TestCase testCase, 
            IKas<
                KasDsaAlgoAttributesFfc, 
                OtherPartySharedInformation<
                    FfcDomainParameters, 
                    FfcKeyPair
                >, 
                FfcDomainParameters, 
                FfcKeyPair
            > serverKas, 
            IKas<
                KasDsaAlgoAttributesFfc, 
                OtherPartySharedInformation<
                    FfcDomainParameters, 
                    FfcKeyPair
                >, 
                FfcDomainParameters, 
                FfcKeyPair
            > iutKas,
            KasResult iutResult)
        {
            TestCaseDispositionHelper.SetTestCaseInformationFromKasResults(
                group, 
                testCase, 
                serverKas, 
                iutKas, 
                iutResult
            );
        }
    }
}