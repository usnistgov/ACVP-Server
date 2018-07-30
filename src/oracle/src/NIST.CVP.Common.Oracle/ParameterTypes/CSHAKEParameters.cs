using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class CShakeParameters
    {
        public int MessageLength { get; set; }
        public int CustomizationLength { get; set; }
        public string FunctionName { get; set; } = "";
        public bool HexCustomization { get; set; }
        public MathDomain OutLens { get; set; }
        public HashFunction HashFunction { get; set; }
    }
}
