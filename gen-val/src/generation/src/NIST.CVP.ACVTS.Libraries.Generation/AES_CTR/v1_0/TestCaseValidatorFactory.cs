﻿using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly List<string> _katTestTypes = new List<string> { "gfsbox", "keysbox", "varkey", "vartxt" };

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
                    var testType = group.InternalTestType.ToLower();
                    var direction = group.Direction.ToLower();

                    if (testType == "singleblock" || testType == "partialblock" || _katTestTypes.Contains(testType))
                    {
                        if (direction == "encrypt")
                        {
                            list.Add(new TestCaseValidatorEncrypt(test));
                        }
                        else if (direction == "decrypt")
                        {
                            list.Add(new TestCaseValidatorDecrypt(test));
                        }
                        else
                        {
                            list.Add(new TestCaseValidatorNull(test));
                        }
                    }
                    else if (testType == "ctr")
                    {
                        if (direction == "encrypt")
                        {
                            list.Add(new TestCaseValidatorCounterEncrypt(
                                group,
                                test,
                                new DeferredIvExtractor(_oracle)
                            ));
                        }
                        else if (direction == "decrypt")
                        {
                            list.Add(new TestCaseValidatorCounterDecrypt(
                                group,
                                test,
                                new DeferredIvExtractor(_oracle)
                            ));
                        }
                        else
                        {
                            list.Add(new TestCaseValidatorNull(test));
                        }
                    }
                    else if (testType.Equals("RFC3686", StringComparison.OrdinalIgnoreCase))
                    {
                        if (test.Deferred)
                        {
                            list.Add(new TestCaseValidatorDeferredRfcEncrypt(
                                group,
                                test,
                                new DeferredRfcResolver(_oracle)));
                        }
                        else
                        {
                            if (direction == "encrypt")
                            {
                                list.Add(new TestCaseValidatorEncrypt(test));
                            }
                            else if (direction == "decrypt")
                            {
                                list.Add(new TestCaseValidatorDecrypt(test));
                            }
                        }
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorNull(test));
                    }
                }
            }

            return list;
        }
    }
}
