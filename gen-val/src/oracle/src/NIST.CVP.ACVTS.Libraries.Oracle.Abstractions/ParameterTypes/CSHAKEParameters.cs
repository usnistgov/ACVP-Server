using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
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

        public override int GetHashCode() => HashCode.Combine(MessageLength, CustomizationLength, FunctionName, HexCustomization, OutLens, HashFunction, IsSample);
    }
}
