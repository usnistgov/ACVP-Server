using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly List<string> _katTestTypes = new List<string> { "gfsbox", "keysbox", "varkey", "vartxt" };

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
