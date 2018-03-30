using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES_CTR;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        private readonly ITdesCtr _algo;
        private readonly List<string> _katTestTypes = new List<string>
        {
            "permutation", "substitutiontable", "variablekey", "variabletext", "inversepermutation"
        };
    
        public TestCaseValidatorFactory(ITdesCtr algo)
        {
            _algo = algo;
        }

        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet, IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                var testType = group.TestType.ToLower();
                var direction = group.Direction.ToLower();

                if (testType == "singleblock" || testType == "partialblock" || _katTestTypes.Contains(testType))
                {
                    if (direction == "encrypt")
                    {
                        list.AddRange(group.Tests.Select(t => (TestCase)t).Select(t => new TestCaseValidatorEncrypt(t)));
                    }
                    else if (direction == "decrypt")
                    {
                        list.AddRange(group.Tests.Select(t => (TestCase)t).Select(t => new TestCaseValidatorDecrypt(t)));
                    }
                }
                else if (testType == "counter")
                {
                    if (direction == "encrypt")
                    {
                        list.AddRange(group.Tests.Select(t => (TestCase)t).Select(t => new TestCaseValidatorCounterEncrypt(group, t, new DeferredTestCaseResolverEncrypt(_algo))));
                    }
                    else if (direction == "decrypt")
                    {
                        list.AddRange(group.Tests.Select(t => (TestCase)t).Select(t => new TestCaseValidatorCounterDecrypt(group, t, new DeferredTestCaseResolverDecrypt(_algo))));
                    }
                }
                else
                {
                    list.AddRange(group.Tests.Select(t => new TestCaseValidatorNull(t)));
                }
            }

            return list;
        }
    }
}
