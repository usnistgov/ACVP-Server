using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public static class BigIntegerExtensions
    {
        /// <summary>
        /// Takes the modulo of a value (or <see cref="BigInteger"/> expression) and ensures it is between [0, <paramref name="modulo"/> - 1]
        /// </summary>
        /// <param name="value"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public static BigInteger PosMod(this BigInteger value, BigInteger modulo)
        {
            return ((value % modulo) + modulo) % modulo;
        }
    }
}
