using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Math.Domain;
using System;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class ParallelHashParameters : IParameters
    {
        public int MessageLength { get; set; }
        public int CustomizationLength { get; set; }
        public int BlockSize { get; set; }
        public bool HexCustomization { get; set; }
        public MathDomain OutLens { get; set; }
        public HashFunction HashFunction { get; set; }

        public override bool Equals(object other)
        {
            if (other is ParallelHashParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        // TODO make sure this works as expected
        public override int GetHashCode() => HashCode.Combine(MessageLength, CustomizationLength, BlockSize, HexCustomization, OutLens, HashFunction);
    }
}
