using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class TdesParameters : IParameters
    {
        public BlockCipherModesOfOperation Mode { get; set; }
        public int KeyingOption { get; set; }
        public int DataLength { get; set; }
        public string Direction { get; set; }

        public override bool Equals(object other)
        {
            if (other is TdesParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(Mode, KeyingOption, DataLength, Direction);
    }
}
