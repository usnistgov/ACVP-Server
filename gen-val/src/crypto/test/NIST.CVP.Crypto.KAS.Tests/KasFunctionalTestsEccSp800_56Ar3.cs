using System.Collections.Generic;
using System.Numerics;
using Moq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Builders;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Helpers;
using NIST.CVP.Crypto.CSHAKE;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.HKDF;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.IKEv1;
using NIST.CVP.Crypto.IKEv2;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.FixedInfo;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.KDF.OneStep;
using NIST.CVP.Crypto.KAS.Sp800_56Ar3.Builders;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.KMAC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.TLS;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests
{
    /// <summary>
    /// Tests against the same test cases from SP800-56Ar1 in <see cref="KasFunctionalTestsEcc" />
    /// </summary>
    public class KasFunctionalTestsEccSp800_56Ar3
    {
        private ISchemeBuilder _schemeBuilder;
        
        private ISecretKeyingMaterialBuilder _secretKeyingMaterialBuilderThisParty;
        private ISecretKeyingMaterialBuilder _secretKeyingMaterialBuilderOtherParty;

        private IKdfFactory _kdfFactory;
        private IKeyConfirmationFactory _keyConfirmationFactory;
        
        private MacParametersBuilder _macParamsBuilder = new MacParametersBuilder();
        private IKasBuilder _subject = new KasBuilder();
        
        private readonly IEccCurveFactory _curveFactory = new EccCurveFactory();
        
        
        [SetUp]
        public void Setup()
        {
            var shaFactory = new ShaFactory();
            var entropyFactory = new EntropyProviderFactory();
            
            var kdfVisitor = new KdfVisitor(
                new KdfOneStepFactory(shaFactory, new HmacFactory(shaFactory), new KmacFactory(new CSHAKEWrapper())),
                new Crypto.KDF.KdfFactory(new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()),
                    new HmacFactory(new ShaFactory())), new HmacFactory(new ShaFactory()),
                new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()),
                new IkeV1Factory(),
                new IkeV2Factory(new HmacFactory(new ShaFactory())),
                new TlsKdfFactory(),
                new HkdfFactory(new HmacFactory(new ShaFactory())));
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
            #region full unified
            new object[]
            {
                // label
                "fullUnified P224 hmac",
                // scheme
                EccScheme.FullUnified,
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
                KeyConfirmationDirection.Unilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("bf64447965afda01a63bdf4e5896a35bb2f2959f1d6affcaacbcec84").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("efc7a590b38e5eddb6d261c4e980d2789d1f50559d011d79344a71e9").ToPositiveBigInteger(),
                    new BitString("8b28ee9d31e28ef6ee4fa4c9ea0d68e1f37891275ea6a55931ce2522").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("413b6d21051c941f6e2356dd0ddf7b531610ebbf24714e9e5057bb74").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("79ed42fb82f0be7c8f966a078a08e24004c3791c3c548fef53ca87c6").ToPositiveBigInteger(),
                    new BitString("87925b6e670956f07fa019a3b2ecc492268e72e228ba72336a5ae70b").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("40131f44557303c9d1a6ac42514d829499e6b863e3a723efd22c2528").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("e868b8ede95e998f4f1b890d3ee9b546aea6b7f3da88caf20fb43d42").ToPositiveBigInteger(),
                    new BitString("2c3151ab1a057d4ac5631a876208376c924b76e76b57f88c0eb827ca").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("4f2f5ffa67781fa04cdfcd308b773cb13d12c3369fa68d6f648bca2a").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("7624b3d84120dd426bc074a21be201de76ba885e51741ea487ab2c5c").ToPositiveBigInteger(),
                    new BitString("25d2b0969c3728a282cec3fe64c71045cf123e4f245ce9500b9dfe5e").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("046c09fd63d2f2a3910190fef505af63deb819e6db948416fcbf89304956e2a9f395878f963c18bc197fbef349392f5fd5bf4731864741b4"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369645f1ec9fdf3d0b09a719ac253b5bcd00932ae058b86611aff51c8ca8448978615854b69b0"),
                // expectedDkm
                new BitString("9ce9dfc61f69d864b6b1133cb01b"),
                // expectedMacData
                new BitString("4b435f315f55a1b2c3d4e543415653696479ed42fb82f0be7c8f966a078a08e24004c3791c3c548fef53ca87c687925b6e670956f07fa019a3b2ecc492268e72e228ba72336a5ae70b7624b3d84120dd426bc074a21be201de76ba885e51741ea487ab2c5c25d2b0969c3728a282cec3fe64c71045cf123e4f245ce9500b9dfe5e"),
                // expectedTag
                new BitString("b8e68a044f36f699")
            },
            new object[]
            {
                // label
                "fullUnified P224 hmac inverse",
                // scheme
                EccScheme.FullUnified,
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
                KeyConfirmationDirection.Unilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("40131f44557303c9d1a6ac42514d829499e6b863e3a723efd22c2528").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("e868b8ede95e998f4f1b890d3ee9b546aea6b7f3da88caf20fb43d42").ToPositiveBigInteger(),
                    new BitString("2c3151ab1a057d4ac5631a876208376c924b76e76b57f88c0eb827ca").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("4f2f5ffa67781fa04cdfcd308b773cb13d12c3369fa68d6f648bca2a").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("7624b3d84120dd426bc074a21be201de76ba885e51741ea487ab2c5c").ToPositiveBigInteger(),
                    new BitString("25d2b0969c3728a282cec3fe64c71045cf123e4f245ce9500b9dfe5e").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,

                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("bf64447965afda01a63bdf4e5896a35bb2f2959f1d6affcaacbcec84").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("efc7a590b38e5eddb6d261c4e980d2789d1f50559d011d79344a71e9").ToPositiveBigInteger(),
                    new BitString("8b28ee9d31e28ef6ee4fa4c9ea0d68e1f37891275ea6a55931ce2522").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("413b6d21051c941f6e2356dd0ddf7b531610ebbf24714e9e5057bb74").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("79ed42fb82f0be7c8f966a078a08e24004c3791c3c548fef53ca87c6").ToPositiveBigInteger(),
                    new BitString("87925b6e670956f07fa019a3b2ecc492268e72e228ba72336a5ae70b").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,

                // expectedZ
                new BitString("046c09fd63d2f2a3910190fef505af63deb819e6db948416fcbf89304956e2a9f395878f963c18bc197fbef349392f5fd5bf4731864741b4"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369645f1ec9fdf3d0b09a719ac253b5bcd00932ae058b86611aff51c8ca8448978615854b69b0"),
                // expectedDkm
                new BitString("9ce9dfc61f69d864b6b1133cb01b"),
                // expectedMacData
                new BitString("4b435f315f55a1b2c3d4e543415653696479ed42fb82f0be7c8f966a078a08e24004c3791c3c548fef53ca87c687925b6e670956f07fa019a3b2ecc492268e72e228ba72336a5ae70b7624b3d84120dd426bc074a21be201de76ba885e51741ea487ab2c5c25d2b0969c3728a282cec3fe64c71045cf123e4f245ce9500b9dfe5e"),
                // expectedTag
                new BitString("b8e68a044f36f699")
            },
            #endregion full unified

            #region fullMqv
            new object[]
            {
                // label
                "fullMqv P224 hmac",
                // scheme
                EccScheme.FullMqv,
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
                KeyConfirmationRole.Recipient,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("a13226f2560825d9ccc656e27d6091d2c036f26abc3b57dc47478c9a").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("59caf428dea1b059f5b580c7adc246b056b99dab5c9fe012b6ec9257").ToPositiveBigInteger(),
                    new BitString("20d77996dc489ce0e557eeccb9ee3d55630c4538f8da0b6ead7deb8c").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("302c1c8a6a6b0df87445dadcbc99be525d970d351a189d99414416d5").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("77a411cb50d7a1da38675bfac72de2f16ca8c2e3c38c46f8b36fc8d0").ToPositiveBigInteger(),
                    new BitString("cffadbd23529b0b529c0b29c36ee6cee58b70cb291789a61b8c3ed42").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("a105eae1bb706d6bfee644f64e064d532740bc3f681056ca4c029296").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("c5d03ee658dbfb5f8787c00819aa44855cb3d47a6ddda74be7586704").ToPositiveBigInteger(),
                    new BitString("cdf7ffd06d781f68e3290ef13ff0fdea07a30a327af2419c2875bef1").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("c6ef9d41f90e1bffe71124720b782d85aa438fe423c4603a59130260").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("55dd195df9d078891b5d02d3e03bf06970086316deeebdf9b9ed6879").ToPositiveBigInteger(),
                    new BitString("f2a8177bc851912347342060a4662d0ffadcbdfb76c7080aaa586c7a").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("fe533ed3ca082bf4abe6ca3775c21192381743ff7c94f27088f526ee"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964d5385cb451bf18d14d128981704c2ddebf71c82bbe02aa09d8cf30ee010d8dd49aa8ca2e"),
                // expectedDkm
                new BitString("adca617377ecad9ea33e244f9110"),
                // expectedMacData
                new BitString("4b435f315f56434156536964a1b2c3d4e555dd195df9d078891b5d02d3e03bf06970086316deeebdf9b9ed6879f2a8177bc851912347342060a4662d0ffadcbdfb76c7080aaa586c7a77a411cb50d7a1da38675bfac72de2f16ca8c2e3c38c46f8b36fc8d0cffadbd23529b0b529c0b29c36ee6cee58b70cb291789a61b8c3ed42"),
                // expectedTag
                new BitString("d62c008cc35958da")
            },
            new object[]
            {
                // label
                "fullMqv P224 hmac inverse",
                // scheme
                EccScheme.FullMqv,
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
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("a105eae1bb706d6bfee644f64e064d532740bc3f681056ca4c029296").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("c5d03ee658dbfb5f8787c00819aa44855cb3d47a6ddda74be7586704").ToPositiveBigInteger(),
                    new BitString("cdf7ffd06d781f68e3290ef13ff0fdea07a30a327af2419c2875bef1").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("c6ef9d41f90e1bffe71124720b782d85aa438fe423c4603a59130260").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("55dd195df9d078891b5d02d3e03bf06970086316deeebdf9b9ed6879").ToPositiveBigInteger(),
                    new BitString("f2a8177bc851912347342060a4662d0ffadcbdfb76c7080aaa586c7a").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,

                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("a13226f2560825d9ccc656e27d6091d2c036f26abc3b57dc47478c9a").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("59caf428dea1b059f5b580c7adc246b056b99dab5c9fe012b6ec9257").ToPositiveBigInteger(),
                    new BitString("20d77996dc489ce0e557eeccb9ee3d55630c4538f8da0b6ead7deb8c").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("302c1c8a6a6b0df87445dadcbc99be525d970d351a189d99414416d5").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("77a411cb50d7a1da38675bfac72de2f16ca8c2e3c38c46f8b36fc8d0").ToPositiveBigInteger(),
                    new BitString("cffadbd23529b0b529c0b29c36ee6cee58b70cb291789a61b8c3ed42").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,

                // expectedZ
                new BitString("fe533ed3ca082bf4abe6ca3775c21192381743ff7c94f27088f526ee"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964d5385cb451bf18d14d128981704c2ddebf71c82bbe02aa09d8cf30ee010d8dd49aa8ca2e"),
                // expectedDkm
                new BitString("adca617377ecad9ea33e244f9110"),
                // expectedMacData
                new BitString("4b435f315f56434156536964a1b2c3d4e555dd195df9d078891b5d02d3e03bf06970086316deeebdf9b9ed6879f2a8177bc851912347342060a4662d0ffadcbdfb76c7080aaa586c7a77a411cb50d7a1da38675bfac72de2f16ca8c2e3c38c46f8b36fc8d0cffadbd23529b0b529c0b29c36ee6cee58b70cb291789a61b8c3ed42"),
                // expectedTag
                new BitString("d62c008cc35958da")
            },
            #endregion fullMqv

            #region onePassUnified
            new object[]
            {
                // label
                "onePassUnified P224 hmac",
                // scheme
                EccScheme.OnePassUnified,
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
                KeyConfirmationDirection.Unilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("d516aa59124b22a426a1564dcc9120a7b2983e6cd509b726ae0dca9f").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("28007e7a035fffcdd9793a48f99f7ebb201726a51add6967eb0a0380").ToPositiveBigInteger(),
                    new BitString("bbbe9e522cd8a6163b4a03b546838a81ddb8ce7d0847017f07e8f35d").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("f59096f7cec93b1442cffb9a27ef849fd43f7019efeaec71a6520df4").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("4ae5ee07691b2655a3c1c3b6b41b2725d54bea28240b787dbd6c6f33").ToPositiveBigInteger(),
                    new BitString("c31c91447ced819f902dcffed894b67da45ede0aca0508fdbbcb433f").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("b445eef72bc8c3e7d09af4ff6dde76a02317dae149d32b252dc714eb").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("d3bb8302551df53d82e78af4eef154df5b8363ae633fa4795c66dc86").ToPositiveBigInteger(),
                    new BitString("dac05c91159e657cbf3c9119f9189876e00a44e5a67217a023ab00fe").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("").ToPositiveBigInteger(),
                    new BitString("").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                new BitString("26dd29a014591bd78aedbf0ebe069c3e1a711abfe37145001c0fce7ba8b252e8b0efdfb144a2b5a7453e49f38261904f21ac797641d1bcd8"),
                // expectedZ
                new BitString("5a2b672094f591810be90ebf0c061faa0e630faabc900b118744bb7694f345eb97b2593b6eafef4c29102c48928073a843370b1d0c6b309d"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964b6bf2797bf8d45e51be53669d048c7c7bea47d8525cb341d6824065011a06ebe14f0adb4"),
                // expectedDkm
                new BitString("033da1261b5def0b27b1964d8bc3"),
                // expectedMacData
                new BitString("4b435f315f55a1b2c3d4e54341565369644ae5ee07691b2655a3c1c3b6b41b2725d54bea28240b787dbd6c6f33c31c91447ced819f902dcffed894b67da45ede0aca0508fdbbcb433f26dd29a014591bd78aedbf0ebe069c3e1a711abfe37145001c0fce7ba8b252e8b0efdfb144a2b5a7453e49f38261904f21ac797641d1bcd8"),
                // expectedTag
                new BitString("b6f41de5d2cd1b58")
            },
            new object[]
            {
                // label
                "onePassUnified P224 hmac inverse",
                // scheme
                EccScheme.OnePassUnified,
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
                KeyConfirmationDirection.Unilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("b445eef72bc8c3e7d09af4ff6dde76a02317dae149d32b252dc714eb").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("d3bb8302551df53d82e78af4eef154df5b8363ae633fa4795c66dc86").ToPositiveBigInteger(),
                    new BitString("dac05c91159e657cbf3c9119f9189876e00a44e5a67217a023ab00fe").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("").ToPositiveBigInteger(),
                    new BitString("").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                new BitString("26dd29a014591bd78aedbf0ebe069c3e1a711abfe37145001c0fce7ba8b252e8b0efdfb144a2b5a7453e49f38261904f21ac797641d1bcd8"),

                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("d516aa59124b22a426a1564dcc9120a7b2983e6cd509b726ae0dca9f").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("28007e7a035fffcdd9793a48f99f7ebb201726a51add6967eb0a0380").ToPositiveBigInteger(),
                    new BitString("bbbe9e522cd8a6163b4a03b546838a81ddb8ce7d0847017f07e8f35d").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("f59096f7cec93b1442cffb9a27ef849fd43f7019efeaec71a6520df4").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("4ae5ee07691b2655a3c1c3b6b41b2725d54bea28240b787dbd6c6f33").ToPositiveBigInteger(),
                    new BitString("c31c91447ced819f902dcffed894b67da45ede0aca0508fdbbcb433f").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,

                // expectedZ
                new BitString("5a2b672094f591810be90ebf0c061faa0e630faabc900b118744bb7694f345eb97b2593b6eafef4c29102c48928073a843370b1d0c6b309d"),
                // expectedOi
                new BitString("a1b2c3d4e5434156536964b6bf2797bf8d45e51be53669d048c7c7bea47d8525cb341d6824065011a06ebe14f0adb4"),
                // expectedDkm
                new BitString("033da1261b5def0b27b1964d8bc3"),
                // expectedMacData
                new BitString("4b435f315f55a1b2c3d4e54341565369644ae5ee07691b2655a3c1c3b6b41b2725d54bea28240b787dbd6c6f33c31c91447ced819f902dcffed894b67da45ede0aca0508fdbbcb433f26dd29a014591bd78aedbf0ebe069c3e1a711abfe37145001c0fce7ba8b252e8b0efdfb144a2b5a7453e49f38261904f21ac797641d1bcd8"),
                // expectedTag
                new BitString("b6f41de5d2cd1b58")
            },
            #endregion onePassUnified

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
                // dkmNonce other party
                null,
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
                // dkmNonce other party
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
                // dkmNonce other party
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
                // dkmNonce other party
                null,
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
            #endregion onePassMqv

            #region onePassDh
            new object[]
            {
                // label
                "onePassDh P224 hmac",
                // scheme
                EccScheme.OnePassDh,
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
                KeyConfirmationRole.Recipient,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("a1b2c3d4e5"),
                // thisPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                null,
                // thisPartyPrivateEphemKey
                new BitString("0fcc94775a976e3f39e45e733ccfe5d6ca27a86f51688ceeb4cae803").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("99c8ad0ed50787053a962c21d1942cc69f2adb327078c8a2a041d87f").ToPositiveBigInteger(),
                    new BitString("8b108bfbfe63d15b87ba3f97efff7d3918c05d8afdd9daa468d3fda8").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("1d73f478d76322bf30410392f561e49b8dd4420aad0076a0e2c23d4f").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("405a354518867d4441719ec73fc8456bf9d7a5e1c18a5f16b02f84e2").ToPositiveBigInteger(),
                    new BitString("1d609ef8513ec4bdeddc281ab7aff0969397bc05e9e7e27952bce6bf").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("1fd40be3414f0981766848fef8a4fff57864b378b1ed7704b97be463"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369646f5791408f3d4c049f3a15aed63488746236d2e66568cd7e3d3f6c7808f15ae3672cdef9"),
                // expectedDkm
                new BitString("6817769ab5fe5875250446959a33"),
                // expectedMacData
                new BitString("4b435f315f56434156536964a1b2c3d4e599c8ad0ed50787053a962c21d1942cc69f2adb327078c8a2a041d87f8b108bfbfe63d15b87ba3f97efff7d3918c05d8afdd9daa468d3fda8"),
                // expectedTag
                new BitString("87ea941c03cc5e8e")
            },
            new object[]
            {
                // label
                "onePassDh P224 hmac inverse",
                // scheme
                EccScheme.OnePassDh,
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
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                112,
                // tagLength
                64,
                // aesCcmNonce
                null,
                
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("1d73f478d76322bf30410392f561e49b8dd4420aad0076a0e2c23d4f").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("405a354518867d4441719ec73fc8456bf9d7a5e1c18a5f16b02f84e2").ToPositiveBigInteger(),
                    new BitString("1d609ef8513ec4bdeddc281ab7aff0969397bc05e9e7e27952bce6bf").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,

                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                null,
                // otherPartyPrivateEphemKey
                new BitString("0fcc94775a976e3f39e45e733ccfe5d6ca27a86f51688ceeb4cae803").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("99c8ad0ed50787053a962c21d1942cc69f2adb327078c8a2a041d87f").ToPositiveBigInteger(),
                    new BitString("8b108bfbfe63d15b87ba3f97efff7d3918c05d8afdd9daa468d3fda8").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,

                // expectedZ
                new BitString("1fd40be3414f0981766848fef8a4fff57864b378b1ed7704b97be463"),
                // expectedOi
                new BitString("a1b2c3d4e54341565369646f5791408f3d4c049f3a15aed63488746236d2e66568cd7e3d3f6c7808f15ae3672cdef9"),
                // expectedDkm
                new BitString("6817769ab5fe5875250446959a33"),
                // expectedMacData
                new BitString("4b435f315f56434156536964a1b2c3d4e599c8ad0ed50787053a962c21d1942cc69f2adb327078c8a2a041d87f8b108bfbfe63d15b87ba3f97efff7d3918c05d8afdd9daa468d3fda8"),
                // expectedTag
                new BitString("87ea941c03cc5e8e")
            },
            #endregion onePassDh

            #region StaticUnified
            #region hmac
            new object[]
            {
                // label
                "StaticUnified P224 hmac",
                // scheme
                EccScheme.StaticUnified,
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
                new BitString("b8706f099ef246aa5fcdaa2062d8820b8f2d0749fe6fd076939e4ca2").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("8eb82fd84b257cbe16bebb643d590e65198e82cdab32a5f2196edc45").ToPositiveBigInteger(),
                    new BitString("df3c23bd8c61aaf6a983fc31ceaf38c3ddeb5614338c289c3865f66e").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                new BitString("34e74cd386476a01a91a80533919"),
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("baa3488d9499c4409e57a7f9efdc7b23eee14c6719e04ffda48e3729").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("b8a97d2a2d8ce6c58537afc41027cf9863aac3cf225533e5eba90e37").ToPositiveBigInteger(),
                    new BitString("e5ff363350ba18ea7b4b3d178e7f9b967b8440e3b82b01b30e83dc3f").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                new BitString("836a4e8914ef58e0f3e8f1cd3f03daa15b2e1ec84d64984264c7b5581ef07b874883cac16706e2d557f3262b320b20b474eec4215ce9f953"),
                // expectedZ
                new BitString("a997f2f38776f1e59dc47a2f3865fad252bcf0954138c3e8ff59ba6f"),
                // expectedOi
                new BitString("a1b2c3d4e534e74cd386476a01a91a80533919434156536964e204a7177157f793f51806ad1a9beee2eefe63ebde51"),
                // expectedDkm
                new BitString("7d740e1d93e4fc2b53df5038c930"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e543415653696434e74cd386476a01a91a80533919836a4e8914ef58e0f3e8f1cd3f03daa15b2e1ec84d64984264c7b5581ef07b874883cac16706e2d557f3262b320b20b474eec4215ce9f953"),
                // expectedTag
                new BitString("0d49be39bb9ee470")
            },
            new object[]
            {
                // label
                "StaticUnified P224 hmac inverse",
                // scheme
                EccScheme.StaticUnified,
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
                new BitString("baa3488d9499c4409e57a7f9efdc7b23eee14c6719e04ffda48e3729").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("b8a97d2a2d8ce6c58537afc41027cf9863aac3cf225533e5eba90e37").ToPositiveBigInteger(),
                    new BitString("e5ff363350ba18ea7b4b3d178e7f9b967b8440e3b82b01b30e83dc3f").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                new BitString("836a4e8914ef58e0f3e8f1cd3f03daa15b2e1ec84d64984264c7b5581ef07b874883cac16706e2d557f3262b320b20b474eec4215ce9f953"),
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("b8706f099ef246aa5fcdaa2062d8820b8f2d0749fe6fd076939e4ca2").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("8eb82fd84b257cbe16bebb643d590e65198e82cdab32a5f2196edc45").ToPositiveBigInteger(),
                    new BitString("df3c23bd8c61aaf6a983fc31ceaf38c3ddeb5614338c289c3865f66e").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                new BitString("34e74cd386476a01a91a80533919"),
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                new BitString("a997f2f38776f1e59dc47a2f3865fad252bcf0954138c3e8ff59ba6f"),
                // expectedOi
                new BitString("a1b2c3d4e534e74cd386476a01a91a80533919434156536964e204a7177157f793f51806ad1a9beee2eefe63ebde51"),
                // expectedDkm
                new BitString("7d740e1d93e4fc2b53df5038c930"),
                // expectedMacData
                new BitString("4b435f325f55a1b2c3d4e543415653696434e74cd386476a01a91a80533919836a4e8914ef58e0f3e8f1cd3f03daa15b2e1ec84d64984264c7b5581ef07b874883cac16706e2d557f3262b320b20b474eec4215ce9f953"),
                // expectedTag
                new BitString("0d49be39bb9ee470")
            },
            #endregion hmac

            #endregion StaticUnified
        };

        [Test]
        [TestCaseSource(nameof(_test_keyConfirmation))]
        public void ShouldKeyConfirmationCorrectly(
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
            BitString thisPartyDkmNonce,
            BitString thisPartyEphemeralNonce,
            BitString otherPartyId,
            BigInteger otherPartyPrivateStaticKey,
            EccPoint otherPartyPublicStaticKey,
            BigInteger otherPartyPrivateEphemKey,
            EccPoint otherPartyPublicEphemKey,
            BitString otherPartyDkmNonce,
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

            KasEnumMapping.EccMap.TryFirst(x => x.Key == scheme, out var schemeResult);
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

            EccKeyPair thisPartyEphemKey = GetKey(thisPartyPublicEphemKey, thisPartyPrivateEphemKey);
            EccKeyPair thisPartyStaticKey = GetKey(thisPartyPublicStaticKey, thisPartyPrivateStaticKey);

            EccKeyPair otherPartyEphemKey = GetKey(otherPartyPublicEphemKey, otherPartyPrivateEphemKey);;
            EccKeyPair otherPartyStaticKey = GetKey(otherPartyPublicStaticKey, otherPartyPrivateStaticKey);
            
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

            Assert.AreEqual(expectedZ, result.Z, nameof(result.Z));
            Assert.AreEqual(expectedDkm, result.Dkm, nameof(result.Dkm));
            Assert.AreEqual(expectedMacData, result.MacData, nameof(result.MacData));
            Assert.AreEqual(expectedTag, result.Tag, nameof(result.Tag));
        }

        private KasKdfOneStepAuxFunction GetAuxFunction(ModeValues dsaKdfHashMode, DigestSizes dsaKdfDigestSize)
        {
            List<(ModeValues hashMode, DigestSizes digestSize, KasKdfOneStepAuxFunction function)> list =
                new List<(ModeValues hashMode, DigestSizes digestSize, KasKdfOneStepAuxFunction function)>()
                {
                    (ModeValues.SHA2, DigestSizes.d224, KasKdfOneStepAuxFunction.SHA2_D224),
                    (ModeValues.SHA2, DigestSizes.d256, KasKdfOneStepAuxFunction.SHA2_D256),
                    (ModeValues.SHA2, DigestSizes.d384, KasKdfOneStepAuxFunction.SHA2_D384),
                    (ModeValues.SHA2, DigestSizes.d512, KasKdfOneStepAuxFunction.SHA2_D512),
                    (ModeValues.SHA3, DigestSizes.d224, KasKdfOneStepAuxFunction.SHA3_D224),
                    (ModeValues.SHA3, DigestSizes.d256, KasKdfOneStepAuxFunction.SHA3_D256),
                    (ModeValues.SHA3, DigestSizes.d384, KasKdfOneStepAuxFunction.SHA3_D384),
                    (ModeValues.SHA3, DigestSizes.d512, KasKdfOneStepAuxFunction.SHA3_D512),
                };

            list.TryFirst(f => f.digestSize == dsaKdfDigestSize && f.hashMode == dsaKdfHashMode, out var result);

            return result.function;
        }


        private EccKeyPair GetKey(EccPoint publicKey, BigInteger privateKey)
        {
            if (privateKey != 0 && publicKey != null)
            {
                return new EccKeyPair(publicKey, privateKey);
            }

            return null;
        }
    }
}