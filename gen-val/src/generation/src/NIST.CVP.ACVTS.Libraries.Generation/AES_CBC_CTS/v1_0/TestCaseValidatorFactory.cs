﻿using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CBC_CTS.v1_0
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
    {
        public List<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => g))
            {
                foreach (var test in group.Tests.Select(t => t))
                {
                    var workingTest = test;
                    if (group.TestType.ToLower() == "mct")
                    {
                        if (group.Function == "encrypt")
                        {
                            list.Add(new TestCaseValidatorMCTEncrypt(workingTest));
                        }
                        else
                        {
                            list.Add(new TestCaseValidatorMCTDecrypt(workingTest));
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
