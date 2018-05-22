using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Builders.Ffc;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KAS.Scheme.Ffc;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests
{
    [TestFixture, FastCryptoTest]
    public class KasFunctionalTestsFfc
    {
        private readonly MacParametersBuilder _macParamsBuilder = new MacParametersBuilder();

        private KasBuilderFfc _subject;
       
        private Mock<IDsaFfc> _dsa;
        private Mock<IDsaFfcFactory> _dsaFactory;
        private IEntropyProvider _entropyProviderScheme;
        private IEntropyProvider _entropyProviderOtherInfo;

        [SetUp]
        public void Setup()
        {
            _dsa = new Mock<IDsaFfc>();
            _dsaFactory = new Mock<IDsaFfcFactory>();
            _dsaFactory
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(_dsa.Object);
            _entropyProviderScheme = new TestableEntropyProvider();
            _entropyProviderOtherInfo = new TestableEntropyProvider();

            _subject = new KasBuilderFfc(
                new SchemeBuilderFfc(
                    _dsaFactory.Object,
                    new KdfFactory(
                        new ShaFactory()
                    ),
                    new KeyConfirmationFactory(),
                    new NoKeyConfirmationFactory(),
                    new OtherInfoFactory(_entropyProviderOtherInfo),
                    _entropyProviderScheme,
                    new DiffieHellmanFfc(),
                    new MqvFfc()
                )
            );
        }

        private static object[] _test_componentOnly = new object[]
        {
            #region dhHybrid1
            new object[]
            {
                // label
                "dhHybrid1 sha2-512",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("94716fbced76ead56114212dba053442715d57b1e2db33be91e267398fac65be4529cc593231420ea177a1e4d3628a45fa837362e64b38f179219d7e9382388e5d530d3095b53618942eda76958601b0f93639803db3eea8ad7730df47ce7f45412c0a6b0837d0dc23a9af4fdd2ddfe966d46af71dff9fb3b2da7bd4577e1045f1c3cf982bbb1ef8f15b6c83f886ed25ac012a66615929bf8f00d560e47d12b5511ce81e5a47944f30ceebfce1dffd13ba56c7497408a1a71ef13fd8da48f7d8be27ef3890ee30894e1487edf110cc42c1eff9fe1279564b6696dda3cf071883de6577d77eff7ec9f22ace2e11c1058b319dd3a6afe74c7d87094db94db54b6d").ToPositiveBigInteger(),
                    // q
                    new BitString("db9247c8257bf9a7b3cc70f9a59010d68597b170a213a38008cbdcfb").ToPositiveBigInteger(), 
                    // g
                    new BitString("1e8d30acd50b0f45e0726491b8940a7f55fd8e55f3aa1737702923d4b036bff27a094551c6fc4ca82d5755c75ba8c4b38cb3723860b9f67618349b211050c419b3f63f7b086a0c0f480b3b05ffa2a420eb725cc5fca1a56ee6f60ea0ae129b56bd92d2400db6e9d5baf5c32271c3dee0b8e0ab7dbc646194e5aaa3672f9d9257c48a827c59e250efd9b2e7d812e078c71d741794d525e9dccdd2c50ad37f45ca11dc4829b0aba6483082c7c0427bd5449fcc1fc868f14eeec9d062f9ed60c4edace2e75b76750ff9d81ea014337791dfeb7e59ccf4aa67dbe1df9b39005ee910e06a6970cb8065127673ed612c77e2d2b883bc6eac1f7c16867faf238355fa27").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhHybrid1,
                // mode
                ModeValues.SHA2,
                // digest
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("5028cc580912357f76c722f7fbde49fd1be76614ffd39eb81547d8e2").ToPositiveBigInteger(),
                // public static this party
                new BitString("145f7d46b201d368b92c9a64d0b6b4d6f645cf1913be83ed03b60f854235d95bccf17628671631a917e765bc8d4198336ed7e0374e4f0075fdd20763f9dd62a00a62b975423fface07c4cf8ed80b6e481914c53a1afb5fcbfb2c973ebc22d7d5b1b9c851ebe235ae10806209fdffa97539ba59089d353f49b5db645a49d3982990d5e712ac01b07290549e44e58a97db57029065cc62dd5e77879f4493d535ebc4f54e76cca6caa85a85159b110c34228b80a95e4b7c96434b271187d99dc1ff8694bd8a5989b55c4a314aa44ddf19957c19c1409b1725a901bd1588eff601050487a6220c32977732b44a7270c3bf280118051eafc3cb1ee01a1dd0e6eadd5c").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("71693f25493c04e6b10fcb9f6f654f3eed81cba441a28befde361c09").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("5919def5afc222e846969713b3902eb9f7436a9ef161de66727445613cfac93c4c30028f87d83545e14b8ae7ea00071c4d3c1a4c8970c5d22a3e9274d1dfe1ac41c688ef64ba876acdf0081db885b64d3fa2b59bde94435129ad2a84542090a63efc7600e9d2a380804ec95c72a260518bb080c69d96f7c99946be24c485b2814ab60cd0d3cef77cd1a06dc435236aab5dc82e5c351886a217817e9ae6febb85693f27b7ec3f62904aa219350b787b7c3770e9e83a3582a6ced6a604f6d90851ebc2869d67e35859870b8a91ef9f9d359cc87bf6672acb70be6fdc7edfe1e33b9aef2f6e66782d57835596e3e0fd290fc1b07a98780b5a2550bb914836cdd38d").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("195e24074a5966824594956b51f8a10875ac51e12f876f6cf43ae29e").ToPositiveBigInteger(),
                // public static other party
                new BitString("4596b250c02656cb3b82ff542cd3743a1121bf6a13a12e9f0d6a2fa9a81189274e448e87b37413d1190483181138fe23d6f361398c328f9790b2dd1bf08e2adc230b0839b2befa96527aa1b8318b793c1e732e77bf34192b492efd00a5daaba4f3ef1ad1456b541cf465a6d207d87b932df8256131dab60c31daf23a1d2ffb719619d94801f602d238928fa631684334d21e0d49e242d6bf60a92a5b4b0c866ea5681768ed28db52a32b51023f3680eec719074c2d4c9691c332b61a3cc37a6b9088168b488f31b29fc43930af5564c62aaf0bf98d4f6a32147f810e66137561b52e2f3a2dbdf270782335dac6578f26ddbd8b6c5687f5aced1fb3424372863f").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("60b87f7263b6f6a6350d3bde3b4b8793a560090e462a8603129e9e07").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("288c0aff0194209185f2af78ed4420cce8da2e5033c603d71eecac36d7782a9ef677fea2b217e9958d81ea9c03fab2f3762b15535f325d9abb6c6bbc3f592a236d595c0b44db311f5b9da91dff742990e0eb7a6b3c19758a27b7d7660b8bc6963a483e28c50328b197f9d4b7b142d76f31338e9a02ed8e41857f893d16682075223184cdc2fe827099a35c4e86540957f346be3095b3a3afdf8667ccb3565d21e43e0dfeffb3ccf57ce090943e15395c83e0a1821708bdc15aead90a2833c973461ab50d134d4d0e7424bb5a1bcf906586df9584c57143bb81a8b556dcdf70c1fe13880225305b7db542645bb06bfdcb926c76b86a0f4d780c02eeb38c8fc431").ToPositiveBigInteger(),
                // expected Z
                new BitString("854b1fe9d69a9e69301318ec9d57f80568e234950bf027e236af05463c5a34440289e3f019c75ccb39105d1171776f53ca0794abd1cf21343cea3a451cde34534d4db6c53f62ec04b5ca2efdad55a376a518f4266cfc539eec76369c9a5c3e4fe0b23beeba918f89537bc872e0494ed8b176c8d0a6267009ca448c2f8fcb23c74b73223db93cc827bdc3e2e2cf483f0c8c0a59f18488004f816d7815ce8be9558287dedcecea613d3cee31a368d21a2f02607ccf9544ed18049831cfe8d4c6147ae1e195f21141a616aca0b199e2198c9a980753443af750090f1ce31c4d71f64dae428c1cc2f589ad8ab1bd48a73d9b98c8d4c9c9f34d824e97591d9fd8fd0f51d7304ec4ce65738d4e2438530cb0f9484a3d4a3f85ce6484921627a22913c335e7f7fe4f8864b985fbef669e14f54bde77af9c15153a172458dd70f061056849910dc514b84283fb65a79aa6352fd2d05294ae34d7de57f8e527a3017f54067864871fbd7f459063fd184c2b01c5d7ea7bad6874bbd9b21e77252c2da0bb29d79e8c887961e7433003401b94d84f5a04656fdc856dbf42bba4372bdcd0b42aec4e6e97371c88a554ba62b78460c166ed3b842077531d9759ea84e69af41773d08ee4ccd21a283ac742273043c72fb4896a814605e36ed06b6b882e82b6f138c8b5c534d2054edac50fbbec9a2c23c4d8add26564835538e2838f184b665aff"),
                // expected Z hash
                new BitString("f26e6d003a72edea244749fadfcfde707d17702dba2c2c7c99f43be9475180cffc1d0a525e67690a0f566016aa8e7e3e69e946860d54384b4f6256ee809650d8")
            },
            new object[]
            {
                // label
                "dhHybrid1 sha2-512 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("94716fbced76ead56114212dba053442715d57b1e2db33be91e267398fac65be4529cc593231420ea177a1e4d3628a45fa837362e64b38f179219d7e9382388e5d530d3095b53618942eda76958601b0f93639803db3eea8ad7730df47ce7f45412c0a6b0837d0dc23a9af4fdd2ddfe966d46af71dff9fb3b2da7bd4577e1045f1c3cf982bbb1ef8f15b6c83f886ed25ac012a66615929bf8f00d560e47d12b5511ce81e5a47944f30ceebfce1dffd13ba56c7497408a1a71ef13fd8da48f7d8be27ef3890ee30894e1487edf110cc42c1eff9fe1279564b6696dda3cf071883de6577d77eff7ec9f22ace2e11c1058b319dd3a6afe74c7d87094db94db54b6d").ToPositiveBigInteger(),
                    // q
                    new BitString("db9247c8257bf9a7b3cc70f9a59010d68597b170a213a38008cbdcfb").ToPositiveBigInteger(), 
                    // g
                    new BitString("1e8d30acd50b0f45e0726491b8940a7f55fd8e55f3aa1737702923d4b036bff27a094551c6fc4ca82d5755c75ba8c4b38cb3723860b9f67618349b211050c419b3f63f7b086a0c0f480b3b05ffa2a420eb725cc5fca1a56ee6f60ea0ae129b56bd92d2400db6e9d5baf5c32271c3dee0b8e0ab7dbc646194e5aaa3672f9d9257c48a827c59e250efd9b2e7d812e078c71d741794d525e9dccdd2c50ad37f45ca11dc4829b0aba6483082c7c0427bd5449fcc1fc868f14eeec9d062f9ed60c4edace2e75b76750ff9d81ea014337791dfeb7e59ccf4aa67dbe1df9b39005ee910e06a6970cb8065127673ed612c77e2d2b883bc6eac1f7c16867faf238355fa27").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhHybrid1,
                // mode
                ModeValues.SHA2,
                // digest
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("195e24074a5966824594956b51f8a10875ac51e12f876f6cf43ae29e").ToPositiveBigInteger(),
                // public static this party
                new BitString("4596b250c02656cb3b82ff542cd3743a1121bf6a13a12e9f0d6a2fa9a81189274e448e87b37413d1190483181138fe23d6f361398c328f9790b2dd1bf08e2adc230b0839b2befa96527aa1b8318b793c1e732e77bf34192b492efd00a5daaba4f3ef1ad1456b541cf465a6d207d87b932df8256131dab60c31daf23a1d2ffb719619d94801f602d238928fa631684334d21e0d49e242d6bf60a92a5b4b0c866ea5681768ed28db52a32b51023f3680eec719074c2d4c9691c332b61a3cc37a6b9088168b488f31b29fc43930af5564c62aaf0bf98d4f6a32147f810e66137561b52e2f3a2dbdf270782335dac6578f26ddbd8b6c5687f5aced1fb3424372863f").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("60b87f7263b6f6a6350d3bde3b4b8793a560090e462a8603129e9e07").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("288c0aff0194209185f2af78ed4420cce8da2e5033c603d71eecac36d7782a9ef677fea2b217e9958d81ea9c03fab2f3762b15535f325d9abb6c6bbc3f592a236d595c0b44db311f5b9da91dff742990e0eb7a6b3c19758a27b7d7660b8bc6963a483e28c50328b197f9d4b7b142d76f31338e9a02ed8e41857f893d16682075223184cdc2fe827099a35c4e86540957f346be3095b3a3afdf8667ccb3565d21e43e0dfeffb3ccf57ce090943e15395c83e0a1821708bdc15aead90a2833c973461ab50d134d4d0e7424bb5a1bcf906586df9584c57143bb81a8b556dcdf70c1fe13880225305b7db542645bb06bfdcb926c76b86a0f4d780c02eeb38c8fc431").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("5028cc580912357f76c722f7fbde49fd1be76614ffd39eb81547d8e2").ToPositiveBigInteger(),
                // public static other party
                new BitString("145f7d46b201d368b92c9a64d0b6b4d6f645cf1913be83ed03b60f854235d95bccf17628671631a917e765bc8d4198336ed7e0374e4f0075fdd20763f9dd62a00a62b975423fface07c4cf8ed80b6e481914c53a1afb5fcbfb2c973ebc22d7d5b1b9c851ebe235ae10806209fdffa97539ba59089d353f49b5db645a49d3982990d5e712ac01b07290549e44e58a97db57029065cc62dd5e77879f4493d535ebc4f54e76cca6caa85a85159b110c34228b80a95e4b7c96434b271187d99dc1ff8694bd8a5989b55c4a314aa44ddf19957c19c1409b1725a901bd1588eff601050487a6220c32977732b44a7270c3bf280118051eafc3cb1ee01a1dd0e6eadd5c").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("71693f25493c04e6b10fcb9f6f654f3eed81cba441a28befde361c09").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("5919def5afc222e846969713b3902eb9f7436a9ef161de66727445613cfac93c4c30028f87d83545e14b8ae7ea00071c4d3c1a4c8970c5d22a3e9274d1dfe1ac41c688ef64ba876acdf0081db885b64d3fa2b59bde94435129ad2a84542090a63efc7600e9d2a380804ec95c72a260518bb080c69d96f7c99946be24c485b2814ab60cd0d3cef77cd1a06dc435236aab5dc82e5c351886a217817e9ae6febb85693f27b7ec3f62904aa219350b787b7c3770e9e83a3582a6ced6a604f6d90851ebc2869d67e35859870b8a91ef9f9d359cc87bf6672acb70be6fdc7edfe1e33b9aef2f6e66782d57835596e3e0fd290fc1b07a98780b5a2550bb914836cdd38d").ToPositiveBigInteger(),
                // expected Z
                new BitString("854b1fe9d69a9e69301318ec9d57f80568e234950bf027e236af05463c5a34440289e3f019c75ccb39105d1171776f53ca0794abd1cf21343cea3a451cde34534d4db6c53f62ec04b5ca2efdad55a376a518f4266cfc539eec76369c9a5c3e4fe0b23beeba918f89537bc872e0494ed8b176c8d0a6267009ca448c2f8fcb23c74b73223db93cc827bdc3e2e2cf483f0c8c0a59f18488004f816d7815ce8be9558287dedcecea613d3cee31a368d21a2f02607ccf9544ed18049831cfe8d4c6147ae1e195f21141a616aca0b199e2198c9a980753443af750090f1ce31c4d71f64dae428c1cc2f589ad8ab1bd48a73d9b98c8d4c9c9f34d824e97591d9fd8fd0f51d7304ec4ce65738d4e2438530cb0f9484a3d4a3f85ce6484921627a22913c335e7f7fe4f8864b985fbef669e14f54bde77af9c15153a172458dd70f061056849910dc514b84283fb65a79aa6352fd2d05294ae34d7de57f8e527a3017f54067864871fbd7f459063fd184c2b01c5d7ea7bad6874bbd9b21e77252c2da0bb29d79e8c887961e7433003401b94d84f5a04656fdc856dbf42bba4372bdcd0b42aec4e6e97371c88a554ba62b78460c166ed3b842077531d9759ea84e69af41773d08ee4ccd21a283ac742273043c72fb4896a814605e36ed06b6b882e82b6f138c8b5c534d2054edac50fbbec9a2c23c4d8add26564835538e2838f184b665aff"),
                // expected Z hash
                new BitString("f26e6d003a72edea244749fadfcfde707d17702dba2c2c7c99f43be9475180cffc1d0a525e67690a0f566016aa8e7e3e69e946860d54384b4f6256ee809650d8")
            },
            #endregion dhHybrid1

            #region mqv2
            new object[]
            {
                // label
                "mqv2 sha2-512",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("c120aad237fd11e9fe2101ab117731c7425f3de17f686155d922e106fa4c901666da20e0af31f3c5f331d241dca19ffb4d8a2a5131b3b8e141f73bf4c16d0ea6f43d8ad6dc41f5c2163a1d582a16f38c47f8133bb43ebabe3758f196a9b01134c172c72794604d2806c73b10ca35ef1808017c471bc2533c78a6b75dd5cb3d3e01eacd6c9f5f0f425788c63114ae2e2a5032a54cc06219b39465a48922b38dac28137868cb2748985c449dc96da187cd92460703f6289805f5a0c0a29c64bc7122a889ae3d74117de4666a1ed6d6c850cfa741731f8aa82f4bbc790f71d36ff06f7b5a461f18180fb58af99881477eced508647330a7da773abaeb1e0c662743").ToPositiveBigInteger(),
                    // q
                    new BitString("faae1d73793ada3b72fac4a0d11579e9945e1f26b9074a9f4a30c491").ToPositiveBigInteger(), 
                    // g
                    new BitString("88abc4824517f4dfe111c60dbb77a7fce7844954e45bda8725c5c032c27c0faec4be2a42cab6f2d48462950423550759d7cd1142d64903d0010809159b9cd701259f90185dce83788f13b3d3685a42c24a48e416833c1c92e4ceb2bd37db75d1c3c89b751b97cf8a64218819c9c4ee2c3a087507d9ab2f0d0e41a636ab0feabd80c76ec559ec12d33f87b7bd180129bbb065a43506f765f8cc6fefa948b7f3b9359f9c96301b5f619f7e8ae48849d935a9b7be1abe3d2911d00f893f7d99ddf11e2d99aa6bd59b8ace5f518a4d1e02e44df979ebe467852e2fe7ce1cad5ab499f53c7424f371cfebfca4b6ba5084e86306fdbcae4941b6bfaf2140d17908e96a").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.Mqv2,
                // mode
                ModeValues.SHA2,
                // digest
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("030a001735dfd21690f3b4e6152d511471a590949c492bfafef58ac7").ToPositiveBigInteger(),
                // public static this party
                new BitString("b8da8de8292163c2cf54a50a3f82856cb2ef81bd8bc7f7d3c0cb103333a3290082dc022f90e45fd636843350001b6d80c4ec58cccda903ab92d34d1677e534d4234ee687232ffa2dd74c337558d4fcd2cf8ef8a109f892f26ea6bee61b1e282a6a95f2b300c97107e3fe81059d4415288e656e67ca9162436bdb0ed44260636fba0e89be7256786fc364dcf6835d13fffc933ea190addb53ebdb583b512b44dfa2a77cdcd84e17e86cda92ea3978fb0b3796ee886be5292fe4e9a1cebabf71795fa98e8f20ffba175fc9d7a6ac4190bc8d6488eb6e857c2082993fd474a67732971c636083ed129f192e5b61dc88c8aa5a5995967b7093c5563295740055af08").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("2c02d3f62ef4a0cd7cc247d936d51772d739b96aacedcdcac537597f").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("b980ab08317c880b475afc5a0373e70e207283c02a9d9a46f94cbc2700f92279d3679a0d3347af0d291465d897513ec1d4c0539a6fc221864c137a55d5d1fd6cd4d41787de797f8a5037056cacb8365aff56758d3dfcdceace005210dcb4bd7f4a8a291ba7096b9bc0d465b9cbdbcf8b00b017419f3aafea3d91575f961e4b7768d062de67d4d9147ad40ec630111642e50f733a89b5cc7b2c147ab7cc9f33b67c360c8efa1ac88d6d0137dd301c50bc1a5e080c38c787d52a20a4877eb5abe776ec905f528d48ccd3e9f6e67f6c00be357c74045265455f417ac4b800962ab1ddf5a2bf702cc06b0a3f5faa95eb545956ae5b2385191c8a9cb7185f87a5fa31").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("c73f44bbf9d46362827cc3251fa199e8e3417b42bdc0c7677539246a").ToPositiveBigInteger(),
                // public static other party
                new BitString("99d4b71fa8fd512d3c6f9b0ed819c9a2b01987a40186e0b905356f196bc54095209ec6ff6c67744e50c87cc9bc61579bd404fb8094798b49d233cde388a39a52e37861bf3dc60639e43baaa55f275f1ee9bb7db4700866fe7390a2a444400507a3ba2c6c5d177b5bb666bd7018a58d40ce2a32d5a66bd89f78438e6b3ad006f740d3b514d9d131ecd1ecb4322987288c703dd5e136e61c317b133f5a30cae67ef9dae692edb0d18158e48bfa29a67b60de7eb85ee8d70d316ead749b6064954495a6e6bd7d24b8b419bc11a159ea93600c43d904f037c5092fbbf586fbe3037dcfd18b92c9f5e4a0b90ef0d2a9e579b094a26b5198eca74b109fe60388e8b4f5").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("b74dd76ebc0f7af22ff0bdf5f909e7677d4b792769d201edefe76d7f").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("93193e14e6bd6d58cef96929cd31fa173193e148d0117e89cfd361c14a9c96329dbf972f7b1cb09e2b82478375489827129424bb9935adced8028a2067dc1beb458367280d07ba9698894cc90daed49b012284de7df350a7de093b133bbff3dfd4b4abaf00c462748027b7ba72e46cf4b3ffb4c3913f4a8bf8c33e47082ec9a6027d1b63487a658181daa432c4280f3d18e526bdff0e8745a1766cc51d0c70d1fe5bf9530b9226543dbbdadddd946e2281078340537b9266e70b6cf61eece75fe75af0ac7703b292cb50c57d11c76206ef28cac60a0457a05b9c306ee83523e8db612ccfc56f6c2998278b16a60729d13aa090ca0a7dd420225ab66fbb089ea5").ToPositiveBigInteger(),
                // expected Z
                new BitString("a7f8d773c7a435bda245f4d3e3660569afc19cc1b85b26a81545995e5e63535085e7486b328176475598f7e5e1e02dac9bca4a7575637d2c32e17f479839b5ecfa0f64edb11003b388ac1b760fea29a57b6f1f6a743d6df0e19946188fed37b4206888565a9b3c2d82065275ddcacf5e9c2625d7db6080132eec701cf60dfccb42282f7e51859e631b1108899cf0b283680cdc84c0d0d892fade6cfc772c245183e0c83e6f0856b10163a49c3b0416f1948d92f3e3a627b6ae021a6708d02fd3d00f048a1e59e7de5cf68ead17dd0d494aac280ebfb9c4c3b5ffc0b8dbcdeec9e47b2d28134711357b02f773e3ad7309a4485d1a40e0af9e3d4744b18f9c4c61"),
                // expected Z hash
                new BitString("5a9a66750072ccde8c7101d307adb4c12ca442d9dae2ca183d15fb4e7b8047d41f162b964791f5970863239e7458329ee2593af29a84d5f7c51b3b4b16d658b2")
            },
            new object[]
            {
                // label
                "mqv2 sha2-512 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("c120aad237fd11e9fe2101ab117731c7425f3de17f686155d922e106fa4c901666da20e0af31f3c5f331d241dca19ffb4d8a2a5131b3b8e141f73bf4c16d0ea6f43d8ad6dc41f5c2163a1d582a16f38c47f8133bb43ebabe3758f196a9b01134c172c72794604d2806c73b10ca35ef1808017c471bc2533c78a6b75dd5cb3d3e01eacd6c9f5f0f425788c63114ae2e2a5032a54cc06219b39465a48922b38dac28137868cb2748985c449dc96da187cd92460703f6289805f5a0c0a29c64bc7122a889ae3d74117de4666a1ed6d6c850cfa741731f8aa82f4bbc790f71d36ff06f7b5a461f18180fb58af99881477eced508647330a7da773abaeb1e0c662743").ToPositiveBigInteger(),
                    // q
                    new BitString("faae1d73793ada3b72fac4a0d11579e9945e1f26b9074a9f4a30c491").ToPositiveBigInteger(), 
                    // g
                    new BitString("88abc4824517f4dfe111c60dbb77a7fce7844954e45bda8725c5c032c27c0faec4be2a42cab6f2d48462950423550759d7cd1142d64903d0010809159b9cd701259f90185dce83788f13b3d3685a42c24a48e416833c1c92e4ceb2bd37db75d1c3c89b751b97cf8a64218819c9c4ee2c3a087507d9ab2f0d0e41a636ab0feabd80c76ec559ec12d33f87b7bd180129bbb065a43506f765f8cc6fefa948b7f3b9359f9c96301b5f619f7e8ae48849d935a9b7be1abe3d2911d00f893f7d99ddf11e2d99aa6bd59b8ace5f518a4d1e02e44df979ebe467852e2fe7ce1cad5ab499f53c7424f371cfebfca4b6ba5084e86306fdbcae4941b6bfaf2140d17908e96a").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.Mqv2,
                // mode
                ModeValues.SHA2,
                // digest
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("c73f44bbf9d46362827cc3251fa199e8e3417b42bdc0c7677539246a").ToPositiveBigInteger(),
                // public static this party
                new BitString("99d4b71fa8fd512d3c6f9b0ed819c9a2b01987a40186e0b905356f196bc54095209ec6ff6c67744e50c87cc9bc61579bd404fb8094798b49d233cde388a39a52e37861bf3dc60639e43baaa55f275f1ee9bb7db4700866fe7390a2a444400507a3ba2c6c5d177b5bb666bd7018a58d40ce2a32d5a66bd89f78438e6b3ad006f740d3b514d9d131ecd1ecb4322987288c703dd5e136e61c317b133f5a30cae67ef9dae692edb0d18158e48bfa29a67b60de7eb85ee8d70d316ead749b6064954495a6e6bd7d24b8b419bc11a159ea93600c43d904f037c5092fbbf586fbe3037dcfd18b92c9f5e4a0b90ef0d2a9e579b094a26b5198eca74b109fe60388e8b4f5").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("b74dd76ebc0f7af22ff0bdf5f909e7677d4b792769d201edefe76d7f").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("93193e14e6bd6d58cef96929cd31fa173193e148d0117e89cfd361c14a9c96329dbf972f7b1cb09e2b82478375489827129424bb9935adced8028a2067dc1beb458367280d07ba9698894cc90daed49b012284de7df350a7de093b133bbff3dfd4b4abaf00c462748027b7ba72e46cf4b3ffb4c3913f4a8bf8c33e47082ec9a6027d1b63487a658181daa432c4280f3d18e526bdff0e8745a1766cc51d0c70d1fe5bf9530b9226543dbbdadddd946e2281078340537b9266e70b6cf61eece75fe75af0ac7703b292cb50c57d11c76206ef28cac60a0457a05b9c306ee83523e8db612ccfc56f6c2998278b16a60729d13aa090ca0a7dd420225ab66fbb089ea5").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("030a001735dfd21690f3b4e6152d511471a590949c492bfafef58ac7").ToPositiveBigInteger(),
                // public static other party
                new BitString("b8da8de8292163c2cf54a50a3f82856cb2ef81bd8bc7f7d3c0cb103333a3290082dc022f90e45fd636843350001b6d80c4ec58cccda903ab92d34d1677e534d4234ee687232ffa2dd74c337558d4fcd2cf8ef8a109f892f26ea6bee61b1e282a6a95f2b300c97107e3fe81059d4415288e656e67ca9162436bdb0ed44260636fba0e89be7256786fc364dcf6835d13fffc933ea190addb53ebdb583b512b44dfa2a77cdcd84e17e86cda92ea3978fb0b3796ee886be5292fe4e9a1cebabf71795fa98e8f20ffba175fc9d7a6ac4190bc8d6488eb6e857c2082993fd474a67732971c636083ed129f192e5b61dc88c8aa5a5995967b7093c5563295740055af08").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("2c02d3f62ef4a0cd7cc247d936d51772d739b96aacedcdcac537597f").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("b980ab08317c880b475afc5a0373e70e207283c02a9d9a46f94cbc2700f92279d3679a0d3347af0d291465d897513ec1d4c0539a6fc221864c137a55d5d1fd6cd4d41787de797f8a5037056cacb8365aff56758d3dfcdceace005210dcb4bd7f4a8a291ba7096b9bc0d465b9cbdbcf8b00b017419f3aafea3d91575f961e4b7768d062de67d4d9147ad40ec630111642e50f733a89b5cc7b2c147ab7cc9f33b67c360c8efa1ac88d6d0137dd301c50bc1a5e080c38c787d52a20a4877eb5abe776ec905f528d48ccd3e9f6e67f6c00be357c74045265455f417ac4b800962ab1ddf5a2bf702cc06b0a3f5faa95eb545956ae5b2385191c8a9cb7185f87a5fa31").ToPositiveBigInteger(),
                // expected Z
                new BitString("a7f8d773c7a435bda245f4d3e3660569afc19cc1b85b26a81545995e5e63535085e7486b328176475598f7e5e1e02dac9bca4a7575637d2c32e17f479839b5ecfa0f64edb11003b388ac1b760fea29a57b6f1f6a743d6df0e19946188fed37b4206888565a9b3c2d82065275ddcacf5e9c2625d7db6080132eec701cf60dfccb42282f7e51859e631b1108899cf0b283680cdc84c0d0d892fade6cfc772c245183e0c83e6f0856b10163a49c3b0416f1948d92f3e3a627b6ae021a6708d02fd3d00f048a1e59e7de5cf68ead17dd0d494aac280ebfb9c4c3b5ffc0b8dbcdeec9e47b2d28134711357b02f773e3ad7309a4485d1a40e0af9e3d4744b18f9c4c61"),
                // expected Z hash
                new BitString("5a9a66750072ccde8c7101d307adb4c12ca442d9dae2ca183d15fb4e7b8047d41f162b964791f5970863239e7458329ee2593af29a84d5f7c51b3b4b16d658b2")
            },
            #endregion mqv2

            #region dhEphem
            new object[]
            {
                // label
                "dhEphem party u sha2-512",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("a3a77cce3b0ea9891fe6ac34b2bdd04e22f9fd5a4976b5e2bd4c9ec43831c4d293779f3c4f826e6c2a8d6bd1ffca323b2360fcaa8bddc8c5268941578eede1f9447a39aaaa9af45bae4596b6df2a7048ce65bdd421ba055c640458abd4fdd07564df3a39ad6375a38dca884e5b67550bd60d789f5167935add6ae77af506e69d48eab2ebc1f17ff671c6d03d2f4f0e53e0ff1bdb488feca5d2b569f510242dd8bd64502c67ee8fe36224860a8b2934e864f75eff5fed4ecea69a1b2e6893df75ae19b266f4a55ccca2307038056aebfd212a4d5b540273d232c38d5cc6595216c3050cf4562989be8b341bd58c183e5e411939b4b34ad5752e87ffe622bd2075").ToPositiveBigInteger(),
                    // q
                    new BitString("f094f4fa8fa36fdcdf4f0378112bfde03cfa532e666b9736b5ab76e9").ToPositiveBigInteger(), 
                    // g
                    new BitString("45308211a07f231181276b44b873eb67726ca6aa5ecd39b4274f780409e15bfc98ac4680be5220a23b963e3b494602a80ce6cb6eb3f056e2a911ff7529f07fc53fa8840174698aac6a9dd540e86171cf2896a7337c0a839bfd9f24779c83f75b376da3c3c4d25d6b454e09dadbe230ee42115ae7ea79ace00b3c73bfd0c9913b0251177de4aae0ed54c041ff071346b2603360e5175faa9bbbc8fc50c5c657bba28da146674fa8a4f936da9d86511959785cd8e34c4b1f390b2cc68f574fd85e96e894d1b225ad43b3489af729c560b513a671e7fde2bd138fbd20605c74347e76ac50e230c57fec6dda275df29f770d47b91631e135778a51f3032bb1ef292f").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhEphem,
                // mode
                ModeValues.SHA2,
                // digest
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("00").ToPositiveBigInteger(),
                // public static this party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("a292aa176f00c2a6690fe22a0d402390cf308aed94d35c524b6ee04a").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("5067495e06d0c6dfd6eada5607f3dc7b9bf93eeeb7513119860f5d60ef332823b0ba58af2ecb7ab2cbccee87ccf232a02c27affb167e1a86811090262771c0fb5574c89ffb1288cd1d0096c0bb62add57fd3fa691ee1152b632778016a1a0c4ec2fce5ade1d3d1d2ae5c5a1a71e1a90dc648b384222e22357b8301536866d70b91a37ff3d88d444ed3e531b19939f3dfa33c4782ec195060cb35a13e0bad6f1f9c9be10720bb1055af93e16999c97d127fde52f16060080656810954f4cf745a57a3909327b2eaaa3ea5b9fa794f186658b186974861a00ee59b125c06398b835de09d2b340f7f0254e69339bdc4257b11543a300e1b8c615cd3a64838ce0d09").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("00").ToPositiveBigInteger(),
                // public static other party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("b0f7d1d0a44993d679599a310a2f56143de4790359fb4746828fbb8e").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("2cc5dcb3cae0bd0052838991e371fca0bb827598c1ffa554f2699ccc29b5bb085bf77634ab3fec24fb81ab1e435c17229bb6872eb4fcf30acee2a3fe9891363f51112f94d2a48ae506bd4dba9e8db6ad59713d4b8a5afbdb717a27483680998bea79baa30e42294005bfacb3e67d113549fa48d058cff1dce03ea2d89be3b61358618c540db7b11b06d4d0e545d5a5ad8d93246946f5d9a9710ebd40a48a2c70e7b93928497fa02d08cb1c591dc3c204e88e933ac2e8c68b85e3757af1b44448d60ec63fc1323f3be369662b2937a419648dc9f3b4b19496e8a4feebaed8e0ccb02d56000e5b1832b2132384efab47950e40eb8482f2d7bd344e019e9c573384").ToPositiveBigInteger(),
                // expected Z
                new BitString("98a315df977502d901014c49a15bb5d6cee17b7fb1a6be0021fd7caa39b3fa3f400161e5ed1c28735c7b6e9b16077b5b208ce4192c676044f0636da25179e4efcb2c35580c81d9b6350aa787150d6c8aefb69082d2031baeeaaf729bd960b315aed59b02c4f625c32a374c20d7ca17fffc17d5bce75c4d1360e01b3ff5871c41f57d13e94e9eb401833e349e0aaf0d7981e7236a493ac04b4d66779c2b637865d9ec8c95d65c4ba777f02f0772f3c60f0a1090bdc9d1cd579bcc92bf02e79d3cca0b2783b7c86be054b403a084392032e6289bbbbc10f169cf4c48fe50b59ebef1db4574a80d0793395068622e138310933a005faf2540ddadb2c4af771c838e"),
                // expected Z hash
                new BitString("39de343caa24b36f02ce0d3b08a7c5f553c7f5f0f65dcca10855a3fed89efd475156b51bfdfb1509e7898ca7bdf57a5485f68c82f4dffe8f1d7fb7b13a0de933")
            },
            new object[]
            {
                // label
                "dhEphem party u sha2-512 2",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("a3a77cce3b0ea9891fe6ac34b2bdd04e22f9fd5a4976b5e2bd4c9ec43831c4d293779f3c4f826e6c2a8d6bd1ffca323b2360fcaa8bddc8c5268941578eede1f9447a39aaaa9af45bae4596b6df2a7048ce65bdd421ba055c640458abd4fdd07564df3a39ad6375a38dca884e5b67550bd60d789f5167935add6ae77af506e69d48eab2ebc1f17ff671c6d03d2f4f0e53e0ff1bdb488feca5d2b569f510242dd8bd64502c67ee8fe36224860a8b2934e864f75eff5fed4ecea69a1b2e6893df75ae19b266f4a55ccca2307038056aebfd212a4d5b540273d232c38d5cc6595216c3050cf4562989be8b341bd58c183e5e411939b4b34ad5752e87ffe622bd2075").ToPositiveBigInteger(),
                    // q
                    new BitString("f094f4fa8fa36fdcdf4f0378112bfde03cfa532e666b9736b5ab76e9").ToPositiveBigInteger(), 
                    // g
                    new BitString("45308211a07f231181276b44b873eb67726ca6aa5ecd39b4274f780409e15bfc98ac4680be5220a23b963e3b494602a80ce6cb6eb3f056e2a911ff7529f07fc53fa8840174698aac6a9dd540e86171cf2896a7337c0a839bfd9f24779c83f75b376da3c3c4d25d6b454e09dadbe230ee42115ae7ea79ace00b3c73bfd0c9913b0251177de4aae0ed54c041ff071346b2603360e5175faa9bbbc8fc50c5c657bba28da146674fa8a4f936da9d86511959785cd8e34c4b1f390b2cc68f574fd85e96e894d1b225ad43b3489af729c560b513a671e7fde2bd138fbd20605c74347e76ac50e230c57fec6dda275df29f770d47b91631e135778a51f3032bb1ef292f").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhEphem,
                // mode
                ModeValues.SHA2,
                // digest
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("00").ToPositiveBigInteger(),
                // public static this party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("a257e0ec23007768643135665f9315164bff024465e6da4aa9bfa44b").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("581a20e7d564dd30ff9e580055ebda3d02b3a60dc7cdf1102e3598bf2934a8ca9961242c58aa927e29755b2a3056050b691401ecb838c74d4cfd0d3c36c3e59df79d3190845b1b49a7a3cf4dba9afea8d3a9f907cf65119f6cc9c9cc8693d3bb52078a1c5f598fc3663a93ecf63231f59a96e11e539962d24dd721318ff7e9515ad759d997c6b09365388938f6b385c1c558baabf4d10ff94883dadf2af43b2b060699e77cec3320f4df883c747a7fd6842e16917ca841247b57fa2dc03cc2b6af9f78ef17a499b9a72c96ec413bd175b9caceafdda65992834afc941ec3097aa2ccdf0decbd6bbd3b19d767e6fa77a3ba2b591a06dc522165d2171a6e2ec2f1").ToPositiveBigInteger(), 
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("00").ToPositiveBigInteger(),
                // public static other party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("ae60297d54ab210b59532e52d82e80c9c927b1a497c81eba42de5458").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("4bdea1f02522d9463166310ac6729a85c3e6435b2f324c9794da73f7ad0a48a07d840b249617a982791f2b8a2cf0442492555464a062dd529981c2d33893c90f62784b8706287fbe715827d5e158e222127f8be8c8607a3b7936ef807089d4e5727db7c3d966c528f89fc20b463c062a26263b9262655d3e0d5d610d78541be0c3000a6c905a1f915b0c0e166990887f544f952e5cd66c79df2cea930c7aedc6b2d12586404f1af21695cf82cb22344e65d904e4b0a855c3956e5fc327505fd6475d0db5ef495b88d825a22455896722936e8b1375a68054d882aa5a186330228540e54dbbf1932a78b7496113551e5d41b0dfdd0934f9f4e1c07e6ec922bac2").ToPositiveBigInteger(),
                // expected Z
                new BitString("0ed41716191aefc64a3ca52433171d1f33941af85dc4941cc0b80a656c1e9ee203108d97776f7c488190f48722b163369f328038edcef0ea4d2f78113eff2485dd83b5e8e71d14b360b7421d1526fdfb4205bfd1195a0adff82e98b458db76147e2a9d1d97c4f467f6d05cb8ee3a34ebb5b00cac90238c9dcea462640ec505c3487c3cff4fac9ecf226ea262abd50cc73606eb3526c2cd83838f5b36c2d50a5e94fc290fcd472e68d08bf308bcd7ced39f2da5cb30c77a57d73258fadd17a1796d0dd8d1b560e0be3c58b910ef8287875cc54a03f50ea3f1ebd455dbb1c609ce25e503c3799e9f15c473a3ec02cf0bb2f17597312bc59b3cc5c8f6bd985fdf4d"),
                // expected Z hash
                new BitString("b2c547e3ff33b9f1980cf486bac95faa6e2c6d7148976439c1e8839eca3d8e54ee2bff9ab812dffa263bd8c0d16c2b2cdca7c62fe35dfd021fbb3493de767088")
            },
            #endregion dhEphem

            #region dhHybridOneFlow
            new object[]
            {
                // label
                "dhHybridOneFlow sha2-512",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("c0566222cd75ab2639590cae334cb891287f4a2fabe85f97cb0449277d73e289f972db53546c98549edf8830eb7ca2486fc2ff82d46deb9f3719785bd54cf3fa365324dbf9fa2797438499242f2c29bfcae51256bde9c9f9a47608b33d7378ef8efe33226c46c2e15f0447a430975c31b2b2b79485b28a8e389f55e83ad0edc1674979a2993ec0c8da1020956185066984a9eacbc765fbe69b5d09d9d9c1d87bb93011c0f4bd35d73ce09b9aef807dc9b9f9e817af223c84312159ae0d0c244f454f9ca0c503b127f65f4066f0379171bc59e9cfdf05e161ca0e9a4f7d65dcc7d6be5d7636ec6876fd0954a2718cd88339bc83eda009b7104485d12eb7977413").ToPositiveBigInteger(),
                    // q
                    new BitString("b0ba107552d3c4ae1b982b8ebe3db0774d3b6e2987c16dfc48ecbcc5").ToPositiveBigInteger(), 
                    // g
                    new BitString("9cfbfae39732e77f528794494fc8773661bc440151235607b137dc2bc34dfc3552e342463e45a4b72f835e38d7a038c4b7b02bc94e721c1f931a673ab16991816fd0d8df5a17f064e957e0693a66ffd990915fa35a6f0e705db9b40c374488583a678199c8ad9936f218f2ec3c7e7657901d54b32bb62674ec5b851998e056f1180b823183b3491ec48d60579365992246a4c7fd80207972c6d85126954eca0b907057c2e3f68117607273be19789f2fa370d8a243b88054de53d99a8b3ec69197b7b0de733c9af51683b2a52850924a008756885f37ee8f145eb3be34f15e10ba39e67822031f1a1b14fee28a7f97b328f9e435e7c175e7828ea4e13259cef1").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhHybridOneFlow,
                // mode
                ModeValues.SHA2,
                // digest
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("7fe5f3497ef3707bb77b0d7a9f84c960fabf130eaee5ee0570a74ec8").ToPositiveBigInteger(),
                // public static this party
                new BitString("a74b05bf6582c74a1a0d68b446ab1c3bab1bc33cc14bc32d35b26476bad1e61fc2855b13c9c38fc14c248be4cca4b8b724ff10c4393487a60347ab53bcf10fc9ae52b9bc1acf62f371348cbeca7917b9903a5795e4c7d406268e43467fdf5f5e27e405f7f4e77f5eac3b9b52bf86afe07140b49e7da4ba2c7bd8eb6e9e5975baa06638839f32fa8b6420754d01f73e9dd5baeafb1ccec2340c3e074610b84d77f370d8b3ecf51b89b2c2e886e22b4249551952877df8cffa93e6546897e247828ba6825d2b7c744fb850ade8e5d1d033e7d278648fb5f2d5d7032d4add89b765bd29a00b39b26149592ecbeafe7bc4e02d91fb7644db181a7788d215c5f99766").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("6b418999b6f4624f8fe210ef7532aae482c41301313164b9b744c5c8").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("577c620b2faa81a8d19cd4ef37bd9f5429fa43a68901d3bb7bd40788b1d77bd5faa3658c69a79e83eaf5d14847f598e869cdcf3446bb2b26395887df423405a07f5b0ee66e135cdc874ea6b49895d7d263090527acbce07391cb5d9875253c3a25416c88360481b54544c42da55d851d000f876a25643e4d1c07d8682f74c5e94a77ea19f133173e68004d6b2dfd80353e1bf909875c87e41f808e5608d133fa6fc8d7c12732350853958963c73f28ad1a885eb9c2b2fe522c6d5d8803d41083dd9bec9d6b148489c544bb1f3b57d9aedfb3a7af31d7881c5501ad72df7ae3a1dcef857108ff25046d5b366705c16629f4cbfda0a3bb500e4cb98b9fcfb77a9d").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("9696d8492f765c0f2875d43d5f1e0a96fc0e8887cace74a65ab9de1d").ToPositiveBigInteger(),
                // public static other party
                new BitString("650fe38cfbfc119b34803928f50c1c02920873c13f3d10cfc4902bc58d64698791a82f80bf3cd8b32a166a781a6b41b1b47fe9a73e9db7f7c41e64f19c816881aefe3274464fb084998feae7d3947c6ebaeeff490e04676c4c4a4f9ee832bf30e5075440b728e5588941d84832ad2d3abb2b6896c6c6f2a446adfd94f33a9bb835696a3388e89f5da062ee6d94b0fba84b9c6e1c9fc1f689657eb50a2bcd8313017ed13155d0a0807ecda28c3541b12263a7b3f51aeff98f37a230464ee58095c2485476065b85bf477bad7f9bd8a0d6fdd99b2b8279c0ca4d5d8c46d0ca9c06b597ea91b5a01b79c20ff104c629038416e8026589764416e62a561cb7824035").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // expected Z
                new BitString("1272785560d19f1ae2d8862e3dd107ccace44e421b4ce8cee3e2b18cbf2e56631b855b234303723489770c8da94b81bf98edd82b2f2c64af56462bb93b7819169872fc1ba3b3f60e3a8bb6f180917952809dae955b33d96e05d32f3f87ff78ee40d515fb97bdfc0789a64b2db06d8336c4ed1c45d4c6cfe948816f31292888b75ed3d9829574801d8d2ac389dbfdd037deb2a48be1889e6cb0b8467b548ef93fcfa19c6b65044d469ddec30f4458ea536fc5a71d345546fa55b0a4027ef81f4e105ed2b2b96f9b4d044503c0f5de4773da7ece1fe8ae7f7bff296cca265d0c667d5a553b8bd05030e0b4309ab841f7b30057ab25efdb972f166f6940bec4fd3d0ff78ac79a4ba1d6268431e7c96a7a66965a872770c6dfad6c9344bb3d0360511c9b35c3bb5d92e8564b18125d54d4f3535fde00a854df1eec6635d1af55b788c44032d6fea58f4e0def195f6da53a3c7265f548933fb3495eea71c3b67a7cbfa5243fb47f92e97782cf56abc7026ad5803601a6f905de058b6485708e547ac5e97a3b4f09c1b66f1df61c662237fa807146b17cb5d0bd03a61e3087c94648b643dbef4d3278707f150b7a2197ad9ff5874422d33951ae465ce4efcc3a7d69ec510cccd17983c389e788548fcf85dcf702dc1cd76febb4e50176ead746dd6b1bd620cd357c9af6681c59c5cf9dab3de8c1d0a0dce15d035691c732eb58e6a4de"),
                // expected Z hash
                new BitString("93e919dbd11d7258343e0db8167c724efdabe885e7423739977c14ae787a8abb1acf51ce3ebac88d5395775b5d45c7992a565015d65ca0ea4668c59a5f306e5c")
            },
            new object[]
            {
                // label
                "dhHybridOneFlow sha2-512 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("c0566222cd75ab2639590cae334cb891287f4a2fabe85f97cb0449277d73e289f972db53546c98549edf8830eb7ca2486fc2ff82d46deb9f3719785bd54cf3fa365324dbf9fa2797438499242f2c29bfcae51256bde9c9f9a47608b33d7378ef8efe33226c46c2e15f0447a430975c31b2b2b79485b28a8e389f55e83ad0edc1674979a2993ec0c8da1020956185066984a9eacbc765fbe69b5d09d9d9c1d87bb93011c0f4bd35d73ce09b9aef807dc9b9f9e817af223c84312159ae0d0c244f454f9ca0c503b127f65f4066f0379171bc59e9cfdf05e161ca0e9a4f7d65dcc7d6be5d7636ec6876fd0954a2718cd88339bc83eda009b7104485d12eb7977413").ToPositiveBigInteger(),
                    // q
                    new BitString("b0ba107552d3c4ae1b982b8ebe3db0774d3b6e2987c16dfc48ecbcc5").ToPositiveBigInteger(), 
                    // g
                    new BitString("9cfbfae39732e77f528794494fc8773661bc440151235607b137dc2bc34dfc3552e342463e45a4b72f835e38d7a038c4b7b02bc94e721c1f931a673ab16991816fd0d8df5a17f064e957e0693a66ffd990915fa35a6f0e705db9b40c374488583a678199c8ad9936f218f2ec3c7e7657901d54b32bb62674ec5b851998e056f1180b823183b3491ec48d60579365992246a4c7fd80207972c6d85126954eca0b907057c2e3f68117607273be19789f2fa370d8a243b88054de53d99a8b3ec69197b7b0de733c9af51683b2a52850924a008756885f37ee8f145eb3be34f15e10ba39e67822031f1a1b14fee28a7f97b328f9e435e7c175e7828ea4e13259cef1").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhHybridOneFlow,
                // mode
                ModeValues.SHA2,
                // digest
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("9696d8492f765c0f2875d43d5f1e0a96fc0e8887cace74a65ab9de1d").ToPositiveBigInteger(),
                // public static this party
                new BitString("650fe38cfbfc119b34803928f50c1c02920873c13f3d10cfc4902bc58d64698791a82f80bf3cd8b32a166a781a6b41b1b47fe9a73e9db7f7c41e64f19c816881aefe3274464fb084998feae7d3947c6ebaeeff490e04676c4c4a4f9ee832bf30e5075440b728e5588941d84832ad2d3abb2b6896c6c6f2a446adfd94f33a9bb835696a3388e89f5da062ee6d94b0fba84b9c6e1c9fc1f689657eb50a2bcd8313017ed13155d0a0807ecda28c3541b12263a7b3f51aeff98f37a230464ee58095c2485476065b85bf477bad7f9bd8a0d6fdd99b2b8279c0ca4d5d8c46d0ca9c06b597ea91b5a01b79c20ff104c629038416e8026589764416e62a561cb7824035").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("7fe5f3497ef3707bb77b0d7a9f84c960fabf130eaee5ee0570a74ec8").ToPositiveBigInteger(),
                // public static other party
                new BitString("a74b05bf6582c74a1a0d68b446ab1c3bab1bc33cc14bc32d35b26476bad1e61fc2855b13c9c38fc14c248be4cca4b8b724ff10c4393487a60347ab53bcf10fc9ae52b9bc1acf62f371348cbeca7917b9903a5795e4c7d406268e43467fdf5f5e27e405f7f4e77f5eac3b9b52bf86afe07140b49e7da4ba2c7bd8eb6e9e5975baa06638839f32fa8b6420754d01f73e9dd5baeafb1ccec2340c3e074610b84d77f370d8b3ecf51b89b2c2e886e22b4249551952877df8cffa93e6546897e247828ba6825d2b7c744fb850ade8e5d1d033e7d278648fb5f2d5d7032d4add89b765bd29a00b39b26149592ecbeafe7bc4e02d91fb7644db181a7788d215c5f99766").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("6b418999b6f4624f8fe210ef7532aae482c41301313164b9b744c5c8").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("577c620b2faa81a8d19cd4ef37bd9f5429fa43a68901d3bb7bd40788b1d77bd5faa3658c69a79e83eaf5d14847f598e869cdcf3446bb2b26395887df423405a07f5b0ee66e135cdc874ea6b49895d7d263090527acbce07391cb5d9875253c3a25416c88360481b54544c42da55d851d000f876a25643e4d1c07d8682f74c5e94a77ea19f133173e68004d6b2dfd80353e1bf909875c87e41f808e5608d133fa6fc8d7c12732350853958963c73f28ad1a885eb9c2b2fe522c6d5d8803d41083dd9bec9d6b148489c544bb1f3b57d9aedfb3a7af31d7881c5501ad72df7ae3a1dcef857108ff25046d5b366705c16629f4cbfda0a3bb500e4cb98b9fcfb77a9d").ToPositiveBigInteger(),
                // expected Z
                new BitString("1272785560d19f1ae2d8862e3dd107ccace44e421b4ce8cee3e2b18cbf2e56631b855b234303723489770c8da94b81bf98edd82b2f2c64af56462bb93b7819169872fc1ba3b3f60e3a8bb6f180917952809dae955b33d96e05d32f3f87ff78ee40d515fb97bdfc0789a64b2db06d8336c4ed1c45d4c6cfe948816f31292888b75ed3d9829574801d8d2ac389dbfdd037deb2a48be1889e6cb0b8467b548ef93fcfa19c6b65044d469ddec30f4458ea536fc5a71d345546fa55b0a4027ef81f4e105ed2b2b96f9b4d044503c0f5de4773da7ece1fe8ae7f7bff296cca265d0c667d5a553b8bd05030e0b4309ab841f7b30057ab25efdb972f166f6940bec4fd3d0ff78ac79a4ba1d6268431e7c96a7a66965a872770c6dfad6c9344bb3d0360511c9b35c3bb5d92e8564b18125d54d4f3535fde00a854df1eec6635d1af55b788c44032d6fea58f4e0def195f6da53a3c7265f548933fb3495eea71c3b67a7cbfa5243fb47f92e97782cf56abc7026ad5803601a6f905de058b6485708e547ac5e97a3b4f09c1b66f1df61c662237fa807146b17cb5d0bd03a61e3087c94648b643dbef4d3278707f150b7a2197ad9ff5874422d33951ae465ce4efcc3a7d69ec510cccd17983c389e788548fcf85dcf702dc1cd76febb4e50176ead746dd6b1bd620cd357c9af6681c59c5cf9dab3de8c1d0a0dce15d035691c732eb58e6a4de"),
                // expected Z hash
                new BitString("93e919dbd11d7258343e0db8167c724efdabe885e7423739977c14ae787a8abb1acf51ce3ebac88d5395775b5d45c7992a565015d65ca0ea4668c59a5f306e5c")
            },
            #endregion dhHybridOneFlow

            #region mqv1
            new object[]
            {
                // label
                "mqv1 sha2-256",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d1ad57e07b69aeeffaf5395bce38fca1e006d94e6c74f0571b32a0ddaf85558b88b960bc18260c0bce051e488cb06adc7eced487726efa9a0f5ab7c888145759667329930c519a29a500430be5a918fc969e776a33f85b1a064a80b7f648e65ff2ce8850029400799dc6bdc8f9835a496d55619cd95e09a77ef37f1b7377cb38210f85ad80d251eedccbed0adea17f1b287c08a6bdd3fc0a55f07b436c1dddfceb4cd6ea3a5522da0b52d647b7bc2008cd53b5f5e26c6b0799b84876b5e39bff722c961f920c4133ed6dcc496eecc503142315dcdb48c443fd7d7d9d5a687be8b43a5c43b776468d818d6170d5ffcbb7707dd14b9f35f89d37b1102e2579e71d").ToPositiveBigInteger(),
                    // q
                    new BitString("8ca04d69cc6464718f281b44b5151628b33a0af263cf0038988b84cd0c47401b").ToPositiveBigInteger(), 
                    // g
                    new BitString("3ef102354d9b8342f2f2637e08daf68879075c979c7a84209e8e3f38d7741b4a7fc99d51ddd4ed083a9f3f3b234c6b5302280852199cfee149aae278dd0501adca1fc5cfb3c73891adea92021fa0d3eb6597b6596475dce61e5865e76d2f87b6070c7add0592833825775b888117eb518ea4e0ddfa2ac725a65fb1ea401be0b2b90526918b50c875e5581e00b9787f52f975393916ce9d6aea877343d2359a0cf15a97d6c0076596eb3545d4c527721db99b94b5c2539e4dfedcd9b23797deb5b61657e0a0d13789d0ebbc2b61bbc63f79608d58a782f3c8cab6afaa85ff92ea41113f95e7c8fa915e27bc14461d2dae784825576a4ef2535a27c2f17ab1e23c").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.Mqv1,
                // mode
                ModeValues.SHA2,
                // digest
                DigestSizes.d256,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("8c12090bd99d9b19b2028bc127f322f6b24b7de8b35a159777f9b3068e12ef60").ToPositiveBigInteger(),
                // public static this party
                new BitString("1a49f141575f77576a2b06f61f43ef92effc3fefa6aaa2fc7734c826796f2fd6cd8eb8d6a85df6f15c1ff1dcb9bfb8396cc7649ce4b7c373736074cfaa02a43e3b92ad2ff23344844ebd07dd5f52f812ec7bf665fba22444fd58828f210d299257bebac9cbe58cb9c4ec40687d64f74b9632b7df8229a8880d6de21da502185c1668b3f20321d46956646934bc28c683d197b752e7d7ba6af3513f34f7860e0dec898ad8dce41e7c5c3b4ef024d09ae786481cf538a50dd69121d850d183c8f253c997bd3553caff1b47ab50d04d9cb3e84f06021c123588881225f0fcf2da50844adadb3c3df615bf1abbb74e145beb81d075ab6b3b34205be490d952fcbe0a").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("6c336e031ffbaf46eeefb1055f6f14920cac86f645c6fd05810bcec824b7bcaf").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("850f2453c22b4e3bfa780fc827bc8815edffb9b46ecadfa37c344304c2501dd1bfae2fb6d53a3c71ab34ae61032c0e78c8e07d3b6bb8d33b657a12aae32148ea4dcbf2561bb42f9ed64692b1c3c4ccc37abe9fbb9b54125f36e9790fa89fde2c09e382d8e59e234ced79be318c14e93b10733e3cb6fbea14a250ad21efbc65a80d5529e79eac82f1a048cf6cbd4fbfe34fa5c5fd10a5e376d8fa33d01e2b4d6340c402916b4861028358f7708e085b95b26ab2153d90dfd0991529ba857b772fd8c297186155634a98ed1848e9e7066324428d58cf68b72a4fde6f6dc86b22841e5cbfdac81daa60d300b8ee4096d34f6fa3359864ab949da9c3ee7d339ef839").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("6746ba08a35c4e185a91d4c0ab486ac9066fd52c8ddd52226a347e3b3b71d5b1").ToPositiveBigInteger(), 
                // public static other party
                new BitString("162960da5731dd062f9106f5570f17ea385137e9a2f37ba6fd94d260062a1cff138e1c615421d2ed6b8e34449ccae7a7965c92c6b7c50ec69c4fd9f952ca92c6421cfdd7bc9b27be71110d9b89930832b958c62293f5082775721630b8a42096697d569bec9e72f7412a6a3ccc6383740b78b573231cc270c98573eb9a89c81ab6b02f49ca9bdc8362419a1b1eeb1e527ece2103c3a82b98fe17c9d9261a90cd66f2ba9bcca6e4d778379bdf49cf647fc66a168e0339d9c6a3c161563f32a58f2dfbb2ba6321ffc7da22dd0037b0641e029eeb8dc5fb180ccda1e733883b4d4af8b51b0eda5b67f7655088d6edbd1cf152268e51daeb297d268619603ae96ba5").ToPositiveBigInteger(),
                // private ephemeral other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephemeral other party
                new BitString("00").ToPositiveBigInteger(),
                // expected Z
                new BitString("29d5f1688600fbe1a77f75058046fb0a9cf6d255138c6743d4183c134e6240e8fb66165d8d36a868080151c39025a6728aff27ce41ee77575c5ac9295e6c5005601e19958425efda7f436cb1606793fb4458408f3fd9bf2b8b6e82527298783ce1a684255f0b7e8e3cdb4feb749f286a12a5672b40dca9abf34f00fd620617f1ee5eb39b4d949f43143217e9f72be671593349d18d3f3d207ebeda32f3fa303f03fa55f0ca56f9567d68d356a8ec07ae18eca207ba070f8215a196a53d292a3fd5918b5d0072c29a5af82f131f2ad34cefd5e9f8fdf2950b83f7c6b30baeeefa509855073922144c9210a17b79f0c66229d4b716c19f78e03ebfec014a180559"),
                // expected Z hash
                new BitString("4a7f810569531686f98614eccc0ad6864fc0f698b08bbdf16e0ed5211b31e5fa")
            },
            new object[]
            {
                // label
                "mqv1 sha2-256 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d1ad57e07b69aeeffaf5395bce38fca1e006d94e6c74f0571b32a0ddaf85558b88b960bc18260c0bce051e488cb06adc7eced487726efa9a0f5ab7c888145759667329930c519a29a500430be5a918fc969e776a33f85b1a064a80b7f648e65ff2ce8850029400799dc6bdc8f9835a496d55619cd95e09a77ef37f1b7377cb38210f85ad80d251eedccbed0adea17f1b287c08a6bdd3fc0a55f07b436c1dddfceb4cd6ea3a5522da0b52d647b7bc2008cd53b5f5e26c6b0799b84876b5e39bff722c961f920c4133ed6dcc496eecc503142315dcdb48c443fd7d7d9d5a687be8b43a5c43b776468d818d6170d5ffcbb7707dd14b9f35f89d37b1102e2579e71d").ToPositiveBigInteger(),
                    // q
                    new BitString("8ca04d69cc6464718f281b44b5151628b33a0af263cf0038988b84cd0c47401b").ToPositiveBigInteger(), 
                    // g
                    new BitString("3ef102354d9b8342f2f2637e08daf68879075c979c7a84209e8e3f38d7741b4a7fc99d51ddd4ed083a9f3f3b234c6b5302280852199cfee149aae278dd0501adca1fc5cfb3c73891adea92021fa0d3eb6597b6596475dce61e5865e76d2f87b6070c7add0592833825775b888117eb518ea4e0ddfa2ac725a65fb1ea401be0b2b90526918b50c875e5581e00b9787f52f975393916ce9d6aea877343d2359a0cf15a97d6c0076596eb3545d4c527721db99b94b5c2539e4dfedcd9b23797deb5b61657e0a0d13789d0ebbc2b61bbc63f79608d58a782f3c8cab6afaa85ff92ea41113f95e7c8fa915e27bc14461d2dae784825576a4ef2535a27c2f17ab1e23c").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.Mqv1,
                // mode
                ModeValues.SHA2,
                // digest
                DigestSizes.d256,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("6746ba08a35c4e185a91d4c0ab486ac9066fd52c8ddd52226a347e3b3b71d5b1").ToPositiveBigInteger(),
                // public static this party
                new BitString("162960da5731dd062f9106f5570f17ea385137e9a2f37ba6fd94d260062a1cff138e1c615421d2ed6b8e34449ccae7a7965c92c6b7c50ec69c4fd9f952ca92c6421cfdd7bc9b27be71110d9b89930832b958c62293f5082775721630b8a42096697d569bec9e72f7412a6a3ccc6383740b78b573231cc270c98573eb9a89c81ab6b02f49ca9bdc8362419a1b1eeb1e527ece2103c3a82b98fe17c9d9261a90cd66f2ba9bcca6e4d778379bdf49cf647fc66a168e0339d9c6a3c161563f32a58f2dfbb2ba6321ffc7da22dd0037b0641e029eeb8dc5fb180ccda1e733883b4d4af8b51b0eda5b67f7655088d6edbd1cf152268e51daeb297d268619603ae96ba5").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("8c12090bd99d9b19b2028bc127f322f6b24b7de8b35a159777f9b3068e12ef60").ToPositiveBigInteger(), 
                // public static other party
                new BitString("1a49f141575f77576a2b06f61f43ef92effc3fefa6aaa2fc7734c826796f2fd6cd8eb8d6a85df6f15c1ff1dcb9bfb8396cc7649ce4b7c373736074cfaa02a43e3b92ad2ff23344844ebd07dd5f52f812ec7bf665fba22444fd58828f210d299257bebac9cbe58cb9c4ec40687d64f74b9632b7df8229a8880d6de21da502185c1668b3f20321d46956646934bc28c683d197b752e7d7ba6af3513f34f7860e0dec898ad8dce41e7c5c3b4ef024d09ae786481cf538a50dd69121d850d183c8f253c997bd3553caff1b47ab50d04d9cb3e84f06021c123588881225f0fcf2da50844adadb3c3df615bf1abbb74e145beb81d075ab6b3b34205be490d952fcbe0a").ToPositiveBigInteger(),
                // private ephemeral other party
                new BitString("6c336e031ffbaf46eeefb1055f6f14920cac86f645c6fd05810bcec824b7bcaf").ToPositiveBigInteger(), 
                // public ephemeral other party
                new BitString("850f2453c22b4e3bfa780fc827bc8815edffb9b46ecadfa37c344304c2501dd1bfae2fb6d53a3c71ab34ae61032c0e78c8e07d3b6bb8d33b657a12aae32148ea4dcbf2561bb42f9ed64692b1c3c4ccc37abe9fbb9b54125f36e9790fa89fde2c09e382d8e59e234ced79be318c14e93b10733e3cb6fbea14a250ad21efbc65a80d5529e79eac82f1a048cf6cbd4fbfe34fa5c5fd10a5e376d8fa33d01e2b4d6340c402916b4861028358f7708e085b95b26ab2153d90dfd0991529ba857b772fd8c297186155634a98ed1848e9e7066324428d58cf68b72a4fde6f6dc86b22841e5cbfdac81daa60d300b8ee4096d34f6fa3359864ab949da9c3ee7d339ef839").ToPositiveBigInteger(),
                // expected Z
                new BitString("29d5f1688600fbe1a77f75058046fb0a9cf6d255138c6743d4183c134e6240e8fb66165d8d36a868080151c39025a6728aff27ce41ee77575c5ac9295e6c5005601e19958425efda7f436cb1606793fb4458408f3fd9bf2b8b6e82527298783ce1a684255f0b7e8e3cdb4feb749f286a12a5672b40dca9abf34f00fd620617f1ee5eb39b4d949f43143217e9f72be671593349d18d3f3d207ebeda32f3fa303f03fa55f0ca56f9567d68d356a8ec07ae18eca207ba070f8215a196a53d292a3fd5918b5d0072c29a5af82f131f2ad34cefd5e9f8fdf2950b83f7c6b30baeeefa509855073922144c9210a17b79f0c66229d4b716c19f78e03ebfec014a180559"),
                // expected Z hash
                new BitString("4a7f810569531686f98614eccc0ad6864fc0f698b08bbdf16e0ed5211b31e5fa")
            },
            #endregion mqv1

            #region dhOneFlow
            new object[]
            {
                // label
                "dhOneFlow sha2-512",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("a6e7b1b0a9d1f28cb17c90be36bed46547ad42ff68d3a7d81412d4d7b1b9218e347e86cc60977b2895a2bbd244d3e70ee582ff927ab7c7f94e6576a8d36e4fb35f6303b9b1771e656fc73df1342080ca205119e2abeb9b731b7cb2ce15e1d8f790dfc8b317a538d34e88068f2d0e7252bfcf712129793ac77394b4a8c657e447802bdaaecc413d9c41b1deb39dd99d69d97ca564017051dc0f185529b9cecbe4b47f4348405b5de2a260ca79d11e3e596fcdb5960e67f7368b792306bd9340c25da28ef53d65887d0ff53e9e005d798bcb33842d1f80eb1990a50792fc56385d7e16ffbe99d6fbb2c9c0e0fa45c44779f0f264f820647d4ff3b5da9335cebc29").ToPositiveBigInteger(),
                    // q
                    new BitString("fac0aab80df75b5e7f6edc4483d29a4944adb01f61772f73d62ce295").ToPositiveBigInteger(), 
                    // g
                    new BitString("95944942f4d63f949e4c70b31bd79b9a3e85d1892b1a588c20fe535028e32c8953092b59a747ba69767c479dbc8d8a77a0d42dacb71654926a9579a91586e37b05ae080544755534e72d14193d1e9a4103f8c4a8838de63f38dbde9643e0e93019af83a9173846018f1ed98f53298791e4fd41fa3b07e1b977ba368bff2d10cc957e9edbfbcbbb5ce2e27f33454f95ff6bdbfb0ffe227d3704f2383380b3594b70a9cc98a2e326118e32831884679cf0f0bd0500dbc15bb111eb7e1cf09ba61f344cd29ca62803555b25f37d6bfb1190666b42f0d121cd44ead48675f0812f9baae4e55afec6f97de315bf7d92ef7be1a27545fc1be4959ad7624da9d04d0aae").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhOneFlow,
                // mode
                ModeValues.SHA2,
                // digest
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("00").ToPositiveBigInteger(),
                // public static this party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("ee02c7f0d41fc0ad78e4f7a8900594597f57ccce709196198da31298").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("78e892dae30c1d12fb45ce77136359250cf362f204f903bd5a4cf07b1d5ce6b55cd60e77b712d0b22b4c4bb97247c2d0ebf5e59fec00aebf69cb932506994c73b6b78b028174f806dcb97a743106d1d7215a3a35861a46363089cd8cb3fe28ac9d50e2c1c0419ee6588d8b8cec8ced474f2545573fe589366f537d2609bdaa04b44330fe7c956e47b3ab6ca0d2bee4b926784111ba1c9df9ee56e09e12079010c51c2b52dd70d7ff5c2e1ee346c679104da94a602010cb5b4b6daca0ae4341755c4ea09c8bfb3e9e761402981999ac47e94d40197833c31156a5a81b020e15995e2b29b26e653236ace405a32a367b5a2231d5342d1a74743f27d761bc75c7cc").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("77e448685cf5da5a5bbf704375a1e11ce1737c6130724378b678ff08").ToPositiveBigInteger(), 
                // public static other party
                new BitString("4e3bf92fb2dacd992f4bec43f1253cba5cb18f4eb5f899ffeb474c265c34fc1e022c8eb7e2debb2396afe6b834077ee868e37e6dd0c55b1b64be2ef061212c44afa2545cd605b14f1de06976d6e618df400ea198eabc7c0ec36ab42478f641c9c190152961a52a619295df59cc9d4fcf8efd38289b1adc4988d8bd9eb32bc984b8c4c2610eca190d6f358c20edde783bfa9a4f8d57dfe64fe8cb96e9c15ac6c256158be7dbe605a09c3da078fecde799c2e6427d8033f8468ea99a896deafc19255fe7abc25cd07c09c1fc48a382e264dfed221b73f98223683cba75cc5511195158c01d92f433d45fbaacbb3c76e0a05cc6d0ffbdfb60894e4ae6e9a703bbf6").ToPositiveBigInteger(),
                // private ephemeral other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephemeral other party
                new BitString("00").ToPositiveBigInteger(),
                // expected Z
                new BitString("74cdcad2e2656ca13794df5414302418f233469cd345bcb53c913e5f29764fc65fad2ee033cafba646e3bfd65d47e4beffbec1ba321c088ad412b7afdcc23b9c20ff0f8fec5f569ac7024b229e54ad0524a85e99d0b53d967a509a31a57a9f29293c220268a87e2a319e1d30f1dfd44b3a60b0c81012c83f4211087466fc15282a4836be27a8bf7884e5148a8fcb83d659b4e8f9e0917092f93ed5184d7281c3c9340a1080234245042069bda971c40496f1a7fb3b8c8fc42b3ce8935ae63471ba3b5905da0f6cd42431415746135bfdeb46e6ef475ac9d58e6d0e285182c7e00e8bf036bfee69dd841ae6cf6a6ec5c11f0b137c12fd289dc63051ab7457bdcd"),
                // expected Z hash
                new BitString("1e26586fd9e758d1116cc37b6fba8fd81fc6c93f165a9b1555be07770eb04d00841f904074647ce8b964378f06317bd2e5912ae1b7f07e1852ede8a5a0a3f2f6")
            },
            new object[]
            {
                // label
                "dhOneFlow sha2-512 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("a6e7b1b0a9d1f28cb17c90be36bed46547ad42ff68d3a7d81412d4d7b1b9218e347e86cc60977b2895a2bbd244d3e70ee582ff927ab7c7f94e6576a8d36e4fb35f6303b9b1771e656fc73df1342080ca205119e2abeb9b731b7cb2ce15e1d8f790dfc8b317a538d34e88068f2d0e7252bfcf712129793ac77394b4a8c657e447802bdaaecc413d9c41b1deb39dd99d69d97ca564017051dc0f185529b9cecbe4b47f4348405b5de2a260ca79d11e3e596fcdb5960e67f7368b792306bd9340c25da28ef53d65887d0ff53e9e005d798bcb33842d1f80eb1990a50792fc56385d7e16ffbe99d6fbb2c9c0e0fa45c44779f0f264f820647d4ff3b5da9335cebc29").ToPositiveBigInteger(),
                    // q
                    new BitString("fac0aab80df75b5e7f6edc4483d29a4944adb01f61772f73d62ce295").ToPositiveBigInteger(), 
                    // g
                    new BitString("95944942f4d63f949e4c70b31bd79b9a3e85d1892b1a588c20fe535028e32c8953092b59a747ba69767c479dbc8d8a77a0d42dacb71654926a9579a91586e37b05ae080544755534e72d14193d1e9a4103f8c4a8838de63f38dbde9643e0e93019af83a9173846018f1ed98f53298791e4fd41fa3b07e1b977ba368bff2d10cc957e9edbfbcbbb5ce2e27f33454f95ff6bdbfb0ffe227d3704f2383380b3594b70a9cc98a2e326118e32831884679cf0f0bd0500dbc15bb111eb7e1cf09ba61f344cd29ca62803555b25f37d6bfb1190666b42f0d121cd44ead48675f0812f9baae4e55afec6f97de315bf7d92ef7be1a27545fc1be4959ad7624da9d04d0aae").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhOneFlow,
                // mode
                ModeValues.SHA2,
                // digest
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("77e448685cf5da5a5bbf704375a1e11ce1737c6130724378b678ff08").ToPositiveBigInteger(),
                // public static this party
                new BitString("4e3bf92fb2dacd992f4bec43f1253cba5cb18f4eb5f899ffeb474c265c34fc1e022c8eb7e2debb2396afe6b834077ee868e37e6dd0c55b1b64be2ef061212c44afa2545cd605b14f1de06976d6e618df400ea198eabc7c0ec36ab42478f641c9c190152961a52a619295df59cc9d4fcf8efd38289b1adc4988d8bd9eb32bc984b8c4c2610eca190d6f358c20edde783bfa9a4f8d57dfe64fe8cb96e9c15ac6c256158be7dbe605a09c3da078fecde799c2e6427d8033f8468ea99a896deafc19255fe7abc25cd07c09c1fc48a382e264dfed221b73f98223683cba75cc5511195158c01d92f433d45fbaacbb3c76e0a05cc6d0ffbdfb60894e4ae6e9a703bbf6").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("00").ToPositiveBigInteger(), 
                // public static other party
                new BitString("00").ToPositiveBigInteger(),
                // private ephemeral other party
                new BitString("ee02c7f0d41fc0ad78e4f7a8900594597f57ccce709196198da31298").ToPositiveBigInteger(), 
                // public ephemeral other party
                new BitString("78e892dae30c1d12fb45ce77136359250cf362f204f903bd5a4cf07b1d5ce6b55cd60e77b712d0b22b4c4bb97247c2d0ebf5e59fec00aebf69cb932506994c73b6b78b028174f806dcb97a743106d1d7215a3a35861a46363089cd8cb3fe28ac9d50e2c1c0419ee6588d8b8cec8ced474f2545573fe589366f537d2609bdaa04b44330fe7c956e47b3ab6ca0d2bee4b926784111ba1c9df9ee56e09e12079010c51c2b52dd70d7ff5c2e1ee346c679104da94a602010cb5b4b6daca0ae4341755c4ea09c8bfb3e9e761402981999ac47e94d40197833c31156a5a81b020e15995e2b29b26e653236ace405a32a367b5a2231d5342d1a74743f27d761bc75c7cc").ToPositiveBigInteger(),
                // expected Z
                new BitString("74cdcad2e2656ca13794df5414302418f233469cd345bcb53c913e5f29764fc65fad2ee033cafba646e3bfd65d47e4beffbec1ba321c088ad412b7afdcc23b9c20ff0f8fec5f569ac7024b229e54ad0524a85e99d0b53d967a509a31a57a9f29293c220268a87e2a319e1d30f1dfd44b3a60b0c81012c83f4211087466fc15282a4836be27a8bf7884e5148a8fcb83d659b4e8f9e0917092f93ed5184d7281c3c9340a1080234245042069bda971c40496f1a7fb3b8c8fc42b3ce8935ae63471ba3b5905da0f6cd42431415746135bfdeb46e6ef475ac9d58e6d0e285182c7e00e8bf036bfee69dd841ae6cf6a6ec5c11f0b137c12fd289dc63051ab7457bdcd"),
                // expected Z hash
                new BitString("1e26586fd9e758d1116cc37b6fba8fd81fc6c93f165a9b1555be07770eb04d00841f904074647ce8b964378f06317bd2e5912ae1b7f07e1852ede8a5a0a3f2f6")
            },
            #endregion dhOneFlow

            #region dhStatic
            new object[]
            {
                // label
                "dhStatic sha2-512",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("dcca1511b2313225f52116e1542789e001f0425bccc7f366f7406407f1c9fa8be610f1778bb170be39dbb76f85bf24ce6880adb7629f7c6d015e61d43fa3ee4de185f2cfd041ffde9d418407e15138bb021daeb35f762d1782acc658d32bd4b0232c927dd38fa097b3d1859fa8acafb98f066608fc644ec7ddb6f08599f92ac1b59825da8432077def695646063c20823c9507ab6f0176d4730d990dbbe6361cd8b2b94d3d2f329b82099bd661f42950f403df3ede62a33188b02798ba823f44b946fe9df677a0c5a1238eaa97b70f80da8cac88e092b1127060ffbf45579994011dc2faa5e7f6c76245e1cc312231c17d1ca6b19007ef0db99f9cb60e1d5f69").ToPositiveBigInteger(),
                    // q
                    new BitString("898b226717ef039e603e82e5c7afe48374ac5f625c54f1ea11acb57d").ToPositiveBigInteger(), 
                    // g
                    new BitString("5ef7b88f2df60139351dfbfe1266805fdf356cdfd13a4da0050c7ede246df59f6abf96ade5f2b28ffe88d6bce7f7894a3d535fc82126ddd424872e16b838df8c51e9016f889c7c203e98a8b631f9c72563d38a49589a0753d358e783318cefd9677c7b2dbb77d6dce2a1963795ca64b92d1c9aac6d0e8d431de5e50060dff78689c9eca1c1248c16ed09c7ad412a17406d2b525aa1cabb237b9734ec7b8ce3fae02f29c5efed30d69187da109c2c9fe2aadbb0c22af54c616655000c431c6b4a379763b0a91658efc84e8b06358c8b4f213710fd10172cf39b830c2dd84a0c8ab82516ecab995fa4215e023e4ecf8074c39d6c88b70d1ee4e96fdc20ea115c32").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhStatic,
                // mode
                ModeValues.SHA2,
                // digest
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("1adf389e2ac131ea502ee38462067b11b1e2648245c26d5d00a11f23").ToPositiveBigInteger(),
                // public static this party
                new BitString("1fc1da341d1a846a96b7be24340f877dd010aa0356d5ad58aae9c7b08f749a32235110b5d88eb5dbfa978d27ecc530f02d3114005b64b1c0e024cb8ae21698bca9e60d42808622f181c56e1de7a96e6efee9d66567e91b977042c7e3d0448f05fb77f522b9bfc8d33cc3c31ed3b31f0fecb6db4f6ea311e77afdbcd47aee1bb150f216873578fb96468e8f9f3de8efbfce75624b1df05322a34f1463e839e8984c4ad0a96e1ac842e5318cc23c062a8ca171b8d575980dde7fc56f1536523820d43192bfd51e8e228978aca5b94472f339caeb9931b42be301268bc99789c9b25571c3c0e4cb3f007f1a511cbb53c8519cdd1302abca6c0f34f96739f17ff48b").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("1433e0b5a917b60a3023f2f8aa2c2d70d2968aba9aeac81540b8fce6").ToPositiveBigInteger(),
                // public static other party
                new BitString("95dd338d29e5710492b918317b72a36936e1951a2ee5a5591699c0486d0d4f9bdd6d5a3f6b98890c62b37652d36e712111e68a735537250699efe330537391fbc2c548bc5ac3e5b23386c3eef5eb43c099d70a5202687e83964248fca91f40908e8fb3319315f6d2606d7f7cd52cc6e7c5843afb22519cf0f0f9d3a0a4e8c88899efede7364351fb6a363ee717e5445adab4c931a6483997b87dad83677e4d1d3a7775e0f6d00fdf73c7ad801e665a0e5a796d0a0380a19fa182efc8a04f5e4db90d1a8637f95db16436bdc8f3fc096c4ff7f234be8fef479ac4b0dc4b77263e07d9959de0f1bf3f0ae3d9d50e4b89c99e3ea1217343dd8c6581acc4959c91d3").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // expected Z
                new BitString("08ff33bb2ecff49a7d4a7912aeb1bb6ab511641b4a76770c8cc1bcc233343dfe700d11813d2c9ed23b211ca9e8786921edca283c68b16153fa01e91ab82c90ddab4a95816770a98710e14c92ab83b6e46e1e426ee852430d6187daa3720a6bcd73235c6b0f941f3364f50420551a4bfeafe2bc438505a59a4a40daca7a895a73db575c74c13a23ad8832957d582d38f0a6165fb0d7e9b8799e42fd3220e332e98185a0c9429757b2d0d02c17dbaa1ff6ed93d7e73e241eaed90caf394d2bc6570f18c81f2be5d01a2ca99ff142b5d963f9f500325e7556f95849b3ffc7479486be1d4596a3106bd5cb4f61c57ec5f100fb7a0c82a10b82526a97d1d97d98eaf6"),
                // expected Z hash
                new BitString("90df31241ea4b4bc0fec4ffafe513fea0a83f442397b1bbeb2a88003cc9aadf467b262c9e42105e26a8424ff1f7b04c33a8be71736972c2d595dc545233fbece")
            },
            new object[]
            {
                // label
                "dhStatic sha2-512 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("dcca1511b2313225f52116e1542789e001f0425bccc7f366f7406407f1c9fa8be610f1778bb170be39dbb76f85bf24ce6880adb7629f7c6d015e61d43fa3ee4de185f2cfd041ffde9d418407e15138bb021daeb35f762d1782acc658d32bd4b0232c927dd38fa097b3d1859fa8acafb98f066608fc644ec7ddb6f08599f92ac1b59825da8432077def695646063c20823c9507ab6f0176d4730d990dbbe6361cd8b2b94d3d2f329b82099bd661f42950f403df3ede62a33188b02798ba823f44b946fe9df677a0c5a1238eaa97b70f80da8cac88e092b1127060ffbf45579994011dc2faa5e7f6c76245e1cc312231c17d1ca6b19007ef0db99f9cb60e1d5f69").ToPositiveBigInteger(),
                    // q
                    new BitString("898b226717ef039e603e82e5c7afe48374ac5f625c54f1ea11acb57d").ToPositiveBigInteger(), 
                    // g
                    new BitString("5ef7b88f2df60139351dfbfe1266805fdf356cdfd13a4da0050c7ede246df59f6abf96ade5f2b28ffe88d6bce7f7894a3d535fc82126ddd424872e16b838df8c51e9016f889c7c203e98a8b631f9c72563d38a49589a0753d358e783318cefd9677c7b2dbb77d6dce2a1963795ca64b92d1c9aac6d0e8d431de5e50060dff78689c9eca1c1248c16ed09c7ad412a17406d2b525aa1cabb237b9734ec7b8ce3fae02f29c5efed30d69187da109c2c9fe2aadbb0c22af54c616655000c431c6b4a379763b0a91658efc84e8b06358c8b4f213710fd10172cf39b830c2dd84a0c8ab82516ecab995fa4215e023e4ecf8074c39d6c88b70d1ee4e96fdc20ea115c32").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhStatic,
                // mode
                ModeValues.SHA2,
                // digest
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("1433e0b5a917b60a3023f2f8aa2c2d70d2968aba9aeac81540b8fce6").ToPositiveBigInteger(),
                // public static this party
                new BitString("95dd338d29e5710492b918317b72a36936e1951a2ee5a5591699c0486d0d4f9bdd6d5a3f6b98890c62b37652d36e712111e68a735537250699efe330537391fbc2c548bc5ac3e5b23386c3eef5eb43c099d70a5202687e83964248fca91f40908e8fb3319315f6d2606d7f7cd52cc6e7c5843afb22519cf0f0f9d3a0a4e8c88899efede7364351fb6a363ee717e5445adab4c931a6483997b87dad83677e4d1d3a7775e0f6d00fdf73c7ad801e665a0e5a796d0a0380a19fa182efc8a04f5e4db90d1a8637f95db16436bdc8f3fc096c4ff7f234be8fef479ac4b0dc4b77263e07d9959de0f1bf3f0ae3d9d50e4b89c99e3ea1217343dd8c6581acc4959c91d3").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("1adf389e2ac131ea502ee38462067b11b1e2648245c26d5d00a11f23").ToPositiveBigInteger(),
                // public static other party
                new BitString("1fc1da341d1a846a96b7be24340f877dd010aa0356d5ad58aae9c7b08f749a32235110b5d88eb5dbfa978d27ecc530f02d3114005b64b1c0e024cb8ae21698bca9e60d42808622f181c56e1de7a96e6efee9d66567e91b977042c7e3d0448f05fb77f522b9bfc8d33cc3c31ed3b31f0fecb6db4f6ea311e77afdbcd47aee1bb150f216873578fb96468e8f9f3de8efbfce75624b1df05322a34f1463e839e8984c4ad0a96e1ac842e5318cc23c062a8ca171b8d575980dde7fc56f1536523820d43192bfd51e8e228978aca5b94472f339caeb9931b42be301268bc99789c9b25571c3c0e4cb3f007f1a511cbb53c8519cdd1302abca6c0f34f96739f17ff48b").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // expected Z
                new BitString("08ff33bb2ecff49a7d4a7912aeb1bb6ab511641b4a76770c8cc1bcc233343dfe700d11813d2c9ed23b211ca9e8786921edca283c68b16153fa01e91ab82c90ddab4a95816770a98710e14c92ab83b6e46e1e426ee852430d6187daa3720a6bcd73235c6b0f941f3364f50420551a4bfeafe2bc438505a59a4a40daca7a895a73db575c74c13a23ad8832957d582d38f0a6165fb0d7e9b8799e42fd3220e332e98185a0c9429757b2d0d02c17dbaa1ff6ed93d7e73e241eaed90caf394d2bc6570f18c81f2be5d01a2ca99ff142b5d963f9f500325e7556f95849b3ffc7479486be1d4596a3106bd5cb4f61c57ec5f100fb7a0c82a10b82526a97d1d97d98eaf6"),
                // expected Z hash
                new BitString("90df31241ea4b4bc0fec4ffafe513fea0a83f442397b1bbeb2a88003cc9aadf467b262c9e42105e26a8424ff1f7b04c33a8be71736972c2d595dc545233fbece")
            },
            #endregion dhStatic
        };

        [Test]
        [TestCaseSource(nameof(_test_componentOnly))]
        public void ShouldTestComponentOnlyCorrectly(
            string label,
            FfcDomainParameters domainParameters,
            FfcScheme scheme,
            ModeValues modeValue,
            DigestSizes digestSize,
            KeyAgreementRole keyAgreementRole,
            BitString thisPartyId,
            BigInteger thisPartyPrivateStaticKey,
            BigInteger thisPartyPublicStaticKey,
            BigInteger thisPartyPrivateEphemKey,
            BigInteger thisPartyPublicEphemKey,
            BitString otherPartyId,
            BigInteger otherPartyPrivateStaticKey,
            BigInteger otherPartyPublicStaticKey,
            BigInteger otherPartyPrivateEphemKey,
            BigInteger otherPartyPublicEphemKey,
            BitString expectedZ,
            BitString expectedHashZ
        )
        {
            var vPartySharedInformation = 
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(
                    domainParameters, 
                    otherPartyId, 
                    new FfcKeyPair(otherPartyPublicStaticKey),
                    new FfcKeyPair(otherPartyPublicEphemKey), 
                    null, 
                    null, 
                    null
                );

            // The DSA sha mode determines which hash function to use in a component only test
            _dsa
                .SetupGet(s => s.Sha)
                .Returns(new ShaFactory().GetShaInstance(new HashFunction(modeValue, digestSize)));
            _dsa
                .Setup(s => s.GenerateKeyPair(domainParameters))
                .Returns(() => new FfcKeyPairGenerateResult(new FfcKeyPair(0, 0)));

            var kas = _subject
                .WithKeyAgreementRole(keyAgreementRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesFfc(scheme, FfcParameterSet.Fb))
                .WithAssurances(KasAssurance.None)
                .WithPartyId(thisPartyId)
                .BuildNoKdfNoKc()
                .Build();

            kas.SetDomainParameters(domainParameters);
            kas.ReturnPublicInfoThisParty();

            if (kas.Scheme.StaticKeyPair != null)
            {
                kas.Scheme.StaticKeyPair.PrivateKeyX = thisPartyPrivateStaticKey;
                kas.Scheme.StaticKeyPair.PublicKeyY = thisPartyPublicStaticKey;
            }
            if (kas.Scheme.EphemeralKeyPair != null)
            {
                kas.Scheme.EphemeralKeyPair.PrivateKeyX = thisPartyPrivateEphemKey;
                kas.Scheme.EphemeralKeyPair.PublicKeyY = thisPartyPublicEphemKey;
            }

            var result = kas.ComputeResult(vPartySharedInformation);

            Assume.That(result.Success, nameof(result.Success));
            Assert.AreEqual(expectedZ, result.Z, nameof(result.Z));
            Assert.AreEqual(expectedHashZ, result.Tag, nameof(result.Tag));
        }

        private static object[] _test_noKeyConfirmation = new object[]
        {
            #region dhHybrid1
            #region hmac
            new object[]
            {
                // label
                "dhHybrid1 hmac512 sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("999acad0cb8401f730b65cc738ef28128c600cfeb0d0e8782be77d029b20a6d2e06977f817e9ba43e8ab9375b29ff43203cc4942297c661bfeb1af26f8f36e151d5466929f1e49ee9685d7154cfbb2726e7485ca49277be275e51497d986ada1066b3c48621ac80f3007e4f1f451ae2d825477eb8d1c95d60f39dc89366a02ce1470e97bc5d87bbfa69ab19a35fcc3a09ccff4436c84f2ca54535580dcfa1d60b97167915b1c772974cb62f34249035af13c1b7391b050da900bfa51e4c17d22a2e095e6fb2ee6b1fddb0e16dd1fd831c3ab5f9b46ddd6f104793db24cb27b1c026cb3ae109abe215be0f3694aa0be95a4613c89dffd77aaf893988a2d17589f").ToPositiveBigInteger(),
                    // q
                    new BitString("dfbb41276faa5b94335b16ebcb91cdd410d2253ca58f479d7df68a39").ToPositiveBigInteger(), 
                    // g
                    new BitString("254d1751afbbf188c23bf94961256c9448c669bafda66ebcdcb76520747d5232032ec2ec9d81d509f72791f9c9f419a5c2ca9e98b9fdd598859865996eee7a445c49daf2954ff9bf0304a1f1a5978f3081ca04bf9a2586c9e5d58bc10d1be0e071dac14adaf60a2ddd0fcf9199c2ca3af46fad032a1f25f412a08495e70e5c7110d81f23efbcfc2ce3c4cfdef50f27ae65a12edcf507baa497d05771168c9ac96da49a2af708fab002133cb249dafa259cf7c644b2b98c4104debdd4082f8013fdc31d5410634d780413de1dc9c4cd07daa957d9f6be4ea0e2cc41af8d7599a5b0db52bf22bad18f946a08747182d5e7cb067c9a8e9261910f3c267f680df0f5").ToPositiveBigInteger()
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
                KeyAgreementMacType.HmacSha2D512,
                // key length
                112,
                // tag length
                112,
                // noKeyConfirmationNonce
                new BitString("cbbaedffb7f967337ad0c0ccff8edcb1"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("2d06746f85db0a3da0788a0a43ea690b99f5ca46b660f9b1780ed038").ToPositiveBigInteger(),
                // public static this party
                new BitString("7642206d7140d374c4e7244f28576615f91cef916c0b8e57959e3b86137ca1224c2454c492c65fcdd4ca42e458d2ae6821e2a6d8446b8a667ca10cfcb4ecd64d67cdcff11b529d06275e6a415ecea6a193c07ad8dfa4b6a09c5885a4b7d4df3382a6c49e38e697f9e523bae1fe218b79316f937d2f62fd43b220ba77eee648b4dcca7f86f796789e7b53de6a8292b66b04a50048f67b3cc439bc8bc7fa8124e3c8e24a08ecdec5585b0cc6ce9f88f9f40fd56dcb581c06ca8ff0b99dd2a3835a94117f959e747b593a694dd14e184845be104a5217819bff2d3749af525d2b21cbabe5685867bd1dd0a8a3c6ab4b10e106efe4a4b17581e07a794427bf4225b0").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("b3e8c72dd64466fe9eca11bc38c3210e4855b5b28f1f08eb1dce241f").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("6a22c61b77f5103d0e2e4a432fd37c53dd7dddff2e673995af3ea187df8616d7698a3d39d72fba3e523bd1a2d33d3404e208cf93d4dcdd9601fd6fcf5e251f99461f0b1636cdce0cf55e9f471dcffc0ce8360a50f60c88e75b177f54f28a191611df4ffdcde306628f57c28cec8b62511c9a8e67fdcb06ea9331e364d9987adee417c70c05b8654c5f771ade98eca46e2bdbabb99dff197a0296c245b864edeef53b3a808fbb97477586e73a6271b46f42cd18f20498fd194e52e6284dcf004d9545129a270e796a3849c6cb6ef2fdf831552cc54f16964fa42b7b23c78fa590d36d1c89aca7328c0ec2ae0025196deb7da21dceb24e18112e220d7423d99335").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("450adfedd9f65db1d2b90ab500cba480eaf49bf11e01bbfeb6a7a841").ToPositiveBigInteger(), 
                // public static other party
                new BitString("597758a29f5f2d2988884467b162955a0e9c27f2e8ced1d504f7d4edd5705330f6441f0eaf05ac7bac9251de1a56c96f9c33559d280748404e695b3ef045dce3df0c575449db627e41c8060c887f068f9d3496131f7d68a71175d1c0b5c50d27c3b1ada0ec3348ca27dc7e2a83c82550aee75b119825d834b29bfa6bbe40cf8b243b07779b0861711f57eff5bbcbe5a4612775dfafbb0cedbdf6a1294933dba82684b0a7b77a5f35b56d702fc4cb830f9934b41cbb02fbce906fdd25dae6f6ea168251a650b61830efaacc210efa470d7c35e8df187f7cb6aff64b680b6e3b13fb2f271de4a351854a502382760120e21988c537b6f7eb7c93bb3fc1ec246496").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("c0aee2c27dadae552c5049589fecb63c88538327b530ce576d5671fb").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("50095b929b8355794e6b4945f5cd2b5a92d724a82a3b5d488f10f5baa1a90ab0f21d1792c0004b1d404c6ce8239a9e07db9e8b8ffed5eb67d4127956a37b7969d9d734cef25a8ada20e5357813675bd28a1f43efe33fd366659ae78fd5765a52e32321f2aab891457dbe26a01dfd1a4abb1fc6f9b775b4f461b3dcc1b4023caa3453b9b7fb8afddcf33f923bf2111fcf3e3663989e24a24673d5c0a935a81f2f05859031d1332161d86a380f062cb2b5b5bc75b9959c1f4ddb96c825d3599c241e81db4f86e00faa59b9c6f944574acc860a887d4c505133009ac3a6cbad36a3eea052ac5761c2349966b24bad1e3b71f8adf6ed902893261fe5eb77f06bdf02").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("34e3113913999902b487266b0a313f6a428614890c06c6be7cd313b76692c8830c9372c1f238dd748c7d9842e106fc781d778c6151e2e2082a645705abaabff3400ec8ca1d3639c573e7cf75f691f04915d571e5190f6de0b9a69b1cc44283d33b58e0359d3529ec0318202e2473cfcae9290089842ea76d0e8ae7fa305f2b3b1094263031c47fd2228c2b167ed8c3f3112ec3f741b7cf5ff23812face12a749ef7361dacceae05c75711ec977ea4b47aff3cdf6e56eb0fdd6f9d14300f2dffb0c57e4c4dc9d27d262131a699138a51b0c55f9db6cdfa5d727f1b98f0fba61170ef34ff01703b90bbf8d6ec41ccd65e4532b63ab808c6f2fcf0f6327170ba3c43c2561dd93211aeadbca281ae2e2fc6f2f2e60142bc4b708ff475f0dc7e52205e55942b2cae6d88ea87638ae6c81ee99453fc9e042105f715402fb52cb957cfe4a80b22afd4c28fad743a90bdd62c75d40206e946216068e6980acbd487f26633d36babdbfa3428ecfae8020374b5b74de65d9e870502ca1bbd51a584880c17a2d25d624ffde839364f7e60c8121e933e12e0c8538a9b7dc14464abab9ac555ac7c533cd8e4c8a2617b1045bccd58c37c2eb426f1741a4d6bd3dfc5b66e407be7922890330f7fb47c3d5080446d38632e33908901ea8e6882262747d07e46f8d320a97a09f1f662f010a6f14c596ecd0ea673cc03e843061fbe4601dc30bffcc"),
                // expected oi
                new BitString("a1b2c3d4e543415653696492c27a2d71c1edc2248e0f01bb0d040b228ab9"),
                // expected dkm
                new BitString("125f52aa43b93d859206ea35ce9b"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765cbbaedffb7f967337ad0c0ccff8edcb1"),
                // expected tag
                new BitString("0bee641989b1a0aa35ac771c9a95")
            },
            new object[]
            {
                // label
                "dhHybrid1 hmac512 sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("999acad0cb8401f730b65cc738ef28128c600cfeb0d0e8782be77d029b20a6d2e06977f817e9ba43e8ab9375b29ff43203cc4942297c661bfeb1af26f8f36e151d5466929f1e49ee9685d7154cfbb2726e7485ca49277be275e51497d986ada1066b3c48621ac80f3007e4f1f451ae2d825477eb8d1c95d60f39dc89366a02ce1470e97bc5d87bbfa69ab19a35fcc3a09ccff4436c84f2ca54535580dcfa1d60b97167915b1c772974cb62f34249035af13c1b7391b050da900bfa51e4c17d22a2e095e6fb2ee6b1fddb0e16dd1fd831c3ab5f9b46ddd6f104793db24cb27b1c026cb3ae109abe215be0f3694aa0be95a4613c89dffd77aaf893988a2d17589f").ToPositiveBigInteger(),
                    // q
                    new BitString("dfbb41276faa5b94335b16ebcb91cdd410d2253ca58f479d7df68a39").ToPositiveBigInteger(), 
                    // g
                    new BitString("254d1751afbbf188c23bf94961256c9448c669bafda66ebcdcb76520747d5232032ec2ec9d81d509f72791f9c9f419a5c2ca9e98b9fdd598859865996eee7a445c49daf2954ff9bf0304a1f1a5978f3081ca04bf9a2586c9e5d58bc10d1be0e071dac14adaf60a2ddd0fcf9199c2ca3af46fad032a1f25f412a08495e70e5c7110d81f23efbcfc2ce3c4cfdef50f27ae65a12edcf507baa497d05771168c9ac96da49a2af708fab002133cb249dafa259cf7c644b2b98c4104debdd4082f8013fdc31d5410634d780413de1dc9c4cd07daa957d9f6be4ea0e2cc41af8d7599a5b0db52bf22bad18f946a08747182d5e7cb067c9a8e9261910f3c267f680df0f5").ToPositiveBigInteger()
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
                KeyAgreementMacType.HmacSha2D512,
                // key length
                112,
                // tag length
                112,
                // noKeyConfirmationNonce
                new BitString("cbbaedffb7f967337ad0c0ccff8edcb1"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("450adfedd9f65db1d2b90ab500cba480eaf49bf11e01bbfeb6a7a841").ToPositiveBigInteger(),
                // public static this party
                new BitString("597758a29f5f2d2988884467b162955a0e9c27f2e8ced1d504f7d4edd5705330f6441f0eaf05ac7bac9251de1a56c96f9c33559d280748404e695b3ef045dce3df0c575449db627e41c8060c887f068f9d3496131f7d68a71175d1c0b5c50d27c3b1ada0ec3348ca27dc7e2a83c82550aee75b119825d834b29bfa6bbe40cf8b243b07779b0861711f57eff5bbcbe5a4612775dfafbb0cedbdf6a1294933dba82684b0a7b77a5f35b56d702fc4cb830f9934b41cbb02fbce906fdd25dae6f6ea168251a650b61830efaacc210efa470d7c35e8df187f7cb6aff64b680b6e3b13fb2f271de4a351854a502382760120e21988c537b6f7eb7c93bb3fc1ec246496").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("c0aee2c27dadae552c5049589fecb63c88538327b530ce576d5671fb").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("50095b929b8355794e6b4945f5cd2b5a92d724a82a3b5d488f10f5baa1a90ab0f21d1792c0004b1d404c6ce8239a9e07db9e8b8ffed5eb67d4127956a37b7969d9d734cef25a8ada20e5357813675bd28a1f43efe33fd366659ae78fd5765a52e32321f2aab891457dbe26a01dfd1a4abb1fc6f9b775b4f461b3dcc1b4023caa3453b9b7fb8afddcf33f923bf2111fcf3e3663989e24a24673d5c0a935a81f2f05859031d1332161d86a380f062cb2b5b5bc75b9959c1f4ddb96c825d3599c241e81db4f86e00faa59b9c6f944574acc860a887d4c505133009ac3a6cbad36a3eea052ac5761c2349966b24bad1e3b71f8adf6ed902893261fe5eb77f06bdf02").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("2d06746f85db0a3da0788a0a43ea690b99f5ca46b660f9b1780ed038").ToPositiveBigInteger(), 
                // public static other party
                new BitString("7642206d7140d374c4e7244f28576615f91cef916c0b8e57959e3b86137ca1224c2454c492c65fcdd4ca42e458d2ae6821e2a6d8446b8a667ca10cfcb4ecd64d67cdcff11b529d06275e6a415ecea6a193c07ad8dfa4b6a09c5885a4b7d4df3382a6c49e38e697f9e523bae1fe218b79316f937d2f62fd43b220ba77eee648b4dcca7f86f796789e7b53de6a8292b66b04a50048f67b3cc439bc8bc7fa8124e3c8e24a08ecdec5585b0cc6ce9f88f9f40fd56dcb581c06ca8ff0b99dd2a3835a94117f959e747b593a694dd14e184845be104a5217819bff2d3749af525d2b21cbabe5685867bd1dd0a8a3c6ab4b10e106efe4a4b17581e07a794427bf4225b0").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("b3e8c72dd64466fe9eca11bc38c3210e4855b5b28f1f08eb1dce241f").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("6a22c61b77f5103d0e2e4a432fd37c53dd7dddff2e673995af3ea187df8616d7698a3d39d72fba3e523bd1a2d33d3404e208cf93d4dcdd9601fd6fcf5e251f99461f0b1636cdce0cf55e9f471dcffc0ce8360a50f60c88e75b177f54f28a191611df4ffdcde306628f57c28cec8b62511c9a8e67fdcb06ea9331e364d9987adee417c70c05b8654c5f771ade98eca46e2bdbabb99dff197a0296c245b864edeef53b3a808fbb97477586e73a6271b46f42cd18f20498fd194e52e6284dcf004d9545129a270e796a3849c6cb6ef2fdf831552cc54f16964fa42b7b23c78fa590d36d1c89aca7328c0ec2ae0025196deb7da21dceb24e18112e220d7423d99335").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("34e3113913999902b487266b0a313f6a428614890c06c6be7cd313b76692c8830c9372c1f238dd748c7d9842e106fc781d778c6151e2e2082a645705abaabff3400ec8ca1d3639c573e7cf75f691f04915d571e5190f6de0b9a69b1cc44283d33b58e0359d3529ec0318202e2473cfcae9290089842ea76d0e8ae7fa305f2b3b1094263031c47fd2228c2b167ed8c3f3112ec3f741b7cf5ff23812face12a749ef7361dacceae05c75711ec977ea4b47aff3cdf6e56eb0fdd6f9d14300f2dffb0c57e4c4dc9d27d262131a699138a51b0c55f9db6cdfa5d727f1b98f0fba61170ef34ff01703b90bbf8d6ec41ccd65e4532b63ab808c6f2fcf0f6327170ba3c43c2561dd93211aeadbca281ae2e2fc6f2f2e60142bc4b708ff475f0dc7e52205e55942b2cae6d88ea87638ae6c81ee99453fc9e042105f715402fb52cb957cfe4a80b22afd4c28fad743a90bdd62c75d40206e946216068e6980acbd487f26633d36babdbfa3428ecfae8020374b5b74de65d9e870502ca1bbd51a584880c17a2d25d624ffde839364f7e60c8121e933e12e0c8538a9b7dc14464abab9ac555ac7c533cd8e4c8a2617b1045bccd58c37c2eb426f1741a4d6bd3dfc5b66e407be7922890330f7fb47c3d5080446d38632e33908901ea8e6882262747d07e46f8d320a97a09f1f662f010a6f14c596ecd0ea673cc03e843061fbe4601dc30bffcc"),
                // expected oi
                new BitString("a1b2c3d4e543415653696492c27a2d71c1edc2248e0f01bb0d040b228ab9"),
                // expected dkm
                new BitString("125f52aa43b93d859206ea35ce9b"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765cbbaedffb7f967337ad0c0ccff8edcb1"),
                // expected tag
                new BitString("0bee641989b1a0aa35ac771c9a95")
            },
            #endregion hmac
            #region ccm
            new object[]
            {
                // label
                "dhHybrid1 ccm sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("9659070a40586b444f1a4c0c4ec3143d22d3376e46f1cfd70f7b51f4a1c1b800f28341c4e1f56682331e90388c8056f8af0366adac9025490dbb4277e0f4ab370d12510e8014376435a314ee287833fedb4e2133e6ccdd32994d94a95fd22b475b6796657abb31f6d374a06282bec3605a9bf019963128a1d977454dd26d0b1f2067e7e9706f90e05d8824b0ab576d198ae7bf5ffb995900fbcdcc19193675c0af7fec72dc82d751f3a088e02be3965803ce94bd59e2f6c6a571430c7892091bc0791b14c1e8c0a0b971d6bcfb8c6fef87f0a0fc6efa28e87c5b167240c748217dd7c42d28c3d0819a9dc2a906aecb2d02e6ed9e2963720f3f51988bdb3180cf").ToPositiveBigInteger(),
                    // q
                    new BitString("990c0fca4ed35e9bdc69c7e0bfc008bdbefa0b2cc32d261800e52ac3").ToPositiveBigInteger(), 
                    // g
                    new BitString("4e178b3256a0cee421288e826ccb051c8b06e3509799f61704c3b69bc81005ca21e8aecca58ff1e2ade046e9ca42d3d52cf61a1e947f867b538f01f32569d08500520476d50da9252567cf589cc481245d3f5a35cf2e35afd26ac65a9c4186fd0ea020a366b2aa1b6812c9d1fbbb8493d0ceb24b51a3bd135145f9c4bb3976340e3899caca3e706ad24c51604d2b7e088ef26f270f46d2529ee5c180cdd9550342b19424c1fb951910a8ba5c39a17e3f9d9f55b9033b286b452b9850c0019f2f76e21bc64360c0900158420a9b179096615390ede11664afdcc1ee00297038406ad3ea502a8721df8a1567bf39909412eac7e17b93298d568742155f546763f9").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // key length
                128,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("d4504328711c368dc8c47757fea68266"),
                // aes-ccm nonce
                new BitString("e6a6612ff48ac8"),
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("38e198b47d39704c01f29e1aa65dde8465d925b141021a5c40d78ff9").ToPositiveBigInteger(),
                // public static this party
                new BitString("7b048a33424220f96f9e2fe63063f5727edfd5e5814afd30d04d12d61dba23aedffbaa7417559acbfc3bbba298a560b4d71574e279429dee195d762b08ad388c0fb01a391db3146616fb99fc7e9af7834461878c8cce578cfb9bc612ea0d2e113c891658e8ff9ec35432c118123d9a18cd7b6ea7b32a5c81ac0d7d0ce65b61ec31924271d89cae67f9fd5d20fc027abf1061fdc5dc6da7d0ca7fc01a953906c2e94859ce8175e981f905a0945e166598776882b3df5e1fc5e3fbf1eadd620b9c08be84d871aaf596d63a466be3046cbf648d745770169c6e12eeb8cb9f3b7947106b3eb14ae73c9b7b90926ec6e66828051f11753f52f76f904314ddd269372b").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("60da9ee395cf6313d92f30c3e0cef7bc999d962fc3404c676add95c7").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("6e091f6fe7c681e81afeffb5b59a7d8c588167d991451ffbf45c77aaa20ea545d960cdb2ecc728dbd576f5d8df7d96079866f1eee454181a3c70d3d89816bd7e57a3a8a447a059a81479a149f5743f5421b03f643fb8585fa259f8253195caaaf367936d0560b7f9f968a85a97e528070d7331864e15919a5f44c6a2781aca5551f0e8d1d7d38a7f72f8d518c9bea74f9255b3c5cfe355a90f4f94664086dcd9ca694c06550f0e6e834ad5ad96e14d294d9e6e76a10529f696f6a6f36a1a4e9d38bcde5841d9aa77bf967449e153752be27af11e15d0479caf7b76980f67fee6fb4566e187d9739aa57b5c0669defddb84f4472c1a37e84d594a0e53004e22a9").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("6828808d448975c83c9025c033db770e2f45dd19990e3d39adcc2380").ToPositiveBigInteger(), 
                // public static other party
                new BitString("102b132d4b252c65ef1c8ed268961117e964f54d15031e4f1851aa72ea0b477f22f25f9d993434e53abc4d2367aac7f696e0abb5cc5d0e8c72e379d59fe1804394e40caecc35a3df6953ac4da827fc67db14349284e7c7a753e5ee7b403d7ae17a004a378a4238575a295ff1bdc20fe6f414c5a74fa36fa2b799eef204e5275e8722afc8d2adde5a67ba741525f115a463167d123906437d27a8f3d14df2292283752fee146ce33c9c2ecfe776393f637d39d50f0cb92882bd605ee5789b904e56f31447945a700fb507baa7dab884f74130472e3302022e5a81d8c3ed9a8c9012854dca10b80e5a568712a4bad0cb2fb500b58c55895c89dafe61c74224fe85").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("4046ead8a7011ca5bcbc45000717f2540935559591eeb204326b0c0b").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("7af39a48dc2e663f5171b77a27df26cc5dfe871a1c8d7bd4ed7629cfaea329655f141869d8bab9e69cb3f9fb9d2f7a13b1c20e1496c294b3ee04fe19a73f9ee57ec62be22c685c0553bffd0bd006f8179b1447fbf07ced8a011b3521367a5a834b35f97c6a18f2311f0497c61539cdcf98aea88b531a169decba8359b5fe35c219ef998d5f07f7f752a47af5003b1288ffd172286e4aba9c27b1ab4ad520d6a806d82d5f22b72c0055c424f92d7bc1abb01e16d7be84950414a8d61768da9c9c262d06c2a75927f2002255cc9284eab5399da68e2f849697b1e8f37676c323c2274c65b0bb58e1f9be5462723019d3ec87e756d3be5036416f7c1e135b4f640c").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("00d8248ddd7bd0dca10af0d7fdbc999a97aa84d7eadc2a08a9eaf085e4dce846d29391ee42bc957763d0be81138cd3490bdde3d27d7206c652c1991bb6f3debbbc435b812407c4b928e114348ffdc2d3f5c24035ead5d7758e3a9ad4b3bb64f83ec7ddd8dc65336de8c02daa3fb0e7978e74408fc2942aec5b5ada83b4165c9f8ca9267544f30a179112ad71966b6e7ccdbdbf2c4a727b6275a637974c21ff5788d89c0d302c9618bb3c2290bd1313edceb14b9be7a0c8f5117011eb4d776993ce5776870db0cf4bd948f1248a93236d83ac8d03514471bb60846e1c4bae11018ef1f9a1a464960c024c8e10d2c28fcc68b9b9955417f5b7d465eb76649766358437aa03d87326905f6ecb48da88c6d73d5763b8203cb1edfe8880f2c8a4ef38e5004c93929f9be3163d171ba266b7330801bb6a14e0c7b6bfab5d91fe1c093a7cd5adf854c82f07d9600eb4cd3fc068968f0bb072673ea908a671c625d25410e08399a00e4a1c91ab8295a0a53f01ec1581318a088332843d08ede937c59128a00c7f920c6200d834b092442af46f073f7e0476e62d784546fc53173979c56e12254f34895d732844630c6a8dd275fa62d6c0f19228f602eeee07a34e3f894f0cfbb51b4e9b97b827bbb36f524fd3038d5561d05f3206ae82c8dfe841344d4bd41bed56c362ef1496a8182a2a7eb5ffe842e87509ec864cbca1c7aceeb7c1c2"),
                // expected oi
                new BitString("a1b2c3d4e54341565369643b2a075ec4c3df3857eb0b044086735ebcb34b"),
                // expected dkm
                new BitString("1acd0357f7281385e1de6ae725258239"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765d4504328711c368dc8c47757fea68266"),
                // expected tag
                new BitString("1ae87d8f1ff3c985")
            },
            new object[]
            {
                // label
                "dhHybrid1 ccm sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("9659070a40586b444f1a4c0c4ec3143d22d3376e46f1cfd70f7b51f4a1c1b800f28341c4e1f56682331e90388c8056f8af0366adac9025490dbb4277e0f4ab370d12510e8014376435a314ee287833fedb4e2133e6ccdd32994d94a95fd22b475b6796657abb31f6d374a06282bec3605a9bf019963128a1d977454dd26d0b1f2067e7e9706f90e05d8824b0ab576d198ae7bf5ffb995900fbcdcc19193675c0af7fec72dc82d751f3a088e02be3965803ce94bd59e2f6c6a571430c7892091bc0791b14c1e8c0a0b971d6bcfb8c6fef87f0a0fc6efa28e87c5b167240c748217dd7c42d28c3d0819a9dc2a906aecb2d02e6ed9e2963720f3f51988bdb3180cf").ToPositiveBigInteger(),
                    // q
                    new BitString("990c0fca4ed35e9bdc69c7e0bfc008bdbefa0b2cc32d261800e52ac3").ToPositiveBigInteger(), 
                    // g
                    new BitString("4e178b3256a0cee421288e826ccb051c8b06e3509799f61704c3b69bc81005ca21e8aecca58ff1e2ade046e9ca42d3d52cf61a1e947f867b538f01f32569d08500520476d50da9252567cf589cc481245d3f5a35cf2e35afd26ac65a9c4186fd0ea020a366b2aa1b6812c9d1fbbb8493d0ceb24b51a3bd135145f9c4bb3976340e3899caca3e706ad24c51604d2b7e088ef26f270f46d2529ee5c180cdd9550342b19424c1fb951910a8ba5c39a17e3f9d9f55b9033b286b452b9850c0019f2f76e21bc64360c0900158420a9b179096615390ede11664afdcc1ee00297038406ad3ea502a8721df8a1567bf39909412eac7e17b93298d568742155f546763f9").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // key length
                128,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("d4504328711c368dc8c47757fea68266"),
                // aes-ccm nonce
                new BitString("e6a6612ff48ac8"),
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("6828808d448975c83c9025c033db770e2f45dd19990e3d39adcc2380").ToPositiveBigInteger(),
                // public static this party
                new BitString("102b132d4b252c65ef1c8ed268961117e964f54d15031e4f1851aa72ea0b477f22f25f9d993434e53abc4d2367aac7f696e0abb5cc5d0e8c72e379d59fe1804394e40caecc35a3df6953ac4da827fc67db14349284e7c7a753e5ee7b403d7ae17a004a378a4238575a295ff1bdc20fe6f414c5a74fa36fa2b799eef204e5275e8722afc8d2adde5a67ba741525f115a463167d123906437d27a8f3d14df2292283752fee146ce33c9c2ecfe776393f637d39d50f0cb92882bd605ee5789b904e56f31447945a700fb507baa7dab884f74130472e3302022e5a81d8c3ed9a8c9012854dca10b80e5a568712a4bad0cb2fb500b58c55895c89dafe61c74224fe85").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("4046ead8a7011ca5bcbc45000717f2540935559591eeb204326b0c0b").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("7af39a48dc2e663f5171b77a27df26cc5dfe871a1c8d7bd4ed7629cfaea329655f141869d8bab9e69cb3f9fb9d2f7a13b1c20e1496c294b3ee04fe19a73f9ee57ec62be22c685c0553bffd0bd006f8179b1447fbf07ced8a011b3521367a5a834b35f97c6a18f2311f0497c61539cdcf98aea88b531a169decba8359b5fe35c219ef998d5f07f7f752a47af5003b1288ffd172286e4aba9c27b1ab4ad520d6a806d82d5f22b72c0055c424f92d7bc1abb01e16d7be84950414a8d61768da9c9c262d06c2a75927f2002255cc9284eab5399da68e2f849697b1e8f37676c323c2274c65b0bb58e1f9be5462723019d3ec87e756d3be5036416f7c1e135b4f640c").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("38e198b47d39704c01f29e1aa65dde8465d925b141021a5c40d78ff9").ToPositiveBigInteger(), 
                // public static other party
                new BitString("7b048a33424220f96f9e2fe63063f5727edfd5e5814afd30d04d12d61dba23aedffbaa7417559acbfc3bbba298a560b4d71574e279429dee195d762b08ad388c0fb01a391db3146616fb99fc7e9af7834461878c8cce578cfb9bc612ea0d2e113c891658e8ff9ec35432c118123d9a18cd7b6ea7b32a5c81ac0d7d0ce65b61ec31924271d89cae67f9fd5d20fc027abf1061fdc5dc6da7d0ca7fc01a953906c2e94859ce8175e981f905a0945e166598776882b3df5e1fc5e3fbf1eadd620b9c08be84d871aaf596d63a466be3046cbf648d745770169c6e12eeb8cb9f3b7947106b3eb14ae73c9b7b90926ec6e66828051f11753f52f76f904314ddd269372b").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("60da9ee395cf6313d92f30c3e0cef7bc999d962fc3404c676add95c7").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("6e091f6fe7c681e81afeffb5b59a7d8c588167d991451ffbf45c77aaa20ea545d960cdb2ecc728dbd576f5d8df7d96079866f1eee454181a3c70d3d89816bd7e57a3a8a447a059a81479a149f5743f5421b03f643fb8585fa259f8253195caaaf367936d0560b7f9f968a85a97e528070d7331864e15919a5f44c6a2781aca5551f0e8d1d7d38a7f72f8d518c9bea74f9255b3c5cfe355a90f4f94664086dcd9ca694c06550f0e6e834ad5ad96e14d294d9e6e76a10529f696f6a6f36a1a4e9d38bcde5841d9aa77bf967449e153752be27af11e15d0479caf7b76980f67fee6fb4566e187d9739aa57b5c0669defddb84f4472c1a37e84d594a0e53004e22a9").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("00d8248ddd7bd0dca10af0d7fdbc999a97aa84d7eadc2a08a9eaf085e4dce846d29391ee42bc957763d0be81138cd3490bdde3d27d7206c652c1991bb6f3debbbc435b812407c4b928e114348ffdc2d3f5c24035ead5d7758e3a9ad4b3bb64f83ec7ddd8dc65336de8c02daa3fb0e7978e74408fc2942aec5b5ada83b4165c9f8ca9267544f30a179112ad71966b6e7ccdbdbf2c4a727b6275a637974c21ff5788d89c0d302c9618bb3c2290bd1313edceb14b9be7a0c8f5117011eb4d776993ce5776870db0cf4bd948f1248a93236d83ac8d03514471bb60846e1c4bae11018ef1f9a1a464960c024c8e10d2c28fcc68b9b9955417f5b7d465eb76649766358437aa03d87326905f6ecb48da88c6d73d5763b8203cb1edfe8880f2c8a4ef38e5004c93929f9be3163d171ba266b7330801bb6a14e0c7b6bfab5d91fe1c093a7cd5adf854c82f07d9600eb4cd3fc068968f0bb072673ea908a671c625d25410e08399a00e4a1c91ab8295a0a53f01ec1581318a088332843d08ede937c59128a00c7f920c6200d834b092442af46f073f7e0476e62d784546fc53173979c56e12254f34895d732844630c6a8dd275fa62d6c0f19228f602eeee07a34e3f894f0cfbb51b4e9b97b827bbb36f524fd3038d5561d05f3206ae82c8dfe841344d4bd41bed56c362ef1496a8182a2a7eb5ffe842e87509ec864cbca1c7aceeb7c1c2"),
                // expected oi
                new BitString("a1b2c3d4e54341565369643b2a075ec4c3df3857eb0b044086735ebcb34b"),
                // expected dkm
                new BitString("1acd0357f7281385e1de6ae725258239"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765d4504328711c368dc8c47757fea68266"),
                // expected tag
                new BitString("1ae87d8f1ff3c985")
            },
            #endregion ccm
            #endregion dhHybrid1

            #region mqv2
            #region hmac
            new object[]
            {
                // label
                "mqv2 hmac512 sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("f127aed13b8097516103ce4d724828fd7703a83f02088d2fed33b1ecba84d0fba96717633211b63e1cff928721416365bc0e114fbaef0ef7e441efab7ecb7b7b31ecb2a665514ef1e4a8baa3c6c5309fef5ee361efba7ef498c50520394ce9868d7d54550993208e0a84c2e946b92f556200ac68865473a686360e8b887a85518112d6ef94176849737a67c4b6a060f7446960ad74350ad2325e05bf0168a79169280121c2f2b57b8ad40480b862c03696574123fc22e5a21fbec5b1a84a3b095997ae2dc1f5870b5a5f37ce4f7653d94268524ea693b0aed3a99e651c39be70785a2afe6766ae08f0bf0377cfbea3eb36601f66272c2637b1a48f6e02a93663").ToPositiveBigInteger(),
                    // q
                    new BitString("a7a8890f303a3f93ddc50239339c8d878840414ab94a305da2f6c0f5").ToPositiveBigInteger(), 
                    // g
                    new BitString("18c18042ae82b1c5e4db0ef668921803d68257becc73989137d52956d0a35f9e31e4c9ce41b5a7537d6ad63124b2b21e0b4121e4e786f78692df64eee81f38f7b1f1fc7ddd69bee960f91a46c8bc5b0d8595cb9a2e4ee1dc22242c813b4ad0d5581fdac0126f233a246e6cdb65a31a3b04c49dae8687e44b2a4a2e1800182a1d7a46f15b6ddd0d949d79f869649578b249d726689cd1494e13f612b9a98389381f9141cb1fe0dd528277ceaafc5254e799d0a65e74d976a8f609621e736062ae5e209cd239ec128e9481ad439b18a78b9913c6df8d1600ebd3c5ec9fda3047d377b4dd2f0097a2175a49ccc45b7d2541dc334dde3447d3617f4e3a1db5947dc8").ToPositiveBigInteger()
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
                KeyAgreementMacType.HmacSha2D512,
                // key length
                112,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("b7a1df1fa2c11cd57c7e477097b16339"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("58ed320e81f45569f31b1db4fa521cbec8756a278658ed10d90a2099").ToPositiveBigInteger(),
                // public static this party
                new BitString("ab607b2ac025b0806fe8e088dbd63832afcc97b222896025a5043199f1147c78757fb6fdf25dea3b48dbbf5970f7f0478f7e112870f4a5d3a170c418cd0f5a510847074d39a7a26fdabc07080ceca86320ddfb48a6527f0589dbd28b7f43b84fe359fcdaea784997403620626f9aa8d114ba9e8357b21e935c7132db1ce47a511eac579cad2a783ad19e8dda48d3031bb3888b58d6eda456a73fbf971a7176ad58638a6220c46118ea31569b0530df5ed2242fdc04e38ab47a42752231c3095cc81209f5f5b4711bcbbeb787efdf3711d8a3dfe67bf5fd0b3fa7cc13627f6b6c37642750453845ec3e09dc6881139bc6cec2106cfdde59ff4beb0fbf6806ece3").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("192ffe21ff0c413d98863b978d70b21724033f22326d78c6480be4c4").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("1e7141c5409c3abf1809b35c67553086964443a5f47eac0c3cccfb97e014fd7dd5d273d214587babb3802939f4a2a56afca9a9a6be4a98f454e190e27d4fd2d7dbb5570bb51628dbd742d8a9d33883df915a17b3a647c39aca71fa4c3eabfe470c520244f990bd152230f1f1e64c7499a9ca1822f186fe5843d0d4d9d2e42d9ff84ad4986bcc6ba2cbdc172033527e6f56ef621fc0153a325c9b5d7bcc648eda70847e1ac086205c4f044904746c9cfc7b921770137cab281c8a48c57c06a533966d963a54b97512da545b9649625c31f33bf184967e06c5d75316a9a39c456dc6f3da321eaa5e19bb685cc1c294206e11d3f9449e5b73f084218c9890defeab").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("5691f72f558a33a66b26dcc45690c8c7a97f4243b12891dff196bc5c").ToPositiveBigInteger(), 
                // public static other party
                new BitString("6abf4e24a7a756ea478b3978629be7b6fa3a3954ef199a2fbe605dcc3c5418bcfa0ac3d4151feaed820d98332b53f3932cf4518f7ad48c8eab7dfdc0328a3b35b00f39936a1968e9081700e03d17ec9982005ac7fa87fd0a54fd941516cdc5bc5a003919433ae7d1b7db9865986a47e4091304ceea95d689958c20d46939ee96157f81485bb419c129f249049bd9372aeeb5bf7a9a942b1dc6c2d28cb35d6ec0e82b96d4734dc7d77bbc06336844f234f44034f17f3d3c1a6f54a079ff2b9847b886f9139c462d79a2142f78ebfcdc25f979fce78b5dd90dbe3397ecca5429eb063b3637cc4e8f2aa792b208935542795b0b8873a2dce56208d740092c1189a1").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("9541c4c455c9cbbddaf56f5aa1bfb8686ed2577b556c033f3889a23b").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("3e893b0d86053c10b2f2e261cf937cd6f92a4862670594064a50d64c4bd32f33f112e683b05e507200faf91b0889fb5036aefbed9cfdf3c114bdabe2a327c171b087f0f8028d5d306c64e7cb7e432d6a83347a4efec30d823a2bd0fc7430b667da61e45535e3b01755b7300341c238350b7789348f478cf1c3e0250ebfe63e1879b482faaeffd167a4fd8db464bbca22754d4236228f81bbd2c727b9b85a6682cbf4453a1219af169491b12c5ceba8001ed28e5e623757ac4fbfa116d70a3322ed4435859d060091aa5f1f265f2cd107a82f298dc9733f08c7a5ec694418cede121262f6640e1a9bfd87bb9847b071687e19aacc7319204129e3522a4d86856e").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("093a7ecfb40d809a7725cad627270872634e7e2e77acb7283372ed79704c85bf280e7e156066da78622b1f787b45d41efcfb35892925b204e5c201b27c0e38054db763477dd9beae965afe5620e815390ca2b9cbda7635f71ba61bdc66bdd1a6a56c85c0751245d403bd37511b4b1b6afe878cf41cbbff4dc8e8b9ddcdf66f32fe436365f909191fa5c30637f322a03b8110f0d807c4619a5478ab8bfa98c3aaa197971abf1d3e25d9a608ae568295b38226bfd301b207ae0ee78e3aa14f0adf598a9f7fbd8c83a4fc0362e0ee0f577915023a111f783b6b7cb7f5edbe6115d29958e34cf710410e8e17f715d59c973ff357d2007147a0120eb7eae067f808a0"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964b5829a07159ce59d2a19780baf5d73fb767def"),
                // expected dkm
                new BitString("4bfb5d44680f22c748cfa5e02da3"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765b7a1df1fa2c11cd57c7e477097b16339"),
                // expected tag
                new BitString("b3e68de202272e8a")
            },
            new object[]
            {
                // label
                "mqv2 hmac512 sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("f127aed13b8097516103ce4d724828fd7703a83f02088d2fed33b1ecba84d0fba96717633211b63e1cff928721416365bc0e114fbaef0ef7e441efab7ecb7b7b31ecb2a665514ef1e4a8baa3c6c5309fef5ee361efba7ef498c50520394ce9868d7d54550993208e0a84c2e946b92f556200ac68865473a686360e8b887a85518112d6ef94176849737a67c4b6a060f7446960ad74350ad2325e05bf0168a79169280121c2f2b57b8ad40480b862c03696574123fc22e5a21fbec5b1a84a3b095997ae2dc1f5870b5a5f37ce4f7653d94268524ea693b0aed3a99e651c39be70785a2afe6766ae08f0bf0377cfbea3eb36601f66272c2637b1a48f6e02a93663").ToPositiveBigInteger(),
                    // q
                    new BitString("a7a8890f303a3f93ddc50239339c8d878840414ab94a305da2f6c0f5").ToPositiveBigInteger(), 
                    // g
                    new BitString("18c18042ae82b1c5e4db0ef668921803d68257becc73989137d52956d0a35f9e31e4c9ce41b5a7537d6ad63124b2b21e0b4121e4e786f78692df64eee81f38f7b1f1fc7ddd69bee960f91a46c8bc5b0d8595cb9a2e4ee1dc22242c813b4ad0d5581fdac0126f233a246e6cdb65a31a3b04c49dae8687e44b2a4a2e1800182a1d7a46f15b6ddd0d949d79f869649578b249d726689cd1494e13f612b9a98389381f9141cb1fe0dd528277ceaafc5254e799d0a65e74d976a8f609621e736062ae5e209cd239ec128e9481ad439b18a78b9913c6df8d1600ebd3c5ec9fda3047d377b4dd2f0097a2175a49ccc45b7d2541dc334dde3447d3617f4e3a1db5947dc8").ToPositiveBigInteger()
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
                KeyAgreementMacType.HmacSha2D512,
                // key length
                112,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("b7a1df1fa2c11cd57c7e477097b16339"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("5691f72f558a33a66b26dcc45690c8c7a97f4243b12891dff196bc5c").ToPositiveBigInteger(),
                // public static this party
                new BitString("6abf4e24a7a756ea478b3978629be7b6fa3a3954ef199a2fbe605dcc3c5418bcfa0ac3d4151feaed820d98332b53f3932cf4518f7ad48c8eab7dfdc0328a3b35b00f39936a1968e9081700e03d17ec9982005ac7fa87fd0a54fd941516cdc5bc5a003919433ae7d1b7db9865986a47e4091304ceea95d689958c20d46939ee96157f81485bb419c129f249049bd9372aeeb5bf7a9a942b1dc6c2d28cb35d6ec0e82b96d4734dc7d77bbc06336844f234f44034f17f3d3c1a6f54a079ff2b9847b886f9139c462d79a2142f78ebfcdc25f979fce78b5dd90dbe3397ecca5429eb063b3637cc4e8f2aa792b208935542795b0b8873a2dce56208d740092c1189a1").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("9541c4c455c9cbbddaf56f5aa1bfb8686ed2577b556c033f3889a23b").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("3e893b0d86053c10b2f2e261cf937cd6f92a4862670594064a50d64c4bd32f33f112e683b05e507200faf91b0889fb5036aefbed9cfdf3c114bdabe2a327c171b087f0f8028d5d306c64e7cb7e432d6a83347a4efec30d823a2bd0fc7430b667da61e45535e3b01755b7300341c238350b7789348f478cf1c3e0250ebfe63e1879b482faaeffd167a4fd8db464bbca22754d4236228f81bbd2c727b9b85a6682cbf4453a1219af169491b12c5ceba8001ed28e5e623757ac4fbfa116d70a3322ed4435859d060091aa5f1f265f2cd107a82f298dc9733f08c7a5ec694418cede121262f6640e1a9bfd87bb9847b071687e19aacc7319204129e3522a4d86856e").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("58ed320e81f45569f31b1db4fa521cbec8756a278658ed10d90a2099").ToPositiveBigInteger(), 
                // public static other party
                new BitString("ab607b2ac025b0806fe8e088dbd63832afcc97b222896025a5043199f1147c78757fb6fdf25dea3b48dbbf5970f7f0478f7e112870f4a5d3a170c418cd0f5a510847074d39a7a26fdabc07080ceca86320ddfb48a6527f0589dbd28b7f43b84fe359fcdaea784997403620626f9aa8d114ba9e8357b21e935c7132db1ce47a511eac579cad2a783ad19e8dda48d3031bb3888b58d6eda456a73fbf971a7176ad58638a6220c46118ea31569b0530df5ed2242fdc04e38ab47a42752231c3095cc81209f5f5b4711bcbbeb787efdf3711d8a3dfe67bf5fd0b3fa7cc13627f6b6c37642750453845ec3e09dc6881139bc6cec2106cfdde59ff4beb0fbf6806ece3").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("192ffe21ff0c413d98863b978d70b21724033f22326d78c6480be4c4").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("1e7141c5409c3abf1809b35c67553086964443a5f47eac0c3cccfb97e014fd7dd5d273d214587babb3802939f4a2a56afca9a9a6be4a98f454e190e27d4fd2d7dbb5570bb51628dbd742d8a9d33883df915a17b3a647c39aca71fa4c3eabfe470c520244f990bd152230f1f1e64c7499a9ca1822f186fe5843d0d4d9d2e42d9ff84ad4986bcc6ba2cbdc172033527e6f56ef621fc0153a325c9b5d7bcc648eda70847e1ac086205c4f044904746c9cfc7b921770137cab281c8a48c57c06a533966d963a54b97512da545b9649625c31f33bf184967e06c5d75316a9a39c456dc6f3da321eaa5e19bb685cc1c294206e11d3f9449e5b73f084218c9890defeab").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("093a7ecfb40d809a7725cad627270872634e7e2e77acb7283372ed79704c85bf280e7e156066da78622b1f787b45d41efcfb35892925b204e5c201b27c0e38054db763477dd9beae965afe5620e815390ca2b9cbda7635f71ba61bdc66bdd1a6a56c85c0751245d403bd37511b4b1b6afe878cf41cbbff4dc8e8b9ddcdf66f32fe436365f909191fa5c30637f322a03b8110f0d807c4619a5478ab8bfa98c3aaa197971abf1d3e25d9a608ae568295b38226bfd301b207ae0ee78e3aa14f0adf598a9f7fbd8c83a4fc0362e0ee0f577915023a111f783b6b7cb7f5edbe6115d29958e34cf710410e8e17f715d59c973ff357d2007147a0120eb7eae067f808a0"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964b5829a07159ce59d2a19780baf5d73fb767def"),
                // expected dkm
                new BitString("4bfb5d44680f22c748cfa5e02da3"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765b7a1df1fa2c11cd57c7e477097b16339"),
                // expected tag
                new BitString("b3e68de202272e8a")
            },
            #endregion hmac
            #region ccm

            #endregion ccm
            new object[]
            {
                // label
                "mqv2 ccm sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("fe150aa39b477dadcb63ad1a4d17ca84f323c5e5b794a2b4b4347867f66d8ba46a74b659b4ad49a98f6c62b03357d8c435c9f02ce89a9600f3eef2f7584f3634c23e5a193af1271860bd9ae77f4becaa36cb9e59d4cb3646895476e4c753502e5fb78361a7c379d1518f2a5f210e9d2e9dade2b8eb1fd363e29c9b7c4b4058bb67cef6ac594fa874d351ee365e00761750b0438be8ab64c348723f880f8a4e589627dcb64025afa0fbcac5313d5d38012eaccde4a017403dcfe39bff4134dfdceaed09d337c86341ed830527f3e7ba706aa3c93cc991a14bf8cabda505c1354da855b4291e2df97cfcfdc17d26a6eb499327fbbe517ec745291623e168b06c01").ToPositiveBigInteger(),
                    // q
                    new BitString("9f9a91057fb89eefb97c339e850fd972a609fb81d78b893d4c5118af").ToPositiveBigInteger(), 
                    // g
                    new BitString("f843ce7a43d4fba0a076a79518dcf73437b1d6a459c26d459cfb8f22447118e86d3dddfbf4423659f62b5e053655bbebce030e39932fdeef52d95ce5cfc21e584b3021a49de6f2ac124614e2dc6802ea2dd1141c2aaab586a2db6ffd14a9fcd694e73416db768b9d9d4904097fe10659a4aabf9ba36d6ee12792076c7db5cc13eb7526e2f044af4f03637af4765eb9a34e87bef5acc5b07f9f0daf2d5095cac43c050b17183a8dd66cb52db9e0e48ff9c521c810857c5e23ab73eed46bc8e52b3325fe7cacb4a4ee439a9d269ba49bdab3ed4093c6c2eb8ca7606718e6fe9ece1b18b76cf11b1ae3ce9c8d49ecf20251293e694753e24cbf57f190b0d23a7b73").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // key length
                128,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("6a74e3e0427e4e436b5036aff4894e60"),
                // aes-ccm nonce
                new BitString("98c05fafeaa757"),
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("7e111120fc832153f989848e645710b45ebb41773676ccceae067965").ToPositiveBigInteger(),
                // public static this party
                new BitString("ea22918d218053fa418d87715734e59af6c08f208c6ccee669f527b17941f302def6a07aeb38ef2b2c9bb880130fc7e97bddfb01c8f5e6864923acbe9c0515a589e306bf50180a7ea9b9a4b99bffa6af0b55df542c904fe504d3fd54179ad2f99397da5d564cbb8225f5b135d05982ceea50d4f23836f37e95c821e1513c2d59303664d9f181d72e229345db4d9d1f9611046817c4af077c4d89b7768b0672fac94b1f67483404aceae6bf271352b0ac6e8ad5606f04c6f9770fc44da75ed1c599cee0922dd11031fc8b0c5f56abf72c1f2093e3ed3774e19c0494ea901715df33a78af60ef155d2dca034b8653b76d142c1b623e1300c8df33f70358137933d").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00009e55e127fa1262a5f45688b44efef2cd2c7067c872d5d4fbc2eb").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("78209298785c1afc11f936d9e23916fb820789837dc79d1ad12315ac76aa06e2e004970c790070a0854b1ece0cb24fd4e8c35cd17e5f2d65dc62bc7a34736b04e58c32853e84eb01241b6d40130e0092e933f8cc122c86525c949ae41f46caa915e32a822381cfd4041816230dd35dbff35c937a95c4486202e2720253a90d5c24bf79135f9f6fbb403f7d1412f832cb078fc0067db04709eb41c445804d4870490d27e90c6d10627e71c360961b90ec4c70812f8fb298fbbb69987babe6a6d34aacc07c513b827aa9cc75b76cb18b6317afdfd7b0c43f0a65c4de97169d27dcd13c29b50d28d03b5370deee474652e0b24695da8eb0cfe6f6f447f175e0eb56").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("83e695a19f8e95dec82cae577a4c1b3f7c7e2e6254af5ebaf5b47f76").ToPositiveBigInteger(), 
                // public static other party
                new BitString("44c29adfe7b7b335adaf38821a6ade0b87cefbb7ff6e41b5332b94659a03487cc2fc89c10caa25e63521741ac5cfd112ab3073d62bf0942e229e8b1334602bfe53b004f0210f774d0880d759e34008d6410f3c4c32051a88ccbd2da5da7801f71e863bef76784ee4184e7a96f6461c73fd361526ae30095084fdde9a8648703da03f6da642465284a3b20085b1500f3b89bf733278599f083daf5f63ec0bc6a43a744bc320f8fc4b7e9301352f034dca7429c3bd6a60c27d9af5145cb53bc398df0f0d87eb206b5719dae188d0bb25eaa9748cdcd0cf2e2dd571ccc56447d949a1b58a28fc00b2b54e3385300d0160eabaaffd9ae5f45b369cb2cbd7adb4ce3c").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("825e78970228785c873121506db1e4bd17b67ffd10cc5c0b81aeed81").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("9144cc4b5624ed84eaab4efa7394a3267ffd63e77e5c0fe23f7d5099351b3eac92533762a8fe13c0865680d2790edb06a2e588eeb3aeac8f513c8f60fcc6612c6fa93f850960edc2d1544ccf083c11350aea2fde70f3753e80830090d26d90f26024d688924c488f5d7f85941757f93322f7cb059bc310f994ae96afdec1aae8bdc54a0f8cf052d195b19a86d55d950c71fd4311a39c952eb6e25f8a77af3454eb86fb20d106114de7801517e144f37e6c4f16c2daab39c43d8501b10afe510ea519b1ee62789e1c5332ac6f358f13615929e13ac049629d84493b35113f9a22a1a31f3ac4ae8a380b6fdb8f1d54607a6595bd214b0cd99d6d329de33e250376").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("17ec645e7229407c5638be23e3378b1610d2bdc162955b0d614fe5a147c2650f14e0abfe6095420f7d3fdf4d7f4089677d33e2a48ff406de07c9ab79c8733597e48b42b39558b94af03d2d5d83959ce8023b947201ea70b2ee5d788719baf36a9fc4861409f505d3559ec674025f1d736d6404d239c490ed3b6902fd2ce851439ec8ade6aa5377c1c8c9367fdaeefe43f9f3d1d69602bad337191fd12fbf092c6d8619018146f340c31e6e5357a73c6ef2553130aac3dd9c11d90189948dbbc251ee17f5dc1d1ebbcd9622a84a3ba28d60dca8abd86ed1191cf8c20904090daf3b335c9c9ad561fce8ed4c5ad0255aff78e9daf66342b8fd1b9ccae813969bfe"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964ee668aa91dfe915fd19cc9c2c89583ed81ffc9"),
                // expected dkm
                new BitString("07dcc06cf9bd351496fd0d41e90543c3"),
                // expected macData
                new BitString("5374616e646172642054657374204d6573736167656a74e3e0427e4e436b5036aff4894e60"),
                // expected tag
                new BitString("1eee9d416cc94848")
            },
            new object[]
            {
                // label
                "mqv2 ccm sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("fe150aa39b477dadcb63ad1a4d17ca84f323c5e5b794a2b4b4347867f66d8ba46a74b659b4ad49a98f6c62b03357d8c435c9f02ce89a9600f3eef2f7584f3634c23e5a193af1271860bd9ae77f4becaa36cb9e59d4cb3646895476e4c753502e5fb78361a7c379d1518f2a5f210e9d2e9dade2b8eb1fd363e29c9b7c4b4058bb67cef6ac594fa874d351ee365e00761750b0438be8ab64c348723f880f8a4e589627dcb64025afa0fbcac5313d5d38012eaccde4a017403dcfe39bff4134dfdceaed09d337c86341ed830527f3e7ba706aa3c93cc991a14bf8cabda505c1354da855b4291e2df97cfcfdc17d26a6eb499327fbbe517ec745291623e168b06c01").ToPositiveBigInteger(),
                    // q
                    new BitString("9f9a91057fb89eefb97c339e850fd972a609fb81d78b893d4c5118af").ToPositiveBigInteger(), 
                    // g
                    new BitString("f843ce7a43d4fba0a076a79518dcf73437b1d6a459c26d459cfb8f22447118e86d3dddfbf4423659f62b5e053655bbebce030e39932fdeef52d95ce5cfc21e584b3021a49de6f2ac124614e2dc6802ea2dd1141c2aaab586a2db6ffd14a9fcd694e73416db768b9d9d4904097fe10659a4aabf9ba36d6ee12792076c7db5cc13eb7526e2f044af4f03637af4765eb9a34e87bef5acc5b07f9f0daf2d5095cac43c050b17183a8dd66cb52db9e0e48ff9c521c810857c5e23ab73eed46bc8e52b3325fe7cacb4a4ee439a9d269ba49bdab3ed4093c6c2eb8ca7606718e6fe9ece1b18b76cf11b1ae3ce9c8d49ecf20251293e694753e24cbf57f190b0d23a7b73").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // key length
                128,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("6a74e3e0427e4e436b5036aff4894e60"),
                // aes-ccm nonce
                new BitString("98c05fafeaa757"),
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("83e695a19f8e95dec82cae577a4c1b3f7c7e2e6254af5ebaf5b47f76").ToPositiveBigInteger(),
                // public static this party
                new BitString("44c29adfe7b7b335adaf38821a6ade0b87cefbb7ff6e41b5332b94659a03487cc2fc89c10caa25e63521741ac5cfd112ab3073d62bf0942e229e8b1334602bfe53b004f0210f774d0880d759e34008d6410f3c4c32051a88ccbd2da5da7801f71e863bef76784ee4184e7a96f6461c73fd361526ae30095084fdde9a8648703da03f6da642465284a3b20085b1500f3b89bf733278599f083daf5f63ec0bc6a43a744bc320f8fc4b7e9301352f034dca7429c3bd6a60c27d9af5145cb53bc398df0f0d87eb206b5719dae188d0bb25eaa9748cdcd0cf2e2dd571ccc56447d949a1b58a28fc00b2b54e3385300d0160eabaaffd9ae5f45b369cb2cbd7adb4ce3c").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("825e78970228785c873121506db1e4bd17b67ffd10cc5c0b81aeed81").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("9144cc4b5624ed84eaab4efa7394a3267ffd63e77e5c0fe23f7d5099351b3eac92533762a8fe13c0865680d2790edb06a2e588eeb3aeac8f513c8f60fcc6612c6fa93f850960edc2d1544ccf083c11350aea2fde70f3753e80830090d26d90f26024d688924c488f5d7f85941757f93322f7cb059bc310f994ae96afdec1aae8bdc54a0f8cf052d195b19a86d55d950c71fd4311a39c952eb6e25f8a77af3454eb86fb20d106114de7801517e144f37e6c4f16c2daab39c43d8501b10afe510ea519b1ee62789e1c5332ac6f358f13615929e13ac049629d84493b35113f9a22a1a31f3ac4ae8a380b6fdb8f1d54607a6595bd214b0cd99d6d329de33e250376").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("7e111120fc832153f989848e645710b45ebb41773676ccceae067965").ToPositiveBigInteger(), 
                // public static other party
                new BitString("ea22918d218053fa418d87715734e59af6c08f208c6ccee669f527b17941f302def6a07aeb38ef2b2c9bb880130fc7e97bddfb01c8f5e6864923acbe9c0515a589e306bf50180a7ea9b9a4b99bffa6af0b55df542c904fe504d3fd54179ad2f99397da5d564cbb8225f5b135d05982ceea50d4f23836f37e95c821e1513c2d59303664d9f181d72e229345db4d9d1f9611046817c4af077c4d89b7768b0672fac94b1f67483404aceae6bf271352b0ac6e8ad5606f04c6f9770fc44da75ed1c599cee0922dd11031fc8b0c5f56abf72c1f2093e3ed3774e19c0494ea901715df33a78af60ef155d2dca034b8653b76d142c1b623e1300c8df33f70358137933d").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00009e55e127fa1262a5f45688b44efef2cd2c7067c872d5d4fbc2eb").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("78209298785c1afc11f936d9e23916fb820789837dc79d1ad12315ac76aa06e2e004970c790070a0854b1ece0cb24fd4e8c35cd17e5f2d65dc62bc7a34736b04e58c32853e84eb01241b6d40130e0092e933f8cc122c86525c949ae41f46caa915e32a822381cfd4041816230dd35dbff35c937a95c4486202e2720253a90d5c24bf79135f9f6fbb403f7d1412f832cb078fc0067db04709eb41c445804d4870490d27e90c6d10627e71c360961b90ec4c70812f8fb298fbbb69987babe6a6d34aacc07c513b827aa9cc75b76cb18b6317afdfd7b0c43f0a65c4de97169d27dcd13c29b50d28d03b5370deee474652e0b24695da8eb0cfe6f6f447f175e0eb56").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("17ec645e7229407c5638be23e3378b1610d2bdc162955b0d614fe5a147c2650f14e0abfe6095420f7d3fdf4d7f4089677d33e2a48ff406de07c9ab79c8733597e48b42b39558b94af03d2d5d83959ce8023b947201ea70b2ee5d788719baf36a9fc4861409f505d3559ec674025f1d736d6404d239c490ed3b6902fd2ce851439ec8ade6aa5377c1c8c9367fdaeefe43f9f3d1d69602bad337191fd12fbf092c6d8619018146f340c31e6e5357a73c6ef2553130aac3dd9c11d90189948dbbc251ee17f5dc1d1ebbcd9622a84a3ba28d60dca8abd86ed1191cf8c20904090daf3b335c9c9ad561fce8ed4c5ad0255aff78e9daf66342b8fd1b9ccae813969bfe"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964ee668aa91dfe915fd19cc9c2c89583ed81ffc9"),
                // expected dkm
                new BitString("07dcc06cf9bd351496fd0d41e90543c3"),
                // expected macData
                new BitString("5374616e646172642054657374204d6573736167656a74e3e0427e4e436b5036aff4894e60"),
                // expected tag
                new BitString("1eee9d416cc94848")
            },
            #endregion mqv2

            #region dhEphem
            #region hmac
            new object[]
            {
                // label
                "dhEphem hmac sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("dbf19f26ff8c38df5b512782b8a8ae646aa0d2228627c6f367db86fd35b6ff9f040ca486b4a3189fe1c54370d93339cf48185efc320b7dd17325b22a019a3fe89516b27930744c3e13bf5b832d9f08e1660d4ebe45794207bd9b193aeb309414c1a22822f69bc70a0565a4937113addf9a5e533ea93770abb9e6cd0faec1a6e9a956654efd7fb3582381cfa2a23431ae86a1e321beb105518d9554e4a83573f59275ff88700d8f619274928fe3dd8281a2b81426037395a76342bfb828957f59d24daba2430d9713d8ae4b71cd61de1498563c0051334ff86ac66e8cbc13069afa597a60ac201378e4578d992f9441f99fd8b8659847d89ddcdf2b92e67e4c2f").ToPositiveBigInteger(),
                    // q
                    new BitString("c270a8242b98b35549e0fc9a47758cdb804ae69f84e7197e5fb6313b").ToPositiveBigInteger(), 
                    // g
                    new BitString("61c9f88be5d914fe0d513bb1fdc319c9420702d1c3b9caa01bfb277b764ddd3312f2067a1251817217e6ff02642dd4d289616f6812a9371b5e4916a714b4ddce38c7a268e6014041af1b4604830c1d9da7f8f16067935f09c5918a1fefed54268489d46f95ae482d76a2d2166a95a28e8d9fa25539238f13c785db59b97c7fc1f9208f30ff594c397a8f5e8532c114b91cbdad95547c78b4a7470cf29acf062f066d5b6160accbdc2ff5fa2b74a3d12acda578ecc8f8898054c5b8f3099f85023c4c4b32cea181e5792614e799693569e6cff8b6ca2753f5b94111ed86b21f8e98b11b37cef7da999223899fff20abd3aa9062d8c03655fbb7d726111641efae").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhEphem,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // key length
                112,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("34705da4d8921273df6330aacc2b6f6f"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("00").ToPositiveBigInteger(),
                // public static this party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("9a605c81ce070651179b45a80c85c11f016acebaef35acd86c6b8689").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("91e613404cd0008dc355938267ff694d322d5ca501b1fee1ad02dfc6e9a594d3a0c4c783b0ca9c356e767c7f88a6eaecc3af9cc3d1feb6eab9e931a01de77621f8384af85335e898f19feb761eb69a06e26d51d52454e80345b40f7ab0f6ea11a317df15c1f27b598a2fb2a2d785e901598aa67e7024969c3ed1a5360cac1055db70e437ab1c090343ccfe1bc5c9646f0632dbb11eae6f363843d03bb5c694f6c5e3d42cd5a725b915c52e19375bd8f5f175968cc96015180864e095d8ae76f9d529ca1f34a22ef16da42cd2bbba2093b7f75b3aa2b2dcc9a0bd562215bd333ca46b54553664958cd63dbce064345bcec96f64b6bdbe3f15ca4ce8c2be28ce43").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("00").ToPositiveBigInteger(),
                // public static other party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("33c07fcae0241ed4848b46ead94f1ce9225027a252e91f2200b3a59f").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("d875db9f7e9ffc6f92219187663b9d243b2b563685723836eaa801cf6ab5422503780ebada23bf8e55ec084c9e84ffc8024b274ac3d4a7a7a483708b7e18b4ba5437b2f295f8bf65dcd7cd87b1d37d64ade05b0853bc36ab8922835bf2139637bda7951745d6cfa8969cc6525d6203204d3f6d986266570147faa92dfab60ab28e743e35a6ef4211f38c66d0afcb0dbc4c78898ad591323fb2dec6e846056a935a6a0a58b5e96b44241d0f52f49e7fde9d44e6590e328541452c138b7f258a72336c281d62c444d081070c2400fdbcf29fb9cad138598e08fddc1de0220d0e8c2254799cd3291bff14e63667137ba00cdc278d3486d07ee06a4cf53696a602a3").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("940702cfcd2b22c7d7125244286012c86f189a2eb179c8203aaeb3358495bc97ee79e79bb6caabe300c9fa8f738dcad9dc88027834b42fb7be984d5a14fe1dd716035b9aba9caa71a0ee1ff0e16505f824d44be4709c3757528bdf41d526aaa68a9a83e0e7ab473ed06d169227dc0dd58087a0d4d77dda346f023766f1dd58ade0541dec4dd5156bf008653085577ba4e0f294555db7cb1c0ea7372e7a16b5abc4b8b03a0aa5258455c2ec8bd680f3a5bed512ef832b25011273952309c3750ffca457960136883d5926d2f804d5e1f28c91bd0a3c2d3b8f54ba1a477f538113e323cf2ddbb699b8bfef9f58795744296dc3c215bb329f3aad1ac44b74ed3f15"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964c93e5f2e205ae6451126e854ebc6c5c3c403f5"),
                // expected dkm
                new BitString("4eea0a68e79bed8f0249ea26378e"),
                // expected macData
                new BitString("5374616e646172642054657374204d65737361676534705da4d8921273df6330aacc2b6f6f"),
                // expected tag
                new BitString("ba849eb027eeb2ad")
            },
            new object[]
            {
                // label
                "dhEphem hmac sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("dbf19f26ff8c38df5b512782b8a8ae646aa0d2228627c6f367db86fd35b6ff9f040ca486b4a3189fe1c54370d93339cf48185efc320b7dd17325b22a019a3fe89516b27930744c3e13bf5b832d9f08e1660d4ebe45794207bd9b193aeb309414c1a22822f69bc70a0565a4937113addf9a5e533ea93770abb9e6cd0faec1a6e9a956654efd7fb3582381cfa2a23431ae86a1e321beb105518d9554e4a83573f59275ff88700d8f619274928fe3dd8281a2b81426037395a76342bfb828957f59d24daba2430d9713d8ae4b71cd61de1498563c0051334ff86ac66e8cbc13069afa597a60ac201378e4578d992f9441f99fd8b8659847d89ddcdf2b92e67e4c2f").ToPositiveBigInteger(),
                    // q
                    new BitString("c270a8242b98b35549e0fc9a47758cdb804ae69f84e7197e5fb6313b").ToPositiveBigInteger(), 
                    // g
                    new BitString("61c9f88be5d914fe0d513bb1fdc319c9420702d1c3b9caa01bfb277b764ddd3312f2067a1251817217e6ff02642dd4d289616f6812a9371b5e4916a714b4ddce38c7a268e6014041af1b4604830c1d9da7f8f16067935f09c5918a1fefed54268489d46f95ae482d76a2d2166a95a28e8d9fa25539238f13c785db59b97c7fc1f9208f30ff594c397a8f5e8532c114b91cbdad95547c78b4a7470cf29acf062f066d5b6160accbdc2ff5fa2b74a3d12acda578ecc8f8898054c5b8f3099f85023c4c4b32cea181e5792614e799693569e6cff8b6ca2753f5b94111ed86b21f8e98b11b37cef7da999223899fff20abd3aa9062d8c03655fbb7d726111641efae").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhEphem,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // key length
                112,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("34705da4d8921273df6330aacc2b6f6f"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("00").ToPositiveBigInteger(),
                // public static this party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("33c07fcae0241ed4848b46ead94f1ce9225027a252e91f2200b3a59f").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("d875db9f7e9ffc6f92219187663b9d243b2b563685723836eaa801cf6ab5422503780ebada23bf8e55ec084c9e84ffc8024b274ac3d4a7a7a483708b7e18b4ba5437b2f295f8bf65dcd7cd87b1d37d64ade05b0853bc36ab8922835bf2139637bda7951745d6cfa8969cc6525d6203204d3f6d986266570147faa92dfab60ab28e743e35a6ef4211f38c66d0afcb0dbc4c78898ad591323fb2dec6e846056a935a6a0a58b5e96b44241d0f52f49e7fde9d44e6590e328541452c138b7f258a72336c281d62c444d081070c2400fdbcf29fb9cad138598e08fddc1de0220d0e8c2254799cd3291bff14e63667137ba00cdc278d3486d07ee06a4cf53696a602a3").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("00").ToPositiveBigInteger(),
                // public static other party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("9a605c81ce070651179b45a80c85c11f016acebaef35acd86c6b8689").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("91e613404cd0008dc355938267ff694d322d5ca501b1fee1ad02dfc6e9a594d3a0c4c783b0ca9c356e767c7f88a6eaecc3af9cc3d1feb6eab9e931a01de77621f8384af85335e898f19feb761eb69a06e26d51d52454e80345b40f7ab0f6ea11a317df15c1f27b598a2fb2a2d785e901598aa67e7024969c3ed1a5360cac1055db70e437ab1c090343ccfe1bc5c9646f0632dbb11eae6f363843d03bb5c694f6c5e3d42cd5a725b915c52e19375bd8f5f175968cc96015180864e095d8ae76f9d529ca1f34a22ef16da42cd2bbba2093b7f75b3aa2b2dcc9a0bd562215bd333ca46b54553664958cd63dbce064345bcec96f64b6bdbe3f15ca4ce8c2be28ce43").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("940702cfcd2b22c7d7125244286012c86f189a2eb179c8203aaeb3358495bc97ee79e79bb6caabe300c9fa8f738dcad9dc88027834b42fb7be984d5a14fe1dd716035b9aba9caa71a0ee1ff0e16505f824d44be4709c3757528bdf41d526aaa68a9a83e0e7ab473ed06d169227dc0dd58087a0d4d77dda346f023766f1dd58ade0541dec4dd5156bf008653085577ba4e0f294555db7cb1c0ea7372e7a16b5abc4b8b03a0aa5258455c2ec8bd680f3a5bed512ef832b25011273952309c3750ffca457960136883d5926d2f804d5e1f28c91bd0a3c2d3b8f54ba1a477f538113e323cf2ddbb699b8bfef9f58795744296dc3c215bb329f3aad1ac44b74ed3f15"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964c93e5f2e205ae6451126e854ebc6c5c3c403f5"),
                // expected dkm
                new BitString("4eea0a68e79bed8f0249ea26378e"),
                // expected macData
                new BitString("5374616e646172642054657374204d65737361676534705da4d8921273df6330aacc2b6f6f"),
                // expected tag
                new BitString("ba849eb027eeb2ad")
            },
            new object[]
            {
                // label
                "dhEphem hmac sha2-512",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("df892333ccd1d5b044fa9f0950ba860a2674a0ad6c9b9fce68172e6dad9fa2dbb83240b1a3ef6658606bc3055447553995d288d3076eddad82deb0b929cc5022827ccfdb294403a1848ab9e26cc4d0be6f5548f3b32c184cb2a14034265763bb74a708124d98e996f61aeca5a7f828fccafde96db3736b125a7db6f6a1c681b1377956e9e7682bd9e407c925f88a840073dc7bb894964be2be7f10a82f9be905024e490ad13942580c40b49721b21a70475fe896612b1d85971db7786dfc71bc445ad92320f8a6b658ee1915df6fe9dac46c9235ec3243a13eef86f598e1facf1fc2d90ee3850225e4a0c280e68c41b0ebdcff69e9ab2367b056bbf47d487c2b").ToPositiveBigInteger(),
                    // q
                    new BitString("9f1ab009208dd4c30838caf271dcba98a50a556c47a11738faafb45bbbdeadbf").ToPositiveBigInteger(), 
                    // g
                    new BitString("a6003a64e8274e64d3f64352d5983af4a9082f85b74461762d1d001f5624ee556d9fae2277a64614f64ff424089fda44ad2ed2d3d6f9c7f1fa45db2128ffd179a6be2485b16fd3facea0e396ce2650631c387d8abe4e8e2ebc90a355543301bf2ca3ddcf9b2d563d28663c1ed2a3411013e91d633f797877f8974d2648dd37197aec63aaaf674210c7849f0fd9b7d8331cd8b3b7d0f76895d7d65951f2c5d414a5d3289b444560c7a5ba1d94e6e011ec2da9341a86252d6ebad9514c0b24aab71e25084ea9ed33f8b66e622c198aa9fde243c636fce29004dc5c5b0bf3d1bdeb9510d374307aaf7f7e63583a9788bc9dd0df9045235ceed562e105bbb0cff992").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhEphem,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d512,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // key length
                112,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("e6a7b7fd7feb0315823e2d6b40061434"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("00").ToPositiveBigInteger(),
                // public static this party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("6eff9e56cc20c156555980739322aa1742f423a5016fbda2844822282511d2f0").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("58e2799a2f8b982158ffb28c16f21a68b457d1f397366d4d754a53f5146ca7a3cc9914c56962caee720bf269a1843a448fe24d85aa9f4b75498ca93ef6dcb293f786988bd61871c21529563c36626108ae1c2686364cc8c0401d95ecd1f31f27f82d4201cae8e7d7e7b47726e9fa0d557dbcfa7ce3a0a6f21ad4dcc290e63d998dca99b2bc6f4aaf785cb283a0cc923c406279d23e0bb36281f65989cd0a6363fa48fa5616e15d889b998792e21dfdabfd944646f610e1f6dd706c44b1c333f01cda17fc9b1f81cacf8d0a91779a4f6ea4e3849e492c30f0a096f2822d06f174e6f3dc741b3c4d792fe6be3a8aefe9f6999c15d983de96729e9f295ca78c9789").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("00").ToPositiveBigInteger(),
                // public static other party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("0564098c67915d2b66823755206978710087d9fc3096d62694cca73b6444a043").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("4fd088a8c4da7e08e9ebb33c9be4b18f2c90bc4f7a74358285586eef953884282af51cf80118cedda8c9b0cfe10346e94ecf7f6a8ccedd8bc8f0e7718a34aab5a6aae166e20be0e4e6670ee5af553c3aeb9bbb0ddb5e2985e804c6e80f79b487b912530604c85cfb8e278e334a94b186a9946aa6f46bfcfa56020ec0954d3a786598b03e7cd0cc63de9ab72a70eefd05446195b66d6748201f6aed3b3963dfe55004ad7af22ada15f23b56b21f1bdfe1fa1f1da5a7cf9123c080d0e44ee507e562be13081d2d399614f855e9ae9d7aa32b8761fb37899808dadc8e0f29a1368ff99e8a7d4c91354b09e0244fac78585cd281830647908442013464df5f127b02").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("18c08b86cdd00b70262b183bf0204bc9eb4b38bd42effb54696e732234acc22a2a1d34db9701b932c913988f46fb635803b89dcfb2566f608174077413935a02269616ff65230bb57e18867cec5ceeef248155b72c1eea984da76fe0fff31ee0b8e4ed4d1d0dcdef83fcbb87956f4b82081238f5c2a7294f2016d7a4252a60609860f19c118b9cf68c4edad8202b08be7b2a0f35b16e20d9a18be602a504d2a5734b9eac3bc69c594faeb5a39d1d2428c4be6f539fc6c979b40e4ca5c1b50001c49051b93c013f371d4f956f4e892ce627a33fe4c96b911c78b8d7193c65dc18bfa09538ec17b3f5408cecdb524175d83c03c03193b4b2512c23476ac86f9b22"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964dddf368b2e51f550172be92f88ee9f548ec1b2"),
                // expected dkm
                new BitString("0758e98ba91f92d4f288a2925893"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765e6a7b7fd7feb0315823e2d6b40061434"),
                // expected tag
                new BitString("59d540f1e488e373")
            },
            new object[]
            {
                // label
                "dhEphem hmac sha2-512 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("df892333ccd1d5b044fa9f0950ba860a2674a0ad6c9b9fce68172e6dad9fa2dbb83240b1a3ef6658606bc3055447553995d288d3076eddad82deb0b929cc5022827ccfdb294403a1848ab9e26cc4d0be6f5548f3b32c184cb2a14034265763bb74a708124d98e996f61aeca5a7f828fccafde96db3736b125a7db6f6a1c681b1377956e9e7682bd9e407c925f88a840073dc7bb894964be2be7f10a82f9be905024e490ad13942580c40b49721b21a70475fe896612b1d85971db7786dfc71bc445ad92320f8a6b658ee1915df6fe9dac46c9235ec3243a13eef86f598e1facf1fc2d90ee3850225e4a0c280e68c41b0ebdcff69e9ab2367b056bbf47d487c2b").ToPositiveBigInteger(),
                    // q
                    new BitString("9f1ab009208dd4c30838caf271dcba98a50a556c47a11738faafb45bbbdeadbf").ToPositiveBigInteger(), 
                    // g
                    new BitString("a6003a64e8274e64d3f64352d5983af4a9082f85b74461762d1d001f5624ee556d9fae2277a64614f64ff424089fda44ad2ed2d3d6f9c7f1fa45db2128ffd179a6be2485b16fd3facea0e396ce2650631c387d8abe4e8e2ebc90a355543301bf2ca3ddcf9b2d563d28663c1ed2a3411013e91d633f797877f8974d2648dd37197aec63aaaf674210c7849f0fd9b7d8331cd8b3b7d0f76895d7d65951f2c5d414a5d3289b444560c7a5ba1d94e6e011ec2da9341a86252d6ebad9514c0b24aab71e25084ea9ed33f8b66e622c198aa9fde243c636fce29004dc5c5b0bf3d1bdeb9510d374307aaf7f7e63583a9788bc9dd0df9045235ceed562e105bbb0cff992").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhEphem,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d512,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // key length
                112,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("e6a7b7fd7feb0315823e2d6b40061434"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("00").ToPositiveBigInteger(),
                // public static this party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("0564098c67915d2b66823755206978710087d9fc3096d62694cca73b6444a043").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("4fd088a8c4da7e08e9ebb33c9be4b18f2c90bc4f7a74358285586eef953884282af51cf80118cedda8c9b0cfe10346e94ecf7f6a8ccedd8bc8f0e7718a34aab5a6aae166e20be0e4e6670ee5af553c3aeb9bbb0ddb5e2985e804c6e80f79b487b912530604c85cfb8e278e334a94b186a9946aa6f46bfcfa56020ec0954d3a786598b03e7cd0cc63de9ab72a70eefd05446195b66d6748201f6aed3b3963dfe55004ad7af22ada15f23b56b21f1bdfe1fa1f1da5a7cf9123c080d0e44ee507e562be13081d2d399614f855e9ae9d7aa32b8761fb37899808dadc8e0f29a1368ff99e8a7d4c91354b09e0244fac78585cd281830647908442013464df5f127b02").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("00").ToPositiveBigInteger(),
                // public static other party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("6eff9e56cc20c156555980739322aa1742f423a5016fbda2844822282511d2f0").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("58e2799a2f8b982158ffb28c16f21a68b457d1f397366d4d754a53f5146ca7a3cc9914c56962caee720bf269a1843a448fe24d85aa9f4b75498ca93ef6dcb293f786988bd61871c21529563c36626108ae1c2686364cc8c0401d95ecd1f31f27f82d4201cae8e7d7e7b47726e9fa0d557dbcfa7ce3a0a6f21ad4dcc290e63d998dca99b2bc6f4aaf785cb283a0cc923c406279d23e0bb36281f65989cd0a6363fa48fa5616e15d889b998792e21dfdabfd944646f610e1f6dd706c44b1c333f01cda17fc9b1f81cacf8d0a91779a4f6ea4e3849e492c30f0a096f2822d06f174e6f3dc741b3c4d792fe6be3a8aefe9f6999c15d983de96729e9f295ca78c9789").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("18c08b86cdd00b70262b183bf0204bc9eb4b38bd42effb54696e732234acc22a2a1d34db9701b932c913988f46fb635803b89dcfb2566f608174077413935a02269616ff65230bb57e18867cec5ceeef248155b72c1eea984da76fe0fff31ee0b8e4ed4d1d0dcdef83fcbb87956f4b82081238f5c2a7294f2016d7a4252a60609860f19c118b9cf68c4edad8202b08be7b2a0f35b16e20d9a18be602a504d2a5734b9eac3bc69c594faeb5a39d1d2428c4be6f539fc6c979b40e4ca5c1b50001c49051b93c013f371d4f956f4e892ce627a33fe4c96b911c78b8d7193c65dc18bfa09538ec17b3f5408cecdb524175d83c03c03193b4b2512c23476ac86f9b22"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964dddf368b2e51f550172be92f88ee9f548ec1b2"),
                // expected dkm
                new BitString("0758e98ba91f92d4f288a2925893"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765e6a7b7fd7feb0315823e2d6b40061434"),
                // expected tag
                new BitString("59d540f1e488e373")
            },
            #endregion hmac
            #region cmac
            new object[]
            {
                // label
                "dhEphem cmac sha2-256",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d09d74eece8be7fdff51dc238636e5207631a584c07c96f47d55a4666d28286db309c6107d8c264358ab091946ff2a1bb79a59eced4467e82a582131d1caf4fbb684d952a749cdf0ad80202b03fa4ecfd85baeb01effb593bc93bf48123b465711431fc11cae1f134ec51cd4b44331a09bc002075542b37f986de757205bd7007b3a3cdd718e9db29780bbc529308af5b9e52096c927316f03761628215c987a3768703a00934580522181ae27a743a7df71cdec20822279ec4283db5ab30a7a52a04f913fb3c5d7f8897993db1dc8a99bc226c8271e3574d1940f4954eb4475f4c77a2a61aa1920507bc152c7331a57dc3431bc7aaadab97958684c9d6028ed").ToPositiveBigInteger(),
                    // q
                    new BitString("8eca313f1b0c9211f5fa2aec825e99bf7a930f1250af846c74dc63df").ToPositiveBigInteger(), 
                    // g
                    new BitString("005b768d45d72e17a87e8b1b34d748066159bd0b6abc802364978fa50a269fba4aead7a2b648b710441b4f99241b1454e10146c217607ef4f19d4a2eed35d3fe29e80b434e43d0c6c016b4c700f780a1b4da0ec03db827ca1b1d583624188a1e21817bf56aef84d0ee1fae7b694a97db9bfbf32027326c7a0add10636090810831920520dd7cfa9f91362c80d34c0e900dcd7d6e234ba45d7af1bcab581d7b9946e4ed7008e7404d01262a9ce8a47d339b42fff35a32bd232a431d1fbae6c27b146e6039fbff5890cd4372e81d2dfed40223e82c90aca36bb4f513574d53c3b3eafe566cccbc77c1255985ca73e91afd0498401f22157a6ea88322ad293512c4").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhEphem,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d256,
                // mac type
                KeyAgreementMacType.CmacAes,
                // key length
                128,
                // tag length
                80,
                // noKeyConfirmationNonce
                new BitString("e95d16220c13dd4f3aa40f44f6e24bf6"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("00").ToPositiveBigInteger(),
                // public static this party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("5e1a7ca61ee4759ba46144cb873ee1ffccc155237df04002efe8c173").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("0d5745eb14a277033fa9db517ecb323ee3997b0b0b230bf528429d4c64cbc7b3c5a141cedc6caca65388d199efcd4ae719cb22160f013bd94a518339361f2ed685599a6f5af16700ac4ff744b37e9abe2d9a4def314308b77eabe0b0af0c3779ea8eecf2ab8d31c81a8120b3cadff1d29e866892b21534daf36417e841e49ad55e7d13e4690bc49c68dbb8667a86c150110b3c05dc1bda7d8a4841539acd25dc32ab822bf045b2789061c2c9db3e4683cf1853a28e4a9af99d0ba66aca315e9ddb3948e756771f0e780da7b5fdfd30ea8e356508199a8ff750089aaa09a8104379c8e2fd9e058694fad10d74de790ebf39dcebe576fd75f6a3c8d35d7161a072").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("00").ToPositiveBigInteger(),
                // public static other party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("236658b70be16ef58355b1de4b0a4c685737435a25bf84b773065159").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("7c6191d1fcc854d0d6dd8a7b69abf10a9fa720d106b087277a1bd03adc6a28c113e082d649870e126c2db976dd97afdbfa74b106e192d6fd053d5cc26c707d474513673d68dbc8d5b17976ba8d2c53414900c06c9fa3dfd5eb380db50cb0d735cb0644277411f5d799a683d5fafa0d2536416d01824f3dd55df2c4dc09194a6fe17a107b15e57fdf91ee2c6cfe3203ca1b8d3975acf3df42f2147f1c7888659602e5ebe68f44426ba2eacccd4a7c1153001c3130fd02dc741c5e847c0c3b012a7d13e3b9eecefa35a9bb40e47713c7f4871dff44b5f6a23ae13d2be0dab0968ce9fb6423ba71e1803ec099ffe32b55d4b70c15957edd04cf9c25d80dc8fc5ef5").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("be54338e08a2c15ea7954eec28c417ad4d048be2af37d91a718b7e5bf0cf5348c35e4771c9b1e63c9bb96009d3ef63015355c762e6c13e28388f70719a612a9a468fcd2278648f28d4a83e4fb378a173b82cc94d12a21859e5887c9d81bda09644d3ccc57c1a87795b4659fb9e2591489774f2e612898d335818e60c7d703e1958077b19d938c4705792cfc1898b5ea1ac408ae0f29bc1da238b78a628a0e25efbea21d95f8d34ae8ab446fa436e9010c566271c225e38b5b0cf96c66ecd68ecc2f58958f9f515f28990336649e08ef8ade0964dbd6795e8ee34b098900023f90785ca46c7bab13a062fb29861968b83be1e10d17d45581d69e85217bc41477e"),
                // expected oi
                new BitString("a1b2c3d4e543415653696468284b6b639dfc69748d0602c3906cbd107695"),
                // expected dkm
                new BitString("09431b666b55dbbd421a9a7e93b5665b"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765e95d16220c13dd4f3aa40f44f6e24bf6"),
                // expected tag
                new BitString("217ad7b7780dbc8a244c")
            },
            new object[]
            {
                // label
                "dhEphem cmac sha2-256 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d09d74eece8be7fdff51dc238636e5207631a584c07c96f47d55a4666d28286db309c6107d8c264358ab091946ff2a1bb79a59eced4467e82a582131d1caf4fbb684d952a749cdf0ad80202b03fa4ecfd85baeb01effb593bc93bf48123b465711431fc11cae1f134ec51cd4b44331a09bc002075542b37f986de757205bd7007b3a3cdd718e9db29780bbc529308af5b9e52096c927316f03761628215c987a3768703a00934580522181ae27a743a7df71cdec20822279ec4283db5ab30a7a52a04f913fb3c5d7f8897993db1dc8a99bc226c8271e3574d1940f4954eb4475f4c77a2a61aa1920507bc152c7331a57dc3431bc7aaadab97958684c9d6028ed").ToPositiveBigInteger(),
                    // q
                    new BitString("8eca313f1b0c9211f5fa2aec825e99bf7a930f1250af846c74dc63df").ToPositiveBigInteger(), 
                    // g
                    new BitString("005b768d45d72e17a87e8b1b34d748066159bd0b6abc802364978fa50a269fba4aead7a2b648b710441b4f99241b1454e10146c217607ef4f19d4a2eed35d3fe29e80b434e43d0c6c016b4c700f780a1b4da0ec03db827ca1b1d583624188a1e21817bf56aef84d0ee1fae7b694a97db9bfbf32027326c7a0add10636090810831920520dd7cfa9f91362c80d34c0e900dcd7d6e234ba45d7af1bcab581d7b9946e4ed7008e7404d01262a9ce8a47d339b42fff35a32bd232a431d1fbae6c27b146e6039fbff5890cd4372e81d2dfed40223e82c90aca36bb4f513574d53c3b3eafe566cccbc77c1255985ca73e91afd0498401f22157a6ea88322ad293512c4").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhEphem,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d256,
                // mac type
                KeyAgreementMacType.CmacAes,
                // key length
                128,
                // tag length
                80,
                // noKeyConfirmationNonce
                new BitString("e95d16220c13dd4f3aa40f44f6e24bf6"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("00").ToPositiveBigInteger(),
                // public static this party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("236658b70be16ef58355b1de4b0a4c685737435a25bf84b773065159").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("7c6191d1fcc854d0d6dd8a7b69abf10a9fa720d106b087277a1bd03adc6a28c113e082d649870e126c2db976dd97afdbfa74b106e192d6fd053d5cc26c707d474513673d68dbc8d5b17976ba8d2c53414900c06c9fa3dfd5eb380db50cb0d735cb0644277411f5d799a683d5fafa0d2536416d01824f3dd55df2c4dc09194a6fe17a107b15e57fdf91ee2c6cfe3203ca1b8d3975acf3df42f2147f1c7888659602e5ebe68f44426ba2eacccd4a7c1153001c3130fd02dc741c5e847c0c3b012a7d13e3b9eecefa35a9bb40e47713c7f4871dff44b5f6a23ae13d2be0dab0968ce9fb6423ba71e1803ec099ffe32b55d4b70c15957edd04cf9c25d80dc8fc5ef5").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("00").ToPositiveBigInteger(),
                // public static other party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("5e1a7ca61ee4759ba46144cb873ee1ffccc155237df04002efe8c173").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("0d5745eb14a277033fa9db517ecb323ee3997b0b0b230bf528429d4c64cbc7b3c5a141cedc6caca65388d199efcd4ae719cb22160f013bd94a518339361f2ed685599a6f5af16700ac4ff744b37e9abe2d9a4def314308b77eabe0b0af0c3779ea8eecf2ab8d31c81a8120b3cadff1d29e866892b21534daf36417e841e49ad55e7d13e4690bc49c68dbb8667a86c150110b3c05dc1bda7d8a4841539acd25dc32ab822bf045b2789061c2c9db3e4683cf1853a28e4a9af99d0ba66aca315e9ddb3948e756771f0e780da7b5fdfd30ea8e356508199a8ff750089aaa09a8104379c8e2fd9e058694fad10d74de790ebf39dcebe576fd75f6a3c8d35d7161a072").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("be54338e08a2c15ea7954eec28c417ad4d048be2af37d91a718b7e5bf0cf5348c35e4771c9b1e63c9bb96009d3ef63015355c762e6c13e28388f70719a612a9a468fcd2278648f28d4a83e4fb378a173b82cc94d12a21859e5887c9d81bda09644d3ccc57c1a87795b4659fb9e2591489774f2e612898d335818e60c7d703e1958077b19d938c4705792cfc1898b5ea1ac408ae0f29bc1da238b78a628a0e25efbea21d95f8d34ae8ab446fa436e9010c566271c225e38b5b0cf96c66ecd68ecc2f58958f9f515f28990336649e08ef8ade0964dbd6795e8ee34b098900023f90785ca46c7bab13a062fb29861968b83be1e10d17d45581d69e85217bc41477e"),
                // expected oi
                new BitString("a1b2c3d4e543415653696468284b6b639dfc69748d0602c3906cbd107695"),
                // expected dkm
                new BitString("09431b666b55dbbd421a9a7e93b5665b"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765e95d16220c13dd4f3aa40f44f6e24bf6"),
                // expected tag
                new BitString("217ad7b7780dbc8a244c")
            },
            #endregion cmac
            #region aes-ccm
            new object[]
            {
                // label
                "dhEphem ccm sha2-256",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d5cf9ba288ff8438650904a5fe2eeb8bf6b52b691a455b21bd3b37998f82544036ae61fe436039c66feab83ba21a5ba13e7b4c1692ac82c65309eed54e593efe9831cc82bfd11e9552d4eb7d8f2c233024bc10819c57093890ec19aa9ee915e524d81c285928a5b87acc7f496f93689ed59b15183689ec5e487fdc9fb994c4bf7e1dba57f8f12c17e404f68b5506d69f4aa98146f6ba3be1f3397e5d41c4235a18b38c75417bb9091a487db1fc89eb2f73b4ac6f6c12dc5634a342cd9d10605090bd544e1c0813aaaa3814166df84a302b8df38babe9c31e4a2c64317046bdb841fa8717e22dcf1496f9ad94c17f1ff9175567d02ab5e30601e2e5b0884e9a77").ToPositiveBigInteger(),
                    // q
                    new BitString("ceb9916bbc14cdc9dda80481135bee68ee94f4ecadc2921261a316d1c9cf9283").ToPositiveBigInteger(), 
                    // g
                    new BitString("37a7b2a094ed6253784d51c71a4a7407f0727df15e480a02b0ff2f9befdb0e92c0d63e482c6909e5337373abe347ceaec25a9f2a23f1770c2447bfb5c35dbda80d3f00d8b6569a0165a3d52f1cff03e02b67be47b2f26ebd0fc13299fd0317719c3bcbbcbba0a982915b5d68fb4c5c483c3df12052b56ceacd16176c783d56422b1366cd5ce65922b734a1780acf35b4a658cfe8166469ace04b87a33e1e8d4603ae3f9607d1e708137d581aaf2cd86608ca06e52cddbe475f8bdb4597cd5b55f47380276c86d08d8ddbbef4ee3ca76cefe6ff9c74d4d5d1be3407e30720be34cf41f482be8bc6ed6e89ea7cc2d164da1dc1c9d0e2762eef983dc9e96f3bfd50").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhEphem,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d256,
                // mac type
                KeyAgreementMacType.AesCcm,
                // key length
                128,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("1d33ef74a9f3c6e7ab1dfa77b40e4191"),
                // aes-ccm nonce
                new BitString("6526d522a19f25"),
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("00").ToPositiveBigInteger(),
                // public static this party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("41f8582a2f3ac19cc925fb5cf72ec98f89630fec8e71853397870c3fda1eb08b").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("a6f3b734391112b2ae81cb3021bc1275383c4a6c80b9a1265ae3f5a8256a0a621667f71e3e081b86bc427d5adddb901aacd4e3b7b48170c7d13cd9dccd7b04072d97a7c39fc44c079a48fa3d2495e90659786275f13b5ad2402509dd12637630de980369c5e55953091df4ef3e369880547737df1d7bd7a9d437488cb01a94b444c05ae33d6ae6126bd0c9c7be19adc752f210f68ee7657a9151ef3299b4e4e12f19ba9f3070cacfe1af329e69a2bd6416c51c7d33b183702dbd20df91ca09c5190321f59f55fca903546cc42c3edcb3c4eb30b3a4228fe2e1906484174715e0b08d9d93c4588c33739f416ca45a33faa861dc6e9fb7831cc95d2d2741533be3").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("00").ToPositiveBigInteger(),
                // public static other party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("a388f6b35f25619d1e76ed7cc6d10f8f3a6d919e6560c3f0090eb96e1f27d908").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("4fc5ed02abf0791bb01ad33527bd16ee58f875bbc85de5b513ccce95d05d6c735e726a102f11a69152f1c430b027ef3f2049e07db96f448d9b016ebd9258cd33175c06c00d107c99ccfebc8e77b19c61ed7c9e1d8a3e912a893b30ec9ab1d59d2bd5133c9669090c7bcc48318a21cabffbe56477e9fda36aba9c5e4462854fa682f503de3e895579b0c51a2f2a6b1fc59bee015e8bd89041a2efafbe8ea491215a88302f6a4858ea3c0d846e52f6ea070b61dda82009bdf0ed13a077ea1777233e059fed4c45249c5bbba1fbaa6f574058ec58d56e6b3a486e0aa306ef2d63d70617f6b384fe238e82832dfeff6317aa887aab894efc9b60e6f5127581fe735c").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("93dd543113b567adb45dad4149a7503768e5c4d43964a578357541df39353ce36910a75ae7833914e105f732f0dc34339007dfb14aba37a65ad825a2a63f70e5522bdce9e2e757c103104139e09671668d8f1c3edc2d4b41a623f0b24332e1d0569e992ca56855443e2d1c587b853dcc4fa816ca63f95f6fd1fa735d58389a3110b4863fa791168ae509ba1a87fa1d9e1ddde40e91126179c9cce0770555078e788add2497f9f30934a917acbc6e8377e9c20573fd594aa5e488847602ba83e721a8a7f24c198bdb81128edef59949e359b590ed933810b692cec57c65ded444ba4a221d86ad5c4ad70d1dcd2b47cc0c70cdf2ffaaf9d893e3bf3a5d0880bf05"),
                // expected oi
                new BitString("a1b2c3d4e54341565369646cfd9fa9ec70ae7f9b0d17cc63ea2103fbaf6b"),
                // expected dkm
                new BitString("0edc6e20af7c11ef41457deb6db9ad0b"),
                // expected macData
                new BitString("5374616e646172642054657374204d6573736167651d33ef74a9f3c6e7ab1dfa77b40e4191"),
                // expected tag
                new BitString("9a0ade2f4b22599e")
            },
            new object[]
            {
                // label
                "dhEphem ccm sha2-256 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d5cf9ba288ff8438650904a5fe2eeb8bf6b52b691a455b21bd3b37998f82544036ae61fe436039c66feab83ba21a5ba13e7b4c1692ac82c65309eed54e593efe9831cc82bfd11e9552d4eb7d8f2c233024bc10819c57093890ec19aa9ee915e524d81c285928a5b87acc7f496f93689ed59b15183689ec5e487fdc9fb994c4bf7e1dba57f8f12c17e404f68b5506d69f4aa98146f6ba3be1f3397e5d41c4235a18b38c75417bb9091a487db1fc89eb2f73b4ac6f6c12dc5634a342cd9d10605090bd544e1c0813aaaa3814166df84a302b8df38babe9c31e4a2c64317046bdb841fa8717e22dcf1496f9ad94c17f1ff9175567d02ab5e30601e2e5b0884e9a77").ToPositiveBigInteger(),
                    // q
                    new BitString("ceb9916bbc14cdc9dda80481135bee68ee94f4ecadc2921261a316d1c9cf9283").ToPositiveBigInteger(), 
                    // g
                    new BitString("37a7b2a094ed6253784d51c71a4a7407f0727df15e480a02b0ff2f9befdb0e92c0d63e482c6909e5337373abe347ceaec25a9f2a23f1770c2447bfb5c35dbda80d3f00d8b6569a0165a3d52f1cff03e02b67be47b2f26ebd0fc13299fd0317719c3bcbbcbba0a982915b5d68fb4c5c483c3df12052b56ceacd16176c783d56422b1366cd5ce65922b734a1780acf35b4a658cfe8166469ace04b87a33e1e8d4603ae3f9607d1e708137d581aaf2cd86608ca06e52cddbe475f8bdb4597cd5b55f47380276c86d08d8ddbbef4ee3ca76cefe6ff9c74d4d5d1be3407e30720be34cf41f482be8bc6ed6e89ea7cc2d164da1dc1c9d0e2762eef983dc9e96f3bfd50").ToPositiveBigInteger()
                ), 
                // scheme
                FfcScheme.DhEphem,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // DSA/KDF hash mode
                ModeValues.SHA2,
                // DSA/KDF digest size
                DigestSizes.d256,
                // mac type
                KeyAgreementMacType.AesCcm,
                // key length
                128,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("1d33ef74a9f3c6e7ab1dfa77b40e4191"),
                // aes-ccm nonce
                new BitString("6526d522a19f25"),
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("00").ToPositiveBigInteger(),
                // public static this party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("a388f6b35f25619d1e76ed7cc6d10f8f3a6d919e6560c3f0090eb96e1f27d908").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("4fc5ed02abf0791bb01ad33527bd16ee58f875bbc85de5b513ccce95d05d6c735e726a102f11a69152f1c430b027ef3f2049e07db96f448d9b016ebd9258cd33175c06c00d107c99ccfebc8e77b19c61ed7c9e1d8a3e912a893b30ec9ab1d59d2bd5133c9669090c7bcc48318a21cabffbe56477e9fda36aba9c5e4462854fa682f503de3e895579b0c51a2f2a6b1fc59bee015e8bd89041a2efafbe8ea491215a88302f6a4858ea3c0d846e52f6ea070b61dda82009bdf0ed13a077ea1777233e059fed4c45249c5bbba1fbaa6f574058ec58d56e6b3a486e0aa306ef2d63d70617f6b384fe238e82832dfeff6317aa887aab894efc9b60e6f5127581fe735c").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("00").ToPositiveBigInteger(),
                // public static other party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("41f8582a2f3ac19cc925fb5cf72ec98f89630fec8e71853397870c3fda1eb08b").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("a6f3b734391112b2ae81cb3021bc1275383c4a6c80b9a1265ae3f5a8256a0a621667f71e3e081b86bc427d5adddb901aacd4e3b7b48170c7d13cd9dccd7b04072d97a7c39fc44c079a48fa3d2495e90659786275f13b5ad2402509dd12637630de980369c5e55953091df4ef3e369880547737df1d7bd7a9d437488cb01a94b444c05ae33d6ae6126bd0c9c7be19adc752f210f68ee7657a9151ef3299b4e4e12f19ba9f3070cacfe1af329e69a2bd6416c51c7d33b183702dbd20df91ca09c5190321f59f55fca903546cc42c3edcb3c4eb30b3a4228fe2e1906484174715e0b08d9d93c4588c33739f416ca45a33faa861dc6e9fb7831cc95d2d2741533be3").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("93dd543113b567adb45dad4149a7503768e5c4d43964a578357541df39353ce36910a75ae7833914e105f732f0dc34339007dfb14aba37a65ad825a2a63f70e5522bdce9e2e757c103104139e09671668d8f1c3edc2d4b41a623f0b24332e1d0569e992ca56855443e2d1c587b853dcc4fa816ca63f95f6fd1fa735d58389a3110b4863fa791168ae509ba1a87fa1d9e1ddde40e91126179c9cce0770555078e788add2497f9f30934a917acbc6e8377e9c20573fd594aa5e488847602ba83e721a8a7f24c198bdb81128edef59949e359b590ed933810b692cec57c65ded444ba4a221d86ad5c4ad70d1dcd2b47cc0c70cdf2ffaaf9d893e3bf3a5d0880bf05"),
                // expected oi
                new BitString("a1b2c3d4e54341565369646cfd9fa9ec70ae7f9b0d17cc63ea2103fbaf6b"),
                // expected dkm
                new BitString("0edc6e20af7c11ef41457deb6db9ad0b"),
                // expected macData
                new BitString("5374616e646172642054657374204d6573736167651d33ef74a9f3c6e7ab1dfa77b40e4191"),
                // expected tag
                new BitString("9a0ade2f4b22599e")
            },
            #endregion aes-ccm
            #endregion dhEphem

            #region dhHybridOneFlow
            #region hmac
            new object[]
            {
                // label
                "dhHybridOneFlow hmac512 sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("e41020ca42ffced66e039d39e5b3a14ee3c6d91feca227811a776ffd9c1b07024dea81fff33963be55a6790873e1ad2d74571dc8741e99c9f1ab8f41b8d0f9ee2c6909c63022c89166976d9bdcf4f3ea5b92e460356d4f21ae49eff275125685452cfdd104e6bbcffb10ce125bd9077987e595d39316ec46b12d86776d7fd11a08fbf5ba991e3e32490bbb86f21a8401be1d4e6d7f9c1181eacf24736b689d83db72066fa2c932916a21f850fc9c67c0014915ce02e4bc74edcb30034879bd836da8050169bdeb8d431ec73bcfa092210930239a52500ba767f7f7133ac006cd6ed03beaa4d3d591ef07375886c107c9cfa8486a0e6956ae358277884f0405cb").ToPositiveBigInteger(),
                    // q
                    new BitString("ff41059866667aebeb8bc5a9cbefc521242844a05afb681d792cb903").ToPositiveBigInteger(), 
                    // g
                    new BitString("225c342e0c6188fbbed166b27144244255d0e79e3e0a43a812602f4ebd8a471b47b4acbe8c365a40fd69dc269df9b64165d2a46c78b5a1e649c86ab4d06d863891d3f5c7f32acf88af48aa40f5d4afcbc0f1e9ae359ec861683b9232b98902da2daf52d03a271b8e92a13f6ea11a381ec55b4da10a83885f5ac65a49a91bbd9a1646753b18d7c5bafaf4e2148da3d9b718c8065716e0ce404ee0b159a3ae54ef7b0f1105a496e5018a415cb4c68b8598f32344796882d9ccd9e913590a5935f0588444769415166dbd27645f6b587b8837703946d8c118df781e765d4ca07d2ee9098b44b508715b624acf5fe07f7ab1fad0ad7e118abe457d530d1d13a8fce8").ToPositiveBigInteger()
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
                KeyAgreementMacType.HmacSha2D512,
                // key length
                112,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("fb879548c9e6626431bdeed2edb4cc2b"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("cf5b1cea8ebcffb91955f1689eb6a3759cf1aa0f44a3b5fe922ceb20").ToPositiveBigInteger(),
                // public static this party
                new BitString("2afb66165b5e59b77e57be6e4c47be34b11f5b7960c4ee99edc041cc60e330a0282ba4082b2af89e3542ff8437bbc411ee4889aa660675988b5f91d49d224546df09540624451b9252e716fac7b6e8d81b642de0decb4ef169e62b15bbf76ca207ef4d9b39fa2dd11a21b4f7eddd24651552ab5f1f2789a619cc10f0d261a9ca4d1a807e823ff8e3cc3c0eda904458a741a82ddec9d18c1acb3374a5aa0f46569344f9b36db30b2b3f4559e8d8c06cd894a4dafa5b6e2a220c038829f896d02d08f1c53042c7d74b70add23a9e16ec0fc400425f32e9ba25cfbbfe86a343b88688ff76e5673c24585f908ecd84c43b45ec98a642c349d58e39684ead88a7ff94").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("ca3bca4cbb74e3335132593e84f5ab5764813d478f31c1bcebf02977").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("62768930a173e8fa697f593b89b0519e082e8662c2a58f1a2669addebced98513de667d9820396d330fa0577cfd3652f9a3887fc449ed8ae44d70852b8a229ad06ad724d76fed90edd2cac1ee42aca4d128f8fb07cb67ba38baa1559ec046d38e67d1500243b49110c0d5d51e950a10d8e71a115c09bce2c33629e8b6fa13640d0436b93c749c79b893899bcd60127d71057567d9f8ae4dd8033055be45a7af27d4711cef54f443fcafda9763ec55491a7ea99717feca6d5d74d04aba0af02faf0396b2a8f375292f73550ef2871576e332b3a71867defce1d9fe05160576bfe546c91a78b51ca6e24b310da0317737dba4a7a502ac57b0a4772f908d18c4cff").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("9463e490b483aa1bb62321b85136ee536640c865966f73867a274886").ToPositiveBigInteger(),
                // public static other party
                new BitString("9b9b78c0a77e1fb5724f4df656542fddc8f2fae5c1fff9245318b637800536404f407779ccb9d559d8a4fdd96dea0698ccaeee70dd4b49bdb7c094d16ad01e5e7a23d48d9c294b775617229e5a666e7cefcf3f22852675c51c81b72693da5fe2370a820be11d2356613a79da7a4cb936e488e9687b5a81092b0e383915577963385a13433ed682a53fcfcde9d57f0bd65906538105164ed5ed885a576231d0b2c3471db7123672ffd97332c54f7b5075004ac940f5080294719820019cf597a57574e08f207d5399d5989abdfc9d96670ba7d677d3c5637c14b80d408d616136a71b97101fa41ff0a763a3556f15f6e616af9c80a0261fa7f4a1ae2fa172bb91").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("a1d0dd93fd9c53bf6401d840c4d73b625bdbf067e4810ad1ef10f5cda7926e52d922960981bd2c7b52e54426f9fdcf1ce6fc61c541e15471500868500bcfefdb571d3ad283733976f136d1778b7d76c4797722ccfe883aee017e298fdcdc05cd536d3d14bbb504ede829e20512b8077648037346afb6293d08e498d10d8f2d555690e3401fd38e827cb8f9d1c17d55f12564fb4ec3e5980287850060520988af90e462f90bc7291608a33c6c3395137fc0fc1e8feea7acedc60f9cab65c0a0f4cc4f71076ab446c57146d9f7beb2163c667b57d04862b60ba457417b3ff6b0a48cddf0d22ff472a38c72a8436933d44b0e13ae043947bf1dc2a6c4e4ff7b63221bc458e77fd4adec36b83a1f0bfbe3602e045d0436b9745a34ee5dd4307d0d34f70e9a8c8b0304fc13a7758b83098b91bfeb20b2ad6f816f59b4230bd136f6f54d2b93380964795c6fdec965a2c8de973069a72760b0cc673e14f78a967e3ab332ea8eec60303b031a1b3e5faf8eea2d025621ea132630b7179120585fccf80adfa7dabaa431f20780f8dcf29afaae63900841d8bb1d3b68a0e7eba7a4343a1b94ae7c1593d161d4747845249cf79fbe51d3af04095de1d1e25dacc7c81cc28cf3149f7cc6ccd2c6e27a3644d7880ed12e35638185af81504e863a199b53a9a42fb5b21f0ab898963829cb6faa14fa4c42bfff1c32466fea599a9edb9ce8a5cd"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964ae831b3f7d7372ff2bda2664b553b8ff3719c0"),
                // expected dkm
                new BitString("09d4a0b260f33660f464445eddbb"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765fb879548c9e6626431bdeed2edb4cc2b"),
                // expected tag
                new BitString("6832276eae40cb8c")
            },
            new object[]
            {
                // label
                "dhHybridOneFlow hmac512 sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("e41020ca42ffced66e039d39e5b3a14ee3c6d91feca227811a776ffd9c1b07024dea81fff33963be55a6790873e1ad2d74571dc8741e99c9f1ab8f41b8d0f9ee2c6909c63022c89166976d9bdcf4f3ea5b92e460356d4f21ae49eff275125685452cfdd104e6bbcffb10ce125bd9077987e595d39316ec46b12d86776d7fd11a08fbf5ba991e3e32490bbb86f21a8401be1d4e6d7f9c1181eacf24736b689d83db72066fa2c932916a21f850fc9c67c0014915ce02e4bc74edcb30034879bd836da8050169bdeb8d431ec73bcfa092210930239a52500ba767f7f7133ac006cd6ed03beaa4d3d591ef07375886c107c9cfa8486a0e6956ae358277884f0405cb").ToPositiveBigInteger(),
                    // q
                    new BitString("ff41059866667aebeb8bc5a9cbefc521242844a05afb681d792cb903").ToPositiveBigInteger(), 
                    // g
                    new BitString("225c342e0c6188fbbed166b27144244255d0e79e3e0a43a812602f4ebd8a471b47b4acbe8c365a40fd69dc269df9b64165d2a46c78b5a1e649c86ab4d06d863891d3f5c7f32acf88af48aa40f5d4afcbc0f1e9ae359ec861683b9232b98902da2daf52d03a271b8e92a13f6ea11a381ec55b4da10a83885f5ac65a49a91bbd9a1646753b18d7c5bafaf4e2148da3d9b718c8065716e0ce404ee0b159a3ae54ef7b0f1105a496e5018a415cb4c68b8598f32344796882d9ccd9e913590a5935f0588444769415166dbd27645f6b587b8837703946d8c118df781e765d4ca07d2ee9098b44b508715b624acf5fe07f7ab1fad0ad7e118abe457d530d1d13a8fce8").ToPositiveBigInteger()
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
                KeyAgreementMacType.HmacSha2D512,
                // key length
                112,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("fb879548c9e6626431bdeed2edb4cc2b"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("9463e490b483aa1bb62321b85136ee536640c865966f73867a274886").ToPositiveBigInteger(),
                // public static this party
                new BitString("9b9b78c0a77e1fb5724f4df656542fddc8f2fae5c1fff9245318b637800536404f407779ccb9d559d8a4fdd96dea0698ccaeee70dd4b49bdb7c094d16ad01e5e7a23d48d9c294b775617229e5a666e7cefcf3f22852675c51c81b72693da5fe2370a820be11d2356613a79da7a4cb936e488e9687b5a81092b0e383915577963385a13433ed682a53fcfcde9d57f0bd65906538105164ed5ed885a576231d0b2c3471db7123672ffd97332c54f7b5075004ac940f5080294719820019cf597a57574e08f207d5399d5989abdfc9d96670ba7d677d3c5637c14b80d408d616136a71b97101fa41ff0a763a3556f15f6e616af9c80a0261fa7f4a1ae2fa172bb91").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("cf5b1cea8ebcffb91955f1689eb6a3759cf1aa0f44a3b5fe922ceb20").ToPositiveBigInteger(),
                // public static other party
                new BitString("2afb66165b5e59b77e57be6e4c47be34b11f5b7960c4ee99edc041cc60e330a0282ba4082b2af89e3542ff8437bbc411ee4889aa660675988b5f91d49d224546df09540624451b9252e716fac7b6e8d81b642de0decb4ef169e62b15bbf76ca207ef4d9b39fa2dd11a21b4f7eddd24651552ab5f1f2789a619cc10f0d261a9ca4d1a807e823ff8e3cc3c0eda904458a741a82ddec9d18c1acb3374a5aa0f46569344f9b36db30b2b3f4559e8d8c06cd894a4dafa5b6e2a220c038829f896d02d08f1c53042c7d74b70add23a9e16ec0fc400425f32e9ba25cfbbfe86a343b88688ff76e5673c24585f908ecd84c43b45ec98a642c349d58e39684ead88a7ff94").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("ca3bca4cbb74e3335132593e84f5ab5764813d478f31c1bcebf02977").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("62768930a173e8fa697f593b89b0519e082e8662c2a58f1a2669addebced98513de667d9820396d330fa0577cfd3652f9a3887fc449ed8ae44d70852b8a229ad06ad724d76fed90edd2cac1ee42aca4d128f8fb07cb67ba38baa1559ec046d38e67d1500243b49110c0d5d51e950a10d8e71a115c09bce2c33629e8b6fa13640d0436b93c749c79b893899bcd60127d71057567d9f8ae4dd8033055be45a7af27d4711cef54f443fcafda9763ec55491a7ea99717feca6d5d74d04aba0af02faf0396b2a8f375292f73550ef2871576e332b3a71867defce1d9fe05160576bfe546c91a78b51ca6e24b310da0317737dba4a7a502ac57b0a4772f908d18c4cff").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("a1d0dd93fd9c53bf6401d840c4d73b625bdbf067e4810ad1ef10f5cda7926e52d922960981bd2c7b52e54426f9fdcf1ce6fc61c541e15471500868500bcfefdb571d3ad283733976f136d1778b7d76c4797722ccfe883aee017e298fdcdc05cd536d3d14bbb504ede829e20512b8077648037346afb6293d08e498d10d8f2d555690e3401fd38e827cb8f9d1c17d55f12564fb4ec3e5980287850060520988af90e462f90bc7291608a33c6c3395137fc0fc1e8feea7acedc60f9cab65c0a0f4cc4f71076ab446c57146d9f7beb2163c667b57d04862b60ba457417b3ff6b0a48cddf0d22ff472a38c72a8436933d44b0e13ae043947bf1dc2a6c4e4ff7b63221bc458e77fd4adec36b83a1f0bfbe3602e045d0436b9745a34ee5dd4307d0d34f70e9a8c8b0304fc13a7758b83098b91bfeb20b2ad6f816f59b4230bd136f6f54d2b93380964795c6fdec965a2c8de973069a72760b0cc673e14f78a967e3ab332ea8eec60303b031a1b3e5faf8eea2d025621ea132630b7179120585fccf80adfa7dabaa431f20780f8dcf29afaae63900841d8bb1d3b68a0e7eba7a4343a1b94ae7c1593d161d4747845249cf79fbe51d3af04095de1d1e25dacc7c81cc28cf3149f7cc6ccd2c6e27a3644d7880ed12e35638185af81504e863a199b53a9a42fb5b21f0ab898963829cb6faa14fa4c42bfff1c32466fea599a9edb9ce8a5cd"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964ae831b3f7d7372ff2bda2664b553b8ff3719c0"),
                // expected dkm
                new BitString("09d4a0b260f33660f464445eddbb"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765fb879548c9e6626431bdeed2edb4cc2b"),
                // expected tag
                new BitString("6832276eae40cb8c")
            },
            #endregion hmac

            #region ccm
            new object[]
            {
                // label
                "dhHybridOneFlow ccm sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("dd9d9d65da07fb08440dc70f5fb8244a21e180c0b401d4cb96ea144734631fc367faaea86fe34fea73c8da74b3d40e0bb4251fd8f3d854d54c31586b176c2ab3527e5c28e4233e6e256f5437f3146df163a0194a3edc583899aa0bbe0a7d87312b9bc7269b66b0f2c69d03b0b4976e95bca5ac28a492329579c88757dbcd505b15bf28aab412fc30eac366ac999329d4a14dde39358ad04b80f555bd08dccefdb30cf74f7db68e4941656efb0d000f0a01b0e2349a2b49e8aa04a718b76067cbb018ab1b39c9a690fbfd33073225585f89dd4204e7e472bb1f652ad7bb6d4d472d644bb6acf1a17af508a4d5746fa57d3999842828780405cb23c1d16ff75129").ToPositiveBigInteger(),
                    // q
                    new BitString("90a60b69890c0b7e7acc62d09faf04ce54fec05cbd23b9cce9a62ba3").ToPositiveBigInteger(), 
                    // g
                    new BitString("ad2c16013862e10ae46bf3a8feaf62ae79ab857b45aed0b27dc5d3e42946f7f89918687ed9634a70feffb5f42c1f5b9e4b38d9084116b7e718726ff68b77a77d977591ff3dd536e9d56fc186d475dc006deac4e29b6543217913ef18a569acb62fa94bf83048699b8625323c85693c43e0e3e96a74bb98598dca45638c63f86c20ca85803c6062752f6e5ef381fa15c0f055f17d06d55dd7ce0f9fe7b728e33744285dca171596782d30b520a873c6dae095abe94868eb1fcdc929537a31643b6f95f6e2a992970494026babc1027fe9443f35bc527adc9a695725f350385e7f387d14b3ba28d8da7a1fbd4cf9cf82494f31f1866a188fe94dee96c5cf71097a").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // key length
                128,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("109066d3eee9c4bf5b4cda0241cff30e"),
                // aes-ccm nonce
                new BitString("33fdde723cbb5e"),
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("8bbf0bf36ff45794afe284a198d094e31c196576c7f11869f67dd224").ToPositiveBigInteger(),
                // public static this party
                new BitString("45678db96cb7764c1cf88df77a232e487df66438766c3b930cc2dd1a3539ce9c6a65473bbc3ee8a7f653bc38c65033679bb36cdd8d26db2fdca0d15e35c6e059e2cc60b5631860e4dd907bc98340fa01bc8895da7a215c3765d63097923603b9da89ef55e8f452f9c9f3021f66dbf17afe8e32c8814ef5a6f2e1908a3f67cadf90f1a4a95751c29752e91ca8c39d0418b6a236159b2513237eb5cc53414689127a58017edc4d1706f23709b876834b5a28e23e840286494afdf1af445a446a22a08c9feef103e90dae2251af3d1e07fa17cfbd3ca6b58d9f5ca0a47780c0d3cd2c7273204d30cd3233363886193d6ebfbb67e4a567a8cfe0b9c738c63b76bfab").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("722b4dedc553af05446df5b80b7977cec92ef76b37cd1ea00458c131").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("9a5d71194f6433dceb94bc74ce512a626cb589f333d36fdce3eb4623313b62848df69c7f98706b17ee3ecd17db5e48d17c94b224cd08d9a3339ef82f357d6f784ada882ca9d0d5ed7b981608f25b7bc2fe12ea17c3c66a52ed4cbe04d02aaa1b240cfca28f1c6d5c0e4553836220b2faf33b262e95450bab93b74a268f4aeef21da78a07cc94fd09f52fc0a67340f551a2362b7daf92caec9e1ea2e399c6043dbc22006190c2ade2f02d54522deccf3ec283a6cc0184591c56f2731d40c9654ac746a671e3ece277f094c0fe91163ccc9f7e79f69623065aa25456714adf5591f7dcc2078570de150043142340d5fd5d4d80cdd2f455c65b9bad925a8197bf05").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("73f71509e55c7adc015d02dcec8600d08274a909887ee4e35305ed8d").ToPositiveBigInteger(),
                // public static other party
                new BitString("513363705e80f9166ff6f105117ae5a1c64c7e1478e2f8f83e74af2ccc9f03241d61d506ef47ea2e789d11a429708ccff298b55fd963b4cee8b68a80aaf6466f482943aa0aac4d0c0121e63105f12724d8f3d5d1cb6e7c4c71c6c9a516db11358716d4254b347a8094989533982da1731e96961880daddc5060da8d8dd8379ee5e1559f8318cf43f2ca0cfdbd3b4cefb1d4d6b9644a7615fafa2ed411e37280049d58ebcf7585e3ba5d369b16fe1f1a1ef7d80dcbae2fad5babf9ddfd27693a5aa527b47e9e5bb9a517e6642dea5c4db5e68f62fc4d060adb4d156a19ec3aa427fd7c3ffd22475ff4f3383bc622cde60a9269f871ea8342bcc01984a8c5bb4fa").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("21e867375764f91b1873567012abbfcf50d7ef38ec06de05a5bb63c99f79c1839b7acedb85783161bb37537208e99410c9517636168248c0174bc38557c3165ccd616ab01ad9e8d0e3e0fbe031989ec8d8cfc7e8370f755bff41087f3f1c9e5b0c551b3aa50872cc19f1d5aa37107ef4fb920186dc25b135a7caf47592d7877987a5448af73955fb1ea5690dc36f0032a0a9159eff416a9cd650d70a59bb4f900f0071258df40bfc38a13e796f06c29fd04b6d935c9cf8a486676fa01efd254c6c23ad69244beb19bdba93db393d8c77e1b86a8607ffb36db8464aff738fde4f8d5504ff3bfdf4ffdbaf55efd0624432c706939d704bf960afcb5170ec65ef2a9d43cb683057c38d4453d1cf79d676e37b4bb36b4983db97885df14d375cdc8b88909223114e2b2238a5cbbb3200b7b5f8320f921499624207bdc19010a6347d1dfb41c45fe196bdad6fc268c20b15ee93f2a68b62e00c7770e0a23e30b1e1c3ef0f90104493b1b6867d2cae4f56945d6242fa9f9400459db44b05d28f74a77da6ca8ab90089e19c9dd94af1ed36be81028f0509f3c7c50a755e63c4157e5f1c50169c0e215eaf3248d7b2fde1df10459870b9feaaf66de33ec290e107b0a84c1fa2ac85b74136d1282f803d5961341f9e1c202a611781e88865adc6abb2050d831b1a3e19fb88b650cb6753de711675c404baf42ac19ccae295bbd50584082a"),
                // expected oi
                new BitString("a1b2c3d4e54341565369648d86dd8341e68e42d111b8fd317e0a7b9ec5ce"),
                // expected dkm
                new BitString("06e38fc68da8a239a9d513a491a9203a"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765109066d3eee9c4bf5b4cda0241cff30e"),
                // expected tag
                new BitString("7ed51e20451d9a56")
            },
            new object[]
            {
                // label
                "dhHybridOneFlow ccm sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("dd9d9d65da07fb08440dc70f5fb8244a21e180c0b401d4cb96ea144734631fc367faaea86fe34fea73c8da74b3d40e0bb4251fd8f3d854d54c31586b176c2ab3527e5c28e4233e6e256f5437f3146df163a0194a3edc583899aa0bbe0a7d87312b9bc7269b66b0f2c69d03b0b4976e95bca5ac28a492329579c88757dbcd505b15bf28aab412fc30eac366ac999329d4a14dde39358ad04b80f555bd08dccefdb30cf74f7db68e4941656efb0d000f0a01b0e2349a2b49e8aa04a718b76067cbb018ab1b39c9a690fbfd33073225585f89dd4204e7e472bb1f652ad7bb6d4d472d644bb6acf1a17af508a4d5746fa57d3999842828780405cb23c1d16ff75129").ToPositiveBigInteger(),
                    // q
                    new BitString("90a60b69890c0b7e7acc62d09faf04ce54fec05cbd23b9cce9a62ba3").ToPositiveBigInteger(), 
                    // g
                    new BitString("ad2c16013862e10ae46bf3a8feaf62ae79ab857b45aed0b27dc5d3e42946f7f89918687ed9634a70feffb5f42c1f5b9e4b38d9084116b7e718726ff68b77a77d977591ff3dd536e9d56fc186d475dc006deac4e29b6543217913ef18a569acb62fa94bf83048699b8625323c85693c43e0e3e96a74bb98598dca45638c63f86c20ca85803c6062752f6e5ef381fa15c0f055f17d06d55dd7ce0f9fe7b728e33744285dca171596782d30b520a873c6dae095abe94868eb1fcdc929537a31643b6f95f6e2a992970494026babc1027fe9443f35bc527adc9a695725f350385e7f387d14b3ba28d8da7a1fbd4cf9cf82494f31f1866a188fe94dee96c5cf71097a").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // key length
                128,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("109066d3eee9c4bf5b4cda0241cff30e"),
                // aes-ccm nonce
                new BitString("33fdde723cbb5e"),
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("73f71509e55c7adc015d02dcec8600d08274a909887ee4e35305ed8d").ToPositiveBigInteger(),
                // public static this party
                new BitString("513363705e80f9166ff6f105117ae5a1c64c7e1478e2f8f83e74af2ccc9f03241d61d506ef47ea2e789d11a429708ccff298b55fd963b4cee8b68a80aaf6466f482943aa0aac4d0c0121e63105f12724d8f3d5d1cb6e7c4c71c6c9a516db11358716d4254b347a8094989533982da1731e96961880daddc5060da8d8dd8379ee5e1559f8318cf43f2ca0cfdbd3b4cefb1d4d6b9644a7615fafa2ed411e37280049d58ebcf7585e3ba5d369b16fe1f1a1ef7d80dcbae2fad5babf9ddfd27693a5aa527b47e9e5bb9a517e6642dea5c4db5e68f62fc4d060adb4d156a19ec3aa427fd7c3ffd22475ff4f3383bc622cde60a9269f871ea8342bcc01984a8c5bb4fa").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("8bbf0bf36ff45794afe284a198d094e31c196576c7f11869f67dd224").ToPositiveBigInteger(),
                // public static other party
                new BitString("45678db96cb7764c1cf88df77a232e487df66438766c3b930cc2dd1a3539ce9c6a65473bbc3ee8a7f653bc38c65033679bb36cdd8d26db2fdca0d15e35c6e059e2cc60b5631860e4dd907bc98340fa01bc8895da7a215c3765d63097923603b9da89ef55e8f452f9c9f3021f66dbf17afe8e32c8814ef5a6f2e1908a3f67cadf90f1a4a95751c29752e91ca8c39d0418b6a236159b2513237eb5cc53414689127a58017edc4d1706f23709b876834b5a28e23e840286494afdf1af445a446a22a08c9feef103e90dae2251af3d1e07fa17cfbd3ca6b58d9f5ca0a47780c0d3cd2c7273204d30cd3233363886193d6ebfbb67e4a567a8cfe0b9c738c63b76bfab").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("722b4dedc553af05446df5b80b7977cec92ef76b37cd1ea00458c131").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("9a5d71194f6433dceb94bc74ce512a626cb589f333d36fdce3eb4623313b62848df69c7f98706b17ee3ecd17db5e48d17c94b224cd08d9a3339ef82f357d6f784ada882ca9d0d5ed7b981608f25b7bc2fe12ea17c3c66a52ed4cbe04d02aaa1b240cfca28f1c6d5c0e4553836220b2faf33b262e95450bab93b74a268f4aeef21da78a07cc94fd09f52fc0a67340f551a2362b7daf92caec9e1ea2e399c6043dbc22006190c2ade2f02d54522deccf3ec283a6cc0184591c56f2731d40c9654ac746a671e3ece277f094c0fe91163ccc9f7e79f69623065aa25456714adf5591f7dcc2078570de150043142340d5fd5d4d80cdd2f455c65b9bad925a8197bf05").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("21e867375764f91b1873567012abbfcf50d7ef38ec06de05a5bb63c99f79c1839b7acedb85783161bb37537208e99410c9517636168248c0174bc38557c3165ccd616ab01ad9e8d0e3e0fbe031989ec8d8cfc7e8370f755bff41087f3f1c9e5b0c551b3aa50872cc19f1d5aa37107ef4fb920186dc25b135a7caf47592d7877987a5448af73955fb1ea5690dc36f0032a0a9159eff416a9cd650d70a59bb4f900f0071258df40bfc38a13e796f06c29fd04b6d935c9cf8a486676fa01efd254c6c23ad69244beb19bdba93db393d8c77e1b86a8607ffb36db8464aff738fde4f8d5504ff3bfdf4ffdbaf55efd0624432c706939d704bf960afcb5170ec65ef2a9d43cb683057c38d4453d1cf79d676e37b4bb36b4983db97885df14d375cdc8b88909223114e2b2238a5cbbb3200b7b5f8320f921499624207bdc19010a6347d1dfb41c45fe196bdad6fc268c20b15ee93f2a68b62e00c7770e0a23e30b1e1c3ef0f90104493b1b6867d2cae4f56945d6242fa9f9400459db44b05d28f74a77da6ca8ab90089e19c9dd94af1ed36be81028f0509f3c7c50a755e63c4157e5f1c50169c0e215eaf3248d7b2fde1df10459870b9feaaf66de33ec290e107b0a84c1fa2ac85b74136d1282f803d5961341f9e1c202a611781e88865adc6abb2050d831b1a3e19fb88b650cb6753de711675c404baf42ac19ccae295bbd50584082a"),
                // expected oi
                new BitString("a1b2c3d4e54341565369648d86dd8341e68e42d111b8fd317e0a7b9ec5ce"),
                // expected dkm
                new BitString("06e38fc68da8a239a9d513a491a9203a"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765109066d3eee9c4bf5b4cda0241cff30e"),
                // expected tag
                new BitString("7ed51e20451d9a56")
            },
            #endregion ccm
            #endregion dhHybridOneFlow

            #region mqv1
            #region hmac
            new object[]
            {
                // label
                "mqv1 hmac sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("a767083605e3ec0bca252ac9a2106dced20fe16eb3236cd79f8ff1d543e6969edf4aa06c432ae8504f5db67fa0f54acb8d6f36fe2d64a7ead26895a8e24606604ec0aff5f31327a2fec732c3ba4c3ddee6e50cc0318e2c7facb694f2d71a22ad97a500196e459c60d670280163bd5f11c6f80a67b56b20d87145e3b11015a7e2f090d81ca65fb97f3a2523fe3a47559651e4b03b599b363b9a78e62bf8ab67f657e545722c5372d8e22dc073abf009065032a02f3362981dbe402656856dbda4421fe5b14c9402820065f6c66d43edc97a581f5cd53748ba51c0f29779041c13b67bbe6077cb90a205cf20abd9b424cfc2e9cf788f52aa5c41206d015c321d43").ToPositiveBigInteger(),
                    // q
                    new BitString("b1b6763b3a80ff88d040211ffb88ff738fca5c695afab6d81256f633").ToPositiveBigInteger(), 
                    // g
                    new BitString("0c4799e69534512c79b1f8655883e3ad8d7cf19b607ba04a33ee68327e910d669df3c83b54ac08050ea5d4dfcc4b8caa4974730a39506f4056765123a992c65ba384be9ba2d661a9b5f05d518d7071f4de8267a396a144ffd71aecc94496ac5c9493cb02bd21b06deb8ecc9eb0a855a597229c5ada597e3bc74dcf53180929c0f8eea0e88790314c837ed5bf016bd0ba9543d0c2e65ea968b0df7171037e7df565192d6fd5f1f6db1da7cd7c5a79b3ef564f72629325d5ae56aa24331b77be766fb6c5921c82eec16341f67dd4e1fc1e14fe8810007e8473e2b5b43c730181871d23b5885bca77ada994c4e71e9f8f881c1c4229b0fb10577cd2b38aa4a4edbb").ToPositiveBigInteger()
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
                KeyAgreementMacType.HmacSha2D512,
                // key length
                112,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("189be28e34b19f92b3d784218cf95835"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("797aac4d1dfb3412af2ae85629fc3d0c1f225f048179bfcfd7156e06").ToPositiveBigInteger(),
                // public static this party
                new BitString("4fb5df11a95c19600f6e27714962064252bcdab35a9035918ff8d932ad239d72f881a8c3d3c5fb7de6a1afd78fe69bfc72d5c275d199734093a4c8b7019d250907edbc392ac2521a8f7a7eec8554a3426948cbf1cbc9e735d2e19d9414f8eabf456316df3e32f175eff9725e4542ecf427a1c046bbf8a8217192a241773480c8d33b251ba4281f155fd4ebaa6f5ac4e7e79416aa4ed6a79542c108a0b9325f493513ca6d3a9e996bc1a0cb8371fbf87990ac33de45f04fc07c7b39ac1de8c236865f6b396852201808ba67caec28a9eded889a3bf73007a593bdd005133698703f1941de46d0a93365a374e128cc6c28f15735b342995c0785e7e40d382fb6d9").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("04ff973d33f9c3df575e75a9f6c4e9d5fbf19e496ba5c83aa77b7afc").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("59f343fa6183210f05e3c339e95d94eeff87fcb26354c0ad6089b9ec8d4a79cd104c9395c4eb598374756d07af5695a8d5bdcae3ef111592f3b907d8d4bffd96ed947b398b9ca25865dde9f4c4dc54aa65ad51ad670755c53e97ebd8cd29070049f8cee4925bec3ed6e862852ae5d1e885aa403f5f097b4c3846c2eaaa9c484eed87f64ddc1cf03207ef080730995aba2b34b4b5543a6f434ae34d8055c687bacd9330151471118907be5c85a18f9106c5e2e9ad2e875778db55bb8db5c50049bab74dab0da9f457b6e9b210365e9acf5676cd9807305270f227359aa18bd356f741c7720e6ff9d9ea64b74bbc24e73b8adbacdd12392bb746351fb7874c5d14").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("079ae49c63c70851317ff4a646f162449572574c866597301c241f82").ToPositiveBigInteger(), 
                // public static other party
                new BitString("9026a630324255b5b495716da0afd1eab4645b651fc54998b2a89b117a1af58b8080fcb5fcacbe37c1ce61df0f28bf2bbdb1b4732144b491fce0cd36cff7d15891716dbca5c97f0cde5270835d73b8a7dba5176505868ac9a060ba458abd233487cfd163925aff2fe38ea07a40c94423b09c67a2b34815b53eed045cf39d70ed6a4ae95275c2102830d9cdd464753c4490e7d680c2ef47e64c7b4124038ade17b4e76a3cb95a4901de3edc728441c6c4157d5e08a5b178f294129c6cce52f910107206d65a7d20e4e11d3184fb44d49884cc79f1290f4f086348051ec8a0180d904f2e97b6117a27fa5ac7dd3123998fe37f27cd314524375780e334c22a409e").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("7c0afe78a0767607d3574f4d7c2fb4ce926c5c0fa444b5bdfa81aa54797f59f2e88fc1406e3745ebb5908fb1940db1346d135307aaee4a37e915b6c144a780650778479acef6dda2b57a125f9031c02eb050283654dcd6ac3d4256acdf22244d5e97d096b9d584d15588a0ff1f50ef8bcbb1c61caa0d8814e7c946598755ea2e3c635af371bbc336cb9a672677c7490068428cd7589584d1fa62968a73b431eae9f12ac3fd9e877017b8b132e2b88c733972720eaa9368e90e8d455b75ecad474ae289b5ce542ca7cf1b779141bce7969b44abedb1a4c403eb1f2a3c73d14e095c3c04b9456ce2b2536918a9c0e46b9b0b921a0dd3fef91e4d3fe76a1f564d62"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964acc52fc4b92d85568b602fcc0e6c963a14a34f"),
                // expected dkm
                new BitString("960c57ca0be8d23820d7b14b8eff"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765189be28e34b19f92b3d784218cf95835"),
                // expected tag
                new BitString("45f1109527ade600")
            },
            new object[]
            {
                // label
                "mqv1 hmac sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("a767083605e3ec0bca252ac9a2106dced20fe16eb3236cd79f8ff1d543e6969edf4aa06c432ae8504f5db67fa0f54acb8d6f36fe2d64a7ead26895a8e24606604ec0aff5f31327a2fec732c3ba4c3ddee6e50cc0318e2c7facb694f2d71a22ad97a500196e459c60d670280163bd5f11c6f80a67b56b20d87145e3b11015a7e2f090d81ca65fb97f3a2523fe3a47559651e4b03b599b363b9a78e62bf8ab67f657e545722c5372d8e22dc073abf009065032a02f3362981dbe402656856dbda4421fe5b14c9402820065f6c66d43edc97a581f5cd53748ba51c0f29779041c13b67bbe6077cb90a205cf20abd9b424cfc2e9cf788f52aa5c41206d015c321d43").ToPositiveBigInteger(),
                    // q
                    new BitString("b1b6763b3a80ff88d040211ffb88ff738fca5c695afab6d81256f633").ToPositiveBigInteger(), 
                    // g
                    new BitString("0c4799e69534512c79b1f8655883e3ad8d7cf19b607ba04a33ee68327e910d669df3c83b54ac08050ea5d4dfcc4b8caa4974730a39506f4056765123a992c65ba384be9ba2d661a9b5f05d518d7071f4de8267a396a144ffd71aecc94496ac5c9493cb02bd21b06deb8ecc9eb0a855a597229c5ada597e3bc74dcf53180929c0f8eea0e88790314c837ed5bf016bd0ba9543d0c2e65ea968b0df7171037e7df565192d6fd5f1f6db1da7cd7c5a79b3ef564f72629325d5ae56aa24331b77be766fb6c5921c82eec16341f67dd4e1fc1e14fe8810007e8473e2b5b43c730181871d23b5885bca77ada994c4e71e9f8f881c1c4229b0fb10577cd2b38aa4a4edbb").ToPositiveBigInteger()
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
                KeyAgreementMacType.HmacSha2D512,
                // key length
                112,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("189be28e34b19f92b3d784218cf95835"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("079ae49c63c70851317ff4a646f162449572574c866597301c241f82").ToPositiveBigInteger(),
                // public static this party
                new BitString("9026a630324255b5b495716da0afd1eab4645b651fc54998b2a89b117a1af58b8080fcb5fcacbe37c1ce61df0f28bf2bbdb1b4732144b491fce0cd36cff7d15891716dbca5c97f0cde5270835d73b8a7dba5176505868ac9a060ba458abd233487cfd163925aff2fe38ea07a40c94423b09c67a2b34815b53eed045cf39d70ed6a4ae95275c2102830d9cdd464753c4490e7d680c2ef47e64c7b4124038ade17b4e76a3cb95a4901de3edc728441c6c4157d5e08a5b178f294129c6cce52f910107206d65a7d20e4e11d3184fb44d49884cc79f1290f4f086348051ec8a0180d904f2e97b6117a27fa5ac7dd3123998fe37f27cd314524375780e334c22a409e").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("797aac4d1dfb3412af2ae85629fc3d0c1f225f048179bfcfd7156e06").ToPositiveBigInteger(), 
                // public static other party
                new BitString("4fb5df11a95c19600f6e27714962064252bcdab35a9035918ff8d932ad239d72f881a8c3d3c5fb7de6a1afd78fe69bfc72d5c275d199734093a4c8b7019d250907edbc392ac2521a8f7a7eec8554a3426948cbf1cbc9e735d2e19d9414f8eabf456316df3e32f175eff9725e4542ecf427a1c046bbf8a8217192a241773480c8d33b251ba4281f155fd4ebaa6f5ac4e7e79416aa4ed6a79542c108a0b9325f493513ca6d3a9e996bc1a0cb8371fbf87990ac33de45f04fc07c7b39ac1de8c236865f6b396852201808ba67caec28a9eded889a3bf73007a593bdd005133698703f1941de46d0a93365a374e128cc6c28f15735b342995c0785e7e40d382fb6d9").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("04ff973d33f9c3df575e75a9f6c4e9d5fbf19e496ba5c83aa77b7afc").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("59f343fa6183210f05e3c339e95d94eeff87fcb26354c0ad6089b9ec8d4a79cd104c9395c4eb598374756d07af5695a8d5bdcae3ef111592f3b907d8d4bffd96ed947b398b9ca25865dde9f4c4dc54aa65ad51ad670755c53e97ebd8cd29070049f8cee4925bec3ed6e862852ae5d1e885aa403f5f097b4c3846c2eaaa9c484eed87f64ddc1cf03207ef080730995aba2b34b4b5543a6f434ae34d8055c687bacd9330151471118907be5c85a18f9106c5e2e9ad2e875778db55bb8db5c50049bab74dab0da9f457b6e9b210365e9acf5676cd9807305270f227359aa18bd356f741c7720e6ff9d9ea64b74bbc24e73b8adbacdd12392bb746351fb7874c5d14").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("7c0afe78a0767607d3574f4d7c2fb4ce926c5c0fa444b5bdfa81aa54797f59f2e88fc1406e3745ebb5908fb1940db1346d135307aaee4a37e915b6c144a780650778479acef6dda2b57a125f9031c02eb050283654dcd6ac3d4256acdf22244d5e97d096b9d584d15588a0ff1f50ef8bcbb1c61caa0d8814e7c946598755ea2e3c635af371bbc336cb9a672677c7490068428cd7589584d1fa62968a73b431eae9f12ac3fd9e877017b8b132e2b88c733972720eaa9368e90e8d455b75ecad474ae289b5ce542ca7cf1b779141bce7969b44abedb1a4c403eb1f2a3c73d14e095c3c04b9456ce2b2536918a9c0e46b9b0b921a0dd3fef91e4d3fe76a1f564d62"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964acc52fc4b92d85568b602fcc0e6c963a14a34f"),
                // expected dkm
                new BitString("960c57ca0be8d23820d7b14b8eff"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765189be28e34b19f92b3d784218cf95835"),
                // expected tag
                new BitString("45f1109527ade600")
            },
            #endregion hmac
            #endregion mqv1

            #region dhOneFlow
            #region hmac
            new object[]
            {
                // label
                "dhOneFlow hmac512 sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("a1963a2b543053e8628829b49938238d19eae1aa65691d7542b06ff67d3b569845673ce7155eab6c14a85a9d807c862f3d93e5d18050d9ddbdde6dda688b6855d06e996adc0a4d82c6168198e00fec18f5c315cc322d15d3c36c35a7939a43cc52aa51b5ec31133fd40234ce57e4d811861efbad18b21067b920d176560df9cccbccfe3e4b0ac0c7a8a8592ede16b95b60c593fa52a0362444181f7a6f3a74986b7d6db8be8cab9d6ef1e4da370da10c8e7997764208091b97bb1b38bd31913b0f4961262766aa4348311eb628b7198bc6216fae7012687c9c1d8b24b876a9d770d4a56774564db6c3529f975e134f2556f3860e7d30ab7a529a35ad323cbafb").ToPositiveBigInteger(),
                    // q
                    new BitString("830c41756a0d6ef905cccc8a2b5a39f5f55eb1b579f1c92cc4caeb7f").ToPositiveBigInteger(), 
                    // g
                    new BitString("36c0321ca9e483519ad89c5b27f363bfd54bf986f2c775b4eb7bde17f0fd210f4abb3f7eee4902317ec172f02496007cd2bd915b6f6aa12e927f936bbdde5e36c25590b6aac8ad930a3874d7fb8ca5a2d8afd3a7a6dd23cfde422d434c15d24b5703b3bcb5ca0ee174c8e9773b910eb6329f2b351bdeda8a5627339409aa416382408db899a24fccae031d6611ccd5664d40e0760916a519394d98c2c0d44b68405aaedd2daaa3960c40366f82fd24377e881478253207510d2be96069b66e7f7ddd05696b38dee9ddabab0b5363dd9913471e58324620bad88546bae87afe5f9690221489816d4b664b1395922ebf5704d0637647b9646debba3737c7141066").ToPositiveBigInteger()
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
                KeyAgreementMacType.HmacSha2D512,
                // key length
                112,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("1875252f72792259d2fcf2210fc64746"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("00").ToPositiveBigInteger(),
                // public static this party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("3a8f3f184184133f5a57c97bf300704938d63855f194f687dd600b94").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("a178a998eb5e45126ffc400d2095248ad444e44cd05efe418ddb30fcb094a2e4b86d3498bd709be6b14aa69324a7d3895ca58480ce10c1d3c14b7f5ba7b899a3693c821fad22d7d035195b3d93a20fcaa6b51e02b5cea9356ff27debf4900497c2a94934605c21b3124395c1802449f6d3fc646fcdaeef59d901047ec11aa7e950f494ca1f4e16c625b0d5c4bdd7164914e8e0306115fd717c9205ea42556e02c51f9647e996f69eb72427fe977d2ad9f26249cdb91460dd70fa80765ce382e1f884cf8fae3714867d25a0332097f0615a6a3ff67900320f504051540d17ec84b5e6b3fc470bbb4461949c710cb8c5f2b23ce2fc9e9664beb6c97d003c8411b6").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("7f0b91cf9fae0267566e282e5b8cb0a686a3afe2fa87fbf0279a793d").ToPositiveBigInteger(), 
                // public static other party
                new BitString("983938e57f3551ad6e6e4f945b440c16b24e079cd8b3cbb0799ddbde87ce9100fb20d246a6b70df0ef7f1b4738c4d77ecdc1102c361ad2c79ffd0a33da7146a99fb5953bf9b95c598f00a4d14b5401983ca4abbb77330173bf4f081e7f9888b231bad459887fd042e6ec5da2b46c025a244a83854d0b7c9e7433c0f7a69ab968c3b4316f304d35c7104e874c965ab3c7bce40237e1f4af4f5302135858b735df225dbde0e88d182d7e777a2df366bc591987c26a598845690af4fc8eb08f226b338d3bdbc1a8ccc7eb1d87a4a22c8af4defa0da009b318f35d0062281f7725729ce59622804cd976480f059f0d276bb1c0c6881fa07418ee25dd2632b6a4c44f").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("5ffb6af52709ff0d191d270baf8225355a27f44bac4c5c7213d593a14588a6ebb9beb96808ed2eb96fca65801eb34acda442b6d703459db4fcd5094bcfc8345e582c30308d5e70f7fec8be46ed456adc5aab0bb4fe82ed2468e7cb1e94a2a771cf4d960c17d84a2bc5c8ed4562262d42cb6e8beee4cb1b42e2ded67c8d8edff8e51d96face8530f11d6b7e38ec649e482bdfd6b278361e999b7548b49e5c26cbfc2b09c861e9e5a307dcfbad904713c650f4fdc457739ec7d8e877d26dc8cbd487f19478d62c5f9e87adc57d5bf5024c1e45727a621009dc57d0ac1abce12d18fa805fc6bd6a0afb8ca2f8cfbf08830f35b0e9f8ffb57375f75c08e76637a312"),
                // expected oi
                new BitString("a1b2c3d4e54341565369645620635f0d5010e5199068de700daf0d14cea1"),
                // expected dkm
                new BitString("02fb1efed8fdd1ef94501a974f9b"),
                // expected macData
                new BitString("5374616e646172642054657374204d6573736167651875252f72792259d2fcf2210fc64746"),
                // expected tag
                new BitString("4b4c2707697f68e0")
            },
            new object[]
            {
                // label
                "dhOneFlow hmac512 sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("a1963a2b543053e8628829b49938238d19eae1aa65691d7542b06ff67d3b569845673ce7155eab6c14a85a9d807c862f3d93e5d18050d9ddbdde6dda688b6855d06e996adc0a4d82c6168198e00fec18f5c315cc322d15d3c36c35a7939a43cc52aa51b5ec31133fd40234ce57e4d811861efbad18b21067b920d176560df9cccbccfe3e4b0ac0c7a8a8592ede16b95b60c593fa52a0362444181f7a6f3a74986b7d6db8be8cab9d6ef1e4da370da10c8e7997764208091b97bb1b38bd31913b0f4961262766aa4348311eb628b7198bc6216fae7012687c9c1d8b24b876a9d770d4a56774564db6c3529f975e134f2556f3860e7d30ab7a529a35ad323cbafb").ToPositiveBigInteger(),
                    // q
                    new BitString("830c41756a0d6ef905cccc8a2b5a39f5f55eb1b579f1c92cc4caeb7f").ToPositiveBigInteger(), 
                    // g
                    new BitString("36c0321ca9e483519ad89c5b27f363bfd54bf986f2c775b4eb7bde17f0fd210f4abb3f7eee4902317ec172f02496007cd2bd915b6f6aa12e927f936bbdde5e36c25590b6aac8ad930a3874d7fb8ca5a2d8afd3a7a6dd23cfde422d434c15d24b5703b3bcb5ca0ee174c8e9773b910eb6329f2b351bdeda8a5627339409aa416382408db899a24fccae031d6611ccd5664d40e0760916a519394d98c2c0d44b68405aaedd2daaa3960c40366f82fd24377e881478253207510d2be96069b66e7f7ddd05696b38dee9ddabab0b5363dd9913471e58324620bad88546bae87afe5f9690221489816d4b664b1395922ebf5704d0637647b9646debba3737c7141066").ToPositiveBigInteger()
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
                KeyAgreementMacType.HmacSha2D512,
                // key length
                112,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("1875252f72792259d2fcf2210fc64746"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("7f0b91cf9fae0267566e282e5b8cb0a686a3afe2fa87fbf0279a793d").ToPositiveBigInteger(),
                // public static this party
                new BitString("983938e57f3551ad6e6e4f945b440c16b24e079cd8b3cbb0799ddbde87ce9100fb20d246a6b70df0ef7f1b4738c4d77ecdc1102c361ad2c79ffd0a33da7146a99fb5953bf9b95c598f00a4d14b5401983ca4abbb77330173bf4f081e7f9888b231bad459887fd042e6ec5da2b46c025a244a83854d0b7c9e7433c0f7a69ab968c3b4316f304d35c7104e874c965ab3c7bce40237e1f4af4f5302135858b735df225dbde0e88d182d7e777a2df366bc591987c26a598845690af4fc8eb08f226b338d3bdbc1a8ccc7eb1d87a4a22c8af4defa0da009b318f35d0062281f7725729ce59622804cd976480f059f0d276bb1c0c6881fa07418ee25dd2632b6a4c44f").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("00").ToPositiveBigInteger(), 
                // public static other party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("3a8f3f184184133f5a57c97bf300704938d63855f194f687dd600b94").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("a178a998eb5e45126ffc400d2095248ad444e44cd05efe418ddb30fcb094a2e4b86d3498bd709be6b14aa69324a7d3895ca58480ce10c1d3c14b7f5ba7b899a3693c821fad22d7d035195b3d93a20fcaa6b51e02b5cea9356ff27debf4900497c2a94934605c21b3124395c1802449f6d3fc646fcdaeef59d901047ec11aa7e950f494ca1f4e16c625b0d5c4bdd7164914e8e0306115fd717c9205ea42556e02c51f9647e996f69eb72427fe977d2ad9f26249cdb91460dd70fa80765ce382e1f884cf8fae3714867d25a0332097f0615a6a3ff67900320f504051540d17ec84b5e6b3fc470bbb4461949c710cb8c5f2b23ce2fc9e9664beb6c97d003c8411b6").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("5ffb6af52709ff0d191d270baf8225355a27f44bac4c5c7213d593a14588a6ebb9beb96808ed2eb96fca65801eb34acda442b6d703459db4fcd5094bcfc8345e582c30308d5e70f7fec8be46ed456adc5aab0bb4fe82ed2468e7cb1e94a2a771cf4d960c17d84a2bc5c8ed4562262d42cb6e8beee4cb1b42e2ded67c8d8edff8e51d96face8530f11d6b7e38ec649e482bdfd6b278361e999b7548b49e5c26cbfc2b09c861e9e5a307dcfbad904713c650f4fdc457739ec7d8e877d26dc8cbd487f19478d62c5f9e87adc57d5bf5024c1e45727a621009dc57d0ac1abce12d18fa805fc6bd6a0afb8ca2f8cfbf08830f35b0e9f8ffb57375f75c08e76637a312"),
                // expected oi
                new BitString("a1b2c3d4e54341565369645620635f0d5010e5199068de700daf0d14cea1"),
                // expected dkm
                new BitString("02fb1efed8fdd1ef94501a974f9b"),
                // expected macData
                new BitString("5374616e646172642054657374204d6573736167651875252f72792259d2fcf2210fc64746"),
                // expected tag
                new BitString("4b4c2707697f68e0")
            },
            #endregion hmac

            #region ccm
            new object[]
            {
                // label
                "dhOneFlow ccm sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("f49e99998348e680d01132863f32231f525797723515944b446e834a85cb0f51155cd59cfb994cebbab21ef18f9849243aef8d5a4df50b4fd72a5c8f812479fa3119afc63341e8af970fafe366f0d6dcf1e63e43323b331ecabc0dabae24d085e45d5e65e3ae3da751eafdf042fbbce6ca5c5c89f2555fc12f88292c031b873b15056d1982ee3d0a10525cd6c83d2c0beab2b7510f629b2ff01dbf1f1529edc71b91f9c48ec2dfc43958ce338ff80dad787a249a99498b8fe3979603a558108cd45b66ae6ba5d657e7fd28862a9211d2252fa3cdbab7bd0702cc50bac1350781761174f3caa1218fc12c59fab986ec08e954de332a686c2b89850944ca384c33").ToPositiveBigInteger(),
                    // q
                    new BitString("be9fe3349b097a34e04379af53981112cd97d473b611744230e85739").ToPositiveBigInteger(), 
                    // g
                    new BitString("eb87d29bdb8d25f4293f66372d066ab6ca8f5f0e35671b26889a66b9c442de3fd2fc4fac6adbedcb5d45e67efcbf592c8bd9134443a4fc81aef7c51a88c4c30a643ab2bce7866fe71bf5561e428fced3c09f877a9d17719b7bfbcd44e3b25f0ccbd1deced7e677e92a084a73a5ebf6d3794f9326d19beaea0cd21a9837bc50fbd1835fe5805c72381c736dbab67a693a31e0a37334821cf570acfc7c490ba149a5cf99c2abaee1ee8734258d1b672912d1a32c90e84643dcc170c462ec0c737e5ccf03561e476a5050e5f1a895648da295f1f55c546eb7b5210be35d09da02eb353da5f76d9dac60db15d81dfb0af14e2d16f5b602d6695dd38f90eb820044f1").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // key length
                128,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("08a2a985ce4907b3abbdca4b7fabfcff"),
                // aes-ccm nonce
                new BitString("72c72eb7119212"),
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("00").ToPositiveBigInteger(),
                // public static this party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("58537d11be06cf88aed2818b9b89767093bb1585246b241f3b2874cc").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("57a11add2a5bfe77bcf5503d4d6c7acf618a64eae420d35d6102900132d76769a142aa4c8cb31a63bf26fcab73224628d4d85446dde7ce1420199d5aa6344610e56b5b65f3246c5070faf9f1d61afb9ea2ae90ed4d96cf9cf7957909c7516042485f436fb32310ea662a647e089c3dc4242e032e203e92c9fd2d9f40df110ad783a16623326c2e36f5ac4ca19c37f9cde00dec3f22990214329828a80a19e796492f30adcaa030cf608ebc81b3726540613fda275014ce597fb251a9b2bc9dda6304cc3fb086c895041d57e1d4c6d85e969bcc88f23bacc1b8fb26e8b5a467c2e943340a46637da26987115d559bb062fefc565db1b3c25d540d4c0671e446da").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("8c93691922dc9790274a00a2d02326242e61ed76bff377d82e2eaeec").ToPositiveBigInteger(), 
                // public static other party
                new BitString("22f13126c707073e2e404195c007ddec4d0dd0759a7fb1616cbe4e5c569f957ba8dc036c260966d18c5b7e9009b5d5df5bd51fc43999d17dc2460f77a251d8fbf83afdf93a941da0ed863a04da9f7e35541ccc96e8728e02d8ac28cf49412f67b2ecca1c73ff91f05a95dd76a811870b662ef996d8c660cb42f9cabd0630dd883c3faa6f38367dc78163c7d7fc85dfa353f58a3a60f07818d0ae0455ecc9263e2354b728c95ea57734afec1ab20ed72458d63c90d296e304237bd8e825a8eecc7cced61462a32170a4f2d14b3ae902e2da182a27d1f04875100a5dccacdf246b485c22ea15d5ada8ddc03885a1d0ae11d489fcc8dcea0db970113479703cab62").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("1803d2578c0ae67b9c8a445ceac100a695a2bb409c9dcdcaffadfe5645def641f33c7c0cd9417ad0eb34b22092f5a66e6b6c0d8b97c2ad7e32b0678a071ff16566a894bf70012e578966c9225cab3128c2c8e04e684a8293f6bc61942d80e62b4b77895a409605ecfb16250fd32aebb88ffcf9a4eb93f3f3b51bb5505e6c7314b76ae5d3653eac5ed1216e002318a9f8eb34a91711733f766666038525671f1b232e9e5723e4ce1c5c8f23637f91f5fac6755337b50f3043e553a25eb79a7e6667194da7ab44af26c23887c8535cdbb3afe2be3c64ae96bb45bfd324c7bb9ffbce4ada90f630e9c4b5a4b6c3d08a87b361f84f87cc509bf59bda9708e72c5930"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964000b44c045788f69ebe56ab0d04a3f64551317"),
                // expected dkm
                new BitString("5984b62d16779dc531e3dfac51cc9501"),
                // expected macData
                new BitString("5374616e646172642054657374204d65737361676508a2a985ce4907b3abbdca4b7fabfcff"),
                // expected tag
                new BitString("5c0ac648799f277f")
            },
            new object[]
            {
                // label
                "dhOneFlow ccm sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("f49e99998348e680d01132863f32231f525797723515944b446e834a85cb0f51155cd59cfb994cebbab21ef18f9849243aef8d5a4df50b4fd72a5c8f812479fa3119afc63341e8af970fafe366f0d6dcf1e63e43323b331ecabc0dabae24d085e45d5e65e3ae3da751eafdf042fbbce6ca5c5c89f2555fc12f88292c031b873b15056d1982ee3d0a10525cd6c83d2c0beab2b7510f629b2ff01dbf1f1529edc71b91f9c48ec2dfc43958ce338ff80dad787a249a99498b8fe3979603a558108cd45b66ae6ba5d657e7fd28862a9211d2252fa3cdbab7bd0702cc50bac1350781761174f3caa1218fc12c59fab986ec08e954de332a686c2b89850944ca384c33").ToPositiveBigInteger(),
                    // q
                    new BitString("be9fe3349b097a34e04379af53981112cd97d473b611744230e85739").ToPositiveBigInteger(), 
                    // g
                    new BitString("eb87d29bdb8d25f4293f66372d066ab6ca8f5f0e35671b26889a66b9c442de3fd2fc4fac6adbedcb5d45e67efcbf592c8bd9134443a4fc81aef7c51a88c4c30a643ab2bce7866fe71bf5561e428fced3c09f877a9d17719b7bfbcd44e3b25f0ccbd1deced7e677e92a084a73a5ebf6d3794f9326d19beaea0cd21a9837bc50fbd1835fe5805c72381c736dbab67a693a31e0a37334821cf570acfc7c490ba149a5cf99c2abaee1ee8734258d1b672912d1a32c90e84643dcc170c462ec0c737e5ccf03561e476a5050e5f1a895648da295f1f55c546eb7b5210be35d09da02eb353da5f76d9dac60db15d81dfb0af14e2d16f5b602d6695dd38f90eb820044f1").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // key length
                128,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("08a2a985ce4907b3abbdca4b7fabfcff"),
                // aes-ccm nonce
                new BitString("72c72eb7119212"),
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("8c93691922dc9790274a00a2d02326242e61ed76bff377d82e2eaeec").ToPositiveBigInteger(),
                // public static this party
                new BitString("22f13126c707073e2e404195c007ddec4d0dd0759a7fb1616cbe4e5c569f957ba8dc036c260966d18c5b7e9009b5d5df5bd51fc43999d17dc2460f77a251d8fbf83afdf93a941da0ed863a04da9f7e35541ccc96e8728e02d8ac28cf49412f67b2ecca1c73ff91f05a95dd76a811870b662ef996d8c660cb42f9cabd0630dd883c3faa6f38367dc78163c7d7fc85dfa353f58a3a60f07818d0ae0455ecc9263e2354b728c95ea57734afec1ab20ed72458d63c90d296e304237bd8e825a8eecc7cced61462a32170a4f2d14b3ae902e2da182a27d1f04875100a5dccacdf246b485c22ea15d5ada8ddc03885a1d0ae11d489fcc8dcea0db970113479703cab62").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("00").ToPositiveBigInteger(), 
                // public static other party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("58537d11be06cf88aed2818b9b89767093bb1585246b241f3b2874cc").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("57a11add2a5bfe77bcf5503d4d6c7acf618a64eae420d35d6102900132d76769a142aa4c8cb31a63bf26fcab73224628d4d85446dde7ce1420199d5aa6344610e56b5b65f3246c5070faf9f1d61afb9ea2ae90ed4d96cf9cf7957909c7516042485f436fb32310ea662a647e089c3dc4242e032e203e92c9fd2d9f40df110ad783a16623326c2e36f5ac4ca19c37f9cde00dec3f22990214329828a80a19e796492f30adcaa030cf608ebc81b3726540613fda275014ce597fb251a9b2bc9dda6304cc3fb086c895041d57e1d4c6d85e969bcc88f23bacc1b8fb26e8b5a467c2e943340a46637da26987115d559bb062fefc565db1b3c25d540d4c0671e446da").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("1803d2578c0ae67b9c8a445ceac100a695a2bb409c9dcdcaffadfe5645def641f33c7c0cd9417ad0eb34b22092f5a66e6b6c0d8b97c2ad7e32b0678a071ff16566a894bf70012e578966c9225cab3128c2c8e04e684a8293f6bc61942d80e62b4b77895a409605ecfb16250fd32aebb88ffcf9a4eb93f3f3b51bb5505e6c7314b76ae5d3653eac5ed1216e002318a9f8eb34a91711733f766666038525671f1b232e9e5723e4ce1c5c8f23637f91f5fac6755337b50f3043e553a25eb79a7e6667194da7ab44af26c23887c8535cdbb3afe2be3c64ae96bb45bfd324c7bb9ffbce4ada90f630e9c4b5a4b6c3d08a87b361f84f87cc509bf59bda9708e72c5930"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964000b44c045788f69ebe56ab0d04a3f64551317"),
                // expected dkm
                new BitString("5984b62d16779dc531e3dfac51cc9501"),
                // expected macData
                new BitString("5374616e646172642054657374204d65737361676508a2a985ce4907b3abbdca4b7fabfcff"),
                // expected tag
                new BitString("5c0ac648799f277f")
            },
            #endregion ccm
            #endregion dhOneFlow

            #region dhStatic
            #region hmac
            new object[]
            {
                // label
                "dhStatic hmac sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("81f15dff1873890c3fdc70ec6615c2c19c1c95cf7f5731669c7a6b42ca6c2bae4369518538c07cb4aa3601eb628161549be2dfe4f69add71c9f1c6340cc514cda1f906883c3a3e55a2840e4bdf2cf81b040fb9bfe4cd597f85ebae1f0b4114532e43fbc5b0e4e958d8ddddf29de886157d85dc401ae96cf9538d5adc53e0d603aa127fa030712fe5e779d31c39bfa892030cddcc50988fbeb8d02ebb904d4401e234d5f3bc50c8dff1e445995edc3417b253f942498e481125f60e48f4eb226774f1b6d666423eaceb066c9edf9284fa0469c1709017f20c8f50df14c5b0b2d8a763f73d7dfcd2dc9da7688aa2fdef3cd9c31b0f6b0c21daf4ceaef28789bfeb").ToPositiveBigInteger(),
                    // q
                    new BitString("cc2c1d163a3e0aea348825e0689af896133dc401a823f59c121e7af3").ToPositiveBigInteger(), 
                    // g
                    new BitString("5941d1178d593f2f404f8f1b1b162cebbaa787d55666edecbd5071dece793d4de1197a19c993ff98d10a5f1c99976077cde0739953db74d50b6fbd6c864d1917085c81f89e0c2a0fa7186992da10fa0bd52494a97a6b36ad6291872a9d1663cf710426f858b23cb510a2494e6fa17ca18ac735890e44a7c579861e3ad778071b15bb2ab8b75e1ca0507e22775ae38290dfed0fcf5d4f0df5fe40ab10967a4dbcdc118aad51dfaf675a274ff4c54652b5ca6da913c141cb158242203f48fae4d78cfd5b94b180e84585d45c7e692d83369c8dae07fb6308a05164ac1854743bc7764e74aca434a40f7f063a04d0f95b501e02045341234658d9c657048b200c3b").ToPositiveBigInteger()
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
                KeyAgreementMacType.HmacSha2D512,
                // key length
                112,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("345e02df604063e4160b7c6d3b9efd7e"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("4e0f5416ee270385fc2244bdf3405902b505362df56d39378f74b326").ToPositiveBigInteger(),
                // public static this party
                new BitString("7451802fe814a0f0dacc6b92d6d356b234a686c3c5c50b0abdb75f252462bd18b06794c073838d134b8d930fc9372caf28c03d8bbb789bb918a692d5afd2b24390e4a55781b500f80ba4f14ee3b261ee9b57b042d3ededd932b2232c5f8ffc3c767aa38b95e847fbc1c9eb6dfca6de98f92b22afb1f260ccae2ed45ea054382c69ca98d3607e8e25281f1736038b5ddeff19a1924700e53633a4d2f6bdd262a9c6fb17a9930d8d20cd011c0a7769b5b80c60a64db4e6c30035f273d9a1bbd4aae661620415b4a0449d0e192b98931a8d10f8b6baa3cc9699869471b92556d305f21c29749ae8ff854a669de10ae3919e8e9d3cbc9ba0b96fa0319314ebba7956").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                new BitString("5b147b3ffcdc63076793989b4f30"),
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("8dfcf6f49c0f7e9f34c62c3dadd82a6e7f9f7800598bbcd030d14c2f").ToPositiveBigInteger(),
                // public static other party
                new BitString("3b071528d852ee6cc08ea58d22e84f0bd416104be2c1278f0ad9139dafcce67c372ee52da9103aeb7dc53c433b9d86a5b10aafe186fa2de8efd444d30b850f310ffb3cdbbe2cb5e7944370c7b02dd962d88521519982dcf49e7921e0f766ad9b2eacfe6ecde37060d0cefed7462be640effa88bf1ad45e2874e22142cae052d7aff505e77d47a178c84893fe92361fcdb73aee9700206c65179fde46de15bcaf56a1b7c5348d0f7880578a15372ebb8c335d82f022e86412b979a137acb69ccd1f42dae383c7e8326b3609b236e7c976a74f56a7d8e1a542734fdb83c733c6fbbf66d3ce44a5ec773624088b4895f0a648aaac37b897f67af894855c8f1240c6").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("05aae6798a2bcb6985e5c774765b4265d23c1415a5569f56ff1ac8c659afbbdf1041f636c0c7c244ccd45ea46f9ceae79e3474514911cc836addb3a8196bf76eee20f8b70229c8fcc5dd20629898e8dca24209908a647efb892d2f056dd7906d2f1c34b673968121918b86b3a8111a61f43e09979231b6bc7f482cd262a5d379e0099f3836fca48395effe7b1cca62a770bbdab52cc91bdddcdba95fdb539ab3104276c2eb6ae6d366af969e447643ddf66ed7376b6aea618a65b9c7aaf7985e7ed690c4bce1fd3fd72dbd5d99d524c7cd575fd80e80b683259f9931c3cf064a67ff3cbda2e8cb1461d4fcd0a2377251d773a6f3521ca1e5b65cee71f4c67b92"),
                // expected oi
                new BitString("a1b2c3d4e55b147b3ffcdc63076793989b4f304341565369649f294b94da"),
                // expected dkm
                new BitString("293fb6866bf5009262b7b4c4e002"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765345e02df604063e4160b7c6d3b9efd7e"),
                // expected tag
                new BitString("b01e09be90315273")
            },
            new object[]
            {
                // label
                "dhStatic hmac sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("81f15dff1873890c3fdc70ec6615c2c19c1c95cf7f5731669c7a6b42ca6c2bae4369518538c07cb4aa3601eb628161549be2dfe4f69add71c9f1c6340cc514cda1f906883c3a3e55a2840e4bdf2cf81b040fb9bfe4cd597f85ebae1f0b4114532e43fbc5b0e4e958d8ddddf29de886157d85dc401ae96cf9538d5adc53e0d603aa127fa030712fe5e779d31c39bfa892030cddcc50988fbeb8d02ebb904d4401e234d5f3bc50c8dff1e445995edc3417b253f942498e481125f60e48f4eb226774f1b6d666423eaceb066c9edf9284fa0469c1709017f20c8f50df14c5b0b2d8a763f73d7dfcd2dc9da7688aa2fdef3cd9c31b0f6b0c21daf4ceaef28789bfeb").ToPositiveBigInteger(),
                    // q
                    new BitString("cc2c1d163a3e0aea348825e0689af896133dc401a823f59c121e7af3").ToPositiveBigInteger(), 
                    // g
                    new BitString("5941d1178d593f2f404f8f1b1b162cebbaa787d55666edecbd5071dece793d4de1197a19c993ff98d10a5f1c99976077cde0739953db74d50b6fbd6c864d1917085c81f89e0c2a0fa7186992da10fa0bd52494a97a6b36ad6291872a9d1663cf710426f858b23cb510a2494e6fa17ca18ac735890e44a7c579861e3ad778071b15bb2ab8b75e1ca0507e22775ae38290dfed0fcf5d4f0df5fe40ab10967a4dbcdc118aad51dfaf675a274ff4c54652b5ca6da913c141cb158242203f48fae4d78cfd5b94b180e84585d45c7e692d83369c8dae07fb6308a05164ac1854743bc7764e74aca434a40f7f063a04d0f95b501e02045341234658d9c657048b200c3b").ToPositiveBigInteger()
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
                KeyAgreementMacType.HmacSha2D512,
                // key length
                112,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("345e02df604063e4160b7c6d3b9efd7e"),
                // aes-ccm nonce
                null,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("8dfcf6f49c0f7e9f34c62c3dadd82a6e7f9f7800598bbcd030d14c2f").ToPositiveBigInteger(),
                // public static this party
                new BitString("3b071528d852ee6cc08ea58d22e84f0bd416104be2c1278f0ad9139dafcce67c372ee52da9103aeb7dc53c433b9d86a5b10aafe186fa2de8efd444d30b850f310ffb3cdbbe2cb5e7944370c7b02dd962d88521519982dcf49e7921e0f766ad9b2eacfe6ecde37060d0cefed7462be640effa88bf1ad45e2874e22142cae052d7aff505e77d47a178c84893fe92361fcdb73aee9700206c65179fde46de15bcaf56a1b7c5348d0f7880578a15372ebb8c335d82f022e86412b979a137acb69ccd1f42dae383c7e8326b3609b236e7c976a74f56a7d8e1a542734fdb83c733c6fbbf66d3ce44a5ec773624088b4895f0a648aaac37b897f67af894855c8f1240c6").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("4e0f5416ee270385fc2244bdf3405902b505362df56d39378f74b326").ToPositiveBigInteger(),
                // public static other party
                new BitString("7451802fe814a0f0dacc6b92d6d356b234a686c3c5c50b0abdb75f252462bd18b06794c073838d134b8d930fc9372caf28c03d8bbb789bb918a692d5afd2b24390e4a55781b500f80ba4f14ee3b261ee9b57b042d3ededd932b2232c5f8ffc3c767aa38b95e847fbc1c9eb6dfca6de98f92b22afb1f260ccae2ed45ea054382c69ca98d3607e8e25281f1736038b5ddeff19a1924700e53633a4d2f6bdd262a9c6fb17a9930d8d20cd011c0a7769b5b80c60a64db4e6c30035f273d9a1bbd4aae661620415b4a0449d0e192b98931a8d10f8b6baa3cc9699869471b92556d305f21c29749ae8ff854a669de10ae3919e8e9d3cbc9ba0b96fa0319314ebba7956").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                new BitString("5b147b3ffcdc63076793989b4f30"),
                // expected Z
                new BitString("05aae6798a2bcb6985e5c774765b4265d23c1415a5569f56ff1ac8c659afbbdf1041f636c0c7c244ccd45ea46f9ceae79e3474514911cc836addb3a8196bf76eee20f8b70229c8fcc5dd20629898e8dca24209908a647efb892d2f056dd7906d2f1c34b673968121918b86b3a8111a61f43e09979231b6bc7f482cd262a5d379e0099f3836fca48395effe7b1cca62a770bbdab52cc91bdddcdba95fdb539ab3104276c2eb6ae6d366af969e447643ddf66ed7376b6aea618a65b9c7aaf7985e7ed690c4bce1fd3fd72dbd5d99d524c7cd575fd80e80b683259f9931c3cf064a67ff3cbda2e8cb1461d4fcd0a2377251d773a6f3521ca1e5b65cee71f4c67b92"),
                // expected oi
                new BitString("a1b2c3d4e55b147b3ffcdc63076793989b4f304341565369649f294b94da"),
                // expected dkm
                new BitString("293fb6866bf5009262b7b4c4e002"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765345e02df604063e4160b7c6d3b9efd7e"),
                // expected tag
                new BitString("b01e09be90315273")
            },
            #endregion hmac
            
            #region ccm
            new object[]
            {
                // label
                "dhStatic ccm sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("f20f2553eb14c2fb9581912a814c3440d2f09fd469f38947d136c9aa804ec64848ed2934390a770e560f4525c47aa3b7901c9f6d5e6c473eb5dfa48183db5203634e135c740480fac71c1816476a90464265391bcffd6bd0af3146a82f31dc48d7a710cb4fdaa4f2c7cc3c3ded1a97a743f22df1d8d1712ad51ab624ccb39aa0f09a6c4094ddb456b2c5b0273ae7ffa18b7ef287cf437dbf0626bf6677902c42e8b3bf5d83e482db7e4f76b33edc042ee2a0ba5ac845825f81550cca3b80f579108d9c4bf8eb64bcf08f0fc714a5a68221e7ee6b6c2e91782d614692118727175b35087466a6fd77aa49067b97964f275fd0e8a775bc1c2de3853800bf2757e9").ToPositiveBigInteger(),
                    // q
                    new BitString("add6bcc7af25dbbfb8ab659623a4163c8291b69bd8fd83916163db09").ToPositiveBigInteger(), 
                    // g
                    new BitString("137986b494e520581bf3c0f40aa4287c505cdca1499c6659ec412231ff88bf3182e6e73945ef1b401d18ca03779d7c68bc5168064d4aa7077ba3d41106f957bdf43fcaf5a4fb3e220af8753c135ff0c3b1652f5f9c800edfe42563badc355862eeceb9b504ac6b245f00c1857070f34642c4b8ab45d13b723b12063146f29e81c2516c675ecc43aca92489626d4350b1ea208c47e48b44b13b26c93cd2f4744bcadf591aa9e9784f76e25ae38206237cbad33f577782ae2a56f7407eac94243f9997b7c8b3967571b989b6cdc364335a93b31543fa956ee7654c6f05ca1cecab9d1cc340e269a0ee7ca042687ccd5d3155df154cd3e67fad4c0a3f8ac0e9b3b3").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // key length
                128,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("ed7b8e50881ba27d0bb6a33dbb0f9e7e"),
                // aes-ccm nonce
                new BitString("42ba55af92c975"),
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("66207fd6fd0f05ffd08ec4b869af6831fd2684ff7be995a9ce3bd279").ToPositiveBigInteger(),
                // public static this party
                new BitString("1261cc852da6a2fb4528462e8ed467a246a50a398d532b013791fc3c373fe6782103af9533cf82fede7404138fc97829595d92162efb0dd8eccd036ec3d68ac27b3e445c8ce0f60db4ebe5094c4bb20644f62bca7d10953dab9a7f668961762e8e034f65f1e2a1a435c16613a8aae0e47432aef4faec19b60c3cc20bfdd0b17bc835df5d8c27d708f6645bd1328c49b74b89c335f1daa0afe7d85fc448d0737036fce056adcae8bcd4488b9ad511e442058d6eef4336b3230b8da7ec497f95b8bd018c241155ec79dede3592aad083c7efa3c542e38324c017db2707ecce6242414463344ec73a2784437c789576b9fc9a5593aa365dfc9fc37e2563029c8e13").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                new BitString("60a8d3fa7d3d957315adde0d2a01"),
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("2d6d09d360cf277a1a749b03677a49ba054e8a4c4b77d8f66a537a89").ToPositiveBigInteger(),
                // public static other party
                new BitString("d0fe5b52c7488f11983d69201b3ceeb559fcb2b1fa98b8b70253dba1e6afdc21f8eba2e7e87f3abbe361a0b6ec97467685f5214b586f0b19c616af39a331bd8c201a6c6f31d8035dd3a94e39b9690040979938363a4c3f8fb2d268025f43a648e04476bf31cf62a36787baa5f8be38944781132d7e989536206c4acc3485d7e9dd878eca978ef48496b1a6c675cd86a436b94135643db088dda98d88ec92c137f16f169a39b27fd85e6136890b3a42b3f2356bd31dc0ba30654ae68a7fc729a112fa73f0f14a6a770e18eb7c40cf609e3d08dd27f35bdc3ebd1c46623398170d7d799800ee43ab8ed00824def52e602fd22c88f0a44dafded16e800e8a54159c").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // expected Z
                new BitString("aae94826523986ec6b460c9b7e5b82666e941870f61505776091fd1af8bdeaec7789dc2227a06b3e40e698115da4a85ad721ddb27891755e321dfaef95ece9b95503a73b5fab771552b9bcebccffbb13dfd682fcd62ac64d8f8bed05473d1bf19a94e88faeb8f07d4135076314ec9ea3cd95ebb071919016dab65598ed7f6b0afe023adfd526ea1128eb375efe496a3de0d93454f35d1c405f34be8a3e1d33f743e76802b758db635e9ac88f09eaf41f03c7481f2897c8c2083a9a90c86cc02de86ed3d85e4be788dca7dfa2b526a6e826cc9dfe3fad5fb2f83f483af92cece3ab62f89f5abb5869a86fed5ab37c24961714f59a59e6886c671b8badf3127fd5"),
                // expected oi
                new BitString("a1b2c3d4e560a8d3fa7d3d957315adde0d2a01434156536964600cf36e99"),
                // expected dkm
                new BitString("23e83fdc4f0e4d4752247ab50fad2e20"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765ed7b8e50881ba27d0bb6a33dbb0f9e7e"),
                // expected tag
                new BitString("377a428156d37e71")
            },
            new object[]
            {
                // label
                "dhStatic ccm sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("f20f2553eb14c2fb9581912a814c3440d2f09fd469f38947d136c9aa804ec64848ed2934390a770e560f4525c47aa3b7901c9f6d5e6c473eb5dfa48183db5203634e135c740480fac71c1816476a90464265391bcffd6bd0af3146a82f31dc48d7a710cb4fdaa4f2c7cc3c3ded1a97a743f22df1d8d1712ad51ab624ccb39aa0f09a6c4094ddb456b2c5b0273ae7ffa18b7ef287cf437dbf0626bf6677902c42e8b3bf5d83e482db7e4f76b33edc042ee2a0ba5ac845825f81550cca3b80f579108d9c4bf8eb64bcf08f0fc714a5a68221e7ee6b6c2e91782d614692118727175b35087466a6fd77aa49067b97964f275fd0e8a775bc1c2de3853800bf2757e9").ToPositiveBigInteger(),
                    // q
                    new BitString("add6bcc7af25dbbfb8ab659623a4163c8291b69bd8fd83916163db09").ToPositiveBigInteger(), 
                    // g
                    new BitString("137986b494e520581bf3c0f40aa4287c505cdca1499c6659ec412231ff88bf3182e6e73945ef1b401d18ca03779d7c68bc5168064d4aa7077ba3d41106f957bdf43fcaf5a4fb3e220af8753c135ff0c3b1652f5f9c800edfe42563badc355862eeceb9b504ac6b245f00c1857070f34642c4b8ab45d13b723b12063146f29e81c2516c675ecc43aca92489626d4350b1ea208c47e48b44b13b26c93cd2f4744bcadf591aa9e9784f76e25ae38206237cbad33f577782ae2a56f7407eac94243f9997b7c8b3967571b989b6cdc364335a93b31543fa956ee7654c6f05ca1cecab9d1cc340e269a0ee7ca042687ccd5d3155df154cd3e67fad4c0a3f8ac0e9b3b3").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // key length
                128,
                // tag length
                64,
                // noKeyConfirmationNonce
                new BitString("ed7b8e50881ba27d0bb6a33dbb0f9e7e"),
                // aes-ccm nonce
                new BitString("42ba55af92c975"),
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("2d6d09d360cf277a1a749b03677a49ba054e8a4c4b77d8f66a537a89").ToPositiveBigInteger(),
                // public static this party
                new BitString("d0fe5b52c7488f11983d69201b3ceeb559fcb2b1fa98b8b70253dba1e6afdc21f8eba2e7e87f3abbe361a0b6ec97467685f5214b586f0b19c616af39a331bd8c201a6c6f31d8035dd3a94e39b9690040979938363a4c3f8fb2d268025f43a648e04476bf31cf62a36787baa5f8be38944781132d7e989536206c4acc3485d7e9dd878eca978ef48496b1a6c675cd86a436b94135643db088dda98d88ec92c137f16f169a39b27fd85e6136890b3a42b3f2356bd31dc0ba30654ae68a7fc729a112fa73f0f14a6a770e18eb7c40cf609e3d08dd27f35bdc3ebd1c46623398170d7d799800ee43ab8ed00824def52e602fd22c88f0a44dafded16e800e8a54159c").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("66207fd6fd0f05ffd08ec4b869af6831fd2684ff7be995a9ce3bd279").ToPositiveBigInteger(),
                // public static other party
                new BitString("1261cc852da6a2fb4528462e8ed467a246a50a398d532b013791fc3c373fe6782103af9533cf82fede7404138fc97829595d92162efb0dd8eccd036ec3d68ac27b3e445c8ce0f60db4ebe5094c4bb20644f62bca7d10953dab9a7f668961762e8e034f65f1e2a1a435c16613a8aae0e47432aef4faec19b60c3cc20bfdd0b17bc835df5d8c27d708f6645bd1328c49b74b89c335f1daa0afe7d85fc448d0737036fce056adcae8bcd4488b9ad511e442058d6eef4336b3230b8da7ec497f95b8bd018c241155ec79dede3592aad083c7efa3c542e38324c017db2707ecce6242414463344ec73a2784437c789576b9fc9a5593aa365dfc9fc37e2563029c8e13").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                new BitString("60a8d3fa7d3d957315adde0d2a01"),
                // expected Z
                new BitString("aae94826523986ec6b460c9b7e5b82666e941870f61505776091fd1af8bdeaec7789dc2227a06b3e40e698115da4a85ad721ddb27891755e321dfaef95ece9b95503a73b5fab771552b9bcebccffbb13dfd682fcd62ac64d8f8bed05473d1bf19a94e88faeb8f07d4135076314ec9ea3cd95ebb071919016dab65598ed7f6b0afe023adfd526ea1128eb375efe496a3de0d93454f35d1c405f34be8a3e1d33f743e76802b758db635e9ac88f09eaf41f03c7481f2897c8c2083a9a90c86cc02de86ed3d85e4be788dca7dfa2b526a6e826cc9dfe3fad5fb2f83f483af92cece3ab62f89f5abb5869a86fed5ab37c24961714f59a59e6886c671b8badf3127fd5"),
                // expected oi
                new BitString("a1b2c3d4e560a8d3fa7d3d957315adde0d2a01434156536964600cf36e99"),
                // expected dkm
                new BitString("23e83fdc4f0e4d4752247ab50fad2e20"),
                // expected macData
                new BitString("5374616e646172642054657374204d657373616765ed7b8e50881ba27d0bb6a33dbb0f9e7e"),
                // expected tag
                new BitString("377a428156d37e71")
            },
            #endregion ccm
            #endregion dhStatic
        };

        [Test]
        [TestCaseSource(nameof(_test_noKeyConfirmation))]
        public void ShouldNoKeyConfirmationCorrectly(
            string label,
            FfcDomainParameters domainParameters,
            FfcScheme scheme,
            KeyAgreementRole keyAgreementRole,
            ModeValues dsaKdfHashMode,
            DigestSizes dsaKdfDigestSize,
            KeyAgreementMacType macType,
            int keyLength,
            int tagLength,
            BitString noKeyConfirmationNonce,
            BitString aesCcmNonce,
            BitString thisPartyId,
            BigInteger thisPartyPrivateStaticKey,
            BigInteger thisPartyPublicStaticKey,
            BigInteger thisPartyPrivateEphemKey,
            BigInteger thisPartyPublicEphemKey,
            BitString thisPartyDkmNonce,
            BitString otherPartyId,
            BigInteger otherPartyPrivateStaticKey,
            BigInteger otherPartyPublicStaticKey,
            BigInteger otherPartyPrivateEphemKey,
            BigInteger otherPartyPublicEphemKey,
            BitString otherPartyDkmNonce,
            BitString expectedZ,
            BitString expectedOi,
            BitString expectedDkm,
            BitString expectedMacData,
            BitString expectedTag
        )
        {
            var otherPartySharedInformation =
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(
                    domainParameters,
                    otherPartyId,
                    new FfcKeyPair(otherPartyPublicStaticKey), 
                    new FfcKeyPair(otherPartyPublicEphemKey),
                    otherPartyDkmNonce,
                    null,
                    // when "party v" noKeyConfirmationNonce is provided as a part of party U's shared information
                    keyAgreementRole == KeyAgreementRole.ResponderPartyV ? noKeyConfirmationNonce : null
                );

            // u/v party info comprised of ID, and dkmNonce (when available), find the bitlength of both parties contributed information 
            // to determine which bits are the "random" bits to inject into the TestableEntropyProvider.
            var composedBitLength = thisPartyId.BitLength +
                                    (thisPartyDkmNonce?.BitLength ?? 0) + // DKM nonce when applicable
                                    (otherPartyDkmNonce?.BitLength ?? 0) + // DKM nonce when applicable
                                    otherPartyId.BitLength;

            var entropyBits = expectedOi.GetLeastSignificantBits(expectedOi.BitLength - composedBitLength);

            _entropyProviderOtherInfo.AddEntropy(entropyBits);

            // add dkm nonce to entropy provided when needed
            if (thisPartyDkmNonce != null)
            {
                _entropyProviderScheme.AddEntropy(thisPartyDkmNonce);
            }

            // MAC no key confirmation data makes use of a nonce
            _entropyProviderScheme.AddEntropy(noKeyConfirmationNonce);

            // The DSA sha mode determines which hash function to use in a component only test
            _dsa
                .SetupGet(s => s.Sha)
                .Returns(new ShaFactory().GetShaInstance(new HashFunction(dsaKdfHashMode, dsaKdfDigestSize)));
            _dsa
                .Setup(s => s.GenerateKeyPair(domainParameters))
                .Returns(() => new FfcKeyPairGenerateResult(new FfcKeyPair(0, 0)));

            var macParams = _macParamsBuilder
                .WithKeyAgreementMacType(macType)
                .WithMacLength(tagLength)
                .WithNonce(aesCcmNonce)
                .Build();

            var kas = _subject
                .WithKeyAgreementRole(keyAgreementRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesFfc(scheme, FfcParameterSet.Fb))
                .WithPartyId(thisPartyId)
                .BuildKdfNoKc()
                .WithKeyLength(keyLength)
                .WithMacParameters(macParams)
                .Build();

            kas.SetDomainParameters(domainParameters);
            kas.ReturnPublicInfoThisParty();

            if (kas.Scheme.StaticKeyPair != null)
            {
                kas.Scheme.StaticKeyPair.PrivateKeyX = thisPartyPrivateStaticKey;
                kas.Scheme.StaticKeyPair.PublicKeyY = thisPartyPublicStaticKey;
            }
            if (kas.Scheme.EphemeralKeyPair != null)
            {
                kas.Scheme.EphemeralKeyPair.PrivateKeyX = thisPartyPrivateEphemKey;
                kas.Scheme.EphemeralKeyPair.PublicKeyY = thisPartyPublicEphemKey;
            }

            var result = kas.ComputeResult(otherPartySharedInformation);

            Assume.That(result.Success, nameof(result.Success));
            Assert.AreEqual(expectedZ, result.Z, nameof(result.Z));
            Assert.AreEqual(expectedOi, result.Oi, nameof(result.Oi));
            Assert.AreEqual(expectedDkm, result.Dkm, nameof(result.Dkm));
            Assert.AreEqual(expectedMacData, result.MacData, nameof(result.MacData));
            Assert.AreEqual(expectedTag, result.Tag, nameof(result.Tag));
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
            #region ccm
            new object[]
            {
                // label
                "dhHybrid1 ccm sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("a31fab315fea239adce47d71d747c3db4aa8323f4409190bcaced2a8fc1ca74e3929d81d341f5b46d8e16cd86074f99cb4bf417281852bcb2f1609dfc5f8bdbc7fd56e44b7cfe70c8f477ccad25bb1e3a4b5d2b4c1c6e40288c9890ca2fa04629a25c28326b5a1ca8a02181abb8f59197eb445998f040758d9947a95ba06ef950465def704d89a6d78fa4bfa70770b3db0aa5d191649d369b14016896ab2daf8f26279d02269615fca2bb599783fdb7277886dadbe68612fc3c92f2a27ce3b83798f8999c77d4416a63ed3219904c34d14f7f2f7e116a0b22f9e0da7a78b2f2b828dca69a7a664f2d5b9fb26ac046e20b01c424fa61bb1618d107a046b0ddf5f").ToPositiveBigInteger(),
                    // q
                    new BitString("901eed7e944ee0ab038532fa86c26d13662b482924645c702e3b1373").ToPositiveBigInteger(), 
                    // g
                    new BitString("06ae53461279f88765605a280114967509e75446bb71fcbfab2ceffdf1697b16a92887170caa959ec63960528df6e55d1265397e01ded37a6d92cf2dcf8849b8fb546e85b710668dd4f9faa9cf319bb06c52e894d2914329ab48c8dd0fa599bf51c5a2241973a7ebab4c1a8ed348ce258b6b80989499a36afa6a483ad9599c135ee6e31ce01b60bc544f0c9b62a22dca23b760c94cf29b7b8173d2d434a6404a3c17134391b9d05d6b1c68c8c31ae11bc2e022cfc08aa0a09f0c7f6cd037077554c7f0ed6e8a62960f03118ff23e166bde2915c05b5fddcab7401e684e11583dd136ae213784d2952dce476e3af64f3b666aea6e2c3439e3a9705401e0874286").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // Key confirmation role
                KeyConfirmationRole.Provider,
                // Key confirmation direction
                KeyConfirmationDirection.Bilateral,
                // key length
                128,
                // tag length
                128,
                // aes-ccm nonce
                new BitString("33952ca4329671378e7efffa41"),
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("83cf8c7791bd7791260d139857df6cd297f85eb1ca6193b9bc0f1f5c").ToPositiveBigInteger(),
                // public static this party
                new BitString("894f1f382017257c55c29eedff69340471e953a4a3f2812b7c6f0f38d2ea9fb5420f5f93bf14397c89c09351b0aebeb30ca833acb8f5472e136d761370fce5cf9a1b8710a2ba2d82a7a38eb39feb06aace32045bc74f5798464b246790881fbef7549aed905ff16db75b0e2023a7b045af6b0059c290ddc787804c098c4744eb39ec630f4e6b4a65af559d5822bf9f691255568685e13499b61edb2881b60390dc46456a97edd4d34290b735976b52dd2a6e36948fa8b18a8c647b8474c99ac7ba9f32ccd52df0f9f3b4b48d11b09739c027c33b5a9c8ddfac4877dbc38e0b32d78a3d96acf5d9d45604b5f8d0bca9061bef72b6fcdfdda92eafa4edc223d6f9").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("42e1780924932bf2f5d66079c8066dc0703cf082195b9c20cbe38227").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("21bc81ba76f97e71b5ab828de9675333d23492f17da5e50f479126e211f643c048a31144fbeccf9f70810cb661ad347c7b15ff2d50a89d5465db776b370fd66c0a2c09ec08828aa23d2282174450b39da1bae1330447ed7ac29e175d49a6498d62489fc9ac1a5d22d755f9990959d84f2c80584bf3854a3e5bc5b8b0853a045bb40b68e8ad48a853b60a8ca01f06235d1fbf1a50946012c2cbe3650055c716f1a96d9f793d04fcd603bdd12f18b43b229974d9b604a4f9e583cbc6c3e9f0b4b88dd23f5572e21cf9e84a25aa2c74a1b04198a7c64c0ea81f22332cd2cf3db43cace8a24a301f76feb5f5c3062c37215cf3b342d2fe7fe292a9951fab34c5e8ba").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("68f99bc4cca57d85c7d582b236c1531c608d44283cd7e24d0f5f4753").ToPositiveBigInteger(), 
                // public static other party
                new BitString("56f22d30e5d73faae89e7911b139bd6b461cf22a755e0822be183ff7f9df9a9caccc7f8f32866ae5d48df0fcec2db215ed621ce0b197870b69505dc5d370f85e097c3ac735c540fed8c6ec95a1712d6322cb1abfca93da7260527d2c673907a6d6f27057ea089f208259c62e2c06578080822af6c81f2586208deeca81b5448f5037c450ff8a467f9c39910d71da52eeef15041a631931098f8c952fd863a1c54233af0b802e38eb9e0b21d211f3398c4e268283c65b3e250e691798ee0471cdd85397e2453512b306866b5823a5dc497bf74a41088f082b28e107f053b30e0e835c3c3d841053755734913b672fed5c1104a4434169c475676cd9632d79e6e1").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("048e96ae669db6d12ce755f3cb9025585e7b8c61a8d7680d5971d3ed").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("6fe96fff9eca9685d5d1afc01066b639cf16bcba3bb07640d5594b2423755b061588b08f59e1de640361f9bc5fe22ad6b7cd77a457cee2746d55ed28c47c80a683a755a6ab770d3dc15b2d49626383eb424e5faa7a9307052e5743c97b687db18372ce4624ac644c4f9b89b1368ed0abaa89cea95512d38d3e62f1b1f0a3ac1a3bac460095b4b370c462862b0003b007e5d21ea115e23e196c0baa617f4440424c40a9b6f4bfd51743eb093b5b573f1fdfe979605c3cb5468c6f43e7764c4d146561d4c3b982ee30626bf274c88d760208f62a620cdb997db9c887da08ac2cae74f9b4a04de1a64dd872ae3070690f43166fe97f57118590adaa6ea50d54f15b").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("05b9da5f3f6f3199b0d9f16c393e6164d9b37a09bdcc44c6a807c9e0ed0a40f8fd754d408cb9e26191da19d05913cd49bb195b6b7cbe8b0e12776e23ab4f48c540f36579d737b60a58390558c0606663178b18bf6ec850ae0dfd9fb72d3243eb379f6ce1127d8301d00a2e59f4046204453b031474c511ae41a6852a230b6dc2fe2094df240f42f4a1e3b27b7533fa4b003d6ace031ed4980637fe38c8b3152754bde211e982d03a986e6890bd7b4ce34144cc6ad4784f2e5c0074b5760d8036343e009905d805ee6ca652fbef8e54d9ad2ae98cc9ca9ffa0625e43115d0949d2544d062651e741c4831b07a095e2b1aa4453987826ea782632c7d7ed5c3812f7b8ff137373b26986dbb5909cc30084bcff42f14366cf79721241024047ab80323909c6e39fcd779cc1ee84fb3b1adba62cab44ac059b0afc4e7825b396ad0f5c1efb65c7afbc46af74267d0b1477c2c1c64616a0013b18e6fa8ebbeb0f14c15617e55b300219fe278d376815074b2ec26b47ab1e39c31f9bcef5c2a3db4ec83a60be97a9604c3b5bd5612f212e15cac96a088c996101af899960418d3b86c06b8051dbb95e8561f40be8d7ad310734cfd19cf35d0e8d547d5d08d2d0a33bf7bfa6ccb11ce1a58e3716f2d61124c45263c752275b9f4c8a580d9239beb46d747b8edcce977ab5f8d10c3742b94ef896e1c81a91912ce2e3b52c42299dbbc2b48"),
                // expected oi
                new BitString("434156536964a1b2c3d4e50a50995d65c4f080c67d6d50bf99487a59ccf0"),
                // expected dkm
                new BitString("0b33ea9b1571a9693bc05b7f6eb7627c"),
                // expected macData
                new BitString("4b435f325f56a1b2c3d4e543415653696421bc81ba76f97e71b5ab828de9675333d23492f17da5e50f479126e211f643c048a31144fbeccf9f70810cb661ad347c7b15ff2d50a89d5465db776b370fd66c0a2c09ec08828aa23d2282174450b39da1bae1330447ed7ac29e175d49a6498d62489fc9ac1a5d22d755f9990959d84f2c80584bf3854a3e5bc5b8b0853a045bb40b68e8ad48a853b60a8ca01f06235d1fbf1a50946012c2cbe3650055c716f1a96d9f793d04fcd603bdd12f18b43b229974d9b604a4f9e583cbc6c3e9f0b4b88dd23f5572e21cf9e84a25aa2c74a1b04198a7c64c0ea81f22332cd2cf3db43cace8a24a301f76feb5f5c3062c37215cf3b342d2fe7fe292a9951fab34c5e8ba6fe96fff9eca9685d5d1afc01066b639cf16bcba3bb07640d5594b2423755b061588b08f59e1de640361f9bc5fe22ad6b7cd77a457cee2746d55ed28c47c80a683a755a6ab770d3dc15b2d49626383eb424e5faa7a9307052e5743c97b687db18372ce4624ac644c4f9b89b1368ed0abaa89cea95512d38d3e62f1b1f0a3ac1a3bac460095b4b370c462862b0003b007e5d21ea115e23e196c0baa617f4440424c40a9b6f4bfd51743eb093b5b573f1fdfe979605c3cb5468c6f43e7764c4d146561d4c3b982ee30626bf274c88d760208f62a620cdb997db9c887da08ac2cae74f9b4a04de1a64dd872ae3070690f43166fe97f57118590adaa6ea50d54f15b"),
                // expected tag
                new BitString("b284d15fa00a53697e1fc73240d184bb")
            },
            new object[]
            {
                // label
                "dhHybrid1 ccm sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("a31fab315fea239adce47d71d747c3db4aa8323f4409190bcaced2a8fc1ca74e3929d81d341f5b46d8e16cd86074f99cb4bf417281852bcb2f1609dfc5f8bdbc7fd56e44b7cfe70c8f477ccad25bb1e3a4b5d2b4c1c6e40288c9890ca2fa04629a25c28326b5a1ca8a02181abb8f59197eb445998f040758d9947a95ba06ef950465def704d89a6d78fa4bfa70770b3db0aa5d191649d369b14016896ab2daf8f26279d02269615fca2bb599783fdb7277886dadbe68612fc3c92f2a27ce3b83798f8999c77d4416a63ed3219904c34d14f7f2f7e116a0b22f9e0da7a78b2f2b828dca69a7a664f2d5b9fb26ac046e20b01c424fa61bb1618d107a046b0ddf5f").ToPositiveBigInteger(),
                    // q
                    new BitString("901eed7e944ee0ab038532fa86c26d13662b482924645c702e3b1373").ToPositiveBigInteger(), 
                    // g
                    new BitString("06ae53461279f88765605a280114967509e75446bb71fcbfab2ceffdf1697b16a92887170caa959ec63960528df6e55d1265397e01ded37a6d92cf2dcf8849b8fb546e85b710668dd4f9faa9cf319bb06c52e894d2914329ab48c8dd0fa599bf51c5a2241973a7ebab4c1a8ed348ce258b6b80989499a36afa6a483ad9599c135ee6e31ce01b60bc544f0c9b62a22dca23b760c94cf29b7b8173d2d434a6404a3c17134391b9d05d6b1c68c8c31ae11bc2e022cfc08aa0a09f0c7f6cd037077554c7f0ed6e8a62960f03118ff23e166bde2915c05b5fddcab7401e684e11583dd136ae213784d2952dce476e3af64f3b666aea6e2c3439e3a9705401e0874286").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // Key confirmation role
                KeyConfirmationRole.Recipient,
                // Key confirmation direction
                KeyConfirmationDirection.Bilateral,
                // key length
                128,
                // tag length
                128,
                // aes-ccm nonce
                new BitString("33952ca4329671378e7efffa41"),
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("68f99bc4cca57d85c7d582b236c1531c608d44283cd7e24d0f5f4753").ToPositiveBigInteger(),
                // public static this party
                new BitString("56f22d30e5d73faae89e7911b139bd6b461cf22a755e0822be183ff7f9df9a9caccc7f8f32866ae5d48df0fcec2db215ed621ce0b197870b69505dc5d370f85e097c3ac735c540fed8c6ec95a1712d6322cb1abfca93da7260527d2c673907a6d6f27057ea089f208259c62e2c06578080822af6c81f2586208deeca81b5448f5037c450ff8a467f9c39910d71da52eeef15041a631931098f8c952fd863a1c54233af0b802e38eb9e0b21d211f3398c4e268283c65b3e250e691798ee0471cdd85397e2453512b306866b5823a5dc497bf74a41088f082b28e107f053b30e0e835c3c3d841053755734913b672fed5c1104a4434169c475676cd9632d79e6e1").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("048e96ae669db6d12ce755f3cb9025585e7b8c61a8d7680d5971d3ed").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("6fe96fff9eca9685d5d1afc01066b639cf16bcba3bb07640d5594b2423755b061588b08f59e1de640361f9bc5fe22ad6b7cd77a457cee2746d55ed28c47c80a683a755a6ab770d3dc15b2d49626383eb424e5faa7a9307052e5743c97b687db18372ce4624ac644c4f9b89b1368ed0abaa89cea95512d38d3e62f1b1f0a3ac1a3bac460095b4b370c462862b0003b007e5d21ea115e23e196c0baa617f4440424c40a9b6f4bfd51743eb093b5b573f1fdfe979605c3cb5468c6f43e7764c4d146561d4c3b982ee30626bf274c88d760208f62a620cdb997db9c887da08ac2cae74f9b4a04de1a64dd872ae3070690f43166fe97f57118590adaa6ea50d54f15b").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("83cf8c7791bd7791260d139857df6cd297f85eb1ca6193b9bc0f1f5c").ToPositiveBigInteger(), 
                // public static other party
                new BitString("894f1f382017257c55c29eedff69340471e953a4a3f2812b7c6f0f38d2ea9fb5420f5f93bf14397c89c09351b0aebeb30ca833acb8f5472e136d761370fce5cf9a1b8710a2ba2d82a7a38eb39feb06aace32045bc74f5798464b246790881fbef7549aed905ff16db75b0e2023a7b045af6b0059c290ddc787804c098c4744eb39ec630f4e6b4a65af559d5822bf9f691255568685e13499b61edb2881b60390dc46456a97edd4d34290b735976b52dd2a6e36948fa8b18a8c647b8474c99ac7ba9f32ccd52df0f9f3b4b48d11b09739c027c33b5a9c8ddfac4877dbc38e0b32d78a3d96acf5d9d45604b5f8d0bca9061bef72b6fcdfdda92eafa4edc223d6f9").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("42e1780924932bf2f5d66079c8066dc0703cf082195b9c20cbe38227").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("21bc81ba76f97e71b5ab828de9675333d23492f17da5e50f479126e211f643c048a31144fbeccf9f70810cb661ad347c7b15ff2d50a89d5465db776b370fd66c0a2c09ec08828aa23d2282174450b39da1bae1330447ed7ac29e175d49a6498d62489fc9ac1a5d22d755f9990959d84f2c80584bf3854a3e5bc5b8b0853a045bb40b68e8ad48a853b60a8ca01f06235d1fbf1a50946012c2cbe3650055c716f1a96d9f793d04fcd603bdd12f18b43b229974d9b604a4f9e583cbc6c3e9f0b4b88dd23f5572e21cf9e84a25aa2c74a1b04198a7c64c0ea81f22332cd2cf3db43cace8a24a301f76feb5f5c3062c37215cf3b342d2fe7fe292a9951fab34c5e8ba").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("05b9da5f3f6f3199b0d9f16c393e6164d9b37a09bdcc44c6a807c9e0ed0a40f8fd754d408cb9e26191da19d05913cd49bb195b6b7cbe8b0e12776e23ab4f48c540f36579d737b60a58390558c0606663178b18bf6ec850ae0dfd9fb72d3243eb379f6ce1127d8301d00a2e59f4046204453b031474c511ae41a6852a230b6dc2fe2094df240f42f4a1e3b27b7533fa4b003d6ace031ed4980637fe38c8b3152754bde211e982d03a986e6890bd7b4ce34144cc6ad4784f2e5c0074b5760d8036343e009905d805ee6ca652fbef8e54d9ad2ae98cc9ca9ffa0625e43115d0949d2544d062651e741c4831b07a095e2b1aa4453987826ea782632c7d7ed5c3812f7b8ff137373b26986dbb5909cc30084bcff42f14366cf79721241024047ab80323909c6e39fcd779cc1ee84fb3b1adba62cab44ac059b0afc4e7825b396ad0f5c1efb65c7afbc46af74267d0b1477c2c1c64616a0013b18e6fa8ebbeb0f14c15617e55b300219fe278d376815074b2ec26b47ab1e39c31f9bcef5c2a3db4ec83a60be97a9604c3b5bd5612f212e15cac96a088c996101af899960418d3b86c06b8051dbb95e8561f40be8d7ad310734cfd19cf35d0e8d547d5d08d2d0a33bf7bfa6ccb11ce1a58e3716f2d61124c45263c752275b9f4c8a580d9239beb46d747b8edcce977ab5f8d10c3742b94ef896e1c81a91912ce2e3b52c42299dbbc2b48"),
                // expected oi
                new BitString("434156536964a1b2c3d4e50a50995d65c4f080c67d6d50bf99487a59ccf0"),
                // expected dkm
                new BitString("0b33ea9b1571a9693bc05b7f6eb7627c"),
                // expected macData
                new BitString("4b435f325f56a1b2c3d4e543415653696421bc81ba76f97e71b5ab828de9675333d23492f17da5e50f479126e211f643c048a31144fbeccf9f70810cb661ad347c7b15ff2d50a89d5465db776b370fd66c0a2c09ec08828aa23d2282174450b39da1bae1330447ed7ac29e175d49a6498d62489fc9ac1a5d22d755f9990959d84f2c80584bf3854a3e5bc5b8b0853a045bb40b68e8ad48a853b60a8ca01f06235d1fbf1a50946012c2cbe3650055c716f1a96d9f793d04fcd603bdd12f18b43b229974d9b604a4f9e583cbc6c3e9f0b4b88dd23f5572e21cf9e84a25aa2c74a1b04198a7c64c0ea81f22332cd2cf3db43cace8a24a301f76feb5f5c3062c37215cf3b342d2fe7fe292a9951fab34c5e8ba6fe96fff9eca9685d5d1afc01066b639cf16bcba3bb07640d5594b2423755b061588b08f59e1de640361f9bc5fe22ad6b7cd77a457cee2746d55ed28c47c80a683a755a6ab770d3dc15b2d49626383eb424e5faa7a9307052e5743c97b687db18372ce4624ac644c4f9b89b1368ed0abaa89cea95512d38d3e62f1b1f0a3ac1a3bac460095b4b370c462862b0003b007e5d21ea115e23e196c0baa617f4440424c40a9b6f4bfd51743eb093b5b573f1fdfe979605c3cb5468c6f43e7764c4d146561d4c3b982ee30626bf274c88d760208f62a620cdb997db9c887da08ac2cae74f9b4a04de1a64dd872ae3070690f43166fe97f57118590adaa6ea50d54f15b"),
                // expected tag
                new BitString("b284d15fa00a53697e1fc73240d184bb")
            },
            #endregion ccm
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

            #region ccm
            new object[]
            {
                // label
                "mqv2 ccm sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("bcfc85efb89fd41d87a772f0143d347c9d7a5cac031b7cbf3adbd93ef1e91e1c1c92a32e57a3d6eafcd8f18c22e4c7760cdb662aaf6431c6c232200f6ca76482304057e0e9766427cee4e829c50a5c1a3fa17c4d66bde11e91066ddad2d22a0116cc23776dc2cdcfff1933b85a70541f397d167d795c899cc7d8966a8e418bedbb234ec9702d17ca12c7eeffb7b5d97978fc1f3a091915857ab613cac05e2968374bc684c0e7ee9c91d80752b61fccc37483b062c3ede9fa33536526c4fa551f5d5c0708eeab2a370e6f08cfed62bedcc993ed023b79fc9d86b13b2dbf38ef0b839c17c4427b6c73e3d51b47b940ae628c133e7e35c2325e4d1afbd65026536d").ToPositiveBigInteger(),
                    // q
                    new BitString("c99e2285022a8924aafac445fb0a764efa55eeab9e63e0a6245ca47b").ToPositiveBigInteger(), 
                    // g
                    new BitString("6a49a33adc600734413902b20763dea3e2abe519badba39d47748284caedf73239f740da98d5d507a400432520c9c7d7bdd3137738bb802836a30435c2f8d12d8fb42e18c6d95f8611d7466e7902426fd348a67febbe6fae8d158a8044c88c2bb57c47c2415f35c5c8a11bdab04fb9b0712c55c180fe6fd8fe7b962e99196ccabd28a71288cd01af39b48c880d637410a3bbaa9936bf43cb3431707ac79e9da07193a04a333cf17e1a878a1788ea22c073ae256c50ad5162488b8e87945a0ea778cab5ca6d91d87444789af36c390a2d8c754394cd7050d64666e05b4b08983f6b6d0b9d29422d05b7e30ee5043819495b93f60c992dde470d1a341865205c8d").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // Key confirmation role
                KeyConfirmationRole.Provider,
                // Key confirmation direction
                KeyConfirmationDirection.Bilateral,
                // key length
                128,
                // tag length
                128,
                // aes-ccm nonce
                new BitString("7ee4874e5bbee6aa184d275cde"),
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("238aa9bd15f933488b137cd78e611c894cc8337ec138a6b28ff4c7e5").ToPositiveBigInteger(),
                // public static this party
                new BitString("10f468f4b6fd453309dc10c7d158c764f78f8b90ab7d91fedbcc20a23be2b685bb5bc1f508a63a4b6899816cd94f04691b5b1b951b2c95584ff5146f710a540a34b2ae21f3b7f6503d36711bfadf548ed0461e7780ba65a64ba7df848f6eb953a2e10ed0a13dc54b6b8d07dae1be0a730c5465ac074b19b581c96ceccd1720452eca23401efa09ceb3a76795af9297723d7bbe48c21f34da22b2b59e4ad53a4718f6a06f78c259c360db00db681cd7f08accf9ccd5b3d95eac598f59ac46b6455bd2ba2bd1b08fe9efaba19e2190ce5ecb870c31055f08dbfd9859ee780f065b1543a56889507f0b79bf8581ae948beb99550620d60f8dd64a0e4af2a22880fb").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("5aac6ae71d099f5d434d3d025bdf7eda0bc46e9b151f201f49243c09").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("8a62c99a00fe56d6ca021d9e7efa32b8b5df37740c24cac95cfa57a98f5c726868779ebd68d654c9952e6174118dee4ec22ab2af3b106f4f41bdc49d2d42a361e6a40bc91ffa9449d1997b366c5408ba93f419e1eaa25ac750fe09d10f474485174a0dde22d9c4a076d713d5463d7635da21da7f043a29fa7a9e796e478e9c632369ffe44c0852ee7147239cff26099c1890a5ca64f05b932ecf104f2ab107e8fe42615eeda3fcfc498b42106395ea085547d778b75c829116f8ca71d62be563a9fcb6ed7ab1e3144efe4f136be692713076b2d86629954851b824d77a917dc6d8681fa70d62a87178ba5b6ed84df9e6727853f331b2ec7303c679dc46e334e8").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("3bd265bed07ececc76e848e082687c916a9a86a07cbc3e6bdacd161d").ToPositiveBigInteger(), 
                // public static other party
                new BitString("504e2a4fef9ef9a2dac329a0b6ded83b96a8662fc1d3f916ac53d5c524e02c78d88cf9efdec83d133ea70e1b054922520476239d1d2ed5d04db5962d1c4cf164bb34e6bdba023e2e60c9e6f554d1c768e1ef30dc6b6c171a3ec5a23fd2d10e668571a8dee06beebc0192d809cf3e36ad0b99b73059fd9b9ebab1953011832143834005f0d7972daf0b1789150801ad64ec8d959d0076db59103092fb7a8c20b77b1ca38b2b77aefe638ba720db3d2c9cfe3d9e53b8217fd82b6cd8851868781b13d5f75c6aeae169db207c4099e21db28bbec0ab1825cce6076a5edbb0012da2a7b4f0cb1c781c5cac3ed6b5898151c8a085ca620978b5c04613c0340fd26584").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("99ce04a1d0b87332be7c67000d3d4c572e49f4a7c1fd8ba6829390ac").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("2a8e351686d839a71dbb284e2ae446fced6f9185678e6b42e94cf914a75114271de8db5677bdfef0323cea92f4a1d64f94a0770f2c70ba46a81c15e0dd9e89a5b73b3019710bd26255298b7aed16c11e14be17e78167bc775f4c9767b6e161af6376c0a2a24d228e1be0522fe36b56e2f00a59bc59fb7f09639b401a9ee0abde41fe50e030a3fd4012a59aa3387f30ed0090dde1f7aef38fada13442ab83bcfb609bf7009986c0ef11b3ad13778990dacbf80601bf15d786c9f9460dd252d936884167e450b6bdddf6bd96ab09daa4aeb86b35df7e0a123065dba91c67efa7dfabd995d19a838fe3512950df657abfb551ceb622317872678fd42aa1f0222245").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("26d288aae4bcd063da14ee9901cc34f7be05a8d7d6a0c366f2d56a1bc6aad9ca1591b2c3c505a23b020eb4b0cbc31f6fa06aa9904206bcfbb52fbe28d1eeb21f60ca8847fa7086b637670afa0de8bdcae1cf7a9f059228bb9c50258ddf96fbcbada9d0d8c4d59ff5d02cbbec88dd7d93eb45cac4197671035d5001757f7cb198af6cb23e452d2a3251e2396492520fb80091b5ee50c512178551279506c0f4aa6c2145c36b812ce6e62651a21bedf905bf71f336ea2e8800d54a0f7e502f19de615c6434686bbf54d905ca15cc90518e24dfa4809653d18144f4ae8634cf58ad8cb40b840192c26922e47253835645f713bb31e7ab2f201d3c0951461e8540b0"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964489ebd71d31f112cb67a34675eb81c0aa2a17d"),
                // expected dkm
                new BitString("ac411211e3bebaeee45907f3a0444ef9"),
                // expected macData
                new BitString("4b435f325f55a1b2c3d4e54341565369648a62c99a00fe56d6ca021d9e7efa32b8b5df37740c24cac95cfa57a98f5c726868779ebd68d654c9952e6174118dee4ec22ab2af3b106f4f41bdc49d2d42a361e6a40bc91ffa9449d1997b366c5408ba93f419e1eaa25ac750fe09d10f474485174a0dde22d9c4a076d713d5463d7635da21da7f043a29fa7a9e796e478e9c632369ffe44c0852ee7147239cff26099c1890a5ca64f05b932ecf104f2ab107e8fe42615eeda3fcfc498b42106395ea085547d778b75c829116f8ca71d62be563a9fcb6ed7ab1e3144efe4f136be692713076b2d86629954851b824d77a917dc6d8681fa70d62a87178ba5b6ed84df9e6727853f331b2ec7303c679dc46e334e82a8e351686d839a71dbb284e2ae446fced6f9185678e6b42e94cf914a75114271de8db5677bdfef0323cea92f4a1d64f94a0770f2c70ba46a81c15e0dd9e89a5b73b3019710bd26255298b7aed16c11e14be17e78167bc775f4c9767b6e161af6376c0a2a24d228e1be0522fe36b56e2f00a59bc59fb7f09639b401a9ee0abde41fe50e030a3fd4012a59aa3387f30ed0090dde1f7aef38fada13442ab83bcfb609bf7009986c0ef11b3ad13778990dacbf80601bf15d786c9f9460dd252d936884167e450b6bdddf6bd96ab09daa4aeb86b35df7e0a123065dba91c67efa7dfabd995d19a838fe3512950df657abfb551ceb622317872678fd42aa1f0222245"),
                // expected tag
                new BitString("78c4810010bb91080fef09cb53e5c00e")
            },
            new object[]
            {
                // label
                "mqv2 ccm sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("bcfc85efb89fd41d87a772f0143d347c9d7a5cac031b7cbf3adbd93ef1e91e1c1c92a32e57a3d6eafcd8f18c22e4c7760cdb662aaf6431c6c232200f6ca76482304057e0e9766427cee4e829c50a5c1a3fa17c4d66bde11e91066ddad2d22a0116cc23776dc2cdcfff1933b85a70541f397d167d795c899cc7d8966a8e418bedbb234ec9702d17ca12c7eeffb7b5d97978fc1f3a091915857ab613cac05e2968374bc684c0e7ee9c91d80752b61fccc37483b062c3ede9fa33536526c4fa551f5d5c0708eeab2a370e6f08cfed62bedcc993ed023b79fc9d86b13b2dbf38ef0b839c17c4427b6c73e3d51b47b940ae628c133e7e35c2325e4d1afbd65026536d").ToPositiveBigInteger(),
                    // q
                    new BitString("c99e2285022a8924aafac445fb0a764efa55eeab9e63e0a6245ca47b").ToPositiveBigInteger(), 
                    // g
                    new BitString("6a49a33adc600734413902b20763dea3e2abe519badba39d47748284caedf73239f740da98d5d507a400432520c9c7d7bdd3137738bb802836a30435c2f8d12d8fb42e18c6d95f8611d7466e7902426fd348a67febbe6fae8d158a8044c88c2bb57c47c2415f35c5c8a11bdab04fb9b0712c55c180fe6fd8fe7b962e99196ccabd28a71288cd01af39b48c880d637410a3bbaa9936bf43cb3431707ac79e9da07193a04a333cf17e1a878a1788ea22c073ae256c50ad5162488b8e87945a0ea778cab5ca6d91d87444789af36c390a2d8c754394cd7050d64666e05b4b08983f6b6d0b9d29422d05b7e30ee5043819495b93f60c992dde470d1a341865205c8d").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // Key confirmation role
                KeyConfirmationRole.Recipient,
                // Key confirmation direction
                KeyConfirmationDirection.Bilateral,
                // key length
                128,
                // tag length
                128,
                // aes-ccm nonce
                new BitString("7ee4874e5bbee6aa184d275cde"),
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("3bd265bed07ececc76e848e082687c916a9a86a07cbc3e6bdacd161d").ToPositiveBigInteger(),
                // public static this party
                new BitString("504e2a4fef9ef9a2dac329a0b6ded83b96a8662fc1d3f916ac53d5c524e02c78d88cf9efdec83d133ea70e1b054922520476239d1d2ed5d04db5962d1c4cf164bb34e6bdba023e2e60c9e6f554d1c768e1ef30dc6b6c171a3ec5a23fd2d10e668571a8dee06beebc0192d809cf3e36ad0b99b73059fd9b9ebab1953011832143834005f0d7972daf0b1789150801ad64ec8d959d0076db59103092fb7a8c20b77b1ca38b2b77aefe638ba720db3d2c9cfe3d9e53b8217fd82b6cd8851868781b13d5f75c6aeae169db207c4099e21db28bbec0ab1825cce6076a5edbb0012da2a7b4f0cb1c781c5cac3ed6b5898151c8a085ca620978b5c04613c0340fd26584").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("99ce04a1d0b87332be7c67000d3d4c572e49f4a7c1fd8ba6829390ac").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("2a8e351686d839a71dbb284e2ae446fced6f9185678e6b42e94cf914a75114271de8db5677bdfef0323cea92f4a1d64f94a0770f2c70ba46a81c15e0dd9e89a5b73b3019710bd26255298b7aed16c11e14be17e78167bc775f4c9767b6e161af6376c0a2a24d228e1be0522fe36b56e2f00a59bc59fb7f09639b401a9ee0abde41fe50e030a3fd4012a59aa3387f30ed0090dde1f7aef38fada13442ab83bcfb609bf7009986c0ef11b3ad13778990dacbf80601bf15d786c9f9460dd252d936884167e450b6bdddf6bd96ab09daa4aeb86b35df7e0a123065dba91c67efa7dfabd995d19a838fe3512950df657abfb551ceb622317872678fd42aa1f0222245").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("238aa9bd15f933488b137cd78e611c894cc8337ec138a6b28ff4c7e5").ToPositiveBigInteger(), 
                // public static other party
                new BitString("10f468f4b6fd453309dc10c7d158c764f78f8b90ab7d91fedbcc20a23be2b685bb5bc1f508a63a4b6899816cd94f04691b5b1b951b2c95584ff5146f710a540a34b2ae21f3b7f6503d36711bfadf548ed0461e7780ba65a64ba7df848f6eb953a2e10ed0a13dc54b6b8d07dae1be0a730c5465ac074b19b581c96ceccd1720452eca23401efa09ceb3a76795af9297723d7bbe48c21f34da22b2b59e4ad53a4718f6a06f78c259c360db00db681cd7f08accf9ccd5b3d95eac598f59ac46b6455bd2ba2bd1b08fe9efaba19e2190ce5ecb870c31055f08dbfd9859ee780f065b1543a56889507f0b79bf8581ae948beb99550620d60f8dd64a0e4af2a22880fb").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("5aac6ae71d099f5d434d3d025bdf7eda0bc46e9b151f201f49243c09").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("8a62c99a00fe56d6ca021d9e7efa32b8b5df37740c24cac95cfa57a98f5c726868779ebd68d654c9952e6174118dee4ec22ab2af3b106f4f41bdc49d2d42a361e6a40bc91ffa9449d1997b366c5408ba93f419e1eaa25ac750fe09d10f474485174a0dde22d9c4a076d713d5463d7635da21da7f043a29fa7a9e796e478e9c632369ffe44c0852ee7147239cff26099c1890a5ca64f05b932ecf104f2ab107e8fe42615eeda3fcfc498b42106395ea085547d778b75c829116f8ca71d62be563a9fcb6ed7ab1e3144efe4f136be692713076b2d86629954851b824d77a917dc6d8681fa70d62a87178ba5b6ed84df9e6727853f331b2ec7303c679dc46e334e8").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("26d288aae4bcd063da14ee9901cc34f7be05a8d7d6a0c366f2d56a1bc6aad9ca1591b2c3c505a23b020eb4b0cbc31f6fa06aa9904206bcfbb52fbe28d1eeb21f60ca8847fa7086b637670afa0de8bdcae1cf7a9f059228bb9c50258ddf96fbcbada9d0d8c4d59ff5d02cbbec88dd7d93eb45cac4197671035d5001757f7cb198af6cb23e452d2a3251e2396492520fb80091b5ee50c512178551279506c0f4aa6c2145c36b812ce6e62651a21bedf905bf71f336ea2e8800d54a0f7e502f19de615c6434686bbf54d905ca15cc90518e24dfa4809653d18144f4ae8634cf58ad8cb40b840192c26922e47253835645f713bb31e7ab2f201d3c0951461e8540b0"),
                // expected oi
                new BitString("a1b2c3d4e5434156536964489ebd71d31f112cb67a34675eb81c0aa2a17d"),
                // expected dkm
                new BitString("ac411211e3bebaeee45907f3a0444ef9"),
                // expected macData
                new BitString("4b435f325f55a1b2c3d4e54341565369648a62c99a00fe56d6ca021d9e7efa32b8b5df37740c24cac95cfa57a98f5c726868779ebd68d654c9952e6174118dee4ec22ab2af3b106f4f41bdc49d2d42a361e6a40bc91ffa9449d1997b366c5408ba93f419e1eaa25ac750fe09d10f474485174a0dde22d9c4a076d713d5463d7635da21da7f043a29fa7a9e796e478e9c632369ffe44c0852ee7147239cff26099c1890a5ca64f05b932ecf104f2ab107e8fe42615eeda3fcfc498b42106395ea085547d778b75c829116f8ca71d62be563a9fcb6ed7ab1e3144efe4f136be692713076b2d86629954851b824d77a917dc6d8681fa70d62a87178ba5b6ed84df9e6727853f331b2ec7303c679dc46e334e82a8e351686d839a71dbb284e2ae446fced6f9185678e6b42e94cf914a75114271de8db5677bdfef0323cea92f4a1d64f94a0770f2c70ba46a81c15e0dd9e89a5b73b3019710bd26255298b7aed16c11e14be17e78167bc775f4c9767b6e161af6376c0a2a24d228e1be0522fe36b56e2f00a59bc59fb7f09639b401a9ee0abde41fe50e030a3fd4012a59aa3387f30ed0090dde1f7aef38fada13442ab83bcfb609bf7009986c0ef11b3ad13778990dacbf80601bf15d786c9f9460dd252d936884167e450b6bdddf6bd96ab09daa4aeb86b35df7e0a123065dba91c67efa7dfabd995d19a838fe3512950df657abfb551ceb622317872678fd42aa1f0222245"),
                // expected tag
                new BitString("78c4810010bb91080fef09cb53e5c00e")
            },
            #endregion ccm
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

            #region ccm
            new object[]
            {
                // label
                "dhHybridOneFlow ccm sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("f063646443f27f0fd7bd07eaf79a2a7d8cac247072a9f72ab77ffc46e4df5539d58d91d8dae2868eba35bcab2114492d64da2d01563acadeb074f165418726ef232e5e43a1b2d0c3ab2f908bd3acc7244097695566257ca9d07fe75c1ecddfa17ce93d7f1f7782b053e179a037809e5ed2199f226a7aa2a43c210c2607e54e586f3559252b29dc11e320e4c75eb86a44ff9b03c7a0727738f72f36bb1e5cf8701eef887d778d04101f8864f4bd97cc58fd19af70fa251e43cca4bcfb9a98d2164baad7c14faaa6b361ce01e072f7108d732c64616d9f85acbfe235c4b47bf93ee6b36af36420bd05601b675fca285bbf28c1686481b89eed9d52042c0fb778c1").ToPositiveBigInteger(),
                    // q
                    new BitString("f387092e29f35654b63a94787a091eb2d75ad43a3c8ab75d5f1b76c3").ToPositiveBigInteger(), 
                    // g
                    new BitString("288484cd7779cb81c12147be8cfd3d31b05a2f580f1a690f6553dfdf588567262105ee219a988afdf274e7e1b2d8d23577852ec2e9ad06138a000ea788a607699dbebabdaa83730aa05073bc340236fdebf1689046716c9aa4a1e59e0edd624f43548576b571000c85e0cb0281d0d08897f9036c8fe7ee8570e9224bcf10fde712fc14ddcd103fb63a4d116f6f8a2d6a6da08b49eae09291650cbf4765591533960d430d258ee7993d547a0e2fd409f999dafe62463f5301a368a64cb37a128d795efd9c9f3e4f9f4cd33ecafbfd09eefea2354275aa835f7fec3ec16bf070b623c57ed63005c44967d35ef258fc57a174fb502d9cfb9dea78cb201cd0097f67").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // Key confirmation role
                KeyConfirmationRole.Recipient,
                // Key confirmation direction
                KeyConfirmationDirection.Unilateral,
                // key length
                128,
                // tag length
                128,
                // aes-ccm nonce
                new BitString("a101cb18e265653bea3772ca54"),
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("68b7dda479f8ec0ddd5dab55022781254786648f4e9f7c44815013e4").ToPositiveBigInteger(),
                // public static this party
                new BitString("5ce302b4be57c9faf1e4cbe04918423a141f0a6bd008030fc00b86d9e43f2c29aee0816f258af38e858341f14fe257cc25d3bdfd9f5f5e830c28a68e1d16c33b9dc0c90fff3c01153012d48b75a6e014143d8fba979e2d7c41d9c15feac29c2e2c286a35799dcffc8a1cb0cf2f6a2c56cceab55bb18bc7b2f106710ae97970b9b97fa1dbb24122bc490d459276ecba41df3564a45b7c596e34222ae6e7f63de237ecf37ba64c7710bf9ec0786aa1279ebbd2a8b1201b6b8d2ab30e320e9639407d4e4c3b1bf09ac1d5bd548f6b5d1bc5062b96bf003e4da467348db8a4daf22e14572655a5c1322d13054a90959e62854788a7e4518a9a9765069c483678f03e").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("072460fda97147f0f994051e3da40154cda210e092926140defe2c90").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("2969087fa9647a62fdfe198ae61b3f2a10cb5cebb3f9638649072a4abaf1f758319bb0a746b54a0f355c9180c9b550a9131edb66406c7eb3e692f093c15298b3fd537a9bb2d02752b36c33b2fb01caa2133703952d2e670558a0cb84fa5777f2573dce480eaa10665f1f02e0ac8df6b16980439aed20b0ceab8b809175c36a922e47d4441d8e28103f7e74e02bdb99c626ac2249f86c754af35f22f18f22cd0cfe0dc81232d11f2fe0ec3f31b2e6b3fadde80a49caceb927e77ded576dff23fc7e4f21b20e906e77f9f5ed03b4d70c045f655531fb3b1bee901d14cbcade8f804ae3a2685e597844f0206f387324bc386d7d7a0e8b760057f7d2716a09b49555").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("e3310ff8f474e28727bf5b466c749b1750ebe20dcedbc67e07f80e5f").ToPositiveBigInteger(), 
                // public static other party
                new BitString("4aadba8cbf6b1a2749da62de3fb970e2d3e048cf0de9fab3ea23a4368c9ebe9007af24dc7eb3c485253c28aa3d3863745b74ab6d71d7903ef702f4b2eba8590313005dce61f1f3b069012f3c391d1dd3f855cc6b4c9d0d89d938ef51f8ae580d1cc6c471ee779142b152f13bd1e218cae5e4ed799599ca75e15c5b939db3a2ca2a8206201253d213b4e9c0511d5374ad8c4b1942f1e7c500ebf8630036ec376e66c2860315e94b9c373d05b5e7767841902490d5be997c0c7c3e07826b72678b23e11fd6a44aae9b3d61c6eae692076a5e212a224c72fe349b0961a49439f7574a5707965f283d9437909750f4546a555878927550f2bc4b83c75e7656c01ca1").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("2a27bcdceabca46101d72cf303ef1c2f46c190ad68fff01c858f69782426946f48b1eb2ff1926a77f759a0d2fdbd00dd700527dd1324bb0872b8b20f38a6be1c51ff191cbf3dd7af83cb9fa9aa9ad7ae5ffe9955f354998f0001c1b735bd1d4fa916fe5f1bff1799eb73cffe4d3ade3ef59cebe6029b44d28c5d5d884734da0c9f694f893777b716bf54e220c4c400742e04a19103e0842964a8d5a7f0618a1c5f5096ab59bd446ef72a77417ac330926fedcc69d1aa0cc3eceb872a1d4def67d691597ee5b2b1ad6f5f918952fe76d4a7baedac66c7ae07a8ce9f7e8d4a6aee84c94b875cc44519739f81d9f0f6def1201c0c3df3775aa518cfc2ce6ecf6f0e1cd8ec09ebcc427f182928bbe584f336b123c552e63b48da9d1e9d5401154506edc20fcd5c94bf940adc3f4536aeab20fb05ed3648a0459445b57171ea1e540e0286113f1ae69986b630df8d198cae279cdbd9a68cd8e65f5d6f0a5ab7ece9c8da6046642a0fecccdaf5f1f0cc1f20aa2c510c3303d4e5e310acd760b4a2d4a609b1561435bc50561f84e3d8d326c8be701e33c9729a45f80a15a01b0fa53040cf5f56b891aa00a10231b8255afa2f1ffb6919f26aa26b69e26c4a46cc3e1150886281388a74c10ed1767d0745312d8aa83f5d55e76b9b55ba6a1caf73ea6c34499689e8999197967a751b6ad901de127a2e57f48aba903d4baf3a3db24f521f"),
                // expected oi
                new BitString("a1b2c3d4e54341565369644bbd67d7ec71c0ae0975e7e1b8d4ca777272cc"),
                // expected dkm
                new BitString("6e4e3f93ddb181ed4917be5679bc3e8f"),
                // expected macData
                new BitString("4b435f315f56434156536964a1b2c3d4e52969087fa9647a62fdfe198ae61b3f2a10cb5cebb3f9638649072a4abaf1f758319bb0a746b54a0f355c9180c9b550a9131edb66406c7eb3e692f093c15298b3fd537a9bb2d02752b36c33b2fb01caa2133703952d2e670558a0cb84fa5777f2573dce480eaa10665f1f02e0ac8df6b16980439aed20b0ceab8b809175c36a922e47d4441d8e28103f7e74e02bdb99c626ac2249f86c754af35f22f18f22cd0cfe0dc81232d11f2fe0ec3f31b2e6b3fadde80a49caceb927e77ded576dff23fc7e4f21b20e906e77f9f5ed03b4d70c045f655531fb3b1bee901d14cbcade8f804ae3a2685e597844f0206f387324bc386d7d7a0e8b760057f7d2716a09b49555"),
                // expected tag
                new BitString("9e40d887e04a902f383f701f5bd1a3f5")
            },
            new object[]
            {
                // label
                "dhHybridOneFlow ccm sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("f063646443f27f0fd7bd07eaf79a2a7d8cac247072a9f72ab77ffc46e4df5539d58d91d8dae2868eba35bcab2114492d64da2d01563acadeb074f165418726ef232e5e43a1b2d0c3ab2f908bd3acc7244097695566257ca9d07fe75c1ecddfa17ce93d7f1f7782b053e179a037809e5ed2199f226a7aa2a43c210c2607e54e586f3559252b29dc11e320e4c75eb86a44ff9b03c7a0727738f72f36bb1e5cf8701eef887d778d04101f8864f4bd97cc58fd19af70fa251e43cca4bcfb9a98d2164baad7c14faaa6b361ce01e072f7108d732c64616d9f85acbfe235c4b47bf93ee6b36af36420bd05601b675fca285bbf28c1686481b89eed9d52042c0fb778c1").ToPositiveBigInteger(),
                    // q
                    new BitString("f387092e29f35654b63a94787a091eb2d75ad43a3c8ab75d5f1b76c3").ToPositiveBigInteger(), 
                    // g
                    new BitString("288484cd7779cb81c12147be8cfd3d31b05a2f580f1a690f6553dfdf588567262105ee219a988afdf274e7e1b2d8d23577852ec2e9ad06138a000ea788a607699dbebabdaa83730aa05073bc340236fdebf1689046716c9aa4a1e59e0edd624f43548576b571000c85e0cb0281d0d08897f9036c8fe7ee8570e9224bcf10fde712fc14ddcd103fb63a4d116f6f8a2d6a6da08b49eae09291650cbf4765591533960d430d258ee7993d547a0e2fd409f999dafe62463f5301a368a64cb37a128d795efd9c9f3e4f9f4cd33ecafbfd09eefea2354275aa835f7fec3ec16bf070b623c57ed63005c44967d35ef258fc57a174fb502d9cfb9dea78cb201cd0097f67").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // Key confirmation role
                KeyConfirmationRole.Provider,
                // Key confirmation direction
                KeyConfirmationDirection.Unilateral,
                // key length
                128,
                // tag length
                128,
                // aes-ccm nonce
                new BitString("a101cb18e265653bea3772ca54"),
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("e3310ff8f474e28727bf5b466c749b1750ebe20dcedbc67e07f80e5f").ToPositiveBigInteger(),
                // public static this party
                new BitString("4aadba8cbf6b1a2749da62de3fb970e2d3e048cf0de9fab3ea23a4368c9ebe9007af24dc7eb3c485253c28aa3d3863745b74ab6d71d7903ef702f4b2eba8590313005dce61f1f3b069012f3c391d1dd3f855cc6b4c9d0d89d938ef51f8ae580d1cc6c471ee779142b152f13bd1e218cae5e4ed799599ca75e15c5b939db3a2ca2a8206201253d213b4e9c0511d5374ad8c4b1942f1e7c500ebf8630036ec376e66c2860315e94b9c373d05b5e7767841902490d5be997c0c7c3e07826b72678b23e11fd6a44aae9b3d61c6eae692076a5e212a224c72fe349b0961a49439f7574a5707965f283d9437909750f4546a555878927550f2bc4b83c75e7656c01ca1").ToPositiveBigInteger(),
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
                new BitString("68b7dda479f8ec0ddd5dab55022781254786648f4e9f7c44815013e4").ToPositiveBigInteger(), 
                // public static other party
                new BitString("5ce302b4be57c9faf1e4cbe04918423a141f0a6bd008030fc00b86d9e43f2c29aee0816f258af38e858341f14fe257cc25d3bdfd9f5f5e830c28a68e1d16c33b9dc0c90fff3c01153012d48b75a6e014143d8fba979e2d7c41d9c15feac29c2e2c286a35799dcffc8a1cb0cf2f6a2c56cceab55bb18bc7b2f106710ae97970b9b97fa1dbb24122bc490d459276ecba41df3564a45b7c596e34222ae6e7f63de237ecf37ba64c7710bf9ec0786aa1279ebbd2a8b1201b6b8d2ab30e320e9639407d4e4c3b1bf09ac1d5bd548f6b5d1bc5062b96bf003e4da467348db8a4daf22e14572655a5c1322d13054a90959e62854788a7e4518a9a9765069c483678f03e").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("072460fda97147f0f994051e3da40154cda210e092926140defe2c90").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("2969087fa9647a62fdfe198ae61b3f2a10cb5cebb3f9638649072a4abaf1f758319bb0a746b54a0f355c9180c9b550a9131edb66406c7eb3e692f093c15298b3fd537a9bb2d02752b36c33b2fb01caa2133703952d2e670558a0cb84fa5777f2573dce480eaa10665f1f02e0ac8df6b16980439aed20b0ceab8b809175c36a922e47d4441d8e28103f7e74e02bdb99c626ac2249f86c754af35f22f18f22cd0cfe0dc81232d11f2fe0ec3f31b2e6b3fadde80a49caceb927e77ded576dff23fc7e4f21b20e906e77f9f5ed03b4d70c045f655531fb3b1bee901d14cbcade8f804ae3a2685e597844f0206f387324bc386d7d7a0e8b760057f7d2716a09b49555").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("2a27bcdceabca46101d72cf303ef1c2f46c190ad68fff01c858f69782426946f48b1eb2ff1926a77f759a0d2fdbd00dd700527dd1324bb0872b8b20f38a6be1c51ff191cbf3dd7af83cb9fa9aa9ad7ae5ffe9955f354998f0001c1b735bd1d4fa916fe5f1bff1799eb73cffe4d3ade3ef59cebe6029b44d28c5d5d884734da0c9f694f893777b716bf54e220c4c400742e04a19103e0842964a8d5a7f0618a1c5f5096ab59bd446ef72a77417ac330926fedcc69d1aa0cc3eceb872a1d4def67d691597ee5b2b1ad6f5f918952fe76d4a7baedac66c7ae07a8ce9f7e8d4a6aee84c94b875cc44519739f81d9f0f6def1201c0c3df3775aa518cfc2ce6ecf6f0e1cd8ec09ebcc427f182928bbe584f336b123c552e63b48da9d1e9d5401154506edc20fcd5c94bf940adc3f4536aeab20fb05ed3648a0459445b57171ea1e540e0286113f1ae69986b630df8d198cae279cdbd9a68cd8e65f5d6f0a5ab7ece9c8da6046642a0fecccdaf5f1f0cc1f20aa2c510c3303d4e5e310acd760b4a2d4a609b1561435bc50561f84e3d8d326c8be701e33c9729a45f80a15a01b0fa53040cf5f56b891aa00a10231b8255afa2f1ffb6919f26aa26b69e26c4a46cc3e1150886281388a74c10ed1767d0745312d8aa83f5d55e76b9b55ba6a1caf73ea6c34499689e8999197967a751b6ad901de127a2e57f48aba903d4baf3a3db24f521f"),
                // expected oi
                new BitString("a1b2c3d4e54341565369644bbd67d7ec71c0ae0975e7e1b8d4ca777272cc"),
                // expected dkm
                new BitString("6e4e3f93ddb181ed4917be5679bc3e8f"),
                // expected macData
                new BitString("4b435f315f56434156536964a1b2c3d4e52969087fa9647a62fdfe198ae61b3f2a10cb5cebb3f9638649072a4abaf1f758319bb0a746b54a0f355c9180c9b550a9131edb66406c7eb3e692f093c15298b3fd537a9bb2d02752b36c33b2fb01caa2133703952d2e670558a0cb84fa5777f2573dce480eaa10665f1f02e0ac8df6b16980439aed20b0ceab8b809175c36a922e47d4441d8e28103f7e74e02bdb99c626ac2249f86c754af35f22f18f22cd0cfe0dc81232d11f2fe0ec3f31b2e6b3fadde80a49caceb927e77ded576dff23fc7e4f21b20e906e77f9f5ed03b4d70c045f655531fb3b1bee901d14cbcade8f804ae3a2685e597844f0206f387324bc386d7d7a0e8b760057f7d2716a09b49555"),
                // expected tag
                new BitString("9e40d887e04a902f383f701f5bd1a3f5")
            },
            #endregion ccm
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
            #region aes-ccm
            new object[]
            {
                // label
                "mqv1 ccm sha2-224",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("c1af450dc2013f4ac1df8c33749db0b6d080bce55da07b477ba0a2b27bcde25d306e16688b654d4f0025e2c60fde974eec89732ccefb5b5b3b4b6b3623d633a186a9982526585a04a01ce866ba2c2930ac82d76bc85475ffa6eb5b5b8a6b8722901aa4fc2c77a1062477c25c7669f26e2a30895e8854cf98ca8a40204a159227291f1530b96493882b9ec09f395e0fdc8b877cd05c94782005cbe4b137f37a9b70da28ec5d64b06b242f3678ac485041121d1514b1ee88bb30f0b0bac3af3e750b8fbc69259f21fbd2d6886af585cd0c607e698d61672905c7a714ef588d7d9f70f0d3d66ea124f5fa0d4f7cbb4680fbc3e2dbc236dcc7ff3f32331272907261").ToPositiveBigInteger(),
                    // q
                    new BitString("da030247ad159d59dfcd8555c2fdefcb6deb3bfa815bc8e1c6d7b681").ToPositiveBigInteger(), 
                    // g
                    new BitString("ba58604c6af428c52c0b5f4048106b202d95eca5ab63c986d0f34cb067cdbf4d3646fee1d234e42096683f2dd7f02d1c764a3626cf3fdd7b1a081e8c2b965d567115afba4d4a9172cdcd7f2d6074816b29860a195f7dae112114166059463a9ea56e6f8e4c65724f7fa962a39a946bfe3c190944ea41c7f62d1a3e3855865da7dac9b21fa26195c6bc7593de3425e429e748309402925364e63985987a7c549795a250c1cf8073ed8f8de64f3d7c098d23dd5e78b8b03065df92e8c6da08b1eb202f8411a49709b7216824274e836729e463dc8d772c2bac7b5c99c2a7a39da208d94ec1a61a4cf12ffef3bbb6098a259a6a56c8d2e57a1840a7a429eabcb2f9").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // Key confirmation role
                KeyConfirmationRole.Recipient,
                // Key confirmation direction
                KeyConfirmationDirection.Unilateral,
                // key length
                128,
                // tag length
                128,
                // aes-ccm nonce
                new BitString("73c1d746d3cc737963e4122ae9"),
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("2b2e840e1b3028f6a1bc28aba8dff68052e2e1e924e931c8fe26786b").ToPositiveBigInteger(),
                // public static this party
                new BitString("aa6384e64b3c6bde2a532dbcbcf2f35ca982a8feccbe6a05e9987b2f57f76b4e1639d7048be1d02dee38c76f404e465c2323348365717a5208e7d5044c18d01ecdad02ced342c67de28e9ae4351aee394f0312abf9b51c45ff78e05ef0862e262621a5046e7a01db656a8a855482252f90811d30618507f338258cd5b8a354204578f11445b79763216faa75a08527b4c732212f7ea6839af6bf33a413b4ba350b0fd38b84bfc822bac679082c7a7ee3987ba376777fc73971965774b9a60addd9678df32d76d4e79ef5f7391515d80fd4e04ec6178371accab286b47dd60eeb67a2abc482beec3e4f0a0d333e7e5561507593ea3573c8ce22bd2ca2a4760c3a").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                new BitString("128236c0241e2db7e529625b716264906aef92c0e2ad28d07bc57a53cc807afe075c805cd95f4bc392ab43878653006db36d1063a468f56e95a06ee3d730f8da760793e01a3fc9ec2baaed9538ada1819a519a215497a89b3e0cf008639e7ce0a7aef7b2ad66b119f74deaeb4f184cb366c4b962bce14741bf31892a377111f8e177323c5a7d0b323fbdc0f3933e0beb61edf48ea2efdc476036c0b11c52bc0a6e8acee5eb2cdd1e4b21fa13ccc4e511d1f5d30dcf686e9569431e06dae987fe94105314261a31c763a21db5c155e30eff03de470bf5061322822a8f39dd79bc9c314733d5f00e14cf68b3403b970bc833409ea51d3eaba9d3196cb501d79a2d"), 
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("4cdede3ff2bfed88e63dfd29325452cd31f97c4fc02e9e6eaec7b835").ToPositiveBigInteger(), 
                // public static other party
                new BitString("6cfa1f6a17f3b7e85272608852b88c526b21f38f4fe853f132e0f181b00a1a38537feabccbcda593ed979714bb2c95f4d7c4a043fb5bf8698f46fdda72d9c8ae002889cc46fdb7e72808728068110e02f12b575e3962eaafeaf5c5ee3789699ee288a41991c9aa8be8f6a148e3ec4d8671467458e41094df5f5915edcc988af0c8c091c5249ee9e58d590736086e4b48087eb7a093236f90ea5f02db1b8875e57607b1bf1b2b0ae5cbaeb7e3acdac65241cb35d9a9e3bc1d01a02b0499a8e771d84526ecda8f9f40a6409cf75ba3e23ae17f3ef0806300f22f9fa2c1d588a16fdcf5180195483260cd7a79dbd75ef23fb3b8d4b165b3dc1e7d0b3bc4dc8f6407").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("717bd203746069b932ad83b54bdcffca60c15dbc09de37d14b6cfe92").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("9ca269eace1bed5502f3c92dbc4a600d1ee707842544c5ce95dc148b2670c68f892596fd7d1b5ceb7d1fa5eb7634fcab5c9e4a35c654e6d6fda18ccd2a3313f16bc0498d024064711d7f820db35f89116f54bf4d5dbbe394d48b43a4571a8e62c328b2a71e2b2405d8aba0fd8f312c01cde53c9a4c58e11efcd512737e953175aa62799a41c7855f7f331a1673874e22a0d78ff9809e124db3b893f79bf7e65917ca08e088b5fed1647466baac40707cc4637b972616511fd491213ea249206a1ab00acfe44996d7f642579a664bd26ca6e46a09cb346dc2fa524dc97d2f5bbc91d5e87af55b5b755ed2a90355275144dabc417d22d21ab7cefaaf5e6749707a").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("19b1ab50dde7e3ca6b2c5afbed9d41ee634ad291f080f803bdecab179494c6d44510bdcaff0e34b278f051ce74199d8056452c6f4a3305882c4a05008893c8c69e3e92abbf458ea448cd45bafa94931fc207af2065d75fb6302262f9edf8ea51b3ff5eaafe9bc486d49a22f1a11d5163b050a69bdcb5215010cd0832a14311e0a60cf3d11fee623bd71f25ed04e494f7a2dee205de8925cd7c63b1c1c1cd35011c903affad26c32bbac159a9b218426f3140723947e696bb8eb5a44ba277497da8ea7ef0b5d4aab573fb094100cadba89e8c7923023464fa95355c05d657a4d1b9c1ac9a36df880c7eabc0ce5076a45f002d8df3ad135cd64cd7ed870dcdb99c"),
                // expected oi
                new BitString("434156536964a1b2c3d4e5ca0b91d1892577c6f92b9fd666495ca1452c70"),
                // expected dkm
                new BitString("5587064ed0c1beac331258f9a33ea966"),
                // expected macData
                new BitString("4b435f315f55434156536964a1b2c3d4e59ca269eace1bed5502f3c92dbc4a600d1ee707842544c5ce95dc148b2670c68f892596fd7d1b5ceb7d1fa5eb7634fcab5c9e4a35c654e6d6fda18ccd2a3313f16bc0498d024064711d7f820db35f89116f54bf4d5dbbe394d48b43a4571a8e62c328b2a71e2b2405d8aba0fd8f312c01cde53c9a4c58e11efcd512737e953175aa62799a41c7855f7f331a1673874e22a0d78ff9809e124db3b893f79bf7e65917ca08e088b5fed1647466baac40707cc4637b972616511fd491213ea249206a1ab00acfe44996d7f642579a664bd26ca6e46a09cb346dc2fa524dc97d2f5bbc91d5e87af55b5b755ed2a90355275144dabc417d22d21ab7cefaaf5e6749707a128236c0241e2db7e529625b716264906aef92c0e2ad28d07bc57a53cc807afe075c805cd95f4bc392ab43878653006db36d1063a468f56e95a06ee3d730f8da760793e01a3fc9ec2baaed9538ada1819a519a215497a89b3e0cf008639e7ce0a7aef7b2ad66b119f74deaeb4f184cb366c4b962bce14741bf31892a377111f8e177323c5a7d0b323fbdc0f3933e0beb61edf48ea2efdc476036c0b11c52bc0a6e8acee5eb2cdd1e4b21fa13ccc4e511d1f5d30dcf686e9569431e06dae987fe94105314261a31c763a21db5c155e30eff03de470bf5061322822a8f39dd79bc9c314733d5f00e14cf68b3403b970bc833409ea51d3eaba9d3196cb501d79a2d"),
                // expected tag
                new BitString("fbac1e11e5290d953aedefd91b5e259d")
            },
            new object[]
            {
                // label
                "mqv1 ccm sha2-224 inverse",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("c1af450dc2013f4ac1df8c33749db0b6d080bce55da07b477ba0a2b27bcde25d306e16688b654d4f0025e2c60fde974eec89732ccefb5b5b3b4b6b3623d633a186a9982526585a04a01ce866ba2c2930ac82d76bc85475ffa6eb5b5b8a6b8722901aa4fc2c77a1062477c25c7669f26e2a30895e8854cf98ca8a40204a159227291f1530b96493882b9ec09f395e0fdc8b877cd05c94782005cbe4b137f37a9b70da28ec5d64b06b242f3678ac485041121d1514b1ee88bb30f0b0bac3af3e750b8fbc69259f21fbd2d6886af585cd0c607e698d61672905c7a714ef588d7d9f70f0d3d66ea124f5fa0d4f7cbb4680fbc3e2dbc236dcc7ff3f32331272907261").ToPositiveBigInteger(),
                    // q
                    new BitString("da030247ad159d59dfcd8555c2fdefcb6deb3bfa815bc8e1c6d7b681").ToPositiveBigInteger(), 
                    // g
                    new BitString("ba58604c6af428c52c0b5f4048106b202d95eca5ab63c986d0f34cb067cdbf4d3646fee1d234e42096683f2dd7f02d1c764a3626cf3fdd7b1a081e8c2b965d567115afba4d4a9172cdcd7f2d6074816b29860a195f7dae112114166059463a9ea56e6f8e4c65724f7fa962a39a946bfe3c190944ea41c7f62d1a3e3855865da7dac9b21fa26195c6bc7593de3425e429e748309402925364e63985987a7c549795a250c1cf8073ed8f8de64f3d7c098d23dd5e78b8b03065df92e8c6da08b1eb202f8411a49709b7216824274e836729e463dc8d772c2bac7b5c99c2a7a39da208d94ec1a61a4cf12ffef3bbb6098a259a6a56c8d2e57a1840a7a429eabcb2f9").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // Key confirmation role
                KeyConfirmationRole.Provider,
                // Key confirmation direction
                KeyConfirmationDirection.Unilateral,
                // key length
                128,
                // tag length
                128,
                // aes-ccm nonce
                new BitString("73c1d746d3cc737963e4122ae9"),
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("4cdede3ff2bfed88e63dfd29325452cd31f97c4fc02e9e6eaec7b835").ToPositiveBigInteger(),
                // public static this party
                new BitString("6cfa1f6a17f3b7e85272608852b88c526b21f38f4fe853f132e0f181b00a1a38537feabccbcda593ed979714bb2c95f4d7c4a043fb5bf8698f46fdda72d9c8ae002889cc46fdb7e72808728068110e02f12b575e3962eaafeaf5c5ee3789699ee288a41991c9aa8be8f6a148e3ec4d8671467458e41094df5f5915edcc988af0c8c091c5249ee9e58d590736086e4b48087eb7a093236f90ea5f02db1b8875e57607b1bf1b2b0ae5cbaeb7e3acdac65241cb35d9a9e3bc1d01a02b0499a8e771d84526ecda8f9f40a6409cf75ba3e23ae17f3ef0806300f22f9fa2c1d588a16fdcf5180195483260cd7a79dbd75ef23fb3b8d4b165b3dc1e7d0b3bc4dc8f6407").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("717bd203746069b932ad83b54bdcffca60c15dbc09de37d14b6cfe92").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("9ca269eace1bed5502f3c92dbc4a600d1ee707842544c5ce95dc148b2670c68f892596fd7d1b5ceb7d1fa5eb7634fcab5c9e4a35c654e6d6fda18ccd2a3313f16bc0498d024064711d7f820db35f89116f54bf4d5dbbe394d48b43a4571a8e62c328b2a71e2b2405d8aba0fd8f312c01cde53c9a4c58e11efcd512737e953175aa62799a41c7855f7f331a1673874e22a0d78ff9809e124db3b893f79bf7e65917ca08e088b5fed1647466baac40707cc4637b972616511fd491213ea249206a1ab00acfe44996d7f642579a664bd26ca6e46a09cb346dc2fa524dc97d2f5bbc91d5e87af55b5b755ed2a90355275144dabc417d22d21ab7cefaaf5e6749707a").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("2b2e840e1b3028f6a1bc28aba8dff68052e2e1e924e931c8fe26786b").ToPositiveBigInteger(), 
                // public static other party
                new BitString("aa6384e64b3c6bde2a532dbcbcf2f35ca982a8feccbe6a05e9987b2f57f76b4e1639d7048be1d02dee38c76f404e465c2323348365717a5208e7d5044c18d01ecdad02ced342c67de28e9ae4351aee394f0312abf9b51c45ff78e05ef0862e262621a5046e7a01db656a8a855482252f90811d30618507f338258cd5b8a354204578f11445b79763216faa75a08527b4c732212f7ea6839af6bf33a413b4ba350b0fd38b84bfc822bac679082c7a7ee3987ba376777fc73971965774b9a60addd9678df32d76d4e79ef5f7391515d80fd4e04ec6178371accab286b47dd60eeb67a2abc482beec3e4f0a0d333e7e5561507593ea3573c8ce22bd2ca2a4760c3a").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                new BitString("128236c0241e2db7e529625b716264906aef92c0e2ad28d07bc57a53cc807afe075c805cd95f4bc392ab43878653006db36d1063a468f56e95a06ee3d730f8da760793e01a3fc9ec2baaed9538ada1819a519a215497a89b3e0cf008639e7ce0a7aef7b2ad66b119f74deaeb4f184cb366c4b962bce14741bf31892a377111f8e177323c5a7d0b323fbdc0f3933e0beb61edf48ea2efdc476036c0b11c52bc0a6e8acee5eb2cdd1e4b21fa13ccc4e511d1f5d30dcf686e9569431e06dae987fe94105314261a31c763a21db5c155e30eff03de470bf5061322822a8f39dd79bc9c314733d5f00e14cf68b3403b970bc833409ea51d3eaba9d3196cb501d79a2d"), 
                // expected Z
                new BitString("19b1ab50dde7e3ca6b2c5afbed9d41ee634ad291f080f803bdecab179494c6d44510bdcaff0e34b278f051ce74199d8056452c6f4a3305882c4a05008893c8c69e3e92abbf458ea448cd45bafa94931fc207af2065d75fb6302262f9edf8ea51b3ff5eaafe9bc486d49a22f1a11d5163b050a69bdcb5215010cd0832a14311e0a60cf3d11fee623bd71f25ed04e494f7a2dee205de8925cd7c63b1c1c1cd35011c903affad26c32bbac159a9b218426f3140723947e696bb8eb5a44ba277497da8ea7ef0b5d4aab573fb094100cadba89e8c7923023464fa95355c05d657a4d1b9c1ac9a36df880c7eabc0ce5076a45f002d8df3ad135cd64cd7ed870dcdb99c"),
                // expected oi
                new BitString("434156536964a1b2c3d4e5ca0b91d1892577c6f92b9fd666495ca1452c70"),
                // expected dkm
                new BitString("5587064ed0c1beac331258f9a33ea966"),
                // expected macData
                new BitString("4b435f315f55434156536964a1b2c3d4e59ca269eace1bed5502f3c92dbc4a600d1ee707842544c5ce95dc148b2670c68f892596fd7d1b5ceb7d1fa5eb7634fcab5c9e4a35c654e6d6fda18ccd2a3313f16bc0498d024064711d7f820db35f89116f54bf4d5dbbe394d48b43a4571a8e62c328b2a71e2b2405d8aba0fd8f312c01cde53c9a4c58e11efcd512737e953175aa62799a41c7855f7f331a1673874e22a0d78ff9809e124db3b893f79bf7e65917ca08e088b5fed1647466baac40707cc4637b972616511fd491213ea249206a1ab00acfe44996d7f642579a664bd26ca6e46a09cb346dc2fa524dc97d2f5bbc91d5e87af55b5b755ed2a90355275144dabc417d22d21ab7cefaaf5e6749707a128236c0241e2db7e529625b716264906aef92c0e2ad28d07bc57a53cc807afe075c805cd95f4bc392ab43878653006db36d1063a468f56e95a06ee3d730f8da760793e01a3fc9ec2baaed9538ada1819a519a215497a89b3e0cf008639e7ce0a7aef7b2ad66b119f74deaeb4f184cb366c4b962bce14741bf31892a377111f8e177323c5a7d0b323fbdc0f3933e0beb61edf48ea2efdc476036c0b11c52bc0a6e8acee5eb2cdd1e4b21fa13ccc4e511d1f5d30dcf686e9569431e06dae987fe94105314261a31c763a21db5c155e30eff03de470bf5061322822a8f39dd79bc9c314733d5f00e14cf68b3403b970bc833409ea51d3eaba9d3196cb501d79a2d"),
                // expected tag
                new BitString("fbac1e11e5290d953aedefd91b5e259d")
            },
            #endregion aes-ccm
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
            
            #region ccm
            new object[]
            {
                // label
                "dhOneFlow ccm sha2-224",
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
                KeyAgreementMacType.AesCcm,
                // Key confirmation role
                KeyConfirmationRole.Recipient,
                // Key confirmation direction
                KeyConfirmationDirection.Unilateral,
                // key length
                128,
                // tag length
                128,
                // aes-ccm nonce
                new BitString("dc2720d73e1945b7ef51f8ca6f"),
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("00").ToPositiveBigInteger(),
                // public static this party
                new BitString("00").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("7a00eb9f012f115d041bf14c8b1098cbf516a193389e73b3f09cce61").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("4189f223b40276ba41cda60d980ddf22e1453c1e2712e01c3dcfc77c3bcfdb2fed11f09eb6d4183eed3950a34ffbf19408e47d558a1ef27ab58ed2bd02be3be19d823a559bcf2118e26d471e67f031d1c32c3f5ff18807a15971fb801b51fb7545e8cd2127da6b4ad7b1827798d65a63cc2471d023f8413fd29604a1ced8e0062d147cf9ec919c86b86a61fa7ff4a4dd5afa6e333dbe3a1d5dc854e109c9915fe59698203b5f6b4c9654db6f2839dc12ebd3f99a1f30076a94e4c9819e841c25c90ca9eb9bff75d242fabb10cd38411a14d08ba9a7fb1fd7f96b40c2e1054909ba14776b47fec48cff2cda0450fda3b2f0d695420144cd655b285da3e2032c1c").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("1de0f19e656a48901b488a5288706d0e653237aa594084f551d23e90").ToPositiveBigInteger(), 
                // public static other party
                new BitString("3a14741b647244daa9a5e893a98a9aeeabeb5128c599d3ff26aef7135f4b13b01e91eca1c26a7e0be79b0352d865d2f3b761bdec406dbed7b48166ec1921b71da252030719d9f5cfff2f1f7bc21540a2d037732ced630f63808a96ffa4cac9119e22e6d7cac686ea44ac5a6c4fcb02976394a082efbf525baf3a866af814eb6f752cb1f18204620c8cd09fcc4dee412718f45404b7b92274abdcf9ea12f5a782e2d8dc4b97218476f9d4ce6a740d5bf28755599f9209d1e8f6a098e7f29e9c5e1799619c04b1fd4944b0ffefba4ba361b4b1153efca67a7323d14b4adfae65436e3cdd97335e27185cc8ca6c46181fcb5d6e2002b14b6109a4889739ea46aa16").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("0f43bfc36a20e301e5a0751d47f72f8f0ef4ecaa1c0eafd5c9caf5bf04f1b84731f9aa70a13d347cce920951b8a50e32abda045d45424997dddd1b937a5fd433bb41712727ac1c6766de279cd6060769d88d61c567ac00a29dec55e081e7a42ddab43d7febb2cbc1a1efc841c32a869c13eadf92634480a19d668b4370542bdce614825fa21112414322ef689497604613bc14400a26cf407d32bc91f3b8f51b2b7c2849f86431b65f0782c688fd8a30e3d5f2c0941c68649ddae63cab1eb0d430c29e0bb13072a68eea1d8439309765d0eb790a905f44fe8c8048e9751f598eb283617b288b9943383a9c094b911c011bcaace5375d86c2b68f1fc20ec42e94"),
                // expected oi
                new BitString("a1b2c3d4e54341565369645e4d29c8c1059fd0ad77e7610784b079a43d14"),
                // expected dkm
                new BitString("95e84b02c11c26fc0aec2f602bf3418f"),
                // expected macData
                new BitString("4b435f315f56434156536964a1b2c3d4e54189f223b40276ba41cda60d980ddf22e1453c1e2712e01c3dcfc77c3bcfdb2fed11f09eb6d4183eed3950a34ffbf19408e47d558a1ef27ab58ed2bd02be3be19d823a559bcf2118e26d471e67f031d1c32c3f5ff18807a15971fb801b51fb7545e8cd2127da6b4ad7b1827798d65a63cc2471d023f8413fd29604a1ced8e0062d147cf9ec919c86b86a61fa7ff4a4dd5afa6e333dbe3a1d5dc854e109c9915fe59698203b5f6b4c9654db6f2839dc12ebd3f99a1f30076a94e4c9819e841c25c90ca9eb9bff75d242fabb10cd38411a14d08ba9a7fb1fd7f96b40c2e1054909ba14776b47fec48cff2cda0450fda3b2f0d695420144cd655b285da3e2032c1c"),
                // expected tag
                new BitString("b957628966e4d0db21872d0188cc500b")
            },
            new object[]
            {
                // label
                "dhOneFlow ccm sha2-224 inverse",
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
                KeyAgreementMacType.AesCcm,
                // Key confirmation role
                KeyConfirmationRole.Provider,
                // Key confirmation direction
                KeyConfirmationDirection.Unilateral,
                // key length
                128,
                // tag length
                128,
                // aes-ccm nonce
                new BitString("dc2720d73e1945b7ef51f8ca6f"),
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("1de0f19e656a48901b488a5288706d0e653237aa594084f551d23e90").ToPositiveBigInteger(),
                // public static this party
                new BitString("3a14741b647244daa9a5e893a98a9aeeabeb5128c599d3ff26aef7135f4b13b01e91eca1c26a7e0be79b0352d865d2f3b761bdec406dbed7b48166ec1921b71da252030719d9f5cfff2f1f7bc21540a2d037732ced630f63808a96ffa4cac9119e22e6d7cac686ea44ac5a6c4fcb02976394a082efbf525baf3a866af814eb6f752cb1f18204620c8cd09fcc4dee412718f45404b7b92274abdcf9ea12f5a782e2d8dc4b97218476f9d4ce6a740d5bf28755599f9209d1e8f6a098e7f29e9c5e1799619c04b1fd4944b0ffefba4ba361b4b1153efca67a7323d14b4adfae65436e3cdd97335e27185cc8ca6c46181fcb5d6e2002b14b6109a4889739ea46aa16").ToPositiveBigInteger(),
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
                new BitString("7a00eb9f012f115d041bf14c8b1098cbf516a193389e73b3f09cce61").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("4189f223b40276ba41cda60d980ddf22e1453c1e2712e01c3dcfc77c3bcfdb2fed11f09eb6d4183eed3950a34ffbf19408e47d558a1ef27ab58ed2bd02be3be19d823a559bcf2118e26d471e67f031d1c32c3f5ff18807a15971fb801b51fb7545e8cd2127da6b4ad7b1827798d65a63cc2471d023f8413fd29604a1ced8e0062d147cf9ec919c86b86a61fa7ff4a4dd5afa6e333dbe3a1d5dc854e109c9915fe59698203b5f6b4c9654db6f2839dc12ebd3f99a1f30076a94e4c9819e841c25c90ca9eb9bff75d242fabb10cd38411a14d08ba9a7fb1fd7f96b40c2e1054909ba14776b47fec48cff2cda0450fda3b2f0d695420144cd655b285da3e2032c1c").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("0f43bfc36a20e301e5a0751d47f72f8f0ef4ecaa1c0eafd5c9caf5bf04f1b84731f9aa70a13d347cce920951b8a50e32abda045d45424997dddd1b937a5fd433bb41712727ac1c6766de279cd6060769d88d61c567ac00a29dec55e081e7a42ddab43d7febb2cbc1a1efc841c32a869c13eadf92634480a19d668b4370542bdce614825fa21112414322ef689497604613bc14400a26cf407d32bc91f3b8f51b2b7c2849f86431b65f0782c688fd8a30e3d5f2c0941c68649ddae63cab1eb0d430c29e0bb13072a68eea1d8439309765d0eb790a905f44fe8c8048e9751f598eb283617b288b9943383a9c094b911c011bcaace5375d86c2b68f1fc20ec42e94"),
                // expected oi
                new BitString("a1b2c3d4e54341565369645e4d29c8c1059fd0ad77e7610784b079a43d14"),
                // expected dkm
                new BitString("95e84b02c11c26fc0aec2f602bf3418f"),
                // expected macData
                new BitString("4b435f315f56434156536964a1b2c3d4e54189f223b40276ba41cda60d980ddf22e1453c1e2712e01c3dcfc77c3bcfdb2fed11f09eb6d4183eed3950a34ffbf19408e47d558a1ef27ab58ed2bd02be3be19d823a559bcf2118e26d471e67f031d1c32c3f5ff18807a15971fb801b51fb7545e8cd2127da6b4ad7b1827798d65a63cc2471d023f8413fd29604a1ced8e0062d147cf9ec919c86b86a61fa7ff4a4dd5afa6e333dbe3a1d5dc854e109c9915fe59698203b5f6b4c9654db6f2839dc12ebd3f99a1f30076a94e4c9819e841c25c90ca9eb9bff75d242fabb10cd38411a14d08ba9a7fb1fd7f96b40c2e1054909ba14776b47fec48cff2cda0450fda3b2f0d695420144cd655b285da3e2032c1c"),
                // expected tag
                new BitString("b957628966e4d0db21872d0188cc500b")
            },
            #endregion ccm
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

            #region ccm
            new object[]
            {
                // label
                "dhStatic ccm sha2-224",
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
                KeyAgreementMacType.AesCcm,
                // Key confirmation role
                KeyConfirmationRole.Provider,
                // Key confirmation direction
                KeyConfirmationDirection.Bilateral,
                // key length
                128,
                // tag length
                128,
                // aes-ccm nonce
                new BitString("46512bdc7752b2eaa5fc4d8ab6"),
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("73e00757c2685bb97c87a4fbca2c613b184272027f36143f9a0250e6").ToPositiveBigInteger(),
                // public static this party
                new BitString("28e39795e31a48210aec7c77a3b1b989a4169e55bb82b98fe10e319c4016d736005642dcf82286d1074bb99d9a5e128dd46cc1becc547e90bf12dc9a66cac6080f3896f7e5aa519f193c63ddf6f1fae39d9339a2d7755e58d944b1e8d23f8b7dbe413477e7f8a9ec87e353a197269ee5be02c594f0839ff359be772853f3063f42495326e5ddee3db13b212bd0d1c60a8b0a4b734ee73e296a87a261f65823b88955f1c50cf602711308e56c1cf66cb2ca758fe11422f56ae16fe1f061dcedae20bfd0f5e94174b40885d589719b82dfb64c40eeb0f5cd1b35f9f9f2ee4139addfcf6609e79606f2a0e2de204f0ab314a0023e6d285eecf98d4ee576c0a4bb00").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                new BitString("04bd510a75a0488718a996a5e9f5"),
                // ephem nonce this party
                null,
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("410046ab3b79867d376e1dab12de7c10c6d531a8fbc408b671292607").ToPositiveBigInteger(), 
                // public static other party
                new BitString("7474e18538a50b92cf3c14fdb7ae2ac36e5e468a166c3657452ec77fbe87e07f8f9fcb665dcc7fa17b4d8c785246c6de841ca2f96c5ae5b36b49bcfe8a8bb9855452b99f8e0784e46b55e7e8c9ba4abde7756cc597031f0d8831242c5e58a2a2f1670f27d7f987327d58458b5722075b9b72af42e5c174fbc49ea8c541c6bbea7769fd2a5705384a8c88833a4ebc5ed313dbf7dbcf22a4fbd58f93c3045829e9a196b2df9a5a14b687282dbb3a02152dffbcbd6b3cfa512fc3d074e8b63db83cf8b059149cb5b9e5b341b54fa4a85304d1ffc44d69bb4129669db0c31ae0033dc992636e8a7987de66a4fc0dcde0f510488702491adf879684b5ede1cdc4bce9").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                null,
                // Ephem nonce other party
                new BitString("9bb7ae95688ac2f1cb3a681f2c9defb90653468c479d7705fa7272e623d00c3dea514797fcfed4712f67d6ef65f4e9d1b27663a2c101da3273fee647ca7c279006c78ada0d3cc986105b313b6c1ea8d8ce89eead9b8323ea8fe48a57a4cf8c46354200c763eba91ab83e006a0ac434b8a2b670159eca5c14944de67f78724249c0ea2fc4c5b47d146d37cce4078e965776237041927f8c98cb608325100c517fefe6a13afd3f4c5c786e1c102b23d59f92f8769b3f49bc5e7c46e8b33246c2d2095fde92d1331eda210c78583d2cf9763d5e0a8a6cd1f34def269d8fa7c89c29587c6c320a39fb76af39682307500bc6c17db576e3bf394f6b292b233639e76b"),
                // expected Z
                new BitString("664c4c3515e4af61fc962939cb63061fc2bd72da8bd5e89f4789a5c50f168cd67449e3c745177ccc1c8b262d41536ed517267539e704c95045cb76f4270439cb97b8b68bd7239f2b311ab8e8c888dd5e3ae1bcddafece8531e4d798f90d6bed8bdd9ffb9cd6e03b9074be27683364bc98ff8e8e8b59eb96c0395ec603e78370ad865c1c0cda2316b41a9f9e798f00255bde8a583767cffb3dcf8ac6765d5c02f01a9f12c94ac535e054f0b2ce7cd93816cf701a6f32d46b37727c13a42ba2c0c1b559c1d168af5a528b36fc654968849021cb157bc528df4da4292d575ce14321fcd35e43f5247aa42c92fbb51165211110d5263f696ef552002098fca97c3fa"),
                // expected oi
                new BitString("a1b2c3d4e504bd510a75a0488718a996a5e9f5434156536964930bb72674"),
                // expected dkm
                new BitString("071ded063e262a2540cfc14d037b2432"),
                // expected macData
                new BitString("4b435f325f55a1b2c3d4e543415653696404bd510a75a0488718a996a5e9f59bb7ae95688ac2f1cb3a681f2c9defb90653468c479d7705fa7272e623d00c3dea514797fcfed4712f67d6ef65f4e9d1b27663a2c101da3273fee647ca7c279006c78ada0d3cc986105b313b6c1ea8d8ce89eead9b8323ea8fe48a57a4cf8c46354200c763eba91ab83e006a0ac434b8a2b670159eca5c14944de67f78724249c0ea2fc4c5b47d146d37cce4078e965776237041927f8c98cb608325100c517fefe6a13afd3f4c5c786e1c102b23d59f92f8769b3f49bc5e7c46e8b33246c2d2095fde92d1331eda210c78583d2cf9763d5e0a8a6cd1f34def269d8fa7c89c29587c6c320a39fb76af39682307500bc6c17db576e3bf394f6b292b233639e76b"),
                // expected tag
                new BitString("0388d182e7969c3a23048bffc8cb5460")
            },
            new object[]
            {
                // label
                "dhStatic ccm sha2-224 inverse",
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
                KeyAgreementMacType.AesCcm,
                // Key confirmation role
                KeyConfirmationRole.Recipient,
                // Key confirmation direction
                KeyConfirmationDirection.Bilateral,
                // key length
                128,
                // tag length
                128,
                // aes-ccm nonce
                new BitString("46512bdc7752b2eaa5fc4d8ab6"),
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("410046ab3b79867d376e1dab12de7c10c6d531a8fbc408b671292607").ToPositiveBigInteger(),
                // public static this party
                new BitString("7474e18538a50b92cf3c14fdb7ae2ac36e5e468a166c3657452ec77fbe87e07f8f9fcb665dcc7fa17b4d8c785246c6de841ca2f96c5ae5b36b49bcfe8a8bb9855452b99f8e0784e46b55e7e8c9ba4abde7756cc597031f0d8831242c5e58a2a2f1670f27d7f987327d58458b5722075b9b72af42e5c174fbc49ea8c541c6bbea7769fd2a5705384a8c88833a4ebc5ed313dbf7dbcf22a4fbd58f93c3045829e9a196b2df9a5a14b687282dbb3a02152dffbcbd6b3cfa512fc3d074e8b63db83cf8b059149cb5b9e5b341b54fa4a85304d1ffc44d69bb4129669db0c31ae0033dc992636e8a7987de66a4fc0dcde0f510488702491adf879684b5ede1cdc4bce9").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                new BitString("9bb7ae95688ac2f1cb3a681f2c9defb90653468c479d7705fa7272e623d00c3dea514797fcfed4712f67d6ef65f4e9d1b27663a2c101da3273fee647ca7c279006c78ada0d3cc986105b313b6c1ea8d8ce89eead9b8323ea8fe48a57a4cf8c46354200c763eba91ab83e006a0ac434b8a2b670159eca5c14944de67f78724249c0ea2fc4c5b47d146d37cce4078e965776237041927f8c98cb608325100c517fefe6a13afd3f4c5c786e1c102b23d59f92f8769b3f49bc5e7c46e8b33246c2d2095fde92d1331eda210c78583d2cf9763d5e0a8a6cd1f34def269d8fa7c89c29587c6c320a39fb76af39682307500bc6c17db576e3bf394f6b292b233639e76b"),
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static other party
                new BitString("73e00757c2685bb97c87a4fbca2c613b184272027f36143f9a0250e6").ToPositiveBigInteger(), 
                // public static other party
                new BitString("28e39795e31a48210aec7c77a3b1b989a4169e55bb82b98fe10e319c4016d736005642dcf82286d1074bb99d9a5e128dd46cc1becc547e90bf12dc9a66cac6080f3896f7e5aa519f193c63ddf6f1fae39d9339a2d7755e58d944b1e8d23f8b7dbe413477e7f8a9ec87e353a197269ee5be02c594f0839ff359be772853f3063f42495326e5ddee3db13b212bd0d1c60a8b0a4b734ee73e296a87a261f65823b88955f1c50cf602711308e56c1cf66cb2ca758fe11422f56ae16fe1f061dcedae20bfd0f5e94174b40885d589719b82dfb64c40eeb0f5cd1b35f9f9f2ee4139addfcf6609e79606f2a0e2de204f0ab314a0023e6d285eecf98d4ee576c0a4bb00").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                new BitString("04bd510a75a0488718a996a5e9f5"),
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("664c4c3515e4af61fc962939cb63061fc2bd72da8bd5e89f4789a5c50f168cd67449e3c745177ccc1c8b262d41536ed517267539e704c95045cb76f4270439cb97b8b68bd7239f2b311ab8e8c888dd5e3ae1bcddafece8531e4d798f90d6bed8bdd9ffb9cd6e03b9074be27683364bc98ff8e8e8b59eb96c0395ec603e78370ad865c1c0cda2316b41a9f9e798f00255bde8a583767cffb3dcf8ac6765d5c02f01a9f12c94ac535e054f0b2ce7cd93816cf701a6f32d46b37727c13a42ba2c0c1b559c1d168af5a528b36fc654968849021cb157bc528df4da4292d575ce14321fcd35e43f5247aa42c92fbb51165211110d5263f696ef552002098fca97c3fa"),
                // expected oi
                new BitString("a1b2c3d4e504bd510a75a0488718a996a5e9f5434156536964930bb72674"),
                // expected dkm
                new BitString("071ded063e262a2540cfc14d037b2432"),
                // expected macData
                new BitString("4b435f325f55a1b2c3d4e543415653696404bd510a75a0488718a996a5e9f59bb7ae95688ac2f1cb3a681f2c9defb90653468c479d7705fa7272e623d00c3dea514797fcfed4712f67d6ef65f4e9d1b27663a2c101da3273fee647ca7c279006c78ada0d3cc986105b313b6c1ea8d8ce89eead9b8323ea8fe48a57a4cf8c46354200c763eba91ab83e006a0ac434b8a2b670159eca5c14944de67f78724249c0ea2fc4c5b47d146d37cce4078e965776237041927f8c98cb608325100c517fefe6a13afd3f4c5c786e1c102b23d59f92f8769b3f49bc5e7c46e8b33246c2d2095fde92d1331eda210c78583d2cf9763d5e0a8a6cd1f34def269d8fa7c89c29587c6c320a39fb76af39682307500bc6c17db576e3bf394f6b292b233639e76b"),
                // expected tag
                new BitString("0388d182e7969c3a23048bffc8cb5460")
            },
            new object[]
            {
                // label
                "dhStatic ccm sha2-224 second scenario",
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("82ae740eef15c71bcb89c686de8c039465eef628707e2d4a6d53d334ad2d3a26d8bd06198379c20a8f961daee6af911cc94e5cfb972ed42ce48d6b91aa18f24407330b88e96fd8f69844edfc10f49e9ae0c3d8c9797de60fc9e4eef11d3fffc8396cb980120f7847a39b0873063c8924e2d8f1571c26f74d11de40631bb36c122372a6d7b93c0816a619e3fbe96257697bc61d1d8004bfc6cd39dffd6cc4dcca4bf0512b0629c7c879e351e82c8612056e3bddaf181699bb423255a04206d1e1d8a848ec291d86439df99fa952387c45f04c09efedbcb18e5239288fc0d0ff4896673038cfe6eeb942ce4d059032521308129f1614717f04bebf242fa78a4c3f").ToPositiveBigInteger(),
                    // q
                    new BitString("e0334792be592fc6374d4b28e952db8b00b82b00b40dca23fd909135").ToPositiveBigInteger(), 
                    // g
                    new BitString("7bc96b3f11ff32708ab273570c5b0c0f6bcde1697c670c839b56950fe4283c3cd376dd2744c7facc11527085180c0eb6ea345311964d65dd076429766a3158308e7df7f4e874ec6cd89a9552ddfca0073d7df985164092d691485ad5c08c77e196c219e9acf3a16413e292389d8c590523924c7f3f0169083f09498fde65957f1dc2712ae239ff5a35183bf15b629da899d33e15943bb04beb1306f4d83545eeaced04a5b7d0e22a16c1a72e8343267b5ec43d9bf6c8993d9bc318684fbd80980ff73a918fdab9a8e028c6d4afcb0237413a73441b7e7b5a70f0795a664811baa265ff577c6817c12eff920dfd470a55da9f0194c554c91ffbe86601c5016b47").ToPositiveBigInteger()
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
                KeyAgreementMacType.AesCcm,
                // Key confirmation role
                KeyConfirmationRole.Provider,
                // Key confirmation direction
                KeyConfirmationDirection.Bilateral,
                // key length
                128,
                // tag length
                128,
                // aes-ccm nonce
                new BitString("06d267f1e30899187587c3e765"),
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("4139a0f4628f1bf97f63ff31367a58537356475157e6766f08a25c29").ToPositiveBigInteger(),
                // public static this party
                new BitString("548fc6df90777506530194196a76dcf148f510f64b38af75541f58c20e2b4bf1a998e7899acf4d3ad6ad433dfb91f319ea8a4b7514544f9d57576d323cb802eab94fa3f7dcf0939612c628f66ada4a3ee8b2ef9b5bf63b3aef6b52e98737ec1e480aea02da89be253d80571d30c24e5e9d428139445e41416518d1546a9ecc348814e55de38b91354b747ab515640f6a0c77eb79965c57c2b911ce3e52ff7d5aa133a56a9316c02be3cfa6e7f6e7eda3c182d9bef8ab9340398a787c6717fe7efd27af9295297c85517c60d938e5889e6fbed99ecbac9972bb19c033ac02f6a4f297ade161799a8c7a8d3e432cc49c30597ce9d71513e00c6298b5085e01c71d").ToPositiveBigInteger(),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce this party
                null,
                // ephem nonce this party
                new BitString("a6438dbbc0efd927c93141a9549e13283dee38b5e25f0b1090546c6e3c420f7974cc1a99952df0723dd747ae035981fc6686395804d41c8e0d02faf9f6fe1c917205d9259e0c64e47b5fe822a288d75933727f6e0fcede3647b6b739d490e5b68836f0e74454de856c114b8f98f1bc488cf9317f6b15f91227b8cab53cc211f09ea68767efcc055ef634997c4d5dd9cf596577117f71162a954f5cf5975a49479c9ec72e083d82760212f9722993d6f982fc7aaeb5aadd847ac495824e2134c46a66d7c3f56ffcd679f194f8945d5acbf00762dd7487f52abc5f9ce3c7df7b6ef046e0ae202a1c84411a9296f6810d4f89cd572725d1072445689aa06c5cc54e"),
                // Other Party ID
                new BitString("434156536964"),
                // private static other party
                new BitString("87b4232259d242288821e400bff08210294d214c5d874f78ce03a762").ToPositiveBigInteger(), 
                // public static other party
                new BitString("3f24eaa216ab1d7cc03e70b1df6a0f7efce065fa8065104aeb0ee59a6dcb0958f909bb7a5ce0c8e7cc26b01a42bab685fe055cfb97861bed4767885181f55c066af8c68f8df27a63bd24288d235ccb66c0021d37def576d1c9b8ed8e2960307b8f6477b838c3a64c6d660282f63c0977d34eeea16e72baeea1b5ae33382c87feb457ad654ffc91a4d8e56fee9c87950df23a2f334357d6f45466fb2b9edee9d1544a9e33b0b6b4d8e910e91d8f0d0872c60ad55a90cf38091b7a7e0144051072823e5478f9acc046c7de943ca1f0d04f618869b0b3e334e3e7658bc1eec8053f6fbdf9a87bc272ff0501254c92c94837353784e3b961501d9bbc1b96d556e9f6").ToPositiveBigInteger(),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("00").ToPositiveBigInteger(),
                // dkmNonce other party
                new BitString("6b44bb3e2d665999c3f7ef3af07b"),
                // Ephem nonce other party
                null,
                // expected Z
                new BitString("33a8ae50e45cc0192d54d81a911711f9e4426e18dc5f3caab0cbc3204798717a57508956266845a4465dd8f8571daa4dd2bd0ced323c6d7143e5174429db1e533497ff56257bd82d6386fef754c5f75e1407a78c0489dfe1df8982ae1928f1e1fdb3fc8447ce744a4515ec1dcacd6d862e9ec2b12718eccd31ac570328d515192b84fb84fd62d2fe237b4763479586dadec16729d1755c25ed3f3dcf2cdc38d21353f385762da214dda8a3e3982169f46b4068d10ecdc050ae88a5c529e1a1350b9371d87b8a9d3a334db46129dbf75b402cf4ca7d735948c5bc402ed7f91016d2de1d218d09a01dd9e6bea3ec3e490257194f33848fd9e0cbf40160c9faa178"),
                // expected oi
                new BitString("4341565369646b44bb3e2d665999c3f7ef3af07ba1b2c3d4e579c3d29376"),
                // expected dkm
                new BitString("6bbd73fa94ae00df561f0944a199611c"),
                // expected macData
                new BitString("4b435f325f56a1b2c3d4e5434156536964a6438dbbc0efd927c93141a9549e13283dee38b5e25f0b1090546c6e3c420f7974cc1a99952df0723dd747ae035981fc6686395804d41c8e0d02faf9f6fe1c917205d9259e0c64e47b5fe822a288d75933727f6e0fcede3647b6b739d490e5b68836f0e74454de856c114b8f98f1bc488cf9317f6b15f91227b8cab53cc211f09ea68767efcc055ef634997c4d5dd9cf596577117f71162a954f5cf5975a49479c9ec72e083d82760212f9722993d6f982fc7aaeb5aadd847ac495824e2134c46a66d7c3f56ffcd679f194f8945d5acbf00762dd7487f52abc5f9ce3c7df7b6ef046e0ae202a1c84411a9296f6810d4f89cd572725d1072445689aa06c5cc54e6b44bb3e2d665999c3f7ef3af07b"),
                // expected tag
                new BitString("8c0292f32157bb43084fe5b072b0ed9b")
            },
            #endregion ccm
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
            var otherPartySharedInformation =
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(
                    domainParameters,
                    otherPartyId,
                    new FfcKeyPair(otherPartyPublicStaticKey),
                    new FfcKeyPair(otherPartyPublicEphemKey),
                    otherPartyDkmNonce,
                    otherPartyEphemeralNonce,
                    null
                );

            // u/v party info comprised of ID, and dkmNonce (when available), find the bitlength of both parties contributed information 
            // to determine which bits are the "random" bits to inject into the TestableEntropyProvider.
            var composedBitLength = thisPartyId.BitLength +
                                    (thisPartyDkmNonce?.BitLength ?? 0) + // DKM nonce when applicable
                                    (otherPartyDkmNonce?.BitLength ?? 0) + // DKM nonce when applicable
                                    otherPartyId.BitLength;

            var entropyBits = expectedOi.GetLeastSignificantBits(expectedOi.BitLength - composedBitLength);

            _entropyProviderOtherInfo.AddEntropy(entropyBits);

            // add dkm nonce to entropy provided when needed
            if (thisPartyDkmNonce != null)
            {
                _entropyProviderScheme.AddEntropy(thisPartyDkmNonce);
            }

            // MAC no key confirmation data makes use of a nonce
            if (thisPartyEphemeralNonce != null)
            {
                _entropyProviderScheme.AddEntropy(thisPartyEphemeralNonce);
            }

            _dsa
                .SetupGet(s => s.Sha)
                .Returns(new ShaFactory().GetShaInstance(new HashFunction(dsaKdfHashMode, dsaKdfDigestSize)));
            _dsa
                .Setup(s => s.GenerateKeyPair(domainParameters))
                .Returns(() => new FfcKeyPairGenerateResult(new FfcKeyPair(0, 0)));

            var macParams = _macParamsBuilder
                .WithKeyAgreementMacType(macType)
                .WithMacLength(tagLength)
                .WithNonce(aesCcmNonce)
                .Build();

            var kas = _subject
                .WithKeyAgreementRole(keyAgreementRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesFfc(scheme, FfcParameterSet.Fb))
                .WithPartyId(thisPartyId)
                .BuildKdfKc()
                .WithKeyLength(keyLength)
                .WithMacParameters(macParams)
                .WithKeyConfirmationRole(keyConfirmationRole)
                .WithKeyConfirmationDirection(keyConfirmationDirection)
                .Build();

            kas.SetDomainParameters(domainParameters);
            kas.ReturnPublicInfoThisParty();

            if (kas.Scheme.StaticKeyPair != null)
            {
                kas.Scheme.StaticKeyPair.PrivateKeyX = thisPartyPrivateStaticKey;
                kas.Scheme.StaticKeyPair.PublicKeyY = thisPartyPublicStaticKey;
            }
            if (kas.Scheme.EphemeralKeyPair != null)
            {
                kas.Scheme.EphemeralKeyPair.PrivateKeyX = thisPartyPrivateEphemKey;
                kas.Scheme.EphemeralKeyPair.PublicKeyY = thisPartyPublicEphemKey;
            }

            var result = kas.ComputeResult(otherPartySharedInformation);

            Assume.That(result.Success, nameof(result.Success));
            Assert.AreEqual(expectedZ, result.Z, nameof(result.Z));
            Assert.AreEqual(expectedOi, result.Oi, nameof(result.Oi));
            Assert.AreEqual(expectedDkm, result.Dkm, nameof(result.Dkm));
            Assert.AreEqual(expectedMacData, result.MacData, nameof(result.MacData));
            Assert.AreEqual(expectedTag, result.Tag, nameof(result.Tag));
        }

    }
}
