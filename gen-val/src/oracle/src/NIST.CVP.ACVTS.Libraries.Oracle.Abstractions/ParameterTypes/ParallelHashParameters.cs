using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class ParallelHashParameters : IParameters
    {
        public string FunctionName { get; set; }
        public int MessageLength { get; set; }
        public int CustomizationLength { get; set; }
        public int BlockSize { get; set; }
        public bool HexCustomization { get; set; }
        public MathDomain BlockSizeDomain { get; set; }
        public MathDomain OutLens { get; set; }
        public HashFunction HashFunction { get; set; }
        public bool IsSample { get; set; }

        public override bool Equals(object other)
        {
            if (other is ParallelHashParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(MessageLength, CustomizationLength, BlockSize, BlockSizeDomain, HexCustomization, OutLens, HashFunction, IsSample);
    }
}
