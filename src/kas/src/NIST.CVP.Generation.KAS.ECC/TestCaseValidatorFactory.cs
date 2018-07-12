using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.ECC
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseValidatorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public IEnumerable<ITestCaseValidator<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => g))
            {
                foreach (var test in group.Tests.Select(t => t))
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
                                        new DeferredTestCaseResolver(_oracle)
                                    )
                                );
                                break;
                            case KasMode.KdfNoKc:
                                list.Add(
                                    new TestCaseValidatorAftKdfNoKc(
                                        workingTest, 
                                        group,
                                        new DeferredTestCaseResolver(_oracle)
                                    )
                                );
                                break;
                            case KasMode.KdfKc:
                                list.Add(
                                    new TestCaseValidatorAftKdfKc(
                                        workingTest, 
                                        group,
                                        new DeferredTestCaseResolver(_oracle)
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