using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class ShaParameters : IParameters
    {
        public MathDomain MessageDomain { get; set; }
        public int MessageLength { get; set; }
        public HashFunction HashFunction { get; set; }
        public int OutputLength { get; set; }
        
        public bool UsingNewMctAlgo()
        {
            if (!MessageDomain.IsWithinDomain(OutputLength * 3))
            {
                // Using new algo
                return true;
            }
            
            return false;
        }

        public override bool Equals(object other)
        {
            if (other is ShaParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MessageLength, HashFunction.DigestSize, HashFunction.Mode);
        }
    }
}
