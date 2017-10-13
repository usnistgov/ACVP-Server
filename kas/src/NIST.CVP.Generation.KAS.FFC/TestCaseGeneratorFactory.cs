using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.KAS.Enums;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Generation.KAS.FFC.Helpers;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {

        private const string aftTest = "aft";
        private const string valTest = "val";

        private readonly IKasBuilder _kasBuilder;
        private readonly ISchemeBuilder _schemeBuilder;
        private readonly IDsaFfcFactory _dsaFactory;
        private readonly IShaFactory _shaFactory;
        private readonly IMacParametersBuilder _macParametersBuilder;
        private readonly IEntropyProviderFactory _entropyProviderFactory;
        private readonly IKdfFactory _kdfFactory;
        private readonly INoKeyConfirmationFactory _noKeyConfirmationFactory;
        private readonly IDiffieHellman _diffieHellman;
        private readonly IMqv _mqv;

        private List<TestCaseDispositionOption> _validityTestCaseOptions = new List<TestCaseDispositionOption>();

        public TestCaseGeneratorFactory(
            IKasBuilder kasBuilder, 
            ISchemeBuilder schemeBuilder,
            IDsaFfcFactory dsaFactory,
            IShaFactory shaFactory,
            IMacParametersBuilder macParametersBuilder, 
            IEntropyProviderFactory entropyProviderFactory,
            IKdfFactory kdfFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IDiffieHellman diffieHellman,
            IMqv mqv
        )
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _dsaFactory = dsaFactory;
            _shaFactory = shaFactory;
            _macParametersBuilder = macParametersBuilder;
            _entropyProviderFactory = entropyProviderFactory;
            _kdfFactory = kdfFactory;
            _noKeyConfirmationFactory = noKeyConfirmationFactory;
            _diffieHellman = diffieHellman;
            _mqv = mqv;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {

            if (testGroup.TestType.Equals(aftTest, StringComparison.OrdinalIgnoreCase))
            {
                switch (testGroup.KasMode)
                {
                    case KasMode.NoKdfNoKc:
                        return new TestCaseGeneratorAftNoKdfNoKc(_kasBuilder, _schemeBuilder, _dsaFactory, _shaFactory);
                    case KasMode.KdfNoKc:
                        return new TestCaseGeneratorAftKdfNoKc(_kasBuilder, _schemeBuilder, _dsaFactory, _shaFactory, _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random), _macParametersBuilder);
                    case KasMode.KdfKc:
                        break;
                    default:
                        return new TestCaseGeneratorNull();
                }
            }

            if (testGroup.TestType.Equals(valTest, StringComparison.OrdinalIgnoreCase))
            {
                if (_validityTestCaseOptions.Count == 0)
                {
                    _validityTestCaseOptions = TestCaseDispositionHelper.PopulateValidityTestCaseOptions(testGroup);
                }

                TestCaseDispositionOption dispositionIntention = TestCaseDispositionHelper.GetTestCaseIntention(_validityTestCaseOptions);

                switch (testGroup.KasMode)
                {
                    case KasMode.NoKdfNoKc:
                        return new TestCaseGeneratorValNoKdfNoKc(_kasBuilder, _schemeBuilder, _dsaFactory, _shaFactory, dispositionIntention);
                    case KasMode.KdfNoKc:
                        return new TestCaseGeneratorValKdfNoKc(_kasBuilder, _schemeBuilder, _dsaFactory, _shaFactory, _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random), _macParametersBuilder, _kdfFactory, _noKeyConfirmationFactory, dispositionIntention);
                }
            }

            return new TestCaseGeneratorNull();
        }
    }
}