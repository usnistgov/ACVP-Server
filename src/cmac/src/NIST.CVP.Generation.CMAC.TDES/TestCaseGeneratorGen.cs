using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC.TDES
{
    public class TestCaseGeneratorGen : TestCaseGeneratorGenBase<TestGroup, TestCase>
    {
        public TestCaseGeneratorGen(IOracle oracle) : base(oracle)
        {
        }

        protected override CmacParameters GetParam(TestGroup group)
        {
            return new CmacParameters()
            {
                CmacType = group.CmacType,
                CouldFail = false,
                KeyingOption = group.KeyingOption,
                KeyLength = group.KeyLength,
                MacLength = group.MacLength,
                PayloadLength = group.MessageLength
            };
        }
    }
}
