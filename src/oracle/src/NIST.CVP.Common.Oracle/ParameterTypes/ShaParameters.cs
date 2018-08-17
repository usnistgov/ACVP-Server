using NIST.CVP.Crypto.Common.Hash.SHA2;
using System;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class ShaParameters : IParameters
    {
        public int MessageLength { get; set; }
        public HashFunction HashFunction { get; set; }

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
            // TODO this is enabled in 2.1
            //return HashCode.Combine(MessageLength, HashFunction.DigestSize, HashFunction.Mode);

            return $"{MessageLength}|{HashFunction.DigestSize}|{HashFunction.Mode}".GetHashCode();
        }
    }
}
