using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;

namespace NIST.CVP.Generation.CMAC.TDES
{
    public class TestCaseGeneratorVer : TestCaseGeneratorVerBase<TestGroup, TestCase>
    {
        public TestCaseGeneratorVer(IOracle oracle) : base(oracle)
        {
        }

        protected override CmacParameters GetParam(TestGroup group)
        {
            return new CmacParameters()
            {
                CmacType = group.CmacType,
                CouldFail = true,
                KeyingOption = group.KeyingOption,
                KeyLength = group.KeyLength,
                MacLength = group.MacLength,
                PayloadLength = group.MessageLength
            };
        }
    }
}