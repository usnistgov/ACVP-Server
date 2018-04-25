using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Generation.KAS.ECC.Helpers;
using NIST.CVP.Generation.KAS.Enums;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.ECC
{
    public abstract class TestCaseGeneratorValBaseEcc : TestCaseGeneratorValBase<
        TestGroup,
        TestCase,
        KasDsaAlgoAttributesEcc,
        EccDomainParameters,
        EccKeyPair
    >
    {

        protected readonly IEccCurveFactory _curveFactory;

        protected TestCaseGeneratorValBaseEcc(
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
            List<TestCaseDispositionOption> dispositionList
        ) : base(
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
        {
            _curveFactory = curveFactory;
        }

        public override EccDomainParameters GetDomainParameters(TestGroup testGroup)
        {
            return new EccDomainParameters(_curveFactory.GetCurve(testGroup.CurveName));
        }

        protected override void MangleKeys(TestCase testCase, TestCaseDispositionOption intendedDisposition, IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> serverKas, IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> iutKas)
        {
            TestCaseDispositionHelper.MangleKeys(
                testCase,
                intendedDisposition,
                serverKas,
                iutKas
            );
        }

        protected override void SetTestCaseInformationFromKasResult(TestGroup @group, TestCase testCase, IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> serverKas, IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> iutKas,
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