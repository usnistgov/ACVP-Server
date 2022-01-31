using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Math
{
    public class PrimeFieldOperator : IFieldOperator
    {
        private readonly BigInteger _m;

        public PrimeFieldOperator(BigInteger modulo)
        {
            _m = modulo;
        }

        public BigInteger Add(BigInteger a, BigInteger b)
        {
            return Modulo(a + b);
        }

        public BigInteger Divide(BigInteger a, BigInteger b)
        {
            return Multiply(a, Inverse(b));
        }

        public BigInteger Negate(BigInteger a)
        {
            return Modulo(_m - a);
        }

        public BigInteger Inverse(BigInteger a)
        {
            return a.ModularInverse(_m);
        }

        public BigInteger Modulo(BigInteger a)
        {
            return a.PosMod(_m);
        }

        public BigInteger Multiply(BigInteger a, BigInteger b)
        {
            return Modulo(a * b);
        }

        public BigInteger Subtract(BigInteger a, BigInteger b)
        {
            return Modulo(a - b);
        }
    }
}
