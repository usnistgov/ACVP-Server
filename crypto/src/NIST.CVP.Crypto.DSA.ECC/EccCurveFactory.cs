using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class EccCurveFactory : IEccCurveFactory
    {
        public IEccCurve GetCurve(Curve curve)
        {
            switch (curve)
            {
                case Curve.P192:
                    return new PrimeCurve(p192P, p192B, new EccPoint(p192Gx, p192Gy), p192N);
                case Curve.P224:
                    return new PrimeCurve(p224P, p224B, new EccPoint(p224Gx, p224Gy), p224N);
                case Curve.P256:
                    return new PrimeCurve(p256P, p256B, new EccPoint(p256Gx, p256Gy), p256N);
                case Curve.P384:
                    return new PrimeCurve(p384P, p384B, new EccPoint(p384Gx, p384Gy), p384N);
                case Curve.P521:
                    return new PrimeCurve(p521P, p521B, new EccPoint(p521Gx, p521Gy), p521N);

                case Curve.B163:
                    return null;
                case Curve.B233:
                    return null;
                case Curve.B283:
                    return null;
                case Curve.B409:
                    return null;
                case Curve.B571:
                    return null;

                case Curve.K163:
                    return null;
                case Curve.K233:
                    return null;
                case Curve.K283:
                    return null;
                case Curve.K409:
                    return null;
                case Curve.K571:
                    return null;
            }

            throw new ArgumentOutOfRangeException("Curve does not exist");
        }

        #region PCurves
        private readonly BigInteger p192P = new BitString("fffffffffffffffffffffffffffffffeffffffffffffffff").ToPositiveBigInteger();
        private readonly BigInteger p192B = new BitString("64210519e59c80e70fa7e9ab72243049feb8deecc146b9b1").ToPositiveBigInteger();
        private readonly BigInteger p192Gx = new BitString("188da80eb03090f67cbf20eb43a18800f4ff0afd82ff1012").ToPositiveBigInteger();
        private readonly BigInteger p192Gy = new BitString("07192b95ffc8da78631011ed6b24cdd573f977a11e794811").ToPositiveBigInteger();
        private readonly BigInteger p192N = new BitString("ffffffffffffffffffffffff99def836146bc9b1b4d22831").ToPositiveBigInteger();

        private readonly BigInteger p224P = new BitString("ffffffffffffffffffffffffffffffff000000000000000000000001").ToPositiveBigInteger();
        private readonly BigInteger p224B = new BitString("b4050a850c04b3abf54132565044b0b7d7bfd8ba270b39432355ffb4").ToPositiveBigInteger();
        private readonly BigInteger p224Gx = new BitString("b70e0cbd6bb4bf7f321390b94a03c1d356c21122343280d6115c1d21").ToPositiveBigInteger();
        private readonly BigInteger p224Gy = new BitString("bd376388b5f723fb4c22dfe6cd4375a05a07476444d5819985007e34").ToPositiveBigInteger();
        private readonly BigInteger p224N = new BitString("ffffffffffffffffffffffffffff16a2e0b8f03e13dd29455c5c2a3d").ToPositiveBigInteger();

        private readonly BigInteger p256P = new BitString("ffffffff00000001000000000000000000000000ffffffffffffffffffffffff").ToPositiveBigInteger();
        private readonly BigInteger p256B = new BitString("5ac635d8aa3a93e7b3ebbd55769886bc651d06b0cc53b0f63bce3c3e27d2604b").ToPositiveBigInteger();
        private readonly BigInteger p256Gx = new BitString("6b17d1f2e12c4247f8bce6e563a440f277037d812deb33a0f4a13945d898c296").ToPositiveBigInteger();
        private readonly BigInteger p256Gy = new BitString("4fe342e2fe1a7f9b8ee7eb4a7c0f9e162bce33576b315ececbb6406837bf51f5").ToPositiveBigInteger();
        private readonly BigInteger p256N = new BitString("ffffffff00000000ffffffffffffffffbce6faada7179e84f3b9cac2fc632551").ToPositiveBigInteger();

        private readonly BigInteger p384P = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffeffffffff0000000000000000ffffffff").ToPositiveBigInteger();
        private readonly BigInteger p384B = new BitString("b3312fa7e23ee7e4988e056be3f82d19181d9c6efe8141120314088f5013875ac656398d8a2ed19d2a85c8edd3ec2aef").ToPositiveBigInteger();
        private readonly BigInteger p384Gx = new BitString("aa87Ca22be8b05378eb1c71ef320ad746e1d3b628ba79b9859f741e082542a385502f25dbf55296c3a545e3872760ab7").ToPositiveBigInteger();
        private readonly BigInteger p384Gy = new BitString("3617de4a96262c6f5d9e98bf9292dc29f8f41dbd289a147ce9da3113b5f0b8c00a60b1ce1d7e819d7a431d7c90ea0e5f").ToPositiveBigInteger();
        private readonly BigInteger p384N = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffc7634d81f4372ddf581a0db248b0a77aecec196accc52973").ToPositiveBigInteger();

        private readonly BigInteger p521P = new BitString("01ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff").ToPositiveBigInteger();
        private readonly BigInteger p521B = new BitString("0051953eb9618e1c9a1f929a21a0b68540eea2da725b99b315f3b8b489918ef109e156193951ec7e937b1652c0bd3bb1bf073573df883d2C34f1ef451fd46b503f00").ToPositiveBigInteger();
        private readonly BigInteger p521Gx = new BitString("00c6858e06b70404e9cd9e3ecb662395b4429c648139053fb521f828af606b4d3dbaa14b5e77efe75928fe1dc127a2ffa8de3348b3c1856a429bf97e7e31c2e5bd66").ToPositiveBigInteger();
        private readonly BigInteger p521Gy = new BitString("011839296a789a3bc0045c8a5fb42c7d1bd998f54449579b446817afbd17273e662c97ee72995ef42640c550b9013fad0761353c7086a272c24088be94769fd16650").ToPositiveBigInteger();
        private readonly BigInteger p521N = new BitString("01fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffa51868783bf2f966b7fcc0148f709a5d03bb5c9b8899c47aebb6fb71e91386409").ToPositiveBigInteger();
        #endregion PCurves

        #region KCurves
        //private KCurve K163 = new KCurve();
        //private KCurve K233 = new KCurve();
        //private KCurve K283 = new KCurve();
        //private KCurve K409 = new KCurve();
        //private KCurve K571 = new KCurve();
        #endregion KCurves

        #region BCurves
        //private BCurve B163 = new BCurve();
        //private BCurve B233 = new BCurve();
        //private BCurve B283 = new BCurve();
        //private BCurve B409 = new BCurve();
        //private BCurve B571 = new BCurve();
        #endregion BCurves
    }
}
