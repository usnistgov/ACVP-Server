using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC.AES
{
    public class TestCaseGeneratorGen : TestCaseGeneratorGenBase<TestGroup, TestCase>
    {
        public override TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            //known answer - need to do an encryption operation to get the tag
            var key = _random800_90.GetRandomBitString(group.KeyLength);
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
