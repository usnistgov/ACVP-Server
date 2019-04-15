using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;

namespace NIST.CVP.Generation.CMAC.v1_0
{
    public class TestCaseGeneratorVerAes : TestCaseGeneratorVerBase
    {
        public TestCaseGeneratorVerAes(IOracle oracle) : base(oracle)
        {
        }

        protected override CmacParameters GetParam(TestGroup group)
        {
            return new CmacParameters()
            {
                CmacType = group.CmacType,
                CouldFail = true,
                KeyingOption = 0,
                KeyLength = group.KeyLength,
                MacLength = group.MacLength,
                PayloadLength = group.MessageLength
            };
        }
    }
}