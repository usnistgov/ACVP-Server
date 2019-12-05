using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Math.Domain;
using System;

namespace NIST.CVP.Common.Oracle.ParameterTypes
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

        // TODO make sure this works as expected
        public override int GetHashCode() => HashCode.Combine(MessageLength, HashFunction, OutputLengths);
    }
}
