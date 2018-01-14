using System;
using System.Collections.Generic;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
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
                    return new PrimeCurve(curve, p192P, p192B, new EccPoint(p192Gx, p192Gy), p192N);
                case Curve.P224:
                    return new PrimeCurve(curve, p224P, p224B, new EccPoint(p224Gx, p224Gy), p224N);
                case Curve.P256:
                    return new PrimeCurve(curve, p256P, p256B, new EccPoint(p256Gx, p256Gy), p256N);
                case Curve.P384:
                    return new PrimeCurve(curve, p384P, p384B, new EccPoint(p384Gx, p384Gy), p384N);
                case Curve.P521:
                    return new PrimeCurve(curve, p521P, p521B, new EccPoint(p521Gx, p521Gy), p521N);

                case Curve.B163:
                    return new BinaryCurve(curve, b163F, b163A, b163B, new EccPoint(b163Gx, b163Gy), b163N, b163H, preComputedB163);
                case Curve.B233:
                    return new BinaryCurve(curve, b233F, b233A, b233B, new EccPoint(b233Gx, b233Gy), b233N, b233H, preComputedB233);
                case Curve.B283:
                    return new BinaryCurve(curve, b283F, b283A, b283B, new EccPoint(b283Gx, b283Gy), b283N, b283H, preComputedB283);
                case Curve.B409:
                    return new BinaryCurve(curve, b409F, b409A, b409B, new EccPoint(b409Gx, b409Gy), b409N, b409H, preComputedB409);
                case Curve.B571:
                    return new BinaryCurve(curve, b571F, b571A, b571B, new EccPoint(b571Gx, b571Gy), b571N, b571H, preComputedB571);

                case Curve.K163:
                    return new BinaryCurve(curve, k163F, k163A, k163B, new EccPoint(k163Gx, k163Gy), k163N, k163H, preComputedK163);
                case Curve.K233:
                    return new BinaryCurve(curve, k233F, k233A, k233B, new EccPoint(k233Gx, k233Gy), k233N, k233H, preComputedK233);
                case Curve.K283:
                    return new BinaryCurve(curve, k283F, k283A, k283B, new EccPoint(k283Gx, k283Gy), k283N, k283H, preComputedK283);
                case Curve.K409:
                    return new BinaryCurve(curve, k409F, k409A, k409B, new EccPoint(k409Gx, k409Gy), k409N, k409H, preComputedK409);
                case Curve.K571:
                    return new BinaryCurve(curve, k571F, k571A, k571B, new EccPoint(k571Gx, k571Gy), k571N, k571H, preComputedK571);
            }

            throw new ArgumentOutOfRangeException(nameof(curve));
        }

        #region PCurves
        private readonly BigInteger p192P = LoadValue("fffffffffffffffffffffffffffffffeffffffffffffffff");
        private readonly BigInteger p192B = LoadValue("64210519e59c80e70fa7e9ab72243049feb8deecc146b9b1");
        private readonly BigInteger p192Gx = LoadValue("188da80eb03090f67cbf20eb43a18800f4ff0afd82ff1012");
        private readonly BigInteger p192Gy = LoadValue("07192b95ffc8da78631011ed6b24cdd573f977a11e794811");
        private readonly BigInteger p192N = LoadValue("ffffffffffffffffffffffff99def836146bc9b1b4d22831");

        private readonly BigInteger p224P = LoadValue("ffffffffffffffffffffffffffffffff000000000000000000000001");
        private readonly BigInteger p224B = LoadValue("b4050a850c04b3abf54132565044b0b7d7bfd8ba270b39432355ffb4");
        private readonly BigInteger p224Gx = LoadValue("b70e0cbd6bb4bf7f321390b94a03c1d356c21122343280d6115c1d21");
        private readonly BigInteger p224Gy = LoadValue("bd376388b5f723fb4c22dfe6cd4375a05a07476444d5819985007e34");
        private readonly BigInteger p224N = LoadValue("ffffffffffffffffffffffffffff16a2e0b8f03e13dd29455c5c2a3d");

        private readonly BigInteger p256P = LoadValue("ffffffff00000001000000000000000000000000ffffffffffffffffffffffff");
        private readonly BigInteger p256B = LoadValue("5ac635d8aa3a93e7b3ebbd55769886bc651d06b0cc53b0f63bce3c3e27d2604b");
        private readonly BigInteger p256Gx = LoadValue("6b17d1f2e12c4247f8bce6e563a440f277037d812deb33a0f4a13945d898c296");
        private readonly BigInteger p256Gy = LoadValue("4fe342e2fe1a7f9b8ee7eb4a7c0f9e162bce33576b315ececbb6406837bf51f5");
        private readonly BigInteger p256N = LoadValue("ffffffff00000000ffffffffffffffffbce6faada7179e84f3b9cac2fc632551");

        private readonly BigInteger p384P = LoadValue("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffeffffffff0000000000000000ffffffff");
        private readonly BigInteger p384B = LoadValue("b3312fa7e23ee7e4988e056be3f82d19181d9c6efe8141120314088f5013875ac656398d8a2ed19d2a85c8edd3ec2aef");
        private readonly BigInteger p384Gx = LoadValue("aa87Ca22be8b05378eb1c71ef320ad746e1d3b628ba79b9859f741e082542a385502f25dbf55296c3a545e3872760ab7");
        private readonly BigInteger p384Gy = LoadValue("3617de4a96262c6f5d9e98bf9292dc29f8f41dbd289a147ce9da3113b5f0b8c00a60b1ce1d7e819d7a431d7c90ea0e5f");
        private readonly BigInteger p384N = LoadValue("ffffffffffffffffffffffffffffffffffffffffffffffffc7634d81f4372ddf581a0db248b0a77aecec196accc52973");

        private readonly BigInteger p521P = LoadValue("01ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff");
        private readonly BigInteger p521B = LoadValue("0051953eb9618e1c9a1f929a21a0b68540eea2da725b99b315f3b8b489918ef109e156193951ec7e937b1652c0bd3bb1bf073573df883d2C34f1ef451fd46b503f00");
        private readonly BigInteger p521Gx = LoadValue("00c6858e06b70404e9cd9e3ecb662395b4429c648139053fb521f828af606b4d3dbaa14b5e77efe75928fe1dc127a2ffa8de3348b3c1856a429bf97e7e31c2e5bd66");
        private readonly BigInteger p521Gy = LoadValue("011839296a789a3bc0045c8a5fb42c7d1bd998f54449579b446817afbd17273e662c97ee72995ef42640c550b9013fad0761353c7086a272c24088be94769fd16650");
        private readonly BigInteger p521N = LoadValue("01fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffa51868783bf2f966b7fcc0148f709a5d03bb5c9b8899c47aebb6fb71e91386409");
        #endregion PCurves

        #region KCurves
        private readonly BigInteger k163F = LoadValue("0800000000000000000000000000000000000000c9");
        private readonly BigInteger k163A = LoadValue("01");
        private readonly BigInteger k163B = LoadValue("01");
        private readonly BigInteger k163Gx = LoadValue("02fe13c0537bbc11acaa07d793de4e6d5e5c94eee8");
        private readonly BigInteger k163Gy = LoadValue("0289070fb05d38ff58321f2e800536d538ccdaa3d9");
        private readonly BigInteger k163N = LoadValue("04000000000000000000020108a2e0cc0d99f8a5ef");
        private readonly int k163H = 2;

        private readonly BigInteger k233F = LoadValue("020000000000000000000000000000000000000004000000000000000001");
        private readonly BigInteger k233A = LoadValue("00");
        private readonly BigInteger k233B = LoadValue("01");
        private readonly BigInteger k233Gx = LoadValue("017232ba853a7e731af129f22ff4149563a419c26bf50a4c9d6eefad6126");
        private readonly BigInteger k233Gy = LoadValue("01db537dece819b7f70f555a67c427a8cd9bf18aeb9b56e0c11056fae6a3");
        private readonly BigInteger k233N = LoadValue("008000000000000000000000000000069d5bb915bcd46efb1ad5f173abdf");
        private readonly int k233H = 4;

        private readonly BigInteger k283F = LoadValue("0800000000000000000000000000000000000000000000000000000000000000000010a1");
        private readonly BigInteger k283A = LoadValue("00");
        private readonly BigInteger k283B = LoadValue("01");
        private readonly BigInteger k283Gx = LoadValue("0503213f78ca44883f1a3b8162f188e553cd265f23c1567a16876913b0c2ac2458492836");
        private readonly BigInteger k283Gy = LoadValue("01ccda380f1c9e318d90f95d07e5426fe87e45c0e8184698e45962364e34116177dd2259");
        private readonly BigInteger k283N = LoadValue("01ffffffffffffffffffffffffffffffffffe9ae2ed07577265dff7f94451e061e163c61");
        private readonly int k283H = 4;

        private readonly BigInteger k409F = LoadValue("02000000000000000000000000000000000000000000000000000000000000000000000000000000008000000000000000000001");
        private readonly BigInteger k409A = LoadValue("00");
        private readonly BigInteger k409B = LoadValue("01");
        private readonly BigInteger k409Gx = LoadValue("60f05f658f49c1ad3ab1890f7184210efd0987e307c84c27accfb8f9f67cc2c460189eb5aaaa62ee222eb1b35540cfe9023746");
        private readonly BigInteger k409Gy = LoadValue("01e369050b7c4e42acba1dacbf04299c3460782f918ea427e6325165e9ea10e3da5f6c42e9c55215aa9ca27a5863ec48d8e0286b");
        private readonly BigInteger k409N = LoadValue("007ffffffffffffffffffffffffffffffffffffffffffffffffffe5f83b2d4ea20400ec4557d5ed3e3e7ca5b4b5c83b8e01e5fcf");
        private readonly int k409H = 4;

        private readonly BigInteger k571F = LoadValue("080000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000425");
        private readonly BigInteger k571A = LoadValue("00");
        private readonly BigInteger k571B = LoadValue("01");
        private readonly BigInteger k571Gx = LoadValue("026eb7a859923fbc82189631f8103fe4ac9ca2970012d5d46024804801841ca44370958493b205e647da304db4ceb08cbbd1ba39494776fb988b47174dca88c7e2945283a01c8972");
        private readonly BigInteger k571Gy = LoadValue("0349dc807f4fbf374f4aeade3bca95314dd58cec9f307a54ffc61efc006d8a2c9d4979c0ac44aea74fbebbb9f772aedcb620b01a7ba7af1b320430c8591984f601cd4c143ef1c7a3");
        private readonly BigInteger k571N = LoadValue("020000000000000000000000000000000000000000000000000000000000000000000000131850e1f19a63e4b391a8db917f4138b630D84be5d639381e91deb45cfe778f637c1001");
        private readonly int k571H = 4;
        #endregion KCurves

        #region BCurves
        private readonly BigInteger b163F = LoadValue("0800000000000000000000000000000000000000c9");
        private readonly BigInteger b163A = LoadValue("01");
        private readonly BigInteger b163B = LoadValue("020a601907b8c953ca1481eb10512f78744a3205fd");
        private readonly BigInteger b163Gx = LoadValue("03f0eba16286a2d57ea0991168d4994637e8343e36");
        private readonly BigInteger b163Gy = LoadValue("00d51fbc6c71a0094fa2cdd545b11c5c0c797324f1");
        private readonly BigInteger b163N = LoadValue("040000000000000000000292fe77e70c12a4234c33");
        private readonly int b163H = 2;

        private readonly BigInteger b233F = LoadValue("020000000000000000000000000000000000000004000000000000000001");
        private readonly BigInteger b233A = LoadValue("01");
        private readonly BigInteger b233B = LoadValue("66647ede6c332c7f8c0923bb58213b333b20e9ce4281fe115f7d8f90ad");
        private readonly BigInteger b233Gx = LoadValue("fac9dfcbac8313bb2139f1bb755fef65bc391f8b36f8f8eb7371fd558b");
        private readonly BigInteger b233Gy = LoadValue("01006a08a41903350678e58528bebf8a0beff867a7ca36716f7e01f81052");
        private readonly BigInteger b233N = LoadValue("01000000000000000000000000000013e974e72f8a6922031d2603cfe0d7");
        private readonly int b233H = 2;

        private readonly BigInteger b283F = LoadValue("0800000000000000000000000000000000000000000000000000000000000000000010a1");
        private readonly BigInteger b283A = LoadValue("01");
        private readonly BigInteger b283B = LoadValue("027b680ac8b8596da5a4af8a19a0303fca97fd7645309fa2a581485af6263e313b79a2f5");
        private readonly BigInteger b283Gx = LoadValue("05f939258db7dd90e1934f8c70b0dfec2eed25b8557eac9c80e2e198f8cdbecd86b12053");
        private readonly BigInteger b283Gy = LoadValue("03676854fe24141cb98fe6d4b20d02b4516ff702350eddb0826779c813f0df45be8112f4");
        private readonly BigInteger b283N = LoadValue("03ffffffffffffffffffffffffffffffffffef90399660fc938a90165b042a7cefadb307");
        private readonly int b283H = 2;

        private readonly BigInteger b409F = LoadValue("02000000000000000000000000000000000000000000000000000000000000000000000000000000008000000000000000000001");
        private readonly BigInteger b409A = LoadValue("01");
        private readonly BigInteger b409B = LoadValue("21a5c2C8ee9feb5c4b9a753b7b476b7fd6422ef1f3dd674761fa99d6ac27c8a9a197b272822f6cd57a55aa4f50ae317b13545f");
        private readonly BigInteger b409Gx = LoadValue("015d4860d088ddb3496b0c6064756260441cde4af1771d4db01ffe5b34e59703dc255a868a1180515603aeab60794e54bb7996a7");
        private readonly BigInteger b409Gy = LoadValue("61b1cfab6be5f32bbfa78324ed106a7636b9c5a7bd198d0158aa4f5488d08f38514f1fdf4b4f40d2181b3681c364ba0273c706");
        private readonly BigInteger b409N = LoadValue("010000000000000000000000000000000000000000000000000001e2aad6a612f33307be5fa47c3c9e052f838164cd37d9a21173");
        private readonly int b409H = 2;

        private readonly BigInteger b571F = LoadValue("080000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000425");
        private readonly BigInteger b571A = LoadValue("01");
        private readonly BigInteger b571B = LoadValue("02f40e7e2221f295de297117b7f3d62f5c6a97ffcb8ceff1cd6ba8ce4a9a18ad84ffabbd8efa59332be7ad6756a66e294afd185a78ff12aa520e4de739baca0c7ffeff7f2955727a");
        private readonly BigInteger b571Gx = LoadValue("0303001d34b856296c16c0d40d3cd7750a93d1d2955fa80aa5f40fc8db7b2abdbde53950f4c0d293cdd711a35b67fb1499ae60038614f1394abfa3b4c850d927e1e7769c8eec2d19");
        private readonly BigInteger b571Gy = LoadValue("037bf27342da639b6dccfffeb73d69d78c6c27a6009cbbca1980f8533921e8a684423e43bab08a576291af8f461bb2a8b3531d2f0485c19b16e2f1516e23dd3c1a4827af1b8aC15b");
        private readonly BigInteger b571N = LoadValue("03ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffe661ce18ff55987308059b186823851ec7dd9ca1161de93d5174d66e8382e9bb2fe84e47");
        private readonly int b571H = 2;
        #endregion BCurves

        #region PreComputationB
        private readonly List<EccPoint> preComputedB163 = new List<EccPoint>
        {
            new EccPoint("infinity"),
            new EccPoint(LoadValue("03f0eba16286a2d57ea0991168d4994637e8343e36"), LoadValue("00d51fbc6c71a0094fa2cdd545b11c5c0c797324f1")),
            new EccPoint(LoadValue("0275f1f52731dd5c036648e974aa17fdf205fd9372"), LoadValue("00b09149609865bdb25d2ff66279117c6adcf990e0")),
            new EccPoint(LoadValue("0534c6885e3b85b2633d3d2364e478fe5878a887a6"), LoadValue("03bf414f7475cc731c53001bd9a6d7e436ef273598")),
            new EccPoint(LoadValue("02baa8682a6f4913ffd1c71b1f6f8daf884f685f08"), LoadValue("0265c6e40104ebaf456454423bb4f6dd1ed5301336")),
            new EccPoint(LoadValue("0648fcdc9504db6da9e8f5f5fd6f842a26a700c73d"), LoadValue("072cf27012466e5be3abfd25b4b5bf42ba1db757cb")),
            new EccPoint(LoadValue("003893822e2fcd54835f9c4ef147e38e87eb7ad12e"), LoadValue("01edd41659dfa83842680ca2a3a41f95897c98bf09")),
            new EccPoint(LoadValue("0290cd449b84b3c40d2288658aaf8c4bb0a3463d19"), LoadValue("05fbf58239ff2113079b2ae97a5e996afa6bb4bf11")),
            new EccPoint(LoadValue("07dbca45da76f09ad93c7efb852fcf02ee4ab8c649"), LoadValue("04271d1b843292b2c1d25e376059adf276eeba93af")),
            new EccPoint(LoadValue("0463075e194c9036e0bf079624f423fd6037f9391c"), LoadValue("020cb8d73b87b541416e0988b738c349b3b726dfb5")),
            new EccPoint(LoadValue("02b44b3919c6708b6f690dfa7364ca1fccd1acdb88"), LoadValue("009118031edb9fdd9a5311dcc8c01ccb1e2b3f67d9")),
            new EccPoint(LoadValue("05ef0f58d78db0dcccaf8532324e99152f9a5c0d6e"), LoadValue("038d1169b456a979bccc819e67efdd7e0a6db63148")),
            new EccPoint(LoadValue("028096413e5cc946556ed8588a3d61a2ac8670b917"), LoadValue("028a20fadc1553a69bd50c5c77c2ce06e6d8715f72")),
            new EccPoint(LoadValue("01f57da9a2adbac1727aa617bbb29788939dd077b2"), LoadValue("00b5ec04b21082c382dac7cd95837ad74e6800bbe2")),
            new EccPoint(LoadValue("036d4462d4fd642a8d8d7f7c4ea986d58b43835328"), LoadValue("065d7f45931ab9c90d67c98e9b60993930f19901e9")),
            new EccPoint(LoadValue("036b5e56c05bcb53b5a867db73de36673039e10752"), LoadValue("06221a9b1f52b45c1a2221c8618bdbf9ffaf59dcaa")),
        };

        private readonly List<EccPoint> preComputedB233 = new List<EccPoint>
        {
            new EccPoint("infinity"),
            new EccPoint(LoadValue("00fac9dfcbac8313bb2139f1bb755fef65bc391f8b36f8f8eb7371fd558b"), LoadValue("01006a08a41903350678e58528bebf8a0beff867a7ca36716f7e01f81052")),
            new EccPoint(LoadValue("01c7f30ea26039f1c5e6f88243dbac4fe43d5502a2bb91e9df26adec3bca"), LoadValue("00e9e8d2f2db1abdcefb4e9a83bd0cc149c3fa309a93c113ec0f6c7b1d42")),
            new EccPoint(LoadValue("00d68a5d0f219fdb68d76adab54e21932c72ff432fe0a7ad0047d89d5bbf"), LoadValue("001187cd25dc635d1d8f41148cde5507c4d23823de5cfd38189a87c1b7cc")),
            new EccPoint(LoadValue("016f13e0dc44422f759154e0b636897c269c962b28189e1607e2c458ea0b"), LoadValue("00d528440b7a49df896b7773655a89e6f28859520d6c31c6eaabdf1d5778")),
            new EccPoint(LoadValue("005f406062f748a06c3abe2ef44ae0658d9195544bfe486b455193c2c8f6"), LoadValue("016069d13a5537bf02c35b9ddee574c58f85a737d211fa6979fee79b8443")),
            new EccPoint(LoadValue("0161c47a4fd51f1032e958fd2722ad760dab89520f9bf4440358cbacc21b"), LoadValue("0180d06df5ff806a89b8c888ebd2a2e441940f72e8ade21c4f2060e85da0")),
            new EccPoint(LoadValue("0017552a5edbba5c799a3c296af703f39d637e73c6a52b8aa7a4037e2d47"), LoadValue("00e515f29792f4cd1b2eea786420ff723918bfee64127e0034910dc6bffe")),
            new EccPoint(LoadValue("019a1ac36145c6ab66f8089414912b19f4cf9f7e2684af1c394bb16435df"), LoadValue("013a817f1116cf904c038efa504604dd075cc1284e5f9604747a3946c962")),
            new EccPoint(LoadValue("01502ee42ee278cf0de15fe09a448d8f31ce57787aa0b98a925d7e82c413"), LoadValue("009a2c801803ba6cdfe69a437f8cdd55ed8279f5ca39bc06a5f01da4460a")),
            new EccPoint(LoadValue("0197aa515e9c84769e6f0f17d9d493400ad49b1fb42dc9de7c7aa1ccb497"), LoadValue("010d6c46598ab8565d7738ec2c7d639185ee9d35bc529fa132602a5d7ff7")),
            new EccPoint(LoadValue("0062c2bc6fd8889ab062a423460d13db50470fcc7e6582af9cfcd9ea158c"), LoadValue("017fa6f3293d980d1fd8820a6c4d21f341770f5bd6779f33151026ff3618")),
            new EccPoint(LoadValue("0132442364715a6f87d841b8ac15bf7653a3b0d54a5013949332bd1b8ca9"), LoadValue("0069272446d8a871b435aed796343de246016bb3b49400067b8aef9926ed")),
            new EccPoint(LoadValue("016630aed2f3d90a19302eafd57923b225d91e3e006c56e6c668ddd87adf"), LoadValue("0099aa5800b5a0a40af8f62b392ca98a97ad0a551f056b1d1f401f88a6d6")),
            new EccPoint(LoadValue("01355d15b0d8b2b755bac5e4fbe42c68ffb6a439afa53458bfd26b25f896"), LoadValue("00994c7d9f3767ecc752d4ce7cb78690e23cb8eb752189e7079142547dba")),
            new EccPoint(LoadValue("00004158ce5e0db19a404bb63c5d15b409636b7b9f2b1a005a87d415e56f"), LoadValue("01cdf4877db09b674434dc7350bb0890705553142bb88c4fe8d9b565ab54")),
        };

        private readonly List<EccPoint> preComputedB283 = new List<EccPoint>
        {
            new EccPoint("infinity"),
            new EccPoint(LoadValue("5f939258db7dd90e1934f8c70b0dfec2eed25b8557eac9c80e2e198f8cdbecd86b12053"),LoadValue("3676854fe24141cb98fe6d4b20d02b4516ff702350eddb0826779c813f0df45be8112f4")),
            new EccPoint(LoadValue("4b04cb833691637263eca7449917c00f33245aba9d9db0178935c7a7692a79fff1f4b31"),LoadValue("081ff57fc68115ef83070e97f72e914cd09a27c3a72648904d68562bb946b579d65a35c")),
            new EccPoint(LoadValue("3323b5253c0762e9e755b2d585a6572a72602c5c23f32d3961b46c3ea5a0b1922d5ba81"),LoadValue("0aaaa728a3401c69e424e3957f06333a728a76e1d58c2e5ac4102c1e7849f847f122afa")),
            new EccPoint(LoadValue("370f9591f4c6f6c7c8f5ad8ac2a51bedb7f7062505bce6c6d2cee294ff98744f28a8e28"),LoadValue("0a9a6a3daa0a281fde20de9aaadf3b0088c7a465003d85f832d6629a1af12d51576a4ac")),
            new EccPoint(LoadValue("1bcc7c0d2cd86e607d461be888ff080bd29be44764ae9c2e7fc48fed57bc45b770bbf9a"),LoadValue("7086b87e6cca778c1c3a502fa6bcb9968eb497af9fc8f13a17cb2afa3bf4dfc0c2c2a83")),
            new EccPoint(LoadValue("3bbf8ddb2f773ad44128c60b5bc21db8662810724c59efa5e0fe83afa0ba615daf97308"),LoadValue("2921d2c78868411bd77e6e623cf216cd0f7501c1d87c5a9b38e6c086a87b5d288512ed7")),
            new EccPoint(LoadValue("0792bfb2e42b909fd37009fe1a2480626f43bbb48b2ad0625ed4b82e371b7571ae44b81"),LoadValue("1c9f92a472d6e000c3c94c5945d13120785606dbe03615c59c947c97f4dcd7cecf07045")),
            new EccPoint(LoadValue("5791913c094bb22ea337af040fc45ed22f23f801a8217b1742a31a4d82f8ec0013626fd"),LoadValue("4f356beae4fb3b86ab4058c13c7621fd898bc81bce97b65a105192e60a40389c0caf122")),
            new EccPoint(LoadValue("5194f17d8ffa8ebb069dfe2360d2315c861fd3b973c75315208ce36d426a6cad6199317"),LoadValue("69264095ac1c47a3ea1812b99794d70ddb3030a102f2212906f65032c5ce1e9ab41e825")),
            new EccPoint(LoadValue("4b7873366ac11d97d8d44489129dd4a7772258ee5d1fecd9e8e433278fb29618d8311ea"),LoadValue("21ef6100697afc0ffa20e42a41bb1395f82b84ce723788564747a1a4ac94b19ddb604b3")),
            new EccPoint(LoadValue("0bffafd298d5bf9cfc1784d47271b59d8246330aeada8225ecb6e95e4fa5206198fd978"),LoadValue("11a143fd2c0794be7242a6b5221a28cbcae44c282b12c08c6b6a10d46813ccbc903f895")),
            new EccPoint(LoadValue("191ac5b6e53b5f01497a5335009e922b0dec2d9ce68412ae176ae3a9b8ce4ad8025fbe8"),LoadValue("55b214c0d70273adf73a2b307a47369164fb87beac2bd3940cbc1d2748a0a39f726636e")),
            new EccPoint(LoadValue("7f5899b77b1e12b16d39c01c84b9994b022e721c54fefaa5ea44e3a88c588157d7204b2"),LoadValue("3e8bf6ddbfb59c177267927c2474f500693f417a5af6f350199aa45cb019c165f8c8cdd")),
            new EccPoint(LoadValue("10b07d94f58736ba8805288ef8764cf4a50d4340d6403b9ad4d80c75da072a54ea55103"),LoadValue("6aedcfc6c0836328bb16d876d9366f674f890d9161285faa3082a84e9679d208f837a7c")),
            new EccPoint(LoadValue("79c3e3257f01934efd1a42edd253cbd93653ae610311f1ac0c8227dcba200216cb01239"),LoadValue("0bb90c1fe3f56853eb0b7c41390d5f4ff1ef5836b9c15143e1ca47926c0bd11d0e9054f"))
        };

        private readonly List<EccPoint> preComputedB409 = new List<EccPoint>
        {
            new EccPoint("infinity"),
            new EccPoint(LoadValue("15d4860d088ddb3496b0c6064756260441cde4af1771d4db01ffe5b34e59703dc255a868a1180515603aeab60794e54bb7996a7"),LoadValue("061b1cfab6be5f32bbfa78324ed106a7636b9c5a7bd198d0158aa4f5488d08f38514f1fdf4b4f40d2181b3681c364ba0273c706")),
            new EccPoint(LoadValue("115c7d8464d91ce03a8c800a9def7651e50a1ac26d874fd46d45dd5c55d171dc9564675ce7888a4c7fb893683c4c5ef2cafd59f"),LoadValue("1bc16530377d8492a90e8e7a6fae930b3484a1924a0c84dbcd42fcfc4864c3e2da05d60d238cfea7d10852e611fe41827947fb2")),
            new EccPoint(LoadValue("06c4983703d108167f9f504549c530ad33fb8a2fb219e2fe2f68dfe50c2400f0b8ea1b8a10b57f5f9175621a52108da6853e7cf"),LoadValue("05c8deba62f403c8f81c5d2f3a56ff63f3451bedabb1d554c96f8e67af6dc8cd16c5f6cc13946c8eb1e75d26cbe5ac059465a9f")),
            new EccPoint(LoadValue("154c757de9a1aa58cda24f34a324d5ce695dbc5ae1cee7c808b7f6efe00f2e12cf6937879049c8094aefd01643798f21a22f03b"),LoadValue("178714afd80f2d2570cd0e7427cc7d7054fb6ed82d7fb51a3ac402afea623b773081c2a8e448a6d5f874b2470e386dd82c8d395")),
            new EccPoint(LoadValue("051e4f2d8a56fd8b196d1f04bb99e83a75f7a62ca23892c19371f2e24c57dc61a8f185c5f5f32f427efb93d2fbc9e95a4004d51"),LoadValue("0ad1d4694f101421e9d423d9ca213b3bb494c7701a8de7b086f1b674224b528a07e426e77bb9f78fd391589b127a3a9f71cc7ed")),
            new EccPoint(LoadValue("1b6aedbb2ad1041ba7983e1b26c8680952fefe8c80f963085c692de263740c0912f82afb0b23764cad23809e9d7137bf001a48b"),LoadValue("07ca5800ae54143771ab93f89efcedaa6f36b010e7b704224295a533e46954b3130b865a93742226b32568aa3c15ebdb29c1460")),
            new EccPoint(LoadValue("05e0bcd7e385337549d68967d047269beff821162218ab7776c8e39e9c708ab2271e2951bd058cd58ae29d6e845a2fce5e2b7cd"),LoadValue("13a5290332d26c6bc33cefbf52e06c988315cf1dddf735d4cfbff5bd0a81ce47edef37b64f6e2a8eb2f44cf704ece52c5efc35e")),
            new EccPoint(LoadValue("14470615d1ddf1d594dc82ba70748bdd42a99b3bf437b29d501e74fdef7c66de5e098169a245a1d2894353ac38a7ab5866f9a7d"),LoadValue("04091712e0c5b277b7ad6f99845259acd3f1786c1b922bde451ac872044aa9f7729f7dba2ad0f460e31939310eebe74dbd0d95d")),
            new EccPoint(LoadValue("11008a58d0f06a279d10652f02470a8c24b12a6a6757cc290cbee31e624d2dfe4379bd85d203701266e124aee227455095d641d"),LoadValue("14cafbf2db73ba75b78b312ed4c1e9b5ddaa9420d17a2845dfdc8d588a1b5aacb4659af1622394a10e3b71ff598f678e487648c")),
            new EccPoint(LoadValue("1daf18756f0d69b65e7ed63d7358547c8adf146e384edef16b9d70c0a1686dd28bf73c74d285fd979c6d3b4f3c10aec9a763457"),LoadValue("17e2bf6b8a10f26ac6d684183d84da33b7c9f5e29c6b5fb32a148c2069740fc1d6ae1e4ef8df7680c80d2cf1e5119afb4c8bf5e")),
            new EccPoint(LoadValue("0247e5c4ea36005399ffe7b2091a1aa5474619fafbd0a5a75736e53f16d4504cb60f1770fa505f6b9b97fc4c671bea9431eadd9"),LoadValue("02dcea7e3f4d930920ac016c1cfc8157b6465f9ec4e6250227fbe892d08c99913b1816e347f9fa47f15b100b06669b711f7cebe")),
            new EccPoint(LoadValue("1f540d6ebbf3361f64e7ca8449d5ef9f80b13345b8a960986e4c55fe3d5493f503252f8d0716b208cfd2934e86f9b2b3730448d"),LoadValue("191f3c8e826dbfc7861bd3f58b8101ec65687b6079ef4e4b8438ed252649dd1936a6652c24560191485cb385cce11f31fca966e")),
            new EccPoint(LoadValue("17b18815f550b5e980581653a6a03fb54fa3fa9f120cfda1a246f94552e01cf1f6df028b4b7a8ca39cbda001bc806ca141c514d"),LoadValue("15bc2378c7ef90ec30297ba80f3a9e815ed93e4ddfc1448a5c8fb6d8719d00a673cb5dd79ec2681be50cd1be4629df1114be663")),
            new EccPoint(LoadValue("12e47d4473934dcb392cc0412ab5c2fdb50eab4ffe6dcc02bfffa8f836cfedced257d1f45f0925848cdc25b18fa3c4b7cef7139"),LoadValue("0ab6952f7529281d0900ef18f5265f53afd366769617a452e91ddf9bb144d741d0556273ab37983d92a6c6328e10d1bc259846c")),
            new EccPoint(LoadValue("0095404d0d77c405bd731dbbe36318b79d8da01430ec8bcfb6dceeef44ab67c4345c41408529b54e362d3eaf236d3dcbcf81e8f"),LoadValue("1d5f9e5db74539e65b838a8c53e2fc96bfba6c9ed687061873d4f2fbb33b4d8ac061072193235cd9d1dc65caea8cb6f7f352f80"))
        };

        private readonly List<EccPoint> preComputedB571 = new List<EccPoint>
        {
            new EccPoint("infinity"),
            new EccPoint(LoadValue("303001d34b856296c16c0d40d3cd7750a93d1d2955fa80aa5f40fc8db7b2abdbde53950f4c0d293cdd711a35b67fb1499ae60038614f1394abfa3b4c850d927e1e7769c8eec2d19"),LoadValue("37bf27342da639b6dccfffeb73d69d78c6c27a6009cbbca1980f8533921e8a684423e43bab08a576291af8f461bb2a8b3531d2f0485c19b16e2f1516e23dd3c1a4827af1b8ac15b")),
            new EccPoint(LoadValue("408f3abf6e7d8a3f7fc1d35bdd23cb54527023ffc2e85097e4b363dbf114cc0faaa88834f72e93a35b4e4b42be9eb39825a3325e609f94dc2bfa1774eaa80a8ff2a0fe9a47491fd"),LoadValue("6fe4d54cec591a6e5bf23ac068b4ef66c792183c16a05ac69a967c7f10b1378c9defd33c040e96b04a00735cbcce21acffee775af186d4e4bee9ce1a4afd5c04d1f463a66b42e78")),
            new EccPoint(LoadValue("63577f9633861197f984f54d505169f21cebccc38c60f59c3b017f4bee3624ee2d822444959cf2dc4c106813833699c5f833254a80ea0b8ad4e2ab1445d390020886c9e4b301e6e"),LoadValue("6fc0f0149a69bd18e43cc3c267108500fddd0d936b109107df2ba93da838101031a6d295e5cbc0d9a7df9db9d423bcc709cfebcde4289b6df5af8f84aaf83663bfdad52ce81bbda")),
            new EccPoint(LoadValue("14288a8945e60ee5dfcfaad48ee25a980b854636923ba501dfac6b6320e7a3156a1da66ff9f0d73cf6d4739d4b0f9738901dc7f98fb351e5146c0b29b26e62be22a95615727f042"),LoadValue("22a3992f79f325f56d3dfda922ffc1ee32183f3cad23935bc9205a25178522f819146ae1ab7b196fbdc03b86a131217c0a9f9950a3f4a69d4bca3ef1571676087985f94d115531d")),
            new EccPoint(LoadValue("23f276d73dab04ab1d9feab60ea2339446aed7b15b76694e8e87d706ad90246f3c73a73b92a522cafcae9f0cd55027f3a97e971e1c7e0256ef9a6b3de1725ddeb3c259b00b37171"),LoadValue("0edbbd2ec40ce3d1ddccf10c0925c74e305b5fc2b6a7f2567aa5ed7c6d80f0f074f5e92b5ae881c02cd401f9bb80cb9aa5c90bb39e79aa445fb3f21c184ccba9244dad84a7fa37a")),
            new EccPoint(LoadValue("0410ebe938e6679335ea92ac62b15cd1ad3de24fb7d4a16ec15a4bb473d8077951d7dbb8b9f73a6025dd6ead93be4c64ab912119e530c97de18027629b7d2cc8f27ab2dfc31401d"),LoadValue("3ba62a9b77262f15c024e150ae1d88b8b805730867f0bba2be691d413b3e4273f7e151cfdd8523ec243b14714605e5cca2dfd0df084083724a428ba4f4db48f6e9f6037db7c9b85")),
            new EccPoint(LoadValue("08415e2c66edc84e4a44bf9edf0ab49d0ca8d80ab0df1ba6b6880adb293b5d5ac79de259693f1b48412b4dc2a0093d6689e0758fd4d1bcf47d6a1f28ce9fc887b141bb4606f078b"),LoadValue("2dc7a5a174d03a7e3ff93242f2e6e1255a6d292dc6b9c6a6aaec9ce38218e922e193cd9bb1e0a14937ced4f770bfa885c5029ba46d2203b747615d7f520a1a17ecfb4b1c7faf84f")),
            new EccPoint(LoadValue("5745a33ecac6de34a50f955b1ffc56ae7a4eb2fce18616f86beb1a3f2b91363a984422d915caee3da54c1e4c6a545054e80dfc3f3817d7fa472b00fe9c5e0f0ddf50a333eb56dd9"),LoadValue("3d9d3f856d9a9fea5a6b023b05c546dccad1a1f9110c1e6b6e243709ea012d548f64519dd7f315cd7d07868205eec038c0e735dbc4e430eeb53eaf12a0e4a8174e906c4e4a55b4b")),
            new EccPoint(LoadValue("4cef9a5f0cecc522bbd98c355cf6e49bf07bd6a234f9b96e869f22acbe5a35ba1df660cffb159825088e3d6c1d54357bb894a54bf1fdc8f323e00d72a59eec94640354035fa5300"),LoadValue("06ceb104823fb1e48de57a643fe8e12e9badd48cdd5910c3c7f1e385d0d213e2445d102ce08708c354da1e69569402156f1d24f5693655d44596976bfa25ab4100006440033c087")),
            new EccPoint(LoadValue("6dddb6811efbfdaec5620864bf05855ff14db25dd71c394ce7e9d8f5d25dbadb2be01d835abe20296223d37196c25db680c28720f42747b1a0c793eb1dab80a0ab40436a2a69ace"),LoadValue("5ea73825ca3b017341ada8d9184c37f6a06c9937b525bd8d3ed11095109e60892bb2487e8a5a7ab078da0ae0d4fc9d99c65ac0c63610c1f36cc9459df8d6ec9a68bcf026f0de33d")),
            new EccPoint(LoadValue("61365d46670a70ec335c1e8b6aedd9c50065d3a6ff046a8d16ae9b8c19763b1b4ad7b698d7a9214c597dcbbff223a91ea387a543351bb9bcf561bff0f4eedfd2ed2bcc65ffeea8b"),LoadValue("66f862dfa03e83ba2cf6e4c5fb8d365ccd7320f7a6e0ce3e06c67f55731852563caf5088c3834be44f707624de21a372d4f15782b9187ff35ec6b6ef31bd88c4a6009a63a74ece0")),
            new EccPoint(LoadValue("4faa6427cb8c7e3bbb8c3525f0cb74c840d7b226763a29b2b325bc5ca5b97875bcf71b56a9c76064616a54d2e95e144ab050aa779fbf22c31237084b4c780357686fc2d0da30ed0"),LoadValue("0aea49a7698b681f3747a425a940c9094e3184212f9deb355d3c69b1410e18eaede39382765db2e0ac38abce9144e8f6e67da88eef1c7ed57d730f79ac34f5f3e185fb61ba81107")),
            new EccPoint(LoadValue("790c9b18eb9ae2d1b2eff4e013d29e346a5c00c9d642a5aadd90fca0033fce012a36342b2fb2f174ef5aa41dc9d6737415316c8a07c483920e329eb806ce621fe2573f86507deaa"),LoadValue("2140486983504f59949ad36d5b290b5c974c47a28ce7379c2cad1bd38da53279eb18bdd21c9dc3089f0f64ca9e936a21ee08debe3f3c19542a38da9a8e00dc226b72d41aa788bef")),
            new EccPoint(LoadValue("02e5f586ee1a6e4357f8a75635c91ea6df4a7973f85b5ccde8c76e2f4cb51c84c6a11bc3900863104abaeedf9cc31bf50e57195b2d591087ca303cf1dfe795054bf3c1c2e2e535e"),LoadValue("00c6ff680494b11ddbe563df5304a97f39e47456c0412cb0139ad58d271a61400c3883d46cc215f37cb1ee88e27c1bfe0dc5466af196b3feae1bdf5777f338e049764011203add8")),
            new EccPoint(LoadValue("07eecdcb52c20b2be55e7f53778a5b134d43dd7bed73d7fd62f5b1d52aa7b0eccfc281ddc81de2bfce7abc96fb0de8da49cecfd8f79ca23fd084d78867d6632f570b30caf1f3612"),LoadValue("063898cd28dd1caab677b8f6841910147108941af051c19bc81f29f5af6b12689f0f871b5336eec080751b385b443e3f2f6224e3ddbfcfb936167fae1b1ec921769969955cc55f7"))
        };
        #endregion PreComputationB

        #region PreComputationK
        private readonly List<EccPoint> preComputedK163 = new List<EccPoint>
        {
            new EccPoint("infinity"),
            new EccPoint(LoadValue("2fe13c0537bbc11acaa07d793de4e6d5e5c94eee8"),LoadValue("289070fb05d38ff58321f2e800536d538ccdaa3d9")),
            new EccPoint(LoadValue("46e0fd0f048657011bc86a3ec0e1488c97689fe10"),LoadValue("032a9d737bbc8af0540bf3ac6dc03f36ba39bf304")),
            new EccPoint(LoadValue("543152d3d02e70f0d8a123d9a30d2c0ae91e6b705"),LoadValue("19f87cf914786988784dabb2d84e503475f41e472")),
            new EccPoint(LoadValue("4b933fde954ad84cae034aa3ff51dd09da59185cc"),LoadValue("121ae7c44dad4b836abd24446c44304f1f60de5da")),
            new EccPoint(LoadValue("443a1b983294c289035f96e139cb40018e1258d15"),LoadValue("09babca3ae1692e1e2453abd457415e1dbbfe5cab")),
            new EccPoint(LoadValue("1e2b0cf5c00bb7b40e6fd0f1590590df6444ded3a"),LoadValue("65be00b4d7e0d90aa43e2cd022e2385daf480155b")),
            new EccPoint(LoadValue("01a2fc4294131b1a3b96b4ec44ecbc6f38dc69ac1"),LoadValue("303b88e76b6041e97c2bc58e1ac602ee6b396c4f5")),
            new EccPoint(LoadValue("5b6598d72b6fad80a16cf09dff8be5086321ce170"),LoadValue("49ba60176818e9e0d72abed6391358327d10252fd")),
            new EccPoint(LoadValue("4d7679b253521c897bd94d9d1c81a163e3208eb07"),LoadValue("289a1943dfc34a87d09ca88cb9a8cfe1b4cb50f65")),
            new EccPoint(LoadValue("2e70d755ce876f6ad40ce06a272ae27f6c2132e24"),LoadValue("6fc5b940081e5331940a6c8daaf98588df22f3968")),
            new EccPoint(LoadValue("3b3c6b3fe8e7fcf85d7b71f1561bd0d9bc3bff840"),LoadValue("460b4f1fd7bc0e732733de4189edc1d76f11a1d17")),
            new EccPoint(LoadValue("7e2186f079db85a2403e14b1c9ceeb8b68ae3cfb0"),LoadValue("389ce4468ccb38915aeabaf107709c67da0edb2a4")),
            new EccPoint(LoadValue("74fe1ecc0db238ee311843db61a4177a4f60b40c7"),LoadValue("72a0849c39049658a1bd26ab91eb42421fb1f5dee")),
            new EccPoint(LoadValue("1df27c5ffc76804943f14fc4920fd709fb179b855"),LoadValue("6d2ff15ae1ff193a021011eaad50c370d93c6c0a1")),
            new EccPoint(LoadValue("1dc10f5289629f540aec51e4a6651a7d5fcc31bdd"),LoadValue("3b29ab6f66d6f8134140c5388d217a51851a195a4"))
        };

        private readonly List<EccPoint> preComputedK233 = new List<EccPoint>
        {
            new EccPoint("infinity"),
            new EccPoint(LoadValue("17232ba853a7e731af129f22ff4149563a419c26bf50a4c9d6eefad6126"),LoadValue("1db537dece819b7f70f555a67c427a8cd9bf18aeb9b56e0c11056fae6a3")),
            new EccPoint(LoadValue("0e0a38ffc511e71346d74beaa75f6247e8b7d67d819a96489bc3f3eb242"),LoadValue("1e63cec413af0f42c6368a91e150b72db8ec275f589e1017b87c0b59251")),
            new EccPoint(LoadValue("000b6818fa2c95ed55c6f939793f7672716cc9b124b9a67eeb06a76dfcc"),LoadValue("09f1727c186d51c4081d2c3652289fa73a6460ce07022dd2320fee66cb8")),
            new EccPoint(LoadValue("0e25ec919b9f6377d4d9ce387e9cf351d8aa34256460cb19f6138d5250f"),LoadValue("0f793931b4ebf16d79ad26c5c45025cc886a84b57461eea8210117dd38a")),
            new EccPoint(LoadValue("086d3970d240e6076e43e80d4b67d73d39aeb8d7a7bb47123644b368420"),LoadValue("10fa566093e738cc77c6543c20f1a134cf2faebb675b3b34c323d609ebb")),
            new EccPoint(LoadValue("017a9e7e05f69de6d373b3d4f08c7d10fde0d6662f03ec5a6ed7870c05a"),LoadValue("1615f597f137c70da72f96d49fde75130960ddf345c5e68ec5cb273b387")),
            new EccPoint(LoadValue("09c04af28078b03121bf1842eb2ba1c0bc5d04c16abae42696927b942bd"),LoadValue("05ac7539446e69c28a1ef8cf7300abae43c2b32fae8fa06e9d626a66ee8")),
            new EccPoint(LoadValue("0ad281f854d2a538214562f6f5478b2032073b39441b5de73efcf75d2ce"),LoadValue("120eba13528ab3aa7a10ce46379e17e573c02ace3d8597245205c44805b")),
            new EccPoint(LoadValue("1f5dd401a00dc69d27b53fe0eb283247cf89ee1a50b7dbc4b28beb1cd58"),LoadValue("126ec067aa6773ed0c2b3fd016d52ad980a7b00b08bfdccc55d3cda23b0")),
            new EccPoint(LoadValue("16ca3f6c43ed2ca39cdfa18d66217a845c972e39280b7f00b38b34cf82d"),LoadValue("09fd7fb7caf0a0beb920b17fbb772529c0f835c9f21862b9dab1efbc4be")),
            new EccPoint(LoadValue("1b8e64261582835b54ea9543cc5976c594412d0861697f5ffda85eabf39"),LoadValue("08682865f8cf62e18305319051cf4a663b184f2f70565ab79c5ca249d5b")),
            new EccPoint(LoadValue("171b6c4165653d125bd9e376f3dfb226b9b8f2fba347d751e12cc8fdcdf"),LoadValue("0e17920d5701267749148c7373944be170ab2bd854ecfd31c27ec0bc2d6")),
            new EccPoint(LoadValue("02f870e798d6d2fc2c75adf0999c03b80779508cbd1bba49e9154ffd10e"),LoadValue("1e0775795b91e2456593b98ee1a4a20cb6c5ef5fea854b9444a74bc914f")),
            new EccPoint(LoadValue("1dc51c8360db7891ed68afec8f3131e3aa74c285006e3e34d1a6eda17b9"),LoadValue("004b7237dc021b0c9ddcd8163af1034e2554729757fffd0096824588a40")),
            new EccPoint(LoadValue("1118112d94f84ac3cff62c6cccba555b5686fe570fc44c9c572f506429e"),LoadValue("1f9c81a16d29a5a1cfb3e0f0f300a6d3ff18ba51a2d688d166146983c99"))
        };

        private readonly List<EccPoint> preComputedK283 = new List<EccPoint>
        {
            new EccPoint("infinity"),
            new EccPoint(LoadValue("503213f78ca44883f1a3b8162f188e553cd265f23c1567a16876913b0c2ac2458492836"),LoadValue("1ccda380f1c9e318d90f95d07e5426fe87e45c0e8184698e45962364e34116177dd2259")),
            new EccPoint(LoadValue("5df02d335ecca2a202d5e420fca15d58677d63bc889cd449db23954af967fd0d2c98794"),LoadValue("4e7ef1456b717272b8a6a713946629442ea3628c5853218fbdb37ed54ea9fa36b58d6cc")),
            new EccPoint(LoadValue("2c69b77958c20211a40944a54c66efb842c804a9588c8ba112c13b22851bf878decbbc1"),LoadValue("4f07dd1ddd4971435af680e8f324363bd0158327fa987e1c8a4271e91f48f3579464a10")),
            new EccPoint(LoadValue("09abe776cb47495af2ad954dd1aa6eb5467e56c44ffe966fba52428248412f01306cd8d"),LoadValue("195ae2cfd22314997859d5c40e8b69d98d4354652575be1b2451257af8dfbcd64bb17a7")),
            new EccPoint(LoadValue("66cb301cd1566e4cfe3c6657f09adab7718c7e2371a39e353373876fbbad48d1729ebd1"),LoadValue("01b8603efe87fb83b450a276d2573f6ab9e5afa8b619ef76313dcd78127ea08e4b41c16")),
            new EccPoint(LoadValue("01144654bbfbd6aefd331db03911af82037760c88d497013cbe3d697a88f55a6944bd7a"),LoadValue("433623414089a95867a5fc8b3424d8c1800352d08800d8f4d66cb2abd6a521e7e1e9934")),
            new EccPoint(LoadValue("4bc6af94619d9d944a80e6d346489d918e18aeaee2d3bf2eee12db947036c43dec2ccc6"),LoadValue("7348f97921c7fc1e9f5a87b260ca4d505542a1dea8e2a61ed14b0d2df06b4fec194842d")),
            new EccPoint(LoadValue("60c69bd9dd890aecf2be00f5f2ae5d358184438097ef7a33c8361a03263c9d75d5ed2a2"),LoadValue("5bed2acbacd2f4383f57c0d10eb5087eaca5bfc0fd07212fec7d78bd9f2843477148443")),
            new EccPoint(LoadValue("01fa38428211b59d9b9fd7acf8203f5547fd495a7f273a358161d7868588c767f34869f"),LoadValue("4f7b5f07807e7c3429526aacdb57b8f86b0f34ede9117d14f556bad30681aba2429a9c6")),
            new EccPoint(LoadValue("146519336346758493f7be033f8dfaf1ce4bfae2fd525b76f94f17636ac802289f0d6b6"),LoadValue("7f69470255fa46ee511e859197046627aab817253391999d6cc14365fe041f0497c6f89")),
            new EccPoint(LoadValue("52701a8b29978f58954d1f606c8e00373baae0f455aeba97a806fcebc53c883f661cf1e"),LoadValue("5e107c2e26b3d0963bdf9a5b552a21039f4317c5274ffb798034d281016d1e6febee508")),
            new EccPoint(LoadValue("470ae5f6e21a5bfab6533854922e5b41ada3a115743567d1c7aa667ee00612b2ef1694b"),LoadValue("5204dace2d24af417c53f1efbd65258882825ad3423772ed1497d4fd2728a039662877e")),
            new EccPoint(LoadValue("14f6f63158d95690cd306eedf56fcc504ae0f56a569a5e4b463814aabbf7a5fc10f43ec"),LoadValue("24114519499e9e4a8264bae492577d2469abd8928257ba4d1bd388c38f9768e3fc28813")),
            new EccPoint(LoadValue("05772c595069b81d0d85b2dff7185f9e7361d854a81faba5227c35e9eb24e1dd418869e"),LoadValue("6a175c3bf9ee9b0fcedf033f71d9178e07da656ccb567bb94e2be04604c2f5dad081e43")),
            new EccPoint(LoadValue("462d1ca85ad18cc4c8ee5cc5824301bd82d01abf7ed9dc24778bdb1aad813da061e011d"),LoadValue("22d64709e60b721ab31c6cffd372a8b844741058849cb04cf1866515aa1425ee5b1c76d"))
        };

        private readonly List<EccPoint> preComputedK409 = new List<EccPoint>
        {
            new EccPoint("infinity"),
            new EccPoint(LoadValue("060f05f658f49c1ad3ab1890f7184210efd0987e307c84c27accfb8f9f67cc2c460189eb5aaaa62ee222eb1b35540cfe9023746"),LoadValue("1e369050b7c4e42acba1dacbf04299c3460782f918ea427e6325165e9ea10e3da5f6c42e9c55215aa9ca27a5863ec48d8e0286b")),
            new EccPoint(LoadValue("160a0518720ec4b636b695ce11de18919ed528ff4f31e204ec494255e2a631c319664c68de81c5d83f59558c7ddea0ffc08f686"),LoadValue("023ec5811579074c52c5fec3344a18dc89c64a4fc466f81331b5d45f32b62eaad8385f2a8b2d8c7c2bcf77fbd07973b56157f6b")),
            new EccPoint(LoadValue("1d552030994e90ba8fe23c1396d8a74ce0f524e391e8035d464e6ce098a295b5b1535e21c3bad3269cd2bdaca1c3fe0bebca744"),LoadValue("1c1811ab58e54b6b11fbf2f697b02d0895bef254ba4121d2639c2f9bbc45279ee753096e40e5053cf03484fa7c2e86bcfb861dd")),
            new EccPoint(LoadValue("0c1273629a1602c349eabd2df3e260fe4ff9f0d4960321f4954a610dd417a34ae783f7cd693e6fe459161511009bc745c700d4c"),LoadValue("0cf73613a07bf11ee4260952c73d3ec2611f3e3c08e4b0a515c740b0590bf2db68a89a954ada1c225ffb4cdd708004ab658aa56")),
            new EccPoint(LoadValue("024183b14410340ade46e1b0d97a51ce737c2a9c32a0aca4912105bde8abde163601061fbf4ca20474eda6f2961fd43e2118252"),LoadValue("0d94472419cfb7d0e863caa1afa5978193549fedd7bf2dcf9d6b651dddbb4e1041516d5ef53d8567e161b21c4cc8944d3adacec")),
            new EccPoint(LoadValue("141dd3cbd044837c0611e1247d4161d6353f714438f797c78b921d915a9e4df84f6989789ae5897fe3087d454785d9077d060a2"),LoadValue("086cbbd10dc9fc4e94a5487ea353e6453e76050d56b035f13ca154c98a0240ed08319bfc4a1b0a46f8cb03e901a391a63c3731f")),
            new EccPoint(LoadValue("0d12ba007c8c7088afb6869c3240d606c7cc1525940b1933c5193d64969fbc3235e452df37b750e62d8b492415dcae9966c1952"),LoadValue("144b6c486ba7ce42c47c5a795c332e04b2c1c2b010b895dc16e8792ab22769aa23b5d0b5fd118eb94077b7cc39dcacb6881ea69")),
            new EccPoint(LoadValue("0ff6ba0818e409c7a0a2cfab8336f2f77ee04ab8da39b8813e9b4a2471fd922ac1501e584efc87198a4ea594b44039f9d85d89e"),LoadValue("1373dc5146a9c2d330cc8dd659cd9d6c0136024b7e0021c9fc2693d9132db0667c53ea3ff7208a8d9edff6d5b80587987edebef")),
            new EccPoint(LoadValue("18a969da3423dbfc542870270f0d77ed6c837d367b653660e540c92609750bd513ca5ab12273038bf3825bbb1476e2d6e2638a8"),LoadValue("1071f969a5851aa13681b3c38e13b5ef0f9da7a1715707407d3f6189055926b11ee81a4e2b87075bf336519254bc0c83a60c027")),
            new EccPoint(LoadValue("10ca3b2ed12a76ef6a41a678a72a0b154ec27baf7eaffcdac6acbf65a0e845e6bc388a947ad2b9c443066833a384db86d9375b0"),LoadValue("1adaef68a1b6b6d4b88d0a5e50e4a8126c6e93a5c8e03ea97f16f23b09485c599c6c3605183b9df43109f1def1101c4c11c3ada")),
            new EccPoint(LoadValue("04ed0aff8ef97fa3ea4fb8b0cad5a618c1e0bb9f644717c349dfc220b89a3951a7b9ec78bac5bb0362887aba4f4ef49a660ffa8"),LoadValue("1d1940ce2e0090e35c58f79fe1f1fe5d4af75a0d2efc3cce6e3f616a4ea7e0f1a815297e04dd71730549ae6ce8b5aee6c7e9519")),
            new EccPoint(LoadValue("050d2316daaa12c06338ef4799f0e42cc7ba84049bfc02d5b46f13bfbb1f47f94a606f3684fa8e6c6eb6cdb822870119141adaa"),LoadValue("02553c47283f7f24eb5108460be9da8da9c447c2667de3452ff992dc495bd1139343b5251aa9ee9ad3452c9828b1062bc4f8227")),
            new EccPoint(LoadValue("0067118e709ef0bf6707eb2a19bdd4cd5944ace43443d4f6fc3d9ebae88ca8f7767f06de931868e39305977e5a1cc2d70209eaa"),LoadValue("1d826b1ef34d921661d2aff8bd1fb45da22604105a5d3e50c8a0816f3f38bb86da1040a03c30103ff4a052f0769d3bbdf7fccb3")),
            new EccPoint(LoadValue("11dafcbc91fdbf84ac0432770c43b4302570f261120608108c50630ed4dc6fa1ce98170995c1bcd108173c7b208a8a7d1ad4076"),LoadValue("07ce4171d0c867e6588975c41b4d7e0b4cf65ce585db293236273d7c45522969de1cb7729e902c381d7680e87fdbf7392b9d400")),
            new EccPoint(LoadValue("1b1cf5b103760dd49a3f83c77794e03e95f6d1ac0c6007b05a8dac49c557761e0efa3df4a61ce3541f42c19649ff04ea2424078"),LoadValue("0ab176c390471d8d44b6ae952ae23339c5fbc0e968d09f8d2c313da47b3d87fa0583f8e83a6d3cb7b0117f95b098014e2260b27"))
        };

        private readonly List<EccPoint> preComputedK571 = new List<EccPoint>
        {
            new EccPoint("infinity"),
            new EccPoint(LoadValue("26eb7a859923fbc82189631f8103fe4ac9ca2970012d5d46024804801841ca44370958493b205e647da304db4ceb08cbbd1ba39494776fb988b47174dca88c7e2945283a01c8972"),LoadValue("349dc807f4fbf374f4aeade3bca95314dd58cec9f307a54ffc61efc006d8a2c9d4979c0ac44aea74fbebbb9f772aedcb620b01a7ba7af1b320430c8591984f601cd4c143ef1c7a3")),
            new EccPoint(LoadValue("27db798497c9b02e376df364d250e761ef0b1b14bedfc2acb06347a8389e3b69aada76c59ffee4257305b07ad154065d9f807a623490b8554ff7c7ba9640b1b6074a7a432c24579"),LoadValue("6043ce86d6d9e473fdd610169cd9e9218eafd42b1f79184e84a458872b4464356c0c4d88f0414180838044c13f80a2d6575d4662792802de03f9afba05ef61fc84f3aee1dd03953")),
            new EccPoint(LoadValue("74956c54e6d8198d37ab56c4d1a2b36e056899008fcd876e6317af4e0a5edbafd3a7bd02b6133c940bd63e72e6c87991873a99e687350e483f409862cea43a682ebf1d7d7ffdb07"),LoadValue("3bbece2113a6e3c64a444a9ee90bd100bb24925509f028ce204637c18ca1979c7c941ed7fe2632d6a365e848dd115d36e2699b15080b6ad1bf15823431def51d749e5d76ccbf436")),
            new EccPoint(LoadValue("623e0b199e767b46626ba8bcaed799bf0ed7b65834382f24a0e7c0eb3b023756b1c9af66b3cb6ee618c0ca2f64f4bf18e4d0738ffb555caec6d58ca0c3531c3d91c145514557636"),LoadValue("4cc3bf8e761ea870216eb16f3f92b19c92885bff7b69c75648d4006f545a6547b00d7b129c82891b5d5238fd4ba831b275550548df3fbf350ca0d6be6931dfb5e1667d676f68722")),
            new EccPoint(LoadValue("08f92beb4c9867229b716c761f0c7c059e9ca6d9cff8669e0fdfa04ba973df61e520ccf4ef9a4d03b53b4aaf7008cde17ca82f30573ed0ae1f4390bf7ed3778f2b56265710d4ffd"),LoadValue("30edd59c66af9def7baf66f4f3d4562089a056bca50abca68ed34fcc335e07ecee3b00861835e3190c5c241e323757dd60522eb8e355cc8502ea1bb3387be584cf023a4769be463")),
            new EccPoint(LoadValue("3aa9ea5037175c1aafc8722944569c48614782703587f56176983138c92caf0761e9cb525aef0e51b40583b1885eb11e8177d65456437a6bc6efbbd064619a6604c45fdd7054078"),LoadValue("17dc33cd7daf66db5879c634218ab8fad61e11ca845d11e7aa9c3b0e9d0058521b65c04061ebfe6962a2fb963c42a656d8a67d005234b6c335659e2555ff456cce5b4560a7632dd")),
            new EccPoint(LoadValue("670a1ff33931e21b6918ab9d5dd8fe62b53db8359ac6ba2bfacd853d4e4af18c4364af5886cae75daedafaebda8436d4d5031f4b501ea7684d07769d1f681bbe16c930ab8e5c8d3"),LoadValue("4def7caf5e195a9d5699309b270df96451c4c1ae96385a17466dda08acfa3ec789d27fa7493a35d38b4b6923b6d3f26d8f447dfde701583a615343e7593d9dac5a8093ec8e8d236")),
            new EccPoint(LoadValue("71f31b91644725a0825df81d9837716099c4c65019309dc1f29ae113bfd40732e15bcd07c5f629b36394d682841534c8f2f997cff534874bb02b65b7e36d5a50d77da1ace669e52"),LoadValue("282be93ddc12893482f46faba9a4275a7cc813df4854d2799bad87343882202fe336042ae2c4858bdffa6ac6c7d85d7017e5cda4ffb50448e9e22f31845db9e9faa001e1f18c1d5")),
            new EccPoint(LoadValue("2f0079f3f58c628d2bf7690f991d39dc4624fa80920273686e73274ef855bf890de1a0db55140ef9486660e46e455324152fef7672161296014dcdeda67f1b2e3a944d9664344d9"),LoadValue("7f7af6cbf3569e03704dedecaabd4a84a5b1e8ab7d25bb1c8a4077b9c086d9bc303f3a97a118fe8bf21961b1158100b3de38429af8f0fe35d32d35d32546c5a8844096b7a727f94")),
            new EccPoint(LoadValue("16449d56bbf6805f2ffd28a2a80238faff765eca193cbf14fed4c9da9e77534529f8dd41fa33fdeebbd3a661360b22f06f14c71ee35eac8e2fdbf08a093aa8ffbe604226afbf0f2"),LoadValue("5af206de507f213a86e8d6f47ebc6a46ed5c84d6e64673c0a41ab288fad8a4f5a449020f1184993e20c9fa435d72f5984b12d7ecfbc1295f84b2620ac023f698ca0158e276f95ec")),
            new EccPoint(LoadValue("5d42759a50a62f6216db1364b8beb061fa5efda071e7e3bbbc74c5d48bb4d57cfdb1a63ca036f7319837a8c81e7b7eaaa1171ba3444f55195df4eb71ae2fa234fad8ad0789ef836"),LoadValue("53fa2c3f9ddf27432dc94d29c518284989ee94f1f017939fbf753c789493ecb82a046212b36ed099206d9818d8ae18b3203e42bdc4bf7e2cca9407b6385a1d1b2315750a9d93bb1")),
            new EccPoint(LoadValue("7a7e2525d93e99dd54fa0f6d2e4c579c04bf2f2e875bbff157b6b442347abce8d8f59a563b54599809cd426dce9c427d4b0d0fff5d8c3e58ce690bdc5c45099c944b7a5abcfb4f2"),LoadValue("2a64ad19b7a7a803d5b446263157fed275e7a69555bef7ac9defd414a918375382a246a2e05f0b4d04578b1981811b25d6f810e7b42bb5e930b9f1a3d2e9b287807dba1ab318316")),
            new EccPoint(LoadValue("3b57b68671ed73452373d866160dfc126408bf85b4c3b54b4ed2c13f15cf0a622f35e46607586b8541a28a332e667aea896e91866218a23fdbac9b9693400f9da91d7a0f3a2aad3"),LoadValue("69a70eb8e59a7388324afd0fe28082806d52ed3e0822f8527d3d2d701f7d5e735b5527ef8313c8b7dd21c55e359fe29ed1e224a5e87f4e43eb5dcf49b4158beddacd788d6cb3eb7")),
            new EccPoint(LoadValue("32d2e5061d3627fc7da9a4fc458b2b12372f49cb350b36c330ab606c98ac6a637282d2a26d145f706a31a68c4c4e68d976192da919977128eaee61c97fae44dd62e40047dd0dc65"),LoadValue("3f95457569cfb8824a628b9cc7edbe7d2980c8301be3cea2e8b1b30d054462cacbc8348e369780e4831fc4b5433e8242a07a933f2d7e0e3c39a41d1390a721ee68a8d28f8e63dfa")),
            new EccPoint(LoadValue("37a271a2d8494aff7017972d8b04d6e6d8c05c47da4931f1667b34275274a677afe3911f2541f9bfc82e1c000d91d16116b61c1f42eaa7694ecec6b25c346637cb8bb032f956ece"),LoadValue("51c4221ea3d2557182b3dc5a40faa6e3d41249dc610974624f988896c186a01d2b83e5a6f31ab43cda9dd3f2b7583de4b1ade761de60db13ec4ccfa3b231caf3464049d1b1dcd21"))
        };
        #endregion PreComputationK

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
