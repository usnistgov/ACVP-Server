using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        private readonly IAES_GCM _algo;

        public TestCaseValidatorFactory(IAES_GCM algo)
        {
            _algo = algo;
        }

        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase)t))
                {
                    var workingTest = test;
                    if (test.Deferred && group.Function == "encrypt")
                    {
                        list.Add(
                            new TestCaseValidatorDeferredEncrypt(
                                group, 
                                workingTest,
                                new DeferredEncryptResolver(_algo)
                            )
                        );
                    }
                    else if (group.Function == "encrypt")
                    {
                        list.Add(new TestCaseValidatorEncrypt(workingTest));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorDecrypt(workingTest));
                    }

                }
            }

            return list;
        }
    }
}
