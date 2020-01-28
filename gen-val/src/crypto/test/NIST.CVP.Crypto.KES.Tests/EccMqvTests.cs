using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KES.Tests
{
    [TestFixture, FastCryptoTest]
    public class EccMqvTests
    {
        private readonly MqvEcc _subject = new MqvEcc();
        private readonly EccCurveFactory _curveFactory = new EccCurveFactory();

        private static object[] _testsAvf = new object[]
        {
            new object[]
            {
                // qExactLength
                5,
                // public key
                new EccKeyPair(new EccPoint(new BigInteger(17), new BigInteger(0))),
                // expectedResult
                new BigInteger(9)
            }
        };

        [Test]
        [TestCaseSource(nameof(_testsAvf))]
        public void ShouldAvfCorrectly(int qExactLength, EccKeyPair publicKey, BigInteger expectedResult)
        {
            var result = MqvEcc.AssociateValueFunction(qExactLength, publicKey);

            Assert.AreEqual(expectedResult, result);
        }

        private static object[] _testsCalculationZ = new object[]
        {
            new object[]
            {
                // label
                "B-233",
                // curve
                Curve.B233,
                // sPartyA
                new EccKeyPair(
                    new EccPoint(
                        new BitString("00000056954ec0f3458ca9661240420dc77a05d845f4583267391ca18da6ec75")
                            .ToPositiveBigInteger(),
                        new BitString("000000694f096dcfaeb57924b5bba879c05d0e99a36c86de22e83d80f4b3470c")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("0000002cd87049a9cc5045017c7bd3d7ead0e3d72c3b6137e967030ac9d0e06d")
                        .ToPositiveBigInteger()
                ),
                // ePartyA
                new EccKeyPair(
                    new EccPoint(
                        new BitString("000000e5c1b24b38e26bb17cb7174061968add442ec6df8f34a525924fa7ee68")
                            .ToPositiveBigInteger(),
                        new BitString("000000884982550956b1f4e87fc5aac790a4da91809bbb0e53511453c4724193")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("0000009ee4daec27f1b93e6dd88e0371a0173c67df74ab7e0144a9ea76cbdc4a")
                        .ToPositiveBigInteger()
                ),
                // sPartyB
                new EccKeyPair(
                    new EccPoint(
                        new BitString("000000b85095ebef7af7b35a630a43033fdc19b9feb9b44caf23d258c2add087")
                            .ToPositiveBigInteger(),
                        new BitString("0000008fa40e6af465839df3909eab40ac288e4d7a519745e3f2c9ddb1cbf1d6")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("000000967fb12273a0ff296dc91b809ff470a02478be51f477bb2ed326d1c031")
                        .ToPositiveBigInteger()
                ),
                // ePartyB
                new EccKeyPair(
                    new EccPoint(
                        new BitString("000000536707130abccd2b094dff90c1570472edda6d879a3fad6b4a9319fc2f")
                            .ToPositiveBigInteger(),
                        new BitString("000000760d211de1291b970e873f7c29e3f7ccea31c1d4a05b6ab6ee2e8c3c66")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("000000f0dfa240c97f677ed554b342759be35a323dc1cb32da0a722a0322b123")
                        .ToPositiveBigInteger()
                ),
                // expected z
                new BitString("005eeb557dff8bde571a1394a5d6c568e5478b3885cf25f8d39f56930b11")
            },
            new object[]
            {
                // label
                "B-233 inverse",
                // curve
                Curve.B233,
                // sPartyA
                new EccKeyPair(
                    new EccPoint(
                        new BitString("000000b85095ebef7af7b35a630a43033fdc19b9feb9b44caf23d258c2add087")
                            .ToPositiveBigInteger(),
                        new BitString("0000008fa40e6af465839df3909eab40ac288e4d7a519745e3f2c9ddb1cbf1d6")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("000000967fb12273a0ff296dc91b809ff470a02478be51f477bb2ed326d1c031")
                        .ToPositiveBigInteger()
                ),
                // ePartyA
                new EccKeyPair(
                    new EccPoint(
                        new BitString("000000536707130abccd2b094dff90c1570472edda6d879a3fad6b4a9319fc2f")
                            .ToPositiveBigInteger(),
                        new BitString("000000760d211de1291b970e873f7c29e3f7ccea31c1d4a05b6ab6ee2e8c3c66")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("000000f0dfa240c97f677ed554b342759be35a323dc1cb32da0a722a0322b123")
                        .ToPositiveBigInteger()
                ),
                // sPartyB
                new EccKeyPair(
                    new EccPoint(
                        new BitString("00000056954ec0f3458ca9661240420dc77a05d845f4583267391ca18da6ec75")
                            .ToPositiveBigInteger(),
                        new BitString("000000694f096dcfaeb57924b5bba879c05d0e99a36c86de22e83d80f4b3470c")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("0000002cd87049a9cc5045017c7bd3d7ead0e3d72c3b6137e967030ac9d0e06d")
                        .ToPositiveBigInteger()
                ),
                // ePartyB
                new EccKeyPair(
                    new EccPoint(
                        new BitString("000000e5c1b24b38e26bb17cb7174061968add442ec6df8f34a525924fa7ee68")
                            .ToPositiveBigInteger(),
                        new BitString("000000884982550956b1f4e87fc5aac790a4da91809bbb0e53511453c4724193")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("0000009ee4daec27f1b93e6dd88e0371a0173c67df74ab7e0144a9ea76cbdc4a")
                        .ToPositiveBigInteger()
                ),
                // expected z
                new BitString("005eeb557dff8bde571a1394a5d6c568e5478b3885cf25f8d39f56930b11")
            },
            new object[]
            {
                // label
                "K-283",
                // curve
                Curve.K283,
                // sPartyA
                new EccKeyPair(
                    new EccPoint(
                        new BitString("037100d30b55d46dc03878fc83c8d81b89175568706730e90c25fe0fd2c164dfcec71182")
                            .ToPositiveBigInteger(),
                        new BitString("04d92bbd9697296829e12f17a859f6b43b018408ab10c8fcf1f08ca3f506250300cacf32")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("01be226d8c776dad67889e3e3c3f23f863ac8d24649f4227a0f2d425b065a5bb5a7b75ad")
                        .ToPositiveBigInteger()
                ),
                // ePartyA
                new EccKeyPair(
                    new EccPoint(
                        new BitString("0523c4d3b734b011a67892ea318bae94ed84b366b465e49591412e7b88fac3ed8ef5a99f")
                            .ToPositiveBigInteger(),
                        new BitString("076c512bc972f157a6c2633e770117c8b8b731e79bd350507f8661650252220cc334c491")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("00205697a4c127422b1b981efa645d70dad2f20ea3355a2e17419b4a6fc667b2e5bca481")
                        .ToPositiveBigInteger()
                ),
                // sPartyB
                new EccKeyPair(
                    new EccPoint(
                        new BitString("006e9afc03d0afb442425a9c4de91344222efd2e4d6ccd9a5d8a8c118df41130f0799c8b")
                            .ToPositiveBigInteger(),
                        new BitString("01476e9eb3e1a2a953fea5e2dc48ed06ce6ff27d4d8fc8870770997110f2479f51816979")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("01ffc68827a9ca6335e4a81b755c9535e7c32da420dac8a5f9c54639db32be46cb34a438")
                        .ToPositiveBigInteger()
                ),
                // ePartyB
                new EccKeyPair(
                    new EccPoint(
                        new BitString("05d210fbee29a24913469c4f071f7b4a18a741cde5d58fe1ca59d783727052c65667af16")
                            .ToPositiveBigInteger(),
                        new BitString("03cbd05aa18380b14aa2f6ebe78b8a4aa470ad9dbe05621c125f8ff602ca0ae26f94c713")
                            .ToPositiveBigInteger()
                    ),
                    new BitString("013bd7f08b0625fb28389dd8260bed71b174572f184172d8713bc2ec13104c56cfaf03be")
                        .ToPositiveBigInteger()
                ),
                // expected z
                new BitString("03e255b07b58d661eb6e4c355a674c084a0eef41294c89095d68df1872f42b3c49eee982")
            },
            new object[]
            {
                // label
                "P-384",
                // curve
                Curve.P384,
                // sPartyA
                new EccKeyPair(
                    new EccPoint(
                        new BitString(
                                "0ce1eeaf61413a413b02960df5a56e2fcbe6e7a306a6002916c3a023273ca7841b7620d730489845df182505f77cd9aa")
                            .ToPositiveBigInteger(),
                        new BitString(
                                "21f275a2b6622a2b5df1e927cf151717d8342dc751865fa339d5faa6244e6f70c399a7b1283297b093b6966239c336f7")
                            .ToPositiveBigInteger()
                    ),
                    new BitString(
                            "86b308a8d2d6decb4f8a233cb4e5df14caf6984687f523db1be1a6c87200fe527c255e4218babb1120d472e0f9abcca5")
                        .ToPositiveBigInteger()
                ),
                // ePartyA
                new EccKeyPair(
                    new EccPoint(
                        new BitString(
                                "580d61793814ee66c5986fbb574cebf1854269c9abfc0eb9e835ef1f47d08d11c9cd45422d016b4e2e4086cbb499d3c3")
                            .ToPositiveBigInteger(),
                        new BitString(
                                "b0ac73c3034b5b636c00c027c21c43828e118f5d43b261c3c1d7e62a14d457a3a559bbb0af2edc2502e2cea419b3e654")
                            .ToPositiveBigInteger()
                    ),
                    new BitString(
                            "50be5745e2f647da446b9ec465e9529e01753151c0cdf628ede2a84b2be66c3278fea51e98e2454b655328fda4c8baa5")
                        .ToPositiveBigInteger()
                ),
                // sPartyB
                new EccKeyPair(
                    new EccPoint(
                        new BitString(
                                "67c4e9dc9c22e21de5afde69906be28cea1841dd625b333be281c9e5899c02f4aa9f16eab85c9f0a4c0ab8b33a3553ea")
                            .ToPositiveBigInteger(),
                        new BitString(
                                "16fbb90d9f903c2553385d695dcfa2fb51f9ba9fe83cd75d3f5ad28a796dba4ee6b5532fb8474da5847188bc9443432b")
                            .ToPositiveBigInteger()
                    ),
                    new BitString(
                            "02229067da09d40d7de47ba28936904e55e62aae9bdf862367a23a914768b8cf94c581b937969bcb572cfbd77025c897")
                        .ToPositiveBigInteger()
                ),
                // ePartyB
                new EccKeyPair(
                    new EccPoint(
                        new BitString(
                                "457c45ebda3021195ddca56c3328570ddbeba1836c2d954a045cae6cff4c3d87424dc6b73252769143c5e867e9e546c8")
                            .ToPositiveBigInteger(),
                        new BitString(
                                "caec5dec81d8893f1a84b12cb24411df4634cfe130009f0d61a45bf5a0b9916e103c4645d7009bff3b05d536f5bf0024")
                            .ToPositiveBigInteger()
                    ),
                    new BitString(
                            "ff7d778241396ffaf8423759c715fac15f799a50f13b750e8ce38bd9d61cfadc97b2fd07f6826f1ff51790dd5eb43736")
                        .ToPositiveBigInteger()
                ),
                // expected z
                new BitString(
                    "14184f19dfe94186aecfaa4e7eb7bf71566679e131453ad8632edb66919aeb1fd8f377795f852f2c306c64e35e5a8fc8")
            },
            //new object[]
            //{
            //    // label
            //    "",
            //    // curve
            //    Curve.B163,
            //    // sPartyA
            //    new EccKeyPair(
            //        new EccPoint(
            //            new BitString("").ToPositiveBigInteger(),
            //            new BitString("").ToPositiveBigInteger()
            //        ),
            //        new BitString("").ToPositiveBigInteger()
            //    ),
            //    // ePartyA
            //    new EccKeyPair(
            //        new EccPoint(
            //            new BitString("").ToPositiveBigInteger(),
            //            new BitString("").ToPositiveBigInteger()
            //        ),
            //        new BitString("").ToPositiveBigInteger()
            //    ),
            //    // sPartyB
            //    new EccKeyPair(
            //        new EccPoint(
            //            new BitString("").ToPositiveBigInteger(),
            //            new BitString("").ToPositiveBigInteger()
            //        ),
            //        new BitString("").ToPositiveBigInteger()
            //    ),
            //    // ePartyB
            //    new EccKeyPair(
            //        new EccPoint(
            //            new BitString("").ToPositiveBigInteger(),
            //            new BitString("").ToPositiveBigInteger()
            //        ),
            //        new BitString("").ToPositiveBigInteger()
            //    ),
            //    // expected z
            //    new BitString("")
            //},
        };

        [Test]
        [TestCaseSource(nameof(_testsCalculationZ))]
        public void ShouldComputeCorrectZ(
            string label, 
            Curve curveEnum, 
            EccKeyPair sPartyA,
            EccKeyPair ePartyA,
            EccKeyPair sPartyB,
            EccKeyPair ePartyB,
            BitString expectedZ)
        {
            var curve = _curveFactory.GetCurve(curveEnum);

            EccDomainParameters dp = new EccDomainParameters(curve);

            var result = _subject.GenerateSharedSecretZ(
                dp,
                sPartyA,
                sPartyB, 
                ePartyA, 
                ePartyA, 
                ePartyB
            );

            Assume.That(result.Success);
            Assert.AreEqual(expectedZ, result.SharedSecretZ);
        }
    }
}