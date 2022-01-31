using System;
using System.Diagnostics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.LargeBitString;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SHA.Tests
{
    [TestFixture]
    [FastCryptoTest]
    public class NativeSha2Tests
    {
        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, "", "da39a3ee5e6b4b0d3255bfef95601890afd80709")]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, "616263", "A9993E364706816ABA3E25717850C26C9CD0D89D")]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, "c0fabb1f2dc66055", "b137b2b7a60e8d2f0552d0ddc3dc960245fffe6a")]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, "638985ba4eed2b8ad7a546b620a1105b6578d86278090c4a6d62982796e16eebb29866e561f64987dba4286ce2aef39af5e34704c77e8653ef062de5e17262161d91cdbfa6a9a9fdb65f1b34b0d6c253561b8f593cc1d7187cc8a638acc457800d3a6151054e7473d09bc5157263a60ef0e85969bf1926217d71ab29df1d74afeb5dcba2672cd1729123ce17109bc6542b124d3d39d09bf758c9e3bf62c6e12d1dc0b3", "fd7f2c5502a682eaf2977d7d12cc1616feb97c3e")]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, "be1dd0f6b02c2e779e5519c5c2f9397ba395ed09384f8ff250cff9393250481580f7e0ec6163b07fde85b7638db2c6b06df688fd12acc18b024bd3c05e8386d7c58908d775af33c1b3370f0e6e6998581fe3c158f331b8102c9d6c49c4484d40547a529d353741a4e5132bafac4735dc7f039f032406feeb976dccbdadc823b8f773c627e92461903bc0122f907327a256ff2ae6ed541b820b62fc025f2b93a7741c6b5dd99d1ced7fe7cc746215f6146b7e6ae99543953f5024030223982275941c49274ccbf824772f62686a572899882968f566faf6892d5ce9a340375a8b1f1c686e8cd57f9bec41dbf2ef5f479873a82bf0a6a2c5c76bb2b7cf0031c24fddc3d118dfe4", "aaa59f66a4f1947ceade2e6c00834bc726c878fc")]

        [TestCase(ModeValues.SHA2, DigestSizes.d224, "", "d14a028c2a3a2bc9476102bb288234c415a2b01f828ea62ac5b3e42f")]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, "e5e09924", "fd19e74690d291467ce59f077df311638f1c3a46e510d0e49a67062d")]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, "43506173978e3393b6f4f5b55e1b67e86d338f693ffeadc7361f71469a956c81a483c554d5322df98c581471d89b1d268b51d0ce908a90927e50cc706c2d79de08eebfb0e3a7f23b0ff7c5bbec8dbaf23febdd12846ab2dd867f6c0b2c3c5fec961ab84e68f56ca166586e5942fb2594b18a1dfdc4a8fdf07634b9df82cb08f4778e19f70d258173ba04791481ec46540bb5f856f84c571277f81bb314e15b3dd2d248", "8b14f57603435df8c5c228016a752a493a103b9dc8b4dd90a26bff8e")]

        [TestCase(ModeValues.SHA2, DigestSizes.d256, "", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, "aeee8a111c0657ac8366b9", "be156389cc2bb54cfb0f67d0bd61e4b9fbc2c6b9311e2e3e5186a0de1f4f2585")]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, "0928d4a8d1d6f9ea3d40f1a271cc1712bb5d53cf6cd49268e7d77348f7d143de66392cfbcb18a97ba3c53d27a3f365da4deecbe3c222bcabd7dddc986a561930f7eddd8266b488c1272f1d3381583b4927e650100dcf09c8790c0dd15d9ceacfe4cb4f064bb47d06f205f88db30282a972cd491df5e46006f6eb6cbe77aca001565ceb4e211f6f902bcf36fee0f92040552920d32468a8ae76026226618d210f752617", "264136193256ba5e7c17b16722ad618ada6e7e83684f7e1dc043f85674390705")]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, "616263", "ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad")]

        [TestCase(ModeValues.SHA2, DigestSizes.d384, "", "38b060a751ac96384cd9327eb1b1e36a21fdb71114be07434c0cc7bf63f6e1da274edebfe76f65fbd51ad2f14898b95b")]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, "c65a27957a33001cedfe", "f89ca74516adfb393ca97c0cb62761b95e0e8de70ec573452ced7d0cecc47bf29afc32be8f1bd8a13fe8d144d0c6ed07")]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, "34275b83f06d5f3db8755e33351961a1cbfb0de46f0f1a23b85a073538f90cbf0c64519db2f45d841ba3d1e5011456147bd3d4a9cfdde527876389dbc7cb1b07ddf883401acbae15e0e93bf1fdbf774337c48cdb28fef5974dc8354059d2c9afcf695b34cff9383830d00420d024ad7528589d42247b34be3112752b9717fffe09413f407a86e53433df943a234bdef376166fa6695b88e35cc8b06628a1a63db407992ec37b9b53109e54079d3af30648866acf9fa6db4df5724db7b678a4b5f744d0c451df44dbf095aa4fe7fbd5f4c631f7856f6514452598b6e7e6a5e3abfb7f4c", "529b0d03a76088a75f08d95d9959b0096aa6092803b820a264ea8c0b8e2ccc3c3cb35fe0f183bf51acd0bcb6a2720e1f")]

        [TestCase(ModeValues.SHA2, DigestSizes.d512, "", "cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, "69677b0353175dc60c25caf9454d", "f2c5b24703b25c215b58976cd0b90b314c38eae5f3b940aa8515cd9317af9ae9f5ecac4dce408afe7850659acbe73e3049263addc35143c80d62067b6770bbbe")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, "95046894b35d7b41403d8858841a37af8c1b2b332135902dfb94f3c4313c57c3f4f0859d7d52a3af444d08c473b04fde9a1baa34a41f79770aa11c59467f05508b771197b34418777909c3c4e8f39daa131a6daa07a9c4dffe3fd286d5957dbdc060b58b3d790121463b5e5fcb2c4a9a5fe01c9d321b96ed3f36bd52c6c4e892fd751880033a873515a9849e04a47d37e6355e150cbd1a2a354e86c501556e58a97ce37eecced23c4d1cdb877ba05f0711e1db1b7fd6751d484ed5e76f9036962b3ebd8ee27f09be555c0b25196b189446ab3cb673b0d24fe10051c1f6bc69d4ed834f", "715c1acf3ca335d258641413cefe130a45846b78b6c167e7d9f6ed8a1208ed47b68c425940c77fbb7f10e5eaaed1647cd9413588f3f7e3a9068ba121a9c662fb")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, "616263", "DDAF35A193617ABACC417349AE20413112E6FA4E89A97EA20A9EEEE64B55D39A2192992A274FC1A836BA3C23A3FEEBBD454D4423643CE80E2A9AC94FA54CA49F")]
        public void ShouldHashCorrectly(ModeValues mode, DigestSizes digestSize, string message, string expectedDigest)
        {
            var shaFactory = new NativeShaFactory();
            var sha = shaFactory.GetShaInstance(new HashFunction(mode, digestSize));
            var messageBs = new BitString(message);
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            var result = RunHash(x => sha.HashMessage(x), messageBs);
            stopWatch.Stop();

            Assert.AreEqual(expectedDigest.ToLower(), result.Digest.ToHex().ToLower(), "New");
            Console.WriteLine(
                $"New: {stopWatch.Elapsed.Minutes:D2}:{stopWatch.Elapsed.Seconds:D2}:{stopWatch.Elapsed.Milliseconds:D3}");
        }

        private HashResult RunHash(Func<BitString, HashResult> hash, BitString message)
        {
            HashResult result = null;
            for (var i = 0; i < 1 /*_000_000*/; i++) result = hash.Invoke(message);

            return result;
        }

        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 1, "00", "bb6b3e18f0115b57925241676f5b1ae88747b08a")]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 1, "80", "59C4526AA2CC59F9A5F56B5579BA7108E7CCB61A")]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 2, "c0", "d90631a32faf316a87b9582bfa4e05a2773005ca")]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 3, "00", "88BAD9D59A0A5195FAF7961BB6625486816C1430")]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 9, "ff80", "7bd5813934a8a67115358b1a5f3c5b97192b7b3b")]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 611, "513b414e77fba286ff1b612d9cc9038618fdafe8015c87bf9f5d39d328aad0cd37c9d1f77bfb8343ac648e8fc46dc7276b674b8bb371cb73059b26115c4b3d96b0e003f7b696d3f1c6a9074b00", "261730f3143cc63add4ec324e3fe2f9ab37d347e")]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 1007, "d271aa31b8b92606a10a52612dd1fab495b82f9a98cade18b9d8a723a71ceb63fd1d27372bd281f9b40aa1839b0cc2f2177a09aa8e7b159ac118d7c145e7a4f032e788d21facde2b4dbc1d5d2238f530d9bf9bd2798f611d03ed8919f0c85bc2da99750b7a8d6322d2e66ff6ab9ebaf7424e8c1c3f4fe92be61f65359106", "0ca6c7fa9aa75bd0d9420b7e4bcf017579d95b70")]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 1007, "", "DA39A3EE5E6B4B0D3255BFEF95601890AFD80709")]

        [TestCase(ModeValues.SHA2, DigestSizes.d224, 1, "00", "d3fe57cb76cdd24e9eb23e7e15684e039c75459beaae100f89712e9d")]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 2, "00", "f9352a492bcad2f1a44d31e697468b2491db0528caa40a7a86f52283")]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 9, "F300", "b613c35a8268594498c8c2e963ce9467a7f4fb905e9e6c7da266af14")]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 24, "616263", "23097D223405D8228642A477BDA255B32AADBCE4BDA0B3F7E36C9DA7")]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 448, "6162636462636465636465666465666765666768666768696768696a68696a6b696a6b6c6a6b6c6d6b6c6d6e6c6d6e6f6d6e6f706e6f7071", "75388B16512776CC5DBA5DA1FD890150B0C6455CB4F58B1952522525")]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 611, "563bb1cf219b8b8d445c0a0e096fe67617b2192e44724b87aa2d8ea0fd9c4b891cf07ca8e55d1c5c4ed5dce1b116187468e609b79a9d3c5654e543ebdcbeaa1f42c52d1237b527fe222e4e1760", "db686c2aaf6f9c0c707b0335470c6ab5da6fa8a21e7b95f1bf8e7e39")]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 1007, "3ae0c67e4d6e0d186f76ebe0340fa9475acc2111a23f2effdebb61ab4c5ef6b3224f06196a45feea10494f1e3abc67c0d79be14f7b02f8636aef4293f3499b97ff215eaae937dc3290c006a3ab666da15d916d650ae5a461680ca927d9b5fb87f8dc38f7714c9039176378373016a33213342e1bf6ef1b410198002da7aa", "fd56f5761fd9e9e13ce10f8017f9d02a1f9bf45f77d701ec8a7e2124")]

        [TestCase(ModeValues.SHA2, DigestSizes.d256, 1, "00", "bd4f9e98beb68c6ead3243b1b4c7fed75fa4feaab1f84795cbd8a98676a2a375")]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, 2, "40", "adf8a3bf1516307c387866f77013775d421cb379a2703d2e633eebab433b8233")]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, 9, "9500", "6db58c0be5e58a4b4fd984e5837bcdb0f094e622ed85b7c8a1ec79726d03d219")]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, 24, "616263", "ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad")]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, 448, "6162636462636465636465666465666765666768666768696768696a68696a6b696a6b6c6a6b6c6d6b6c6d6e6c6d6e6f6d6e6f706e6f7071", "248d6a61d20638b8e5c026930c3e6039a33ce45964ff2167f6ecedd419db06c1")]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, 611, "af899bed1245f67bd013b30b0ed24b012db0449ffb9003832ab0e2710188825351f5637eab96b137d076617669ec7deaf8c0bc3c0f8f5698c071166a54d8b1653fbc56ca54f586e736e826de00", "766ae82a86cd52451adb8b84baa205d6dfbb5bc067a19eb1e71be1963fc64a5a")]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, 1007, "830b9ddaf8109fdf06cef473cd2eee7442a64094e015585b9beeef701699de14151633ef35e4840fdb21832e6af2d333b9251afcc415585ba1ce775eb67223d42adcdb0c998248e00ca0cef228936236a5829bb102440b80777ecf1b2c0c65dba8a3beba8cb4945b8193fe4a6fd84284f006ea3a046a1ad20646202fde2e", "ee83d8c124590a97880e66a1c2d92073f1ef377b09c41f1814421ee772685cea")]

        [TestCase(ModeValues.SHA2, DigestSizes.d384, 1, "80", "9eef0094544d88a6e9ccdf9e31d039c5ca96682293ab1cc3afc6016486190f3d20c89d5a13ebc9d13ff011b411af9186")]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, 2, "00", "c6b08368812f4f02aaf84c1b8fcd549f53099816b212fe68cb32f6d73563fae8cec52b96051ade12ba8f3c6a6e98a616")]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, 9, "a400", "277ec060a3ff558addd832d749e020666a68e6045dec015a2cfd7f507e871910969d650061ddf080a9d991c94370b524")]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, 24, "616263", "CB00753F45A35E8BB5A03D699AC65007272C32AB0EDED1631A8B605A43FF5BED8086072BA1E7CC2358BAECA134C825A7")]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, 448, "6162636462636465636465666465666765666768666768696768696a68696a6b696a6b6c6a6b6c6d6b6c6d6e6c6d6e6f6d6e6f706e6f7071", "3391FDDDFC8DC7393707A65B1B4709397CF8B1D162AF05ABFE8F450DE5F36BC6B0455A8520BC4E6F5FE95B1FE3C8452B")]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, 1123, "f90c330859e9dc8b942b8589f63511269e01a3c9e9c3b3da2ec962caaca60a54c08f1a035c0521f5b4b528de3a9c454fcd6e6d392d398aadb345cde53f4046887197b22a1c279412f715874196fad2b099443dd824d923778ffdc9925e75eeb673eca38482975d6dc61549b9f299dfce6b4abaafb4eaa7c329b6fcd8f18e2b672e569619769ca58b877b164e20", "700f6f98aa5aaf90d5dc934dcbadd7f71b399fef366c5adfe6e442d0d71e4de25bc66bc63f147e10b0ef1026e6c60d62")]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, 2014, "244a7972a79d2c9f96aea08f203da4a7a876ccff76cec166ac2edca67b53f3099675731be726afb2827e782cd775eef82b90955849c0ac647487ba53a15c69f6e351978527f3ab9e2171258b86017ac47c801eb44f58e2c065a78086749e7462f2270e37cdcac66c5dd1cf3497aaef13838e8d9ef0600c8266d857c75ce0bd56ab3e00b94374a8231ce49db07038f5ed28010b9b949ed1b05e60679fc1ececd9f6e09494efb9facb48f4b8867b73365b5323bf35a3dcda543788d7950b88a8f4bb53f24f3b60646dc847483e1e235764ec3ad2f385e0ce75d898d032a27d99aee2e043728e338d0f84257461c30f0311db8f6b5da272561c29d879fc", "1e16f1b69251ff30733a63c2d9f38e1a9c8045d47a12596915ea06b75314a4846ebcf3fb858158e6153ed5950c2e356e")]

        [TestCase(ModeValues.SHA2, DigestSizes.d512, 1, "00", "b4594eb12959fc2e6979b6783554299cc0369f44083a8b0955baefd8830cda22894b0b46c0ed49490e391ad99af856cc1bd96f238c7f2a17cf37aeb7e793395a")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, 2, "00", "c28f076e71233070d7586154c144577eaa578c3b2d9827010690594ca8677f7ed56ad3b30450c2abebba721abfa5df0030f4b493f03c51fb89760a7fc75a174b")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, 9, "db80", "03dcbb4b6ee5723acb2e61500a776138ea192ae9fb5a79c9eac1d727702ef51b88f50cc372ad13e7a50391bad9ca38a987ad22eb00b7da86d81abddf50ace80d")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, 24, "616263", "DDAF35A193617ABACC417349AE20413112E6FA4E89A97EA20A9EEEE64B55D39A2192992A274FC1A836BA3C23A3FEEBBD454D4423643CE80E2A9AC94FA54CA49F")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, 448, "6162636462636465636465666465666765666768666768696768696a68696a6b696a6b6c6a6b6c6d6b6c6d6e6c6d6e6f6d6e6f706e6f7071", "204A8FC6DDA82F0A0CED7BEB8E08A41657C16EF468B228A8279BE331A703C33596FD15C13B1B07F9AA1D3BEA57789CA031AD85C7A71DD70354EC631238CA3445")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, 1123, "de091056a9ad543931c4430b1836b98c2e641dd342b1dbed394114c75d01944df7e704d41f7b60b5d396ac3b4769e3c07cc23112148b611c659399c2cc92450c4367518736a39be8934ba8f321da969b1286516bdbc40a24430ee5a8bae4f216ed115f37972fec199a6d9ef94e90b8661938e7a44065bd4ffc396040d10084b51b6d96abea263a8f0f84f81660", "e85f94b10407d364a989fcf290c60967e94be383369d8b492962605c5e8dbc7bb303049c4899fb8228083bdb14581ee281b0b6ac8cfe1ed7766007c5620c7e99")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, 2014, "c41a4d431e05daef61acd570cddf1c39d7c97feb7423242393a82996ebc98c61f8a1489a402bc12bdbb362e0328a85be07fe9397b6896cdbba4909b172b06046257d876545eb1d49f028e6afbc08b52fb4fb8fcb9e3d0dc9804c02be629f5eec549552cdb3acb5f0a9f3a704f3c27433e7a8bbb1b5a5cf94ed995de641fd4ff88dd1f1fa14d552c90cfcee085e1e8972a7ed5e6e822a79e4091962509932fb15d619ab13efd0bc7b242c02e28585be95fcb2c12d8d34d563ddb25825f1a62ae93955ca41cd04bbaff6692dbbf060d944eedf2d145f2ca9b7704d898cd2c1a41cbd6d94ab35d9170d8c9cb5bb2815a326865ce84b7f78be88cad33bac", "9d05bfc1549b1156abc2b95d52d519d7f19c794d193d271fc1ac1305af20b1a73cb9a7ad9dced16517e19a2e23f06863f1b9350227e0accf7107e9d0b00e0a78")]

        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, 1, "00", "5cb2c91954ab4fc72c555fd379268bf272782516ec5da0660c421dd1")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, 2, "40", "e507c39a514a4bb104c4999f1f1f1b9022b7ce0e17fbf1cb65834605")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, 9, "E200", "f8e683bf8b0f0327202030f404c1f53774b0266f8b935cc64822c8a3")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, 24, "616263", "4634270F707B6A54DAAE7530460842E20E37ED265CEEE9A43E8924AA")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, 448, "6162636462636465636465666465666765666768666768696768696a68696a6b696a6b6c6a6b6c6d6b6c6d6e6c6d6e6f6d6e6f706e6f7071", "E5302D6D54BB242275D1E7622D68DF6EB02DEDD13F564C13DBDA2174")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, 1123, "f4648b26d43ae9a6c169ba5781f012ccec95aba9a1cf34adf9e2ebef394c6e670c60ceb13031e60e6fb1ebacd11a4427108d9b4f4cf9342c003f7d3f3594e70bf526f5d4030292a7c935a89a9231ee55f97302d1a163b760277cb0ee55ac2e8699fe2618b5769779b73c1aab2cfebb5f918f07b80ad4665257e0ab83035eeae0de2f8903ae559a8c2210686600", "b0c8e44c052a5ff2f0f7195fa8d20ebfc81b6ea1e0bd9bbf2879500d")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, 2014, "6de16c47b13840802e883c0b4c29a52b178a7ee94025aaac64b25209919b86dc07b127983b1d04db22994346abe0038b8e623fc7443df51356698855db32645d3cece02d3be26897348fdb0ea21132b6e4da63bd42a984ee2bb83483c037373b97fa60adb86fb4dcecd2cc0b38a47bd3a459b17240519f6669091c3b499348a19d436fc1bbac30d1d3cadee67581260a5346f28f491d8ea39ac309247d2cdfb5d830d4114b81239f70e0d84761907b857b0beebb62f598cc454ec4e664eb439fcf27594470d6d76d4b7a84d505bac169a30f6d9f95c10709f312122a06b8be880a91c40232939264ed02a83968e542e154ba37e3e96921842b77be98", "f0aa77a4847c2cbe7ffbce5f947457984816a2f6d40d4032785920db")]

        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, 1, "00", "d2a8cc81374ea74aa3d9e4cd62a5c5bc7a0dc516399855300cb90b0c2960dd1e")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, 2, "00", "0d98f1627ee8713255aedb71e0bfeb065a16b5b242ddd57f0e4147f30b40ba66")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, 9, "A800", "816a2b11eb9d40f418f3bd563e702e00287dd6aa0b92b696ed3c2edb156c36da")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, 24, "616263", "53048E2681941EF99B2E29B76B4C7DABE4C2D0C634FC6D46E0E2F13107E7AF23")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, 448, "6162636462636465636465666465666765666768666768696768696a68696a6b696a6b6c6a6b6c6d6b6c6d6e6c6d6e6f6d6e6f706e6f7071", "BDE8E1F9F19BB9FD3406C90EC6BC47BD36D8ADA9F11880DBC8A22A7078B6A461")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, 1123, "90fdb8f79b7ca049bffd905e109a4bbe86b9aaa7f334355538b7f481b96983280ece658dfcec9a81a3ac74e2f884a66276539e89118c15fe6c97e4024b74193d4ba24abdd13f293c407059a16f0380ca5a539e89176c9dec0c7029c2603ac15aaf400e8e02be7400fe11e8a45b60017f988251aeed1cf5a9820a6bc9de01a408a2725a0977b1a4584556c8f2a0", "b6861fd68c4e0e0d2b8c68b9be84d94e06f47839e82c64c05d212bf8746790fe")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, 2014, "f1c85d82d73dcc0cf6ecddbb7ed3e79e14974d9bd276a2078f7d107461fbfaca6135e09aae5b83aeb5bb4ae5af35e53e6aef30324d4a5a47971446e2e7ff315c01519659cfde4e55a633fc80cb15a8cc362aaa4495f5ca9f9408a5155c7453715985466841ac752952bdba35db5a7971fdaf8379b3613a3810c0f5b6c844a630f237ba6e0caf405140bf4eace5eea05449e8837bae74f13992a6fe6c345673c253d1b81439cdf7f6f8a37e8cf4b8659fa13c71ef8e1839cba32187e0a792014f04b90902cff0e13f03cf137d0da010778e1317805c345816ca995ab929e09afe8e5975dfd6ff4755e8add5293a8ee80697d63bd41faf97418f763da0", "a5e0679f13242f45a9f5f1dd6711613b7dc4f9ae28fd679ac05a963d7a6af17b")]
        public void ShouldHashBitsCorrectly(ModeValues mode, DigestSizes digestSize, int bitLength, string message, string expectedDigest)
        {
            var shaFactory = new NativeShaFactory();
            var sha = shaFactory.GetShaInstance(new HashFunction(mode, digestSize));
            var messageBs = new BitString(message, bitLength);
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            var bitResult = RunHash(x => sha.HashMessage(x), messageBs);
            stopWatch.Stop();

            Assert.AreEqual(expectedDigest.ToLower(), bitResult.Digest.ToHex().ToLower());
            Console.WriteLine(
                $"BitOriented: {stopWatch.Elapsed.Minutes:D2}:{stopWatch.Elapsed.Seconds:D2}:{stopWatch.Elapsed.Milliseconds:D3}");
        }

        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 1, 1, "80", "80", "d90631a32faf316a87b9582bfa4e05a2773005ca")]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 610, 1,
            "513b414e77fba286ff1b612d9cc9038618fdafe8015c87bf9f5d39d328aad0cd37c9d1f77bfb8343ac648e8fc46dc7276b674b8bb371cb73059b26115c4b3d96b0e003f7b696d3f1c6a9074b00",
            "00", "261730f3143cc63add4ec324e3fe2f9ab37d347e")]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 123, 488, "513B414E77FBA286FF1B612D9CC90380",
            "30C7ED7F400AE43DFCFAE9CE9945568669BE4E8FBBDFDC1A1D6324747E236E393B5B3A5C5D9B8E5B982CD9308AE259ECB587001FBDB4B69F8E35483A58",
            "261730f3143cc63add4ec324e3fe2f9ab37d347e")]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 511, 496,
            "d271aa31b8b92606a10a52612dd1fab495b82f9a98cade18b9d8a723a71ceb63fd1d27372bd281f9b40aa1839b0cc2f2177a09aa8e7b159ac118d7c145e7a4f0",
            "1973C4690FD66F15A6DE0EAE911C7A986CDFCDE93CC7B08E81F6C48CF8642DE16D4CBA85BD46B191697337FB55CF5D7BA127460E1FA7F495F30FB29AC883",
            "0ca6c7fa9aa75bd0d9420b7e4bcf017579d95b70")]

        [TestCase(ModeValues.SHA2, DigestSizes.d512, 1, 1, "00", "00",
            "c28f076e71233070d7586154c144577eaa578c3b2d9827010690594ca8677f7ed56ad3b30450c2abebba721abfa5df0030f4b493f03c51fb89760a7fc75a174b")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, 7, 2, "da", "c0",
            "03dcbb4b6ee5723acb2e61500a776138ea192ae9fb5a79c9eac1d727702ef51b88f50cc372ad13e7a50391bad9ca38a987ad22eb00b7da86d81abddf50ace80d")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, 607, 516,
            "DE091056A9AD543931C4430B1836B98C2E641DD342B1DBED394114C75D01944DF7E704D41F7B60B5D396AC3B4769E3C07CC23112148B611C659399C2CC92450C4367518736A39BE8934BA8F2",
            "90ED4B4D894328B5EDE20512218772D45D72790B7688AF9BCB97F60CCD36CF7CA7485C330C9C73D22032DEA7FE1CB0206880425A8DB6CB55F5131D4787C27C0B30",
            "e85f94b10407d364a989fcf290c60967e94be383369d8b492962605c5e8dbc7bb303049c4899fb8228083bdb14581ee281b0b6ac8cfe1ed7766007c5620c7e99")]
        public void ShouldSupportMultipleUpdates(ModeValues mode, DigestSizes digestSize, int bitLength1,
            int bitLength2, string message1, string message2, string expectedDigest)
        {
            var messageBytes1 = new BitString(message1, bitLength1).GetPaddedBytes();
            var messageBytes2 = new BitString(message2, bitLength2).GetPaddedBytes();

            var hashFunction = new HashFunction(mode, digestSize);
            var shaFactory = new NativeShaFactory();
            var sha = shaFactory.GetShaInstance(hashFunction);

            sha.Init();
            sha.Update(messageBytes1, bitLength1);
            sha.Update(messageBytes2, bitLength2);

            var resultBytes = new byte[hashFunction.OutputLen / 8];
            sha.Final(resultBytes);
            var result = new BitString(resultBytes);

            Assert.AreEqual(expectedDigest.ToLower(), result.ToHex().ToLower());
        }

        // This is pulled from https://csrc.nist.gov/CSRC/media/Projects/Hash-Functions/documents/SHA3-KATMCT1.pdf which uses SHA2-256 as their example for the SHA3 competition
        [TestCase(ModeValues.SHA2, DigestSizes.d256, 8589934592, "61626364656667686263646566676869636465666768696A6465666768696A6B65666768696A6B6C666768696A6B6C6D6768696A6B6C6D6E68696A6B6C6D6E6F", "50e72a0e26442fe2552dc3938ac58658228c0cbfb1d2ca872ae435266fcd055e")]

        // Remaining tests have their "expected hex" generated from this test case
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 8589934592, "61626364656667686263646566676869636465666768696A6465666768696A6B65666768696A6B6C666768696A6B6C6D6768696A6B6C6D6E68696A6B6C6D6E6F", "7789F0C9EF7BFC40D93311143DFBE69E2017F592")]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 8589934592, "61626364656667686263646566676869636465666768696A6465666768696A6B65666768696A6B6C666768696A6B6C6D6768696A6B6C6D6E68696A6B6C6D6E6F", "B5989713CA4FE47A009F8621980B34E6D63ED3063B2A0A2C867D8A85")]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, 8589934592, "61626364656667686263646566676869636465666768696A6465666768696A6B65666768696A6B6C666768696A6B6C6D6768696A6B6C6D6E68696A6B6C6D6E6F", "5441235CC0235341ED806A64FB354742B5E5C02A3C5CB71B5F63FB793458D8FDAE599C8CD8884943C04F11B31B89F023")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, 8589934592, "61626364656667686263646566676869636465666768696A6465666768696A6B65666768696A6B6C666768696A6B6C6D6768696A6B6C6D6E68696A6B6C6D6E6F", "B47C933421EA2DB149AD6E10FCE6C7F93D0752380180FFD7F4629A712134831D77BE6091B819ED352C2967A2E2D4FA5050723C9630691F1A05A7281DBE6C1086")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, 8589934592, "61626364656667686263646566676869636465666768696A6465666768696A6B65666768696A6B6C666768696A6B6C6D6768696A6B6C6D6E68696A6B6C6D6E6F", "9A7F86727C3BE1403D6702617646B15589B8C5A92C70F1703CD25B52")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, 8589934592, "61626364656667686263646566676869636465666768696A6465666768696A6B65666768696A6B6C666768696A6B6C6D6768696A6B6C6D6E68696A6B6C6D6E6F", "B5855A6179802CE567CBF43888284C6AC7C3F6C48B08C5BC1E8AD75D12782C9E")]

        [TestCase(ModeValues.SHA2, DigestSizes.d256, 8589934592 * 8, "ABCDABCDABCDABCD", "1BA3312BCEA4C7230B2EE95A7E59AAF804D74331DD366E3C0B1C25DA3368374F")]
        public void Sha2LargeDataShouldBeCorrect(ModeValues mode, DigestSizes digestSize, long extendSize, string inputHex, string outputHex)
        {
            var message = new BitString(inputHex);
            var expectedResult = new BitString(outputHex);

            var largeMessage = new LargeBitString
            {
                Content = message,
                ExpansionTechnique = ExpansionMode.Repeating,
                FullLength = extendSize
            };

            var shaFactory = new NativeShaFactory();
            var sha = shaFactory.GetShaInstance(new HashFunction(mode, digestSize));
            var result = sha.HashLargeMessage(largeMessage);

            Assert.That(result.Success);

            //Console.WriteLine(result.Digest.ToHex());
            Assert.AreEqual(expectedResult.ToHex(), result.Digest.ToHex());
        }

        [Test]
        public void WhenUsingPartialWordUpdateChunks_Should256HashIntoSameDigestAsSingleUpdate()
        {
            var sha = new NativeFastSha2_256();

            var bs = new BitString("112233 445566 778899 AABBCC DDEEFF 1122");

            var oneUpdate = sha.HashMessage(bs);

            sha.Init();
            sha.Update(new BitString("112233").ToBytes(), 24);
            sha.Update(new BitString("445566").ToBytes(), 24);
            sha.Update(new BitString("778899").ToBytes(), 24);
            sha.Update(new BitString("AABBCC").ToBytes(), 24);
            sha.Update(new BitString("DDEEFF").ToBytes(), 24);
            sha.Update(new BitString("1122").ToBytes(), 16);
            var buffer = new byte[32];
            sha.Final(buffer, 256);

            Assert.AreEqual(oneUpdate.Digest.ToHex(), new BitString(buffer).ToHex());
        }

        [Test]
        public void WhenUsingPartialWordUpdateChunks_Should256HashIntoSameDigestAsSingleUpdate2()
        {
            var sha = new NativeFastSha2_256();

            var bs = new BitString("1122 33 4455667788");

            var oneUpdate = sha.HashMessage(bs);

            sha.Init();
            sha.Update(new BitString("1122").ToBytes(), 16);
            sha.Update(new BitString("33").ToBytes(), 8);
            sha.Update(new BitString("4455667788").ToBytes(), 40);

            var buffer = new byte[32];
            sha.Final(buffer, 256);

            Assert.AreEqual(oneUpdate.Digest.ToHex(), new BitString(buffer).ToHex());
        }

        [Test]
        public void WhenUsingPartialWordUpdateChunks_Should512HashIntoSameDigestAsSingleUpdate()
        {
            var sha = new NativeFastSha2_512();

            var bs = new BitString("112233 445566 778899 AABBCC DDEEFF 1122");

            var oneUpdate = sha.HashMessage(bs);

            sha.Init();
            sha.Update(new BitString("112233").ToBytes(), 24);
            sha.Update(new BitString("445566").ToBytes(), 24);
            sha.Update(new BitString("778899").ToBytes(), 24);
            sha.Update(new BitString("AABBCC").ToBytes(), 24);
            sha.Update(new BitString("DDEEFF").ToBytes(), 24);
            sha.Update(new BitString("1122").ToBytes(), 16);
            var buffer = new byte[64];
            sha.Final(buffer, 512);

            Assert.AreEqual(oneUpdate.Digest.ToHex(), new BitString(buffer).ToHex());
        }

        [Test]
        public void WhenUsingPartialWordUpdateChunks_Should512HashIntoSameDigestAsSingleUpdate2()
        {
            var sha = new NativeFastSha2_512();

            var bs = new BitString("1122 33 4455667788");

            var oneUpdate = sha.HashMessage(bs);

            sha.Init();
            sha.Update(new BitString("1122").ToBytes(), 16);
            sha.Update(new BitString("33").ToBytes(), 8);
            sha.Update(new BitString("4455667788").ToBytes(), 40);

            var buffer = new byte[64];
            sha.Final(buffer, 512);

            Assert.AreEqual(oneUpdate.Digest.ToHex(), new BitString(buffer).ToHex());
        }
    }
}
