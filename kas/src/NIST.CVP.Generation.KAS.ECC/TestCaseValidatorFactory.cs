using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.KAS.Scheme.Ecc;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.ECC
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        private readonly IEccCurveFactory _curveFactory;
        private readonly IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> _kasBuilder;
        private readonly IMacParametersBuilder _macParametersBuilder;
        private readonly ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> _schemeBuilder;
        private readonly IEntropyProviderFactory _entropyProviderFactory;

        public TestCaseValidatorFactory(
            IEccCurveFactory curveFactory,
            IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> kasBuilder,
            IMacParametersBuilder macParametersBuilder,
            ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> schemeBuilder,
            IEntropyProviderFactory entropyProviderFactory
        )
        {
            _curveFactory = curveFactory;
            _kasBuilder = kasBuilder;
            _macParametersBuilder = macParametersBuilder;
            _schemeBuilder = schemeBuilder;
            _entropyProviderFactory = entropyProviderFactory;
        }

        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet)
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
                                            _curveFactory,
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
                                            _curveFactory,
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
                                            _curveFactory,
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