using System;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.KAS.Scheme.Ecc;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.ECC
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {

        private const string aftTest = "aft";
        private const string valTest = "val";

        private readonly IEccCurveFactory _curveFactory;
        private readonly IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> _kasBuilder;
        private readonly ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> _schemeBuilder;
        private readonly IShaFactory _shaFactory;
        private readonly IMacParametersBuilder _macParametersBuilder;
        private readonly IEntropyProviderFactory _entropyProviderFactory;
        private readonly IKdfFactory _kdfFactory;
        private readonly INoKeyConfirmationFactory _noKeyConfirmationFactory;
        private readonly IKeyConfirmationFactory _keyConfirmationFactory;
        
        public TestCaseGeneratorFactory(
            IEccCurveFactory curveFactory,
            IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> kasBuilder, 
            ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> schemeBuilder,
            IShaFactory shaFactory,
            IMacParametersBuilder macParametersBuilder, 
            IEntropyProviderFactory entropyProviderFactory,
            IKdfFactory kdfFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IKeyConfirmationFactory keyConfirmationFactory
        )
        {
            _curveFactory = curveFactory;
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _shaFactory = shaFactory;
            _macParametersBuilder = macParametersBuilder;
            _entropyProviderFactory = entropyProviderFactory;
            _kdfFactory = kdfFactory;
            _noKeyConfirmationFactory = noKeyConfirmationFactory;
            _keyConfirmationFactory = keyConfirmationFactory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {

            if (testGroup.TestType.Equals(aftTest, StringComparison.OrdinalIgnoreCase))
            {
                switch (testGroup.KasMode)
                {
                    case KasMode.NoKdfNoKc:
                        return new TestCaseGeneratorAftNoKdfNoKc(_curveFactory, _kasBuilder, _schemeBuilder, _entropyProviderFactory, _macParametersBuilder);
                    case KasMode.KdfNoKc:
                        return new TestCaseGeneratorAftKdfNoKc(_curveFactory, _kasBuilder, _schemeBuilder, _entropyProviderFactory, _macParametersBuilder);
                    case KasMode.KdfKc:
                        return new TestCaseGeneratorAftKdfKc(_curveFactory, _kasBuilder, _schemeBuilder, _entropyProviderFactory, _macParametersBuilder);
                    default:
                        return new TestCaseGeneratorNull();
                }
            }

            if (testGroup.TestType.Equals(valTest, StringComparison.OrdinalIgnoreCase))
            {
                var validityTestCaseOptions = KAS.Helpers.TestCaseDispositionHelper.PopulateValidityTestCaseOptions(testGroup);

                switch (testGroup.KasMode)
                {
                    case KasMode.NoKdfNoKc:
                        return new TestCaseGeneratorValNoKdfNoKc(_curveFactory, _kasBuilder, _schemeBuilder, _shaFactory, _entropyProviderFactory, _macParametersBuilder, _kdfFactory, _keyConfirmationFactory, _noKeyConfirmationFactory, validityTestCaseOptions);
                    case KasMode.KdfNoKc:
                        return new TestCaseGeneratorValKdfNoKc(_curveFactory, _kasBuilder, _schemeBuilder, _shaFactory, _entropyProviderFactory, _macParametersBuilder, _kdfFactory, _keyConfirmationFactory, _noKeyConfirmationFactory, validityTestCaseOptions);
                    case KasMode.KdfKc:
                        return new TestCaseGeneratorValKdfKc(_curveFactory, _kasBuilder, _schemeBuilder, _shaFactory, _entropyProviderFactory, _macParametersBuilder, _kdfFactory, _keyConfirmationFactory, _noKeyConfirmationFactory, validityTestCaseOptions);
                    default:
                        return new TestCaseGeneratorNull();
                }
            }

            return new TestCaseGeneratorNull();
        }
    }
}