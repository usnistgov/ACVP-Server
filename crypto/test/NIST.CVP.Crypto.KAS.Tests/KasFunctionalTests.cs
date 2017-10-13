using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Builders;
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
using NuGet.Packaging;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests
{
    [TestFixture, FastIntegrationTest]
    public class KasFunctionalTests
    {
        private KasBuilder _subject;
        private MacParametersBuilder _macParamsBuilder;
        private Mock<IDsaFfc> _dsa;
        private IEntropyProvider _entropyProviderScheme;
        private IEntropyProvider _entropyProviderOtherInfo;

        [SetUp]
        public void Setup()
        {
            _dsa = new Mock<IDsaFfc>();
            _entropyProviderScheme = new TestableEntropyProvider();
            _entropyProviderOtherInfo = new TestableEntropyProvider();

            _subject = new KasBuilder(
                new SchemeBuilder(
                    _dsa.Object,
                    new KdfFactory(
                        new ShaFactory()
                    ),
                    new KeyConfirmationFactory(),
                    new NoKeyConfirmationFactory(),
                    new OtherInfoFactory(
                        _entropyProviderOtherInfo
                    ),
                    _entropyProviderScheme,
                    new DiffieHellman(),
                    new Mqv()
                )
            );
            
            _macParamsBuilder = new MacParametersBuilder();
        }

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
        public void ShouldDhEphemComponentOnlySha512Correctly(FfcDomainParameters domainParameters, KeyAgreementRole keyAgreementRole, BitString thisPartyId, BigInteger thisPartyPrivateEphemKey, BigInteger thisPartyPublicEphemKey, BitString otherPartyId, BigInteger otherPartyPrivateEphemKey, BigInteger otherPartyPublicEphemKey, BitString expectedZ, BitString expectedHashZ)
        {
            var vPartySharedInformation = 
                new FfcSharedInformation(
                    domainParameters, 
                    otherPartyId, 
                    otherPartyPrivateEphemKey, 
                    otherPartyPublicEphemKey, 
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
                .WithScheme(FfcScheme.DhEphem)
                .WithParameterSet(FfcParameterSet.Fb)
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

        #region dhEphem, no key confirmation, HMAC
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
                new FfcSharedInformation(
                    domainParameters,
                    otherPartyId,
                    otherPartyPrivateEphemKey,
                    otherPartyPublicEphemKey,
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
                .WithScheme(FfcScheme.DhEphem)
                .WithParameterSet(FfcParameterSet.Fb)
                .WithAssurances(KasAssurance.None)
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
            Assert.AreEqual(expectedMacData.ToHex(), result.MacData.ToHex(), nameof(result.MacData));
            Assert.AreEqual(expectedTag, result.Tag, nameof(result.Tag));
        }

        [Test]
        [TestCaseSource(nameof(_test_dhEphemNoKeyConfirmation))]
        public void ShouldDhEphemNoKeyConfirmationCorrectly_UsesBuilder(
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
                new FfcSharedInformation(
                    domainParameters,
                    otherPartyId,
                    otherPartyPrivateEphemKey,
                    otherPartyPublicEphemKey,
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
                .WithScheme(FfcScheme.DhEphem)
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
    }
}
