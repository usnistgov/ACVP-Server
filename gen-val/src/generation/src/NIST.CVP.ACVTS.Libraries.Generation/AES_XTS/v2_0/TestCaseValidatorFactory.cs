using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0
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

                    switch (group.Direction)
                    {
                        case BlockCipherDirections.Encrypt:
                            list.Add(new TestCaseValidatorEncrypt(workingTest));
                            break;

                        case BlockCipherDirections.Decrypt:
                            list.Add(new TestCaseValidatorDecrypt(workingTest));
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
