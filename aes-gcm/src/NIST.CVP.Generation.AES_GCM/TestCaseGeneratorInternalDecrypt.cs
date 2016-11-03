using System;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCaseGeneratorInternalDecrypt : ITestCaseGenerator
    {
        private readonly IAES_GCM _aesGcm;
        private readonly IRandom800_90 _random800_90;

        public TestCaseGeneratorInternalDecrypt(IRandom800_90 random800_90, IAES_GCM aesGcm)
        {
            _random800_90 = random800_90;
            _aesGcm = aesGcm;
        }

        public string IVGen { get { return "internal"; } }
        public string Direction { get { return "decrypt"; } }

        public TestCaseGenerateResponse Generate(TestGroup group, BitString key, BitString plainText, BitString aad)
        {
            // @@@ todo
            throw new NotImplementedException();
        }
    }
}