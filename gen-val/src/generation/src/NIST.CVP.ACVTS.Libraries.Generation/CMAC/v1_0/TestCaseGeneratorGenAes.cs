using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0
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
