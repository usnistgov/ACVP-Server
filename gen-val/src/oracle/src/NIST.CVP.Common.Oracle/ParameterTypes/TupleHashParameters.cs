using NIST.CVP.Crypto.Common.Hash.TupleHash;
using System;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class TupleHashParameters : IParameters
    {
        public int[] MessageLength { get; set; }
        public int CustomizationLength { get; set; }
        public bool HexCustomization { get; set; }
        public HashFunction HashFunction { get; set; }
        public string FunctionName { get; set; }
        
        // MCT Properties
        public MathDomain InputLengths { get; set; }
        public MathDomain OutputLengths { get; set; }
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
        public override int GetHashCode() => HashCode.Combine(MessageLength, HashFunction, CustomizationLength, HexCustomization, IsSample);
    }
}
