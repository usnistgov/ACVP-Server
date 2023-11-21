using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class AesParameters : IParameters
    {
        public BlockCipherModesOfOperation Mode { get; set; }
        public int KeyLength { get; set; }
        public int DataLength { get; set; }
        public string Direction { get; set; }
        public BitString Key { get; set; }
        public override bool Equals(object other)
        {
            if (other is AesParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(Mode, KeyLength, DataLength, Direction);
    }
}
