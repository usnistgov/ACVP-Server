using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class AesWithPayloadParameters : IParameters
    {
        public BlockCipherModesOfOperation Mode { get; set; }
        public BlockCipherDirections Direction { get; set; }
        public BitString Payload { get; set; }
        public BitString Key { get; set; }
        public BitString Iv { get; set; }

        public override bool Equals(object other)
        {
            if (other is AesWithPayloadParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(Mode, Direction, Payload, Key, Iv);
    }
}
