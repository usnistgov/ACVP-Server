using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Builders.Ffc;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
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
                    new OtherInfoFactory<OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair>(
                        _entropyProviderOtherInfo
                    ),
                    _entropyProviderScheme,
                    new DiffieHellmanFfc(),
                    new MqvFfc()
                )
            );
        }

        #region dhEphem
        #region initiator, dhEphem, component only, sha512
        private static object[] _test_dhEphemComponentOnlySha512 = new object[]
        {
            new object[]
            {
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("a3a77cce3b0ea9891fe6ac34b2bdd04e22f9fd5a4976b5e2bd4c9ec43831c4d293779f3c4f826e6c2a8d6bd1ffca323b2360fcaa8bddc8c5268941578eede1f9447a39aaaa9af45bae4596b6df2a7048ce65bdd421ba055c640458abd4fdd07564df3a39ad6375a38dca884e5b67550bd60d789f5167935add6ae77af506e69d48eab2ebc1f17ff671c6d03d2f4f0e53e0ff1bdb488feca5d2b569f510242dd8bd64502c67ee8fe36224860a8b2934e864f75eff5fed4ecea69a1b2e6893df75ae19b266f4a55ccca2307038056aebfd212a4d5b540273d232c38d5cc6595216c3050cf4562989be8b341bd58c183e5e411939b4b34ad5752e87ffe622bd2075").ToPositiveBigInteger(),
                    // q
                    new BitString("f094f4fa8fa36fdcdf4f0378112bfde03cfa532e666b9736b5ab76e9").ToPositiveBigInteger(), 
                    // g
                    new BitString("45308211a07f231181276b44b873eb67726ca6aa5ecd39b4274f780409e15bfc98ac4680be5220a23b963e3b494602a80ce6cb6eb3f056e2a911ff7529f07fc53fa8840174698aac6a9dd540e86171cf2896a7337c0a839bfd9f24779c83f75b376da3c3c4d25d6b454e09dadbe230ee42115ae7ea79ace00b3c73bfd0c9913b0251177de4aae0ed54c041ff071346b2603360e5175faa9bbbc8fc50c5c657bba28da146674fa8a4f936da9d86511959785cd8e34c4b1f390b2cc68f574fd85e96e894d1b225ad43b3489af729c560b513a671e7fde2bd138fbd20605c74347e76ac50e230c57fec6dda275df29f770d47b91631e135778a51f3032bb1ef292f").ToPositiveBigInteger()
                ), 
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private ephem this party
                new BitString("a292aa176f00c2a6690fe22a0d402390cf308aed94d35c524b6ee04a").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("5067495e06d0c6dfd6eada5607f3dc7b9bf93eeeb7513119860f5d60ef332823b0ba58af2ecb7ab2cbccee87ccf232a02c27affb167e1a86811090262771c0fb5574c89ffb1288cd1d0096c0bb62add57fd3fa691ee1152b632778016a1a0c4ec2fce5ade1d3d1d2ae5c5a1a71e1a90dc648b384222e22357b8301536866d70b91a37ff3d88d444ed3e531b19939f3dfa33c4782ec195060cb35a13e0bad6f1f9c9be10720bb1055af93e16999c97d127fde52f16060080656810954f4cf745a57a3909327b2eaaa3ea5b9fa794f186658b186974861a00ee59b125c06398b835de09d2b340f7f0254e69339bdc4257b11543a300e1b8c615cd3a64838ce0d09").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("434156536964"),
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
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("a3a77cce3b0ea9891fe6ac34b2bdd04e22f9fd5a4976b5e2bd4c9ec43831c4d293779f3c4f826e6c2a8d6bd1ffca323b2360fcaa8bddc8c5268941578eede1f9447a39aaaa9af45bae4596b6df2a7048ce65bdd421ba055c640458abd4fdd07564df3a39ad6375a38dca884e5b67550bd60d789f5167935add6ae77af506e69d48eab2ebc1f17ff671c6d03d2f4f0e53e0ff1bdb488feca5d2b569f510242dd8bd64502c67ee8fe36224860a8b2934e864f75eff5fed4ecea69a1b2e6893df75ae19b266f4a55ccca2307038056aebfd212a4d5b540273d232c38d5cc6595216c3050cf4562989be8b341bd58c183e5e411939b4b34ad5752e87ffe622bd2075").ToPositiveBigInteger(),
                    // q
                    new BitString("f094f4fa8fa36fdcdf4f0378112bfde03cfa532e666b9736b5ab76e9").ToPositiveBigInteger(), 
                    // g
                    new BitString("45308211a07f231181276b44b873eb67726ca6aa5ecd39b4274f780409e15bfc98ac4680be5220a23b963e3b494602a80ce6cb6eb3f056e2a911ff7529f07fc53fa8840174698aac6a9dd540e86171cf2896a7337c0a839bfd9f24779c83f75b376da3c3c4d25d6b454e09dadbe230ee42115ae7ea79ace00b3c73bfd0c9913b0251177de4aae0ed54c041ff071346b2603360e5175faa9bbbc8fc50c5c657bba28da146674fa8a4f936da9d86511959785cd8e34c4b1f390b2cc68f574fd85e96e894d1b225ad43b3489af729c560b513a671e7fde2bd138fbd20605c74347e76ac50e230c57fec6dda275df29f770d47b91631e135778a51f3032bb1ef292f").ToPositiveBigInteger()
                ), 
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private ephem this party
                new BitString("a257e0ec23007768643135665f9315164bff024465e6da4aa9bfa44b").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("581a20e7d564dd30ff9e580055ebda3d02b3a60dc7cdf1102e3598bf2934a8ca9961242c58aa927e29755b2a3056050b691401ecb838c74d4cfd0d3c36c3e59df79d3190845b1b49a7a3cf4dba9afea8d3a9f907cf65119f6cc9c9cc8693d3bb52078a1c5f598fc3663a93ecf63231f59a96e11e539962d24dd721318ff7e9515ad759d997c6b09365388938f6b385c1c558baabf4d10ff94883dadf2af43b2b060699e77cec3320f4df883c747a7fd6842e16917ca841247b57fa2dc03cc2b6af9f78ef17a499b9a72c96ec413bd175b9caceafdda65992834afc941ec3097aa2ccdf0decbd6bbd3b19d767e6fa77a3ba2b591a06dc522165d2171a6e2ec2f1").ToPositiveBigInteger(), 
                // Other Party ID
                new BitString("434156536964"),
                // private ephem other party
                new BitString("ae60297d54ab210b59532e52d82e80c9c927b1a497c81eba42de5458").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("4bdea1f02522d9463166310ac6729a85c3e6435b2f324c9794da73f7ad0a48a07d840b249617a982791f2b8a2cf0442492555464a062dd529981c2d33893c90f62784b8706287fbe715827d5e158e222127f8be8c8607a3b7936ef807089d4e5727db7c3d966c528f89fc20b463c062a26263b9262655d3e0d5d610d78541be0c3000a6c905a1f915b0c0e166990887f544f952e5cd66c79df2cea930c7aedc6b2d12586404f1af21695cf82cb22344e65d904e4b0a855c3956e5fc327505fd6475d0db5ef495b88d825a22455896722936e8b1375a68054d882aa5a186330228540e54dbbf1932a78b7496113551e5d41b0dfdd0934f9f4e1c07e6ec922bac2").ToPositiveBigInteger(),
                // expected Z
                new BitString("0ed41716191aefc64a3ca52433171d1f33941af85dc4941cc0b80a656c1e9ee203108d97776f7c488190f48722b163369f328038edcef0ea4d2f78113eff2485dd83b5e8e71d14b360b7421d1526fdfb4205bfd1195a0adff82e98b458db76147e2a9d1d97c4f467f6d05cb8ee3a34ebb5b00cac90238c9dcea462640ec505c3487c3cff4fac9ecf226ea262abd50cc73606eb3526c2cd83838f5b36c2d50a5e94fc290fcd472e68d08bf308bcd7ced39f2da5cb30c77a57d73258fadd17a1796d0dd8d1b560e0be3c58b910ef8287875cc54a03f50ea3f1ebd455dbb1c609ce25e503c3799e9f15c473a3ec02cf0bb2f17597312bc59b3cc5c8f6bd985fdf4d"),
                // expected Z hash
                new BitString("b2c547e3ff33b9f1980cf486bac95faa6e2c6d7148976439c1e8839eca3d8e54ee2bff9ab812dffa263bd8c0d16c2b2cdca7c62fe35dfd021fbb3493de767088")
            }
        };

        [Test]
        [TestCaseSource(nameof(_test_dhEphemComponentOnlySha512))]
        public void ShouldDhEphemComponentOnlySha512Correctly(
            FfcDomainParameters domainParameters, 
            KeyAgreementRole keyAgreementRole, 
            BitString thisPartyId, 
            BigInteger thisPartyPrivateEphemKey, 
            BigInteger thisPartyPublicEphemKey, 
            BitString otherPartyId, 
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
                    new FfcKeyPair(0),
                    new FfcKeyPair(otherPartyPublicEphemKey), 
                    null, 
                    null, 
                    null
                );

            // The DSA sha mode determines which hash function to use in a component only test
            _dsa
                .SetupGet(s => s.Sha)
                .Returns(new ShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512)));
            _dsa
                .Setup(s => s.GenerateKeyPair(domainParameters))
                .Returns(new FfcKeyPairGenerateResult(new FfcKeyPair(thisPartyPrivateEphemKey, thisPartyPublicEphemKey)));

            var kas = _subject
                .WithKeyAgreementRole(keyAgreementRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesFfc(FfcScheme.DhEphem, FfcParameterSet.Fb))
                .WithAssurances(KasAssurance.None)
                .WithPartyId(thisPartyId)
                .BuildNoKdfNoKc()
                .Build();

            kas.SetDomainParameters(domainParameters);
            var result = kas.ComputeResult(vPartySharedInformation);

            Assume.That(result.Success, nameof(result.Success));
            Assert.AreEqual(expectedZ, result.Z, nameof(result.Z));
            Assert.AreEqual(expectedHashZ, result.Tag, nameof(result.Tag));
        }
        #endregion dhEphem, component only, sha512

        #region dhEphem, no key confirmation
        private static object[] _test_dhEphemNoKeyConfirmation = new object[]
        {
            #region hmac
            new object[]
            {
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("dbf19f26ff8c38df5b512782b8a8ae646aa0d2228627c6f367db86fd35b6ff9f040ca486b4a3189fe1c54370d93339cf48185efc320b7dd17325b22a019a3fe89516b27930744c3e13bf5b832d9f08e1660d4ebe45794207bd9b193aeb309414c1a22822f69bc70a0565a4937113addf9a5e533ea93770abb9e6cd0faec1a6e9a956654efd7fb3582381cfa2a23431ae86a1e321beb105518d9554e4a83573f59275ff88700d8f619274928fe3dd8281a2b81426037395a76342bfb828957f59d24daba2430d9713d8ae4b71cd61de1498563c0051334ff86ac66e8cbc13069afa597a60ac201378e4578d992f9441f99fd8b8659847d89ddcdf2b92e67e4c2f").ToPositiveBigInteger(),
                    // q
                    new BitString("c270a8242b98b35549e0fc9a47758cdb804ae69f84e7197e5fb6313b").ToPositiveBigInteger(), 
                    // g
                    new BitString("61c9f88be5d914fe0d513bb1fdc319c9420702d1c3b9caa01bfb277b764ddd3312f2067a1251817217e6ff02642dd4d289616f6812a9371b5e4916a714b4ddce38c7a268e6014041af1b4604830c1d9da7f8f16067935f09c5918a1fefed54268489d46f95ae482d76a2d2166a95a28e8d9fa25539238f13c785db59b97c7fc1f9208f30ff594c397a8f5e8532c114b91cbdad95547c78b4a7470cf29acf062f066d5b6160accbdc2ff5fa2b74a3d12acda578ecc8f8898054c5b8f3099f85023c4c4b32cea181e5792614e799693569e6cff8b6ca2753f5b94111ed86b21f8e98b11b37cef7da999223899fff20abd3aa9062d8c03655fbb7d726111641efae").ToPositiveBigInteger()
                ), 
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
                // private ephem this party
                new BitString("9a605c81ce070651179b45a80c85c11f016acebaef35acd86c6b8689").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("91e613404cd0008dc355938267ff694d322d5ca501b1fee1ad02dfc6e9a594d3a0c4c783b0ca9c356e767c7f88a6eaecc3af9cc3d1feb6eab9e931a01de77621f8384af85335e898f19feb761eb69a06e26d51d52454e80345b40f7ab0f6ea11a317df15c1f27b598a2fb2a2d785e901598aa67e7024969c3ed1a5360cac1055db70e437ab1c090343ccfe1bc5c9646f0632dbb11eae6f363843d03bb5c694f6c5e3d42cd5a725b915c52e19375bd8f5f175968cc96015180864e095d8ae76f9d529ca1f34a22ef16da42cd2bbba2093b7f75b3aa2b2dcc9a0bd562215bd333ca46b54553664958cd63dbce064345bcec96f64b6bdbe3f15ca4ce8c2be28ce43").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("434156536964"),
                // private ephem other party
                new BitString("33c07fcae0241ed4848b46ead94f1ce9225027a252e91f2200b3a59f").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("d875db9f7e9ffc6f92219187663b9d243b2b563685723836eaa801cf6ab5422503780ebada23bf8e55ec084c9e84ffc8024b274ac3d4a7a7a483708b7e18b4ba5437b2f295f8bf65dcd7cd87b1d37d64ade05b0853bc36ab8922835bf2139637bda7951745d6cfa8969cc6525d6203204d3f6d986266570147faa92dfab60ab28e743e35a6ef4211f38c66d0afcb0dbc4c78898ad591323fb2dec6e846056a935a6a0a58b5e96b44241d0f52f49e7fde9d44e6590e328541452c138b7f258a72336c281d62c444d081070c2400fdbcf29fb9cad138598e08fddc1de0220d0e8c2254799cd3291bff14e63667137ba00cdc278d3486d07ee06a4cf53696a602a3").ToPositiveBigInteger(),
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
                // inverse of previous test
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("dbf19f26ff8c38df5b512782b8a8ae646aa0d2228627c6f367db86fd35b6ff9f040ca486b4a3189fe1c54370d93339cf48185efc320b7dd17325b22a019a3fe89516b27930744c3e13bf5b832d9f08e1660d4ebe45794207bd9b193aeb309414c1a22822f69bc70a0565a4937113addf9a5e533ea93770abb9e6cd0faec1a6e9a956654efd7fb3582381cfa2a23431ae86a1e321beb105518d9554e4a83573f59275ff88700d8f619274928fe3dd8281a2b81426037395a76342bfb828957f59d24daba2430d9713d8ae4b71cd61de1498563c0051334ff86ac66e8cbc13069afa597a60ac201378e4578d992f9441f99fd8b8659847d89ddcdf2b92e67e4c2f").ToPositiveBigInteger(),
                    // q
                    new BitString("c270a8242b98b35549e0fc9a47758cdb804ae69f84e7197e5fb6313b").ToPositiveBigInteger(), 
                    // g
                    new BitString("61c9f88be5d914fe0d513bb1fdc319c9420702d1c3b9caa01bfb277b764ddd3312f2067a1251817217e6ff02642dd4d289616f6812a9371b5e4916a714b4ddce38c7a268e6014041af1b4604830c1d9da7f8f16067935f09c5918a1fefed54268489d46f95ae482d76a2d2166a95a28e8d9fa25539238f13c785db59b97c7fc1f9208f30ff594c397a8f5e8532c114b91cbdad95547c78b4a7470cf29acf062f066d5b6160accbdc2ff5fa2b74a3d12acda578ecc8f8898054c5b8f3099f85023c4c4b32cea181e5792614e799693569e6cff8b6ca2753f5b94111ed86b21f8e98b11b37cef7da999223899fff20abd3aa9062d8c03655fbb7d726111641efae").ToPositiveBigInteger()
                ), 
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
                // private ephem this party
                new BitString("33c07fcae0241ed4848b46ead94f1ce9225027a252e91f2200b3a59f").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("d875db9f7e9ffc6f92219187663b9d243b2b563685723836eaa801cf6ab5422503780ebada23bf8e55ec084c9e84ffc8024b274ac3d4a7a7a483708b7e18b4ba5437b2f295f8bf65dcd7cd87b1d37d64ade05b0853bc36ab8922835bf2139637bda7951745d6cfa8969cc6525d6203204d3f6d986266570147faa92dfab60ab28e743e35a6ef4211f38c66d0afcb0dbc4c78898ad591323fb2dec6e846056a935a6a0a58b5e96b44241d0f52f49e7fde9d44e6590e328541452c138b7f258a72336c281d62c444d081070c2400fdbcf29fb9cad138598e08fddc1de0220d0e8c2254799cd3291bff14e63667137ba00cdc278d3486d07ee06a4cf53696a602a3").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private ephem other party
                new BitString("9a605c81ce070651179b45a80c85c11f016acebaef35acd86c6b8689").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("91e613404cd0008dc355938267ff694d322d5ca501b1fee1ad02dfc6e9a594d3a0c4c783b0ca9c356e767c7f88a6eaecc3af9cc3d1feb6eab9e931a01de77621f8384af85335e898f19feb761eb69a06e26d51d52454e80345b40f7ab0f6ea11a317df15c1f27b598a2fb2a2d785e901598aa67e7024969c3ed1a5360cac1055db70e437ab1c090343ccfe1bc5c9646f0632dbb11eae6f363843d03bb5c694f6c5e3d42cd5a725b915c52e19375bd8f5f175968cc96015180864e095d8ae76f9d529ca1f34a22ef16da42cd2bbba2093b7f75b3aa2b2dcc9a0bd562215bd333ca46b54553664958cd63dbce064345bcec96f64b6bdbe3f15ca4ce8c2be28ce43").ToPositiveBigInteger(),
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
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("df892333ccd1d5b044fa9f0950ba860a2674a0ad6c9b9fce68172e6dad9fa2dbb83240b1a3ef6658606bc3055447553995d288d3076eddad82deb0b929cc5022827ccfdb294403a1848ab9e26cc4d0be6f5548f3b32c184cb2a14034265763bb74a708124d98e996f61aeca5a7f828fccafde96db3736b125a7db6f6a1c681b1377956e9e7682bd9e407c925f88a840073dc7bb894964be2be7f10a82f9be905024e490ad13942580c40b49721b21a70475fe896612b1d85971db7786dfc71bc445ad92320f8a6b658ee1915df6fe9dac46c9235ec3243a13eef86f598e1facf1fc2d90ee3850225e4a0c280e68c41b0ebdcff69e9ab2367b056bbf47d487c2b").ToPositiveBigInteger(),
                    // q
                    new BitString("9f1ab009208dd4c30838caf271dcba98a50a556c47a11738faafb45bbbdeadbf").ToPositiveBigInteger(), 
                    // g
                    new BitString("a6003a64e8274e64d3f64352d5983af4a9082f85b74461762d1d001f5624ee556d9fae2277a64614f64ff424089fda44ad2ed2d3d6f9c7f1fa45db2128ffd179a6be2485b16fd3facea0e396ce2650631c387d8abe4e8e2ebc90a355543301bf2ca3ddcf9b2d563d28663c1ed2a3411013e91d633f797877f8974d2648dd37197aec63aaaf674210c7849f0fd9b7d8331cd8b3b7d0f76895d7d65951f2c5d414a5d3289b444560c7a5ba1d94e6e011ec2da9341a86252d6ebad9514c0b24aab71e25084ea9ed33f8b66e622c198aa9fde243c636fce29004dc5c5b0bf3d1bdeb9510d374307aaf7f7e63583a9788bc9dd0df9045235ceed562e105bbb0cff992").ToPositiveBigInteger()
                ), 
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
                // private ephem this party
                new BitString("6eff9e56cc20c156555980739322aa1742f423a5016fbda2844822282511d2f0").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("58e2799a2f8b982158ffb28c16f21a68b457d1f397366d4d754a53f5146ca7a3cc9914c56962caee720bf269a1843a448fe24d85aa9f4b75498ca93ef6dcb293f786988bd61871c21529563c36626108ae1c2686364cc8c0401d95ecd1f31f27f82d4201cae8e7d7e7b47726e9fa0d557dbcfa7ce3a0a6f21ad4dcc290e63d998dca99b2bc6f4aaf785cb283a0cc923c406279d23e0bb36281f65989cd0a6363fa48fa5616e15d889b998792e21dfdabfd944646f610e1f6dd706c44b1c333f01cda17fc9b1f81cacf8d0a91779a4f6ea4e3849e492c30f0a096f2822d06f174e6f3dc741b3c4d792fe6be3a8aefe9f6999c15d983de96729e9f295ca78c9789").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("434156536964"),
                // private ephem other party
                new BitString("0564098c67915d2b66823755206978710087d9fc3096d62694cca73b6444a043").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("4fd088a8c4da7e08e9ebb33c9be4b18f2c90bc4f7a74358285586eef953884282af51cf80118cedda8c9b0cfe10346e94ecf7f6a8ccedd8bc8f0e7718a34aab5a6aae166e20be0e4e6670ee5af553c3aeb9bbb0ddb5e2985e804c6e80f79b487b912530604c85cfb8e278e334a94b186a9946aa6f46bfcfa56020ec0954d3a786598b03e7cd0cc63de9ab72a70eefd05446195b66d6748201f6aed3b3963dfe55004ad7af22ada15f23b56b21f1bdfe1fa1f1da5a7cf9123c080d0e44ee507e562be13081d2d399614f855e9ae9d7aa32b8761fb37899808dadc8e0f29a1368ff99e8a7d4c91354b09e0244fac78585cd281830647908442013464df5f127b02").ToPositiveBigInteger(),
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
                // inverse of previous test
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("df892333ccd1d5b044fa9f0950ba860a2674a0ad6c9b9fce68172e6dad9fa2dbb83240b1a3ef6658606bc3055447553995d288d3076eddad82deb0b929cc5022827ccfdb294403a1848ab9e26cc4d0be6f5548f3b32c184cb2a14034265763bb74a708124d98e996f61aeca5a7f828fccafde96db3736b125a7db6f6a1c681b1377956e9e7682bd9e407c925f88a840073dc7bb894964be2be7f10a82f9be905024e490ad13942580c40b49721b21a70475fe896612b1d85971db7786dfc71bc445ad92320f8a6b658ee1915df6fe9dac46c9235ec3243a13eef86f598e1facf1fc2d90ee3850225e4a0c280e68c41b0ebdcff69e9ab2367b056bbf47d487c2b").ToPositiveBigInteger(),
                    // q
                    new BitString("9f1ab009208dd4c30838caf271dcba98a50a556c47a11738faafb45bbbdeadbf").ToPositiveBigInteger(), 
                    // g
                    new BitString("a6003a64e8274e64d3f64352d5983af4a9082f85b74461762d1d001f5624ee556d9fae2277a64614f64ff424089fda44ad2ed2d3d6f9c7f1fa45db2128ffd179a6be2485b16fd3facea0e396ce2650631c387d8abe4e8e2ebc90a355543301bf2ca3ddcf9b2d563d28663c1ed2a3411013e91d633f797877f8974d2648dd37197aec63aaaf674210c7849f0fd9b7d8331cd8b3b7d0f76895d7d65951f2c5d414a5d3289b444560c7a5ba1d94e6e011ec2da9341a86252d6ebad9514c0b24aab71e25084ea9ed33f8b66e622c198aa9fde243c636fce29004dc5c5b0bf3d1bdeb9510d374307aaf7f7e63583a9788bc9dd0df9045235ceed562e105bbb0cff992").ToPositiveBigInteger()
                ), 
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
                // private ephem this party
                new BitString("0564098c67915d2b66823755206978710087d9fc3096d62694cca73b6444a043").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("4fd088a8c4da7e08e9ebb33c9be4b18f2c90bc4f7a74358285586eef953884282af51cf80118cedda8c9b0cfe10346e94ecf7f6a8ccedd8bc8f0e7718a34aab5a6aae166e20be0e4e6670ee5af553c3aeb9bbb0ddb5e2985e804c6e80f79b487b912530604c85cfb8e278e334a94b186a9946aa6f46bfcfa56020ec0954d3a786598b03e7cd0cc63de9ab72a70eefd05446195b66d6748201f6aed3b3963dfe55004ad7af22ada15f23b56b21f1bdfe1fa1f1da5a7cf9123c080d0e44ee507e562be13081d2d399614f855e9ae9d7aa32b8761fb37899808dadc8e0f29a1368ff99e8a7d4c91354b09e0244fac78585cd281830647908442013464df5f127b02").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private ephem other party
                new BitString("6eff9e56cc20c156555980739322aa1742f423a5016fbda2844822282511d2f0").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("58e2799a2f8b982158ffb28c16f21a68b457d1f397366d4d754a53f5146ca7a3cc9914c56962caee720bf269a1843a448fe24d85aa9f4b75498ca93ef6dcb293f786988bd61871c21529563c36626108ae1c2686364cc8c0401d95ecd1f31f27f82d4201cae8e7d7e7b47726e9fa0d557dbcfa7ce3a0a6f21ad4dcc290e63d998dca99b2bc6f4aaf785cb283a0cc923c406279d23e0bb36281f65989cd0a6363fa48fa5616e15d889b998792e21dfdabfd944646f610e1f6dd706c44b1c333f01cda17fc9b1f81cacf8d0a91779a4f6ea4e3849e492c30f0a096f2822d06f174e6f3dc741b3c4d792fe6be3a8aefe9f6999c15d983de96729e9f295ca78c9789").ToPositiveBigInteger(),
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
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d09d74eece8be7fdff51dc238636e5207631a584c07c96f47d55a4666d28286db309c6107d8c264358ab091946ff2a1bb79a59eced4467e82a582131d1caf4fbb684d952a749cdf0ad80202b03fa4ecfd85baeb01effb593bc93bf48123b465711431fc11cae1f134ec51cd4b44331a09bc002075542b37f986de757205bd7007b3a3cdd718e9db29780bbc529308af5b9e52096c927316f03761628215c987a3768703a00934580522181ae27a743a7df71cdec20822279ec4283db5ab30a7a52a04f913fb3c5d7f8897993db1dc8a99bc226c8271e3574d1940f4954eb4475f4c77a2a61aa1920507bc152c7331a57dc3431bc7aaadab97958684c9d6028ed").ToPositiveBigInteger(),
                    // q
                    new BitString("8eca313f1b0c9211f5fa2aec825e99bf7a930f1250af846c74dc63df").ToPositiveBigInteger(), 
                    // g
                    new BitString("005b768d45d72e17a87e8b1b34d748066159bd0b6abc802364978fa50a269fba4aead7a2b648b710441b4f99241b1454e10146c217607ef4f19d4a2eed35d3fe29e80b434e43d0c6c016b4c700f780a1b4da0ec03db827ca1b1d583624188a1e21817bf56aef84d0ee1fae7b694a97db9bfbf32027326c7a0add10636090810831920520dd7cfa9f91362c80d34c0e900dcd7d6e234ba45d7af1bcab581d7b9946e4ed7008e7404d01262a9ce8a47d339b42fff35a32bd232a431d1fbae6c27b146e6039fbff5890cd4372e81d2dfed40223e82c90aca36bb4f513574d53c3b3eafe566cccbc77c1255985ca73e91afd0498401f22157a6ea88322ad293512c4").ToPositiveBigInteger()
                ), 
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
                // private ephem this party
                new BitString("5e1a7ca61ee4759ba46144cb873ee1ffccc155237df04002efe8c173").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("0d5745eb14a277033fa9db517ecb323ee3997b0b0b230bf528429d4c64cbc7b3c5a141cedc6caca65388d199efcd4ae719cb22160f013bd94a518339361f2ed685599a6f5af16700ac4ff744b37e9abe2d9a4def314308b77eabe0b0af0c3779ea8eecf2ab8d31c81a8120b3cadff1d29e866892b21534daf36417e841e49ad55e7d13e4690bc49c68dbb8667a86c150110b3c05dc1bda7d8a4841539acd25dc32ab822bf045b2789061c2c9db3e4683cf1853a28e4a9af99d0ba66aca315e9ddb3948e756771f0e780da7b5fdfd30ea8e356508199a8ff750089aaa09a8104379c8e2fd9e058694fad10d74de790ebf39dcebe576fd75f6a3c8d35d7161a072").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("434156536964"),
                // private ephem other party
                new BitString("236658b70be16ef58355b1de4b0a4c685737435a25bf84b773065159").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("7c6191d1fcc854d0d6dd8a7b69abf10a9fa720d106b087277a1bd03adc6a28c113e082d649870e126c2db976dd97afdbfa74b106e192d6fd053d5cc26c707d474513673d68dbc8d5b17976ba8d2c53414900c06c9fa3dfd5eb380db50cb0d735cb0644277411f5d799a683d5fafa0d2536416d01824f3dd55df2c4dc09194a6fe17a107b15e57fdf91ee2c6cfe3203ca1b8d3975acf3df42f2147f1c7888659602e5ebe68f44426ba2eacccd4a7c1153001c3130fd02dc741c5e847c0c3b012a7d13e3b9eecefa35a9bb40e47713c7f4871dff44b5f6a23ae13d2be0dab0968ce9fb6423ba71e1803ec099ffe32b55d4b70c15957edd04cf9c25d80dc8fc5ef5").ToPositiveBigInteger(),
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
                // inverse of previous test
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d09d74eece8be7fdff51dc238636e5207631a584c07c96f47d55a4666d28286db309c6107d8c264358ab091946ff2a1bb79a59eced4467e82a582131d1caf4fbb684d952a749cdf0ad80202b03fa4ecfd85baeb01effb593bc93bf48123b465711431fc11cae1f134ec51cd4b44331a09bc002075542b37f986de757205bd7007b3a3cdd718e9db29780bbc529308af5b9e52096c927316f03761628215c987a3768703a00934580522181ae27a743a7df71cdec20822279ec4283db5ab30a7a52a04f913fb3c5d7f8897993db1dc8a99bc226c8271e3574d1940f4954eb4475f4c77a2a61aa1920507bc152c7331a57dc3431bc7aaadab97958684c9d6028ed").ToPositiveBigInteger(),
                    // q
                    new BitString("8eca313f1b0c9211f5fa2aec825e99bf7a930f1250af846c74dc63df").ToPositiveBigInteger(), 
                    // g
                    new BitString("005b768d45d72e17a87e8b1b34d748066159bd0b6abc802364978fa50a269fba4aead7a2b648b710441b4f99241b1454e10146c217607ef4f19d4a2eed35d3fe29e80b434e43d0c6c016b4c700f780a1b4da0ec03db827ca1b1d583624188a1e21817bf56aef84d0ee1fae7b694a97db9bfbf32027326c7a0add10636090810831920520dd7cfa9f91362c80d34c0e900dcd7d6e234ba45d7af1bcab581d7b9946e4ed7008e7404d01262a9ce8a47d339b42fff35a32bd232a431d1fbae6c27b146e6039fbff5890cd4372e81d2dfed40223e82c90aca36bb4f513574d53c3b3eafe566cccbc77c1255985ca73e91afd0498401f22157a6ea88322ad293512c4").ToPositiveBigInteger()
                ), 
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
                // private ephem this party
                new BitString("236658b70be16ef58355b1de4b0a4c685737435a25bf84b773065159").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("7c6191d1fcc854d0d6dd8a7b69abf10a9fa720d106b087277a1bd03adc6a28c113e082d649870e126c2db976dd97afdbfa74b106e192d6fd053d5cc26c707d474513673d68dbc8d5b17976ba8d2c53414900c06c9fa3dfd5eb380db50cb0d735cb0644277411f5d799a683d5fafa0d2536416d01824f3dd55df2c4dc09194a6fe17a107b15e57fdf91ee2c6cfe3203ca1b8d3975acf3df42f2147f1c7888659602e5ebe68f44426ba2eacccd4a7c1153001c3130fd02dc741c5e847c0c3b012a7d13e3b9eecefa35a9bb40e47713c7f4871dff44b5f6a23ae13d2be0dab0968ce9fb6423ba71e1803ec099ffe32b55d4b70c15957edd04cf9c25d80dc8fc5ef5").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private ephem other party
                new BitString("5e1a7ca61ee4759ba46144cb873ee1ffccc155237df04002efe8c173").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("0d5745eb14a277033fa9db517ecb323ee3997b0b0b230bf528429d4c64cbc7b3c5a141cedc6caca65388d199efcd4ae719cb22160f013bd94a518339361f2ed685599a6f5af16700ac4ff744b37e9abe2d9a4def314308b77eabe0b0af0c3779ea8eecf2ab8d31c81a8120b3cadff1d29e866892b21534daf36417e841e49ad55e7d13e4690bc49c68dbb8667a86c150110b3c05dc1bda7d8a4841539acd25dc32ab822bf045b2789061c2c9db3e4683cf1853a28e4a9af99d0ba66aca315e9ddb3948e756771f0e780da7b5fdfd30ea8e356508199a8ff750089aaa09a8104379c8e2fd9e058694fad10d74de790ebf39dcebe576fd75f6a3c8d35d7161a072").ToPositiveBigInteger(),
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
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d5cf9ba288ff8438650904a5fe2eeb8bf6b52b691a455b21bd3b37998f82544036ae61fe436039c66feab83ba21a5ba13e7b4c1692ac82c65309eed54e593efe9831cc82bfd11e9552d4eb7d8f2c233024bc10819c57093890ec19aa9ee915e524d81c285928a5b87acc7f496f93689ed59b15183689ec5e487fdc9fb994c4bf7e1dba57f8f12c17e404f68b5506d69f4aa98146f6ba3be1f3397e5d41c4235a18b38c75417bb9091a487db1fc89eb2f73b4ac6f6c12dc5634a342cd9d10605090bd544e1c0813aaaa3814166df84a302b8df38babe9c31e4a2c64317046bdb841fa8717e22dcf1496f9ad94c17f1ff9175567d02ab5e30601e2e5b0884e9a77").ToPositiveBigInteger(),
                    // q
                    new BitString("ceb9916bbc14cdc9dda80481135bee68ee94f4ecadc2921261a316d1c9cf9283").ToPositiveBigInteger(), 
                    // g
                    new BitString("37a7b2a094ed6253784d51c71a4a7407f0727df15e480a02b0ff2f9befdb0e92c0d63e482c6909e5337373abe347ceaec25a9f2a23f1770c2447bfb5c35dbda80d3f00d8b6569a0165a3d52f1cff03e02b67be47b2f26ebd0fc13299fd0317719c3bcbbcbba0a982915b5d68fb4c5c483c3df12052b56ceacd16176c783d56422b1366cd5ce65922b734a1780acf35b4a658cfe8166469ace04b87a33e1e8d4603ae3f9607d1e708137d581aaf2cd86608ca06e52cddbe475f8bdb4597cd5b55f47380276c86d08d8ddbbef4ee3ca76cefe6ff9c74d4d5d1be3407e30720be34cf41f482be8bc6ed6e89ea7cc2d164da1dc1c9d0e2762eef983dc9e96f3bfd50").ToPositiveBigInteger()
                ), 
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
                // private ephem this party
                new BitString("41f8582a2f3ac19cc925fb5cf72ec98f89630fec8e71853397870c3fda1eb08b").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("a6f3b734391112b2ae81cb3021bc1275383c4a6c80b9a1265ae3f5a8256a0a621667f71e3e081b86bc427d5adddb901aacd4e3b7b48170c7d13cd9dccd7b04072d97a7c39fc44c079a48fa3d2495e90659786275f13b5ad2402509dd12637630de980369c5e55953091df4ef3e369880547737df1d7bd7a9d437488cb01a94b444c05ae33d6ae6126bd0c9c7be19adc752f210f68ee7657a9151ef3299b4e4e12f19ba9f3070cacfe1af329e69a2bd6416c51c7d33b183702dbd20df91ca09c5190321f59f55fca903546cc42c3edcb3c4eb30b3a4228fe2e1906484174715e0b08d9d93c4588c33739f416ca45a33faa861dc6e9fb7831cc95d2d2741533be3").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("434156536964"),
                // private ephem other party
                new BitString("a388f6b35f25619d1e76ed7cc6d10f8f3a6d919e6560c3f0090eb96e1f27d908").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("4fc5ed02abf0791bb01ad33527bd16ee58f875bbc85de5b513ccce95d05d6c735e726a102f11a69152f1c430b027ef3f2049e07db96f448d9b016ebd9258cd33175c06c00d107c99ccfebc8e77b19c61ed7c9e1d8a3e912a893b30ec9ab1d59d2bd5133c9669090c7bcc48318a21cabffbe56477e9fda36aba9c5e4462854fa682f503de3e895579b0c51a2f2a6b1fc59bee015e8bd89041a2efafbe8ea491215a88302f6a4858ea3c0d846e52f6ea070b61dda82009bdf0ed13a077ea1777233e059fed4c45249c5bbba1fbaa6f574058ec58d56e6b3a486e0aa306ef2d63d70617f6b384fe238e82832dfeff6317aa887aab894efc9b60e6f5127581fe735c").ToPositiveBigInteger(),
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
                // inverse of previous test
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d5cf9ba288ff8438650904a5fe2eeb8bf6b52b691a455b21bd3b37998f82544036ae61fe436039c66feab83ba21a5ba13e7b4c1692ac82c65309eed54e593efe9831cc82bfd11e9552d4eb7d8f2c233024bc10819c57093890ec19aa9ee915e524d81c285928a5b87acc7f496f93689ed59b15183689ec5e487fdc9fb994c4bf7e1dba57f8f12c17e404f68b5506d69f4aa98146f6ba3be1f3397e5d41c4235a18b38c75417bb9091a487db1fc89eb2f73b4ac6f6c12dc5634a342cd9d10605090bd544e1c0813aaaa3814166df84a302b8df38babe9c31e4a2c64317046bdb841fa8717e22dcf1496f9ad94c17f1ff9175567d02ab5e30601e2e5b0884e9a77").ToPositiveBigInteger(),
                    // q
                    new BitString("ceb9916bbc14cdc9dda80481135bee68ee94f4ecadc2921261a316d1c9cf9283").ToPositiveBigInteger(), 
                    // g
                    new BitString("37a7b2a094ed6253784d51c71a4a7407f0727df15e480a02b0ff2f9befdb0e92c0d63e482c6909e5337373abe347ceaec25a9f2a23f1770c2447bfb5c35dbda80d3f00d8b6569a0165a3d52f1cff03e02b67be47b2f26ebd0fc13299fd0317719c3bcbbcbba0a982915b5d68fb4c5c483c3df12052b56ceacd16176c783d56422b1366cd5ce65922b734a1780acf35b4a658cfe8166469ace04b87a33e1e8d4603ae3f9607d1e708137d581aaf2cd86608ca06e52cddbe475f8bdb4597cd5b55f47380276c86d08d8ddbbef4ee3ca76cefe6ff9c74d4d5d1be3407e30720be34cf41f482be8bc6ed6e89ea7cc2d164da1dc1c9d0e2762eef983dc9e96f3bfd50").ToPositiveBigInteger()
                ), 
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
                // private ephem this party
                new BitString("a388f6b35f25619d1e76ed7cc6d10f8f3a6d919e6560c3f0090eb96e1f27d908").ToPositiveBigInteger(),
                // public ephem this party
                new BitString("4fc5ed02abf0791bb01ad33527bd16ee58f875bbc85de5b513ccce95d05d6c735e726a102f11a69152f1c430b027ef3f2049e07db96f448d9b016ebd9258cd33175c06c00d107c99ccfebc8e77b19c61ed7c9e1d8a3e912a893b30ec9ab1d59d2bd5133c9669090c7bcc48318a21cabffbe56477e9fda36aba9c5e4462854fa682f503de3e895579b0c51a2f2a6b1fc59bee015e8bd89041a2efafbe8ea491215a88302f6a4858ea3c0d846e52f6ea070b61dda82009bdf0ed13a077ea1777233e059fed4c45249c5bbba1fbaa6f574058ec58d56e6b3a486e0aa306ef2d63d70617f6b384fe238e82832dfeff6317aa887aab894efc9b60e6f5127581fe735c").ToPositiveBigInteger(),
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private ephem other party
                new BitString("41f8582a2f3ac19cc925fb5cf72ec98f89630fec8e71853397870c3fda1eb08b").ToPositiveBigInteger(), 
                // public ephem other party
                new BitString("a6f3b734391112b2ae81cb3021bc1275383c4a6c80b9a1265ae3f5a8256a0a621667f71e3e081b86bc427d5adddb901aacd4e3b7b48170c7d13cd9dccd7b04072d97a7c39fc44c079a48fa3d2495e90659786275f13b5ad2402509dd12637630de980369c5e55953091df4ef3e369880547737df1d7bd7a9d437488cb01a94b444c05ae33d6ae6126bd0c9c7be19adc752f210f68ee7657a9151ef3299b4e4e12f19ba9f3070cacfe1af329e69a2bd6416c51c7d33b183702dbd20df91ca09c5190321f59f55fca903546cc42c3edcb3c4eb30b3a4228fe2e1906484174715e0b08d9d93c4588c33739f416ca45a33faa861dc6e9fb7831cc95d2d2741533be3").ToPositiveBigInteger(),
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
        };

        [Test]
        [TestCaseSource(nameof(_test_dhEphemNoKeyConfirmation))]
        public void ShouldDhEphemNoKeyConfirmationCorrectly(
                    FfcDomainParameters domainParameters,
                    KeyAgreementRole keyAgreementRole,
                    ModeValues dsaKdfHashMode,
                    DigestSizes dsaKdfDigestSize,
                    KeyAgreementMacType macType,
                    int keyLength,
                    int tagLength,
                    BitString noKeyConfirmationNonce,
                    BitString aesCcmNonce,
                    BitString thisPartyId,
                    BigInteger thisPartyPrivateEphemKey,
                    BigInteger thisPartyPublicEphemKey,
                    BitString otherPartyId,
                    BigInteger otherPartyPrivateEphemKey,
                    BigInteger otherPartyPublicEphemKey,
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
                    new FfcKeyPair(0), 
                    new FfcKeyPair(otherPartyPublicEphemKey),
                    null,
                    null,
                    // when "party v" noKeyConfirmationNonce is provided as a part of party U's shared information
                    keyAgreementRole == KeyAgreementRole.ResponderPartyV ? noKeyConfirmationNonce : null
                );

            // u/v party info comprised of ID, and dkmNonce (when available), find the bitlength of both parties contributed information 
            // to determine which bits are the "random" bits to inject into the TestableEntropyProvider.
            var composedBitLength = thisPartyId.BitLength +
                                    new BitString(0).BitLength + // DKM nonce when applicable
                                    otherPartyId.BitLength;

            var entropyBits = expectedOi.GetLeastSignificantBits(expectedOi.BitLength - composedBitLength);

            _entropyProviderOtherInfo.AddEntropy(entropyBits);

            // MAC no key confirmation data makes use of a nonce
            _entropyProviderScheme.AddEntropy(noKeyConfirmationNonce);

            // The DSA sha mode determines which hash function to use in a component only test
            _dsa
                .SetupGet(s => s.Sha)
                .Returns(new ShaFactory().GetShaInstance(new HashFunction(dsaKdfHashMode, dsaKdfDigestSize)));
            _dsa
                .Setup(s => s.GenerateKeyPair(domainParameters))
                .Returns(new FfcKeyPairGenerateResult(new FfcKeyPair(thisPartyPrivateEphemKey, thisPartyPublicEphemKey)));

            var macParams = _macParamsBuilder
                .WithKeyAgreementMacType(macType)
                .WithMacLength(tagLength)
                .WithNonce(aesCcmNonce)
                .Build();

            var kas = _subject
                .WithKeyAgreementRole(keyAgreementRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesFfc(FfcScheme.DhEphem, FfcParameterSet.Fb))
                .WithPartyId(thisPartyId)
                .BuildKdfNoKc()
                .WithKeyLength(keyLength)
                .WithMacParameters(macParams)
                .Build();

            kas.SetDomainParameters(domainParameters);
            var result = kas.ComputeResult(otherPartySharedInformation);

            Assume.That(result.Success, nameof(result.Success));
            Assert.AreEqual(expectedZ, result.Z, nameof(result.Z));
            Assert.AreEqual(expectedOi, result.Oi, nameof(result.Oi));
            Assert.AreEqual(expectedDkm, result.Dkm, nameof(result.Dkm));
            Assert.AreEqual(expectedMacData, result.MacData, nameof(result.MacData));
            Assert.AreEqual(expectedTag, result.Tag, nameof(result.Tag));
        }
        #endregion dhEphem, no key confirmation
        #endregion dhEphem

        #region mqv1
        #region mqv1, component only
        private static object[] _test_mqv1ComponentOnlySha256 = new object[]
        {
            new object[]
            {
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d1ad57e07b69aeeffaf5395bce38fca1e006d94e6c74f0571b32a0ddaf85558b88b960bc18260c0bce051e488cb06adc7eced487726efa9a0f5ab7c888145759667329930c519a29a500430be5a918fc969e776a33f85b1a064a80b7f648e65ff2ce8850029400799dc6bdc8f9835a496d55619cd95e09a77ef37f1b7377cb38210f85ad80d251eedccbed0adea17f1b287c08a6bdd3fc0a55f07b436c1dddfceb4cd6ea3a5522da0b52d647b7bc2008cd53b5f5e26c6b0799b84876b5e39bff722c961f920c4133ed6dcc496eecc503142315dcdb48c443fd7d7d9d5a687be8b43a5c43b776468d818d6170d5ffcbb7707dd14b9f35f89d37b1102e2579e71d").ToPositiveBigInteger(),
                    // q
                    new BitString("8ca04d69cc6464718f281b44b5151628b33a0af263cf0038988b84cd0c47401b").ToPositiveBigInteger(), 
                    // g
                    new BitString("3ef102354d9b8342f2f2637e08daf68879075c979c7a84209e8e3f38d7741b4a7fc99d51ddd4ed083a9f3f3b234c6b5302280852199cfee149aae278dd0501adca1fc5cfb3c73891adea92021fa0d3eb6597b6596475dce61e5865e76d2f87b6070c7add0592833825775b888117eb518ea4e0ddfa2ac725a65fb1ea401be0b2b90526918b50c875e5581e00b9787f52f975393916ce9d6aea877343d2359a0cf15a97d6c0076596eb3545d4c527721db99b94b5c2539e4dfedcd9b23797deb5b61657e0a0d13789d0ebbc2b61bbc63f79608d58a782f3c8cab6afaa85ff92ea41113f95e7c8fa915e27bc14461d2dae784825576a4ef2535a27c2f17ab1e23c").ToPositiveBigInteger()
                ), 
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
            // other party perspective of previous test
            new object[]
            {
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d1ad57e07b69aeeffaf5395bce38fca1e006d94e6c74f0571b32a0ddaf85558b88b960bc18260c0bce051e488cb06adc7eced487726efa9a0f5ab7c888145759667329930c519a29a500430be5a918fc969e776a33f85b1a064a80b7f648e65ff2ce8850029400799dc6bdc8f9835a496d55619cd95e09a77ef37f1b7377cb38210f85ad80d251eedccbed0adea17f1b287c08a6bdd3fc0a55f07b436c1dddfceb4cd6ea3a5522da0b52d647b7bc2008cd53b5f5e26c6b0799b84876b5e39bff722c961f920c4133ed6dcc496eecc503142315dcdb48c443fd7d7d9d5a687be8b43a5c43b776468d818d6170d5ffcbb7707dd14b9f35f89d37b1102e2579e71d").ToPositiveBigInteger(),
                    // q
                    new BitString("8ca04d69cc6464718f281b44b5151628b33a0af263cf0038988b84cd0c47401b").ToPositiveBigInteger(), 
                    // g
                    new BitString("3ef102354d9b8342f2f2637e08daf68879075c979c7a84209e8e3f38d7741b4a7fc99d51ddd4ed083a9f3f3b234c6b5302280852199cfee149aae278dd0501adca1fc5cfb3c73891adea92021fa0d3eb6597b6596475dce61e5865e76d2f87b6070c7add0592833825775b888117eb518ea4e0ddfa2ac725a65fb1ea401be0b2b90526918b50c875e5581e00b9787f52f975393916ce9d6aea877343d2359a0cf15a97d6c0076596eb3545d4c527721db99b94b5c2539e4dfedcd9b23797deb5b61657e0a0d13789d0ebbc2b61bbc63f79608d58a782f3c8cab6afaa85ff92ea41113f95e7c8fa915e27bc14461d2dae784825576a4ef2535a27c2f17ab1e23c").ToPositiveBigInteger()
                ), 
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
            }
        };

        [Test]
        [TestCaseSource(nameof(_test_mqv1ComponentOnlySha256))]
        public void ShouldMqv1ComponentOnlySha256Correctly(
            FfcDomainParameters domainParameters, 
            KeyAgreementRole keyAgreementRole, 
            BitString thisPartyId,
            BigInteger thisPartyPrivateStaticKey,
            BigInteger thisPartyPublicStaticKey,
            BigInteger thisPartyPrivateEphemKey, 
            BigInteger thisPartyPublicEphemKey, 
            BitString otherPartyId, 
            BigInteger otherPartyPrivateStaticKey, 
            BigInteger otherPartyPublicStaticKey,
            BigInteger otherPartyPrivateEphemeralKey,
            BigInteger otherPartyPublicEphemeralKey,
            BitString expectedZ, 
            BitString expectedHashZ
        )
        {
            var vPartySharedInformation =
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(
                    domainParameters,
                    otherPartyId,
                    new FfcKeyPair(otherPartyPublicStaticKey),
                        new FfcKeyPair(otherPartyPublicEphemeralKey),
                    null,
                    null,
                    null
                );

            FfcDsa dsa = new FfcDsa(new ShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256)));
            _dsaFactory
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), EntropyProviderTypes.Random))
                .Returns(dsa);
                
            var kas = _subject
                .WithKeyAgreementRole(keyAgreementRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesFfc(FfcScheme.Mqv1, FfcParameterSet.Fc))
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
        #endregion mqv1, component only

        #region dhEphem, no key confirmation
        private static object[] _test_mqv1NoKeyConfirmation = new object[]
        {
            #region hmac
            new object[]
            {
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("a767083605e3ec0bca252ac9a2106dced20fe16eb3236cd79f8ff1d543e6969edf4aa06c432ae8504f5db67fa0f54acb8d6f36fe2d64a7ead26895a8e24606604ec0aff5f31327a2fec732c3ba4c3ddee6e50cc0318e2c7facb694f2d71a22ad97a500196e459c60d670280163bd5f11c6f80a67b56b20d87145e3b11015a7e2f090d81ca65fb97f3a2523fe3a47559651e4b03b599b363b9a78e62bf8ab67f657e545722c5372d8e22dc073abf009065032a02f3362981dbe402656856dbda4421fe5b14c9402820065f6c66d43edc97a581f5cd53748ba51c0f29779041c13b67bbe6077cb90a205cf20abd9b424cfc2e9cf788f52aa5c41206d015c321d43").ToPositiveBigInteger(),
                    // q
                    new BitString("b1b6763b3a80ff88d040211ffb88ff738fca5c695afab6d81256f633").ToPositiveBigInteger(), 
                    // g
                    new BitString("0c4799e69534512c79b1f8655883e3ad8d7cf19b607ba04a33ee68327e910d669df3c83b54ac08050ea5d4dfcc4b8caa4974730a39506f4056765123a992c65ba384be9ba2d661a9b5f05d518d7071f4de8267a396a144ffd71aecc94496ac5c9493cb02bd21b06deb8ecc9eb0a855a597229c5ada597e3bc74dcf53180929c0f8eea0e88790314c837ed5bf016bd0ba9543d0c2e65ea968b0df7171037e7df565192d6fd5f1f6db1da7cd7c5a79b3ef564f72629325d5ae56aa24331b77be766fb6c5921c82eec16341f67dd4e1fc1e14fe8810007e8473e2b5b43c730181871d23b5885bca77ada994c4e71e9f8f881c1c4229b0fb10577cd2b38aa4a4edbb").ToPositiveBigInteger()
                ), 
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
                // inverse of previous test
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("a767083605e3ec0bca252ac9a2106dced20fe16eb3236cd79f8ff1d543e6969edf4aa06c432ae8504f5db67fa0f54acb8d6f36fe2d64a7ead26895a8e24606604ec0aff5f31327a2fec732c3ba4c3ddee6e50cc0318e2c7facb694f2d71a22ad97a500196e459c60d670280163bd5f11c6f80a67b56b20d87145e3b11015a7e2f090d81ca65fb97f3a2523fe3a47559651e4b03b599b363b9a78e62bf8ab67f657e545722c5372d8e22dc073abf009065032a02f3362981dbe402656856dbda4421fe5b14c9402820065f6c66d43edc97a581f5cd53748ba51c0f29779041c13b67bbe6077cb90a205cf20abd9b424cfc2e9cf788f52aa5c41206d015c321d43").ToPositiveBigInteger(),
                    // q
                    new BitString("b1b6763b3a80ff88d040211ffb88ff738fca5c695afab6d81256f633").ToPositiveBigInteger(), 
                    // g
                    new BitString("0c4799e69534512c79b1f8655883e3ad8d7cf19b607ba04a33ee68327e910d669df3c83b54ac08050ea5d4dfcc4b8caa4974730a39506f4056765123a992c65ba384be9ba2d661a9b5f05d518d7071f4de8267a396a144ffd71aecc94496ac5c9493cb02bd21b06deb8ecc9eb0a855a597229c5ada597e3bc74dcf53180929c0f8eea0e88790314c837ed5bf016bd0ba9543d0c2e65ea968b0df7171037e7df565192d6fd5f1f6db1da7cd7c5a79b3ef564f72629325d5ae56aa24331b77be766fb6c5921c82eec16341f67dd4e1fc1e14fe8810007e8473e2b5b43c730181871d23b5885bca77ada994c4e71e9f8f881c1c4229b0fb10577cd2b38aa4a4edbb").ToPositiveBigInteger()
                ), 
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
            #region cmac
            
            #endregion cmac
            #region aes-ccm
            
            #endregion aes-ccm
        };

        [Test]
        [TestCaseSource(nameof(_test_mqv1NoKeyConfirmation))]
        public void ShouldMqv1NoKeyConfirmationCorrectly(
            FfcDomainParameters domainParameters,
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
            BitString otherPartyId,
            BigInteger otherPartyPrivateStaticKey,
            BigInteger otherPartyPublicStaticKey,
            BigInteger otherPartyPrivateEphemKey,
            BigInteger otherPartyPublicEphemKey,
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
                    null,
                    null,
                    // when "party v" noKeyConfirmationNonce is provided as a part of party U's shared information
                    keyAgreementRole == KeyAgreementRole.ResponderPartyV ? noKeyConfirmationNonce : null
                );

            // u/v party info comprised of ID, and dkmNonce (when available), find the bitlength of both parties contributed information 
            // to determine which bits are the "random" bits to inject into the TestableEntropyProvider.
            var composedBitLength = thisPartyId.BitLength +
                                    new BitString(0).BitLength + // DKM nonce when applicable
                                    otherPartyId.BitLength;

            var entropyBits = expectedOi.GetLeastSignificantBits(expectedOi.BitLength - composedBitLength);

            _entropyProviderOtherInfo.AddEntropy(entropyBits);

            // MAC no key confirmation data makes use of a nonce
            _entropyProviderScheme.AddEntropy(noKeyConfirmationNonce);

            FfcDsa dsa = new FfcDsa(new ShaFactory().GetShaInstance(new HashFunction(dsaKdfHashMode, dsaKdfDigestSize)));
            _dsaFactory
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), EntropyProviderTypes.Random))
                .Returns(dsa);

            var macParams = _macParamsBuilder
                .WithKeyAgreementMacType(macType)
                .WithMacLength(tagLength)
                .WithNonce(aesCcmNonce)
                .Build();

            var kas = _subject
                .WithKeyAgreementRole(keyAgreementRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesFfc(FfcScheme.Mqv1, FfcParameterSet.Fb))
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
        #endregion mqv1, no key confirmation

        #region mqv1, no key confirmation
        private static object[] _test_mqv1KeyConfirmation = new object[]
        {
            #region hmac
            new object[]
            {
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("934b7ea4a560a3958f5fb38d60ca5ae88550ca4565815051286afddc4fccc80e4848a4baed688651899de6c2ce9b31404dfe38875e0780c0be958c433ba0870896e9f98933adcaa8643efa70ad8dde7efce48ead3f6b89f342032d84aefa0caee8008a1930742a7203ea3b074f6238cedea9b5876db87e06bc4a16b39de3a870caf7d00a4f5bd9a4aa6882946d7ea6bf6a9c5e690fe907f0612e194ca82b99ccc82d616370ba09ea99f25b148d4c3d99d30da5ac3b83c8d2e716472e551c45c8c0541e9a69e846a89882a109b6480afae983cc3fff932eb64e2ca0ecd266752a727139215910716e2929b985d51359f0c4fd5dd6c9d3c764cb6fa7f609761d91").ToPositiveBigInteger(),
                    // q
                    new BitString("a2fa20a69a6c9881ac8886b8a3fb1f396db84fde38831784faa268db").ToPositiveBigInteger(), 
                    // g
                    new BitString("886f8001eb11413c9337f492408ea76e0ab4120aef7f709e3fabc8b2e672de15495b51d0a04e5359eb5fe39f4069d2bc3b281cc6192058839cc24952caaab93e875fd0b9d70fc3cce00594eae1aa97842c5486b3b4800dd6630d242d9ff29c60bdb6f382fb289783b6d38c4676d8f36a7bf08daac29deece03537604b51e17f0bc8d02f73ee73ddaa4c26f88869ee96964d2102fb8c0f949ef390e311984d1849ea194a5c4e2f80f01c0db7fabbff92d118f872941169866730545622c7b22bd6c2c34144ae0e0544cf2c389108717866a8a289852b06f3e69de77c36b72f649af9edf2ece4ceba76e478809826829d8198cb5d403f80b505ec1717635352895").ToPositiveBigInteger()
                ), 
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
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("934b7ea4a560a3958f5fb38d60ca5ae88550ca4565815051286afddc4fccc80e4848a4baed688651899de6c2ce9b31404dfe38875e0780c0be958c433ba0870896e9f98933adcaa8643efa70ad8dde7efce48ead3f6b89f342032d84aefa0caee8008a1930742a7203ea3b074f6238cedea9b5876db87e06bc4a16b39de3a870caf7d00a4f5bd9a4aa6882946d7ea6bf6a9c5e690fe907f0612e194ca82b99ccc82d616370ba09ea99f25b148d4c3d99d30da5ac3b83c8d2e716472e551c45c8c0541e9a69e846a89882a109b6480afae983cc3fff932eb64e2ca0ecd266752a727139215910716e2929b985d51359f0c4fd5dd6c9d3c764cb6fa7f609761d91").ToPositiveBigInteger(),
                    // q
                    new BitString("a2fa20a69a6c9881ac8886b8a3fb1f396db84fde38831784faa268db").ToPositiveBigInteger(), 
                    // g
                    new BitString("886f8001eb11413c9337f492408ea76e0ab4120aef7f709e3fabc8b2e672de15495b51d0a04e5359eb5fe39f4069d2bc3b281cc6192058839cc24952caaab93e875fd0b9d70fc3cce00594eae1aa97842c5486b3b4800dd6630d242d9ff29c60bdb6f382fb289783b6d38c4676d8f36a7bf08daac29deece03537604b51e17f0bc8d02f73ee73ddaa4c26f88869ee96964d2102fb8c0f949ef390e311984d1849ea194a5c4e2f80f01c0db7fabbff92d118f872941169866730545622c7b22bd6c2c34144ae0e0544cf2c389108717866a8a289852b06f3e69de77c36b72f649af9edf2ece4ceba76e478809826829d8198cb5d403f80b505ec1717635352895").ToPositiveBigInteger()
                ), 
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
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("85bba508c4b43e791c4816b4d7044eba4277b505ec6bd0d6033d7f576ebe9387b0a5534f48221f00c4ed85522eeb0e21e512feb3a0e5c45ec601af82d94f65dade907113a5858ca306d92253f5b7f75bdcf7cef3e028ae611e2ba541f080a7edeb487a81680f92502e050d6d57ee9c64ca00c134a777139fd45f82f28bb8c24e7adade99baa395b8c8d0567e493e4e728dfd356650ec6cb6ecf5b6dff477ad0d803f74053899788d3aac26034d00bb5ed81da2cee86c2dca9d416cb058c0fb79ff3390b234bee37d0c06b3b66313ff3dbbf5705bead0ebc5c9985182a8969ef14a016e23c05c90ae9c351fbdf9913d4cf7357f3994eedc725de27a9e81e82b97").ToPositiveBigInteger(),
                    // q
                    new BitString("984a6504d640038f9819c23652282aeb6bb7ec7ec22629c12ce93a45").ToPositiveBigInteger(), 
                    // g
                    new BitString("2f543534c508fcabce49d5ff56ef750da66df09286d6648338541a4a69e0cf340ae122df4695c2eb6b53e687d84840d56f61fd9c4491f624c7c9a1e5d16b9b6032d647b7f4ed31e0726ce91820e73bf0e58df3c3e940cb9bf1b09c396d8bfa5a68f253e1146ecbdf906871bbbb38ac8bad1aec1c4728a6bf6702ef01498af9904d8c7092bf0d333500b7f2c92f89f82bd689a38152ec7f04bee4aad000439f244bb4890389a020534130fd8fbf479a35e0e3da09538c890a379bfe37306feca78939ba19c7d8c567628971c5a0c26cbc2cd67a335299d036c929a906d6e9078682b36c6a1f770ec1c5afa11d903a324e99d65d3273f9dcd6df0b001c38f338a6").ToPositiveBigInteger()
                ), 
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
                // inverse of previous test
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("85bba508c4b43e791c4816b4d7044eba4277b505ec6bd0d6033d7f576ebe9387b0a5534f48221f00c4ed85522eeb0e21e512feb3a0e5c45ec601af82d94f65dade907113a5858ca306d92253f5b7f75bdcf7cef3e028ae611e2ba541f080a7edeb487a81680f92502e050d6d57ee9c64ca00c134a777139fd45f82f28bb8c24e7adade99baa395b8c8d0567e493e4e728dfd356650ec6cb6ecf5b6dff477ad0d803f74053899788d3aac26034d00bb5ed81da2cee86c2dca9d416cb058c0fb79ff3390b234bee37d0c06b3b66313ff3dbbf5705bead0ebc5c9985182a8969ef14a016e23c05c90ae9c351fbdf9913d4cf7357f3994eedc725de27a9e81e82b97").ToPositiveBigInteger(),
                    // q
                    new BitString("984a6504d640038f9819c23652282aeb6bb7ec7ec22629c12ce93a45").ToPositiveBigInteger(), 
                    // g
                    new BitString("2f543534c508fcabce49d5ff56ef750da66df09286d6648338541a4a69e0cf340ae122df4695c2eb6b53e687d84840d56f61fd9c4491f624c7c9a1e5d16b9b6032d647b7f4ed31e0726ce91820e73bf0e58df3c3e940cb9bf1b09c396d8bfa5a68f253e1146ecbdf906871bbbb38ac8bad1aec1c4728a6bf6702ef01498af9904d8c7092bf0d333500b7f2c92f89f82bd689a38152ec7f04bee4aad000439f244bb4890389a020534130fd8fbf479a35e0e3da09538c890a379bfe37306feca78939ba19c7d8c567628971c5a0c26cbc2cd67a335299d036c929a906d6e9078682b36c6a1f770ec1c5afa11d903a324e99d65d3273f9dcd6df0b001c38f338a6").ToPositiveBigInteger()
                ), 
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
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d684e03c751fc12cbd59585836600df9a2618ad86b82d1a00a5aefba6a9482259bcac0aa7c4d7a532a6036376e4fe401a8d05a89837e9c2c0640af0bf889af57381895dbebf4ece9544d2317e5c1dd1eac8e69aa2aa5a83915955a08f536ccf7afe11a92a4ce4fee373e1469ee2e42d3404125d78de57103d440407e04be5aa42a8251ccd97517ea8549edd21d149fed4f8642b8d1b6af97b13f540650f19a67ba778a4c3403385b655701b8f906c752bfa9142948cf12a9378e44a1fd7e2a7eee2798d5814d2e08a6348da9f3c53c2055f3dd19e2bb9f3ce6fc955604cab74168f5669cc04e2e2756caa06d4f1cb2f4a8ab53b40672b669455e4234d7d312cb").ToPositiveBigInteger(),
                    // q
                    new BitString("e40c394eb7f7751824b1a016f2b5e9ac111c6be1ecc4d76ac4227e15").ToPositiveBigInteger(), 
                    // g
                    new BitString("7ba2f238010f845b20b1f5c2c94f6ce0775f47861549b755d88dbf747849deaf36a74fb29ecc9d2cddbea65024699366c494a5903eda91f186c2889eee738d120a117d2a2a840e2b3ed9a1c51ab001f2e424ef30c49f3d162f89143ac274521c728eb9699889c0463faece6681d663a9ed75be934de527df8463a1331a859cfa5d963fcdfb446e3c8e0ea8c5379c9c2505ea3fa22695a35a9001a3641ba10ef759f7516e1f171b7c414790818ff5bc3422ee0ec51f4f53f7e4ac288336dd749002e9e62678b2c80ec1d783ea1dcd674a9d7d5d050994c373d29d51f69b91c4f160e2de5f1c91448c48e71748106c0406598aadee3c4a4aba09b86fccf4d33e89").ToPositiveBigInteger()
                ), 
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
                // inverse of previous
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("d684e03c751fc12cbd59585836600df9a2618ad86b82d1a00a5aefba6a9482259bcac0aa7c4d7a532a6036376e4fe401a8d05a89837e9c2c0640af0bf889af57381895dbebf4ece9544d2317e5c1dd1eac8e69aa2aa5a83915955a08f536ccf7afe11a92a4ce4fee373e1469ee2e42d3404125d78de57103d440407e04be5aa42a8251ccd97517ea8549edd21d149fed4f8642b8d1b6af97b13f540650f19a67ba778a4c3403385b655701b8f906c752bfa9142948cf12a9378e44a1fd7e2a7eee2798d5814d2e08a6348da9f3c53c2055f3dd19e2bb9f3ce6fc955604cab74168f5669cc04e2e2756caa06d4f1cb2f4a8ab53b40672b669455e4234d7d312cb").ToPositiveBigInteger(),
                    // q
                    new BitString("e40c394eb7f7751824b1a016f2b5e9ac111c6be1ecc4d76ac4227e15").ToPositiveBigInteger(), 
                    // g
                    new BitString("7ba2f238010f845b20b1f5c2c94f6ce0775f47861549b755d88dbf747849deaf36a74fb29ecc9d2cddbea65024699366c494a5903eda91f186c2889eee738d120a117d2a2a840e2b3ed9a1c51ab001f2e424ef30c49f3d162f89143ac274521c728eb9699889c0463faece6681d663a9ed75be934de527df8463a1331a859cfa5d963fcdfb446e3c8e0ea8c5379c9c2505ea3fa22695a35a9001a3641ba10ef759f7516e1f171b7c414790818ff5bc3422ee0ec51f4f53f7e4ac288336dd749002e9e62678b2c80ec1d783ea1dcd674a9d7d5d050994c373d29d51f69b91c4f160e2de5f1c91448c48e71748106c0406598aadee3c4a4aba09b86fccf4d33e89").ToPositiveBigInteger()
                ), 
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
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("c1af450dc2013f4ac1df8c33749db0b6d080bce55da07b477ba0a2b27bcde25d306e16688b654d4f0025e2c60fde974eec89732ccefb5b5b3b4b6b3623d633a186a9982526585a04a01ce866ba2c2930ac82d76bc85475ffa6eb5b5b8a6b8722901aa4fc2c77a1062477c25c7669f26e2a30895e8854cf98ca8a40204a159227291f1530b96493882b9ec09f395e0fdc8b877cd05c94782005cbe4b137f37a9b70da28ec5d64b06b242f3678ac485041121d1514b1ee88bb30f0b0bac3af3e750b8fbc69259f21fbd2d6886af585cd0c607e698d61672905c7a714ef588d7d9f70f0d3d66ea124f5fa0d4f7cbb4680fbc3e2dbc236dcc7ff3f32331272907261").ToPositiveBigInteger(),
                    // q
                    new BitString("da030247ad159d59dfcd8555c2fdefcb6deb3bfa815bc8e1c6d7b681").ToPositiveBigInteger(), 
                    // g
                    new BitString("ba58604c6af428c52c0b5f4048106b202d95eca5ab63c986d0f34cb067cdbf4d3646fee1d234e42096683f2dd7f02d1c764a3626cf3fdd7b1a081e8c2b965d567115afba4d4a9172cdcd7f2d6074816b29860a195f7dae112114166059463a9ea56e6f8e4c65724f7fa962a39a946bfe3c190944ea41c7f62d1a3e3855865da7dac9b21fa26195c6bc7593de3425e429e748309402925364e63985987a7c549795a250c1cf8073ed8f8de64f3d7c098d23dd5e78b8b03065df92e8c6da08b1eb202f8411a49709b7216824274e836729e463dc8d772c2bac7b5c99c2a7a39da208d94ec1a61a4cf12ffef3bbb6098a259a6a56c8d2e57a1840a7a429eabcb2f9").ToPositiveBigInteger()
                ), 
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
                // Domain parameters
                new FfcDomainParameters(
                    // p
                    new BitString("c1af450dc2013f4ac1df8c33749db0b6d080bce55da07b477ba0a2b27bcde25d306e16688b654d4f0025e2c60fde974eec89732ccefb5b5b3b4b6b3623d633a186a9982526585a04a01ce866ba2c2930ac82d76bc85475ffa6eb5b5b8a6b8722901aa4fc2c77a1062477c25c7669f26e2a30895e8854cf98ca8a40204a159227291f1530b96493882b9ec09f395e0fdc8b877cd05c94782005cbe4b137f37a9b70da28ec5d64b06b242f3678ac485041121d1514b1ee88bb30f0b0bac3af3e750b8fbc69259f21fbd2d6886af585cd0c607e698d61672905c7a714ef588d7d9f70f0d3d66ea124f5fa0d4f7cbb4680fbc3e2dbc236dcc7ff3f32331272907261").ToPositiveBigInteger(),
                    // q
                    new BitString("da030247ad159d59dfcd8555c2fdefcb6deb3bfa815bc8e1c6d7b681").ToPositiveBigInteger(), 
                    // g
                    new BitString("ba58604c6af428c52c0b5f4048106b202d95eca5ab63c986d0f34cb067cdbf4d3646fee1d234e42096683f2dd7f02d1c764a3626cf3fdd7b1a081e8c2b965d567115afba4d4a9172cdcd7f2d6074816b29860a195f7dae112114166059463a9ea56e6f8e4c65724f7fa962a39a946bfe3c190944ea41c7f62d1a3e3855865da7dac9b21fa26195c6bc7593de3425e429e748309402925364e63985987a7c549795a250c1cf8073ed8f8de64f3d7c098d23dd5e78b8b03065df92e8c6da08b1eb202f8411a49709b7216824274e836729e463dc8d772c2bac7b5c99c2a7a39da208d94ec1a61a4cf12ffef3bbb6098a259a6a56c8d2e57a1840a7a429eabcb2f9").ToPositiveBigInteger()
                ), 
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
        };

        [Test]
        [TestCaseSource(nameof(_test_mqv1KeyConfirmation))]
        public void ShouldMqv1KeyConfirmationCorrectly(
            FfcDomainParameters domainParameters,
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
            BitString thisPartyEphemeralNonce,
            BitString otherPartyId,
            BigInteger otherPartyPrivateStaticKey,
            BigInteger otherPartyPublicStaticKey,
            BigInteger otherPartyPrivateEphemKey,
            BigInteger otherPartyPublicEphemKey,
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
                    otherPartyEphemeralNonce,
                    null,
                    null
                );

            // u/v party info comprised of ID, and dkmNonce (when available), find the bitlength of both parties contributed information 
            // to determine which bits are the "random" bits to inject into the TestableEntropyProvider.
            var composedBitLength = thisPartyId.BitLength +
                                    new BitString(0).BitLength + // DKM nonce when applicable
                                    otherPartyId.BitLength;

            var entropyBits = expectedOi.GetLeastSignificantBits(expectedOi.BitLength - composedBitLength);

            _entropyProviderOtherInfo.AddEntropy(entropyBits);

            // MAC no key confirmation data makes use of a nonce
            if (thisPartyEphemeralNonce != null)
            {
                _entropyProviderScheme.AddEntropy(thisPartyEphemeralNonce);
            }

            FfcDsa dsa = new FfcDsa(new ShaFactory().GetShaInstance(new HashFunction(dsaKdfHashMode, dsaKdfDigestSize)));
            _dsaFactory
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), EntropyProviderTypes.Random))
                .Returns(dsa);

            var macParams = _macParamsBuilder
                .WithKeyAgreementMacType(macType)
                .WithMacLength(tagLength)
                .WithNonce(aesCcmNonce)
                .Build();

            var kas = _subject
                .WithKeyAgreementRole(keyAgreementRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesFfc(FfcScheme.Mqv1, FfcParameterSet.Fb))
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
        #endregion mqv1, key confirmation
        #endregion mqv1
    }
}
