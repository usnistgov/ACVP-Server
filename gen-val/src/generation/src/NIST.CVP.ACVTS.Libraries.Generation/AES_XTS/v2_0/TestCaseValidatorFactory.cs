using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0
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
                var deferred = group.TweakMode == XtsTweakModes.IutGenerated;
                foreach (var test in group.Tests.Select(t => t))
                {
                    var workingTest = test;

                    switch (group.Direction)
                    {
                        case BlockCipherDirections.Encrypt:
                            if (deferred)
                            {
                                list.Add(
                                    new TestCaseValidatorDeferredEncrypt(
                                        group,
                                        workingTest,
                                        new DeferredResolver(_oracle)
                                    )
                                );
                            }
                            else
                            {
                                list.Add(new TestCaseValidatorEncrypt(workingTest));
                            }
                            break;

                        case BlockCipherDirections.Decrypt:
                            if (deferred)
                            {
                                list.Add(
                                    new TestCaseValidatorDeferredDecrypt(
                                        group,
                                        workingTest,
                                        new DeferredResolver(_oracle)
                                    )
                                );
                            }
                            else
                            {
                                list.Add(new TestCaseValidatorDecrypt(workingTest));
                            }
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return list;
        }
    }
}
