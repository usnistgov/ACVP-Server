using System;
using NIST.CVP.Crypto.KeyWrap;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KeyWrap.TDES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.KeyWrap.AES
{
    public class TestCaseGeneratorEncrypt : TestCaseGeneratorEncryptBase<TestGroup, TestCase>
    {
        public TestCaseGeneratorEncrypt(IKeyWrapFactory iKeyWrapFactory, IRandom800_90 iRandom800_90) : base(iKeyWrapFactory, iRandom800_90)
        {
        }
    }
}