using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Math;
using System;
using System.Numerics;

namespace NIST.CVP.Crypto.DSA.Ed
{
    public class EdwardsCurveFactory : IEdwardsCurveFactory
    {
        public IEdwardsCurve GetCurve(Curve curve)
        {
            switch (curve)
            {
                case Curve.Ed25519:
                    return new EdwardsCurve(curve, ed25519P, ed25519A, ed25519D, new EdPoint(ed25519Gx, ed25519Gy), ed25519N, ed25519b, ed25519n, ed25519c);
                case Curve.Ed448:
                    return new EdwardsCurve(curve, ed448P, ed448A, ed448D, new EdPoint(ed448Gx, ed448Gy), ed448N, ed448b, ed448n, ed448c);
            }

            throw new ArgumentOutOfRangeException(nameof(curve));
        }

        #region Ed25519
        private readonly BigInteger ed25519P = LoadValue("7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffed");
        private readonly BigInteger ed25519A = LoadValue("7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffec");
        private readonly BigInteger ed25519D = LoadValue("52036cee2b6ffe738cc740797779e89800700a4d4141d8ab75eb4dca135978a3");
        private readonly BigInteger ed25519Gx = LoadValue("216936d3cd6e53fec0a4e231fdd6dc5c692cc7609525a7b2c9562d608f25d51a");
        private readonly BigInteger ed25519Gy = LoadValue("6666666666666666666666666666666666666666666666666666666666666658");
        private readonly BigInteger ed25519N = LoadValue("10000000000000000000000000000000014def9dea2f79cd65812631a5cf5d3ed");
        private readonly int ed25519b = 256;
        private readonly int ed25519n = 254;
        private readonly int ed25519c = 3;
        #endregion Ed25519

        #region Ed448
        // need to set these
        private readonly BigInteger ed448P = LoadValue("fffffffffffffffffffffffffffffffffffffffffffffffffffffffeffffffffffffffffffffffffffffffffffffffffffffffffffffffff");
        private readonly BigInteger ed448A = LoadValue("01");
        private readonly BigInteger ed448D = LoadValue("fffffffffffffffffffffffffffffffffffffffffffffffffffffffeffffffffffffffffffffffffffffffffffffffffffffffffffff6756");
        private readonly BigInteger ed448Gx = LoadValue("4f1970c66bed0ded221d15a622bf36da9e146570470f1767ea6de324a3d3a46412ae1af72ab66511433b80e18b00938e2626a82bc70cc05e");
        private readonly BigInteger ed448Gy = LoadValue("693f46716eb6bc248876203756c9c7624bea73736ca3984087789c1e05a0c2d73ad3ff1ce67c39c4fdbd132c4ed7c8ad9808795bf230fa14");
        private readonly BigInteger ed448N = LoadValue("3fffffffffffffffffffffffffffffffffffffffffffffffffffffff7cca23e9c44edb49aed63690216cc2728dc58f552378c292ab5844f3");
        private readonly int ed448b = 456;
        private readonly int ed448n = 447;
        private readonly int ed448c = 2;
        #endregion Ed448

        private static BigInteger LoadValue(string hex)
        {
            // Prepend a "0" if the string isn't valid hex (even number of characters)
            if (hex.Length % 2 != 0)
            {
                hex = "0" + hex;
            }

            return new BitString(hex).ToPositiveBigInteger();
        }
    }
}
