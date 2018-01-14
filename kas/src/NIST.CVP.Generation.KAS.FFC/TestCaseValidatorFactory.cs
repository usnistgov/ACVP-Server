using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.KAS.Scheme.Ffc;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        private readonly IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> _kasBuilder;
        private readonly IMacParametersBuilder _macParametersBuilder;
        private readonly ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> _schemeBuilder;
        private readonly IEntropyProviderFactory _entropyProviderFactory;

        public TestCaseValidatorFactory(
            IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> kasBuilder,
            IMacParametersBuilder macParametersBuilder,
            ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> schemeBuilder,
            IEntropyProviderFactory entropyProviderFactory
        )
        {
            _kasBuilder = kasBuilder;
            _macParametersBuilder = macParametersBuilder;
            _schemeBuilder = schemeBuilder;
            _entropyProviderFactory = entropyProviderFactory;
        }

        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet, IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase)t))
                {
                    var workingTest = test;

                    if (group.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase))
                    {
                        switch (group.KasMode)
                        {
                            case KasMode.NoKdfNoKc:
                                list.Add(
                                    new TestCaseValidatorAftNoKdfNoKc(
                                        workingTest, 
                                        group,
                                        new DeferredTestCaseResolverAftNoKdfNoKc(
                                            _kasBuilder,
                                            _schemeBuilder,
                                            _macParametersBuilder,
                                            _entropyProviderFactory
                                        )
                                    )
                                );
                                break;
                            case KasMode.KdfNoKc:
                                list.Add(
                                    new TestCaseValidatorAftKdfNoKc(
                                        workingTest, 
                                        group,
                                        new DeferredTestCaseResolverAftKdfNoKc(
                                            _kasBuilder,
                                            _schemeBuilder,
                                            _macParametersBuilder,
                                            _entropyProviderFactory
                                        )
                                    )
                                );
                                break;
                            case KasMode.KdfKc:
                                list.Add(
                                    new TestCaseValidatorAftKdfKc(
                                        workingTest, 
                                        group,
                                        new DeferredTestCaseResolverAftKdfKc(
                                            _kasBuilder,
                                            _schemeBuilder,
                                            _macParametersBuilder,
                                            _entropyProviderFactory
                                        )
                                    )
                                );
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorVal(workingTest));
                    }
                }
            }

            return list;
        }
    }
}