using System.Collections.Generic;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.TDES_CBCI.v1_0
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
    {
        public List<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups)
            {
                foreach (var test in group.Tests)
                {
                    var workingTest = test;
                    if (group.TestType.ToLower() == "mct")
                    {
                        if (group.Function == "encrypt")
                        {
                            list.Add(new TestCaseValidatorMonteCarloEncrypt(workingTest));
                        }
                        else
                        {
                            list.Add(new TestCaseValidatorMonteCarloDecrypt(workingTest));
                        }
                    }
                    else
                    {
                        if (group.Function == "encrypt")
                        {
                            list.Add(new TestCaseValidatorEncrypt(workingTest));
                        }
                        else
                        {
                            list.Add(new TestCaseValidatorDecrypt(workingTest));
                        }
                    }
                }
            }

            return list;
        }
    }
}
