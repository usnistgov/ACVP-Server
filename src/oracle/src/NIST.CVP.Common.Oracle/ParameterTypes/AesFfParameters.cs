using System;
using NIST.CVP.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class AesFfParameters : IParameters
    {
        public AlgoMode AlgoMode { get; set; }
        public int KeyLength { get; set; }
        public int DataLength { get; set; }
        public BlockCipherDirections Direction { get; set; }
        public int TweakLength { get; set; }
        public int Radix { get; set; }

        public override bool Equals(object other)
        {
            if (other is AesParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(AlgoMode, KeyLength, DataLength, Direction, TweakLength, Radix);
    }
}