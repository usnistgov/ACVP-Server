using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.CMAC.Enums;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests.KC
{
    [TestFixture, UnitTest, FastIntegrationTest]
    public class KeyConfirmationCmacTests
    {
        private KeyConfirmationCmac _subject;
        private readonly CmacFactory _cmacFactory = new CmacFactory();

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
                // serverPublicKey
                new BitString("5ad3aa155207e04b347dae7d8b6c746a8438c66791594ec8b1614b17aa9eb85eea4021f35f09d1bb2771594996e351e9bec6f0a95bcc64ac8460642945358e417e163f10b01d410b20421de42b7060695becf00ca39cc45fc6fbd0186894fa94ad40897143eec3ae93188acf8ee92b85479569274fba6dce9d2396e38b8811f79846a98939a53da980add4b7f85784dcb0c92913b4d34de12a5668e503e8ba56487c29d36970073e49717c813b1b117a5980bf5f8adc1d53e5746271767f864dd75dfe9cecc140226bb8c20702e4308356f14d4c4a74d4c8e98c4ab4dd4cf602afe10fb2056b04fcdce38e632c0e4004d878c529c115df1ad14050d7a98342e9"),
                // iutPublicKey
                new BitString("4dffd3e0be20ed9bb8b4b7aa8972e5bae0cd42f5cd7d6613ad8c2391eb4dc0ce7543456e2cb3afc7b7c905f6bf9f8b6d5b3a7d25a336bdbcd97d53333efa2334b7c0c73efed5ccd2049e555511202ec96b65c106dd95e46e33c2d1179d47a1713c143020882e26783366fdcdb968e3c8bce46360d9d5667df92ae76b701dd896c98bea5afd3dc81c9e374775ce7cf0aa369f92cce2cd82e403326a02f60308f0710eec38ddd55c5c3b1133b658ed5b95eada0447f0748092ed63545e33047fc2b61d75515f7af5ccd3bb82db850dcc82c9286143ba95ff37e6a2c445b406638eaf80dd54c1863d8ef82fdc2a6dbff2fd38ceb10459229b90d64e35adf5fc1f87"),
                // derivedKeyingMaterial
                new BitString("8dc40d2587dde12abb4e05fb0fa01108"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e54341565369644dffd3e0be20ed9bb8b4b7aa8972e5bae0cd42f5cd7d6613ad8c2391eb4dc0ce7543456e2cb3afc7b7c905f6bf9f8b6d5b3a7d25a336bdbcd97d53333efa2334b7c0c73efed5ccd2049e555511202ec96b65c106dd95e46e33c2d1179d47a1713c143020882e26783366fdcdb968e3c8bce46360d9d5667df92ae76b701dd896c98bea5afd3dc81c9e374775ce7cf0aa369f92cce2cd82e403326a02f60308f0710eec38ddd55c5c3b1133b658ed5b95eada0447f0748092ed63545e33047fc2b61d75515f7af5ccd3bb82db850dcc82c9286143ba95ff37e6a2c445b406638eaf80dd54c1863d8ef82fdc2a6dbff2fd38ceb10459229b90d64e35adf5fc1f875ad3aa155207e04b347dae7d8b6c746a8438c66791594ec8b1614b17aa9eb85eea4021f35f09d1bb2771594996e351e9bec6f0a95bcc64ac8460642945358e417e163f10b01d410b20421de42b7060695becf00ca39cc45fc6fbd0186894fa94ad40897143eec3ae93188acf8ee92b85479569274fba6dce9d2396e38b8811f79846a98939a53da980add4b7f85784dcb0c92913b4d34de12a5668e503e8ba56487c29d36970073e49717c813b1b117a5980bf5f8adc1d53e5746271767f864dd75dfe9cecc140226bb8c20702e4308356f14d4c4a74d4c8e98c4ab4dd4cf602afe10fb2056b04fcdce38e632c0e4004d878c529c115df1ad14050d7a98342e9"),
                // expectedTag
                new BitString("47d3892afc9074b2039c7280828fae84")
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
                // serverPublicKey
                new BitString("b5850df11ada1fbae1a4111190ae74b530a1dca62f6e1d42e2ae5baabaeea5090ae1b45ddc650248109d1719c4f7859721fd1e5092faf8df3c2465e69217925bedecfb8f7f00187a9ace4372041482f7403870afa64934f731149e88977c53fe9d06db34ec3ebee08782263a20b1b240a8fb01d7e5e9a930c22e96a489c2c14f478831df4c39fdc627559c55da60a6eec7df8d25738767ce64826c7b1f2312ee1783c3ea70543d84717110012cfd7f5b993a230919be010c171ace8c1ce158fad2ebe7ebea8611b9d7660e31f505b0e1aa7696f0ea92038d417c2fa05dea082f384db13101748fa8fecdfe71f4b819035145dd3867f33c38e8078306cfa8a6a3"),
                // iutPublicKey
                new BitString("2ac2dd3f9d924ecbaffe685f0bc5987b485399d5c996ffc18c960074cdb6f016c1889d25b2cfad79192c57b7e22e1e9bdf4e0ab9b84fe2945b57b74b5f41e75f98417cc717930e355f51bfb761e05f38f01b61c046de52440d3a8d8818d9cf46039e577b744916922dabbd393d91dc85dd46722db7bf744d3b7fbfb2d55d9cba3a3b366ad712dde0d0a50eba2b736791b327b3a2ac11887cdb274e7ae42f5951a59af28a115e761d9141e685216f8d5fdb2ddcf45b7e88ae6f2e14585f22a5908e5b6c7bc8f78872be222e32b87d7cacc03fb801202c29703284639c32a6dde89f37d6549c97e2c5f8b73d5c36f84c4af45740734c910c7d0972441e87eb45e8"),
                // derivedKeyingMaterial
                new BitString("bf109270e4f8edde6493210aa04349dadb185a296e567698"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e54341565369642ac2dd3f9d924ecbaffe685f0bc5987b485399d5c996ffc18c960074cdb6f016c1889d25b2cfad79192c57b7e22e1e9bdf4e0ab9b84fe2945b57b74b5f41e75f98417cc717930e355f51bfb761e05f38f01b61c046de52440d3a8d8818d9cf46039e577b744916922dabbd393d91dc85dd46722db7bf744d3b7fbfb2d55d9cba3a3b366ad712dde0d0a50eba2b736791b327b3a2ac11887cdb274e7ae42f5951a59af28a115e761d9141e685216f8d5fdb2ddcf45b7e88ae6f2e14585f22a5908e5b6c7bc8f78872be222e32b87d7cacc03fb801202c29703284639c32a6dde89f37d6549c97e2c5f8b73d5c36f84c4af45740734c910c7d0972441e87eb45e8b5850df11ada1fbae1a4111190ae74b530a1dca62f6e1d42e2ae5baabaeea5090ae1b45ddc650248109d1719c4f7859721fd1e5092faf8df3c2465e69217925bedecfb8f7f00187a9ace4372041482f7403870afa64934f731149e88977c53fe9d06db34ec3ebee08782263a20b1b240a8fb01d7e5e9a930c22e96a489c2c14f478831df4c39fdc627559c55da60a6eec7df8d25738767ce64826c7b1f2312ee1783c3ea70543d84717110012cfd7f5b993a230919be010c171ace8c1ce158fad2ebe7ebea8611b9d7660e31f505b0e1aa7696f0ea92038d417c2fa05dea082f384db13101748fa8fecdfe71f4b819035145dd3867f33c38e8078306cfa8a6a3"),
                // expectedTag
                new BitString("c4168d29215307f69ea0bf15d652cab0")
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
                // serverPublicKey
                new BitString("3a9b3dbd65a4ce6412b48d6e6694da9655921f9197f2568b14881797595c96d4b7b907557a425239d69dd00141ed271ac66315e936e6a7675d87f72117d9019e9d436e79f90cfde9cbb699efa6674b98aab041b64a0d2d25e7d4d93f890983cea09a407807c52ac56d468c948ddd6ea3944c7f81e98638e41bd5fee91ea2d2abed58ff9c8f2f226c0d119564d001d894b4f4d54d0ca69021999b832ad7e14d66b7418043f469269fef0c43b2de256b9380588b8e1733c38614fd8d6f1a2138068239cdc24983b4a499ec5d1b598164a39dec3ab69f4a1e7afff67e29aa64853343e7666c6d1e976b8d9b58cd9d73d94f7820e12bb4650333f7cd139f0a3ec224"),
                // iutPublicKey
                new BitString("1a4b7200c86b5269f99f93d0a1e326474b65b724a87340e14a0d0519824c7497d22ae33101f46aaa71b72ad80706db15aefb49284de2fc63acf68ffc326e390b8220c1b7ce8408146f367909641785627ae282cab7aad55505c79d6eae3a837a70f496f3e343f0199adc7b522e9364eff95c557b5131ec4a4ea5e518a0d0c6613eff34c6694c4a982dc2f28ccf98c6aec7f9790d7b6995ab0e0622cbdd9ad6f010e0977d579fca02b5ed701b1b747635875424eced01aaae79a35803226ae4e9edc9bdac006041f96af9a97785568de69919060ffabc6eede9873ee81b673bf50688062bf48a0b09de339bf3f547eb38b24ab4c847cfa5635e73a0b54357a9eb"),
                // derivedKeyingMaterial
                new BitString("07c966e01f4874764f861fbcf57218fb06561dff4040f77bb78b3c6d58dca51d"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e54341565369641a4b7200c86b5269f99f93d0a1e326474b65b724a87340e14a0d0519824c7497d22ae33101f46aaa71b72ad80706db15aefb49284de2fc63acf68ffc326e390b8220c1b7ce8408146f367909641785627ae282cab7aad55505c79d6eae3a837a70f496f3e343f0199adc7b522e9364eff95c557b5131ec4a4ea5e518a0d0c6613eff34c6694c4a982dc2f28ccf98c6aec7f9790d7b6995ab0e0622cbdd9ad6f010e0977d579fca02b5ed701b1b747635875424eced01aaae79a35803226ae4e9edc9bdac006041f96af9a97785568de69919060ffabc6eede9873ee81b673bf50688062bf48a0b09de339bf3f547eb38b24ab4c847cfa5635e73a0b54357a9eb3a9b3dbd65a4ce6412b48d6e6694da9655921f9197f2568b14881797595c96d4b7b907557a425239d69dd00141ed271ac66315e936e6a7675d87f72117d9019e9d436e79f90cfde9cbb699efa6674b98aab041b64a0d2d25e7d4d93f890983cea09a407807c52ac56d468c948ddd6ea3944c7f81e98638e41bd5fee91ea2d2abed58ff9c8f2f226c0d119564d001d894b4f4d54d0ca69021999b832ad7e14d66b7418043f469269fef0c43b2de256b9380588b8e1733c38614fd8d6f1a2138068239cdc24983b4a499ec5d1b598164a39dec3ab69f4a1e7afff67e29aa64853343e7666c6d1e976b8d9b58cd9d73d94f7820e12bb4650333f7cd139f0a3ec224"),
                // expectedTag
                new BitString("09622a920c8f285f74b7a9b76ccd69ba")
            }
        };

        [Test]
        [TestCaseSource(nameof(_tests))]
        public void ShouldReturnCorrectMac(
            int keySize, int tagLength,
            BitString serverId, BitString iutId,
            BitString serverPublicKey, BitString iutPublicKey,
            BitString derivedKeyingMaterial,
            BitString expectedMacData, BitString expectedTag)
        {
            var cmac = _cmacFactory.GetCmacInstance(CmacTypes.AES128); // doesn't matter for scope of testing this, as long as AES

            var p = new KeyConfirmationParameters(
                KeyAgreementRole.InitiatorPartyU,
                KeyConfirmationRole.Provider,
                KeyConfirmationDirection.Bilateral,
                KeyAgreementMacType.CmacAes, // note this doesn't matter for the scope of this test
                keySize,
                tagLength,
                iutId,
                serverId,
                iutPublicKey,
                serverPublicKey,
                derivedKeyingMaterial
            );

            _subject = new KeyConfirmationCmac(cmac, p);

            var result = _subject.ComputeMac();

            Assume.That(result.Success);
            Assert.AreEqual(expectedMacData.ToHex(), result.MacData.ToHex(), nameof(expectedMacData));
            Assert.AreEqual(expectedTag.ToHex(), result.Mac.ToHex(), nameof(expectedTag));
        }
    }
}