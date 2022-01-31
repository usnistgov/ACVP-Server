using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseValidatorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public List<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

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
