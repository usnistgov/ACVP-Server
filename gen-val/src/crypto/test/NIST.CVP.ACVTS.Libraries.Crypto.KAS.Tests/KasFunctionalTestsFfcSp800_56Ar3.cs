using System.Collections.Generic;
using System.Numerics;
using Moq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.HKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.IKEv1;
using NIST.CVP.ACVTS.Libraries.Crypto.IKEv2;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF.OneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Sp800_56Ar3.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.TLS;
using NIST.CVP.ACVTS.Libraries.Math;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests
{
    /// <summary>
    /// Tests against the same test cases from SP800-56Ar1 in <see cref="KasFunctionalTestsFfc" />
    /// </summary>
    public class KasFunctionalTestsFfcSp800_56Ar3
    {
        private ISchemeBuilder _schemeBuilder;

        private ISecretKeyingMaterialBuilder _secretKeyingMaterialBuilderThisParty;
        private ISecretKeyingMaterialBuilder _secretKeyingMaterialBuilderOtherParty;

        private IKdfFactory _kdfFactory;
        private IKeyConfirmationFactory _keyConfirmationFactory;

        private MacParametersBuilder _macParamsBuilder = new MacParametersBuilder();
        private IKasBuilder _subject = new KasBuilder();

        [SetUp]
        public void Setup()
        {
            var shaFactory = new NativeShaFactory();
            var hmacFactory = new HmacFactory(shaFactory);

            var kdfVisitor = new KdfVisitor(
                new KdfOneStepFactory(shaFactory, new HmacFactory(shaFactory), new KmacFactory(new cSHAKEWrapper())),
                new Crypto.KDF.KdfFactory(new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()),
                    hmacFactory, new KmacFactory(new cSHAKEWrapper())), hmacFactory,
                new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()),
                new IkeV1Factory(hmacFactory, shaFactory),
                new IkeV2Factory(hmacFactory),
                new TlsKdfFactory(hmacFactory),
                new HkdfFactory(hmacFactory));
            _kdfFactory = new KdfFactory(kdfVisitor);

            var eccDh = new DiffieHellmanEcc();
            var eccMqv = new MqvEcc();
            var ffcDh = new DiffieHellmanFfc();
            var ffcMqv = new MqvFfc();

            _schemeBuilder = new SchemeBuilder(eccDh, ffcDh, eccMqv, ffcMqv);

            _secretKeyingMaterialBuilderThisParty = new SecretKeyingMaterialBuilder();
            _secretKeyingMaterialBuilderOtherParty = new SecretKeyingMaterialBuilder();

            _keyConfirmationFactory = new KeyConfirmationFactory(new KeyConfirmationMacDataCreator());
        }

        private static object[] _test_keyConfirmation = new object[]
        {
            #region dhHybrid1
            #region hmac
            new object[]
            {
                // label
                "dhHybrid1 hmac224 sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("cd9069c8f4497a33edd795125fcd941793406829d8d5a78a45c82382a0f44cfc8c694eabae503587b34e6c90e58cc10a1471f87d3fb3679070f49d504cc46b4df58e58cc5c8c53db8d2979b7cb57a265519d192c851293b6658c5b4c37114995e169b97ecbfae8a2b2033885d4e2aa1964c3edfbaff5c38179099ac5671f6a6e5c87d92671879f0a49e69bde1db49fe1b6209387aaa67458d02157b2f2c343ba17cf6e24e53e6c14ae9998b41e557bdb2ed4935e8f86b56a14565252bfdd12f1ad54d794e09ab3f49db890a80ae03951377218150cd4f53e65b94d50ab017f18b8c609c42cd7ecc2590e36ced770bcc370a38369d1832c33ac1a1910aa815739").ToPositiveBigInteger(),
                    // q
                    new BitString("b289a7b89fc29cc8e1337aa819e506c47dbb7372a2162b90983fca43").ToPositiveBigInteger(), 
                    // g
                    new BitString("b2947e9fd2ec7ed4af6d2c4057480110c7c7862441f3c19f39e1dd26eab8d6925a7ef2dfe4bb84388c0d1b0379fcde2d7c677e829abbd3fc7921d95165eeb01e75ef188ff687e6fd69b5638e6ad93cedbc6855be28c8fdd7e4fe85f5d61d6711fed1abd4702cf2efbc7c6e1608f2a3cc4fca635c3f33454553f42cc3706987d160059120e8ed72587b5ea36690aa9fafade666db05f23d095c760899539b7e333db6317e548f4ff7c1d7a111459e3681ffe05ba4fd3473f02e952626f5c6d499caf761ba4185a4b3a4141268128c645916ad5fa3c43fea4f391783dc5172742c1e07151f5202a707a4812b6883433f9a266c611fb083afdd84118d9f69f3012e").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhHybrid1,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D224,
                // Key confirmation role
                KeyConfirmationRole.Provider,
                // Key confirmation direction
                KeyConfirmationDirection.Bilateral,
                // key length
                112,
                // tag length
                64,
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("3f6b43bb5239c5ddfef5394ecf46db3753439328d598ba1ad907879d").ToPositiveBigInteger(),
                // public static this party
                new BitString("6c92ec906040391563b4b861963c5bb8174d7a60bc1eb6027dd77bf0f77427b3041cfa0af4dc8ea65a5c66d34fb237fb6db2a67a104d09f1babd7eb4e09d6e0394ad118c876f9789a99dea0c55ae4504827eb05c2f262da1b002e71194dce1cd3141b1f54f284233800259b9c6d0fb3ad92cf04e4d05219ee12138b837bfc789cdf360939f0417836c6b2e4e1c09027737d1166206ea16bdceb91bceb5017ab867ec862e38fee0ee42788a3bae1130fcfba54439254cd192bae28c36a70cf1d6fbd0a322d3ab6e3b86b3a352a6e29e7aa88d7a60aa0d6d34ef1b26af428b273b6fc10e4d8b29eb6f4dfb271c16b95d536c9a12a4d0f423946eec79eb46cd999a").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("89f1b69071e0fa857399ed1a3ea0a830803601c6dd93ffa3a5b014fa").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("54f4ad107e849716774b3910c6f9b920e5fdc2f69f94bed21adf5e87c4343a0832f78360c7e2febbe39e00778a8a781640ca53a12f3280763a8ab2c53fa1d1fbd1a6b8d5a3e5b2b1097af9aea3370240a846f97105fa90ce238e504e1dd17247fd8a9950bc54651eba7a19a320f0c4e8586d570f5d0b09141d36a42f03c2ba1e33d61ebe4ead935b10fce1098c54baa08f79a204fbc80beb2fcf10d37213bc90246ab59cc27406d15c63ec585280c749dbf33e2e79ead1acb035245c45788813f33a9eb1865942f208d7b30bfbdb6ee70febd0159a01a4e89882264615b5a6d84a7cdec5966a68f7751b8b544d41e4a59b5971574ffa620719cf18b135b29210").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("3a1854365855619c81b6bf33c16da93451a6b6ff356bca71c5c402f8").ToPositiveBigInteger(), 
                // public static other party
                new BitString("374af107d6a6be79a383a786458ecc7585ad613cb6ca219d3d3ea2137cba62f1157abc047cf2183dedd7b372ccd2f78a75ae89fd0c8368f7cfbd6a238f343636f859913daf254a7c626c5ce8f42b40597a3a202413d658a4cf1909554240c7dfe0bb9ae56ff3c22bd670a85be08928ef2e2a91beb8ae9122acc47562d116813533c31418bd4b7c128bb172774bfd3f8cf467d4f0bbdb3c6969d31a2d2265578b0d4d3af04fa423ac3d9ab6cba7dec988eeccbadcec5f86cc83d7ad7da7740e191cb986819221ac81434cbc28cd56ec508bf327bcd5fb05db21a6bc48b1821d4c06e635faf34ef619bd5446de7af484f2d5606883b5c3ffdddbd97699fe488bcd").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("18a2d714d92487ac795b9b4e2c867ddeb9c7da42e6dfcf85eeb239c1").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("07bf775ab92262b1a2e271795236b41712b830373954b2c74cb548a9b044c97b44788afebd34736ea8a21f580e859d26b615e7aa50d5faf856f3decb66998a790d5058af1f38885ed5e106b5cfc5c0c92525445b9d352069673c3b0dafb2a949b414332683f5fd30ec66f56f5b529ed68ac493d91578889b7d8136518edc1f38decd59ae72c4d32adc8237e2c2db4f905dd9a438a8c7680b07c7a5dcb5b2cbf28c0706ce92e6b1ab9ece33b13dd2c5640a4547b2ea1649def0711572b4a8f05fc3b32ca6b840965cce5c76bd94663513ceb768fbb20a2d1214f704552a63cf3ca7a947d3a506d7dbacffc20c77053c0abbe6f75287ff3aac3fe05b855217d817").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("61174babcd75988baffe79e92de5d5d535b85e26fa24c07db7a6e555d41a18ae3536fa2f58d178094d27c88204fba5a40a7565f9944fc7cec68b549580bfe08047bb85e20a6334ef3c7dbf64958b40ad7d8753d17419348aba820cb24749e529e30f360b9b6073b8d88608f58126a4d269d32001eac59b724d259f598e705d63c3a338e21ead16d7961faf00f0ecb1fe3ad765277e236d468d86ba4b92cf78b0f81d327303de8c72b82f8cd787db88b37d0b4deeeba0f7249b6222efae59c697297c977544e477dfb9aa00ec71a852c829aa288a1dd218cf38ca17a8910d9f408434ec7c8b6c7f75392a70ca2d60980b2ed535b7cd643eb4f59b6befbb7dad0e010d57826496e645b78bb4d2539f0ac091cfa2eadb3bf45be9c8ba12aab5ea8c4307bc35fd187643cc8903507f083efdb576945fceadaaeaeb4e2d655ad24eb21f4e821bf4231741264d861b8691a186195e41d1f92c3eaece42641293d85697f0ff3aca31684cae5b92d4eedbec0472ce71425821e862cf481967d80b6f4127c286f52deefc0acb6919d92d908d577ee8e3955e967e92748027b3af46f96e9445523a30893242a10c7a39156772a719fe2fcbcbebc1a98ae38c25888aa40cad6a01fcbb08564968a337b1883da8ba540745ec918e52edb799510b1253077a7a9ab037ad869e3e90fd626ecbf757521283a842bb7f120855014f8f52c08f3a30"),
                // expected oi
                new BitString("a1b2c3d4e543415653696486db011671fe2a20ff93f27057f1e7ab9b8350"),
                // expected dkm
                new BitString("53516f8f0a2c05ed378a59178cca"),
                // expected macData
                new BitString("4b435f325f55a1b2c3d4e543415653696454f4ad107e849716774b3910c6f9b920e5fdc2f69f94bed21adf5e87c4343a0832f78360c7e2febbe39e00778a8a781640ca53a12f3280763a8ab2c53fa1d1fbd1a6b8d5a3e5b2b1097af9aea3370240a846f97105fa90ce238e504e1dd17247fd8a9950bc54651eba7a19a320f0c4e8586d570f5d0b09141d36a42f03c2ba1e33d61ebe4ead935b10fce1098c54baa08f79a204fbc80beb2fcf10d37213bc90246ab59cc27406d15c63ec585280c749dbf33e2e79ead1acb035245c45788813f33a9eb1865942f208d7b30bfbdb6ee70febd0159a01a4e89882264615b5a6d84a7cdec5966a68f7751b8b544d41e4a59b5971574ffa620719cf18b135b2921007bf775ab92262b1a2e271795236b41712b830373954b2c74cb548a9b044c97b44788afebd34736ea8a21f580e859d26b615e7aa50d5faf856f3decb66998a790d5058af1f38885ed5e106b5cfc5c0c92525445b9d352069673c3b0dafb2a949b414332683f5fd30ec66f56f5b529ed68ac493d91578889b7d8136518edc1f38decd59ae72c4d32adc8237e2c2db4f905dd9a438a8c7680b07c7a5dcb5b2cbf28c0706ce92e6b1ab9ece33b13dd2c5640a4547b2ea1649def0711572b4a8f05fc3b32ca6b840965cce5c76bd94663513ceb768fbb20a2d1214f704552a63cf3ca7a947d3a506d7dbacffc20c77053c0abbe6f75287ff3aac3fe05b855217d817"),
                // expected tag
                new BitString("f9f49d07d00040cb")
            },
            new object[]
            {
                // label
                "dhHybrid1 hmac224 sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("cd9069c8f4497a33edd795125fcd941793406829d8d5a78a45c82382a0f44cfc8c694eabae503587b34e6c90e58cc10a1471f87d3fb3679070f49d504cc46b4df58e58cc5c8c53db8d2979b7cb57a265519d192c851293b6658c5b4c37114995e169b97ecbfae8a2b2033885d4e2aa1964c3edfbaff5c38179099ac5671f6a6e5c87d92671879f0a49e69bde1db49fe1b6209387aaa67458d02157b2f2c343ba17cf6e24e53e6c14ae9998b41e557bdb2ed4935e8f86b56a14565252bfdd12f1ad54d794e09ab3f49db890a80ae03951377218150cd4f53e65b94d50ab017f18b8c609c42cd7ecc2590e36ced770bcc370a38369d1832c33ac1a1910aa815739").ToPositiveBigInteger(),
                    // q
                    new BitString("b289a7b89fc29cc8e1337aa819e506c47dbb7372a2162b90983fca43").ToPositiveBigInteger(), 
                    // g
                    new BitString("b2947e9fd2ec7ed4af6d2c4057480110c7c7862441f3c19f39e1dd26eab8d6925a7ef2dfe4bb84388c0d1b0379fcde2d7c677e829abbd3fc7921d95165eeb01e75ef188ff687e6fd69b5638e6ad93cedbc6855be28c8fdd7e4fe85f5d61d6711fed1abd4702cf2efbc7c6e1608f2a3cc4fca635c3f33454553f42cc3706987d160059120e8ed72587b5ea36690aa9fafade666db05f23d095c760899539b7e333db6317e548f4ff7c1d7a111459e3681ffe05ba4fd3473f02e952626f5c6d499caf761ba4185a4b3a4141268128c645916ad5fa3c43fea4f391783dc5172742c1e07151f5202a707a4812b6883433f9a266c611fb083afdd84118d9f69f3012e").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhHybrid1,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D224,
                // Key confirmation role
                KeyConfirmationRole.Recipient,
                // Key confirmation direction
                KeyConfirmationDirection.Bilateral,
                // key length
                112,
                // tag length
                64,
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("3a1854365855619c81b6bf33c16da93451a6b6ff356bca71c5c402f8").ToPositiveBigInteger(),
                // public static this party
                new BitString("374af107d6a6be79a383a786458ecc7585ad613cb6ca219d3d3ea2137cba62f1157abc047cf2183dedd7b372ccd2f78a75ae89fd0c8368f7cfbd6a238f343636f859913daf254a7c626c5ce8f42b40597a3a202413d658a4cf1909554240c7dfe0bb9ae56ff3c22bd670a85be08928ef2e2a91beb8ae9122acc47562d116813533c31418bd4b7c128bb172774bfd3f8cf467d4f0bbdb3c6969d31a2d2265578b0d4d3af04fa423ac3d9ab6cba7dec988eeccbadcec5f86cc83d7ad7da7740e191cb986819221ac81434cbc28cd56ec508bf327bcd5fb05db21a6bc48b1821d4c06e635faf34ef619bd5446de7af484f2d5606883b5c3ffdddbd97699fe488bcd").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("18a2d714d92487ac795b9b4e2c867ddeb9c7da42e6dfcf85eeb239c1").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("07bf775ab92262b1a2e271795236b41712b830373954b2c74cb548a9b044c97b44788afebd34736ea8a21f580e859d26b615e7aa50d5faf856f3decb66998a790d5058af1f38885ed5e106b5cfc5c0c92525445b9d352069673c3b0dafb2a949b414332683f5fd30ec66f56f5b529ed68ac493d91578889b7d8136518edc1f38decd59ae72c4d32adc8237e2c2db4f905dd9a438a8c7680b07c7a5dcb5b2cbf28c0706ce92e6b1ab9ece33b13dd2c5640a4547b2ea1649def0711572b4a8f05fc3b32ca6b840965cce5c76bd94663513ceb768fbb20a2d1214f704552a63cf3ca7a947d3a506d7dbacffc20c77053c0abbe6f75287ff3aac3fe05b855217d817").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("3f6b43bb5239c5ddfef5394ecf46db3753439328d598ba1ad907879d").ToPositiveBigInteger(), 
                // public static other party
                new BitString("6c92ec906040391563b4b861963c5bb8174d7a60bc1eb6027dd77bf0f77427b3041cfa0af4dc8ea65a5c66d34fb237fb6db2a67a104d09f1babd7eb4e09d6e0394ad118c876f9789a99dea0c55ae4504827eb05c2f262da1b002e71194dce1cd3141b1f54f284233800259b9c6d0fb3ad92cf04e4d05219ee12138b837bfc789cdf360939f0417836c6b2e4e1c09027737d1166206ea16bdceb91bceb5017ab867ec862e38fee0ee42788a3bae1130fcfba54439254cd192bae28c36a70cf1d6fbd0a322d3ab6e3b86b3a352a6e29e7aa88d7a60aa0d6d34ef1b26af428b273b6fc10e4d8b29eb6f4dfb271c16b95d536c9a12a4d0f423946eec79eb46cd999a").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("89f1b69071e0fa857399ed1a3ea0a830803601c6dd93ffa3a5b014fa").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("54f4ad107e849716774b3910c6f9b920e5fdc2f69f94bed21adf5e87c4343a0832f78360c7e2febbe39e00778a8a781640ca53a12f3280763a8ab2c53fa1d1fbd1a6b8d5a3e5b2b1097af9aea3370240a846f97105fa90ce238e504e1dd17247fd8a9950bc54651eba7a19a320f0c4e8586d570f5d0b09141d36a42f03c2ba1e33d61ebe4ead935b10fce1098c54baa08f79a204fbc80beb2fcf10d37213bc90246ab59cc27406d15c63ec585280c749dbf33e2e79ead1acb035245c45788813f33a9eb1865942f208d7b30bfbdb6ee70febd0159a01a4e89882264615b5a6d84a7cdec5966a68f7751b8b544d41e4a59b5971574ffa620719cf18b135b29210").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("61174babcd75988baffe79e92de5d5d535b85e26fa24c07db7a6e555d41a18ae3536fa2f58d178094d27c88204fba5a40a7565f9944fc7cec68b549580bfe08047bb85e20a6334ef3c7dbf64958b40ad7d8753d17419348aba820cb24749e529e30f360b9b6073b8d88608f58126a4d269d32001eac59b724d259f598e705d63c3a338e21ead16d7961faf00f0ecb1fe3ad765277e236d468d86ba4b92cf78b0f81d327303de8c72b82f8cd787db88b37d0b4deeeba0f7249b6222efae59c697297c977544e477dfb9aa00ec71a852c829aa288a1dd218cf38ca17a8910d9f408434ec7c8b6c7f75392a70ca2d60980b2ed535b7cd643eb4f59b6befbb7dad0e010d57826496e645b78bb4d2539f0ac091cfa2eadb3bf45be9c8ba12aab5ea8c4307bc35fd187643cc8903507f083efdb576945fceadaaeaeb4e2d655ad24eb21f4e821bf4231741264d861b8691a186195e41d1f92c3eaece42641293d85697f0ff3aca31684cae5b92d4eedbec0472ce71425821e862cf481967d80b6f4127c286f52deefc0acb6919d92d908d577ee8e3955e967e92748027b3af46f96e9445523a30893242a10c7a39156772a719fe2fcbcbebc1a98ae38c25888aa40cad6a01fcbb08564968a337b1883da8ba540745ec918e52edb799510b1253077a7a9ab037ad869e3e90fd626ecbf757521283a842bb7f120855014f8f52c08f3a30"),
                // expected oi
                new BitString("a1b2c3d4e543415653696486db011671fe2a20ff93f27057f1e7ab9b8350"),
                // expected dkm
                new BitString("53516f8f0a2c05ed378a59178cca"),
                // expected macData
                new BitString("4b435f325f55a1b2c3d4e543415653696454f4ad107e849716774b3910c6f9b920e5fdc2f69f94bed21adf5e87c4343a0832f78360c7e2febbe39e00778a8a781640ca53a12f3280763a8ab2c53fa1d1fbd1a6b8d5a3e5b2b1097af9aea3370240a846f97105fa90ce238e504e1dd17247fd8a9950bc54651eba7a19a320f0c4e8586d570f5d0b09141d36a42f03c2ba1e33d61ebe4ead935b10fce1098c54baa08f79a204fbc80beb2fcf10d37213bc90246ab59cc27406d15c63ec585280c749dbf33e2e79ead1acb035245c45788813f33a9eb1865942f208d7b30bfbdb6ee70febd0159a01a4e89882264615b5a6d84a7cdec5966a68f7751b8b544d41e4a59b5971574ffa620719cf18b135b2921007bf775ab92262b1a2e271795236b41712b830373954b2c74cb548a9b044c97b44788afebd34736ea8a21f580e859d26b615e7aa50d5faf856f3decb66998a790d5058af1f38885ed5e106b5cfc5c0c92525445b9d352069673c3b0dafb2a949b414332683f5fd30ec66f56f5b529ed68ac493d91578889b7d8136518edc1f38decd59ae72c4d32adc8237e2c2db4f905dd9a438a8c7680b07c7a5dcb5b2cbf28c0706ce92e6b1ab9ece33b13dd2c5640a4547b2ea1649def0711572b4a8f05fc3b32ca6b840965cce5c76bd94663513ceb768fbb20a2d1214f704552a63cf3ca7a947d3a506d7dbacffc20c77053c0abbe6f75287ff3aac3fe05b855217d817"),
                // expected tag
                new BitString("f9f49d07d00040cb")
            },
            #endregion hmac
            #endregion dhHybrid1

            #region mqv2
            #region hmac
            new object[]
            {
                // label
                "mqv2 hmac224 sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("c1e94773723df1f4793fc3e75e0fc293ca25ed228531dfdfab1c04f9e7087caa41bc8787c71cae7cbf4b8929b481415cd94dd36d3b0957c3b364e4904c51adda6224ec33419c1efae50057d82d87bffcf240ba16852bb97370036e8cf950e3857d5fd0d45fc9c9500e8bb8e718ca7bf0f5fc93cd11d80d773f8701127dd0fa843f2690012bb740102c7dc3fee44f3a828e0d64b4d8f39176c90fd02aa250db0d0152ede4e909166b1e493220d8026954587d1566c5286f86e3f0a3647628b2de5a113d0ed7e9bf8a784ff87a15873040ecae8bf0de69a189f72e256bbb2fb615ee8cdfe9b5e9bab5440394199cac4902697c1b2e5ee92e42c48e8cd296f054b5").ToPositiveBigInteger(),
                    // q
                    new BitString("f4fade85ffeb298fc51cee176bbb39c53b435dd87c65850c385ddfd5").ToPositiveBigInteger(), 
                    // g
                    new BitString("8d790c6253a45a4748341fb2785b46d23a2c213a4999723f19bc665e641a323a125a7af60091987725dded363e3b117bf332ef1573d3f237daa9dc85c657ce0b6feb6ef660856ffbad4a7654048c9bdd7f1d0bb14b5ea260cc291064e4212d176131fb34fc6b1d57ef8886e311beedeafb806e0741b10108c0d40f8ea9aaf47bf5433f3ab4a636346d1445e5ae41f6f87ea293dfec178be05a19d94411c383c3379f4c0b9c1339057c0023dca757c565f0745bbfe188c11454afb4f0f34de535ab11b36e8fcb37ec6423d5ed126859b42b923096be75b4b98312bcef14348e9bf85be4d198150886b019b4d3aa2036cc4fc6b19bc9b3c6642e0ca5cedc233ddb").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.Mqv2,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D224,
                // Key confirmation role
                KeyConfirmationRole.Provider,
                // Key confirmation direction
                KeyConfirmationDirection.Unilateral,
                // key length
                112,
                // tag length
                64,
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("d62d20b94227c8b47a96b644dd7445cb053c6daf2d1eee03862d3973").ToPositiveBigInteger(),
                // public static this party
                new BitString("5935a6b51b5c2dfeee3e6dd402918c1d71efcd66bc8007f9d8bf2669c90c76cebeacfa950fe9018510f906ceaebd2a5a1b61f6acb30b72787521ff7593f85c937bb6afac567334c0ba2f953af5169e26b4987f8379198542f9741d4b5ae9e5bbfb57761f99daea04ffa3de07a564c5bafcbbb6f163c7c5aafb98035020d2c33b7686d4c5b38bec471192f4fa0d5da73549ed4983b3641810f5ccae53fbf85499c73cf90264248020aac965454cfe60a505c825463e048cf3d9cf24b138ed3fc88404d04f85a2eb30bb3bdc00ac980799949f8bd3766bcaa62f16a0df4fb6abc9792d6c1c5306b6f9f546b9b19a02ec25e93eda5a387e120963e7869be17cccc5").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("2e2dc83d78d707129c0c08898af72a3439cdddf449ec6f50936d1da7").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("70e0f8beb815d30223d34e0f20e83cd6bb6346a156f658cf5ffbdb0dec48b85c0acd4e4b6ea3f3bcdedae55af5bb85184dd25ff2b2acbcf5cab7f551582e4e45462ed64ab11d068a0ad5a62478807a4cec92a4fcaf53ce3d8681575a592f2805f282e28214dfb1727514f6c67932eb409545176e70d5b912e0976e5027aa9e1eb7d26a64d02d66b95e6ca62cbfdc6e36eb49e3b0bf334bd47ca5e54e3adce36c8d24e87c41827f3ee256dcb8d359c892f991a905eae0668dd371e2dda9977ffb74ebf118f164df23f9fc1ab8d7eb23b59d58d7ccb72538aa667b1cc327348df34dfea2cccad690354bb7bdd118da0c0901aad4078f82265da0c01711c3d13b2a").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("c977345bbd3ea17b71171bf5e1e868643ec2cbafa45be6a666edd2af").ToPositiveBigInteger(), 
                // public static other party
                new BitString("1610df550826fa07a2790c038edc77c22305c3f12a10f4b8c987c868436f4a88d5dca0524fbd4bf4f3217afa95aca0b364a3f9911cfd6ea3fdb8c8947bed24a8dcd09a26d7bcb66f4340794fb1dca19a6022d1ee533cd0e87a771f746feabbb0b6ff6a288da94b272d61fb8d9e0d26433cfb02065396d995d3c0015fd5f73156b167679d864cd1f75c2df994b57e0dfa3628b2c8a2dcfffbf2e34da006d2808ae3998072791e3d8ad34434f8afa697a6c3394c8275925e15be7ba7436067b25c371fa5b8aeb9e80198e15fe705e9c1b48b279c14a9ce8b4d7378c2e042207a1c4832d4dc72ec50e363a8869814bd378d29be46a03a0c4f89a8d973aa46787b62").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("7e7705a23819b19a42fe3f8edafbd8969e166fec10f1e7d57e9ae9f8").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("99e13b3d73f742b1868b50bc4da529542640c399494250a4767158f0e27555fbbbc8ccaf0789456422b85127674ca0372f8d40fb339be2796d2e1a379fdc6ce8c17ab94150c9252d316f9eaef6354a3e05389dcef36357bed870c3852cc342c17bee00b85e356c5691e463f7904206ad88527ede9bcf5f7fe2bab98db0214af5c97c56098e650c5ae9cb70dd735c01b7048c13f584013b7386e36201c09c8f795f0bd13f79e570d7957e6fd1803921d84c7588f4e9ff83a0efd16f49f293f394102deeac81ede25c395f7ce493a8290a3589a62ffac0f333592fc3f85ebdd863eac4a309b10ea531e2327a5e4085eba1fd46dfed2c1a55965affbe18d1540b33").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("a3aeb0489a6cdaf6cb46a693a06df3a2614947a4e9eb6add958cd65eb89e46ccd4d148140fc3338510855447ea969268bdc2c07f6e0552fb62cd8dd0e9bc962ede53e445cec4b0acca8b5bc62599690e1ac2d7d6e17cd42781d49eccb5236f469aa36d50baab3892458f776a80e27e9383daefe757b79ff596a2c24f3e2f61fa4522b6e7cc6c69e4152fbb2ac61a8a7970d37ffabdd7ca84bc1febc73914f142b4e19913aa76312eeee906d44967c336d389f6993846a43c3c7c16e0a75dcab3095f4045a42e8452d05ef2a8f40ac778616cd779fe8a5877693f26fb5aad44fe7d18d325bbb2e7b65cc47df0c8446c8c3850de9594bd9bbc447d3be78a192699"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964faa2c5f7c2b0aa6276f2032ec3330908b71854"),
                // expected dkm
                new BitString("62089802ce7f0a5480b7518192b5"),
                // expected macData
                new BitString("4b435f315f55a1b2c3d4e543415653696470e0f8beb815d30223d34e0f20e83cd6bb6346a156f658cf5ffbdb0dec48b85c0acd4e4b6ea3f3bcdedae55af5bb85184dd25ff2b2acbcf5cab7f551582e4e45462ed64ab11d068a0ad5a62478807a4cec92a4fcaf53ce3d8681575a592f2805f282e28214dfb1727514f6c67932eb409545176e70d5b912e0976e5027aa9e1eb7d26a64d02d66b95e6ca62cbfdc6e36eb49e3b0bf334bd47ca5e54e3adce36c8d24e87c41827f3ee256dcb8d359c892f991a905eae0668dd371e2dda9977ffb74ebf118f164df23f9fc1ab8d7eb23b59d58d7ccb72538aa667b1cc327348df34dfea2cccad690354bb7bdd118da0c0901aad4078f82265da0c01711c3d13b2a99e13b3d73f742b1868b50bc4da529542640c399494250a4767158f0e27555fbbbc8ccaf0789456422b85127674ca0372f8d40fb339be2796d2e1a379fdc6ce8c17ab94150c9252d316f9eaef6354a3e05389dcef36357bed870c3852cc342c17bee00b85e356c5691e463f7904206ad88527ede9bcf5f7fe2bab98db0214af5c97c56098e650c5ae9cb70dd735c01b7048c13f584013b7386e36201c09c8f795f0bd13f79e570d7957e6fd1803921d84c7588f4e9ff83a0efd16f49f293f394102deeac81ede25c395f7ce493a8290a3589a62ffac0f333592fc3f85ebdd863eac4a309b10ea531e2327a5e4085eba1fd46dfed2c1a55965affbe18d1540b33"),
                // expected tag
                new BitString("e2763850acc9f7d8")
            },
            new object[]
            {
                // label
                "mqv2 hmac224 sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("c1e94773723df1f4793fc3e75e0fc293ca25ed228531dfdfab1c04f9e7087caa41bc8787c71cae7cbf4b8929b481415cd94dd36d3b0957c3b364e4904c51adda6224ec33419c1efae50057d82d87bffcf240ba16852bb97370036e8cf950e3857d5fd0d45fc9c9500e8bb8e718ca7bf0f5fc93cd11d80d773f8701127dd0fa843f2690012bb740102c7dc3fee44f3a828e0d64b4d8f39176c90fd02aa250db0d0152ede4e909166b1e493220d8026954587d1566c5286f86e3f0a3647628b2de5a113d0ed7e9bf8a784ff87a15873040ecae8bf0de69a189f72e256bbb2fb615ee8cdfe9b5e9bab5440394199cac4902697c1b2e5ee92e42c48e8cd296f054b5").ToPositiveBigInteger(),
                    // q
                    new BitString("f4fade85ffeb298fc51cee176bbb39c53b435dd87c65850c385ddfd5").ToPositiveBigInteger(), 
                    // g
                    new BitString("8d790c6253a45a4748341fb2785b46d23a2c213a4999723f19bc665e641a323a125a7af60091987725dded363e3b117bf332ef1573d3f237daa9dc85c657ce0b6feb6ef660856ffbad4a7654048c9bdd7f1d0bb14b5ea260cc291064e4212d176131fb34fc6b1d57ef8886e311beedeafb806e0741b10108c0d40f8ea9aaf47bf5433f3ab4a636346d1445e5ae41f6f87ea293dfec178be05a19d94411c383c3379f4c0b9c1339057c0023dca757c565f0745bbfe188c11454afb4f0f34de535ab11b36e8fcb37ec6423d5ed126859b42b923096be75b4b98312bcef14348e9bf85be4d198150886b019b4d3aa2036cc4fc6b19bc9b3c6642e0ca5cedc233ddb").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.Mqv2,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D224,
                // Key confirmation role
                KeyConfirmationRole.Recipient,
                // Key confirmation direction
                KeyConfirmationDirection.Unilateral,
                // key length
                112,
                // tag length
                64,
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("c977345bbd3ea17b71171bf5e1e868643ec2cbafa45be6a666edd2af").ToPositiveBigInteger(),
                // public static this party
                new BitString("1610df550826fa07a2790c038edc77c22305c3f12a10f4b8c987c868436f4a88d5dca0524fbd4bf4f3217afa95aca0b364a3f9911cfd6ea3fdb8c8947bed24a8dcd09a26d7bcb66f4340794fb1dca19a6022d1ee533cd0e87a771f746feabbb0b6ff6a288da94b272d61fb8d9e0d26433cfb02065396d995d3c0015fd5f73156b167679d864cd1f75c2df994b57e0dfa3628b2c8a2dcfffbf2e34da006d2808ae3998072791e3d8ad34434f8afa697a6c3394c8275925e15be7ba7436067b25c371fa5b8aeb9e80198e15fe705e9c1b48b279c14a9ce8b4d7378c2e042207a1c4832d4dc72ec50e363a8869814bd378d29be46a03a0c4f89a8d973aa46787b62").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("7e7705a23819b19a42fe3f8edafbd8969e166fec10f1e7d57e9ae9f8").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("99e13b3d73f742b1868b50bc4da529542640c399494250a4767158f0e27555fbbbc8ccaf0789456422b85127674ca0372f8d40fb339be2796d2e1a379fdc6ce8c17ab94150c9252d316f9eaef6354a3e05389dcef36357bed870c3852cc342c17bee00b85e356c5691e463f7904206ad88527ede9bcf5f7fe2bab98db0214af5c97c56098e650c5ae9cb70dd735c01b7048c13f584013b7386e36201c09c8f795f0bd13f79e570d7957e6fd1803921d84c7588f4e9ff83a0efd16f49f293f394102deeac81ede25c395f7ce493a8290a3589a62ffac0f333592fc3f85ebdd863eac4a309b10ea531e2327a5e4085eba1fd46dfed2c1a55965affbe18d1540b33").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("d62d20b94227c8b47a96b644dd7445cb053c6daf2d1eee03862d3973").ToPositiveBigInteger(), 
                // public static other party
                new BitString("5935a6b51b5c2dfeee3e6dd402918c1d71efcd66bc8007f9d8bf2669c90c76cebeacfa950fe9018510f906ceaebd2a5a1b61f6acb30b72787521ff7593f85c937bb6afac567334c0ba2f953af5169e26b4987f8379198542f9741d4b5ae9e5bbfb57761f99daea04ffa3de07a564c5bafcbbb6f163c7c5aafb98035020d2c33b7686d4c5b38bec471192f4fa0d5da73549ed4983b3641810f5ccae53fbf85499c73cf90264248020aac965454cfe60a505c825463e048cf3d9cf24b138ed3fc88404d04f85a2eb30bb3bdc00ac980799949f8bd3766bcaa62f16a0df4fb6abc9792d6c1c5306b6f9f546b9b19a02ec25e93eda5a387e120963e7869be17cccc5").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("2e2dc83d78d707129c0c08898af72a3439cdddf449ec6f50936d1da7").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("70e0f8beb815d30223d34e0f20e83cd6bb6346a156f658cf5ffbdb0dec48b85c0acd4e4b6ea3f3bcdedae55af5bb85184dd25ff2b2acbcf5cab7f551582e4e45462ed64ab11d068a0ad5a62478807a4cec92a4fcaf53ce3d8681575a592f2805f282e28214dfb1727514f6c67932eb409545176e70d5b912e0976e5027aa9e1eb7d26a64d02d66b95e6ca62cbfdc6e36eb49e3b0bf334bd47ca5e54e3adce36c8d24e87c41827f3ee256dcb8d359c892f991a905eae0668dd371e2dda9977ffb74ebf118f164df23f9fc1ab8d7eb23b59d58d7ccb72538aa667b1cc327348df34dfea2cccad690354bb7bdd118da0c0901aad4078f82265da0c01711c3d13b2a").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("a3aeb0489a6cdaf6cb46a693a06df3a2614947a4e9eb6add958cd65eb89e46ccd4d148140fc3338510855447ea969268bdc2c07f6e0552fb62cd8dd0e9bc962ede53e445cec4b0acca8b5bc62599690e1ac2d7d6e17cd42781d49eccb5236f469aa36d50baab3892458f776a80e27e9383daefe757b79ff596a2c24f3e2f61fa4522b6e7cc6c69e4152fbb2ac61a8a7970d37ffabdd7ca84bc1febc73914f142b4e19913aa76312eeee906d44967c336d389f6993846a43c3c7c16e0a75dcab3095f4045a42e8452d05ef2a8f40ac778616cd779fe8a5877693f26fb5aad44fe7d18d325bbb2e7b65cc47df0c8446c8c3850de9594bd9bbc447d3be78a192699"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964faa2c5f7c2b0aa6276f2032ec3330908b71854"),
                // expected dkm
                new BitString("62089802ce7f0a5480b7518192b5"),
                // expected macData
                new BitString("4b435f315f55a1b2c3d4e543415653696470e0f8beb815d30223d34e0f20e83cd6bb6346a156f658cf5ffbdb0dec48b85c0acd4e4b6ea3f3bcdedae55af5bb85184dd25ff2b2acbcf5cab7f551582e4e45462ed64ab11d068a0ad5a62478807a4cec92a4fcaf53ce3d8681575a592f2805f282e28214dfb1727514f6c67932eb409545176e70d5b912e0976e5027aa9e1eb7d26a64d02d66b95e6ca62cbfdc6e36eb49e3b0bf334bd47ca5e54e3adce36c8d24e87c41827f3ee256dcb8d359c892f991a905eae0668dd371e2dda9977ffb74ebf118f164df23f9fc1ab8d7eb23b59d58d7ccb72538aa667b1cc327348df34dfea2cccad690354bb7bdd118da0c0901aad4078f82265da0c01711c3d13b2a99e13b3d73f742b1868b50bc4da529542640c399494250a4767158f0e27555fbbbc8ccaf0789456422b85127674ca0372f8d40fb339be2796d2e1a379fdc6ce8c17ab94150c9252d316f9eaef6354a3e05389dcef36357bed870c3852cc342c17bee00b85e356c5691e463f7904206ad88527ede9bcf5f7fe2bab98db0214af5c97c56098e650c5ae9cb70dd735c01b7048c13f584013b7386e36201c09c8f795f0bd13f79e570d7957e6fd1803921d84c7588f4e9ff83a0efd16f49f293f394102deeac81ede25c395f7ce493a8290a3589a62ffac0f333592fc3f85ebdd863eac4a309b10ea531e2327a5e4085eba1fd46dfed2c1a55965affbe18d1540b33"),
                // expected tag
                new BitString("e2763850acc9f7d8")
            },
            #endregion hmac

            #endregion mqv2

            #region dhHybridOneFlow
            #region hmac
            new object[]
            {
                // label
                "dhHybridOneFlow hmac sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d7f305edb822fa8d6c492c871b72e726934063a0993674a832cdc5d4e5682a4915ee9b976f3d4e6c00778e12ca7f9a833cb3be8ef02279584df92e76722756442d026bf3a6dafd2cd934f28964579f3ccf05a8b0ae8429f847ee4ff87922708be22b30df28bb3ead0b9fb468a0b8628b1be5a2050faad91819e1cf8743fbcc19ca16136fb1a4a26e180ad0a15b55754a68770e67e08da0ddf71feb96ab90ef424a425db0dcc14f9abd523d3ccfa1efa1050e49c876c3a5b772371c3e3dc19239a67a2b9d686bc0b523ad82f9ee988a663f20af3f0038e0fb62d0e8e8be31e66e91c9ffc9406db2b3583d85f6b28d4e5e5c5e3b07530d5f9933dbd60ba5461147").ToPositiveBigInteger(),
                    // q
                    new BitString("a327d88d9b5acbe4643488550a8400c52936ca686c3dcddc5c4689ed").ToPositiveBigInteger(), 
                    // g
                    new BitString("b5154316cb88c52dacc1cdf09d9d7d50945ee475f27d1b29d3dbce87403d8e5cbc89c7c34900fbfc0aff84a9cd020dd450f11511063df15a55525e5c98b80ee696d0ea0c207e5fb69580f1a06165f4f4971c1167abf7e4419b42d4d33b8f2a2c0920066bffbda6658c2d40dcd0d44244d3e98e451293a5b6412ab6054614bbd37fb3d5fbece3f02fe3e2e5936228212698df432259418f6f133b2753919fcf55f6b281da87c051ad81eba569573ec2e97cd88b11557160ceb9669efb2d455f1a1012fe416a10cbae3c717cfc85f354cd5801fef45dbe11a2597de73c01c8d06d4ac02d39c7c656aa7713f0c2322e5528cc1b80f6e4ad9ff50ce39d35b9c515dc").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhHybridOneFlow,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D224,
                // Key confirmation role
                KeyConfirmationRole.Provider,
                // Key confirmation direction
                KeyConfirmationDirection.Bilateral,
                // key length
                112,
                // tag length
                64,
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("872ac01539460a49730fd1a03f04b1ba9dd80978280be716dda60b9a").ToPositiveBigInteger(),
                // public static this party
                new BitString("9b159189fef6a81e724119e920b010cd5760d24ca9246ea99bc7bda065b572aa1f96dacc3ec9e67a7dc7587a350ca49687d4a1ab392a46cfe08c09b8cd22b82983975916820c12103b6033186261173bd68d5152c7379a07108969042efcc65190b7d67634ee4b8f3af9df9aed5ed5a6ceb1d3019a57feaaac21bbee9c4ed093a9901b0d96c5818e6da5cdb7251134ae364fef7b39e7841eeec1f90127f2592747f1ffee1b0a552a26d6e26ba9275da05db94c1c27815b4727172385f6edf5e1c0ad9f9dd3daf77a5218519f1dbdcfa25f2a36d766284be28e41fb7d4e7df92d3bb0d361b769ae9e20a32054a1a171af64d72f131461c84a1dad937daeb9088f").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("874130248cc38e4b9b0723b24334f814b3c473da47585e13b251b8e3").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("6c6424a6d40b3cce7f34ec75bf84f5f4298207dcee1e5df07918e1de09c8216d2a6c6750dc0d9f8933330cdb214aa674b93844763574da8f8968b952678370c5d0cba2b0274fd53c5d5bcef649bf00673a09fb8acabe6e2d4a1314da1489a95ffe751a76927602398c64bbf2cdb451ea02c5d8ed36dc3edfec1e3aa93ecfd89d2a12e7ba72ded6ab2003d6da562a2d7cc50193ad39459214cb783f691546fd457ab0e3a7100a4c3000ca4ff0aee22290571ab9676c8c866d5d5cd2d3ed89b7d2442506194bb418d0d568159935df8b4c93427085d4df9626a72b4d7b8b7952e752a0ae7bfc36fb929caa74021686e937dd159b758da3f8e3b42c67f2d23b6e27").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("72ab526cfe147772fd7016f371ffe1d0181dc8471eccaddad3a28865").ToPositiveBigInteger(), 
                // public static other party
                new BitString("965bfd67b828f49cf0af68997853d00388d3c359edfb6976408fe41c5ebcc1e572f3625740bd20a600162c54603d21d81e8ea1a9befc91e1d2ed58d005c55ec2940bb5e5b5d1a678493ccd2aa4950ede77275d0f43da83d624b697363bb2e990a9685fb5dc2c232c365846fa6b430ddc5433ab36e09baf6da10bb4a1c8f1cf831dbc02705b3b7d97c2ed8ca310f769b50fc0b61898258bb609527dcadf7d569123a89d393b0e59671bb5e58013dcd8bd185fc97638c520a8a1797c445acd116995dcf906aa77b8cdf0a947054954e9e0b5356e3bcf153e35146bb1563e117e6b526bc79d25cdec877c0b8d0e76ce3e7aab0397e768a3f81125b1a4b6c6293692").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                new BitString("ce22de377026529085cee76c5e3711ffb445f7618ff1798567e4726a89443a95bdce70be2625f6ca69693cf9120cc26133e1d89b4d76a9bc74a47786af6428e09ee402845fdbb3f8b11994bf5a622ed0c77cfe47f387755b8a8d43904d5ad52db88e1cf1e2f09102a605762600e25e35b93eefcf47cbe74aefc75dee2ccd4a6554f2476d790d98d08d566a96ca335979514e359912eb0671eb794d0a14668d6eba380a60eadad14ab133f87782fd2782d7d5580f1b8edab8c8cb9c4bcdcda8323289ee32fefe435959c5a931eeead13a93fade972c5d6c08cce6d1181ea9a298040d7a4b7d22f6e3cc34042cd7a05e88cde6519b0b00c34741f274dbd0a38487"),
                // expected Z
                new BitString("c30cb9a8738ae8261af398db213fdaa2acf0397b8b81084a62219e741d188549bc458b1f24fcdac6e6404b7830ce95547fc86fe5d8e106957c72819828039580bdc0c7ce1b18c86072b43c985d4b56e39f428d26d78250d4f36c09a01b6d490d815fa586da75add7b87e162bef62f428609e240562d7c208f6a43c9eeff3bc3ef950af4c3ab346c52af612f1c936506de23db01f92be6cbf73b060ba39b5ba8a2ba1e6e33d43efc86c1da60156e7869fd092f420f7974579581be2658fb3e509a464f1d25fb6d85effeefa3e3d20e11c622085be5e3292bc7cfab10beed9fcd164bdacaaa719552c3eccbc034a84a413b165cc930966cd1dbbe5a177c169340a988d89c50bf2264c7db127630dcb7d7286047331f117b7f755343beaf6bff759fe2c18c793cf4d8c101a89230950bb6e79037a28d117e11ebeffb7d8d2577d63ab9fbbaf2b68b6bb6d7773630477141bc5ecd9aa64f499b73e4e38d112624f09beea06ea9c2ff0961d667a9c92996a561b9bbbd5189907293803876c5666e6e5f25beef5a0847ab1959a0e9928264daa4bb274f1a0f9e3fb23fc755834087a7a99a37efe617dc564140e6be612f1096d829e5dd8afa32dbd4a414571cab847bcef6f28afb8752db04ee78aeabc8d5d2fd21ba56ecc424c69e82b0848d61ab07089048020fb85b8f0eda91fd9263677abd0458c058069ca5347a62b342e9a9056"),
                // expected oi
                new BitString("a1b2c3d4e543415653696479ff90a67582e0666856a1ae28840206f5e4d1"),
                // expected dkm
                new BitString("066c3f099fe66972d4c34ef1ead6"),
                // expected macData
                new BitString("4b435f325f55a1b2c3d4e54341565369646c6424a6d40b3cce7f34ec75bf84f5f4298207dcee1e5df07918e1de09c8216d2a6c6750dc0d9f8933330cdb214aa674b93844763574da8f8968b952678370c5d0cba2b0274fd53c5d5bcef649bf00673a09fb8acabe6e2d4a1314da1489a95ffe751a76927602398c64bbf2cdb451ea02c5d8ed36dc3edfec1e3aa93ecfd89d2a12e7ba72ded6ab2003d6da562a2d7cc50193ad39459214cb783f691546fd457ab0e3a7100a4c3000ca4ff0aee22290571ab9676c8c866d5d5cd2d3ed89b7d2442506194bb418d0d568159935df8b4c93427085d4df9626a72b4d7b8b7952e752a0ae7bfc36fb929caa74021686e937dd159b758da3f8e3b42c67f2d23b6e27ce22de377026529085cee76c5e3711ffb445f7618ff1798567e4726a89443a95bdce70be2625f6ca69693cf9120cc26133e1d89b4d76a9bc74a47786af6428e09ee402845fdbb3f8b11994bf5a622ed0c77cfe47f387755b8a8d43904d5ad52db88e1cf1e2f09102a605762600e25e35b93eefcf47cbe74aefc75dee2ccd4a6554f2476d790d98d08d566a96ca335979514e359912eb0671eb794d0a14668d6eba380a60eadad14ab133f87782fd2782d7d5580f1b8edab8c8cb9c4bcdcda8323289ee32fefe435959c5a931eeead13a93fade972c5d6c08cce6d1181ea9a298040d7a4b7d22f6e3cc34042cd7a05e88cde6519b0b00c34741f274dbd0a38487"),
                // expected tag
                new BitString("b24eea129bbf80b7")
            },
            new object[]
            {
                // label
                "dhHybridOneFlow hmac sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d7f305edb822fa8d6c492c871b72e726934063a0993674a832cdc5d4e5682a4915ee9b976f3d4e6c00778e12ca7f9a833cb3be8ef02279584df92e76722756442d026bf3a6dafd2cd934f28964579f3ccf05a8b0ae8429f847ee4ff87922708be22b30df28bb3ead0b9fb468a0b8628b1be5a2050faad91819e1cf8743fbcc19ca16136fb1a4a26e180ad0a15b55754a68770e67e08da0ddf71feb96ab90ef424a425db0dcc14f9abd523d3ccfa1efa1050e49c876c3a5b772371c3e3dc19239a67a2b9d686bc0b523ad82f9ee988a663f20af3f0038e0fb62d0e8e8be31e66e91c9ffc9406db2b3583d85f6b28d4e5e5c5e3b07530d5f9933dbd60ba5461147").ToPositiveBigInteger(),
                    // q
                    new BitString("a327d88d9b5acbe4643488550a8400c52936ca686c3dcddc5c4689ed").ToPositiveBigInteger(), 
                    // g
                    new BitString("b5154316cb88c52dacc1cdf09d9d7d50945ee475f27d1b29d3dbce87403d8e5cbc89c7c34900fbfc0aff84a9cd020dd450f11511063df15a55525e5c98b80ee696d0ea0c207e5fb69580f1a06165f4f4971c1167abf7e4419b42d4d33b8f2a2c0920066bffbda6658c2d40dcd0d44244d3e98e451293a5b6412ab6054614bbd37fb3d5fbece3f02fe3e2e5936228212698df432259418f6f133b2753919fcf55f6b281da87c051ad81eba569573ec2e97cd88b11557160ceb9669efb2d455f1a1012fe416a10cbae3c717cfc85f354cd5801fef45dbe11a2597de73c01c8d06d4ac02d39c7c656aa7713f0c2322e5528cc1b80f6e4ad9ff50ce39d35b9c515dc").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhHybridOneFlow,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D224,
                // Key confirmation role
                KeyConfirmationRole.Recipient,
                // Key confirmation direction
                KeyConfirmationDirection.Bilateral,
                // key length
                112,
                // tag length
                64,
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("72ab526cfe147772fd7016f371ffe1d0181dc8471eccaddad3a28865").ToPositiveBigInteger(),
                // public static this party
                new BitString("965bfd67b828f49cf0af68997853d00388d3c359edfb6976408fe41c5ebcc1e572f3625740bd20a600162c54603d21d81e8ea1a9befc91e1d2ed58d005c55ec2940bb5e5b5d1a678493ccd2aa4950ede77275d0f43da83d624b697363bb2e990a9685fb5dc2c232c365846fa6b430ddc5433ab36e09baf6da10bb4a1c8f1cf831dbc02705b3b7d97c2ed8ca310f769b50fc0b61898258bb609527dcadf7d569123a89d393b0e59671bb5e58013dcd8bd185fc97638c520a8a1797c445acd116995dcf906aa77b8cdf0a947054954e9e0b5356e3bcf153e35146bb1563e117e6b526bc79d25cdec877c0b8d0e76ce3e7aab0397e768a3f81125b1a4b6c6293692").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                new BitString("ce22de377026529085cee76c5e3711ffb445f7618ff1798567e4726a89443a95bdce70be2625f6ca69693cf9120cc26133e1d89b4d76a9bc74a47786af6428e09ee402845fdbb3f8b11994bf5a622ed0c77cfe47f387755b8a8d43904d5ad52db88e1cf1e2f09102a605762600e25e35b93eefcf47cbe74aefc75dee2ccd4a6554f2476d790d98d08d566a96ca335979514e359912eb0671eb794d0a14668d6eba380a60eadad14ab133f87782fd2782d7d5580f1b8edab8c8cb9c4bcdcda8323289ee32fefe435959c5a931eeead13a93fade972c5d6c08cce6d1181ea9a298040d7a4b7d22f6e3cc34042cd7a05e88cde6519b0b00c34741f274dbd0a38487"),
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("872ac01539460a49730fd1a03f04b1ba9dd80978280be716dda60b9a").ToPositiveBigInteger(), 
                // public static other party
                new BitString("9b159189fef6a81e724119e920b010cd5760d24ca9246ea99bc7bda065b572aa1f96dacc3ec9e67a7dc7587a350ca49687d4a1ab392a46cfe08c09b8cd22b82983975916820c12103b6033186261173bd68d5152c7379a07108969042efcc65190b7d67634ee4b8f3af9df9aed5ed5a6ceb1d3019a57feaaac21bbee9c4ed093a9901b0d96c5818e6da5cdb7251134ae364fef7b39e7841eeec1f90127f2592747f1ffee1b0a552a26d6e26ba9275da05db94c1c27815b4727172385f6edf5e1c0ad9f9dd3daf77a5218519f1dbdcfa25f2a36d766284be28e41fb7d4e7df92d3bb0d361b769ae9e20a32054a1a171af64d72f131461c84a1dad937daeb9088f").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("874130248cc38e4b9b0723b24334f814b3c473da47585e13b251b8e3").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("6c6424a6d40b3cce7f34ec75bf84f5f4298207dcee1e5df07918e1de09c8216d2a6c6750dc0d9f8933330cdb214aa674b93844763574da8f8968b952678370c5d0cba2b0274fd53c5d5bcef649bf00673a09fb8acabe6e2d4a1314da1489a95ffe751a76927602398c64bbf2cdb451ea02c5d8ed36dc3edfec1e3aa93ecfd89d2a12e7ba72ded6ab2003d6da562a2d7cc50193ad39459214cb783f691546fd457ab0e3a7100a4c3000ca4ff0aee22290571ab9676c8c866d5d5cd2d3ed89b7d2442506194bb418d0d568159935df8b4c93427085d4df9626a72b4d7b8b7952e752a0ae7bfc36fb929caa74021686e937dd159b758da3f8e3b42c67f2d23b6e27").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("c30cb9a8738ae8261af398db213fdaa2acf0397b8b81084a62219e741d188549bc458b1f24fcdac6e6404b7830ce95547fc86fe5d8e106957c72819828039580bdc0c7ce1b18c86072b43c985d4b56e39f428d26d78250d4f36c09a01b6d490d815fa586da75add7b87e162bef62f428609e240562d7c208f6a43c9eeff3bc3ef950af4c3ab346c52af612f1c936506de23db01f92be6cbf73b060ba39b5ba8a2ba1e6e33d43efc86c1da60156e7869fd092f420f7974579581be2658fb3e509a464f1d25fb6d85effeefa3e3d20e11c622085be5e3292bc7cfab10beed9fcd164bdacaaa719552c3eccbc034a84a413b165cc930966cd1dbbe5a177c169340a988d89c50bf2264c7db127630dcb7d7286047331f117b7f755343beaf6bff759fe2c18c793cf4d8c101a89230950bb6e79037a28d117e11ebeffb7d8d2577d63ab9fbbaf2b68b6bb6d7773630477141bc5ecd9aa64f499b73e4e38d112624f09beea06ea9c2ff0961d667a9c92996a561b9bbbd5189907293803876c5666e6e5f25beef5a0847ab1959a0e9928264daa4bb274f1a0f9e3fb23fc755834087a7a99a37efe617dc564140e6be612f1096d829e5dd8afa32dbd4a414571cab847bcef6f28afb8752db04ee78aeabc8d5d2fd21ba56ecc424c69e82b0848d61ab07089048020fb85b8f0eda91fd9263677abd0458c058069ca5347a62b342e9a9056"),
                // expected oi
                new BitString("a1b2c3d4e543415653696479ff90a67582e0666856a1ae28840206f5e4d1"),
                // expected dkm
                new BitString("066c3f099fe66972d4c34ef1ead6"),
                // expected macData
                new BitString("4b435f325f55a1b2c3d4e54341565369646c6424a6d40b3cce7f34ec75bf84f5f4298207dcee1e5df07918e1de09c8216d2a6c6750dc0d9f8933330cdb214aa674b93844763574da8f8968b952678370c5d0cba2b0274fd53c5d5bcef649bf00673a09fb8acabe6e2d4a1314da1489a95ffe751a76927602398c64bbf2cdb451ea02c5d8ed36dc3edfec1e3aa93ecfd89d2a12e7ba72ded6ab2003d6da562a2d7cc50193ad39459214cb783f691546fd457ab0e3a7100a4c3000ca4ff0aee22290571ab9676c8c866d5d5cd2d3ed89b7d2442506194bb418d0d568159935df8b4c93427085d4df9626a72b4d7b8b7952e752a0ae7bfc36fb929caa74021686e937dd159b758da3f8e3b42c67f2d23b6e27ce22de377026529085cee76c5e3711ffb445f7618ff1798567e4726a89443a95bdce70be2625f6ca69693cf9120cc26133e1d89b4d76a9bc74a47786af6428e09ee402845fdbb3f8b11994bf5a622ed0c77cfe47f387755b8a8d43904d5ad52db88e1cf1e2f09102a605762600e25e35b93eefcf47cbe74aefc75dee2ccd4a6554f2476d790d98d08d566a96ca335979514e359912eb0671eb794d0a14668d6eba380a60eadad14ab133f87782fd2782d7d5580f1b8edab8c8cb9c4bcdcda8323289ee32fefe435959c5a931eeead13a93fade972c5d6c08cce6d1181ea9a298040d7a4b7d22f6e3cc34042cd7a05e88cde6519b0b00c34741f274dbd0a38487"),
                // expected tag
                new BitString("b24eea129bbf80b7")
            },
            #endregion hmac

            #endregion dhHybridOneFlow

            #region mqv1
            #region hmac
            new object[]
            {
                // label
                "mqv1 hmac224 sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("934b7ea4a560a3958f5fb38d60ca5ae88550ca4565815051286afddc4fccc80e4848a4baed688651899de6c2ce9b31404dfe38875e0780c0be958c433ba0870896e9f98933adcaa8643efa70ad8dde7efce48ead3f6b89f342032d84aefa0caee8008a1930742a7203ea3b074f6238cedea9b5876db87e06bc4a16b39de3a870caf7d00a4f5bd9a4aa6882946d7ea6bf6a9c5e690fe907f0612e194ca82b99ccc82d616370ba09ea99f25b148d4c3d99d30da5ac3b83c8d2e716472e551c45c8c0541e9a69e846a89882a109b6480afae983cc3fff932eb64e2ca0ecd266752a727139215910716e2929b985d51359f0c4fd5dd6c9d3c764cb6fa7f609761d91").ToPositiveBigInteger(),
                    // q
                    new BitString("a2fa20a69a6c9881ac8886b8a3fb1f396db84fde38831784faa268db").ToPositiveBigInteger(), 
                    // g
                    new BitString("886f8001eb11413c9337f492408ea76e0ab4120aef7f709e3fabc8b2e672de15495b51d0a04e5359eb5fe39f4069d2bc3b281cc6192058839cc24952caaab93e875fd0b9d70fc3cce00594eae1aa97842c5486b3b4800dd6630d242d9ff29c60bdb6f382fb289783b6d38c4676d8f36a7bf08daac29deece03537604b51e17f0bc8d02f73ee73ddaa4c26f88869ee96964d2102fb8c0f949ef390e311984d1849ea194a5c4e2f80f01c0db7fabbff92d118f872941169866730545622c7b22bd6c2c34144ae0e0544cf2c389108717866a8a289852b06f3e69de77c36b72f649af9edf2ece4ceba76e478809826829d8198cb5d403f80b505ec1717635352895").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.Mqv1,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D224,
                // Key confirmation role
                KeyConfirmationRole.Provider,
                // Key confirmation direction
                KeyConfirmationDirection.Unilateral,
                // key length
                112,
                // tag length
                64,
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("6e5ccacd83781a5d36c146ce1c9ebf3beba3d29e1844e39130039374").ToPositiveBigInteger(),
                // public static this party
                new BitString("65988d04b5a4c0fa13e2e26187583a6f392c86e74b77e22e284e4daf533f35887caaf0e607095263f955f65bc52d72d92af1114925f1bdaab68ac75b344891718cb7236c05726b28a37e959e710bd1c855af2c1f5aa03288a3981bf5cd80b27ab68e22f78f69401a1204dc6870a31766ac753d4ce98772bb511b11cab20d8d4dd810130fe6113d0a3952b1455aeab4478522611971811430d3da004ea38a5ad27cf07fdcf571a7670cc3bc5a904675744d9850f82ec0efc7711ff5865d8258d675d51ab3545f503181cc39a47520594822c4d4dc139e71084267e4c20de173fcafe928d8380af21e9372f28b89c158f0ed4567bb6d6428bf8ab49a2403681ede").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("3096e167964d6d2c7c463fb3a38de40389560f5c421cf3890db62d08").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("4a19abc15c6e7052605d378114ba85a699f658388a88af654587ee18fc917895011bf6eb9c43f18ade1b1d7127b3b7528a1d9e41de9b46bdb38ce0777ae268d82cfd9e52ad1b1bfa2f3cdef66097900e959a96dda09fb42172a8200ceeda5f154a941335fda014974cab11ba0eb5f1ebbf60be8e7ab1fffbb07d6e28f6dfb3f81d13b595037893baacdb21427dc644798ccd76c2d136a44270d3dea67d2bc26d6bd0f0fd346ca3021acc69f3a20b514a56d6f191acd0ba51c4b92b55b8eb9393349feafc1090e8a8efd518fa23cf7215a393b766f7d683bd07900be924ce24c9e46730b6b747a6e1276900f4e3e4b7e930d7db26445a1beb306e589fde7b2b40").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("73b6a0ce6af52bb0f1b94112b94a72eb6925e01c7861b70be71609ed").ToPositiveBigInteger(), 
                // public static other party
                new BitString("59bd2018603e4030af13c18cd10b9f007bba33711b09d2d904de7336511a89d788f1386bd17e8db9e092228758fa58e05edd7ff3c4f747807af478568b22c19fb66e641832716f2697fc17619c0ec3cfdeeab637adc6a08c34e4df5ffd11809149a039a5491dfa116f319b3c065198b0afa2fb8862390c0872f3f5364c1e0bc87bff058b91e09fada5687f25dac6013d193056ff9e112b4a94a3dab90084a9ab0b78002200403923a98427a1b28a5028d67e96b8cee4b6f894e23fb9b1bd153b9a1d42cc32592b12a3cccbfe7b6862f7464a8cb48c4c67afde886dfaf3c2aec2365a598e4bd36ce1741a8998916d9e385e9c43157a9862e4617dde032d8b8dd4").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                new BitString("6e1487efa42e42dcbf73f582cd844543b77f70b4cf3883fea8c6cd852a4293c7e5cb0586a6cd71294883b760cdbbfd07aedaa34d6e1f7915752c309c1cc20f5c05cce731ad9f49bb4480f603bc6697612002af9d27fdcd540981069d91faa2ed731e1638e12b3359596f9cb4c10e7d1ab5007aed2059e68409cd36d0b073b540dac979e369f995cd2f5a90bb043a16fa16dbee8481bb2c6d2018a87da2b4ae1fe0d6f7796d91d8dd6f48ba60ae72cac8eb9bf2ac73ab07d6f46b44ec8e45f7502d4c7a44177c6552c14202ebe73f014edb496dab1eb2df882dcdf2659daff69b6a33e98a8d41a3f3cc4f50a3d62723528feb49caa9571c4c74479a2ff77915ca"),
                // expected Z
                new BitString("6e601f20287b31d3d1cb3fbb28b06299a800c28df5fae37b0d3c97a22ee85a7e33418b3d1cab7b5c26638bcb36e88a0f371a4df20abbc73c61f7ba8f68c9510297bfd4dd12575f4ee424ef64387e1db764d764239becca7cba9936f44e06f09c54f6a72d586e7b1cf392874e9c6525205b72ea2cca58dbe5086d360bf1f3ceb874ca817b16533ee5a6c65b2448400e294a3268beea1ef9c6b34619db7de66d728303b0719e9da380e46da52028cf5047069b40933671e175f4aeac9886416f0be462cf904186689c2dc70bdc93f3d5c9e169d64e4e0e2ced60d83aacd395c35425e4a248655188cc0c756ebffa56a0e3901df8f9cf4ade38d9a98d2a66a74506"),
                // expected oi
                new BitString("a1b2c3d4e543415653696413e138a20a0fb550c9b26f7d6d8209d150814d"),
                // expected dkm
                new BitString("3cce1470dfd7adc27781ac74e986"),
                // expected macData
                new BitString("4b435f315f55a1b2c3d4e54341565369644a19abc15c6e7052605d378114ba85a699f658388a88af654587ee18fc917895011bf6eb9c43f18ade1b1d7127b3b7528a1d9e41de9b46bdb38ce0777ae268d82cfd9e52ad1b1bfa2f3cdef66097900e959a96dda09fb42172a8200ceeda5f154a941335fda014974cab11ba0eb5f1ebbf60be8e7ab1fffbb07d6e28f6dfb3f81d13b595037893baacdb21427dc644798ccd76c2d136a44270d3dea67d2bc26d6bd0f0fd346ca3021acc69f3a20b514a56d6f191acd0ba51c4b92b55b8eb9393349feafc1090e8a8efd518fa23cf7215a393b766f7d683bd07900be924ce24c9e46730b6b747a6e1276900f4e3e4b7e930d7db26445a1beb306e589fde7b2b406e1487efa42e42dcbf73f582cd844543b77f70b4cf3883fea8c6cd852a4293c7e5cb0586a6cd71294883b760cdbbfd07aedaa34d6e1f7915752c309c1cc20f5c05cce731ad9f49bb4480f603bc6697612002af9d27fdcd540981069d91faa2ed731e1638e12b3359596f9cb4c10e7d1ab5007aed2059e68409cd36d0b073b540dac979e369f995cd2f5a90bb043a16fa16dbee8481bb2c6d2018a87da2b4ae1fe0d6f7796d91d8dd6f48ba60ae72cac8eb9bf2ac73ab07d6f46b44ec8e45f7502d4c7a44177c6552c14202ebe73f014edb496dab1eb2df882dcdf2659daff69b6a33e98a8d41a3f3cc4f50a3d62723528feb49caa9571c4c74479a2ff77915ca"),
                // expected tag
                new BitString("270649bed27bed6f")
            },
            new object[]
            {
                // label
                "mqv1 hmac224 sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("934b7ea4a560a3958f5fb38d60ca5ae88550ca4565815051286afddc4fccc80e4848a4baed688651899de6c2ce9b31404dfe38875e0780c0be958c433ba0870896e9f98933adcaa8643efa70ad8dde7efce48ead3f6b89f342032d84aefa0caee8008a1930742a7203ea3b074f6238cedea9b5876db87e06bc4a16b39de3a870caf7d00a4f5bd9a4aa6882946d7ea6bf6a9c5e690fe907f0612e194ca82b99ccc82d616370ba09ea99f25b148d4c3d99d30da5ac3b83c8d2e716472e551c45c8c0541e9a69e846a89882a109b6480afae983cc3fff932eb64e2ca0ecd266752a727139215910716e2929b985d51359f0c4fd5dd6c9d3c764cb6fa7f609761d91").ToPositiveBigInteger(),
                    // q
                    new BitString("a2fa20a69a6c9881ac8886b8a3fb1f396db84fde38831784faa268db").ToPositiveBigInteger(), 
                    // g
                    new BitString("886f8001eb11413c9337f492408ea76e0ab4120aef7f709e3fabc8b2e672de15495b51d0a04e5359eb5fe39f4069d2bc3b281cc6192058839cc24952caaab93e875fd0b9d70fc3cce00594eae1aa97842c5486b3b4800dd6630d242d9ff29c60bdb6f382fb289783b6d38c4676d8f36a7bf08daac29deece03537604b51e17f0bc8d02f73ee73ddaa4c26f88869ee96964d2102fb8c0f949ef390e311984d1849ea194a5c4e2f80f01c0db7fabbff92d118f872941169866730545622c7b22bd6c2c34144ae0e0544cf2c389108717866a8a289852b06f3e69de77c36b72f649af9edf2ece4ceba76e478809826829d8198cb5d403f80b505ec1717635352895").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.Mqv1,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D224,
                // Key confirmation role
                KeyConfirmationRole.Recipient,
                // Key confirmation direction
                KeyConfirmationDirection.Unilateral,
                // key length
                112,
                // tag length
                64,
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("73b6a0ce6af52bb0f1b94112b94a72eb6925e01c7861b70be71609ed").ToPositiveBigInteger(),
                // public static this party
                new BitString("59bd2018603e4030af13c18cd10b9f007bba33711b09d2d904de7336511a89d788f1386bd17e8db9e092228758fa58e05edd7ff3c4f747807af478568b22c19fb66e641832716f2697fc17619c0ec3cfdeeab637adc6a08c34e4df5ffd11809149a039a5491dfa116f319b3c065198b0afa2fb8862390c0872f3f5364c1e0bc87bff058b91e09fada5687f25dac6013d193056ff9e112b4a94a3dab90084a9ab0b78002200403923a98427a1b28a5028d67e96b8cee4b6f894e23fb9b1bd153b9a1d42cc32592b12a3cccbfe7b6862f7464a8cb48c4c67afde886dfaf3c2aec2365a598e4bd36ce1741a8998916d9e385e9c43157a9862e4617dde032d8b8dd4").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                new BitString("6e1487efa42e42dcbf73f582cd844543b77f70b4cf3883fea8c6cd852a4293c7e5cb0586a6cd71294883b760cdbbfd07aedaa34d6e1f7915752c309c1cc20f5c05cce731ad9f49bb4480f603bc6697612002af9d27fdcd540981069d91faa2ed731e1638e12b3359596f9cb4c10e7d1ab5007aed2059e68409cd36d0b073b540dac979e369f995cd2f5a90bb043a16fa16dbee8481bb2c6d2018a87da2b4ae1fe0d6f7796d91d8dd6f48ba60ae72cac8eb9bf2ac73ab07d6f46b44ec8e45f7502d4c7a44177c6552c14202ebe73f014edb496dab1eb2df882dcdf2659daff69b6a33e98a8d41a3f3cc4f50a3d62723528feb49caa9571c4c74479a2ff77915ca"), 
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("6e5ccacd83781a5d36c146ce1c9ebf3beba3d29e1844e39130039374").ToPositiveBigInteger(), 
                // public static other party
                new BitString("65988d04b5a4c0fa13e2e26187583a6f392c86e74b77e22e284e4daf533f35887caaf0e607095263f955f65bc52d72d92af1114925f1bdaab68ac75b344891718cb7236c05726b28a37e959e710bd1c855af2c1f5aa03288a3981bf5cd80b27ab68e22f78f69401a1204dc6870a31766ac753d4ce98772bb511b11cab20d8d4dd810130fe6113d0a3952b1455aeab4478522611971811430d3da004ea38a5ad27cf07fdcf571a7670cc3bc5a904675744d9850f82ec0efc7711ff5865d8258d675d51ab3545f503181cc39a47520594822c4d4dc139e71084267e4c20de173fcafe928d8380af21e9372f28b89c158f0ed4567bb6d6428bf8ab49a2403681ede").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("3096e167964d6d2c7c463fb3a38de40389560f5c421cf3890db62d08").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("4a19abc15c6e7052605d378114ba85a699f658388a88af654587ee18fc917895011bf6eb9c43f18ade1b1d7127b3b7528a1d9e41de9b46bdb38ce0777ae268d82cfd9e52ad1b1bfa2f3cdef66097900e959a96dda09fb42172a8200ceeda5f154a941335fda014974cab11ba0eb5f1ebbf60be8e7ab1fffbb07d6e28f6dfb3f81d13b595037893baacdb21427dc644798ccd76c2d136a44270d3dea67d2bc26d6bd0f0fd346ca3021acc69f3a20b514a56d6f191acd0ba51c4b92b55b8eb9393349feafc1090e8a8efd518fa23cf7215a393b766f7d683bd07900be924ce24c9e46730b6b747a6e1276900f4e3e4b7e930d7db26445a1beb306e589fde7b2b40").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("6e601f20287b31d3d1cb3fbb28b06299a800c28df5fae37b0d3c97a22ee85a7e33418b3d1cab7b5c26638bcb36e88a0f371a4df20abbc73c61f7ba8f68c9510297bfd4dd12575f4ee424ef64387e1db764d764239becca7cba9936f44e06f09c54f6a72d586e7b1cf392874e9c6525205b72ea2cca58dbe5086d360bf1f3ceb874ca817b16533ee5a6c65b2448400e294a3268beea1ef9c6b34619db7de66d728303b0719e9da380e46da52028cf5047069b40933671e175f4aeac9886416f0be462cf904186689c2dc70bdc93f3d5c9e169d64e4e0e2ced60d83aacd395c35425e4a248655188cc0c756ebffa56a0e3901df8f9cf4ade38d9a98d2a66a74506"),
                // expected oi
                new BitString("a1b2c3d4e543415653696413e138a20a0fb550c9b26f7d6d8209d150814d"),
                // expected dkm
                new BitString("3cce1470dfd7adc27781ac74e986"),
                // expected macData
                new BitString("4b435f315f55a1b2c3d4e54341565369644a19abc15c6e7052605d378114ba85a699f658388a88af654587ee18fc917895011bf6eb9c43f18ade1b1d7127b3b7528a1d9e41de9b46bdb38ce0777ae268d82cfd9e52ad1b1bfa2f3cdef66097900e959a96dda09fb42172a8200ceeda5f154a941335fda014974cab11ba0eb5f1ebbf60be8e7ab1fffbb07d6e28f6dfb3f81d13b595037893baacdb21427dc644798ccd76c2d136a44270d3dea67d2bc26d6bd0f0fd346ca3021acc69f3a20b514a56d6f191acd0ba51c4b92b55b8eb9393349feafc1090e8a8efd518fa23cf7215a393b766f7d683bd07900be924ce24c9e46730b6b747a6e1276900f4e3e4b7e930d7db26445a1beb306e589fde7b2b406e1487efa42e42dcbf73f582cd844543b77f70b4cf3883fea8c6cd852a4293c7e5cb0586a6cd71294883b760cdbbfd07aedaa34d6e1f7915752c309c1cc20f5c05cce731ad9f49bb4480f603bc6697612002af9d27fdcd540981069d91faa2ed731e1638e12b3359596f9cb4c10e7d1ab5007aed2059e68409cd36d0b073b540dac979e369f995cd2f5a90bb043a16fa16dbee8481bb2c6d2018a87da2b4ae1fe0d6f7796d91d8dd6f48ba60ae72cac8eb9bf2ac73ab07d6f46b44ec8e45f7502d4c7a44177c6552c14202ebe73f014edb496dab1eb2df882dcdf2659daff69b6a33e98a8d41a3f3cc4f50a3d62723528feb49caa9571c4c74479a2ff77915ca"),
                // expected tag
                new BitString("270649bed27bed6f")
            },
            new object[]
            {
                // label
                "mqv1 hmac224 sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("85bba508c4b43e791c4816b4d7044eba4277b505ec6bd0d6033d7f576ebe9387b0a5534f48221f00c4ed85522eeb0e21e512feb3a0e5c45ec601af82d94f65dade907113a5858ca306d92253f5b7f75bdcf7cef3e028ae611e2ba541f080a7edeb487a81680f92502e050d6d57ee9c64ca00c134a777139fd45f82f28bb8c24e7adade99baa395b8c8d0567e493e4e728dfd356650ec6cb6ecf5b6dff477ad0d803f74053899788d3aac26034d00bb5ed81da2cee86c2dca9d416cb058c0fb79ff3390b234bee37d0c06b3b66313ff3dbbf5705bead0ebc5c9985182a8969ef14a016e23c05c90ae9c351fbdf9913d4cf7357f3994eedc725de27a9e81e82b97").ToPositiveBigInteger(),
                    // q
                    new BitString("984a6504d640038f9819c23652282aeb6bb7ec7ec22629c12ce93a45").ToPositiveBigInteger(), 
                    // g
                    new BitString("2f543534c508fcabce49d5ff56ef750da66df09286d6648338541a4a69e0cf340ae122df4695c2eb6b53e687d84840d56f61fd9c4491f624c7c9a1e5d16b9b6032d647b7f4ed31e0726ce91820e73bf0e58df3c3e940cb9bf1b09c396d8bfa5a68f253e1146ecbdf906871bbbb38ac8bad1aec1c4728a6bf6702ef01498af9904d8c7092bf0d333500b7f2c92f89f82bd689a38152ec7f04bee4aad000439f244bb4890389a020534130fd8fbf479a35e0e3da09538c890a379bfe37306feca78939ba19c7d8c567628971c5a0c26cbc2cd67a335299d036c929a906d6e9078682b36c6a1f770ec1c5afa11d903a324e99d65d3273f9dcd6df0b001c38f338a6").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.Mqv1,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D224,
                // Key confirmation role
                KeyConfirmationRole.Recipient,
                // Key confirmation direction
                KeyConfirmationDirection.Bilateral,
                // key length
                112,
                // tag length
                64,
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("9519208ba70d18615efc39b9605c21570254baa1e1218a00b73d7489").ToPositiveBigInteger(),
                // public static this party
                new BitString("0266c505a59cc98604a019b3cad86970695c8a257f2f865bd421016cf853769071f4f58c35910b829b8534676e2ea12adda935c97c326616bec13865278c2a48256dffe794798b39e614c88167b23389882ebedd45ec75d4c9b77ac552e3b52fbaeeb7cb36f5f9a4693c147968d3ba18070296204a988d0bb3a502c16f28561459c2f1a8e731f55eed58c496d584036f7c8afe4e352581d22d03a3032ef46ab9e44526ffa1d4c566168889494ac34f5b7759513a3a92fd2ca5366882ba108e98c080f5060ed3c378ff374430254f634c6eb3611db8238ebd8f28aaa74b4213899db0b80ad47ac0d0fc102a316e38a7e79acd857d00bdd4d5b32964ad7192e07c").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("0d506f7b3ad2aa2ee044650b5de99518e9fa04ef13aaa6f6ce05d181").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("80df83a01c5f220696783a1c1edab66238eb67674dd3fb614f6aa51d555c5886632c721149e872a094af916e21062dde0620041edb231d7e3e853f48e91f424faef2e52afd5c9e7ce53ca907d1b77f78282a46804ea8524ccf27f5e122390a5dd6bf3543b02cd794600367d6abaa3365e499903a2e07739a845b6b947ba9b5cdab834686f0f16c4c98ff495c1fd2135bede92b5e10e4a1d0891a35ed842a36b2a45edd58e31eafac094d0c4228c9600e4b7dc04702fb56c00b913a7ce34d4f2b69660eb0b634e792fabc9098324f73b8ab7d626b95f7246261eda515ca6b7d115081bb9e5fc7a67866b8b831548769d0bef6dda41f202dc74645d7f25edb0c92").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("0e3cbaf1daa0d5bd7a735a3a7292aba817793c7988708ab34cf383a5").ToPositiveBigInteger(), 
                // public static other party
                new BitString("3902c150cd01d5a3024b09a5cd4dd674703c7a87d2944a089689c9a9187baee639721ea6913cf81dd0b6bb8490f18897626996895230343a6dfc795b996865ed3f6ce5d6ec1ec66db06ac95edb801cb2e179631c22107928b2c7f6f0b57d65d3429102c750fe0283b99a0adea50dbc731b9a3e08cf1e567b91bb9457b4c79e889f91a92c5656c23f7da74beb70c263a66e283eb18e47b25b87348d46360bb2f67fa0dae3604299548648aa6380f049483c2758ffdf5155b6e9a85b1b7b40025c7af1763b08d7412b11d73c98c3de9eaba2e7aacdaa96bbae25bd1f2c7f47c7370ba0d205c0bd517bcc489fd4554c4964458151f54a204fd3cc5fce600f44734d").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                new BitString("8f8bcb5edf1ecf5efdaca7936697bb13ed2db5359a2aee3bb87e25352a517722fe458b5745fb0e2521d85fb61f098b38c7c47b3d72d16c625c4aa02ffa9f533f8edcf1a1f82f867dcfa576fbb0346e60d6fe9a7bebd4193f1ef9aeb17629b80566da6403a1c221b02f99542bc1204dd542627ab4ae3adb1a2715b68146f98cbaafc74b47e8bdc5056b3e610ef9d611dd327984b3630b9c3a9f2622681117b9a79029103374275cc3a81c066b015da0c1cec91e3fb15043e8adb2592e7f892514328a1a90ee08cd3311b9a90b80bde4c940dcb11f4010b86c7943c39b3959ba48bc71d125fd68009dcb9fb4b71fffc43cad3aa61cb953e30e2c60c877e78f5e8d"),
                // expected Z
                new BitString("067eaa6c8cef6e8a6a167839cc77c3148ea13595e930bf5ad3f449b8093d3a498e7f34c28cba650f92a9d3105618e05bba0b3627e7d17d5fbf8c1975c6078a9f64f3c76420da768f3d6d351da38f249e5989e9e354365a1c99b9b3c97408c7496702e5defb2476dce07527db48899fd61ec6e238d9aa3e102c972bfd2bfe20b80dc5a5ea59005959f8b163b9677d75aec1820d966b1c8bf986ca901ea7983dccfa456e01e471638086544695d9e0068e1dd2390981a20dd65f60308a29b75c1338ecb89a66d368260c607415c74966021c0c1261e73174b6f942b2c90cfb4c829974eecd4f7d00e1398d7cfeef1ae7dd2e897a7b08a7e82f8e6df4dd9a65012b"),
                // expected oi
                new BitString("a1b2c3d4e54341565369646736c8c4a9a036ae8a5b2f8fca3414ce80fb48"),
                // expected dkm
                new BitString("60d79cee554b9e526d586ae668a6"),
                // expected macData
                new BitString("4b435f325f56434156536964a1b2c3d4e58f8bcb5edf1ecf5efdaca7936697bb13ed2db5359a2aee3bb87e25352a517722fe458b5745fb0e2521d85fb61f098b38c7c47b3d72d16c625c4aa02ffa9f533f8edcf1a1f82f867dcfa576fbb0346e60d6fe9a7bebd4193f1ef9aeb17629b80566da6403a1c221b02f99542bc1204dd542627ab4ae3adb1a2715b68146f98cbaafc74b47e8bdc5056b3e610ef9d611dd327984b3630b9c3a9f2622681117b9a79029103374275cc3a81c066b015da0c1cec91e3fb15043e8adb2592e7f892514328a1a90ee08cd3311b9a90b80bde4c940dcb11f4010b86c7943c39b3959ba48bc71d125fd68009dcb9fb4b71fffc43cad3aa61cb953e30e2c60c877e78f5e8d80df83a01c5f220696783a1c1edab66238eb67674dd3fb614f6aa51d555c5886632c721149e872a094af916e21062dde0620041edb231d7e3e853f48e91f424faef2e52afd5c9e7ce53ca907d1b77f78282a46804ea8524ccf27f5e122390a5dd6bf3543b02cd794600367d6abaa3365e499903a2e07739a845b6b947ba9b5cdab834686f0f16c4c98ff495c1fd2135bede92b5e10e4a1d0891a35ed842a36b2a45edd58e31eafac094d0c4228c9600e4b7dc04702fb56c00b913a7ce34d4f2b69660eb0b634e792fabc9098324f73b8ab7d626b95f7246261eda515ca6b7d115081bb9e5fc7a67866b8b831548769d0bef6dda41f202dc74645d7f25edb0c92"),
                // expected tag
                new BitString("61e5614c99c3db71")
            },
            new object[]
            {
                // label
                "mqv1 hmac224 sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("85bba508c4b43e791c4816b4d7044eba4277b505ec6bd0d6033d7f576ebe9387b0a5534f48221f00c4ed85522eeb0e21e512feb3a0e5c45ec601af82d94f65dade907113a5858ca306d92253f5b7f75bdcf7cef3e028ae611e2ba541f080a7edeb487a81680f92502e050d6d57ee9c64ca00c134a777139fd45f82f28bb8c24e7adade99baa395b8c8d0567e493e4e728dfd356650ec6cb6ecf5b6dff477ad0d803f74053899788d3aac26034d00bb5ed81da2cee86c2dca9d416cb058c0fb79ff3390b234bee37d0c06b3b66313ff3dbbf5705bead0ebc5c9985182a8969ef14a016e23c05c90ae9c351fbdf9913d4cf7357f3994eedc725de27a9e81e82b97").ToPositiveBigInteger(),
                    // q
                    new BitString("984a6504d640038f9819c23652282aeb6bb7ec7ec22629c12ce93a45").ToPositiveBigInteger(), 
                    // g
                    new BitString("2f543534c508fcabce49d5ff56ef750da66df09286d6648338541a4a69e0cf340ae122df4695c2eb6b53e687d84840d56f61fd9c4491f624c7c9a1e5d16b9b6032d647b7f4ed31e0726ce91820e73bf0e58df3c3e940cb9bf1b09c396d8bfa5a68f253e1146ecbdf906871bbbb38ac8bad1aec1c4728a6bf6702ef01498af9904d8c7092bf0d333500b7f2c92f89f82bd689a38152ec7f04bee4aad000439f244bb4890389a020534130fd8fbf479a35e0e3da09538c890a379bfe37306feca78939ba19c7d8c567628971c5a0c26cbc2cd67a335299d036c929a906d6e9078682b36c6a1f770ec1c5afa11d903a324e99d65d3273f9dcd6df0b001c38f338a6").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.Mqv1,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D224,
                // Key confirmation role
                KeyConfirmationRole.Provider,
                // Key confirmation direction
                KeyConfirmationDirection.Bilateral,
                // key length
                112,
                // tag length
                64,
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("0e3cbaf1daa0d5bd7a735a3a7292aba817793c7988708ab34cf383a5").ToPositiveBigInteger(),
                // public static this party
                new BitString("3902c150cd01d5a3024b09a5cd4dd674703c7a87d2944a089689c9a9187baee639721ea6913cf81dd0b6bb8490f18897626996895230343a6dfc795b996865ed3f6ce5d6ec1ec66db06ac95edb801cb2e179631c22107928b2c7f6f0b57d65d3429102c750fe0283b99a0adea50dbc731b9a3e08cf1e567b91bb9457b4c79e889f91a92c5656c23f7da74beb70c263a66e283eb18e47b25b87348d46360bb2f67fa0dae3604299548648aa6380f049483c2758ffdf5155b6e9a85b1b7b40025c7af1763b08d7412b11d73c98c3de9eaba2e7aacdaa96bbae25bd1f2c7f47c7370ba0d205c0bd517bcc489fd4554c4964458151f54a204fd3cc5fce600f44734d").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                new BitString("8f8bcb5edf1ecf5efdaca7936697bb13ed2db5359a2aee3bb87e25352a517722fe458b5745fb0e2521d85fb61f098b38c7c47b3d72d16c625c4aa02ffa9f533f8edcf1a1f82f867dcfa576fbb0346e60d6fe9a7bebd4193f1ef9aeb17629b80566da6403a1c221b02f99542bc1204dd542627ab4ae3adb1a2715b68146f98cbaafc74b47e8bdc5056b3e610ef9d611dd327984b3630b9c3a9f2622681117b9a79029103374275cc3a81c066b015da0c1cec91e3fb15043e8adb2592e7f892514328a1a90ee08cd3311b9a90b80bde4c940dcb11f4010b86c7943c39b3959ba48bc71d125fd68009dcb9fb4b71fffc43cad3aa61cb953e30e2c60c877e78f5e8d"), 
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("9519208ba70d18615efc39b9605c21570254baa1e1218a00b73d7489").ToPositiveBigInteger(), 
                // public static other party
                new BitString("0266c505a59cc98604a019b3cad86970695c8a257f2f865bd421016cf853769071f4f58c35910b829b8534676e2ea12adda935c97c326616bec13865278c2a48256dffe794798b39e614c88167b23389882ebedd45ec75d4c9b77ac552e3b52fbaeeb7cb36f5f9a4693c147968d3ba18070296204a988d0bb3a502c16f28561459c2f1a8e731f55eed58c496d584036f7c8afe4e352581d22d03a3032ef46ab9e44526ffa1d4c566168889494ac34f5b7759513a3a92fd2ca5366882ba108e98c080f5060ed3c378ff374430254f634c6eb3611db8238ebd8f28aaa74b4213899db0b80ad47ac0d0fc102a316e38a7e79acd857d00bdd4d5b32964ad7192e07c").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("0d506f7b3ad2aa2ee044650b5de99518e9fa04ef13aaa6f6ce05d181").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("80df83a01c5f220696783a1c1edab66238eb67674dd3fb614f6aa51d555c5886632c721149e872a094af916e21062dde0620041edb231d7e3e853f48e91f424faef2e52afd5c9e7ce53ca907d1b77f78282a46804ea8524ccf27f5e122390a5dd6bf3543b02cd794600367d6abaa3365e499903a2e07739a845b6b947ba9b5cdab834686f0f16c4c98ff495c1fd2135bede92b5e10e4a1d0891a35ed842a36b2a45edd58e31eafac094d0c4228c9600e4b7dc04702fb56c00b913a7ce34d4f2b69660eb0b634e792fabc9098324f73b8ab7d626b95f7246261eda515ca6b7d115081bb9e5fc7a67866b8b831548769d0bef6dda41f202dc74645d7f25edb0c92").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("067eaa6c8cef6e8a6a167839cc77c3148ea13595e930bf5ad3f449b8093d3a498e7f34c28cba650f92a9d3105618e05bba0b3627e7d17d5fbf8c1975c6078a9f64f3c76420da768f3d6d351da38f249e5989e9e354365a1c99b9b3c97408c7496702e5defb2476dce07527db48899fd61ec6e238d9aa3e102c972bfd2bfe20b80dc5a5ea59005959f8b163b9677d75aec1820d966b1c8bf986ca901ea7983dccfa456e01e471638086544695d9e0068e1dd2390981a20dd65f60308a29b75c1338ecb89a66d368260c607415c74966021c0c1261e73174b6f942b2c90cfb4c829974eecd4f7d00e1398d7cfeef1ae7dd2e897a7b08a7e82f8e6df4dd9a65012b"),
                // expected oi
                new BitString("a1b2c3d4e54341565369646736c8c4a9a036ae8a5b2f8fca3414ce80fb48"),
                // expected dkm
                new BitString("60d79cee554b9e526d586ae668a6"),
                // expected macData
                new BitString("4b435f325f56434156536964a1b2c3d4e58f8bcb5edf1ecf5efdaca7936697bb13ed2db5359a2aee3bb87e25352a517722fe458b5745fb0e2521d85fb61f098b38c7c47b3d72d16c625c4aa02ffa9f533f8edcf1a1f82f867dcfa576fbb0346e60d6fe9a7bebd4193f1ef9aeb17629b80566da6403a1c221b02f99542bc1204dd542627ab4ae3adb1a2715b68146f98cbaafc74b47e8bdc5056b3e610ef9d611dd327984b3630b9c3a9f2622681117b9a79029103374275cc3a81c066b015da0c1cec91e3fb15043e8adb2592e7f892514328a1a90ee08cd3311b9a90b80bde4c940dcb11f4010b86c7943c39b3959ba48bc71d125fd68009dcb9fb4b71fffc43cad3aa61cb953e30e2c60c877e78f5e8d80df83a01c5f220696783a1c1edab66238eb67674dd3fb614f6aa51d555c5886632c721149e872a094af916e21062dde0620041edb231d7e3e853f48e91f424faef2e52afd5c9e7ce53ca907d1b77f78282a46804ea8524ccf27f5e122390a5dd6bf3543b02cd794600367d6abaa3365e499903a2e07739a845b6b947ba9b5cdab834686f0f16c4c98ff495c1fd2135bede92b5e10e4a1d0891a35ed842a36b2a45edd58e31eafac094d0c4228c9600e4b7dc04702fb56c00b913a7ce34d4f2b69660eb0b634e792fabc9098324f73b8ab7d626b95f7246261eda515ca6b7d115081bb9e5fc7a67866b8b831548769d0bef6dda41f202dc74645d7f25edb0c92"),
                // expected tag
                new BitString("61e5614c99c3db71")
            },
            #endregion hmac
            #region cmac
            new object[]
            {
                // label
                "mqv1 cmac sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d684e03c751fc12cbd59585836600df9a2618ad86b82d1a00a5aefba6a9482259bcac0aa7c4d7a532a6036376e4fe401a8d05a89837e9c2c0640af0bf889af57381895dbebf4ece9544d2317e5c1dd1eac8e69aa2aa5a83915955a08f536ccf7afe11a92a4ce4fee373e1469ee2e42d3404125d78de57103d440407e04be5aa42a8251ccd97517ea8549edd21d149fed4f8642b8d1b6af97b13f540650f19a67ba778a4c3403385b655701b8f906c752bfa9142948cf12a9378e44a1fd7e2a7eee2798d5814d2e08a6348da9f3c53c2055f3dd19e2bb9f3ce6fc955604cab74168f5669cc04e2e2756caa06d4f1cb2f4a8ab53b40672b669455e4234d7d312cb").ToPositiveBigInteger(),
                    // q
                    new BitString("e40c394eb7f7751824b1a016f2b5e9ac111c6be1ecc4d76ac4227e15").ToPositiveBigInteger(), 
                    // g
                    new BitString("7ba2f238010f845b20b1f5c2c94f6ce0775f47861549b755d88dbf747849deaf36a74fb29ecc9d2cddbea65024699366c494a5903eda91f186c2889eee738d120a117d2a2a840e2b3ed9a1c51ab001f2e424ef30c49f3d162f89143ac274521c728eb9699889c0463faece6681d663a9ed75be934de527df8463a1331a859cfa5d963fcdfb446e3c8e0ea8c5379c9c2505ea3fa22695a35a9001a3641ba10ef759f7516e1f171b7c414790818ff5bc3422ee0ec51f4f53f7e4ac288336dd749002e9e62678b2c80ec1d783ea1dcd674a9d7d5d050994c373d29d51f69b91c4f160e2de5f1c91448c48e71748106c0406598aadee3c4a4aba09b86fccf4d33e89").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.Mqv1,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.CmacAes,
                // Key confirmation role
                KeyConfirmationRole.Provider,
                // Key confirmation direction
                KeyConfirmationDirection.Unilateral,
                // key length
                128,
                // tag length
                128,
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("d25f344ff0d0c71b4ea918e06be44705c1b72659e43b948dc2540e90").ToPositiveBigInteger(),
                // public static this party
                new BitString("06376c179e00bdc8963508f7edecce3e81e0e353930c69953c7d85e398c33d5985008132e54852f92dc8ee0a935f21ad7ec7289a30c0b90f170f2b489645bb260f4ba24aac6cf07853f44f13b24105eda12fea3173b364819acb8183055dd4ed72b8b7ab05b497c27f50f78610e04ff9b08e1ef10d8af73a2df7e69b51c5c550d3cf41c8f81ea566f877755f2de7e7aefa2ec5fc568e75c16c04a751aa289e5d81ebeae85969522be8ce8e169c24ebf8d8335fd0f924b2d70c49012fb8e9ff9b8a1eaa6d87e946678779d7d45f6b9cd295dde59175c6f39f7cfcb77914bd0475009645822047317950a0f9cdfbd366b9c109fb9a0494a18feaf266243fe03ecd").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null, 
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("a71ab81a9b23864786d51793c9767f9147d486830c2893770b6333b9").ToPositiveBigInteger(), 
                // public static other party
                new BitString("026242c7fc58e1afbfae6b135305330ef0f65d2c266e61e60dab66d6269e9d01ba82b07107b5ffd286c84e23fc36c04df51add862faceca156c772b8d522384e82329079a15253911b2dfe7dbf076c382738359e22dc13c54c7a344a931553eafd58935c03ccc1627531f6f5c46989ebc59de7d75ea969e2f8f3093f5fe22d1eceeb06e1080058505e27b8e395c2dc6f4daef8f19d10503674bacef12a7145639aa56da7d1b9331b5fc274f0cfd1970438bbf4b9428e09e963c47f41b1cb1f600fb7a6f66182a568717cf58e1e21331dfd83278ea004877ed629b8f2f53d7339d777d29610fe4709678f765dc7e12e52cd3463147cf87a2aa601ed0fa530e3b1").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("4bb251a2ed4732b17d94b486d09aa7e2a9001a8b6b3260778987bc5e").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("b13450c04274a280dee69ace9597a9895f53ba7962aa64a56316590d5150dd4b1bfc95472a9d58b7b002e8ee708eda07faae935a15481fae90b08ef7b6e88e8487ee558f5829ab699e055cfb18351e7ddb15f57ae0e2c1a9e61793be1e8f1109b34d059a01bc959ad985e47c092e9fec5a938b24efe0a8e349628b82443563086925522c1d8f1c20a4b7fa49582874de48a70c9514718b6c41787e9c6e924180d19efcf3cd728526f95a4d185cbb7ef76dda971dd01bbcfec7b7ef982d9f1d95aeb7e1020f950ca25a3468c3254edfb9727fba9b3a0b1f848428f2276bb895cb2aff7153e1ec37aac009349d7e644ce11f27172fc9f661307b986034b544a516").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("74f8fc37495c61824e9eb621e7c6cada1f8bcc47ea4e8defca7b50d3e93289f5887df5b5bd887f2f92767fbb66b5c7b1fa73c9d5843ef9e8ea103ab2be644838cbc8fd42ba8583c59c7dac4f7a97e98e18808043b6c35ecc7e77a4bb88f2cc0b6536269ea30319304777efd19976ebc2fe7888ae0fb57946069f0e744c79eafc69adae08b2b3e0d8cf8aaf1bf08cd112c931daf371bb85fd1a2c7a5a97114354f91f32e3e696182903d4c808551fd89e6cfa3c71724358a9e46707d22bd54d35017cc5a014d8306f3ed2d86021096009ec2f097f92e69f9f868925823ceb948901fbc1ec4fdf5b69dbd1bca1b04c36d84d7c64db5da01483b07ecc4aa5ea9c6f"),
                // expected oi
                new BitString("434156536964a1b2c3d4e5c2ee059c2ea62b83b2b27d69a717bd9c78a647"),
                // expected dkm
                new BitString("daa34031dac9b5e937251bbcebd9ffc3"),
                // expected macData
                new BitString("4b435f315f56a1b2c3d4e5434156536964b13450c04274a280dee69ace9597a9895f53ba7962aa64a56316590d5150dd4b1bfc95472a9d58b7b002e8ee708eda07faae935a15481fae90b08ef7b6e88e8487ee558f5829ab699e055cfb18351e7ddb15f57ae0e2c1a9e61793be1e8f1109b34d059a01bc959ad985e47c092e9fec5a938b24efe0a8e349628b82443563086925522c1d8f1c20a4b7fa49582874de48a70c9514718b6c41787e9c6e924180d19efcf3cd728526f95a4d185cbb7ef76dda971dd01bbcfec7b7ef982d9f1d95aeb7e1020f950ca25a3468c3254edfb9727fba9b3a0b1f848428f2276bb895cb2aff7153e1ec37aac009349d7e644ce11f27172fc9f661307b986034b544a516"),
                // expected tag
                new BitString("88e8009536cc42c9276c3e09e6515200")
            },
            new object[]
            {
                // label
                "mqv1 cmac sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d684e03c751fc12cbd59585836600df9a2618ad86b82d1a00a5aefba6a9482259bcac0aa7c4d7a532a6036376e4fe401a8d05a89837e9c2c0640af0bf889af57381895dbebf4ece9544d2317e5c1dd1eac8e69aa2aa5a83915955a08f536ccf7afe11a92a4ce4fee373e1469ee2e42d3404125d78de57103d440407e04be5aa42a8251ccd97517ea8549edd21d149fed4f8642b8d1b6af97b13f540650f19a67ba778a4c3403385b655701b8f906c752bfa9142948cf12a9378e44a1fd7e2a7eee2798d5814d2e08a6348da9f3c53c2055f3dd19e2bb9f3ce6fc955604cab74168f5669cc04e2e2756caa06d4f1cb2f4a8ab53b40672b669455e4234d7d312cb").ToPositiveBigInteger(),
                    // q
                    new BitString("e40c394eb7f7751824b1a016f2b5e9ac111c6be1ecc4d76ac4227e15").ToPositiveBigInteger(), 
                    // g
                    new BitString("7ba2f238010f845b20b1f5c2c94f6ce0775f47861549b755d88dbf747849deaf36a74fb29ecc9d2cddbea65024699366c494a5903eda91f186c2889eee738d120a117d2a2a840e2b3ed9a1c51ab001f2e424ef30c49f3d162f89143ac274521c728eb9699889c0463faece6681d663a9ed75be934de527df8463a1331a859cfa5d963fcdfb446e3c8e0ea8c5379c9c2505ea3fa22695a35a9001a3641ba10ef759f7516e1f171b7c414790818ff5bc3422ee0ec51f4f53f7e4ac288336dd749002e9e62678b2c80ec1d783ea1dcd674a9d7d5d050994c373d29d51f69b91c4f160e2de5f1c91448c48e71748106c0406598aadee3c4a4aba09b86fccf4d33e89").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.Mqv1,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.CmacAes,
                // Key confirmation role
                KeyConfirmationRole.Recipient,
                // Key confirmation direction
                KeyConfirmationDirection.Unilateral,
                // key length
                128,
                // tag length
                128,
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("a71ab81a9b23864786d51793c9767f9147d486830c2893770b6333b9").ToPositiveBigInteger(),
                // public static this party
                new BitString("026242c7fc58e1afbfae6b135305330ef0f65d2c266e61e60dab66d6269e9d01ba82b07107b5ffd286c84e23fc36c04df51add862faceca156c772b8d522384e82329079a15253911b2dfe7dbf076c382738359e22dc13c54c7a344a931553eafd58935c03ccc1627531f6f5c46989ebc59de7d75ea969e2f8f3093f5fe22d1eceeb06e1080058505e27b8e395c2dc6f4daef8f19d10503674bacef12a7145639aa56da7d1b9331b5fc274f0cfd1970438bbf4b9428e09e963c47f41b1cb1f600fb7a6f66182a568717cf58e1e21331dfd83278ea004877ed629b8f2f53d7339d777d29610fe4709678f765dc7e12e52cd3463147cf87a2aa601ed0fa530e3b1").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("4bb251a2ed4732b17d94b486d09aa7e2a9001a8b6b3260778987bc5e").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("b13450c04274a280dee69ace9597a9895f53ba7962aa64a56316590d5150dd4b1bfc95472a9d58b7b002e8ee708eda07faae935a15481fae90b08ef7b6e88e8487ee558f5829ab699e055cfb18351e7ddb15f57ae0e2c1a9e61793be1e8f1109b34d059a01bc959ad985e47c092e9fec5a938b24efe0a8e349628b82443563086925522c1d8f1c20a4b7fa49582874de48a70c9514718b6c41787e9c6e924180d19efcf3cd728526f95a4d185cbb7ef76dda971dd01bbcfec7b7ef982d9f1d95aeb7e1020f950ca25a3468c3254edfb9727fba9b3a0b1f848428f2276bb895cb2aff7153e1ec37aac009349d7e644ce11f27172fc9f661307b986034b544a516").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null, 
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("d25f344ff0d0c71b4ea918e06be44705c1b72659e43b948dc2540e90").ToPositiveBigInteger(), 
                // public static other party
                new BitString("06376c179e00bdc8963508f7edecce3e81e0e353930c69953c7d85e398c33d5985008132e54852f92dc8ee0a935f21ad7ec7289a30c0b90f170f2b489645bb260f4ba24aac6cf07853f44f13b24105eda12fea3173b364819acb8183055dd4ed72b8b7ab05b497c27f50f78610e04ff9b08e1ef10d8af73a2df7e69b51c5c550d3cf41c8f81ea566f877755f2de7e7aefa2ec5fc568e75c16c04a751aa289e5d81ebeae85969522be8ce8e169c24ebf8d8335fd0f924b2d70c49012fb8e9ff9b8a1eaa6d87e946678779d7d45f6b9cd295dde59175c6f39f7cfcb77914bd0475009645822047317950a0f9cdfbd366b9c109fb9a0494a18feaf266243fe03ecd").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("74f8fc37495c61824e9eb621e7c6cada1f8bcc47ea4e8defca7b50d3e93289f5887df5b5bd887f2f92767fbb66b5c7b1fa73c9d5843ef9e8ea103ab2be644838cbc8fd42ba8583c59c7dac4f7a97e98e18808043b6c35ecc7e77a4bb88f2cc0b6536269ea30319304777efd19976ebc2fe7888ae0fb57946069f0e744c79eafc69adae08b2b3e0d8cf8aaf1bf08cd112c931daf371bb85fd1a2c7a5a97114354f91f32e3e696182903d4c808551fd89e6cfa3c71724358a9e46707d22bd54d35017cc5a014d8306f3ed2d86021096009ec2f097f92e69f9f868925823ceb948901fbc1ec4fdf5b69dbd1bca1b04c36d84d7c64db5da01483b07ecc4aa5ea9c6f"),
                // expected oi
                new BitString("434156536964a1b2c3d4e5c2ee059c2ea62b83b2b27d69a717bd9c78a647"),
                // expected dkm
                new BitString("daa34031dac9b5e937251bbcebd9ffc3"),
                // expected macData
                new BitString("4b435f315f56a1b2c3d4e5434156536964b13450c04274a280dee69ace9597a9895f53ba7962aa64a56316590d5150dd4b1bfc95472a9d58b7b002e8ee708eda07faae935a15481fae90b08ef7b6e88e8487ee558f5829ab699e055cfb18351e7ddb15f57ae0e2c1a9e61793be1e8f1109b34d059a01bc959ad985e47c092e9fec5a938b24efe0a8e349628b82443563086925522c1d8f1c20a4b7fa49582874de48a70c9514718b6c41787e9c6e924180d19efcf3cd728526f95a4d185cbb7ef76dda971dd01bbcfec7b7ef982d9f1d95aeb7e1020f950ca25a3468c3254edfb9727fba9b3a0b1f848428f2276bb895cb2aff7153e1ec37aac009349d7e644ce11f27172fc9f661307b986034b544a516"),
                // expected tag
                new BitString("88e8009536cc42c9276c3e09e6515200")
            },
            #endregion cmac
            #endregion mqv1

            #region dhOneFlow
            #region hmac
            new object[]
            {
                // label
                "dhOneFlow hmac sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d777e471a5f7d9efc83f0cd1e13707e6af1ab5f41a8c444e68a06e828f5fb4fd26603b2b4b952f45f26f5323b3718ba03b19529a499831169d8d87ad33eba23178b218c6a95b80326c8a278537919f0ef7a580ad8a32bb4e794d3f7f3972c0542e8a39bfe27792021c006e393a5b10009d270fb9cee13f3d051d107d4e7c39566f4c2fe1b5b1338ffeb3b45b9400ac9a4288cc04026533ce4ae4361fc7dc1d25b7d83c836687e5784a6ade7b4124cfe395224bdd14755ebb7011e10bf412c7c34e273f198301f835a1627b22d86cf29b843d4ca48e046faf9483c75c02d6f1d94bf4d17615ece13c2fd4fbfdcd6d7b169026fb60884e1b7ab6ce0fc6c370188d").ToPositiveBigInteger(),
                    // q
                    new BitString("801b6ddcd67434f4dae12e447de5a1e491c912e274a2ed1966eeb8ef").ToPositiveBigInteger(), 
                    // g
                    new BitString("7c78351afba6ee232c5b83bbb75de697d7d8c1954525285dcfb2e8218eca22113272e7abbce09128de4a485f665c378a02cd629a2c1b823f5e0f0e046bac806846452ea251b10ba2a74b147b45b346bcd7506c4294a23c2bce8a26e0744332f2893c9c5559aa109b05f249bc83f6bd57c3cab1b2493cb55cda2588809fd6165e931c6fb71387d5aec1150f7316d2a9d5d60d3cb1fba6fb15fd3eb7a7300939a6c5063a42c43a5adbf72720e4835352e7db16b6a0d7440d784031a73e9ef35d674a4676d5e3a54ee08303e1e6d3b52eeba1f5311e76e2ae2f63718d042e34615500955f1174caf2626a6bdbfe96ebc35b3bdb458d1a8f6c13b67ae821dd3a22cb").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhOneFlow,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D224,
                // Key confirmation role
                KeyConfirmationRole.Recipient,
                // Key confirmation direction
                KeyConfirmationDirection.Unilateral,
                // key length
                112,
                // tag length
                64,
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("00").ToPositiveBigInteger(),
                // public static this party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("51339958488bd28561c721793462ec4ff618a198b58d99a38d18c879").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("d0645d05b0275e094729f28b3b56c79addab90a2c4589209e74d136d9a354b511c739be4f1396ce84fae4450250169552ee18b11a7d9d0667bdfd2551c8188cde08a2e64c425809d0f191c66fe5d32d6ff155b4280ba29df28e21c95f1204edb75fc60b37bb5a22f58ca33a12981f7f4a9cf6d69df1b4ff3141a269a8df29691b8c04c73fc50dfbc28d245ff021cf2840cea667e60948441a1539cbf3b7e8a4bec0ac3ae43fdbd0dab65e7d3fee96142d91f93963e8d6fe4b73e3fe7459348fdb5d428a25a7b60f5d07e4224fa90f0543597e708607a5f947cd2d989b3d6cc5c48963cebe7c9554d2bbbae4d61b43d41106cb2c0157601c49233c29529ca4356").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("616e7e515429f0ccfae9e6d0a1f7b8763124048b9fe5052caa9e71e1").ToPositiveBigInteger(), 
                // public static other party
                new BitString("1b51c9a907522d2f77b0c60b6dec0806bed1fd25425cafa8713e6519539590befe0be68b1d2a50fb2b27eb29ba55fe01b3a143bcade734a5243fcb5c739aaa2cfb2d4af72df2e692b5a9d247ddaf9ad4adeb90c4ba7dbaebea38bab46323fd1e17dfe41f97bba894b078dd868d3568607fe55bcc71f2003bd2f22ff0c42bc85907259eb9546d50a1b5cc1a768761010a496193e922b1b2bde0f08b37d6181a653679082f139a73b19a484aff6e701c70170e62c25307a3331944fea5d7badd1dd3445da608bd5b2b70a8df07a59b6d96138f9eee83f686e2bb85469ef8a57ec6f6e68094358d3534267a6d485805d10368411673f35c40cd30e668f6fa70afca").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("ca5d02cac48b56dfafbf02355522c76fca2145fc44a85a22399e6e0480caba0e430507527230157a85828159d05c92d78953ff636ff429c033053126df52a36b40fa2474b37bfbb067725eb092e81b0afed9283efa2cf8ea7836f1d19b127e5fa89b9b43683fdf41f5c6ca0aad33aa26ca2d2d697bff5983ad0e4da8533504221dd6c629b96a6bbfef1143c492660f3b05c37c7517b4c9d9abd4d28bc98aeacf6b3a5b193989073832a680d3c77f368446ca60da3ab3a194650582b7e3d724816fb47742eb99f1dc42991124d6198bd117da51fccfce37b813a710b8353663f4dd6e9282aaaeb5a444dff8b03c501880479875052a0c9080e490b638c383de8e"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964c46df6844fbc7ae3ac7135635a223569778134"),
                // expected dkm
                new BitString("7cf6a3699e6ed7269e35064a71d3"),
                // expected macData
                new BitString("4b435f315f56434156536964a1b2c3d4e5d0645d05b0275e094729f28b3b56c79addab90a2c4589209e74d136d9a354b511c739be4f1396ce84fae4450250169552ee18b11a7d9d0667bdfd2551c8188cde08a2e64c425809d0f191c66fe5d32d6ff155b4280ba29df28e21c95f1204edb75fc60b37bb5a22f58ca33a12981f7f4a9cf6d69df1b4ff3141a269a8df29691b8c04c73fc50dfbc28d245ff021cf2840cea667e60948441a1539cbf3b7e8a4bec0ac3ae43fdbd0dab65e7d3fee96142d91f93963e8d6fe4b73e3fe7459348fdb5d428a25a7b60f5d07e4224fa90f0543597e708607a5f947cd2d989b3d6cc5c48963cebe7c9554d2bbbae4d61b43d41106cb2c0157601c49233c29529ca4356"),
                // expected tag
                new BitString("a726074b3e719d8b")
            },
            new object[]
            {
                // label
                "dhOneFlow hmac sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d777e471a5f7d9efc83f0cd1e13707e6af1ab5f41a8c444e68a06e828f5fb4fd26603b2b4b952f45f26f5323b3718ba03b19529a499831169d8d87ad33eba23178b218c6a95b80326c8a278537919f0ef7a580ad8a32bb4e794d3f7f3972c0542e8a39bfe27792021c006e393a5b10009d270fb9cee13f3d051d107d4e7c39566f4c2fe1b5b1338ffeb3b45b9400ac9a4288cc04026533ce4ae4361fc7dc1d25b7d83c836687e5784a6ade7b4124cfe395224bdd14755ebb7011e10bf412c7c34e273f198301f835a1627b22d86cf29b843d4ca48e046faf9483c75c02d6f1d94bf4d17615ece13c2fd4fbfdcd6d7b169026fb60884e1b7ab6ce0fc6c370188d").ToPositiveBigInteger(),
                    // q
                    new BitString("801b6ddcd67434f4dae12e447de5a1e491c912e274a2ed1966eeb8ef").ToPositiveBigInteger(), 
                    // g
                    new BitString("7c78351afba6ee232c5b83bbb75de697d7d8c1954525285dcfb2e8218eca22113272e7abbce09128de4a485f665c378a02cd629a2c1b823f5e0f0e046bac806846452ea251b10ba2a74b147b45b346bcd7506c4294a23c2bce8a26e0744332f2893c9c5559aa109b05f249bc83f6bd57c3cab1b2493cb55cda2588809fd6165e931c6fb71387d5aec1150f7316d2a9d5d60d3cb1fba6fb15fd3eb7a7300939a6c5063a42c43a5adbf72720e4835352e7db16b6a0d7440d784031a73e9ef35d674a4676d5e3a54ee08303e1e6d3b52eeba1f5311e76e2ae2f63718d042e34615500955f1174caf2626a6bdbfe96ebc35b3bdb458d1a8f6c13b67ae821dd3a22cb").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhOneFlow,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D224,
                // Key confirmation role
                KeyConfirmationRole.Provider,
                // Key confirmation direction
                KeyConfirmationDirection.Unilateral,
                // key length
                112,
                // tag length
                64,
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("616e7e515429f0ccfae9e6d0a1f7b8763124048b9fe5052caa9e71e1").ToPositiveBigInteger(),
                // public static this party
                new BitString("1b51c9a907522d2f77b0c60b6dec0806bed1fd25425cafa8713e6519539590befe0be68b1d2a50fb2b27eb29ba55fe01b3a143bcade734a5243fcb5c739aaa2cfb2d4af72df2e692b5a9d247ddaf9ad4adeb90c4ba7dbaebea38bab46323fd1e17dfe41f97bba894b078dd868d3568607fe55bcc71f2003bd2f22ff0c42bc85907259eb9546d50a1b5cc1a768761010a496193e922b1b2bde0f08b37d6181a653679082f139a73b19a484aff6e701c70170e62c25307a3331944fea5d7badd1dd3445da608bd5b2b70a8df07a59b6d96138f9eee83f686e2bb85469ef8a57ec6f6e68094358d3534267a6d485805d10368411673f35c40cd30e668f6fa70afca").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("00").ToPositiveBigInteger(), 
                // public static other party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("51339958488bd28561c721793462ec4ff618a198b58d99a38d18c879").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("d0645d05b0275e094729f28b3b56c79addab90a2c4589209e74d136d9a354b511c739be4f1396ce84fae4450250169552ee18b11a7d9d0667bdfd2551c8188cde08a2e64c425809d0f191c66fe5d32d6ff155b4280ba29df28e21c95f1204edb75fc60b37bb5a22f58ca33a12981f7f4a9cf6d69df1b4ff3141a269a8df29691b8c04c73fc50dfbc28d245ff021cf2840cea667e60948441a1539cbf3b7e8a4bec0ac3ae43fdbd0dab65e7d3fee96142d91f93963e8d6fe4b73e3fe7459348fdb5d428a25a7b60f5d07e4224fa90f0543597e708607a5f947cd2d989b3d6cc5c48963cebe7c9554d2bbbae4d61b43d41106cb2c0157601c49233c29529ca4356").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("ca5d02cac48b56dfafbf02355522c76fca2145fc44a85a22399e6e0480caba0e430507527230157a85828159d05c92d78953ff636ff429c033053126df52a36b40fa2474b37bfbb067725eb092e81b0afed9283efa2cf8ea7836f1d19b127e5fa89b9b43683fdf41f5c6ca0aad33aa26ca2d2d697bff5983ad0e4da8533504221dd6c629b96a6bbfef1143c492660f3b05c37c7517b4c9d9abd4d28bc98aeacf6b3a5b193989073832a680d3c77f368446ca60da3ab3a194650582b7e3d724816fb47742eb99f1dc42991124d6198bd117da51fccfce37b813a710b8353663f4dd6e9282aaaeb5a444dff8b03c501880479875052a0c9080e490b638c383de8e"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964c46df6844fbc7ae3ac7135635a223569778134"),
                // expected dkm
                new BitString("7cf6a3699e6ed7269e35064a71d3"),
                // expected macData
                new BitString("4b435f315f56434156536964a1b2c3d4e5d0645d05b0275e094729f28b3b56c79addab90a2c4589209e74d136d9a354b511c739be4f1396ce84fae4450250169552ee18b11a7d9d0667bdfd2551c8188cde08a2e64c425809d0f191c66fe5d32d6ff155b4280ba29df28e21c95f1204edb75fc60b37bb5a22f58ca33a12981f7f4a9cf6d69df1b4ff3141a269a8df29691b8c04c73fc50dfbc28d245ff021cf2840cea667e60948441a1539cbf3b7e8a4bec0ac3ae43fdbd0dab65e7d3fee96142d91f93963e8d6fe4b73e3fe7459348fdb5d428a25a7b60f5d07e4224fa90f0543597e708607a5f947cd2d989b3d6cc5c48963cebe7c9554d2bbbae4d61b43d41106cb2c0157601c49233c29529ca4356"),
                // expected tag
                new BitString("a726074b3e719d8b")
            },
            #endregion hmac
            
            #endregion dhOneFlow

            #region dhStatic
            #region hmac
            new object[]
            {
                // label
                "dhStatic hmac224 sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("c60dc06b8b8420743720e16fef08882a033a42c223bad697d1f56ccee3c0fe222220d8135350bfe1774316cb28fbb72885baa0134bee49c8521ccf97d54b60116564c6eeeb746d9730564181148298014551837183e8fabd66d37b9a5f07cd14c9c3ea75ce0cd5715acebe13313516c280d4110503eaa5dea4510e264da649de54b61a9f55370f1a11391bd8b076285c5b45208966de507f9b6001efa7f443fb8eb4b81128324e67d1a7ccb71596bb953a1ddcc8bf567e5df2151a1959fb5c58055ed7815dc9b0644111914e9152cb48d602c6c110f675621fae32b439c9a164d007c692f2960ac8723e81e1f55eff3fadd651916915d26f23d41116bd824637").ToPositiveBigInteger(),
                    // q
                    new BitString("82d514e0cd3611f40a6d428d788b4f11cc2065052ea7d1db02497b95").ToPositiveBigInteger(), 
                    // g
                    new BitString("3737f0812f25c65bd7e1e7837213e9cd21f1b60e10f0bc80239d60be25b89a0f0066f68a28b8a0eb875f82f7a49658f6b8b244744001599c115d3b17aaff184957fa9eca76a277c3f08642761abffbe2164238494c56228b23f7e18f84553ca4dfb7cf267e95a2b34003c551b193f8318b6bd3577f79279fb85ae7853ee86580af854ef279e143fe8309ac9f401942d2037c0a1f60cffee28129f12f7afcc9a1ff49654101093914f0c81318ab776a544f4d128eeb4be339f47bb427a719ecd85fa08ebadfd611f3f85462eefd70c164074ad284633120b7dd6e6de6d8582e4fab867acef5801dbae83fcdd5e1fd7ff1881d1ac620bf934b0cb886e429aacc43").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhStatic,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D224,
                // Key confirmation role
                KeyConfirmationRole.Provider,
                // Key confirmation direction
                KeyConfirmationDirection.Bilateral,
                // key length
                112,
                // tag length
                64,
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("0b4233e6a119f82ce165e22b3d068ada253429fab41f8ee4a69d51dc").ToPositiveBigInteger(),
                // public static this party
                new BitString("c0f6dfd92837a4fd74e33177f509361b5736599e18d80274993fc21fde6f41793f228314d71dde22a6e1f72b3041757c615d45ed56832c7399ecd23e6b0df02e5ade4ce22d70425c74a35d5641af3023f46e148c78f1d51d6410a2fb4ef7654e851d51ee24108b17068b516c5f548aec29105228f1b1ae0cee6f5b830f4e103fe39c434bc7ad1b9c6c1919101b2fdce36983ad6daca68bb5bd25db1bf79a0929703bf8019093bd299913d5883faecaf2c99de78cd42d6881c3061c6aa583a1d7c3a6ba401d54d0f0edcc897590de75200a601c29a954cf8790526df79d750214c1d54fa1a5804f175f240d3d1d06d087644aadff81fce370d55ee97f29a414f4").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                new BitString("9afd538159dba31263c604c40bd5"),
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("61a4b5421a4bfe392a79171642680eb073cb6f0730f437ba7f074f7e").ToPositiveBigInteger(), 
                // public static other party
                new BitString("bb2d9413137a77e0ce948508541b6a1ee30c186a68b1a98ece69004d722fd429e1f8036a181877a669f6e6033a241060d1006fa676f7227bd8813f5b027913d3748fd061216c02501673f4cb07be6ce43e2e2f769730b96565833834b284cc1853ddddd5af7b7a8c0e8d619c7d4ca5ce8002f377ccf9c9a0076371411783e452089005a582f155d54e95b6232d752e38076f9915a60f022bb8746eea91a8994fba87b0b2e821181a08a799d0f6921c9b89ce97bcf401d78df33d9ac0a1c3225f939de2bbf6ba93523d60d3828e0346b03a99021db50b9192d43a0705f9c94d09f4fdcf48dcd2b82b642780b3b95d5d57fb73740840813ee583914fa6541c1298").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                new BitString("ea7b68eee90677f96400870ba420c8aae542f3b55a5c76f872b4d1fe5a504026c3ae9af9e5b679bbc3ed9b40b1eb8802140f75706a7169888a189ea72c6e2aa0abf0de0c0a2a6fe236e3d3488a95eea898206e0527bef490783a7476ce5ddefd2caafecf614ba195424af9c8370442c4f9dba61f992c5e38c5811b134707a464ce42c0e9f1ff56fd718ad36bbe22cc80bdaae564c9a1f1aaf8545a259f52c3fd1821ed03c22fd7424a0b2ad629d5d3026ef4f27cbe06f30b991dfa54de2885f192af4dc4ddc46d8b5536c5b48007a172932096108044ad863043c44a0d703068c454a7d549a45e001b716eabc99f7cf9b59899c818416740462e7dac34120d89"),
                // expected Z
                new BitString("8ca3791ef965eb80f9812b8992c69e1f04042c367934731f9df277a69235f63181aadbba2d683ef8f6e930ffc0bc935f1a6173bfb7d62fc00587e8f6c7b1342313c7459e729d87195345abde542829b705001e413a36420f710b8628d1f7003193d37d90448a693dbabbdc02747ee9d51bc7584ab65d4d37650dc0c2da0350f047546db8deaf448b31d0b9bcf422c668aba142c4235e7ac6d49316edcf8f274904ca62c49d7a94b6e7003b18401b1aa3ace4f5d9d640c506da3f7a1f913d6dda75c81e7c5aa6912af173d1fd88fb59f8f60670f1e8651b8d687b4a6cba161c331590acc98826ab2f0a0f3a25eddb04cb15a572ea9a35db3c02e21f78a0d2a061"),
                // expected oi
                new BitString("a1b2c3d4e59afd538159dba31263c604c40bd5434156536964d05684ab56"),
                // expected dkm
                new BitString("e14e32dd55560ca3534f5eec9bdb"),
                // expected macData
                new BitString("4b435f325f55a1b2c3d4e54341565369649afd538159dba31263c604c40bd5ea7b68eee90677f96400870ba420c8aae542f3b55a5c76f872b4d1fe5a504026c3ae9af9e5b679bbc3ed9b40b1eb8802140f75706a7169888a189ea72c6e2aa0abf0de0c0a2a6fe236e3d3488a95eea898206e0527bef490783a7476ce5ddefd2caafecf614ba195424af9c8370442c4f9dba61f992c5e38c5811b134707a464ce42c0e9f1ff56fd718ad36bbe22cc80bdaae564c9a1f1aaf8545a259f52c3fd1821ed03c22fd7424a0b2ad629d5d3026ef4f27cbe06f30b991dfa54de2885f192af4dc4ddc46d8b5536c5b48007a172932096108044ad863043c44a0d703068c454a7d549a45e001b716eabc99f7cf9b59899c818416740462e7dac34120d89"),
                // expected tag
                new BitString("e3e6a856a5496419")
            },
            new object[]
            {
                // label
                "dhStatic hmac224 sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("c60dc06b8b8420743720e16fef08882a033a42c223bad697d1f56ccee3c0fe222220d8135350bfe1774316cb28fbb72885baa0134bee49c8521ccf97d54b60116564c6eeeb746d9730564181148298014551837183e8fabd66d37b9a5f07cd14c9c3ea75ce0cd5715acebe13313516c280d4110503eaa5dea4510e264da649de54b61a9f55370f1a11391bd8b076285c5b45208966de507f9b6001efa7f443fb8eb4b81128324e67d1a7ccb71596bb953a1ddcc8bf567e5df2151a1959fb5c58055ed7815dc9b0644111914e9152cb48d602c6c110f675621fae32b439c9a164d007c692f2960ac8723e81e1f55eff3fadd651916915d26f23d41116bd824637").ToPositiveBigInteger(),
                    // q
                    new BitString("82d514e0cd3611f40a6d428d788b4f11cc2065052ea7d1db02497b95").ToPositiveBigInteger(), 
                    // g
                    new BitString("3737f0812f25c65bd7e1e7837213e9cd21f1b60e10f0bc80239d60be25b89a0f0066f68a28b8a0eb875f82f7a49658f6b8b244744001599c115d3b17aaff184957fa9eca76a277c3f08642761abffbe2164238494c56228b23f7e18f84553ca4dfb7cf267e95a2b34003c551b193f8318b6bd3577f79279fb85ae7853ee86580af854ef279e143fe8309ac9f401942d2037c0a1f60cffee28129f12f7afcc9a1ff49654101093914f0c81318ab776a544f4d128eeb4be339f47bb427a719ecd85fa08ebadfd611f3f85462eefd70c164074ad284633120b7dd6e6de6d8582e4fab867acef5801dbae83fcdd5e1fd7ff1881d1ac620bf934b0cb886e429aacc43").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhStatic,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D224,
                // Key confirmation role
                KeyConfirmationRole.Recipient,
                // Key confirmation direction
                KeyConfirmationDirection.Bilateral,
                // key length
                112,
                // tag length
                64,
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("61a4b5421a4bfe392a79171642680eb073cb6f0730f437ba7f074f7e").ToPositiveBigInteger(),
                // public static this party
                new BitString("bb2d9413137a77e0ce948508541b6a1ee30c186a68b1a98ece69004d722fd429e1f8036a181877a669f6e6033a241060d1006fa676f7227bd8813f5b027913d3748fd061216c02501673f4cb07be6ce43e2e2f769730b96565833834b284cc1853ddddd5af7b7a8c0e8d619c7d4ca5ce8002f377ccf9c9a0076371411783e452089005a582f155d54e95b6232d752e38076f9915a60f022bb8746eea91a8994fba87b0b2e821181a08a799d0f6921c9b89ce97bcf401d78df33d9ac0a1c3225f939de2bbf6ba93523d60d3828e0346b03a99021db50b9192d43a0705f9c94d09f4fdcf48dcd2b82b642780b3b95d5d57fb73740840813ee583914fa6541c1298").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                new BitString("ea7b68eee90677f96400870ba420c8aae542f3b55a5c76f872b4d1fe5a504026c3ae9af9e5b679bbc3ed9b40b1eb8802140f75706a7169888a189ea72c6e2aa0abf0de0c0a2a6fe236e3d3488a95eea898206e0527bef490783a7476ce5ddefd2caafecf614ba195424af9c8370442c4f9dba61f992c5e38c5811b134707a464ce42c0e9f1ff56fd718ad36bbe22cc80bdaae564c9a1f1aaf8545a259f52c3fd1821ed03c22fd7424a0b2ad629d5d3026ef4f27cbe06f30b991dfa54de2885f192af4dc4ddc46d8b5536c5b48007a172932096108044ad863043c44a0d703068c454a7d549a45e001b716eabc99f7cf9b59899c818416740462e7dac34120d89"),
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("0b4233e6a119f82ce165e22b3d068ada253429fab41f8ee4a69d51dc").ToPositiveBigInteger(), 
                // public static other party
                new BitString("c0f6dfd92837a4fd74e33177f509361b5736599e18d80274993fc21fde6f41793f228314d71dde22a6e1f72b3041757c615d45ed56832c7399ecd23e6b0df02e5ade4ce22d70425c74a35d5641af3023f46e148c78f1d51d6410a2fb4ef7654e851d51ee24108b17068b516c5f548aec29105228f1b1ae0cee6f5b830f4e103fe39c434bc7ad1b9c6c1919101b2fdce36983ad6daca68bb5bd25db1bf79a0929703bf8019093bd299913d5883faecaf2c99de78cd42d6881c3061c6aa583a1d7c3a6ba401d54d0f0edcc897590de75200a601c29a954cf8790526df79d750214c1d54fa1a5804f175f240d3d1d06d087644aadff81fce370d55ee97f29a414f4").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                new BitString("9afd538159dba31263c604c40bd5"),
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("8ca3791ef965eb80f9812b8992c69e1f04042c367934731f9df277a69235f63181aadbba2d683ef8f6e930ffc0bc935f1a6173bfb7d62fc00587e8f6c7b1342313c7459e729d87195345abde542829b705001e413a36420f710b8628d1f7003193d37d90448a693dbabbdc02747ee9d51bc7584ab65d4d37650dc0c2da0350f047546db8deaf448b31d0b9bcf422c668aba142c4235e7ac6d49316edcf8f274904ca62c49d7a94b6e7003b18401b1aa3ace4f5d9d640c506da3f7a1f913d6dda75c81e7c5aa6912af173d1fd88fb59f8f60670f1e8651b8d687b4a6cba161c331590acc98826ab2f0a0f3a25eddb04cb15a572ea9a35db3c02e21f78a0d2a061"),
                // expected oi
                new BitString("a1b2c3d4e59afd538159dba31263c604c40bd5434156536964d05684ab56"),
                // expected dkm
                new BitString("e14e32dd55560ca3534f5eec9bdb"),
                // expected macData
                new BitString("4b435f325f55a1b2c3d4e54341565369649afd538159dba31263c604c40bd5ea7b68eee90677f96400870ba420c8aae542f3b55a5c76f872b4d1fe5a504026c3ae9af9e5b679bbc3ed9b40b1eb8802140f75706a7169888a189ea72c6e2aa0abf0de0c0a2a6fe236e3d3488a95eea898206e0527bef490783a7476ce5ddefd2caafecf614ba195424af9c8370442c4f9dba61f992c5e38c5811b134707a464ce42c0e9f1ff56fd718ad36bbe22cc80bdaae564c9a1f1aaf8545a259f52c3fd1821ed03c22fd7424a0b2ad629d5d3026ef4f27cbe06f30b991dfa54de2885f192af4dc4ddc46d8b5536c5b48007a172932096108044ad863043c44a0d703068c454a7d549a45e001b716eabc99f7cf9b59899c818416740462e7dac34120d89"),
                // expected tag
                new BitString("e3e6a856a5496419")
            },
            #endregion hmac

            #endregion dhStatic
        };

        [Test]
        [TestCaseSource(nameof(_test_keyConfirmation))]
        public void ShouldKeyConfirmationCorrectly(
            string label,
            FfcDomainParameters domainParameters,
            FfcScheme scheme,
            KeyAgreementRole keyAgreementRole,
            ModeValues dsaKdfHashMode,
            DigestSizes dsaKdfDigestSize,
            KeyAgreementMacType macType,
            KeyConfirmationRole keyConfirmationRole,
            KeyConfirmationDirection keyConfirmationDirection,
            int keyLength,
            int tagLength,
            BitString aesCcmNonce,
            BitString thisPartyId,
            BigInteger thisPartyPrivateStaticKey,
            BigInteger thisPartyPublicStaticKey,
            BigInteger thisPartyPrivateEphemKey,
            BigInteger thisPartyPublicEphemKey,
            BitString thisPartyDkmNonce,
            BitString thisPartyEphemeralNonce,
            BitString otherPartyId,
            BigInteger otherPartyPrivateStaticKey,
            BigInteger otherPartyPublicStaticKey,
            BigInteger otherPartyPrivateEphemKey,
            BigInteger otherPartyPublicEphemKey,
            BitString otherPartyDkmNonce,
            BitString otherPartyEphemeralNonce,
            BitString expectedZ,
            BitString expectedOi,
            BitString expectedDkm,
            BitString expectedMacData,
            BitString expectedTag
        )
        {
            KasEnumMapping.FfcMap.TryFirst(x => x.Key == scheme, out var schemeResult);
            var kasScheme = schemeResult.Value;

            var kdfParameter = new KdfParameterOneStep()
            {
                L = keyLength,
                AuxFunction = GetAuxFunction(dsaKdfHashMode, dsaKdfDigestSize)
            };

            var fixedInfo = new Mock<IFixedInfo>();
            fixedInfo
                .Setup(s => s.Get(It.IsAny<FixedInfoParameter>()))
                .Returns(expectedOi);
            var fixedInfoFactory = new Mock<IFixedInfoFactory>();
            fixedInfoFactory.Setup(s => s.Get()).Returns(fixedInfo.Object);

            var macParams = _macParamsBuilder
                .WithKeyAgreementMacType(macType)
                .WithKeyLength(keyLength)
                .WithMacLength(tagLength)
                .WithNonce(aesCcmNonce)
                .Build();

            FfcKeyPair thisPartyEphemKey = GetKey(thisPartyPublicEphemKey, thisPartyPrivateEphemKey);
            FfcKeyPair thisPartyStaticKey = GetKey(thisPartyPublicStaticKey, thisPartyPrivateStaticKey);

            FfcKeyPair otherPartyEphemKey = GetKey(otherPartyPublicEphemKey, otherPartyPrivateEphemKey); ;
            FfcKeyPair otherPartyStaticKey = GetKey(otherPartyPublicStaticKey, otherPartyPrivateStaticKey);

            var secretKeyingMaterialThisParty = _secretKeyingMaterialBuilderThisParty
                .WithDkmNonce(thisPartyDkmNonce)
                .WithDomainParameters(domainParameters)
                .WithEphemeralKey(thisPartyEphemKey)
                .WithEphemeralNonce(thisPartyEphemeralNonce)
                .WithPartyId(thisPartyId)
                .WithStaticKey(thisPartyStaticKey)
                .Build(kasScheme, KasMode.KdfKc, keyAgreementRole, keyConfirmationRole, keyConfirmationDirection);
            var secretKeyingMaterialOtherParty = _secretKeyingMaterialBuilderOtherParty
                .WithDkmNonce(otherPartyDkmNonce)
                .WithDomainParameters(domainParameters)
                .WithEphemeralKey(otherPartyEphemKey)
                .WithEphemeralNonce(otherPartyEphemeralNonce)
                .WithPartyId(otherPartyId)
                .WithStaticKey(otherPartyStaticKey)
                .Build(kasScheme, KasMode.KdfKc,
                    KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(keyAgreementRole),
                    KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(keyConfirmationRole), keyConfirmationDirection);

            _schemeBuilder
                .WithKdf(_kdfFactory, kdfParameter)
                .WithFixedInfo(fixedInfoFactory.Object, new FixedInfoParameter())
                .WithKeyConfirmation(_keyConfirmationFactory, macParams)
                .WithThisPartyKeyingMaterial(secretKeyingMaterialThisParty)
                .WithSchemeParameters(new SchemeParameters(new KasAlgoAttributes(kasScheme), keyAgreementRole,
                    KasMode.KdfKc, keyConfirmationRole, keyConfirmationDirection, KasAssurance.None, thisPartyId));

            var kas = _subject
                .WithSchemeBuilder(_schemeBuilder)
                .Build();

            var result = kas.ComputeResult(secretKeyingMaterialOtherParty);

            Assert.That(result.Z, Is.EqualTo(expectedZ), nameof(result.Z));
            //Assert.AreEqual(expectedDkm, result.Dkm, nameof(result.Dkm));
            Assert.That(result.MacData, Is.EqualTo(expectedMacData), nameof(result.MacData));
            Assert.That(result.Tag, Is.EqualTo(expectedTag), nameof(result.Tag));
        }

        private KdaOneStepAuxFunction GetAuxFunction(ModeValues dsaKdfHashMode, DigestSizes dsaKdfDigestSize)
        {
            List<(ModeValues hashMode, DigestSizes digestSize, KdaOneStepAuxFunction function)> list =
                new List<(ModeValues hashMode, DigestSizes digestSize, KdaOneStepAuxFunction function)>()
                {
                    (ModeValues.SHA2, DigestSizes.d224, KdaOneStepAuxFunction.SHA2_D224),
                    (ModeValues.SHA2, DigestSizes.d256, KdaOneStepAuxFunction.SHA2_D256),
                    (ModeValues.SHA2, DigestSizes.d384, KdaOneStepAuxFunction.SHA2_D384),
                    (ModeValues.SHA2, DigestSizes.d512, KdaOneStepAuxFunction.SHA2_D512),
                    (ModeValues.SHA3, DigestSizes.d224, KdaOneStepAuxFunction.SHA3_D224),
                    (ModeValues.SHA3, DigestSizes.d256, KdaOneStepAuxFunction.SHA3_D256),
                    (ModeValues.SHA3, DigestSizes.d384, KdaOneStepAuxFunction.SHA3_D384),
                    (ModeValues.SHA3, DigestSizes.d512, KdaOneStepAuxFunction.SHA3_D512),
                };

            list.TryFirst(f => f.digestSize == dsaKdfDigestSize && f.hashMode == dsaKdfHashMode, out var result);

            return result.function;
        }


        private FfcKeyPair GetKey(BigInteger publicKey, BigInteger privateKey)
        {
            if (privateKey != 0 && publicKey != 0)
            {
                return new FfcKeyPair(privateKey, publicKey);
            }

            return null;
        }
    }
}
