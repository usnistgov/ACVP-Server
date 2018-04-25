using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_XPN
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IAES_GCM _algo;

        public TestCaseValidatorFactory(IAES_GCM algo)
        {
            _algo = algo;
        }

        public IEnumerable<ITestCaseValidator<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => g))
            {
                foreach (var test in group.Tests.Select(t => t))
                {
                    var workingTest = test;

                    if (group.Function == "encrypt")
                    {
                        if (workingTest.Deferred)
                        {
                            list.Add(
                                new TestCaseValidatorInternalEncrypt(
                                    group, 
                                    workingTest, 
                                    new DeferredTestCaseResolver(_algo)
                                )
                            );
                        }
                        else
                        {
                            list.Add(new TestCaseValidatorExternalEncrypt(workingTest));
                        }
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
