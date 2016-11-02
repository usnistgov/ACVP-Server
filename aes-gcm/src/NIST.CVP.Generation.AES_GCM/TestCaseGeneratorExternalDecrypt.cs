using System;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_GCM
{
    internal class TestCaseGeneratorExternalDecrypt : ITestCaseGenerator
    {
        private IAES_GCM _aesGcm;
        private IRandom800_90 _random800_90;

        public TestCaseGeneratorExternalDecrypt(IRandom800_90 random800_90, IAES_GCM aesGcm)
        {
            _random800_90 = random800_90;
            _aesGcm = aesGcm;
        }

        public string IVGen { get { return "external"; } }
        public string Direction { get { return "decrypt"; } }

        public TestCaseGenerateResponse Generate(TestGroup group, BitString key, BitString plainText, BitString aad)
        {
            // @@@ todo
            throw new NotImplementedException();
        }
    }
}