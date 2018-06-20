using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_XPN
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IAeadModeBlockCipher _algo;

        public TestCaseValidatorFactory(IAeadModeBlockCipherFactory cipherFactory, IBlockCipherEngineFactory engineFactory)
        {
            _algo = cipherFactory.GetAeadCipher(engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), BlockCipherModesOfOperation.Gcm);
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
