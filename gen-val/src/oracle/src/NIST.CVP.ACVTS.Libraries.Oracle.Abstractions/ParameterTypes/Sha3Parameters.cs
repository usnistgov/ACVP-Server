using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class Sha3Parameters : IParameters
    {
        public int MessageLength { get; set; }
        public HashFunction HashFunction { get; set; }

        public override bool Equals(object other)
        {
            if (other is Sha3Parameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(MessageLength, HashFunction);
    }
}
