using NIST.CVP.Crypto.Common.Symmetric.Enums;
using System;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class AesParameters : IParameters
    {
        public BlockCipherModesOfOperation Mode { get; set; }
        public int KeyLength { get; set; }
        public int DataLength { get; set; }
        public string Direction { get; set; }

        public override bool Equals(object other)
        {
            if (other is AesParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Mode, KeyLength, DataLength, Direction);
        }
    }
}
