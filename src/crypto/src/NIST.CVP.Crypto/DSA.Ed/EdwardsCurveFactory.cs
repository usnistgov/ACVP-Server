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
                    return new EdwardsCurve(curve, ed25519P, ed25519A, ed25519D, new EdPoint(ed25519Gx, ed25519Gy), ed25519N);
                case Curve.Ed448:
                    return new EdwardsCurve(curve, ed448P, ed448A, ed448D, new EdPoint(ed448Gx, ed448Gy), ed448N);
            }

            throw new ArgumentOutOfRangeException(nameof(curve));
        }

        #region Ed25519
        private readonly BigInteger ed25519P = LoadValue("7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffed");
        private readonly BigInteger ed25519A = LoadValue("7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffec");
        private readonly BigInteger ed25519D = LoadValue("52036cee2b6ffe738cc740797779e89800700a4d4141d8ab75eb4dca135978a3");
        // need to set the rest
        private readonly BigInteger ed25519Gx = LoadValue("188da80eb03090f67cbf20eb43a18800f4ff0afd82ff1012");
        private readonly BigInteger ed25519Gy = LoadValue("07192b95ffc8da78631011ed6b24cdd573f977a11e794811");
        private readonly BigInteger ed25519N = LoadValue("ffffffffffffffffffffffff99def836146bc9b1b4d22831");
        #endregion Ed25519

        #region Ed448
        // need to set these
        private readonly BigInteger ed448P = LoadValue("ffffffffffffffffffffffffffffffff000000000000000000000001");
        private readonly BigInteger ed448A = LoadValue("b4050a850c04b3abf54132565044b0b7d7bfd8ba270b39432355ffb4");
        private readonly BigInteger ed448D = LoadValue("b4050a850c04b3abf54132565044b0b7d7bfd8ba270b39432355ffb4");
        private readonly BigInteger ed448Gx = LoadValue("b70e0cbd6bb4bf7f321390b94a03c1d356c21122343280d6115c1d21");
        private readonly BigInteger ed448Gy = LoadValue("bd376388b5f723fb4c22dfe6cd4375a05a07476444d5819985007e34");
        private readonly BigInteger ed448N = LoadValue("ffffffffffffffffffffffffffff16a2e0b8f03e13dd29455c5c2a3d");
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
