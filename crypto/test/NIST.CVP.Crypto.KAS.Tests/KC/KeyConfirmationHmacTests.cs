using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests.KC
{
    [TestFixture, UnitTest, FastIntegrationTest]
    public class KeyConfirmationHmacTests
    {
        private KeyConfirmationHmac _subject;
        private readonly HmacFactory _hmacFactory = new HmacFactory(new ShaFactory());

        private static object[] _tests = new object[]
        {
            new object[]
            {
                // ModeValues
                ModeValues.SHA2,
                // DigestSizes
                DigestSizes.d224,
                // keySize
                112,
                // tagLength
                64,
                // serverId
                new BitString("434156536964"),
                // iutId
                new BitString("a1b2c3d4e5"),
                // serverPublicKey
                new BitString("83ddd87d2b60ee92cb3490984c170ccaac190855d433cd9244ea0128b584472be3246e4ea28fb9b814d9ec2906b52dc553529d26954f70f41eec32170d439b0aa119a327a1572ee9c08f503f344159f12cbe4f26e0d8ef52bb5217e7451b7c823ab6cab07e9bcbc2340369dcced6aaaff0241f19ec85cf158722f3129e417a5148304a5a8f98f85446cac4c9800ffe5811947d0ee849de502f21df65814a781006a123752cf631fe16c899803e7640fee967c1458462e593b45028660faf5e781a4282fc4c1d9e9deebe21b14e932f5f29a10ae82229cb21a287ad73a5cb3f21f88f1f8eb35455a8dc47d91436980c308f4c840e10a628ccce3b92270199b3c0"),
                // iutPublicKey
                new BitString("1d2dc46bfd84b299eef8aa7c532e115c25fc7514233c99641abf48173a89124ced0a140fe1830e7b4ed22b548927544fe15c637990ffc3591df54855b6722e096cca18622c32edf777c193fdd8b0ddc10d8056f8650d78abf55284819197c65cdf0d46cf95e8b8ccfb43701f7e92e701116527e7c6bf2feca53372fb9dbef87b7647514a843f58cb5519ffe502226076e686a3126675d02497b82f6f3be0a091f5fc4985ddcb05f6afda71c8dbe2a9cb77a85f02ea1a561fb9a23a9ef5045297d5443ea94f81eb8003b2ce6ec7819a21bf708bbb8aeea8130bbcb5790dbec52c9227cb7c23e0fa7af38f7eca2af356423b441c564bdc9fcff5c7aa70429c861b"),
                // derivedKeyingMaterial
                new BitString("e0b90f7d396f0032b9416625de4e"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e54341565369641d2dc46bfd84b299eef8aa7c532e115c25fc7514233c99641abf48173a89124ced0a140fe1830e7b4ed22b548927544fe15c637990ffc3591df54855b6722e096cca18622c32edf777c193fdd8b0ddc10d8056f8650d78abf55284819197c65cdf0d46cf95e8b8ccfb43701f7e92e701116527e7c6bf2feca53372fb9dbef87b7647514a843f58cb5519ffe502226076e686a3126675d02497b82f6f3be0a091f5fc4985ddcb05f6afda71c8dbe2a9cb77a85f02ea1a561fb9a23a9ef5045297d5443ea94f81eb8003b2ce6ec7819a21bf708bbb8aeea8130bbcb5790dbec52c9227cb7c23e0fa7af38f7eca2af356423b441c564bdc9fcff5c7aa70429c861b83ddd87d2b60ee92cb3490984c170ccaac190855d433cd9244ea0128b584472be3246e4ea28fb9b814d9ec2906b52dc553529d26954f70f41eec32170d439b0aa119a327a1572ee9c08f503f344159f12cbe4f26e0d8ef52bb5217e7451b7c823ab6cab07e9bcbc2340369dcced6aaaff0241f19ec85cf158722f3129e417a5148304a5a8f98f85446cac4c9800ffe5811947d0ee849de502f21df65814a781006a123752cf631fe16c899803e7640fee967c1458462e593b45028660faf5e781a4282fc4c1d9e9deebe21b14e932f5f29a10ae82229cb21a287ad73a5cb3f21f88f1f8eb35455a8dc47d91436980c308f4c840e10a628ccce3b92270199b3c0"),
                // expectedTag
                new BitString("ceaedd23dd158725")
            },
            new object[]
            {
                // ModeValues
                ModeValues.SHA2,
                // DigestSizes
                DigestSizes.d256,
                // keySize
                112,
                // tagLength
                64,
                // serverId
                new BitString("434156536964"),
                // iutId
                new BitString("a1b2c3d4e5"),
                // serverPublicKey
                new BitString("187231105236b90b67401051dd416b78123eb6fce2363fa225c97734c95b06954570e860408bc7c3489de687ce620f81027ee955afe6123be919afe02f1aa9d71fcb2ebbb2c6f8f4676ae1676a460642f0d85190a4b309d39a353901165fc37682c7e5a8f55df1b9b7e7dbcb0d894ac1e2e0b89411827be260ec03c0a9a451c86f0ee60ab1b18153539adac4cad269c9efd41ce1c32bb76e68ce4d7702b26a12ad497554ca989886c3bc8f87c9beb1a14daae896386f21d796ae3c52c1d231fbcdb6f5ae78f564ee5b782eee004e2ffb69efb6b983a8ed3b03cec3f0f118ec28017e794203235effd5c27a4f22ed611cd47afcf1b0c1f02959ef400f5523ad25"),
                // iutPublicKey
                new BitString("1e2421ba67346bed37c0c271d29560db9c5f8399f94a8ccd4749c07d04835bfbdea1ae401df62cd10762b534460124e991c57d2e05254c34ef1197bfbdcd8d4cdeff5abde6e0606eef44ca57a8c7e60497e4b01c2e7681eda6fc66c58c22cf45e39d6881ae03189db2985aa3cc7961cd1cef5420b295a7bf394d7fba4c6747b063fb96435f176ff64c5e5a9c939ba03c280328367c3addd9340ba9c98926eae527be6269697bee01c0b8bc121ec0eeadb8212cdae78db862ad7dc8547cc922665477fd88becb013ee1e0bdac6ea429f7869603a3fbece8b5b9eaa1702c7deb6d96a3665b31381e60af26cb6e83eca02579d964743fdc8a29cb1b5b61af7b1609"),
                // derivedKeyingMaterial
                new BitString("01ed4c447ac4177cae1ea929fce2"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e54341565369641e2421ba67346bed37c0c271d29560db9c5f8399f94a8ccd4749c07d04835bfbdea1ae401df62cd10762b534460124e991c57d2e05254c34ef1197bfbdcd8d4cdeff5abde6e0606eef44ca57a8c7e60497e4b01c2e7681eda6fc66c58c22cf45e39d6881ae03189db2985aa3cc7961cd1cef5420b295a7bf394d7fba4c6747b063fb96435f176ff64c5e5a9c939ba03c280328367c3addd9340ba9c98926eae527be6269697bee01c0b8bc121ec0eeadb8212cdae78db862ad7dc8547cc922665477fd88becb013ee1e0bdac6ea429f7869603a3fbece8b5b9eaa1702c7deb6d96a3665b31381e60af26cb6e83eca02579d964743fdc8a29cb1b5b61af7b1609187231105236b90b67401051dd416b78123eb6fce2363fa225c97734c95b06954570e860408bc7c3489de687ce620f81027ee955afe6123be919afe02f1aa9d71fcb2ebbb2c6f8f4676ae1676a460642f0d85190a4b309d39a353901165fc37682c7e5a8f55df1b9b7e7dbcb0d894ac1e2e0b89411827be260ec03c0a9a451c86f0ee60ab1b18153539adac4cad269c9efd41ce1c32bb76e68ce4d7702b26a12ad497554ca989886c3bc8f87c9beb1a14daae896386f21d796ae3c52c1d231fbcdb6f5ae78f564ee5b782eee004e2ffb69efb6b983a8ed3b03cec3f0f118ec28017e794203235effd5c27a4f22ed611cd47afcf1b0c1f02959ef400f5523ad25"),
                // expectedTag
                new BitString("6ce4a176fe2f575a")
            },
            new object[]
            {
                // ModeValues
                ModeValues.SHA2,
                // DigestSizes
                DigestSizes.d384,
                // keySize
                112,
                // tagLength
                64,
                // serverId
                new BitString("434156536964"),
                // iutId
                new BitString("a1b2c3d4e5"),
                // serverPublicKey
                new BitString("29baf7e2e8ce9ec922c12b103b18260e7d592bffebc525795b3776e5e7dbb37cc3a75b3e296b3eedbb09ecaae25da98b5da2b04922d8925e52c82c4b11c51475086358267f54532826d3e6864855fb861c603706bbec648ad82a465603bfb91d2e7ca549f07b81f90dab131549e14d8c8983f118ef5fc2ff0fc4a3465f7573c9bae87335979d0016757d67882ae6ac6f8ac0dbe879c1e793164c65be0922ab238fc7a58b49d543a52a41ba8b1688178647742364a7aa3bcc0ad434f6092d855bb5c56144cf7683649160fb84f7c7e576576617fe95e6ce1c815485912dd30f4a54faccb06dd853892833b36be89e06e7e85de7a900dfd35d13d06cf4a0a1d11e"),
                // iutPublicKey
                new BitString("25290c8973a8ad54a9093c446b25f28b447457a542a1ca3004ff0af0e138472b326f841429f148b510ec4be23a4dc917c890bcd1db3ce94c20706382f1ae0400fa0c132c840687ecd349ad4362a38c3e59935a93b498056213e00e6a2ca18d49f42d00022d38f15c300d28c9b0694596e6fb66e5db3b988fd95836a69769e42937ecae3d28b0417c8732b1bd53539c05453dc9b284e214f8a7bb46f94c20bd6d0482c80bbe1f99bca455c1de6314c00ca0a98fdd89d193c92935b52401760a112e45ab3bf0575b74bece3e5dd15532bcc8438b93b5cc7c065a06e3c3c099edcc4e185c001ff1ba9dd268f9a814c95de4c5a636b353cb446a446b341e29c6a386"),
                // derivedKeyingMaterial
                new BitString("acfa86143737c7ac4167af5e0125"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e543415653696425290c8973a8ad54a9093c446b25f28b447457a542a1ca3004ff0af0e138472b326f841429f148b510ec4be23a4dc917c890bcd1db3ce94c20706382f1ae0400fa0c132c840687ecd349ad4362a38c3e59935a93b498056213e00e6a2ca18d49f42d00022d38f15c300d28c9b0694596e6fb66e5db3b988fd95836a69769e42937ecae3d28b0417c8732b1bd53539c05453dc9b284e214f8a7bb46f94c20bd6d0482c80bbe1f99bca455c1de6314c00ca0a98fdd89d193c92935b52401760a112e45ab3bf0575b74bece3e5dd15532bcc8438b93b5cc7c065a06e3c3c099edcc4e185c001ff1ba9dd268f9a814c95de4c5a636b353cb446a446b341e29c6a38629baf7e2e8ce9ec922c12b103b18260e7d592bffebc525795b3776e5e7dbb37cc3a75b3e296b3eedbb09ecaae25da98b5da2b04922d8925e52c82c4b11c51475086358267f54532826d3e6864855fb861c603706bbec648ad82a465603bfb91d2e7ca549f07b81f90dab131549e14d8c8983f118ef5fc2ff0fc4a3465f7573c9bae87335979d0016757d67882ae6ac6f8ac0dbe879c1e793164c65be0922ab238fc7a58b49d543a52a41ba8b1688178647742364a7aa3bcc0ad434f6092d855bb5c56144cf7683649160fb84f7c7e576576617fe95e6ce1c815485912dd30f4a54faccb06dd853892833b36be89e06e7e85de7a900dfd35d13d06cf4a0a1d11e"),
                // expectedTag
                new BitString("51c467043b6695f7")
            },
            new object[]
            {
                // ModeValues
                ModeValues.SHA2,
                // DigestSizes
                DigestSizes.d512,
                // keySize
                112,
                // tagLength
                64,
                // serverId
                new BitString("434156536964"),
                // iutId
                new BitString("a1b2c3d4e5"),
                // serverPublicKey
                new BitString("aef8cf75e29d30620e88e913cf6d9d4a09ddaa4e131e2954ba8c74498eff73a0b8c7577ab0044b08bbef96ab222e19638657fb6b07ef76aaf6c72ed2f8c17742d39ff9cf27fcf31c31d53309d9b4b602bf49e0eb5fa4d1f8bd195c905d06ac72375bf5712ee804c15cb4889798be826df5640c7f866bc70c01061b378de3e0adf02bf899bdd85fdeae7220496b631889fe2901d08cfd2992594589a442e7ca7f68b8dd940a54171a7fedc977d11858c8ac6357308b51ad5beb16223d83ba7635696bb737dd28d4c702873f0c4886e44c9ca1f136574833c85b2434ea9586f3edca75ba4bbc0a41392a4a095f430f702f3639a0f425f4c6cb117433289e1fd30c"),
                // iutPublicKey
                new BitString("ba44e71b224818391a9b854e4eca9aabacbbe57dd6943af7d42d100324b809b4892e7961711495eee681d6a20ca4917d8813b0e86b6bae04a3035deade2b2532cf66b8692cb93de91f425654900eb47c1c9d123f7a68e4fcf76bb30aeabb08ca77b64f45a9ab04c10901cca4a1787d36949449e2657a98501e2f5e264c5bd4c1fb825b122f5d56050d7a90999ae44dc8ea25a3bd97d5a7e7764ecbee827edc95d1e4e4863474c3191673574c2dfc097791afb7481951eccf2b48204fa94113e9199931e62d59755e43b1869440e49e1c81c0e27eb7c99492bdfe53b63a66eee9a6bbff8d69e4211b43525537af6294e5ad52bdca4392a1ea57a6edf08d392a3f"),
                // derivedKeyingMaterial
                new BitString("0f7e0a9047c1774324c591b6aa14"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e5434156536964ba44e71b224818391a9b854e4eca9aabacbbe57dd6943af7d42d100324b809b4892e7961711495eee681d6a20ca4917d8813b0e86b6bae04a3035deade2b2532cf66b8692cb93de91f425654900eb47c1c9d123f7a68e4fcf76bb30aeabb08ca77b64f45a9ab04c10901cca4a1787d36949449e2657a98501e2f5e264c5bd4c1fb825b122f5d56050d7a90999ae44dc8ea25a3bd97d5a7e7764ecbee827edc95d1e4e4863474c3191673574c2dfc097791afb7481951eccf2b48204fa94113e9199931e62d59755e43b1869440e49e1c81c0e27eb7c99492bdfe53b63a66eee9a6bbff8d69e4211b43525537af6294e5ad52bdca4392a1ea57a6edf08d392a3faef8cf75e29d30620e88e913cf6d9d4a09ddaa4e131e2954ba8c74498eff73a0b8c7577ab0044b08bbef96ab222e19638657fb6b07ef76aaf6c72ed2f8c17742d39ff9cf27fcf31c31d53309d9b4b602bf49e0eb5fa4d1f8bd195c905d06ac72375bf5712ee804c15cb4889798be826df5640c7f866bc70c01061b378de3e0adf02bf899bdd85fdeae7220496b631889fe2901d08cfd2992594589a442e7ca7f68b8dd940a54171a7fedc977d11858c8ac6357308b51ad5beb16223d83ba7635696bb737dd28d4c702873f0c4886e44c9ca1f136574833c85b2434ea9586f3edca75ba4bbc0a41392a4a095f430f702f3639a0f425f4c6cb117433289e1fd30c"),
                // expectedTag
                new BitString("c498933d03751361"),
            }
        };

        [Test]
        [TestCaseSource(nameof(_tests))]
        public void ShouldReturnCorrectMac(ModeValues modeValue, DigestSizes digestSize, 
            int keySize, int tagLength,
            BitString serverId, BitString iutId, 
            BitString serverPublicKey, BitString iutPublicKey,
            BitString derivedKeyingMaterial, 
            BitString expectedMacData, BitString expectedTag)
        {
            var hmac = _hmacFactory.GetHmacInstance(new HashFunction(modeValue, digestSize));

            var p = new KeyConfirmationParameters(
                KeyAgreementRole.UPartyInitiator,
                KeyConfirmationRole.Provider,
                KeyConfirmationDirection.Bilateral,
                KeyConfirmationMacType.AesCcm, // note this doesn't matter for the scope of this test
                keySize,
                tagLength,
                iutId,
                serverId,
                iutPublicKey,
                serverPublicKey,
                derivedKeyingMaterial,
                null
            );

            _subject = new KeyConfirmationHmac(hmac, p);

            var result = _subject.ComputeKeyMac();

            Assume.That(result.Success);
            Assert.AreEqual(expectedMacData.ToHex(), result.MacData.ToHex(), nameof(expectedMacData));
            Assert.AreEqual(expectedTag.ToHex(), result.Mac.ToHex(), nameof(expectedTag));
        }

    }
}
