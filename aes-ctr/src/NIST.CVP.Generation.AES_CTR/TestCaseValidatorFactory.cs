using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet, IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup) g))
            {
                if (group.TestType == "singleblock" || group.TestType == "partialblock")
                {
                    if (group.Direction == "encrypt")
                    {
                        list.AddRange(group.Tests.Select(t => (TestCase)t).Select(t => new TestCaseValidatorEncrypt(t)));
                    }
                    else if (group.Direction == "decrypt")
                    {
                        list.AddRange(group.Tests.Select(t => (TestCase)t).Select(t => new TestCaseValidatorDecrypt(t)));
                    }
                }
                else if (group.TestType == "counter")
                {
                    if (group.Direction == "encrypt")
                    {

                    }
                    else if (group.Direction == "decrypt")
                    {

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
