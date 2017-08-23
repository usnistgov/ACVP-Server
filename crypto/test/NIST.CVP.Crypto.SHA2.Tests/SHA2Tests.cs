using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.SHA2.Tests
{
    [TestFixture, FastIntegrationTest]
    public class SHA2Tests
    {
        private static object[] hashTests224 = new object[]
        {
            new object[]
            {
                new HashFunction {
                    DigestSize = DigestSizes.d224,
                    Mode = ModeValues.SHA2
                },
                "",
                "d14a028c2a3a2bc9476102bb288234c415a2b01f828ea62ac5b3e42f"
            },
            new object[]
            {
                new HashFunction {
                    DigestSize = DigestSizes.d224,
                    Mode = ModeValues.SHA2
                },
                "e5e09924",
                "fd19e746 90d29146 7ce59f07 7df31163 8f1c3a46 e510d0e4 9a67062d"
            },
            new object[]
            {
                new HashFunction
                {
                    DigestSize = DigestSizes.d224,
                    Mode = ModeValues.SHA2
                },
                "43506173978e3393b6f4f5b55e1b67e86d338f693ffeadc7361f71469a956c81a483c554d5322df98c581471d89b1d268b51d0ce908a90927e50cc706c2d79de08eebfb0e3a7f23b0ff7c5bbec8dbaf23febdd12846ab2dd867f6c0b2c3c5fec961ab84e68f56ca166586e5942fb2594b18a1dfdc4a8fdf07634b9df82cb08f4778e19f70d258173ba04791481ec46540bb5f856f84c571277f81bb314e15b3dd2d248",
                "8b14f57603435df8c5c228016a752a493a103b9dc8b4dd90a26bff8e"
            }
        };

        private static object[] hashTests256 = new object[]
        {
            new object[]
            {
                new HashFunction {
                    DigestSize = DigestSizes.d256,
                    Mode = ModeValues.SHA2
                },
                "",
                "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855"
            },
            new object[]
            {
                new HashFunction {
                    DigestSize = DigestSizes.d256,
                    Mode = ModeValues.SHA2
                },
                "aeee8a111c0657ac8366b9",
                "be156389cc2bb54cfb0f67d0bd61e4b9fbc2c6b9311e2e3e5186a0de1f4f2585"
            },
            new object[]
            {
                new HashFunction
                {
                    DigestSize = DigestSizes.d256,
                    Mode = ModeValues.SHA2
                },
                "0928d4a8d1d6f9ea3d40f1a271cc1712bb5d53cf6cd49268e7d77348f7d143de66392cfbcb18a97ba3c53d27a3f365da4deecbe3c222bcabd7dddc986a561930f7eddd8266b488c1272f1d3381583b4927e650100dcf09c8790c0dd15d9ceacfe4cb4f064bb47d06f205f88db30282a972cd491df5e46006f6eb6cbe77aca001565ceb4e211f6f902bcf36fee0f92040552920d32468a8ae76026226618d210f752617",
                "264136193256ba5e7c17b16722ad618ada6e7e83684f7e1dc043f85674390705"
            }
        };

        private static object[] hashTests384 = new object[]
        {
            new object[]
            {
                new HashFunction {
                    DigestSize = DigestSizes.d384,
                    Mode = ModeValues.SHA2
                },
                "",
                "38b060a751ac96384cd9327eb1b1e36a21fdb71114be07434c0cc7bf63f6e1da274edebfe76f65fbd51ad2f14898b95b"
            },
            new object[]
            {
                new HashFunction {
                    DigestSize = DigestSizes.d384,
                    Mode = ModeValues.SHA2
                },
                "c65a27957a33001cedfe",
                "f89ca74516adfb393ca97c0cb62761b95e0e8de70ec573452ced7d0cecc47bf29afc32be8f1bd8a13fe8d144d0c6ed07"
            },
            new object[]
            {
                new HashFunction
                {
                    DigestSize = DigestSizes.d384,
                    Mode = ModeValues.SHA2
                },
                "34275b83f06d5f3db8755e33351961a1cbfb0de46f0f1a23b85a073538f90cbf0c64519db2f45d841ba3d1e5011456147bd3d4a9cfdde527876389dbc7cb1b07ddf883401acbae15e0e93bf1fdbf774337c48cdb28fef5974dc8354059d2c9afcf695b34cff9383830d00420d024ad7528589d42247b34be3112752b9717fffe09413f407a86e53433df943a234bdef376166fa6695b88e35cc8b06628a1a63db407992ec37b9b53109e54079d3af30648866acf9fa6db4df5724db7b678a4b5f744d0c451df44dbf095aa4fe7fbd5f4c631f7856f6514452598b6e7e6a5e3abfb7f4c",
                "529b0d03a76088a75f08d95d9959b0096aa6092803b820a264ea8c0b8e2ccc3c3cb35fe0f183bf51acd0bcb6a2720e1f"
            }
        };

        private static object[] hashTests512 = new object[] {
            new object[]
            {
                new HashFunction {
                    DigestSize = DigestSizes.d512,
                    Mode = ModeValues.SHA2
                },
                "",
                "cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e"
            },
            new object[]
            {
                new HashFunction {
                    DigestSize = DigestSizes.d512,
                    Mode = ModeValues.SHA2
                },
                "69677b0353175dc60c25caf9454d",
                "f2c5b24703b25c215b58976cd0b90b314c38eae5f3b940aa8515cd9317af9ae9f5ecac4dce408afe7850659acbe73e3049263addc35143c80d62067b6770bbbe"
            },
            new object[]
            {
                new HashFunction {
                    DigestSize = DigestSizes.d512,
                    Mode = ModeValues.SHA2
                },
                "95046894b35d7b41403d8858841a37af8c1b2b332135902dfb94f3c4313c57c3f4f0859d7d52a3af444d08c473b04fde9a1baa34a41f79770aa11c59467f05508b771197b34418777909c3c4e8f39daa131a6daa07a9c4dffe3fd286d5957dbdc060b58b3d790121463b5e5fcb2c4a9a5fe01c9d321b96ed3f36bd52c6c4e892fd751880033a873515a9849e04a47d37e6355e150cbd1a2a354e86c501556e58a97ce37eecced23c4d1cdb877ba05f0711e1db1b7fd6751d484ed5e76f9036962b3ebd8ee27f09be555c0b25196b189446ab3cb673b0d24fe10051c1f6bc69d4ed834f",
                "715c1acf3ca335d258641413cefe130a45846b78b6c167e7d9f6ed8a1208ed47b68c425940c77fbb7f10e5eaaed1647cd9413588f3f7e3a9068ba121a9c662fb"
            }
        };

        private static object[] hashTests512t224 = new object[]
        {
            new object[]
            {
                new HashFunction {
                    DigestSize = DigestSizes.d512t224,
                    Mode = ModeValues.SHA2
                },
                "",
                "6ed0dd02806fa89e25de060c19d3ac86cabb87d6a0ddd05c333b84f4"
            },
            new object[]
            {
                new HashFunction {
                    DigestSize = DigestSizes.d512t224,
                    Mode = ModeValues.SHA2
                },
                "a9071cc0e0",
                "95ecd0654e3a32385c285e9e490427362d69154bb93c59d0b2f12ce0"
            },
            new object[]
            {
                new HashFunction
                {
                    DigestSize = DigestSizes.d512t224,
                    Mode = ModeValues.SHA2
                },
                "1aa0ce7698d46c2ff175bed56a0947b1b9bc8dfdba4b6fed6320b9f0fabff6338560856683ddefc4d9cf784d0ff4f456bcd5dc443fa5a195b06b40201764179ddba2f7677dfa7a81851d97d7bd0573c79e937ae00876afea364e4026f0115ed702ae4b00eef1b570dea742fcdc026c0c453f8f597b8540f4db11e18bef9072e8e4ccaaba3e8b4798cab4a243d4b4862b9b204235029cfdbc88fb4ad67aa8fdd868443c1cd590d901328edf340ee36a2e8780bcfe04818e492655a48ffb20a4af755d28ae1bc812b4ff7b2057f057c01cf0a5253be9fd9aa39b67163ed9c11176f56197",
                "e08de1e77b20f1d62f35ef5bc6c74ee2c9b84461c4b53af7c66ddae9"
            }
        };

        private static object[] hashTests512t256 = new object[]
        {
            new object[]
            {
                new HashFunction {
                    DigestSize = DigestSizes.d512t256,
                    Mode = ModeValues.SHA2
                },
                "",
                "c672b8d1ef56ed28ab87c3622c5114069bdd3ad7b8f9737498d0c01ecef0967a"
            },
            new object[]
            {
                new HashFunction {
                    DigestSize = DigestSizes.d512t256,
                    Mode = ModeValues.SHA2
                },
                "1cb5dd77a49fdc888fc8",
                "4e483c4045558d5dadf9e25b85332e7c5273eb5d6b730225c0cd1ce2446d497c"
            },
            new object[]
            {
                new HashFunction
                {
                    DigestSize = DigestSizes.d512t256,
                    Mode = ModeValues.SHA2
                },
                "3dffcbe4114fb2e91257b8b550f41b1f97d49d744ed0846b9f7ce65ad9fc23a91219b387109422dd6ab95880ea381eab3631deacaf8eaadd9a9580d67ebdcde1ccc12fafc5f47f4999d40f48cce13049deb0f89c2b3b423c107bf5044f398a1fb521c8c1fa18d214e8cf6476bf985ae1d97972aa8a7f526f4a55cd4b1615624b135e052675a723289fd3e0708b05a35b6db5d4409304e45e904c8e1299fc5d4e2fa36f46ff4a786c0607099ff8cf139fe38ca7c50f6ffff12a88c2c3a194840e51168c895fa9dbc864946a6bce9fb296832571a1c76bac105f30f0c5f685de74c1dfe7",
                "4a0004ee298008558fff4662989f2ad424bc236570244297073c47910c057e1d"
            }
        };

        [Test]
        [TestCaseSource(nameof(hashTests224))]
        [TestCaseSource(nameof(hashTests256))]
        [TestCaseSource(nameof(hashTests384))]
        [TestCaseSource(nameof(hashTests512))]
        [TestCaseSource(nameof(hashTests512t224))]
        [TestCaseSource(nameof(hashTests512t256))]
        public void ShouldSHA2HashCorrectly(HashFunction hashFunction, string messageHex, string digestHex)
        {
            var sha2 = new SHA2(new SHAInternals(hashFunction));
            var message = new BitString(messageHex);
            var expectedDigest = new BitString(digestHex);

            var result = sha2.HashMessage(message);

            Assert.AreEqual(expectedDigest, result);
        }

        [Test]
        [TestCase("224", 1, "00", "d3fe57cb76cdd24e9eb23e7e15684e039c75459beaae100f89712e9d")]
        [TestCase("224", 2, "00", "f9352a492bcad2f1a44d31e697468b2491db0528caa40a7a86f52283")]
        [TestCase("224", 9, "F300", "b613c35a8268594498c8c2e963ce9467a7f4fb905e9e6c7da266af14")]
        [TestCase("224", 611, "563bb1cf219b8b8d445c0a0e096fe67617b2192e44724b87aa2d8ea0fd9c4b891cf07ca8e55d1c5c4ed5dce1b116187468e609b79a9d3c5654e543ebdcbeaa1f42c52d1237b527fe222e4e1760", "db686c2aaf6f9c0c707b0335470c6ab5da6fa8a21e7b95f1bf8e7e39")]
        [TestCase("224", 1007, "3ae0c67e4d6e0d186f76ebe0340fa9475acc2111a23f2effdebb61ab4c5ef6b3224f06196a45feea10494f1e3abc67c0d79be14f7b02f8636aef4293f3499b97ff215eaae937dc3290c006a3ab666da15d916d650ae5a461680ca927d9b5fb87f8dc38f7714c9039176378373016a33213342e1bf6ef1b410198002da7aa", "fd56f5761fd9e9e13ce10f8017f9d02a1f9bf45f77d701ec8a7e2124")]

        [TestCase("256", 1, "00", "bd4f9e98beb68c6ead3243b1b4c7fed75fa4feaab1f84795cbd8a98676a2a375")]
        [TestCase("256", 2, "40", "adf8a3bf1516307c387866f77013775d421cb379a2703d2e633eebab433b8233")]
        [TestCase("256", 9, "9500", "6db58c0be5e58a4b4fd984e5837bcdb0f094e622ed85b7c8a1ec79726d03d219")]
        [TestCase("256", 611, "af899bed1245f67bd013b30b0ed24b012db0449ffb9003832ab0e2710188825351f5637eab96b137d076617669ec7deaf8c0bc3c0f8f5698c071166a54d8b1653fbc56ca54f586e736e826de00", "766ae82a86cd52451adb8b84baa205d6dfbb5bc067a19eb1e71be1963fc64a5a")]
        [TestCase("256", 1007, "830b9ddaf8109fdf06cef473cd2eee7442a64094e015585b9beeef701699de14151633ef35e4840fdb21832e6af2d333b9251afcc415585ba1ce775eb67223d42adcdb0c998248e00ca0cef228936236a5829bb102440b80777ecf1b2c0c65dba8a3beba8cb4945b8193fe4a6fd84284f006ea3a046a1ad20646202fde2e", "ee83d8c124590a97880e66a1c2d92073f1ef377b09c41f1814421ee772685cea")]

        [TestCase("384", 1, "80", "9eef0094544d88a6e9ccdf9e31d039c5ca96682293ab1cc3afc6016486190f3d20c89d5a13ebc9d13ff011b411af9186")]
        [TestCase("384", 2, "00", "c6b08368812f4f02aaf84c1b8fcd549f53099816b212fe68cb32f6d73563fae8cec52b96051ade12ba8f3c6a6e98a616")]
        [TestCase("384", 9, "a400", "277ec060a3ff558addd832d749e020666a68e6045dec015a2cfd7f507e871910969d650061ddf080a9d991c94370b524")]
        [TestCase("384", 1123, "f90c330859e9dc8b942b8589f63511269e01a3c9e9c3b3da2ec962caaca60a54c08f1a035c0521f5b4b528de3a9c454fcd6e6d392d398aadb345cde53f4046887197b22a1c279412f715874196fad2b099443dd824d923778ffdc9925e75eeb673eca38482975d6dc61549b9f299dfce6b4abaafb4eaa7c329b6fcd8f18e2b672e569619769ca58b877b164e20", "700f6f98aa5aaf90d5dc934dcbadd7f71b399fef366c5adfe6e442d0d71e4de25bc66bc63f147e10b0ef1026e6c60d62")]
        [TestCase("384", 2014, "244a7972a79d2c9f96aea08f203da4a7a876ccff76cec166ac2edca67b53f3099675731be726afb2827e782cd775eef82b90955849c0ac647487ba53a15c69f6e351978527f3ab9e2171258b86017ac47c801eb44f58e2c065a78086749e7462f2270e37cdcac66c5dd1cf3497aaef13838e8d9ef0600c8266d857c75ce0bd56ab3e00b94374a8231ce49db07038f5ed28010b9b949ed1b05e60679fc1ececd9f6e09494efb9facb48f4b8867b73365b5323bf35a3dcda543788d7950b88a8f4bb53f24f3b60646dc847483e1e235764ec3ad2f385e0ce75d898d032a27d99aee2e043728e338d0f84257461c30f0311db8f6b5da272561c29d879fc", "1e16f1b69251ff30733a63c2d9f38e1a9c8045d47a12596915ea06b75314a4846ebcf3fb858158e6153ed5950c2e356e")]

        [TestCase("512", 1, "00", "b4594eb12959fc2e6979b6783554299cc0369f44083a8b0955baefd8830cda22894b0b46c0ed49490e391ad99af856cc1bd96f238c7f2a17cf37aeb7e793395a")]
        [TestCase("512", 2, "00", "c28f076e71233070d7586154c144577eaa578c3b2d9827010690594ca8677f7ed56ad3b30450c2abebba721abfa5df0030f4b493f03c51fb89760a7fc75a174b")]
        [TestCase("512", 9, "db80", "03dcbb4b6ee5723acb2e61500a776138ea192ae9fb5a79c9eac1d727702ef51b88f50cc372ad13e7a50391bad9ca38a987ad22eb00b7da86d81abddf50ace80d")]
        [TestCase("512", 1123, "de091056a9ad543931c4430b1836b98c2e641dd342b1dbed394114c75d01944df7e704d41f7b60b5d396ac3b4769e3c07cc23112148b611c659399c2cc92450c4367518736a39be8934ba8f321da969b1286516bdbc40a24430ee5a8bae4f216ed115f37972fec199a6d9ef94e90b8661938e7a44065bd4ffc396040d10084b51b6d96abea263a8f0f84f81660", "e85f94b10407d364a989fcf290c60967e94be383369d8b492962605c5e8dbc7bb303049c4899fb8228083bdb14581ee281b0b6ac8cfe1ed7766007c5620c7e99")]
        [TestCase("512", 2014, "c41a4d431e05daef61acd570cddf1c39d7c97feb7423242393a82996ebc98c61f8a1489a402bc12bdbb362e0328a85be07fe9397b6896cdbba4909b172b06046257d876545eb1d49f028e6afbc08b52fb4fb8fcb9e3d0dc9804c02be629f5eec549552cdb3acb5f0a9f3a704f3c27433e7a8bbb1b5a5cf94ed995de641fd4ff88dd1f1fa14d552c90cfcee085e1e8972a7ed5e6e822a79e4091962509932fb15d619ab13efd0bc7b242c02e28585be95fcb2c12d8d34d563ddb25825f1a62ae93955ca41cd04bbaff6692dbbf060d944eedf2d145f2ca9b7704d898cd2c1a41cbd6d94ab35d9170d8c9cb5bb2815a326865ce84b7f78be88cad33bac", "9d05bfc1549b1156abc2b95d52d519d7f19c794d193d271fc1ac1305af20b1a73cb9a7ad9dced16517e19a2e23f06863f1b9350227e0accf7107e9d0b00e0a78")]

        [TestCase("512t224", 1, "00", "5cb2c91954ab4fc72c555fd379268bf272782516ec5da0660c421dd1")]
        [TestCase("512t224", 2, "40", "e507c39a514a4bb104c4999f1f1f1b9022b7ce0e17fbf1cb65834605")]
        [TestCase("512t224", 9, "E200", "f8e683bf8b0f0327202030f404c1f53774b0266f8b935cc64822c8a3")]
        [TestCase("512t224", 1123, "f4648b26d43ae9a6c169ba5781f012ccec95aba9a1cf34adf9e2ebef394c6e670c60ceb13031e60e6fb1ebacd11a4427108d9b4f4cf9342c003f7d3f3594e70bf526f5d4030292a7c935a89a9231ee55f97302d1a163b760277cb0ee55ac2e8699fe2618b5769779b73c1aab2cfebb5f918f07b80ad4665257e0ab83035eeae0de2f8903ae559a8c2210686600", "b0c8e44c052a5ff2f0f7195fa8d20ebfc81b6ea1e0bd9bbf2879500d")]
        [TestCase("512t224", 2014, "6de16c47b13840802e883c0b4c29a52b178a7ee94025aaac64b25209919b86dc07b127983b1d04db22994346abe0038b8e623fc7443df51356698855db32645d3cece02d3be26897348fdb0ea21132b6e4da63bd42a984ee2bb83483c037373b97fa60adb86fb4dcecd2cc0b38a47bd3a459b17240519f6669091c3b499348a19d436fc1bbac30d1d3cadee67581260a5346f28f491d8ea39ac309247d2cdfb5d830d4114b81239f70e0d84761907b857b0beebb62f598cc454ec4e664eb439fcf27594470d6d76d4b7a84d505bac169a30f6d9f95c10709f312122a06b8be880a91c40232939264ed02a83968e542e154ba37e3e96921842b77be98", "f0aa77a4847c2cbe7ffbce5f947457984816a2f6d40d4032785920db")]

        [TestCase("512t256", 1, "00", "d2a8cc81374ea74aa3d9e4cd62a5c5bc7a0dc516399855300cb90b0c2960dd1e")]
        [TestCase("512t256", 2, "00", "0d98f1627ee8713255aedb71e0bfeb065a16b5b242ddd57f0e4147f30b40ba66")]
        [TestCase("512t256", 9, "A800", "816a2b11eb9d40f418f3bd563e702e00287dd6aa0b92b696ed3c2edb156c36da")]
        [TestCase("512t256", 1123, "90fdb8f79b7ca049bffd905e109a4bbe86b9aaa7f334355538b7f481b96983280ece658dfcec9a81a3ac74e2f884a66276539e89118c15fe6c97e4024b74193d4ba24abdd13f293c407059a16f0380ca5a539e89176c9dec0c7029c2603ac15aaf400e8e02be7400fe11e8a45b60017f988251aeed1cf5a9820a6bc9de01a408a2725a0977b1a4584556c8f2a0", "b6861fd68c4e0e0d2b8c68b9be84d94e06f47839e82c64c05d212bf8746790fe")]
        [TestCase("512t256", 2014, "f1c85d82d73dcc0cf6ecddbb7ed3e79e14974d9bd276a2078f7d107461fbfaca6135e09aae5b83aeb5bb4ae5af35e53e6aef30324d4a5a47971446e2e7ff315c01519659cfde4e55a633fc80cb15a8cc362aaa4495f5ca9f9408a5155c7453715985466841ac752952bdba35db5a7971fdaf8379b3613a3810c0f5b6c844a630f237ba6e0caf405140bf4eace5eea05449e8837bae74f13992a6fe6c345673c253d1b81439cdf7f6f8a37e8cf4b8659fa13c71ef8e1839cba32187e0a792014f04b90902cff0e13f03cf137d0da010778e1317805c345816ca995ab929e09afe8e5975dfd6ff4755e8add5293a8ee80697d63bd41faf97418f763da0", "a5e0679f13242f45a9f5f1dd6711613b7dc4f9ae28fd679ac05a963d7a6af17b")]
        public void ShouldSHA2HashCorrectlyWithBits(string digestSize, int len, string messageHex, string digestHex)
        {
            var sha2 = new SHA2(new SHAInternals(GetHashFunction(digestSize)));
            var message = new BitString(messageHex, len);
            var expectedDigest = new BitString(digestHex);

            var result = sha2.HashMessage(message);

            Assume.That(message.BitLength == len);
            Assert.AreEqual(expectedDigest.ToHex(), result.ToHex());
        }


        private HashFunction GetHashFunction(string digestSize)
        {
            switch (digestSize)
            {
                case "224":
                    return new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d224 };
                case "256":
                    return new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d256 };
                case "384":
                    return new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d384 };
                case "512":
                    return new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d512 };
                case "512t224":
                    return new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d512t224 };
                case "512t256":
                    return new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d512t256 };
            }

            return new HashFunction { Mode = ModeValues.SHA1, DigestSize = DigestSizes.d160 };
        }
    }
}
