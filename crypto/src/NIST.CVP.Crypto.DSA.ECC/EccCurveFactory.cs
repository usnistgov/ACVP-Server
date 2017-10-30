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
                    return new BinaryCurve(b163F, b163A, b163B, new EccPoint(b163Gx, b163Gy), b163N, b163H);
                case Curve.B233:
                    return new BinaryCurve(b233F, b233A, b233B, new EccPoint(b233Gx, b233Gy), b233N, b233H);
                case Curve.B283:
                    return new BinaryCurve(b283F, b283A, b283B, new EccPoint(b283Gx, b283Gy), b283N, b283H);
                case Curve.B409:
                    return new BinaryCurve(b409F, b409A, b409B, new EccPoint(b409Gx, b409Gy), b409N, b409H);
                case Curve.B571:
                    return new BinaryCurve(b571F, b571A, b571B, new EccPoint(b571Gx, b571Gy), b571N, b571H);

                case Curve.K163:
                    return new BinaryCurve(k163F, k163A, k163B, new EccPoint(k163Gx, k163Gy), k163N, k163H);
                case Curve.K233:
                    return new BinaryCurve(k233F, k233A, k233B, new EccPoint(k233Gx, k233Gy), k233N, k233H);
                case Curve.K283:
                    return new BinaryCurve(k283F, k283A, k283B, new EccPoint(k283Gx, k283Gy), k283N, k283H);
                case Curve.K409:
                    return new BinaryCurve(k409F, k409A, k409B, new EccPoint(k409Gx, k409Gy), k409N, k409H);
                case Curve.K571:
                    return new BinaryCurve(k571F, k571A, k571B, new EccPoint(k571Gx, k571Gy), k571N, k571H);
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
        private readonly BigInteger k163F = new BitString("0800000000000000000000000000000000000000c9").ToPositiveBigInteger();
        private readonly BigInteger k163A = new BitString("01").ToPositiveBigInteger();
        private readonly BigInteger k163B = new BitString("01").ToPositiveBigInteger();
        private readonly BigInteger k163Gx = new BitString("02fe13c0537bbc11acaa07d793de4e6d5e5c94eee8").ToPositiveBigInteger();
        private readonly BigInteger k163Gy = new BitString("0289070fb05d38ff58321f2e800536d538ccdaa3d9").ToPositiveBigInteger();
        private readonly BigInteger k163N = new BitString("04000000000000000000020108a2e0cc0d99f8a5ef").ToPositiveBigInteger();
        private readonly int k163H = 2;

        private readonly BigInteger k233F = new BitString("020000000000000000000000000000000000000004000000000000000001").ToPositiveBigInteger();
        private readonly BigInteger k233A = new BitString("00").ToPositiveBigInteger();
        private readonly BigInteger k233B = new BitString("01").ToPositiveBigInteger();
        private readonly BigInteger k233Gx = new BitString("017232ba853a7e731af129f22ff4149563a419c26bf50a4c9d6eefad6126").ToPositiveBigInteger();
        private readonly BigInteger k233Gy = new BitString("01db537dece819b7f70f555a67c427a8cd9bf18aeb9b56e0c11056fae6a3").ToPositiveBigInteger();
        private readonly BigInteger k233N = new BitString("008000000000000000000000000000069d5bb915bcd46efb1ad5f173abdf").ToPositiveBigInteger();
        private readonly int k233H = 4;

        private readonly BigInteger k283F = new BitString("0800000000000000000000000000000000000000000000000000000000000000000010a1").ToPositiveBigInteger();
        private readonly BigInteger k283A = new BitString("00").ToPositiveBigInteger();
        private readonly BigInteger k283B = new BitString("01").ToPositiveBigInteger();
        private readonly BigInteger k283Gx = new BitString("0503213f78ca44883f1a3b8162f188e553cd265f23c1567a16876913b0c2ac2458492836").ToPositiveBigInteger();
        private readonly BigInteger k283Gy = new BitString("01ccda380f1c9e318d90f95d07e5426fe87e45c0e8184698e45962364e34116177dd2259").ToPositiveBigInteger();
        private readonly BigInteger k283N = new BitString("01ffffffffffffffffffffffffffffffffffe9ae2ed07577265dff7f94451e061e163c61").ToPositiveBigInteger();
        private readonly int k283H = 4;

        private readonly BigInteger k409F = new BitString("02000000000000000000000000000000000000000000000000000000000000000000000000000000008000000000000000000001").ToPositiveBigInteger();
        private readonly BigInteger k409A = new BitString("00").ToPositiveBigInteger();
        private readonly BigInteger k409B = new BitString("01").ToPositiveBigInteger();
        private readonly BigInteger k409Gx = new BitString("60f05f658f49c1ad3ab1890f7184210efd0987e307c84c27accfb8f9f67cc2c460189eb5aaaa62ee222eb1b35540cfe9023746").ToPositiveBigInteger();
        private readonly BigInteger k409Gy = new BitString("01e369050b7c4e42acba1dacbf04299c3460782f918ea427e6325165e9ea10e3da5f6c42e9c55215aa9ca27a5863ec48d8e0286b").ToPositiveBigInteger();
        private readonly BigInteger k409N = new BitString("007ffffffffffffffffffffffffffffffffffffffffffffffffffe5f83b2d4ea20400ec4557d5ed3e3e7ca5b4b5c83b8e01e5fcf").ToPositiveBigInteger();
        private readonly int k409H = 4;

        private readonly BigInteger k571F = new BitString("080000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000425").ToPositiveBigInteger();
        private readonly BigInteger k571A = new BitString("00").ToPositiveBigInteger();
        private readonly BigInteger k571B = new BitString("01").ToPositiveBigInteger();
        private readonly BigInteger k571Gx = new BitString("026eb7a859923fbc82189631f8103fe4ac9ca2970012d5d46024804801841ca44370958493b205e647da304db4ceb08cbbd1ba39494776fb988b47174dca88c7e2945283a01c8972").ToPositiveBigInteger();
        private readonly BigInteger k571Gy = new BitString("0349dc807f4fbf374f4aeade3bca95314dd58cec9f307a54ffc61efc006d8a2c9d4979c0ac44aea74fbebbb9f772aedcb620b01a7ba7af1b320430c8591984f601cd4c143ef1c7a3").ToPositiveBigInteger();
        private readonly BigInteger k571N = new BitString("020000000000000000000000000000000000000000000000000000000000000000000000131850e1f19a63e4b391a8db917f4138b630D84be5d639381e91deb45cfe778f637c1001").ToPositiveBigInteger();
        private readonly int k571H = 4;
        #endregion KCurves

        #region BCurves
        private readonly BigInteger b163F = new BitString("0800000000000000000000000000000000000000c9").ToPositiveBigInteger();
        private readonly BigInteger b163A = new BitString("01").ToPositiveBigInteger();
        private readonly BigInteger b163B = new BitString("020a601907b8c953ca1481eb10512f78744a3205fd").ToPositiveBigInteger();
        private readonly BigInteger b163Gx = new BitString("03f0eba16286a2d57ea0991168d4994637e8343e36").ToPositiveBigInteger();
        private readonly BigInteger b163Gy = new BitString("00d51fbc6c71a0094fa2cdd545b11c5c0c797324f1").ToPositiveBigInteger();
        private readonly BigInteger b163N = new BitString("040000000000000000000292fe77e70c12a4234c33").ToPositiveBigInteger();
        private readonly int b163H = 2;

        private readonly BigInteger b233F = new BitString("020000000000000000000000000000000000000004000000000000000001").ToPositiveBigInteger();
        private readonly BigInteger b233A = new BitString("01").ToPositiveBigInteger();
        private readonly BigInteger b233B = new BitString("66647ede6c332c7f8c0923bb58213b333b20e9ce4281fe115f7d8f90ad").ToPositiveBigInteger();
        private readonly BigInteger b233Gx = new BitString("fac9dfcbac8313bb2139f1bb755fef65bc391f8b36f8f8eb7371fd558b").ToPositiveBigInteger();
        private readonly BigInteger b233Gy = new BitString("01006a08a41903350678e58528bebf8a0beff867a7ca36716f7e01f81052").ToPositiveBigInteger();
        private readonly BigInteger b233N = new BitString("01000000000000000000000000000013e974e72f8a6922031d2603cfe0d7").ToPositiveBigInteger();
        private readonly int b233H = 2;

        private readonly BigInteger b283F = new BitString("0800000000000000000000000000000000000000000000000000000000000000000010a1").ToPositiveBigInteger();
        private readonly BigInteger b283A = new BitString("01").ToPositiveBigInteger();
        private readonly BigInteger b283B = new BitString("027b680ac8b8596da5a4af8a19a0303fca97fd7645309fa2a581485af6263e313b79a2f5").ToPositiveBigInteger();
        private readonly BigInteger b283Gx = new BitString("05f939258db7dd90e1934f8c70b0dfec2eed25b8557eac9c80e2e198f8cdbecd86b12053").ToPositiveBigInteger();
        private readonly BigInteger b283Gy = new BitString("03676854fe24141cb98fe6d4b20d02b4516ff702350eddb0826779c813f0df45be8112f4").ToPositiveBigInteger();
        private readonly BigInteger b283N = new BitString("03ffffffffffffffffffffffffffffffffffef90399660fc938a90165b042a7cefadb307").ToPositiveBigInteger();
        private readonly int b283H = 2;

        private readonly BigInteger b409F = new BitString("02000000000000000000000000000000000000000000000000000000000000000000000000000000008000000000000000000001").ToPositiveBigInteger();
        private readonly BigInteger b409A = new BitString("01").ToPositiveBigInteger();
        private readonly BigInteger b409B = new BitString("21a5c2C8ee9feb5c4b9a753b7b476b7fd6422ef1f3dd674761fa99d6ac27c8a9a197b272822f6cd57a55aa4f50ae317b13545f").ToPositiveBigInteger();
        private readonly BigInteger b409Gx = new BitString("015d4860d088ddb3496b0c6064756260441cde4af1771d4db01ffe5b34e59703dc255a868a1180515603aeab60794e54bb7996a7").ToPositiveBigInteger();
        private readonly BigInteger b409Gy = new BitString("61b1cfab6be5f32bbfa78324ed106a7636b9c5a7bd198d0158aa4f5488d08f38514f1fdf4b4f40d2181b3681c364ba0273c706").ToPositiveBigInteger();
        private readonly BigInteger b409N = new BitString("010000000000000000000000000000000000000000000000000001e2aad6a612f33307be5fa47c3c9e052f838164cd37d9a21173").ToPositiveBigInteger();
        private readonly int b409H = 2;

        private readonly BigInteger b571F = new BitString("080000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000425").ToPositiveBigInteger();
        private readonly BigInteger b571A = new BitString("01").ToPositiveBigInteger();
        private readonly BigInteger b571B = new BitString("02f40e7e2221f295de297117b7f3d62f5c6a97ffcb8ceff1cd6ba8ce4a9a18ad84ffabbd8efa59332be7ad6756a66e294afd185a78ff12aa520e4de739baca0c7ffeff7f2955727a").ToPositiveBigInteger();
        private readonly BigInteger b571Gx = new BitString("0303001d34b856296c16c0d40d3cd7750a93d1d2955fa80aa5f40fc8db7b2abdbde53950f4c0d293cdd711a35b67fb1499ae60038614f1394abfa3b4c850d927e1e7769c8eec2d19").ToPositiveBigInteger();
        private readonly BigInteger b571Gy = new BitString("037bf27342da639b6dccfffeb73d69d78c6c27a6009cbbca1980f8533921e8a684423e43bab08a576291af8f461bb2a8b3531d2f0485c19b16e2f1516e23dd3c1a4827af1b8aC15b").ToPositiveBigInteger();
        private readonly BigInteger b571N = new BitString("03ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffe661ce18ff55987308059b186823851ec7dd9ca1161de93d5174d66e8382e9bb2fe84e47").ToPositiveBigInteger();
        private readonly int b571H = 2;
        #endregion BCurves
    }
}
