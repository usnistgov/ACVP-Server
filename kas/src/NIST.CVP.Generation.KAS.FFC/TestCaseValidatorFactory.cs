using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        private readonly IKasBuilder _kasBuilder;
        private readonly IMacParametersBuilder _macParametersBuilder;
        private readonly ISchemeBuilder _schemeBuilder;
        private readonly IEntropyProviderFactory _entropyProviderFactory;

        public TestCaseValidatorFactory(IKasBuilder kasBuilder, IMacParametersBuilder macParametersBuilder, ISchemeBuilder schemeBuilder, IEntropyProviderFactory entropyProviderFactory)
        {
            _kasBuilder = kasBuilder;
            _macParametersBuilder = macParametersBuilder;
            _schemeBuilder = schemeBuilder;
            _entropyProviderFactory = entropyProviderFactory;
        }

        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet, IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup) g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase) t))
                {
                    var workingTest = test;

                    if (group.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase))
                    {
                        switch (group.KasMode)
                        {
                            case KasMode.NoKdfNoKc:
                                list.Add(new TestCaseValidatorAftNoKdfNoKc(workingTest, group, _kasBuilder));
                                break;
                            case KasMode.KdfNoKc:
                                list.Add(new TestCaseValidatorAftKdfNoKc(workingTest, group, _kasBuilder, _macParametersBuilder, _schemeBuilder, _entropyProviderFactory));
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