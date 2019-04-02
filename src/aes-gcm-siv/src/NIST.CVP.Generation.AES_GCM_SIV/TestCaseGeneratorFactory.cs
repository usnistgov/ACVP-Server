using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.AES_GCM_SIV
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>
    {
        //private readonly IOracle _oracle;
        private readonly IRandom800_90 _rand;
        private readonly IAeadModeBlockCipherFactory _aeadCipherFactory;
        private readonly IBlockCipherEngineFactory _engineFactory;

        public TestCaseGeneratorFactory(IRandom800_90 rand, IAeadModeBlockCipherFactory aeadCipherFactory, IBlockCipherEngineFactory engineFactory)
        {
            //_oracle = oracle;
            _rand = rand;
            _aeadCipherFactory = aeadCipherFactory;
            _engineFactory = engineFactory;
        }

        public ITestCaseGeneratorAsync<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var direction = testGroup.Function.ToLower();
            
            if (direction == "encrypt")
            {
                return new TestCaseGeneratorEncrypt(_rand, _aeadCipherFactory, _engineFactory);
            }
            else
            {
                return new TestCaseGeneratorDecrypt(_rand, _aeadCipherFactory, _engineFactory);
            }
        }
    }
}
