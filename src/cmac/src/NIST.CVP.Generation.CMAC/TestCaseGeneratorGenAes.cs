using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;

namespace NIST.CVP.Generation.CMAC
{
    public class TestCaseGeneratorGenAes : TestCaseGeneratorGenBase
    {
        public TestCaseGeneratorGenAes(IOracle oracle) : base(oracle)
        {
        }

        protected override CmacParameters GetParam(TestGroup group)
        {
            return new CmacParameters()
            {
                CmacType = group.CmacType,
                CouldFail = false,
                KeyingOption = 0,
                KeyLength = group.KeyLength,
                MacLength = group.MacLength,
                PayloadLength = group.MessageLength
            };
        }
    }
}
