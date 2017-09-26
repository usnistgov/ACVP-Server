using System;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC.TDES
{
    public class TestCaseGeneratorGen : TestCaseGeneratorGenBase<TestGroup, TestCase>
    {
        public override TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            BitString key = null;
            if (group.KeyingOption == 1)
            {
                key = _random800_90.GetRandomBitString(group.KeyLength);
            }
            else if (group.KeyingOption == 2)
            {
                var k1 = _random800_90.GetRandomBitString(64);
                var k2 = _random800_90.GetRandomBitString(64);
                key = k1.ConcatenateBits(k2).ConcatenateBits(k1);
            }
            var msg = _random800_90.GetRandomBitString(group.MessageLength);
            var testCase = new TestCase
            {
                Key = key,
                Message = msg
            };
            return Generate(group, testCase);
        }

        public TestCaseGeneratorGen(IRandom800_90 random800_90, ICmac algo) : base(random800_90, algo)
        {
        }
    }
}
