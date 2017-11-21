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
                    new OtherInfoFactory<OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair>(
                        _entropyProviderOtherInfo
                    ),
                    _entropyProviderScheme,
                    new DiffieHellmanEcc(),
                    new MqvEcc()
                )
            );
        }

        #region ephemeralUnified
        #region ephemeralUnified, component only
        private static object[] _test_ephemUnifiedComponentOnly = new object[]
        {
            new object[]
            {
                // label
                "B233",
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
                // private ephem this party
                new BitString("000000c2ca50d96624bd2e6857c98a0d14206a4402ecb21cda2135c48c652844").ToPositiveBigInteger(),
                // public ephem this party
                new EccPoint(
                    new BitString("000000483b5e148ac734963d7aac2194c6e96294d83289ba77ef1edb45c0debd").ToPositiveBigInteger(), 
                    new BitString("0000014e8ca9033f04c5b102f37d4b253874a8bd54260fd35b61a32a800c9d79").ToPositiveBigInteger()
                ),
                // Other Party ID
                new BitString("434156536964"),
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
                "B233 inverse",
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
                // private ephem this party
                new BitString("00000080360eaea873fb66a2341845ce310f0b8b144deb1cb5e938520aedae52").ToPositiveBigInteger(),
                // public ephem this party
                new EccPoint(
                    new BitString("000000dfe9cd18b4613008e13a086e7a69b3752916829a32e06aa246cf91aa79").ToPositiveBigInteger(),
                    new BitString("000001c68825ce9f2355eaa86f7bc58235988c0d7ff5441acdf0eca1ee2494a9").ToPositiveBigInteger()
                ),
                // Other Party ID
                new BitString("a1b2c3d4e5"),
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
            }
        };

        [Test]
        [TestCaseSource(nameof(_test_ephemUnifiedComponentOnly))]
        public void ShouldDhEphemComponentOnlySha512Correctly(
            string label,
            Curve curveName,
            ModeValues modeValue,
            DigestSizes digestSize,
            KeyAgreementRole keyAgreementRole,
            BitString thisPartyId,
            BigInteger thisPartyPrivateEphemKey,
            EccPoint thisPartyPublicEphemKey,
            BitString otherPartyId,
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
                    null,
                    new EccKeyPair(otherPartyPublicEphemKey),
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
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(thisPartyPublicEphemKey, thisPartyPrivateEphemKey)));

            var kas = _subject
                .WithKeyAgreementRole(keyAgreementRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(EccScheme.EphemeralUnified, EccParameterSet.Eb, curveName))
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
        #endregion ephemeralUnified, component only
        #endregion ephemeralUnified

        #region ephemeralUnified, no key confirmation
        private static object[] _test_ephemeralUnifiedNoKeyConfirmation = new object[]
        {
            new object[]
            {
                // label
                "P-224 hmac",
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
                // thisPartyPrivateEphemKey
                new BitString("e935434303d842605d07112b3b7789ccbe4c6d987db5fa15ea1cdadb").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("784028a2246950401ec81f8e4e03fd3765c4da4d45eba652ed5ba2b5").ToPositiveBigInteger(),
                    new BitString("d071c32634bcf058f072493b6943453a0bd117f5ee5c5866f037f6ab").ToPositiveBigInteger()
                ),
                // otherPartyId
                new BitString("434156536964"),
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
                "P-224 hmac inverse",
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
                // thisPartyPrivateEphemKey
                new BitString("d58751997b3a551ccdc6f507bf6ab87e2be7f8267067a455a5815a54").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("c7893ced15c3009b93cb0abeaea2ede308b67fbbd902c6c5d94c24a4").ToPositiveBigInteger(),
                    new BitString("858afc2edd61fcefef3469dd5a004e74a3c727d16498a9408a8e3224").ToPositiveBigInteger()
                ),
                // otherPartyId
                new BitString("a1b2c3d4e5"),
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
            new object[]
            {
                // label
                "P-224 ccm",
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
                // thisPartyPrivateEphemKey
                new BitString("4ee19a1e8dafef8d84ad06f9a3176c7105d645fc61f66d5bb74175b2").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("2240e3d5ef61fc9e516f726bd4fd2d18caea337451ad42c07b97dcb6").ToPositiveBigInteger(),
                    new BitString("56700b229e6655d635d9af60923fff9bc21709c3308ec956d7ff35de").ToPositiveBigInteger()
                ),
                // otherPartyId
                new BitString("434156536964"),
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
                "P-224 ccm inverse",
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
                // thisPartyPrivateEphemKey
                new BitString("d8d47bd33e59d933a11f21b3ece887c6dab61bf398df4933b64ce48c").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("9ce739dfcf76b8f1d8472eb8bf7bdf3e60a72913578edfd2e5891a29").ToPositiveBigInteger(),
                    new BitString("4359df09d4435c3c20611e97d22e4a058cf249fa342c1993181bc569").ToPositiveBigInteger()
                ),
                // otherPartyId
                new BitString("a1b2c3d4e5"),
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
        };

        [Test]
        [TestCaseSource(nameof(_test_ephemeralUnifiedNoKeyConfirmation))]
        public void ShouldDhEphemNoKeyConfirmationCorrectly(
            string label,
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
            BigInteger thisPartyPrivateEphemKey,
            EccPoint thisPartyPublicEphemKey,
            BitString otherPartyId,
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
                    null,
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

            // The DSA sha mode determines which hash function to use in a component only test
            _dsa
                .SetupGet(s => s.Sha)
                .Returns(new ShaFactory().GetShaInstance(new HashFunction(dsaKdfHashMode, dsaKdfDigestSize)));
            _dsa
                .Setup(s => s.GenerateKeyPair(domainParameters))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(thisPartyPublicEphemKey, thisPartyPrivateEphemKey)));

            var macParams = _macParamsBuilder
                .WithKeyAgreementMacType(macType)
                .WithMacLength(tagLength)
                .WithNonce(aesCcmNonce)
                .Build();

            var kas = _subject
                .WithKeyAgreementRole(keyAgreementRole)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(EccScheme.EphemeralUnified, EccParameterSet.Eb, curveName))
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
        #endregion #region ephemeralUnified, no key confirmation
    }
}
