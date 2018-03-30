using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        private readonly IAesCtr _algo;
        private readonly List<string> _katTestTypes = new List<string> { "gfsbox", "keysbox", "varkey", "vartxt" };

        public TestCaseValidatorFactory(IAesCtr algo)
        {
            _algo = algo;
        }

        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet, IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup) g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase) t))
                {
                    var testType = group.TestType.ToLower();
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
                    else if (testType == "counter")
                    {
                        if (direction == "encrypt")
                        {
                            list.Add(new TestCaseValidatorCounterEncrypt(group, test, new DeferredTestCaseResolverEncrypt(_algo)));
                        }
                        else if (direction == "decrypt")
                        {
                            list.Add(new TestCaseValidatorCounterDecrypt(group, test, new DeferredTestCaseResolverDecrypt(_algo)));
                        }
                        else
                        {
                            list.Add(new TestCaseValidatorNull(test));
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
