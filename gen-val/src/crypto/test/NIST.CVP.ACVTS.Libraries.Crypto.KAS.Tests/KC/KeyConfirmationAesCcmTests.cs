using NIST.CVP.ACVTS.Libraries.Crypto.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.AES_CCM;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.Aead;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests.KC
{
    [TestFixture, FastCryptoTest]
    public class KeyConfirmationAesCcmTests
    {
        private KeyConfirmationAesCcm _subject;

        private static object[] _tests = new object[]
        {
            new object[]
            {
                // keySize
                128,
                // tagLength
                128,
                // serverId
                new BitString("434156536964"),
                // iutId
                new BitString("a1b2c3d4e5"),
                // nonce
                new BitString("fe92de5495d53efa905c7dfb90"), 
                // serverPublicKey
                new BitString("735f50f6044dd279fee52ccb31e8c363ff14535af81acd63c9cc675e1d4382261f2c70466e2697ade096c15d8bbda455458493788e7b784b45f6ffe5b54fda13d39f4f117fc9374dc9b0dd2ac7cf5d0b4d952a8f5d07e8f77481ece22e34a77dadcbcb896f8cdb259de5f0859d2c3f4cb716cfe89838ccca9a0208adec5b80d7b01696264ca565fef997f15c72aa1c60124f02d243c62d84921a24ada5b066964a3d8aa0a10ce53ead02ebcaa75e14bfbeff9a7cafc9505487bd9c27cc41f9dc04261fc29983e02bd0a99dc6b9a4d46fcb71422a16ebcd54975a55adf4a320da6faccd8a719ba951b143bd09f0cc6406b036b971418c3be96128c55ca13964e3"),
                // iutPublicKey
                new BitString("5ba9beae28a33ef1f63f3395dddc60ea48e9c86c633f13a34f6eea75a7fc17c8218121b47bfbb724b6a168786b291216f2c9c4d906cdd33d8237903fc9ab843ed51d1ea679432535db1a1d4ba4b5dd573c82538682c966886c0a35c1803d3691a15fdc045686088a24278af3911750c6e88f708fc605992d13203b2af4729f6568ea68d82fbb491b53287013fbef40e9bb13c0c025eb3b418c43f5025b0875b83d574f249dab2636868460f17c16a3f50c9f3bc7253500c529f3f95392b7cd990ce04a4e6ddfe9a63b3011a51839e0d049833c617a558867eebc918678bcd6d667c6d212912d2ed2353a100b8954c328cce020bff7187f7a0faa37805f600796"),
                // derivedKeyingMaterial
                new BitString("0cf962452307c27f76a16a91c3c33e46"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e54341565369645ba9beae28a33ef1f63f3395dddc60ea48e9c86c633f13a34f6eea75a7fc17c8218121b47bfbb724b6a168786b291216f2c9c4d906cdd33d8237903fc9ab843ed51d1ea679432535db1a1d4ba4b5dd573c82538682c966886c0a35c1803d3691a15fdc045686088a24278af3911750c6e88f708fc605992d13203b2af4729f6568ea68d82fbb491b53287013fbef40e9bb13c0c025eb3b418c43f5025b0875b83d574f249dab2636868460f17c16a3f50c9f3bc7253500c529f3f95392b7cd990ce04a4e6ddfe9a63b3011a51839e0d049833c617a558867eebc918678bcd6d667c6d212912d2ed2353a100b8954c328cce020bff7187f7a0faa37805f600796735f50f6044dd279fee52ccb31e8c363ff14535af81acd63c9cc675e1d4382261f2c70466e2697ade096c15d8bbda455458493788e7b784b45f6ffe5b54fda13d39f4f117fc9374dc9b0dd2ac7cf5d0b4d952a8f5d07e8f77481ece22e34a77dadcbcb896f8cdb259de5f0859d2c3f4cb716cfe89838ccca9a0208adec5b80d7b01696264ca565fef997f15c72aa1c60124f02d243c62d84921a24ada5b066964a3d8aa0a10ce53ead02ebcaa75e14bfbeff9a7cafc9505487bd9c27cc41f9dc04261fc29983e02bd0a99dc6b9a4d46fcb71422a16ebcd54975a55adf4a320da6faccd8a719ba951b143bd09f0cc6406b036b971418c3be96128c55ca13964e3"),
                // expectedTag
                new BitString("0239860fe82fe1723e17c00562d95876")
            },
            new object[]
            {
                // keySize
                192,
                // tagLength
                128,
                // serverId
                new BitString("434156536964"),
                // iutId
                new BitString("a1b2c3d4e5"),
                // nonce
                new BitString("81f1bdb3b5339f562ee18415e2"), 
                // serverPublicKey
                new BitString("e2d8056326135bbff4d8170ddb2e2bb386d18cb7b337539678b92080cf928dcf157c136266003eefbe93451afb55989b8d8b8b927ba65a19970bab6f38cf15ef57afa84aa3f1aa2c3609693861141b63b1c77c088396fef04363403978eb64e50ed129348542a8051962a4c345ed75921881e322a6f7d93dc3688b62344c38a1459876f55bc94fff0d919ddce3f62a92ac5f2a86730a941c4597cd2b1a0a8a26b29e4bd4c63e740f417be468b4f7ab0706a3db82b490619af53bdaf7182e6db2cc4fb5c09e6a5e26b637b1b4028cc23ccdd693bdc70ef6987833309156876b5bd14edafcafed33fda40fb5af4b967551ab0b67f0d188e3a13e704dc81142bd6d"),
                // iutPublicKey
                new BitString("754cbcb5102b4ea1c43c80c597788ccbbe1471dcee5f36c24b3e6357d85e9577bf7041a8e99a758364cf4f496a9bcfccee98431e9c46fd044b95e4c167d4cf1e76d4f2f70d30fac43e8562bf60355742bd88421768eaae6dcd4defc5a21973ccef1dda5a6622c675dce57450d1106568243b142fbd71e9298ed7067d9a6914c61f3eccfd3b8b55938371a9f1e16a5d0edf7ae90ce71591aeb5783f6ea3cb68c451ba274e4407e78b355a9c44e36e6d28ae4a914dae952c663c47f71d0646fa21aebd0692789108b98b598e83178543caf33878a611b34c8424d497caf0d238da87affff37f69d01346e23af3ccc441f05819526871ec856d739fec1697ecd2f4"),
                // derivedKeyingMaterial
                new BitString("e59516a95058b00207434fd25c1838c2f0153babefd3c945"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e5434156536964754cbcb5102b4ea1c43c80c597788ccbbe1471dcee5f36c24b3e6357d85e9577bf7041a8e99a758364cf4f496a9bcfccee98431e9c46fd044b95e4c167d4cf1e76d4f2f70d30fac43e8562bf60355742bd88421768eaae6dcd4defc5a21973ccef1dda5a6622c675dce57450d1106568243b142fbd71e9298ed7067d9a6914c61f3eccfd3b8b55938371a9f1e16a5d0edf7ae90ce71591aeb5783f6ea3cb68c451ba274e4407e78b355a9c44e36e6d28ae4a914dae952c663c47f71d0646fa21aebd0692789108b98b598e83178543caf33878a611b34c8424d497caf0d238da87affff37f69d01346e23af3ccc441f05819526871ec856d739fec1697ecd2f4e2d8056326135bbff4d8170ddb2e2bb386d18cb7b337539678b92080cf928dcf157c136266003eefbe93451afb55989b8d8b8b927ba65a19970bab6f38cf15ef57afa84aa3f1aa2c3609693861141b63b1c77c088396fef04363403978eb64e50ed129348542a8051962a4c345ed75921881e322a6f7d93dc3688b62344c38a1459876f55bc94fff0d919ddce3f62a92ac5f2a86730a941c4597cd2b1a0a8a26b29e4bd4c63e740f417be468b4f7ab0706a3db82b490619af53bdaf7182e6db2cc4fb5c09e6a5e26b637b1b4028cc23ccdd693bdc70ef6987833309156876b5bd14edafcafed33fda40fb5af4b967551ab0b67f0d188e3a13e704dc81142bd6d"),
                // expectedTag
                new BitString("9a6eada7f37edefc20bbf8b9d57b5df5")
            },
            new object[]
            {
                // keySize
                256,
                // tagLength
                128,
                // serverId
                new BitString("434156536964"),
                // iutId
                new BitString("a1b2c3d4e5"),
                // nonce
                new BitString("39ed699fad69afdb1135a432a7"), 
                // serverPublicKey
                new BitString("61e791284db0d748953c015e8954783ccea9b67669c953513ad43512bfe5f2a3057f2b13d2c25d30df6aaa168f38de0b59aba5e37144b46b3dba0bf6a03896abee4cd55c92c906e1c90b19da9a9ecb0d5e6fcb189891bf7163533b1212d5f5d8cbd3c4532371bdfce2dd70d838f778c191199365ebfad9787178c851d3be05e95f4671cb8fac6b87d24a6487768c9155809f6b8b053f5820d7675860282e2c6460457dea9365d6ca783472f58fdd816c9e26a96a95116bed03c5cb0f4dd74856ae42e41cee6762695ec3cc10290248b558a4702b1f87f33472749038d6595c6b3c71be04c78fb64bf76484eb9849e5ff2fef6d11abe54cba0318fff91bdc9891"),
                // iutPublicKey
                new BitString("93d9ec5c0e0af85106e17108073af8bc78710ee57177a9101422317f2acab20f8b9ed811d41f700641ed171282196a983aec2a426242dcbd68d41f3c34e59afbe54c6d0aadce3e30cbb27fb01277bc4aac734523b379b0e34c03e24fd783d85501d336c0a4e758bdaabb5639b115f8d8e328cc61288f2fb553370fdbd60f6e03010b740175c2e463feac257c0de96d421bcbbf943136024def1dc4524d27758eda2be322f6e05e5681adc4973e82fdf535ba2a31e4260730467c05aae1ad24eae38bc5ccfb78ee2eea92760bfa03e8a1393b19561b2ed526d73103dbd5e2eda6e6494841bf4311182113487e1c0b7ee2ff47a945844b4850fb2add6cccbeaae0"),
                // derivedKeyingMaterial
                new BitString("6caa18e3cbe837c449114e0e463214640422231e5208b67207528d548f4b1d7d"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e543415653696493d9ec5c0e0af85106e17108073af8bc78710ee57177a9101422317f2acab20f8b9ed811d41f700641ed171282196a983aec2a426242dcbd68d41f3c34e59afbe54c6d0aadce3e30cbb27fb01277bc4aac734523b379b0e34c03e24fd783d85501d336c0a4e758bdaabb5639b115f8d8e328cc61288f2fb553370fdbd60f6e03010b740175c2e463feac257c0de96d421bcbbf943136024def1dc4524d27758eda2be322f6e05e5681adc4973e82fdf535ba2a31e4260730467c05aae1ad24eae38bc5ccfb78ee2eea92760bfa03e8a1393b19561b2ed526d73103dbd5e2eda6e6494841bf4311182113487e1c0b7ee2ff47a945844b4850fb2add6cccbeaae061e791284db0d748953c015e8954783ccea9b67669c953513ad43512bfe5f2a3057f2b13d2c25d30df6aaa168f38de0b59aba5e37144b46b3dba0bf6a03896abee4cd55c92c906e1c90b19da9a9ecb0d5e6fcb189891bf7163533b1212d5f5d8cbd3c4532371bdfce2dd70d838f778c191199365ebfad9787178c851d3be05e95f4671cb8fac6b87d24a6487768c9155809f6b8b053f5820d7675860282e2c6460457dea9365d6ca783472f58fdd816c9e26a96a95116bed03c5cb0f4dd74856ae42e41cee6762695ec3cc10290248b558a4702b1f87f33472749038d6595c6b3c71be04c78fb64bf76484eb9849e5ff2fef6d11abe54cba0318fff91bdc9891"),
                // expectedTag
                new BitString("5ff8e6147a11d82357b6d113f4e11fc1")
            }
        };

        [Test]
        [TestCaseSource(nameof(_tests))]
        public void ShouldReturnCorrectMac(
            int keySize, int tagLength,
            BitString serverId, BitString iutId,
            BitString nonce,
            BitString serverPublicKey, BitString iutPublicKey,
            BitString derivedKeyingMaterial,
            BitString expectedMacData, BitString expectedTag)
        {
            var ccm = new CcmBlockCipher(new AesEngine(), new ModeBlockCipherFactory(), new AES_CCMInternals());

            var p = new KeyConfirmationParameters(
                KeyAgreementRole.InitiatorPartyU,
                KeyConfirmationRole.Provider,
                KeyConfirmationDirection.Bilateral,
                KeyAgreementMacType.AesCcm, // note this doesn't matter for the scope of this test
                keySize,
                tagLength,
                iutId,
                serverId,
                iutPublicKey,
                serverPublicKey,
                derivedKeyingMaterial,
                nonce
            );

            _subject = new KeyConfirmationAesCcm(new KeyConfirmationMacDataCreator(), p, ccm);

            var result = _subject.ComputeMac();

            Assert.That(result.Success);
            Assert.That(result.MacData.ToHex(), Is.EqualTo(expectedMacData.ToHex()), nameof(expectedMacData));
            Assert.That(result.Mac.ToHex(), Is.EqualTo(expectedTag.ToHex()), nameof(expectedTag));
        }
    }
}
