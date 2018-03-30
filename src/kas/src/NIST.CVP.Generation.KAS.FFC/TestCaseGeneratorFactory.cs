using System;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.KAS.Scheme.Ffc;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {

        private const string aftTest = "aft";
        private const string valTest = "val";

        private readonly IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> _kasBuilder;
        private readonly ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> _schemeBuilder;
        private readonly IShaFactory _shaFactory;
        private readonly IMacParametersBuilder _macParametersBuilder;
        private readonly IEntropyProviderFactory _entropyProviderFactory;
        private readonly IKdfFactory _kdfFactory;
        private readonly INoKeyConfirmationFactory _noKeyConfirmationFactory;
        private readonly IKeyConfirmationFactory _keyConfirmationFactory;

        public TestCaseGeneratorFactory(
            IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> kasBuilder, 
            ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> schemeBuilder,
            IShaFactory shaFactory,
            IMacParametersBuilder macParametersBuilder, 
            IEntropyProviderFactory entropyProviderFactory,
            IKdfFactory kdfFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IKeyConfirmationFactory keyConfirmationFactory
        )
        {
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
                        return new TestCaseGeneratorAftNoKdfNoKc(_kasBuilder, _schemeBuilder, _entropyProviderFactory, _macParametersBuilder);
                    case KasMode.KdfNoKc:
                        return new TestCaseGeneratorAftKdfNoKc(_kasBuilder, _schemeBuilder, _entropyProviderFactory, _macParametersBuilder);
                    case KasMode.KdfKc:
                        return new TestCaseGeneratorAftKdfKc(_kasBuilder, _schemeBuilder, _entropyProviderFactory, _macParametersBuilder);
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
                        return new TestCaseGeneratorValNoKdfNoKc(_kasBuilder, _schemeBuilder, _shaFactory, _entropyProviderFactory, _macParametersBuilder, _kdfFactory, _keyConfirmationFactory, _noKeyConfirmationFactory, validityTestCaseOptions);
                    case KasMode.KdfNoKc:
                        return new TestCaseGeneratorValKdfNoKc(_kasBuilder, _schemeBuilder, _shaFactory, _entropyProviderFactory, _macParametersBuilder, _kdfFactory, _keyConfirmationFactory, _noKeyConfirmationFactory, validityTestCaseOptions);
                    case KasMode.KdfKc:
                        return new TestCaseGeneratorValKdfKc(_kasBuilder, _schemeBuilder, _shaFactory, _entropyProviderFactory, _macParametersBuilder, _kdfFactory, _keyConfirmationFactory, _noKeyConfirmationFactory, validityTestCaseOptions);
                    default:
                        return new TestCaseGeneratorNull();
                }
            }

            return new TestCaseGeneratorNull();
        }
    }
}