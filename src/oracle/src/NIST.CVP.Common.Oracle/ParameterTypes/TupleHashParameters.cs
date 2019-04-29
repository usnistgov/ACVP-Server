using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Math.Domain;
using System;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class TupleHashParameters : IParameters
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
        public string FunctionName { get; set; }
        public bool IsSample { get; set; }

        public override bool Equals(object other)
        {
            if (other is TupleHashParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        // TODO make sure this works as expected
        public override int GetHashCode() => HashCode.Combine(MessageLength, TupleSize, CustomizationLength, HexCustomization, BitOrientedInput, SemiEmptyCase, LongRandomCase, IsSample);
    }
}
