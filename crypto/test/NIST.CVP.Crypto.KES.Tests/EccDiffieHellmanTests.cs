using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KES.Tests
{
    [TestFixture, FastCryptoTest]
    public class EccDiffieHellmanTests
    {
        private readonly DiffieHellmanEcc _subject = new DiffieHellmanEcc();
        private readonly EccCurveFactory _curveFactory = new EccCurveFactory();

        private static object[] _eccTests = new object[]
        {
            new object[]
            {
                "B233 curve",
                // Curve
                Curve.B233,
                // Party A
                new EccKeyPair(
                    new EccPoint(
                        new BitString("000000dfe9cd18b4613008e13a086e7a69b3752916829a32e06aa246cf91aa79")
                            .ToPositiveBigInteger(),
                        new BitString("000001c68825ce9f2355eaa86f7bc58235988c0d7ff5441acdf0eca1ee2494a9")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("00000080360eaea873fb66a2341845ce310f0b8b144deb1cb5e938520aedae52")
                        .ToPositiveBigInteger()
                ),
                // Party B
                new EccKeyPair(
                    new EccPoint(
                        new BitString("000000483b5e148ac734963d7aac2194c6e96294d83289ba77ef1edb45c0debd")
                            .ToPositiveBigInteger(),
                        new BitString("0000014e8ca9033f04c5b102f37d4b253874a8bd54260fd35b61a32a800c9d79")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("000000c2ca50d96624bd2e6857c98a0d14206a4402ecb21cda2135c48c652844")
                        .ToPositiveBigInteger()
                ),
                // Expected Z
                new BitString("005d1321a08c24af3bb28b7a9e98873fa00593d4b9b2200155af5e97765e")
            },
            new object[]
            {
                // inverse of previous test
                "B233 curve inverse",
                // Curve
                Curve.B233,
                // Party A
                new EccKeyPair(
                    new EccPoint(
                        new BitString("000000483b5e148ac734963d7aac2194c6e96294d83289ba77ef1edb45c0debd")
                            .ToPositiveBigInteger(),
                        new BitString("0000014e8ca9033f04c5b102f37d4b253874a8bd54260fd35b61a32a800c9d79")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("000000c2ca50d96624bd2e6857c98a0d14206a4402ecb21cda2135c48c652844")
                        .ToPositiveBigInteger()
                ),
                // Party B
                new EccKeyPair(
                    new EccPoint(
                        new BitString("000000dfe9cd18b4613008e13a086e7a69b3752916829a32e06aa246cf91aa79")
                            .ToPositiveBigInteger(),
                        new BitString("000001c68825ce9f2355eaa86f7bc58235988c0d7ff5441acdf0eca1ee2494a9")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("00000080360eaea873fb66a2341845ce310f0b8b144deb1cb5e938520aedae52")
                        .ToPositiveBigInteger()
                ),
                // Expected Z
                new BitString("005d1321a08c24af3bb28b7a9e98873fa00593d4b9b2200155af5e97765e")
            },
            new object[]
            {
                "k-283 curve",
                // Curve
                Curve.K283,
                // Party A
                new EccKeyPair(
                    new EccPoint(
                        new BitString("02131f6ca31d87f381fb38d8213a066b9a1cf531d10aafee38316080b6237be4e5a5da52")
                            .ToPositiveBigInteger(),
                        new BitString("04050a48e13a3d6f6e575160684f6477ce4fe14355cb0599645d878f60346b05cacfa970")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("001fa9074745b3793eb7794feb37fd20add637cac06c925b75782e38255c40112f69146a")
                        .ToPositiveBigInteger()
                ),
                // Party B
                new EccKeyPair(
                    new EccPoint(
                        new BitString("06fd5b1a2ac0d432737c2420fc529a3aa34f949e8ba214c54198d283b29c82154ac9daa2")
                            .ToPositiveBigInteger(),
                        new BitString("006f59428d210e380aafb225820f2a269f7e4732207c6d437e20eb5de981967642a2628b")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("001f6b4f9a0828cb4cd9ca6c9539313f3892342cc20e5c568b3a44539eb9e483bd0c9652")
                        .ToPositiveBigInteger()
                ),
                // Expected Z
                new BitString("045d9d63c63cf0e24b45972a6deda075e07bf89b151ea5b4e479cb30445f879b06b7722d")
            },
            new object[]
            {
                "k-283 curve inverse",
                // Curve
                Curve.K283,
                // Party A
                new EccKeyPair(
                    new EccPoint(
                        new BitString("06fd5b1a2ac0d432737c2420fc529a3aa34f949e8ba214c54198d283b29c82154ac9daa2")
                            .ToPositiveBigInteger(),
                        new BitString("006f59428d210e380aafb225820f2a269f7e4732207c6d437e20eb5de981967642a2628b")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("001f6b4f9a0828cb4cd9ca6c9539313f3892342cc20e5c568b3a44539eb9e483bd0c9652")
                        .ToPositiveBigInteger()
                ),
                // Party B
                new EccKeyPair(
                    new EccPoint(
                        new BitString("02131f6ca31d87f381fb38d8213a066b9a1cf531d10aafee38316080b6237be4e5a5da52")
                            .ToPositiveBigInteger(),
                        new BitString("04050a48e13a3d6f6e575160684f6477ce4fe14355cb0599645d878f60346b05cacfa970")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("001fa9074745b3793eb7794feb37fd20add637cac06c925b75782e38255c40112f69146a")
                        .ToPositiveBigInteger()
                ),
                // Expected Z
                new BitString("045d9d63c63cf0e24b45972a6deda075e07bf89b151ea5b4e479cb30445f879b06b7722d")
            },
            new object[]
            {
                "P-256 curve",
                // Curve
                Curve.P256,
                // Party A
                new EccKeyPair(
                    new EccPoint(
                        new BitString("3489d2053cf82075ef9f30bb1086375f387ff1ab8af671bfd38f40ccf423832a")
                            .ToPositiveBigInteger(),
                        new BitString("b16f43c8eabeb14a30afe2bb7455a59de1e8d0cd4d6c75859846dd9a65fec5ff")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("725ea400498d4a16889f550401809d9897475834334234ba51a72729112238c6")
                        .ToPositiveBigInteger()
                ),
                // Party B
                new EccKeyPair(
                    new EccPoint(
                        new BitString("61148505ca40d300147c2cf20e135adfe1772bd13db5976945d21b4dc8d0ca2e")
                            .ToPositiveBigInteger(),
                        new BitString("0b7a707e3d30c65712e271fd99c66c570b73a1568aadcbd6e177dc6c10cc5ccf")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("e926c9111d9162197231595b7c6b0ff7ab2c6a95d5dde5061fc8bd293a1f9368")
                        .ToPositiveBigInteger()
                ),
                // Expected Z
                new BitString("2af2fa799671f28a3e748f963f01cc37efd6849e70fcc29f0e1c88d0e87ff8ef")
            },
            new object[]
            {
                "P-256 curve inverse",
                // Curve
                Curve.P256,
                // Party A
                new EccKeyPair(
                    new EccPoint(
                        new BitString("61148505ca40d300147c2cf20e135adfe1772bd13db5976945d21b4dc8d0ca2e")
                            .ToPositiveBigInteger(),
                        new BitString("0b7a707e3d30c65712e271fd99c66c570b73a1568aadcbd6e177dc6c10cc5ccf")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("e926c9111d9162197231595b7c6b0ff7ab2c6a95d5dde5061fc8bd293a1f9368")
                        .ToPositiveBigInteger()
                ),
                // Party B
                new EccKeyPair(
                    new EccPoint(
                        new BitString("3489d2053cf82075ef9f30bb1086375f387ff1ab8af671bfd38f40ccf423832a")
                            .ToPositiveBigInteger(),
                        new BitString("b16f43c8eabeb14a30afe2bb7455a59de1e8d0cd4d6c75859846dd9a65fec5ff")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("725ea400498d4a16889f550401809d9897475834334234ba51a72729112238c6")
                        .ToPositiveBigInteger()
                ),
                // Expected Z
                new BitString("2af2fa799671f28a3e748f963f01cc37efd6849e70fcc29f0e1c88d0e87ff8ef")
            },
            new object[]
            {
                "K-571 curve",
                // Curve
                Curve.K571,
                // Party A
                new EccKeyPair(
                    new EccPoint(
                        new BitString("01a1c810bd91f92a9419a94431aefb12d68c3d78db2c93ad90c1f21f8be24e47f7adf2c750f3d1aa3d7eb4c3a50566e7a8e9f8d03b0cfb104a7d95bb1e5f253d3b8d9a8c443d601c")
                            .ToPositiveBigInteger(),
                        new BitString("0541742f37113aa5181279822fa35d70db72427828851ea7e7614f994d131c952ac72734216f5a27b8df0e6bae75080e8ebabb8a29156c1cf4d73988bfa8278c6201db672b9dadcf")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("01df628afed47b4bdd28ac8bb2739125b8878818116a7f49109f862cab190b18bab7686479088100d2ac21a95a153a7a8242621ac4e0d1597643fadccfa96f56eb1b874aedc7dcf9")
                        .ToPositiveBigInteger()
                ),
                // Party B
                new EccKeyPair(
                    new EccPoint(
                        new BitString("0164bd10f733efc119c8bdbfbdd6c6409c02871a857f44cd89781ba05851aa8e4b72893e0bd6a2d02b08f7032bd19b767ea844308323e9327de0414ef3efc9bac9f14999225814b8")
                            .ToPositiveBigInteger(),
                        new BitString("014514608197b736ec334d4c67dabda793302b27b19ef7e54f82a65c7356a441492bc2bf8b63dc930f90f20610fe4fa65dd47adab9d08b4c41b63de7a802359115c4207c77c60d7e")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("019f38ea5e56c86cdd1c51471ebefe5b3277d422a766bed84d2fdc8cefa525cba4597b3c8dd951122985ab0d5b5b55501a55239aeee992c362091460dbb17832c3dd13aaffbe241c")
                        .ToPositiveBigInteger()
                ),
                // Expected Z
                new BitString("06882f6a49c2e2fc7582810b6ba830683ecfe7b8a5e007cedfe140ebd0b9292b5b44ead5c0fe823ab8491ddd7d48e5e9f552cab152c2060fcdb06d733ea25c5d0e0fe668c9b6160f")
            },
            new object[]
            {
                "K-571 curve inverse",
                // Curve
                Curve.K571,
                // Party A
                new EccKeyPair(
                    new EccPoint(
                        new BitString("0164bd10f733efc119c8bdbfbdd6c6409c02871a857f44cd89781ba05851aa8e4b72893e0bd6a2d02b08f7032bd19b767ea844308323e9327de0414ef3efc9bac9f14999225814b8")
                            .ToPositiveBigInteger(),
                        new BitString("014514608197b736ec334d4c67dabda793302b27b19ef7e54f82a65c7356a441492bc2bf8b63dc930f90f20610fe4fa65dd47adab9d08b4c41b63de7a802359115c4207c77c60d7e")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("019f38ea5e56c86cdd1c51471ebefe5b3277d422a766bed84d2fdc8cefa525cba4597b3c8dd951122985ab0d5b5b55501a55239aeee992c362091460dbb17832c3dd13aaffbe241c")
                        .ToPositiveBigInteger()
                ),
                // Party B
                new EccKeyPair(
                    new EccPoint(
                        new BitString("01a1c810bd91f92a9419a94431aefb12d68c3d78db2c93ad90c1f21f8be24e47f7adf2c750f3d1aa3d7eb4c3a50566e7a8e9f8d03b0cfb104a7d95bb1e5f253d3b8d9a8c443d601c")
                            .ToPositiveBigInteger(),
                        new BitString("0541742f37113aa5181279822fa35d70db72427828851ea7e7614f994d131c952ac72734216f5a27b8df0e6bae75080e8ebabb8a29156c1cf4d73988bfa8278c6201db672b9dadcf")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("01df628afed47b4bdd28ac8bb2739125b8878818116a7f49109f862cab190b18bab7686479088100d2ac21a95a153a7a8242621ac4e0d1597643fadccfa96f56eb1b874aedc7dcf9")
                        .ToPositiveBigInteger()
                ),
                // Expected Z
                new BitString("06882f6a49c2e2fc7582810b6ba830683ecfe7b8a5e007cedfe140ebd0b9292b5b44ead5c0fe823ab8491ddd7d48e5e9f552cab152c2060fcdb06d733ea25c5d0e0fe668c9b6160f")
            },
            //new object[]
            //{
            //    "",
            //    // Curve
            //    Curve.B233,
            //    // Party A
            //    new EccKeyPair(
            //        new EccPoint(
            //            new BitString("")
            //                .ToPositiveBigInteger(),
            //            new BitString("")
            //                .ToPositiveBigInteger()
            //        ),
            //        new BitString("")
            //            .ToPositiveBigInteger()
            //    ),
            //    // Party B
            //    new EccKeyPair(
            //        new EccPoint(
            //            new BitString("")
            //                .ToPositiveBigInteger(),
            //            new BitString("")
            //                .ToPositiveBigInteger()
            //        ),
            //        new BitString("")
            //            .ToPositiveBigInteger()
            //    ),
            //    // Expected Z
            //    new BitString("")
            //},
        };

        [Test]
        [TestCaseSource(nameof(_eccTests))]
        public void ShouldGenerateCorrectSecretZ(
            string label, 
            Curve curveEnum, 
            EccKeyPair keyPairPartyA, 
            EccKeyPair keyPairPartyB, 
            BitString expectedZ
        )
        {
            var curve = _curveFactory.GetCurve(curveEnum);

            EccDomainParameters dp = new EccDomainParameters(curve);

            var result = _subject.GenerateSharedSecretZ(dp, keyPairPartyA, keyPairPartyB);

            Assume.That(result.Success);
            Assert.AreEqual(expectedZ, result.SharedSecretZ);
        }
    }
}