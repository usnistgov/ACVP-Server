using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class TupleHashParameters
    {
        public int MessageLength { get; set; }
        public int TupleSize { get; set; }
        public int CustomizationLength { get; set; }
        public bool HexCustomization { get; set; }
        public bool BitOrientedInput { get; set; }
        public bool SemiEmptyCase { get; set; }
        public bool LongRandomCase { get; set; }
        public MathDomain OutLens { get; set; }
        public HashFunction HashFunction { get; set; }
    }
}
