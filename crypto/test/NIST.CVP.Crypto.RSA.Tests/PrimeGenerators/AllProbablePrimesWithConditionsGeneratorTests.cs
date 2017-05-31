using System;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.RSA.Tests.PrimeGenerators
{
    [TestFixture, FastIntegrationTest]
    public class AllProbablePrimesWithConditionsGeneratorTests
    {
        [Test]
        [TestCase(0, "010001", "ABCD")]
        [TestCase(2048, "03", "ABCD")]
        public void ShouldFailWithBadParameters(int nlen, string e, string seed)
        {
            var subject = new AllProbablePrimesWithConditionsGenerator(EntropyProviderTypes.Random);
            var result = subject.GeneratePrimes(nlen, new BitString(e).ToPositiveBigInteger(), new BitString(seed));
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase(1024, "0100000001", "5c029cd058da46698662234f46ca7fc9eabe138c")]
        [TestCase(1024, "d70ff9", "5c029cd058da46698662234f46ca7fc9eabe138c")]
        [TestCase(2048, "0100000001", "5c029cd058da46698662234f46ca7fc9eabe138cb20173cc1559100b")]
        [TestCase(2048, "d70ff9", "5c029cd058da46698662234f46ca7fc9eabe138cb20173cc1559100b")]
        [TestCase(3072, "0100000001", "d37b22fa27c9d330b9be0dd829c736ab87128aaae055a4f26c070693bb26e9df")]
        [TestCase(3072, "d70ff9", "d37b22fa27c9d330b9be0dd829c736ab87128aaae055a4f26c070693bb26e9df")]
        public void ShouldPassWithGoodParameters(int nlen, string e, string seed)
        {
            var subject = new AllProbablePrimesWithConditionsGenerator(EntropyProviderTypes.Random);
            subject.SetBitlens(208, 231, 144, 244);
            var result = subject.GeneratePrimes(nlen, new BitString(e).ToPositiveBigInteger(), new BitString(seed));
            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        [Test]
        [TestCase(2048, 200, 176, 352, 144, "0100000001",       // Line 31391
            "3bca340579017b28e5e162f455e4d61c72e7701915214f5051",
            "16a0e3ad12cad2f7789dde4b1612f0e366cf943244f7",
            "cfd9fa055727e42f85b1799036a4ac82fd65075bb333266f28d68a2bed7a09e7bf0cdd82ce8a4a86fd373db1aad256155c15a1c9fba5413c1e7ead6a0e71f8898dbb5f1d2131e3433de7fb658fd46cf4c1e914dd423bbfe2b6e00507b088954d29b4ca0ed0c5d78f19e91dc0420a448785f28327e6618182db7963e3f5c75ba8",
            "e30b4afef880afdff67cf23693919d70a10f8f816120b7d66a6aa119a9a9e5283bb3ac080624892d595ecc6f",
            "161f14d96abeeb7ba4c76c0ceb6c0aa457b7",
            "f8a240d2a81f380bcdeae5ce6a375e92b6388cb43661225eeec7ae2a016fd740f3e762189060797e07f4bbcb426e9ea4020cdd04cd007191b5c9730f8649133238daebc7964fdd5b8a967a791a89810b515073972cfa7745374ca75cdf3540a1be60751cc498b8e2098d59a2ffe256d38241255a6c8d312c07ed5f12f3530440",
            "cfd9fa055727e42f85b1799036a4ac82fd65075bb333266f28d68a2bed7a09e7bf0cdd82ce8a4a86fd373db1aad256155c15a1c9fba5413c1e7ead6a0e71f8898dbb5f1d2131e3433de7fb658fd46cf4cdd2fecf1feea0a42e40638497065c6c415160109adfbfd93df218d5ad96bbd2d313878e74541403eef05a5f276791f7",
            "f8a240d2a81f380bcdeae5ce6a375e92b6388cb43661225eeec7ae2a016fd740f3e762189060797e07f4bbcb426e9ea4020cdd04cd007191b5c9730f86491332390802159c1f607b33e3e9cac50252fed3df71c68862a5fe789c7122a7a6e9a058d41b1a5e275aaeb2d9748a0ecd65c7f13fa42dd882bc7dece3bd2153e1949b"
        )]
        [TestCase(2048, 216, 144, 280, 176, "0100000001",
            "0831ae7b2c7d8bee85ec6247bd044249d8728ad230a519fde6ace5",
            "3de67f42d8d8b93ad04c24f6bc27d2b52319",
            "ef207ff9176b61275de0dc6d8741e3a89d06fc09b24650d57b625be2a72cc86925751de2216b3812337818a3a09f33d17aa39035665c43e327fea74c87c013cdbad72115ac0340ce00c4c81a8dcea81f9ac158db347c8ad4856ea009b1800a70c78fd87107415c1aae8adce4cc7abe75872ecb682e7594eef39ed4d24451cf13",
            "e35bec9b3a8f41e380f638694aaf7b481c79528384fb60cbb514fde7fb885602366f59",
            "02e51d45906cb388c963212e2211500a42f799a2ea89",
            "fd39115c29fc5a755af9e1832574ee91f84517e97de018cd72c0aeb17600f9b0fcb3dbf17ac824a0c2d3fa58c4280c2eacae3a5ea28270d45bac53157826a8f116c01361579d33276120f1e0f4c5b666ec8993a608a7b917f7aee392fbdd5f1fb5adb817fa9e004137eb4e2e78428ce8830ed1e7cac2eda2dc6ef00dee377d63",
            "ef207ff9176b61275de0dc6d8741e3a89d06fc09b24650d57b625be2a72cc86925751de2216b3812337818a3a09f33d17aa39035665c43e327fea74c87c013cdbad72115ac0340ce00c4c81a8dcea81f9ac15ce3ed453a5b3df87361f73337f8630c8b0cc25c2c3dfb33ffde7669e3352f71be2ebc878a8f93ce88cb5e77fb0b",
            "fd39115c29fc5a755af9e1832574ee91f84517e97de018cd72c0aeb17600f9b0fcb3dbf17ac824a0c2d3fa58c4280c2eacae3a5ea28270d45bac53157826a8f116c01361579d4a25764c9f51462b8f6bc780e21e2c530110f4f86194aeaf367e698c1c81107832a9b620549074a64f03d8b649ea6339d0e4988753ee4914f147"
        )]
        [TestCase(3072, 528, 208, 544, 192, "0100000001",
            "8f30b941465f2ae92200867ba5b90e6aa3c704a79d657e4ba7a9701c4c59c7cf952965d693b7e506d4b0856f8ad75c9d38db0163c27753905566174d7df30f111677",
            "7ee5a347a6c0b9a733cee629d5ee3f265f925150ccf3b8d9ccb3",
            "f57f3544326ab0cb2da6a8bf8a28b0830ba70a50fafb53514bc443efcb3f28222cac63b5536cf7e2cb2a2427ced3251193b5092de923bfe26254eb798c296d0feb2ba7e8c09d282047f8cb15353f6ad4268b46570fca1338f5b5bab9aeafb905623f758824a4f1c3e43a86c62acc4fa9e4b6f91dba59d41b34b8549477f17cabc143d1a58e80bd432f6d69b59005b86476917c2d6946ada1f084a2f906ad840814be5004306a4a87b36d8df632dd43da45899b6a041f0554c3331d677224ee95",
            "b77afdae8d1a401375d2d940c3625eab39b761c508067e4335bfeb47f4027f301e70e7d3edfd4e9e6e353f61699045ab07241976e64631d47285db16167befc6380d40f3",
            "1c5dc9f839851150af8910c83320ba14f76edeeee7eb4abf",
            "b6bbf9ef00fbd60aee10b2bb4c57e19cdf9e05c4b4489ab0592cf6c1fbbe5b7f02720e01c6943c661f7f3607570368a9bdf51dbe027d3f00805a6058079d2b55dbe1649c3154727d80832e0f43a50d2dd104ff080d9c732b82e77a739f1f07189b54ba66948bff8a5adf93a3121bebe39275b7e0886aa30c7974c7c69fb7e7ef4f5d91ee572aa3e2efc262246772afade2f1501b50627c56447c10887e289fccb5d089409bdc45f372958d5b78e82a9b41ee6b111ff6988608dceca0a0e74646",
            "f57f3544326ab0cb2da6a8bf8a28b0830ba70a50fafb53514bc443efcb3f28222cac63b5536cf7e2cb2a2427ced3251193b5092de923bfe26254eb798c296d0feb2ba7e8c09d282047f8cb15353f6ad4268b46570fca1338f5b5bab9aeafb905623f761665e71b5a8eb64a5d037fa6eb1b1143d9c9dad0fa70eba170a2dc94d679ce57d911c8e69030672a86f1a1e0619177b6c6bbb55b3e2b74a8b97ce1798a4060fc6aac8d8b7991e7b2baefce026f59aa19db386e16f3c5d6ec3111d1c107",
            "b6bbf9ef00fbd60aee10b2bb4c57e19cdf9e05c4b4489ab0592cf6c1fbbe5b7f02720e01c6943c661f7f3607570368a9bdf51dbe027d3f00805a6058079d2b55dbe1649c3154727d80832e0f43a50d2dd104ff080d9c732b82e77a739f1f07189b54ba8026ec218290b256c272a8060e38bb5a7c4aab6534c5f907cd5d518a2a72ba240ca460d03b6d8ba044bbd4015e6d55f9902014c4a5571cfbaeea018c3d58556d7a65d0877bf298383fda800f824e314634cbad7ff4ad934e21b3528ca5"
        )]
        [TestCase(3072, 456, 176, 384, 360, "957a31",
            "0db49da0bbfbc493e76e0de28dbeb77c9e0cba0637964c2d3a0496e7bd61893cac9db8835b89c2d9a20a2fada2badf1b04ec794a08a65db431",
            "7b17c1d883d74bbf56ef7f5f7edff92b166984678c5f",
            "fac2da5142d21b94d6c8940bd88c5f2c7a2cb8f01aa1f3505d218e9f08c4f241754fafa226858257f8e13fd24b549d65f638a3b93aecd0bb9c0e9fa4e46a74a208403a836d1885657ca9abf0fd61d56e6031c7317752f6f0eea6163f99d538ab4bb13a97e66e2fa10218af191ac15d18b973b33ed6147f5b72dc66bf965b2ef94c5db61802bc2ac33fc1d61ff89e17e9a3114819b101611dd975a13a1100b363a61918f315a5f2871549866b86f5e1682ad4dbbf451afa398dbb3051b1c61555",
            "ed02a9f6cad62a2eb74ee2fa9198935eb1f6220fb67f94d14495abbcc741d9709c2dcd2a2b9876b48fc591a9d9752f67",
            "35f8b4af8d2422143091d5240cc844bb561ae2ea44220672fdc175deb4b5b58fab73f814fa53dc14a42c015831",
            "ce800174dd58fa6b980e59b03033feafcdb9920fce453a0134bdfe137852c944b4e588bb442d0104d6634f95e590a2b16b5dab73ae4d27495b01c45e837c501e34210ec6dc2ac2e73918ae114dfc01664fea913749ef7960947072d646b03da3ac8a0b695b772af9d50556d62bd5aefa1cddf4bdf3ee82d7f0318b1921f94b0f0e60fca8e0dd4358f27b71e7d5d5b0a87fc8353dc032f3ff5c1f469ee32475cd0e8b008195c0b6f4b063e76a505f65c3aa1f49d448fff5c22fd44b2979bea250",
            "fac2da5142d21b94d6c8940bd88c5f2c7a2cb8f01aa1f3505d218e9f08c4f241754fafa226858257f8e13fd24b549d65f638a3b93aecd0bb9c0e9fa4e46a74a208403a836d1885657ca9abf0fd61d56e6031c7317752f6f0eea6163f99d538ab4bb13a97e66e2fa10218af191ac15d18e4904a1af3e62e3654628c7b2ae29712945f99e8a6b0272805dbaadd83990d7186f5be2711a286023365e6fd72920b2de5d29748095c49ec631d5d5eba61925ebafbe73eba61186936af638c1e9ddd1d",
            "ce800174dd58fa6b980e59b03033feafcdb9920fce453a0134bdfe137852c944b4e588bb442d0104d6634f95e590a2b16b5dab73ae4d27495b01c45e837c501e34210ec6dc2ac2e73918ae114dfc01664fea913749ef7960947072d646b03da3ac8d3e42c179be675f81f8533ac22d24752f8dd48064bceaa8b6e1945dd0de52912bb3f544b58825c372d59e3f5bb6f0b6d7470fd4c5efee507f38a40d55e74791677e80271d89ef09d1f2958eb249d788038f5c16b1f66b1febd6ee7d1fbc9d"
        )]
        public void ShouldCorrectlyGeneratePrimes(int nlen, int bitlen1, int bitlen2, int bitlen3, int bitlen4, string e,
            string xp1, string xp2, string xp, string xq1, string xq2, string xq, string p, string q)
        {
            var eBS = new BitString(e).ToPositiveBigInteger();

            var subject = new AllProbablePrimesWithConditionsGenerator(EntropyProviderTypes.Testable);
            subject.SetBitlens(bitlen1, bitlen2, bitlen3, bitlen4);
            subject.AddEntropy(new BitString(xp1, bitlen1));
            subject.AddEntropy(new BitString(xp2, bitlen2));
            subject.AddEntropy(new BitString(xp).ToPositiveBigInteger());
            subject.AddEntropy(new BitString(xq1, bitlen3));
            subject.AddEntropy(new BitString(xq2, bitlen4));
            subject.AddEntropy(new BitString(xq).ToPositiveBigInteger());

            var result = subject.GeneratePrimes(nlen, eBS, null);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(new BitString(p).ToPositiveBigInteger(), result.P, "p");
            Assert.AreEqual(new BitString(q).ToPositiveBigInteger(), result.Q, "q");
        }
    }
}
