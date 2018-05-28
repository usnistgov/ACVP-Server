using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;
        private readonly List<string> _katTestTypes = new List<string>
        {
            "permutation", "substitutiontable", "variablekey", "variabletext", "inversepermutation"
        };
        
        public TestCaseValidatorFactory(
            IBlockCipherEngineFactory engineFactory,
            IModeBlockCipherFactory modeFactory
        )
        {
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
        }
        
        public IEnumerable<ITestCaseValidator<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups)
            {
                var testType = group.TestType.ToLower();
                var direction = group.Direction.ToLower();

                if (testType == "singleblock" || testType == "partialblock" || _katTestTypes.Contains(testType))
                {
                    if (direction == "encrypt")
                    {
                        list.AddRange(group.Tests.Select(t => new TestCaseValidatorEncrypt(t)));
                    }
                    else if (direction == "decrypt")
                    {
                        list.AddRange(group.Tests.Select(t => new TestCaseValidatorDecrypt(t)));
                    }
                }
                else if (testType == "counter")
                {
                    if (direction == "encrypt")
                    {
                        list.AddRange(group.Tests.Select(t => 
                            new TestCaseValidatorCounterEncrypt(
                                group, 
                                t, 
                                new DeferredTestCaseResolverEncrypt(_engineFactory, _modeFactory))
                        ));
                    }
                    else if (direction == "decrypt")
                    {
                        list.AddRange(group.Tests.Select(t => 
                            new TestCaseValidatorCounterDecrypt(
                                group, 
                                t, 
                                new DeferredTestCaseResolverDecrypt(_engineFactory, _modeFactory))
                        ));
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
