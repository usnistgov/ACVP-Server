using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Math.Domain;
using System;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class CShakeParameters : IParameters
    {
        public int MessageLength { get; set; }
        public int CustomizationLength { get; set; }
        public string FunctionName { get; set; } = "";
        public bool HexCustomization { get; set; }
        public MathDomain OutLens { get; set; }
        public HashFunction HashFunction { get; set; }
        public bool IsSample { get; set; } = false;

        public override bool Equals(object other)
        {
            if (other is CShakeParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        // TODO make sure this works as expected with MathDomain and HashFunction
        public override int GetHashCode() => HashCode.Combine(MessageLength, CustomizationLength, FunctionName, HexCustomization, OutLens, HashFunction, IsSample);
    }
}
