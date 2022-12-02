using System.Collections.Generic;
using System.Numerics;
using Moq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.HKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.IKEv1;
using NIST.CVP.ACVTS.Libraries.Crypto.IKEv2;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.FixedInfo;
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
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests
{
    /// <summary>
    /// Tests against the same test cases from SP800-56Ar1 in <see cref="KasFunctionalTestsEcc" />
    /// </summary>
    public class KasFunctionalTestsEccSp800_56Ar3
    {
        private static object[] _test_keyConfirmation = {
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
            #region Additional Tests
            new object[]
            {
                // label
                "working test from vs",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.B409,
                // keyAgreementRole
                KeyAgreementRole.InitiatorPartyU,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d384,
                // macType
                KeyAgreementMacType.HmacSha2D384,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                192,
                // tagLength
                128,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("00E7233A212FB43EE637202BAFBAF7A091ED10813DC60DC3CBD56A0155CCA1CA0A25F775FE6232E737A23655B9763D59597C85BD").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("000E103B086AC76E472A7B45842B89AA572B34B87599DF749AE0591EEE1168EC9120C6B366C6F7E6559397A86B78AA47175C75EB").ToPositiveBigInteger(),
                    new BitString("009480400218DFEBEAE0B15FA4064EC02BF251F35DE3ABFF91E2D9BCD759DD1D839016D2F3443F827DF37001170E101610FF8013").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00C6DF4A8BBD99C85A57CDED34039FB233E93D53F22CCC61198512D295624348473B304B5675C7905F5CFBB5B8BDC8EA56E3B64C").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("0172FF5F701104355F304C39135643E141601F054BA7165BAC750744FE5691CA2BB4583FF2D29C5DE705958F75F1B25EA09C0C97").ToPositiveBigInteger(),
                    new BitString("00F25385054D3E19C9D8E219651C4B721540695DF0487FDDD0425AC90DCEDF173F9CB805EDF44E37FACD073E74242372023DE621").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("cfd42c6561"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("01dccc77900e5b9d30af936b60a9d75c2d8b2481535f5309a31778e732ddbadf9b2e634703d61543c270c5bac83077f5293bdd69").ToPositiveBigInteger(),
                    new BitString("01dfb27a0712092b13c1c42616e14d67cc3ae96753fd679001b0cfa4f99693d0145b5dc7ff545306ae351e8a5851e91712b16c2f").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                new BitString("22a77e474945fc29b0b763cc8248ba4419ade9b2925f1f0a376cc7dbf17a0fa5bcbcbaf0235b75f3c96d558ca41b6333a43e165e"),
                // expectedZ
                null,
                // expectedOi
                new BitString("434156536964cfd42c65618b5be1fe60c8c7b6b296c7f9dd95723665c217c41901113f370b474f448fd4c0d41515d4"),
                // expectedDkm
                new BitString("7fbec7e6e329e263f5a111f3085deeb96810dffdaeae7201"),
                // expectedMacData
                null,
                // expectedTag
                new BitString("5211226d40ad22466f00d7bac5f1c7fa")
            },
            new object[]
            {
                // label
                "broken test from vs",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.B409,
                // keyAgreementRole
                KeyAgreementRole.ResponderPartyV,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d384,
                // macType
                KeyAgreementMacType.HmacSha2D384,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Bilateral,
                // keyLength
                192,
                // tagLength
                128,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("00BAB618FC65189F8D34E1639F986111B1114F68E2A5186B4A477920404ADE5DA9E17AC685D89EA16F0088F29C1123AA1D5E35AB").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("012FE188907418A2A3908E387C5651E4A8DADE4BC87A229A03E2FF6F4EE31BABF1BC09A228F8E3758BB9E0CBF4F8B79C36840FDB").ToPositiveBigInteger(),
                    new BitString("011190DFE859474A8216264949B6AA42E22F50900CFD46D96A1558ABED86237E64F190D5BF82B542E698E53FE39F222EFF98F600").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                new BitString("057A81200E0E7DD00E9AAB63F089BF6B23E9DCBED36E7377972C365A1D37D842E78C1FEE50C606B3D12B33EBB419758C45D8605F8FA4CE652911E654C24EFB02861F7D265605038449AC23E1939C497D40F8E9187A5EA224B2B633948A901FF4CC8787FD56B680"),
                // otherPartyId
                new BitString("6b606d9a64"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("017a34ad2769e1a68bca80c0d3b5a3d6d980cc9b81af02f4c7a4dbef72e6c3817bcd61660e71f883de3eb891d24b03d7c4a355d3").ToPositiveBigInteger(),
                    new BitString("008a111fcd9b24dc28c318e9832e598579a5757d81beeab2cc9abbecb8f22194bea467970de0e99b2e9fea7b53dfde457a09ac3d").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("013c06618a28208b1acdc7a233c53d1e0e9ea77c2dc71c45e703f53bbdc9bacb75104a8be3eb8524cfc4676ceda8dd0faf92f141").ToPositiveBigInteger(),
                    new BitString("0146c14a61ee963602f40289769e2418b02bfc3c778fe8e4a70c5615482bba781e337169126e70b0ad9af05acf81164b033f2bd6").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                null,
                // expectedOi
                new BitString("6b606d9a64434156536964ac58b58d8a93dd86c698f0d97734fec1c7af8541e92229ce48d1796a1b4ef8666181b9ab"),
                // expectedDkm
                new BitString("792febdd0e0f9fa4b2b2b5d902ec9cb9238646b249fafb8e"),
                // expectedMacData
                new BitString("4b435f325f564341565369646b606d9a64057a81200e0e7dd00e9aab63f089bf6b23e9dcbed36e7377972c365a1d37d842e78c1fee50c606b3d12b33ebb419758c45d8605f8fa4ce652911e654c24efb02861f7d265605038449ac23e1939c497d40f8e9187a5ea224b2b633948a901ff4cc8787fd56b680013c06618a28208b1acdc7a233c53d1e0e9ea77c2dc71c45e703f53bbdc9bacb75104a8be3eb8524cfc4676ceda8dd0faf92f1410146c14a61ee963602f40289769e2418b02bfc3c778fe8e4a70c5615482bba781e337169126e70b0ad9af05acf81164b033f2bd6"),
                // expectedTag
                new BitString("7071549402f33c56537b4a2e86af2942")
            },
            new object[]
            {
                // label
                "broken test from vs 2",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.B409,
                // keyAgreementRole
                KeyAgreementRole.ResponderPartyV,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d384,
                // macType
                KeyAgreementMacType.HmacSha2D384,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Bilateral,
                // keyLength
                192,
                // tagLength
                128,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("0094E99F7413BC9FAB5EFFDA8F8C267BB4C2D0D9FF565D2CE9DCFE73B46DBF9F94403EB46028B99DBD7D9AF7918DC040C436A226").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("00120D018CD0D97C4D357812F3C1CBCA195629666E25D36005FD8A542DBF42C83B56952C2999F0D1F7ADE7C879D0067EADA3729D").ToPositiveBigInteger(),
                    new BitString("018AE7B24209B970600AAF3B386E17E02165C862DD330AC46033B70BF68137BA2F93B88D23A04316F4FB1B2D94C128C83549DB0E").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                new BitString("5F024066397ADEEFA49776BF05613816773E8590D2E05423BF54DFA3C7678618BD3702848544D3826F6D19972769E912DC2A618EEE0E6B61C6C5EB0CFD4E5CFE2E9DE92EB3D440D8CD73DF3E025AE96A61DD326425F4F6A625B698C65DA0F64EAB5BF210940B00"),
                // otherPartyId
                new BitString("71f5451ec8"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("006bbad541403bcfe900deecd71f790e287b3ca3777fc010bf34f0b42ae848e98379a81f1a0aad8c7308bf1c6fd6d9b32578be23").ToPositiveBigInteger(),
                    new BitString("00626812fc54d53de968b5ec92f8943dc5a75abb58156f90cf42ec2ac9ee464b30747ec8d466c208386d773130ccd4f28abf5c63").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString("01b6cdbe1dbf16c3bac71a42597444511ec3d5e1f76e19194d058088b264dd3a8cb8202fddc22d4d7609fa1c6a723fe943e03b95").ToPositiveBigInteger(),
                    new BitString("000ae1fa9ee2696510964180237f926ce8a1a66da3a9b121ae60a3fe7fa71d9de2ecad7d493377caba0a265574b88152dc1b6ed6").ToPositiveBigInteger()
                ),
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                null,
                // expectedZ
                null,
                // expectedOi
                new BitString("71f5451ec84341565369643f835d2c1b2644d451fe4c65bab63398eee5c44e3654d83cb71ef6316134c6749c3c5791"),
                // expectedDkm
                new BitString("99301c57ec3754807df5fddfd2e8559d50e639b2f35515a5"),
                // expectedMacData
                null,
                // expectedTag
                new BitString("34a89d93cad928180c0e3e9faa7a1bcc")
            },
            new object[]
            {
                // label
                "passing test with iut provided ephemeral nonce",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.B409,
                // keyAgreementRole
                KeyAgreementRole.InitiatorPartyU,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d384,
                // macType
                KeyAgreementMacType.HmacSha2D384,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                192,
                // tagLength
                128,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("00E7233A212FB43EE637202BAFBAF7A091ED10813DC60DC3CBD56A0155CCA1CA0A25F775FE6232E737A23655B9763D59597C85BD").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("000E103B086AC76E472A7B45842B89AA572B34B87599DF749AE0591EEE1168EC9120C6B366C6F7E6559397A86B78AA47175C75EB").ToPositiveBigInteger(),
                    new BitString("009480400218DFEBEAE0B15FA4064EC02BF251F35DE3ABFF91E2D9BCD759DD1D839016D2F3443F827DF37001170E101610FF8013").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00C6DF4A8BBD99C85A57CDED34039FB233E93D53F22CCC61198512D295624348473B304B5675C7905F5CFBB5B8BDC8EA56E3B64C").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("0172FF5F701104355F304C39135643E141601F054BA7165BAC750744FE5691CA2BB4583FF2D29C5DE705958F75F1B25EA09C0C97").ToPositiveBigInteger(),
                    new BitString("00F25385054D3E19C9D8E219651C4B721540695DF0487FDDD0425AC90DCEDF173F9CB805EDF44E37FACD073E74242372023DE621").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("cfd42c6561"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("01dccc77900e5b9d30af936b60a9d75c2d8b2481535f5309a31778e732ddbadf9b2e634703d61543c270c5bac83077f5293bdd69").ToPositiveBigInteger(),
                    new BitString("01dfb27a0712092b13c1c42616e14d67cc3ae96753fd679001b0cfa4f99693d0145b5dc7ff545306ae351e8a5851e91712b16c2f").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                new BitString("22a77e474945fc29b0b763cc8248ba4419ade9b2925f1f0a376cc7dbf17a0fa5bcbcbaf0235b75f3c96d558ca41b6333a43e165e"), 
                // expectedZ
                null,
                // expectedOi
                new BitString("434156536964cfd42c65618b5be1fe60c8c7b6b296c7f9dd95723665c217c41901113f370b474f448fd4c0d41515d4"),
                // expectedDkm
                new BitString("7fbec7e6e329e263f5a111f3085deeb96810dffdaeae7201"),
                // expectedMacData
                null,
                // expectedTag
                new BitString("5211226d40ad22466f00d7bac5f1c7fa")
            },
            new object[]
            {
                // label
                "val test",
                // scheme
                EccScheme.OnePassMqv,
                // curveName
                Curve.B409,
                // keyAgreementRole
                KeyAgreementRole.InitiatorPartyU,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d384,
                // macType
                KeyAgreementMacType.HmacSha2D384,
                // keyConfirmationRole
                KeyConfirmationRole.Provider,
                // keyConfirmationDirection
                KeyConfirmationDirection.Unilateral,
                // keyLength
                192,
                // tagLength
                128,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("434156536964"),
                // thisPartyPrivateStaticKey
                new BitString("00A315FC644BEE86E61253323302889E0E6E7DF0553CF367A8217D77D62B53DFC4CF7CABFD41F55D497D637016448D2C7A58323C").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString("019D9AC69D63F472733FCEA108F5B4620F08E9E675082D9620197B8480CEEF14EFE6DE6FD7FB07217063A67513F82A4DF8F43EF3").ToPositiveBigInteger(),
                    new BitString("018E5BF863639989BD712C8278543C595A72FD66D86BBD9A64DEA4A4A1C1A28B3CB00332F4D880EED3BD7E00A605D226A3070077").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString("00831C0B591426A443FD21A3E9080B17EB701FFA95012209841A49C8932738A0603FAD1960F4BCA1BD961A2FFA96E47FEF376CE6").ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("01308E09994AB2B7F0849CFCB755187158A0774AA8926CB1DA50C96A552A043ED27D59EE47598926991DEFCE489BD32DF2EC8A33").ToPositiveBigInteger(),
                    new BitString("007B3D2A57DC787957674299617218ABC9AB378ED5C3A4BEC8DEF9E11F95498568DEC887D093D3B74309AB8E36B85FF128351D4F").ToPositiveBigInteger()
                ),
                // dkmNonce this party
                null,
                // thisPartyEphemeralNonce
                null,
                // otherPartyId
                new BitString("a1b2c3d4e5"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                new EccPoint(
                    new BitString("00F284F1BED2B959DAC0FE46FFEDE99EDECC8A4A90983B6501BB3A2E3AF75E8DC394ACF4CA694E3B1A293C5DE1428F5ACAFEFD49").ToPositiveBigInteger(),
                    new BitString("012B3EEE38EEE409F2298B7D697C97129ED34F79C8C3BCBE7E3F9D47C5D21924B6DEACC425871902C9CAE0FAEA179D3676ADF6C3").ToPositiveBigInteger()
                ),
                // otherPartyPrivateEphemKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                null,
                // dkmNonce other party
                null,
                // otherPartyEphemeralNonce
                new BitString("824AF45EC5B4FAAD1127B9A14DB710CC9B074DB1F0D9F9012475424B4F89C8622515B68A838A946C83BDC7C8817B05FC5F7EA4188D25A938D577E959E17E2B18F03ADC93B3704B7F5FDAD75A128EB90DCF63897AE2F89D01A92A3ACF09F4475AD81A573A491500"), 
                // expectedZ
                null,
                // expectedOi
                new BitString("434156536964A1B2C3D4E541378881DF6738743957CAC1A2AC70F19F1F6444AC0A4AA21E758DCB8EDFED7C2D9E0F18"),
                // expectedDkm
                new BitString("648AE6180D8B2767E83D01929C3DCE424E51506F9B6197A4"),
                // expectedMacData
                null,
                // expectedTag
                new BitString("6da6d68cc7351814a6ab709bc4d78aae")
            },
            #endregion Additional Tests
        };

        private static IEnumerable<object[]> _test_noKeyConfirmation = new[]
        {
            new object[]
            {
                // label
                "https://github.com/usnistgov/ACVP/issues/941",
                // scheme
                EccScheme.OnePassDh,
                // curveName
                Curve.P384,
                // keyAgreementRole
                KeyAgreementRole.ResponderPartyV,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d384,
                // macType
                KeyAgreementMacType.None,
                // keyLength
                1024,
                // aesCcmNonce
                null,
                // thisPartyId
                new BitString("1234567890ABCDEF"),
                // thisPartyPrivateStaticKey
                new BitString("9BC987D110653608CFDAD2E42CA8D70C82065628262BBEA57790B2736A9E08595D8ABA1702578BC97B13147DDD06FBF2").ToPositiveBigInteger(),
                // thisPartyPublicStaticKey
                new EccPoint(
                    new BitString(
                            "C229BA54D0831CFA6956B4BC0AAA1F45E657B3EB89F452110D6C6A36290262899325AF0E1B55BBD3F80D9404E0EACB6D").ToPositiveBigInteger(),
                    new BitString(
                            "94CC09EDB22CF0C8C43335CC5C722C3EB1BA303103864F62257A272C471E0BC225215255DBD8074138078D2DD663040B").ToPositiveBigInteger()
                ),
                // thisPartyPrivateEphemKey
                new BitString(
                        "00")
                    .ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                null,
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                new BitString("00").ToPositiveBigInteger(),
                // otherPartyPublicStaticKey
                null,
                // otherPartyPrivateEphemKey
                new BitString(
                        "3A2E6557D6F12CF36ADFD77C959D560157C68EA5625839C36DBA22199B7505C19405C4A3F32937506C13809C6B1E77FD")
                    .ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString(
                            "12AD7CE2A59268153E79ECC8234577073F2EFC99EDE450CFFD910318C8FD368EA128548659956CFE4BAFF46EAA4687DC")
                        .ToPositiveBigInteger(),
                    new BitString(
                            "756B19DF266BDEB5112E3C5916C2BEA1CDC62EACADD47151BF2419582FB6D9CCAB132B72A92C946AF39F28DFADF14D77")
                        .ToPositiveBigInteger()
                ),
                // expectedZ
                new BitString(
                    "0791B6A73F89C54CD17060B69C221D7A93AFDBA2159CB2C90BFF315D251F8EB94488A9A1694AF35329EB5DED47BA5F08"),
                // expectedDkm
                new BitString(
                    "80B1AEFD67CB13881256C823086C0041565FBD4D8D5728015A3EA39AE0F199B061C30F2C77C639C01553C233BA85EF8E1EAE175237C69D26F12464AFDDAB0DED64FF9F9F399CE3DBB680D65BC1C0752ABA4E33E587FEF1129EAAF3E7E30FBC21A500AAF7732EB52CACDEB3F2113260D805D873DBB4CAAD162EBCF6F8D43413D5"),
            },
        };

        private static IEnumerable<object> _test_twoStepNoKeyConfirmationIntermediateValues = new List<object>
        {
            new object[]
            {
                "https://admin.demo.acvts.nist.gov/api/testSessions/vectorSet/418219/json/InternalProjection tcId1",
                // scheme
                EccScheme.EphemeralUnified,
                // curveName
                Curve.P256,
                // keyAgreementRole
                KeyAgreementRole.InitiatorPartyU,
                // dsaKdfHashMode
                ModeValues.SHA2,
                // dsaKdfDigestSize
                DigestSizes.d256,
                // macType
                KeyAgreementMacType.None,
                // keyLength
                256,
                
                // salt
                new BitString("DED045F5E00EDAEE0703981173DCA31D74538270A7E48725871859C4719CB426"),
                // kdf mode
                KdfModes.Counter,
                // mac mode
                MacModes.HMAC_SHA256,
                // counter location
                CounterLocations.BeforeFixedData,
                // counter length
                8,
                // fixed info pattern
                "literal[123456]||uPartyInfo||vPartyInfo",
                
                // thisPartyId
                new BitString("ACEECD"),
                // thisPartyPrivateStaticKey
                null,
                // thisPartyPublicStaticKey
                null,
                // thisPartyPrivateEphemKey
                new BitString(
                        "EA59C958D88FD7F14AC71C76A80AE337B1DA3DE85F98AEE2775C510E46E377DB")
                    .ToPositiveBigInteger(),
                // thisPartyPublicEphemKey
                new EccPoint(
                    new BitString("9D0B05CFF7E49C981A04D758D714D14220FFF90D1B8C51A3834662AE71571B20").ToPositiveBigInteger(),
                    new BitString("A25AB71D3CFE49E33F0B2B83C9BAF70CE20B380739E4A8E63A2D8B8676C3AD30").ToPositiveBigInteger()), 
                // otherPartyId
                new BitString("434156536964"),
                // otherPartyPrivateStaticKey
                null,
                // otherPartyPublicStaticKey
                null,
                // otherPartyPrivateEphemKey
                new BitString(
                        "DEA161990574813EC33FEC8E05520AE250CC3C9F3ED0D2BA9521BECC08F912CF")
                    .ToPositiveBigInteger(),
                // otherPartyPublicEphemKey
                new EccPoint(
                    new BitString(
                            "A1EA78721AE8C8FBE2C785646307EFDF102C2A7BE2E238CF9A10437ABF5BCAC2")
                        .ToPositiveBigInteger(),
                    new BitString(
                            "1E06B72EA3AC38573AF3DF313C3C87DEE1516F0A96C09748C9B92A357A5AC15E")
                        .ToPositiveBigInteger()
                ),
                // expectedZ
                new BitString(
                    "3575CED03DD83BDF2B124D332117207348CD1D2998D7EE2815F86DC7EF9B8DAB"),
                // expectedDkm
                new BitString(
                    "6AC022972857CE788DA7F29FEA16BAB82E6E6E11258633E768909DAD3344807A"),
            },
        };

        private readonly IEccCurveFactory _curveFactory = new EccCurveFactory();

        private IKdfFactory _kdfFactory;
        private IKeyConfirmationFactory _keyConfirmationFactory;

        private MacParametersBuilder _macParamsBuilder = new MacParametersBuilder();
        private ISchemeBuilder _schemeBuilder;
        private ISecretKeyingMaterialBuilder _secretKeyingMaterialBuilderOtherParty;

        private ISecretKeyingMaterialBuilder _secretKeyingMaterialBuilderThisParty;
        private IKasBuilder _subject = new KasBuilder();


        [SetUp]
        public void Setup()
        {
            var shaFactory = new NativeShaFactory();
            var hmacFactory = new HmacFactory(shaFactory);
            var entropyFactory = new EntropyProviderFactory();

            var kdfVisitor = new KdfVisitor(
                new KdfOneStepFactory(shaFactory, hmacFactory, new KmacFactory(new cSHAKEWrapper())),
                new Crypto.KDF.KdfFactory(new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()),
                    hmacFactory, new KmacFactory(new cSHAKEWrapper())), hmacFactory,
                new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()),
                new IkeV1Factory(hmacFactory, shaFactory),
                new IkeV2Factory(new HmacFactory(shaFactory)),
                new TlsKdfFactory(hmacFactory),
                new HkdfFactory(new HmacFactory(shaFactory)));
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

            var kdfParameter = new KdfParameterOneStep
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

            EccKeyPair otherPartyEphemKey = GetKey(otherPartyPublicEphemKey, otherPartyPrivateEphemKey); ;
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

            Assert.Multiple(() =>
            {
                if (expectedZ != null)
                {
                    Assert.AreEqual(expectedZ.ToHex(), result.Z.ToHex(), nameof(result.Z));
                }

                if (expectedMacData != null)
                {
                    Assert.AreEqual(expectedMacData.ToHex(), result.MacData.ToHex(), nameof(result.MacData));
                }

                if (expectedTag != null)
                {
                    Assert.AreEqual(expectedTag.ToHex(), result.Tag.ToHex(), nameof(result.Tag));
                }
            });
        }

        private KdaOneStepAuxFunction GetAuxFunction(ModeValues dsaKdfHashMode, DigestSizes dsaKdfDigestSize)
        {
            List<(ModeValues hashMode, DigestSizes digestSize, KdaOneStepAuxFunction function)> list =
                new List<(ModeValues hashMode, DigestSizes digestSize, KdaOneStepAuxFunction function)>
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

        [Test]
        [TestCaseSource(nameof(_test_noKeyConfirmation))]
        public void ShouldNoKeyConfirmationCorrectly(
            string label,
            EccScheme scheme,
            Curve curveName,
            KeyAgreementRole keyAgreementRole,
            ModeValues dsaKdfHashMode,
            DigestSizes dsaKdfDigestSize,
            KeyAgreementMacType macType,
            int keyLength,
            BitString aesCcmNonce,
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
            BitString expectedDkm
        )
        {
            var curve = _curveFactory.GetCurve(curveName);
            var domainParameters = new EccDomainParameters(curve);

            KasEnumMapping.EccMap.TryFirst(x => x.Key == scheme, out var schemeResult);
            var kasScheme = schemeResult.Value;

            var kdfParameter = new KdfParameterOneStep
            {
                L = keyLength,
                AuxFunction = GetAuxFunction(dsaKdfHashMode, dsaKdfDigestSize)
            };

            var fixedInfoFactory = new FixedInfoFactory(new FixedInfoStrategyFactory());
            var fixedInfoParameter = new FixedInfoParameter
            {
                Encoding = FixedInfoEncoding.Concatenation,
                FixedInfoPattern = "uPartyInfo||vPartyInfo||literal[0123456789abcdef]",
                L = keyLength
            };

            EccKeyPair thisPartyEphemKey = GetKey(thisPartyPublicEphemKey, thisPartyPrivateEphemKey);
            EccKeyPair thisPartyStaticKey = GetKey(thisPartyPublicStaticKey, thisPartyPrivateStaticKey);

            EccKeyPair otherPartyEphemKey = GetKey(otherPartyPublicEphemKey, otherPartyPrivateEphemKey); ;
            EccKeyPair otherPartyStaticKey = GetKey(otherPartyPublicStaticKey, otherPartyPrivateStaticKey);

            var secretKeyingMaterialThisParty = _secretKeyingMaterialBuilderThisParty
                .WithDomainParameters(domainParameters)
                .WithEphemeralKey(thisPartyEphemKey)
                .WithPartyId(thisPartyId)
                .WithStaticKey(thisPartyStaticKey)
                .Build(kasScheme, KasMode.KdfNoKc, keyAgreementRole, KeyConfirmationRole.None, KeyConfirmationDirection.None);
            var secretKeyingMaterialOtherParty = _secretKeyingMaterialBuilderOtherParty
                .WithDomainParameters(domainParameters)
                .WithEphemeralKey(otherPartyEphemKey)
                .WithPartyId(otherPartyId)
                .WithStaticKey(otherPartyStaticKey)
                .Build(kasScheme, KasMode.KdfNoKc,
                    KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(keyAgreementRole),
                    KeyConfirmationRole.None, KeyConfirmationDirection.None);

            _schemeBuilder
                .WithKdf(_kdfFactory, kdfParameter)
                .WithFixedInfo(fixedInfoFactory, fixedInfoParameter)
                .WithThisPartyKeyingMaterial(secretKeyingMaterialThisParty)
                .WithSchemeParameters(new SchemeParameters(new KasAlgoAttributes(kasScheme), keyAgreementRole,
                    KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, KasAssurance.None, thisPartyId));

            var kas = _subject
                .WithSchemeBuilder(_schemeBuilder)
                .Build();

            var result = kas.ComputeResult(secretKeyingMaterialOtherParty);

            Assert.Multiple(() =>
            {
                if (expectedZ != null)
                {
                    Assert.AreEqual(expectedZ.ToHex(), result.Z.ToHex(), nameof(result.Z));
                }

                if (expectedDkm != null)
                {
                    Assert.AreEqual(expectedDkm.ToHex(), result.Dkm.ToHex(), nameof(result.Dkm));
                }
            });
        }

        [Test]
        [TestCaseSource(nameof(_test_twoStepNoKeyConfirmationIntermediateValues))]
        public void ShouldProduceIntermediateValuesTwoStepKdfNoKeyConfirmatino(
            string label,
            EccScheme scheme,
            Curve curveName,
            KeyAgreementRole keyAgreementRole,
            ModeValues dsaKdfHashMode,
            DigestSizes dsaKdfDigestSize,
            KeyAgreementMacType macType,
            int keyLength,

            BitString salt,
            KdfModes kdfMode,
            MacModes macMode,
            CounterLocations counterLocation,
            int counterLength,
            string fixedInfoPattern,

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
            BitString expectedDkm
        )
        {
            var curve = _curveFactory.GetCurve(curveName);
            var domainParameters = new EccDomainParameters(curve);

            KasEnumMapping.EccMap.TryFirst(x => x.Key == scheme, out var schemeResult);
            var kasScheme = schemeResult.Value;

            var kdfParameter = new KdfParameterTwoStep
            {
                L = keyLength,
                Salt = salt,
                CounterLen = counterLength,
                CounterLocation = counterLocation,
                KdfMode = kdfMode,
                MacMode = macMode,
                FixedInputEncoding = FixedInfoEncoding.Concatenation,
                FixedInfoPattern = fixedInfoPattern
            };

            var fixedInfoFactory = new FixedInfoFactory(new FixedInfoStrategyFactory());
            var fixedInfoParameter = new FixedInfoParameter
            {
                Encoding = FixedInfoEncoding.Concatenation,
                FixedInfoPattern = fixedInfoPattern,
                L = keyLength
            };

            EccKeyPair thisPartyEphemKey = GetKey(thisPartyPublicEphemKey, thisPartyPrivateEphemKey);
            EccKeyPair thisPartyStaticKey = GetKey(thisPartyPublicStaticKey, thisPartyPrivateStaticKey);

            EccKeyPair otherPartyEphemKey = GetKey(otherPartyPublicEphemKey, otherPartyPrivateEphemKey); ;
            EccKeyPair otherPartyStaticKey = GetKey(otherPartyPublicStaticKey, otherPartyPrivateStaticKey);

            var secretKeyingMaterialThisParty = _secretKeyingMaterialBuilderThisParty
                .WithDomainParameters(domainParameters)
                .WithEphemeralKey(thisPartyEphemKey)
                .WithPartyId(thisPartyId)
                .WithStaticKey(thisPartyStaticKey)
                .Build(kasScheme, KasMode.KdfNoKc, keyAgreementRole, KeyConfirmationRole.None, KeyConfirmationDirection.None);
            var secretKeyingMaterialOtherParty = _secretKeyingMaterialBuilderOtherParty
                .WithDomainParameters(domainParameters)
                .WithEphemeralKey(otherPartyEphemKey)
                .WithPartyId(otherPartyId)
                .WithStaticKey(otherPartyStaticKey)
                .Build(kasScheme, KasMode.KdfNoKc,
                    KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(keyAgreementRole),
                    KeyConfirmationRole.None, KeyConfirmationDirection.None);

            _schemeBuilder
                .WithKdf(_kdfFactory, kdfParameter)
                .WithFixedInfo(fixedInfoFactory, fixedInfoParameter)
                .WithThisPartyKeyingMaterial(secretKeyingMaterialThisParty)
                .WithSchemeParameters(new SchemeParameters(new KasAlgoAttributes(kasScheme), keyAgreementRole,
                    KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, KasAssurance.None, thisPartyId));

            var kas = _subject
                .WithSchemeBuilder(_schemeBuilder)
                .Build();

            var result = kas.ComputeResult(secretKeyingMaterialOtherParty);

            Assert.Multiple(() =>
            {
                if (expectedZ != null)
                {
                    Assert.AreEqual(expectedZ.ToHex(), result.Z.ToHex(), nameof(result.Z));
                }

                if (expectedDkm != null)
                {
                    Assert.AreEqual(expectedDkm.ToHex(), result.Dkm.ToHex(), nameof(result.Dkm));
                }
            });
        }

        private EccKeyPair GetKey(EccPoint publicKey, BigInteger privateKey)
        {
            if (privateKey != 0 && publicKey != null)
            {
                return new EccKeyPair(publicKey, privateKey);
            }

            if (privateKey == 0 && publicKey != null)
            {
                return new EccKeyPair(publicKey);
            }

            return null;
        }
    }
}
