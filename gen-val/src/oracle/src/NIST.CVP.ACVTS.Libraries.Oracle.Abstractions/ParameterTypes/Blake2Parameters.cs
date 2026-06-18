using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.Blake2;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class Blake2Parameters : IParameters
    {
        public int MessageLength { get; set; }
        public int KeyLength { get; set; }
        public Blake2HashFunction HashFunction { get; set; }

        public override bool Equals(object other)
        {
            if (other is Blake2Parameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(MessageLength, KeyLength, HashFunction?.Variant, HashFunction?.DigestLength);
    }
}
