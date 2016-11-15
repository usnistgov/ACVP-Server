using System;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCaseGeneratorDecrypt : ITestCaseGenerator
    {
        private readonly IAES_GCM _aesGcm;
        private readonly IRandom800_90 _random800_90;

        public TestCaseGeneratorDecrypt(IRandom800_90 random800_90, IAES_GCM aesGcm)
        {
            _random800_90 = random800_90;
            _aesGcm = aesGcm;
        }

        public string IVGen { get { return "internal"; } }
        public string Direction { get { return "decrypt"; } }

        public TestCaseGenerateResponse Generate(TestGroup @group)
        {
            // @@@ todo
            throw new NotImplementedException();
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            // @@@ todo
            throw new NotImplementedException();
        }
    }
}