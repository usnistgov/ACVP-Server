using System;
using NIST.CVP.Crypto.KeyWrap;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.KeyWrap.TDES
{
    public class TestCaseGeneratorEncrypt : TestCaseGeneratorEncryptBase<TestGroup, TestCase>
    {
        public TestCaseGeneratorEncrypt(IKeyWrapFactory iKeyWrapFactory, IRandom800_90 iRandom800_90) : base(iKeyWrapFactory, iRandom800_90)
        {
        }
    }
}