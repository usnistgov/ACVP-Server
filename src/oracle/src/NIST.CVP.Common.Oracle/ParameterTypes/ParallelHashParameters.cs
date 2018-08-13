using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class ParallelHashParameters
    {
        public int MessageLength { get; set; }
        public int CustomizationLength { get; set; }
        public int BlockSize { get; set; }
        public bool HexCustomization { get; set; }
        public MathDomain OutLens { get; set; }
        public HashFunction HashFunction { get; set; }
    }
}
