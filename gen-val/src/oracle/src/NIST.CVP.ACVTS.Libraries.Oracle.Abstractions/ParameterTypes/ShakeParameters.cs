using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class ShakeParameters : IParameters
    {
        public int MessageLength { get; set; }
        public HashFunction HashFunction { get; set; }
        public MathDomain OutputLengths { get; set; }

        public override bool Equals(object other)
        {
            if (other is ShakeParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(MessageLength, HashFunction, OutputLengths);
    }
}
