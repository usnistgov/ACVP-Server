using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Moq;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Builders.Ecc;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KAS.Scheme.Ecc;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Tests
{
    [TestFixture, FastCryptoTest]
    public class KasFunctionalTestsEcc
    {
        private readonly EccCurveFactory _curveFactory = new EccCurveFactory();

        private readonly MacParametersBuilder _macParamsBuilder = new MacParametersBuilder();

        private KasBuilderEcc _subject;
        private Mock<IDsaEcc> _dsa;
        private Mock<IDsaEccFactory> _dsaFactory;
        private IEntropyProvider _entropyProviderScheme;
        private IEntropyProvider _entropyProviderOtherInfo;

        [SetUp]
        public void Setup()
        {
            _dsa = new Mock<IDsaEcc>();
            _dsaFactory = new Mock<IDsaEccFactory>();
            _dsaFactory
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(_dsa.Object);
            _entropyProviderScheme = new TestableEntropyProvider();
            _entropyProviderOtherInfo = new TestableEntropyProvider();

            _subject = new KasBuilderEcc(
                new SchemeBuilderEcc(
                    _dsaFactory.Object,
                    _curveFactory,
                    new KdfFactory(
                        new ShaFactory()
                    ),
                    new KeyConfirmationFactory(),
                    new NoKeyConfirmationFactory(),
                    new OtherInfoFactory(_entropyProviderOtherInfo),
                    _entropyProviderScheme,
                    new DiffieHellmanEcc(),
                    new MqvEcc()
                )
            );
        }

        private static object[] _test_componentOnly = new object[]
        {
            #region ephemeralUnified
            new object[]
            {
                // label
                "ephemeralUnified B233",
                // scheme
                EccScheme.EphemeralUnified,
                // Curve
                Curve.B233, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                (BigInteger)0,
                // public static this party
                null,
                // private ephem this party
                new BitString("000000c2ca50d96624bd2e6857c98a0d14206a4402ecb21cda2135c48c652844").ToPositiveBigInteger(),
                // public ephem this party
                new EccPoint(
                    new BitString("000000483b5e148ac734963d7aac2194c6e96294d83289ba77ef1edb45c0debd").ToPositiveBigInteger(),
                    new BitString("0000014e8ca9033f04c5b102f37d4b253874a8bd54260fd35b61a32a800c9d79").ToPositiveBigInteger()
                ),
                // Other Party ID
                new BitString("434156536964"),
                // private static this party
                (BigInteger)0,
                // public static this party
                null,
                // private ephem other party
                new BitString("00000080360eaea873fb66a2341845ce310f0b8b144deb1cb5e938520aedae52").ToPositiveBigInteger(), 
                // public ephem other party
                new EccPoint(
                    new BitString("000000dfe9cd18b4613008e13a086e7a69b3752916829a32e06aa246cf91aa79").ToPositiveBigInteger(),
                    new BitString("000001c68825ce9f2355eaa86f7bc58235988c0d7ff5441acdf0eca1ee2494a9").ToPositiveBigInteger()
                ),
                // expected Z
                new BitString("005d1321a08c24af3bb28b7a9e98873fa00593d4b9b2200155af5e97765e"),
                // expected Z hash
                new BitString("694eabfc6465a165e20ca9dc9b605de69ed28f4582e18d785c6136ad2b61636907d4c724d66541d540fc23008f94e3c18917105d848cdd1ed3f5708ab26dd1cf")
            },
            new object[]
            {
                // label
                "ephemeralUnified B233 inverse",
                // scheme
                EccScheme.EphemeralUnified,
                // Curve
                Curve.B233, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d512,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                (BigInteger)0,
                // public static this party
                null,
                // private ephem this party
                new BitString("00000080360eaea873fb66a2341845ce310f0b8b144deb1cb5e938520aedae52").ToPositiveBigInteger(),
                // public ephem this party
                new EccPoint(
                    new BitString("000000dfe9cd18b4613008e13a086e7a69b3752916829a32e06aa246cf91aa79").ToPositiveBigInteger(),
                    new BitString("000001c68825ce9f2355eaa86f7bc58235988c0d7ff5441acdf0eca1ee2494a9").ToPositiveBigInteger()
                ),
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static this party
                (BigInteger)0,
                // public static this party
                null,
                // private ephem other party
                new BitString("000000c2ca50d96624bd2e6857c98a0d14206a4402ecb21cda2135c48c652844").ToPositiveBigInteger(), 
                // public ephem other party
                new EccPoint(
                    new BitString("000000483b5e148ac734963d7aac2194c6e96294d83289ba77ef1edb45c0debd").ToPositiveBigInteger(),
                    new BitString("0000014e8ca9033f04c5b102f37d4b253874a8bd54260fd35b61a32a800c9d79").ToPositiveBigInteger()
                ),
                // expected Z
                new BitString("005d1321a08c24af3bb28b7a9e98873fa00593d4b9b2200155af5e97765e"),
                // expected Z hash
                new BitString("694eabfc6465a165e20ca9dc9b605de69ed28f4582e18d785c6136ad2b61636907d4c724d66541d540fc23008f94e3c18917105d848cdd1ed3f5708ab26dd1cf")
            },
            #endregion ephemeralUnified

            #region onePassMqv
            new object[]
            {
                // label
                "onePassMqv B233",
                // scheme
                EccScheme.OnePassMqv,
                // Curve
                Curve.B233, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d256,
                // this party role
                KeyAgreementRole.InitiatorPartyU,
                // this party Id
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("00000052c23ea9f876d6d847c0c54578a16612ce384f69d6d4751bda6c107bc3").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("0000017f5b2f350ffc44026977cbbe5a9f33f8d3a1beace814eb0e3693c8202b").ToPositiveBigInteger(),
                    new BitString("000001623a8e762e7b21e10359a046b64aca711c146e253dd2fa35a2f12cd5ab").ToPositiveBigInteger()
                ),
                // private ephem this party
                new BitString("000000059bf1926cda4bd3823f3e0ed440a18c1a54a33701eafd053a2e6ffae5").ToPositiveBigInteger(),
                // public ephem this party
                new EccPoint(
                    new BitString("00000181cfd86c22f5d417a0228418581e7ce910ce8cc04ff56593af460da035").ToPositiveBigInteger(),
                    new BitString("000000586324d10c8715dbe36dd7b8ff01c9c95340bd5e7c0d229e7279887c0b").ToPositiveBigInteger()
                ),
                // Other Party ID
                new BitString("434156536964"),
                // private static this party
                new BitString("0000007ad5b7faad03268f34de6cef03527efc1ca333ffa66172d386a0b4ab88").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("000000a24f0077bee53e51fd02beddadec5744f48068195313c5060ba93accbf").ToPositiveBigInteger(),
                    new BitString("000001cb61023be720448673bcd653ee1b17e8426b7467160d89a4bce9eea19d").ToPositiveBigInteger()
                ),
                // private ephem other party
                new BitString("00").ToPositiveBigInteger(), 
                // public ephem other party
                null,
                // expected Z
                new BitString("007bf51ab995289a403b20dc669c509af1eeadbe6d4e710d9420964a4849"),
                // expected Z hash
                new BitString("60fb697f0f2e456df819aa5e38730410cc8cd712d7f4bf1827f787912bcf73d4")
            },
            new object[]
            {
                // label
                "onePassMqv B233 inverse",
                // scheme
                EccScheme.OnePassMqv,
                // Curve
                Curve.B233, 
                // Sha mode,
                ModeValues.SHA2,
                // Digest size
                DigestSizes.d256,
                // this party role
                KeyAgreementRole.ResponderPartyV,
                // this party Id
                new BitString("434156536964"),
                // private static this party
                new BitString("0000007ad5b7faad03268f34de6cef03527efc1ca333ffa66172d386a0b4ab88").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("000000a24f0077bee53e51fd02beddadec5744f48068195313c5060ba93accbf").ToPositiveBigInteger(),
                    new BitString("000001cb61023be720448673bcd653ee1b17e8426b7467160d89a4bce9eea19d").ToPositiveBigInteger()
                ),
                // private ephem this party
                new BitString("00").ToPositiveBigInteger(),
                // public ephem this party
                null,
                // Other Party ID
                new BitString("a1b2c3d4e5"),
                // private static this party
                new BitString("00000052c23ea9f876d6d847c0c54578a16612ce384f69d6d4751bda6c107bc3").ToPositiveBigInteger(),
                // public static this party
                new EccPoint(
                    new BitString("0000017f5b2f350ffc44026977cbbe5a9f33f8d3a1beace814eb0e3693c8202b").ToPositiveBigInteger(),
                    new BitString("000001623a8e762e7b21e10359a046b64aca711c146e253dd2fa35a2f12cd5ab").ToPositiveBigInteger()
                ),
                // private ephem other party
                new BitString("000000059bf1926cda4bd3823f3e0ed440a18c1a54a33701eafd053a2e6ffae5").ToPositiveBigInteger(), 
                // public ephem other party
                new EccPoint(
                    new BitString("00000181cfd86c22f5d417a0228418581e7ce910ce8cc04ff56593af460da035").ToPositiveBigInteger(),
                    new BitString("000000586324d10c8715dbe36dd7b8ff01c9c95340bd5e7c0d229e7279887c0b").ToPositiveBigInteger()
                ),
                // expected Z
                new BitString("007bf51ab995289a403b20dc669c509af1eeadbe6d4e710d9420964a4849"),
                // expected Z hash
                new BitString("60fb697f0f2e456df819aa5e38730410cc8cd712d7f4bf1827f787912bcf73d4")
            },
            #endregion onePassMqv
        };

        [Test]
        [TestCaseSource(nameof(_test_componentOnly))]
        public void ShouldComponentOnlyCorrectly(
            string label,
            EccScheme scheme,
            Curve curveName,
            ModeValues modeValue,
            DigestSizes digestSize,
            KeyAgreementRole keyAgreementRole,
            BitString thisPartyId,
            BigInteger thisPartyPrivateStaticKey,
            EccPoint thisPartyPublicStaticKey,
            BigInteger thisPartyPrivateEphemKey,
            EccPoint thisPartyPublicEphemKey,
            BitString otherPartyId,
            BigInteger otherPartyPrivateStaticKey,
            EccPoint otherPartyPublicStaticKey,
            BigInteger otherPartyPrivateEphemKey,
            EccPoint otherPartyPublicEphemKey,
            BitString expectedZ,
            BitString expectedHashZ
        )
        {
            var curve = _curveFactory.GetCurve(curveName);
            var domainParameters = new EccDomainParameters(curve);

            var vPartySharedInformation =
                new OtherPartySharedInformation<EccDomainParameters, EccKeyPair>(
                    domainParameters,
                    otherPartyId,
                    otherPartyPublicStaticKey != null ? new EccKeyPair(otherPartyPublicStaticKey) : null,
                    otherPartyPublicEphemKey != null ? new EccKeyPair(otherPartyPublicEphemKey) : null,
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
                .Returns(() => new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 1), 1)));

            var kas = _subject
                .WithKeyAgreementRole(keyAgreementRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(scheme, EccParameterSet.Eb, curveName))
                .WithAssurances(KasAssurance.None)
                .WithPartyId(thisPartyId)
                .BuildNoKdfNoKc()
                .Build();

            kas.SetDomainParameters(domainParameters);
            kas.ReturnPublicInfoThisParty();

            if (kas.Scheme.StaticKeyPair != null)
            {
                kas.Scheme.StaticKeyPair.PrivateD = thisPartyPrivateStaticKey;
                kas.Scheme.StaticKeyPair.PublicQ = thisPartyPublicStaticKey;
            }
            if (kas.Scheme.EphemeralKeyPair != null)
            {
                kas.Scheme.EphemeralKeyPair.PrivateD = thisPartyPrivateEphemKey;
                kas.Scheme.EphemeralKeyPair.PublicQ = thisPartyPublicEphemKey;
            }

            var result = kas.ComputeResult(vPartySharedInformation);

            Assume.That(result.Success, nameof(result.Success));
            Assert.AreEqual(expectedZ, result.Z, nameof(result.Z));
            Assert.AreEqual(expectedHashZ, result.Tag, nameof(result.Tag));
        }

        private static object[] _test_NoKeyConfirmation = new object[]
        {
            #region ephemeralunified
            #region hmac
            new object[]
            {
                // label
                "ephem unified P-224 hmac",
                // scheme
                EccScheme.EphemeralUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                64,
                // noKeyConfirmationNonce
                new BitString("123817b69860aa195a9cb405b9554750"),
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                null,
                // thisPartyPrivateEphemKey
                new BitString("e935434303d842605d07112b3b7789ccbe4c6d987db5fa15ea1cdadb").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("784028a2246950401ec81f8e4e03fd3765c4da4d45eba652ed5ba2b5").ToPositiveBigInteger(),
                    new BitString("d071c32634bcf058f072493b6943453a0bd117f5ee5c5866f037f6ab").ToPositiveBigInteger()
                ),
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                null,
                // otherPartyPrivateEphemKey
                new BitString("d58751997b3a551ccdc6f507bf6ab87e2be7f8267067a455a5815a54").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("c7893ced15c3009b93cb0abeaea2ede308b67fbbd902c6c5d94c24a4").ToPositiveBigInteger(),
                    new BitString("858afc2edd61fcefef3469dd5a004e74a3c727d16498a9408a8e3224").ToPositiveBigInteger()
                ),
                // expectedZ
                new BitString("dfd0570d4ae5c9f690c757aead04f7e14758fdc4ee05d8f0d089b91a"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964a6a68a65ab05652a9cb3152d9bdf5d3526995afae189667f96e13da17a905ff0b2b9b5bf"),
                // expectedDkm
                new BitString("24b6b6e6d10b2e5482c20ba3b06c"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d657373616765123817b69860aa195a9cb405b9554750"),
                // expectedTag
                new BitString("5f5f30f60bb868ca"),
            },
            new object[]
            {
                // label
                "ephem unified P-224 hmac inverse",
                // scheme
                EccScheme.EphemeralUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                64,
                // noKeyConfirmationNonce
                new BitString("123817b69860aa195a9cb405b9554750"),
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                null,
                // thisPartyPrivateEphemKey
                new BitString("d58751997b3a551ccdc6f507bf6ab87e2be7f8267067a455a5815a54").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("c7893ced15c3009b93cb0abeaea2ede308b67fbbd902c6c5d94c24a4").ToPositiveBigInteger(),
                    new BitString("858afc2edd61fcefef3469dd5a004e74a3c727d16498a9408a8e3224").ToPositiveBigInteger()
                ),
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                null,
                // otherPartyPrivateEphemKey
                new BitString("e935434303d842605d07112b3b7789ccbe4c6d987db5fa15ea1cdadb").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("784028a2246950401ec81f8e4e03fd3765c4da4d45eba652ed5ba2b5").ToPositiveBigInteger(),
                    new BitString("d071c32634bcf058f072493b6943453a0bd117f5ee5c5866f037f6ab").ToPositiveBigInteger()
                ),
                // expectedZ
                new BitString("dfd0570d4ae5c9f690c757aead04f7e14758fdc4ee05d8f0d089b91a"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964a6a68a65ab05652a9cb3152d9bdf5d3526995afae189667f96e13da17a905ff0b2b9b5bf"),
                // expectedDkm
                new BitString("24b6b6e6d10b2e5482c20ba3b06c"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d657373616765123817b69860aa195a9cb405b9554750"),
                // expectedTag
                new BitString("5f5f30f60bb868ca"),
            },
            #endregion hmac
            #region ccm
            new object[]
            {
                // label
                "ephem unified P-224 ccm",
                // scheme
                EccScheme.EphemeralUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.AesCcm,
                // keyLength
                192,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("3d851423f00ce4c41c9d2b41ef878dfd"),
                // aesCcmNonce
                new BitString("dec3f7a2013552"),
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                null,
                // thisPartyPrivateEphemKey
                new BitString("4ee19a1e8dafef8d84ad06f9a3176c7105d645fc61f66d5bb74175b2").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("2240e3d5ef61fc9e516f726bd4fd2d18caea337451ad42c07b97dcb6").ToPositiveBigInteger(),
                    new BitString("56700b229e6655d635d9af60923fff9bc21709c3308ec956d7ff35de").ToPositiveBigInteger()
                ),
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                null,
                // otherPartyPrivateEphemKey
                new BitString("d8d47bd33e59d933a11f21b3ece887c6dab61bf398df4933b64ce48c").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("9ce739dfcf76b8f1d8472eb8bf7bdf3e60a72913578edfd2e5891a29").ToPositiveBigInteger(),
                    new BitString("4359df09d4435c3c20611e97d22e4a058cf249fa342c1993181bc569").ToPositiveBigInteger()
                ),
                // expectedZ
                new BitString("ea83ece5641d41ad2486145f76d2f38145955efc6e67443050b7df07"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369648ad6b7b9f10253bf904d3bf3765d08d7247c69b0e49f199f5da00e0e65cff44d16a9d97d"),
                // expectedDkm
                new BitString("3e4f4074018299964ae03f473b87e009aa8a7bbf0eccc6e5"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d6573736167653d851423f00ce4c41c9d2b41ef878dfd"),
                // expectedTag
                new BitString("a927c08177ca67141ec1f867e38c"),
            },
            new object[]
            {
                // label
                "ephem unified P-224 ccm inverse",
                // scheme
                EccScheme.EphemeralUnified,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.AesCcm,
                // keyLength
                192,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("3d851423f00ce4c41c9d2b41ef878dfd"),
                // aesCcmNonce
                new BitString("dec3f7a2013552"),
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                null,
                // thisPartyPrivateEphemKey
                new BitString("d8d47bd33e59d933a11f21b3ece887c6dab61bf398df4933b64ce48c").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("9ce739dfcf76b8f1d8472eb8bf7bdf3e60a72913578edfd2e5891a29").ToPositiveBigInteger(),
                    new BitString("4359df09d4435c3c20611e97d22e4a058cf249fa342c1993181bc569").ToPositiveBigInteger()
                ),
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                null,
                // otherPartyPrivateEphemKey
                new BitString("4ee19a1e8dafef8d84ad06f9a3176c7105d645fc61f66d5bb74175b2").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("2240e3d5ef61fc9e516f726bd4fd2d18caea337451ad42c07b97dcb6").ToPositiveBigInteger(),
                    new BitString("56700b229e6655d635d9af60923fff9bc21709c3308ec956d7ff35de").ToPositiveBigInteger()
                ),
                // expectedZ
                new BitString("ea83ece5641d41ad2486145f76d2f38145955efc6e67443050b7df07"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369648ad6b7b9f10253bf904d3bf3765d08d7247c69b0e49f199f5da00e0e65cff44d16a9d97d"),
                // expectedDkm
                new BitString("3e4f4074018299964ae03f473b87e009aa8a7bbf0eccc6e5"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d6573736167653d851423f00ce4c41c9d2b41ef878dfd"),
                // expectedTag
                new BitString("a927c08177ca67141ec1f867e38c"),
            },
            #endregion ccm
            #endregion ephemeralunified
            #region onePassMqv
            #region hmac
            new object[]
            {
                // label
                "onePassMqv P-224 hmac",
                // scheme
                EccScheme.OnePassMqv,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                64,
                // noKeyConfirmationNonce
                new BitString("62f3fe59c92289ac2a2e163157b59b93"),
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("fdf1feb01cda935cca067242e357dc0a5a5bf4b914dbb8670f664914").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey,
                new EccPoint(
                    new BitString("a954f07bcf4c4303d4ac8816fcfbc24905fcc5e39cead1a3e5ab7d3f").ToPositiveBigInteger(),
                    new BitString("b13966f224cc7259406862621e162875454d98ed819984c1e3d32931").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("f58ee8be4bef57de9902df7604e644eef304c3768b973da2c6a07628").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey,
                new EccPoint(
                    new BitString("a419bafb8d27df03f8a025acb8f3f9d2106ae3f513915a6e5c5c8653").ToPositiveBigInteger(),
                    new BitString("30afdf737f87f5c9721c1ccb729d84b7e5a32fbf45e7cbd21dbd74a3").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("da58b80aa06dbe87e766501d4f5818c153930103a6e3ac86b7e9b14a").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("dcd2a6c0b73310b90912bfb613c9217dd91078b2789f2412ba43121e").ToPositiveBigInteger(),
                    new BitString("a971971c9d3666e22ce30914ccbaeafae8ca8b6d09625412cbfd7aa0").ToPositiveBigInteger()
                ),
                // expectedZ
                new BitString("0d3e8be1ac2b5e270828f3c8522f5108fa309c9872da4a0d2ba0b253"),
                // expectedOi
                new BitString("434156536964a1b2c3d4e521336022af7ddeca89b71c8e0628529345c5b1161d54f6af57d13ed71b111ca9a9a5e9e5"),
                // expectedDkm
                new BitString("e86d0bc928ee8328317d25c30404"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d65737361676562f3fe59c92289ac2a2e163157b59b93"),
                // expectedTag
                new BitString("579a8c9df7eb7df7"),
            },
            new object[]
            {
                // label
                "onePassMqv P-224 hmac inverse",
                // scheme
                EccScheme.OnePassMqv,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.HmacSha2D512,
                // keyLength
                112,
                // tagLength
                64,
                // noKeyConfirmationNonce
                new BitString("62f3fe59c92289ac2a2e163157b59b93"),
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("f58ee8be4bef57de9902df7604e644eef304c3768b973da2c6a07628").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey,
                new EccPoint(
                    new BitString("a419bafb8d27df03f8a025acb8f3f9d2106ae3f513915a6e5c5c8653").ToPositiveBigInteger(),
                    new BitString("30afdf737f87f5c9721c1ccb729d84b7e5a32fbf45e7cbd21dbd74a3").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("da58b80aa06dbe87e766501d4f5818c153930103a6e3ac86b7e9b14a").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("dcd2a6c0b73310b90912bfb613c9217dd91078b2789f2412ba43121e").ToPositiveBigInteger(),
                    new BitString("a971971c9d3666e22ce30914ccbaeafae8ca8b6d09625412cbfd7aa0").ToPositiveBigInteger()
                ),
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("fdf1feb01cda935cca067242e357dc0a5a5bf4b914dbb8670f664914").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey,
                new EccPoint(
                    new BitString("a954f07bcf4c4303d4ac8816fcfbc24905fcc5e39cead1a3e5ab7d3f").ToPositiveBigInteger(),
                    new BitString("b13966f224cc7259406862621e162875454d98ed819984c1e3d32931").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // expectedZ
                new BitString("0d3e8be1ac2b5e270828f3c8522f5108fa309c9872da4a0d2ba0b253"),
                // expectedOi
                new BitString("434156536964a1b2c3d4e521336022af7ddeca89b71c8e0628529345c5b1161d54f6af57d13ed71b111ca9a9a5e9e5"),
                // expectedDkm
                new BitString("e86d0bc928ee8328317d25c30404"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d65737361676562f3fe59c92289ac2a2e163157b59b93"),
                // expectedTag
                new BitString("579a8c9df7eb7df7"),
            },
            #endregion hmac
            #region ccm
            new object[]
            {
                // label
                "onePassMqv P-224 ccm",
                // scheme
                EccScheme.OnePassMqv,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.InitiatorPartyU,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.AesCcm,
                // keyLength
                128,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("ee8953da84f6c1a909d22ea0a7afb00e"),
                // aesCcmNonce
                new BitString("186ce451fae07e"),
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("7ab8b524fde9a252e2ec9a3c8636dfc8a88ae4da9e9217da06bd9828").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey,
                new EccPoint(
                    new BitString("037b7e9bd1a1f3fea4fd7bc61b4a9de59c0800f6dd2853021519eb42").ToPositiveBigInteger(),
                    new BitString("de8ac642772ef8f85cccc5bfe9056bc2b1cc0dd3f9859716633340fd").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("7f90aea5ad562af710b22be47f72e08f69360e285de9798eaf5d51cb").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("96efadc75bf3091b8d74de7a415fd3b3e4ef0a6d59485ba6d35b2b6c").ToPositiveBigInteger(),
                    new BitString("90ab198b4e8d4861fbf7d06f30890f1d754645aa5fce67e5798a4b13").ToPositiveBigInteger()
                ),
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("4c2fd3e8c11e41c27d9641f5715b6d12e4af3f27208b40e0fcda7b93").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey,
                new EccPoint(
                    new BitString("053f9177098d02f86fa40b2426906a5246cb49cfcc39fe05ed4b9744").ToPositiveBigInteger(),
                    new BitString("cba4acf518d15743eeec4fcd32e1e421d5137dcd361ca38f19eb4e29").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // expectedZ
                new BitString("37258088597e2d33628acd795d6d80358c87ae30ed0b8076f31ca2bd"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369646cf0ab95b430de5f6a12723a5cf6d525988696c83a3fd513822df9232af5a165f619dcfd"),
                // expectedDkm
                new BitString("08349e46bf880dab70cc14901186248d"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d657373616765ee8953da84f6c1a909d22ea0a7afb00e"),
                // expectedTag
                new BitString("a62579ad4b73caf3bd6ce3ec2e12"),
            },
            new object[]
            {
                // label
                "onePassMqv P-224 ccm inverse",
                // scheme
                EccScheme.OnePassMqv,
                // curve
                Curve.P224,
                // key agreement role
                KeyAgreementRole.ResponderPartyV,
                // kdf hash mode
                ModeValues.SHA2,
                // kdf digest size,
                DigestSizes.d224,
                // mac type
                KeyAgreementMacType.AesCcm,
                // keyLength
                128,
                // tagLength
                112,
                // noKeyConfirmationNonce
                new BitString("ee8953da84f6c1a909d22ea0a7afb00e"),
                // aesCcmNonce
                new BitString("186ce451fae07e"),
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("4c2fd3e8c11e41c27d9641f5715b6d12e4af3f27208b40e0fcda7b93").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey,
                new EccPoint(
                    new BitString("053f9177098d02f86fa40b2426906a5246cb49cfcc39fe05ed4b9744").ToPositiveBigInteger(),
                    new BitString("cba4acf518d15743eeec4fcd32e1e421d5137dcd361ca38f19eb4e29").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("7ab8b524fde9a252e2ec9a3c8636dfc8a88ae4da9e9217da06bd9828").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey,
                new EccPoint(
                    new BitString("037b7e9bd1a1f3fea4fd7bc61b4a9de59c0800f6dd2853021519eb42").ToPositiveBigInteger(),
                    new BitString("de8ac642772ef8f85cccc5bfe9056bc2b1cc0dd3f9859716633340fd").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("7f90aea5ad562af710b22be47f72e08f69360e285de9798eaf5d51cb").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("96efadc75bf3091b8d74de7a415fd3b3e4ef0a6d59485ba6d35b2b6c").ToPositiveBigInteger(),
                    new BitString("90ab198b4e8d4861fbf7d06f30890f1d754645aa5fce67e5798a4b13").ToPositiveBigInteger()
                ),
                // expectedZ
                new BitString("37258088597e2d33628acd795d6d80358c87ae30ed0b8076f31ca2bd"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369646cf0ab95b430de5f6a12723a5cf6d525988696c83a3fd513822df9232af5a165f619dcfd"),
                // expectedDkm
                new BitString("08349e46bf880dab70cc14901186248d"),
                // expectedMacData
                new BitString("5374616e646172642054657374204d657373616765ee8953da84f6c1a909d22ea0a7afb00e"),
                // expectedTag
                new BitString("a62579ad4b73caf3bd6ce3ec2e12"),
            },
            #endregion ccm
            #endregion onePassMqv
        };

        [Test]
        [TestCaseSource(nameof(_test_NoKeyConfirmation))]
        public void ShouldDhEphemNoKeyConfirmationCorrectly(
            string label,
            EccScheme scheme,
            Curve curveName,
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
            EccPoint thisPartyPublicStaticKey,
            BigInteger thisPartyPrivateEphemKey,
            EccPoint thisPartyPublicEphemKey,
            BitString otherPartyId,
            BigInteger otherPartyPrivateStaticKey,
            EccPoint otherpartyPublicStaticKey,
            BigInteger otherPartyPrivateEphemKey,
            EccPoint otherPartyPublicEphemKey,
            BitString expectedZ,
            BitString expectedOi,
            BitString expectedDkm,
            BitString expectedMacData,
            BitString expectedTag
        )
        {
            var curve = _curveFactory.GetCurve(curveName);
            var domainParameters = new EccDomainParameters(curve);

            var otherPartySharedInformation =
                new OtherPartySharedInformation<EccDomainParameters, EccKeyPair>(
                    domainParameters,
                    otherPartyId,
                    new EccKeyPair(otherpartyPublicStaticKey),
                    new EccKeyPair(otherPartyPublicEphemKey),
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

            _dsa
                .SetupGet(s => s.Sha)
                .Returns(new ShaFactory().GetShaInstance(new HashFunction(dsaKdfHashMode, dsaKdfDigestSize)));
            _dsa
                .Setup(s => s.GenerateKeyPair(domainParameters))
                .Returns(() => new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 1), 1)));

            var macParams = _macParamsBuilder
                .WithKeyAgreementMacType(macType)
                .WithMacLength(tagLength)
                .WithNonce(aesCcmNonce)
                .Build();

            var kas = _subject
                .WithKeyAgreementRole(keyAgreementRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(scheme, EccParameterSet.Eb, curveName))
                .WithPartyId(thisPartyId)
                .BuildKdfNoKc()
                .WithKeyLength(keyLength)
                .WithMacParameters(macParams)
                .Build();

            kas.SetDomainParameters(domainParameters);
            kas.ReturnPublicInfoThisParty();

            if (kas.Scheme.StaticKeyPair != null)
            {
                kas.Scheme.StaticKeyPair.PrivateD = thisPartyPrivateStaticKey;
                kas.Scheme.StaticKeyPair.PublicQ = thisPartyPublicStaticKey;
            }
            if (kas.Scheme.EphemeralKeyPair != null)
            {
                kas.Scheme.EphemeralKeyPair.PrivateD = thisPartyPrivateEphemKey;
                kas.Scheme.EphemeralKeyPair.PublicQ = thisPartyPublicEphemKey;
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
            #region onePassMqv
            #region hmac
            new object[]
            {
                // label
                "onePassMqv P224 hmac",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.InitiatorPartyU,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.HmacSha2D224,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Bilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("71b94b0b139630a558fec67d8525e1e71fda06d8b5171ead65dd9079").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("d9049254bea3b30830bf808ef63290049d1e1bf2627e11add742c6e7").ToPositiveBigInteger(),
                    new BitString("71ab72bcb8de04453b4e1ddc5984d7b79759d193ed7e54c13eaf9eed").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("a94036779eca96197f9c9bc601fd9614631d284ed4f32dc6f5f46de9").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("00e7ae5ff2b618eca669eb723e5a19233460532ac0aff7e59a5b2087").ToPositiveBigInteger(),
                    new BitString("71f14c792f9a37050cbf7814538cce3db13f91c0d3ebd4ebe8857ecf").ToPositiveBigInteger()
                ),
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("8b6606df3fae46702c192a54d37839be34b78dee0f00c6b5ea192cbb").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("5ffc623fa5d9c0f2839a0b68ae1d2cf737d53fb9b3e015269f1c59f2").ToPositiveBigInteger(),
                    new BitString("543e22fc09775e36d4d9d0865680a4ecfc843ff1f545a4b63ad252e4").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // otherPartyEphemeralNonce
                new BitString("f7320c90bd3a25e9ffccaedbc9d0b6c252b6c2a5dcc5cc20dcb08ba15b6bf5d89c3c0368ff15b4e3252154f5559dfedf1ee960273edfbe8e"),
                // expectedZ
                new BitString("5cb7d19d782b3bc5ebf88374507afbfa52d61f6ccd3d25d048af3d0f"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964670d390c7d9a654abb3dac2f24f9ce052a8b6591d0fe26460c7a64b96f5dd555078c9950"),
                // expectedDkm
                new BitString("526ee2c03cbb91c42ec40441c182"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e543415653696400e7ae5ff2b618eca669eb723e5a19233460532ac0aff7e59a5b208771f14c792f9a37050cbf7814538cce3db13f91c0d3ebd4ebe8857ecff7320c90bd3a25e9ffccaedbc9d0b6c252b6c2a5dcc5cc20dcb08ba15b6bf5d89c3c0368ff15b4e3252154f5559dfedf1ee960273edfbe8e"),
                // expectedTag
                new BitString("89fdcffc87f5fc57")
            },
            new object[]
            {
                // label
                "onePassMqv P224 hmac inverse",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.ResponderPartyV,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.HmacSha2D224,
                // keyConfirmationRole
                KeyConfirmationRole.Recipient,
                // keyConfirmationDirection
                KeyConfirmationDirection.Bilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("8b6606df3fae46702c192a54d37839be34b78dee0f00c6b5ea192cbb").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("5ffc623fa5d9c0f2839a0b68ae1d2cf737d53fb9b3e015269f1c59f2").ToPositiveBigInteger(),
                    new BitString("543e22fc09775e36d4d9d0865680a4ecfc843ff1f545a4b63ad252e4").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // thisPartyEphemeralNonce
                new BitString("f7320c90bd3a25e9ffccaedbc9d0b6c252b6c2a5dcc5cc20dcb08ba15b6bf5d89c3c0368ff15b4e3252154f5559dfedf1ee960273edfbe8e"),
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("71b94b0b139630a558fec67d8525e1e71fda06d8b5171ead65dd9079").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("d9049254bea3b30830bf808ef63290049d1e1bf2627e11add742c6e7").ToPositiveBigInteger(),
                    new BitString("71ab72bcb8de04453b4e1ddc5984d7b79759d193ed7e54c13eaf9eed").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("a94036779eca96197f9c9bc601fd9614631d284ed4f32dc6f5f46de9").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("00e7ae5ff2b618eca669eb723e5a19233460532ac0aff7e59a5b2087").ToPositiveBigInteger(),
                    new BitString("71f14c792f9a37050cbf7814538cce3db13f91c0d3ebd4ebe8857ecf").ToPositiveBigInteger()
                ),
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("5cb7d19d782b3bc5ebf88374507afbfa52d61f6ccd3d25d048af3d0f"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964670d390c7d9a654abb3dac2f24f9ce052a8b6591d0fe26460c7a64b96f5dd555078c9950"),
                // expectedDkm
                new BitString("526ee2c03cbb91c42ec40441c182"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e543415653696400e7ae5ff2b618eca669eb723e5a19233460532ac0aff7e59a5b208771f14c792f9a37050cbf7814538cce3db13f91c0d3ebd4ebe8857ecff7320c90bd3a25e9ffccaedbc9d0b6c252b6c2a5dcc5cc20dcb08ba15b6bf5d89c3c0368ff15b4e3252154f5559dfedf1ee960273edfbe8e"),
                // expectedTag
                new BitString("89fdcffc87f5fc57")
            },
            #endregion hmac
            #region ccm
            new object[]
            {
                // label
                "onePassMqv P224 ccm",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.ResponderPartyV,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.AesCcm,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                128,
                // tagLength
                128,
                // aesCcmNonce
                new BitString("eb00272f902f7ccb2864262e1c"),
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("a419d2a7ff0a8f2afdde83179a32a0d3b9c85a530d21b91eb9729bd0").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("59ebbf2011c46d2c4cae1f5682c112ff3218b751e43707a9735a863a").ToPositiveBigInteger(),
                    new BitString("32210cc6ee3157ee335c033c34f1e62ae85e45eab47a7d17b3f3a554").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("039f0667038ff32de9afe36b8e43146287b0212ba7803d5456378c8d").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("d30c08261704c0c2dc943ae8fc1163558eaa0e25e8413acfb960e997").ToPositiveBigInteger(),
                    new BitString("513e1d89d42fe78ba80e6bb7ddce96cca7392f86615c81aaa436eebd").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("6d87c79e515313ad0cf573465f6ead1d61c23f5e1c8c16d1378fdbfa").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("72e8163f3a8eb6bacf29049d4cddd2888c8ca6fbf05e592793b602c5").ToPositiveBigInteger(),
                    new BitString("3451e29cefed856ccedbf60360094b338c440d04ee299e451bbfb048").ToPositiveBigInteger()
                ),
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("673d08f0eb98493c43a9a4abd3fa8dbff8d65e919ebc98851c5cbf06"),
                // expectedOi
                new BitString("434156536964a1b2c3d4e51e1867f25cab0a3450e07a00e1946c3f1f221e4ccddbf6071db06625ba3212ca0bbd3559"),
                // expectedDkm
                new BitString("073880b44cc965a6b48619b75a18a9c2"),
                // expectedMacData
                new BitString("4b435f315f56a1b2c3d4e543415653696472e8163f3a8eb6bacf29049d4cddd2888c8ca6fbf05e592793b602c53451e29cefed856ccedbf60360094b338c440d04ee299e451bbfb048"),
                // expectedTag
                new BitString("ab12ea5ab6a2def71227768a131c650c")
            },
            new object[]
            {
                // label
                "onePassMqv P224 ccm inverse",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.InitiatorPartyU,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.AesCcm,
                // keyConfirmationRole
                KeyConfirmationRole.Recipient,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                128,
                // tagLength
                128,
                // aesCcmNonce
                new BitString("eb00272f902f7ccb2864262e1c"),
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("039f0667038ff32de9afe36b8e43146287b0212ba7803d5456378c8d").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("d30c08261704c0c2dc943ae8fc1163558eaa0e25e8413acfb960e997").ToPositiveBigInteger(),
                    new BitString("513e1d89d42fe78ba80e6bb7ddce96cca7392f86615c81aaa436eebd").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("6d87c79e515313ad0cf573465f6ead1d61c23f5e1c8c16d1378fdbfa").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("72e8163f3a8eb6bacf29049d4cddd2888c8ca6fbf05e592793b602c5").ToPositiveBigInteger(),
                    new BitString("3451e29cefed856ccedbf60360094b338c440d04ee299e451bbfb048").ToPositiveBigInteger()
                ),
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("a419d2a7ff0a8f2afdde83179a32a0d3b9c85a530d21b91eb9729bd0").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("59ebbf2011c46d2c4cae1f5682c112ff3218b751e43707a9735a863a").ToPositiveBigInteger(),
                    new BitString("32210cc6ee3157ee335c033c34f1e62ae85e45eab47a7d17b3f3a554").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("673d08f0eb98493c43a9a4abd3fa8dbff8d65e919ebc98851c5cbf06"),
                // expectedOi
                new BitString("434156536964a1b2c3d4e51e1867f25cab0a3450e07a00e1946c3f1f221e4ccddbf6071db06625ba3212ca0bbd3559"),
                // expectedDkm
                new BitString("073880b44cc965a6b48619b75a18a9c2"),
                // expectedMacData
                new BitString("4b435f315f56a1b2c3d4e543415653696472e8163f3a8eb6bacf29049d4cddd2888c8ca6fbf05e592793b602c53451e29cefed856ccedbf60360094b338c440d04ee299e451bbfb048"),
                // expectedTag
                new BitString("ab12ea5ab6a2def71227768a131c650c")
            },
            new object[]
            {
                // label
                "onePassMqv P224 ccm the weird scenario",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.ResponderPartyV,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.AesCcm,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                128,
                // tagLength
                128,
                // aesCcmNonce
                new BitString("eb00272f902f7ccb2864262e1c"),
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("a419d2a7ff0a8f2afdde83179a32a0d3b9c85a530d21b91eb9729bd0").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("59ebbf2011c46d2c4cae1f5682c112ff3218b751e43707a9735a863a").ToPositiveBigInteger(),
                    new BitString("32210cc6ee3157ee335c033c34f1e62ae85e45eab47a7d17b3f3a554").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("039f0667038ff32de9afe36b8e43146287b0212ba7803d5456378c8d").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("d30c08261704c0c2dc943ae8fc1163558eaa0e25e8413acfb960e997").ToPositiveBigInteger(),
                    new BitString("513e1d89d42fe78ba80e6bb7ddce96cca7392f86615c81aaa436eebd").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("6d87c79e515313ad0cf573465f6ead1d61c23f5e1c8c16d1378fdbfa").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("72e8163f3a8eb6bacf29049d4cddd2888c8ca6fbf05e592793b602c5").ToPositiveBigInteger(),
                    new BitString("3451e29cefed856ccedbf60360094b338c440d04ee299e451bbfb048").ToPositiveBigInteger()
                ),
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("673d08f0eb98493c43a9a4abd3fa8dbff8d65e919ebc98851c5cbf06"),
                // expectedOi
                new BitString("434156536964a1b2c3d4e51e1867f25cab0a3450e07a00e1946c3f1f221e4ccddbf6071db06625ba3212ca0bbd3559"),
                // expectedDkm
                new BitString("073880b44cc965a6b48619b75a18a9c2"),
                // expectedMacData
                new BitString("4b435f315f56a1b2c3d4e543415653696472e8163f3a8eb6bacf29049d4cddd2888c8ca6fbf05e592793b602c53451e29cefed856ccedbf60360094b338c440d04ee299e451bbfb048"),
                // expectedTag
                new BitString("ab12ea5ab6a2def71227768a131c650c")
            },
            new object[]
            {
                // label
                "onePassMqv P224 ccm the weird scenario inverse",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.P224,
                // keyAgreementRole
                KeyAgreementRole.InitiatorPartyU,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d224,
                // macType
                KeyAgreementMacType.AesCcm,
                // keyConfirmationRole
                KeyConfirmationRole.Recipient,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                128,
                // tagLength
                128,
                // aesCcmNonce
                new BitString("eb00272f902f7ccb2864262e1c"),
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("039f0667038ff32de9afe36b8e43146287b0212ba7803d5456378c8d").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("d30c08261704c0c2dc943ae8fc1163558eaa0e25e8413acfb960e997").ToPositiveBigInteger(),
                    new BitString("513e1d89d42fe78ba80e6bb7ddce96cca7392f86615c81aaa436eebd").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("6d87c79e515313ad0cf573465f6ead1d61c23f5e1c8c16d1378fdbfa").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("72e8163f3a8eb6bacf29049d4cddd2888c8ca6fbf05e592793b602c5").ToPositiveBigInteger(),
                    new BitString("3451e29cefed856ccedbf60360094b338c440d04ee299e451bbfb048").ToPositiveBigInteger()
                ),
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("a419d2a7ff0a8f2afdde83179a32a0d3b9c85a530d21b91eb9729bd0").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("59ebbf2011c46d2c4cae1f5682c112ff3218b751e43707a9735a863a").ToPositiveBigInteger(),
                    new BitString("32210cc6ee3157ee335c033c34f1e62ae85e45eab47a7d17b3f3a554").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("673d08f0eb98493c43a9a4abd3fa8dbff8d65e919ebc98851c5cbf06"),
                // expectedOi
                new BitString("434156536964a1b2c3d4e51e1867f25cab0a3450e07a00e1946c3f1f221e4ccddbf6071db06625ba3212ca0bbd3559"),
                // expectedDkm
                new BitString("073880b44cc965a6b48619b75a18a9c2"),
                // expectedMacData
                new BitString("4b435f315f56a1b2c3d4e543415653696472e8163f3a8eb6bacf29049d4cddd2888c8ca6fbf05e592793b602c53451e29cefed856ccedbf60360094b338c440d04ee299e451bbfb048"),
                // expectedTag
                new BitString("ab12ea5ab6a2def71227768a131c650c")
            },
            #endregion ccm
            #endregion onePassMqv
        };

        [Test]
        [TestCaseSource(nameof(_test_keyConfirmation))]
        public void ShouldMqv1KeyConfirmationCorrectly(
            string label,
            EccScheme scheme,
            Curve curveName,
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
            EccPoint thisPartyPublicStaticKey,
            BigInteger thisPartyPrivateEphemKey,
            EccPoint thisPartyPublicEphemKey,
            BitString thisPartyEphemeralNonce,
            BitString otherPartyId,
            BigInteger otherPartyPrivateStaticKey,
            EccPoint otherPartyPublicStaticKey,
            BigInteger otherPartyPrivateEphemKey,
            EccPoint otherPartyPublicEphemKey,
            BitString otherPartyEphemeralNonce,
            BitString expectedZ,
            BitString expectedOi,
            BitString expectedDkm,
            BitString expectedMacData,
            BitString expectedTag
        )
        {
            var curve = _curveFactory.GetCurve(curveName);
            var domainParameters = new EccDomainParameters(curve);

            var otherPartySharedInformation =
                new OtherPartySharedInformation<EccDomainParameters, EccKeyPair>(
                    domainParameters,
                    otherPartyId,
                    new EccKeyPair(otherPartyPublicStaticKey),
                    new EccKeyPair(otherPartyPublicEphemKey),
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

            _dsa
                .SetupGet(s => s.Sha)
                .Returns(new ShaFactory().GetShaInstance(new HashFunction(dsaKdfHashMode, dsaKdfDigestSize)));
            _dsa
                .Setup(s => s.GenerateKeyPair(domainParameters))
                .Returns(() => new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 1), 1)));

            var macParams = _macParamsBuilder
                .WithKeyAgreementMacType(macType)
                .WithMacLength(tagLength)
                .WithNonce(aesCcmNonce)
                .Build();

            var kas = _subject
                .WithKeyAgreementRole(keyAgreementRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(scheme, EccParameterSet.Eb, curveName))
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
                kas.Scheme.StaticKeyPair.PrivateD = thisPartyPrivateStaticKey;
                kas.Scheme.StaticKeyPair.PublicQ = thisPartyPublicStaticKey;
            }
            if (kas.Scheme.EphemeralKeyPair != null)
            {
                kas.Scheme.EphemeralKeyPair.PrivateD = thisPartyPrivateEphemKey;
                kas.Scheme.EphemeralKeyPair.PublicQ = thisPartyPublicEphemKey;
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
