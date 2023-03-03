// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
// using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
// using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
// using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;
// using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native;
// using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys;
// using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
// using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
// using NUnit.Framework;
//
// namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Tests.Native.Keys
// {
//     [TestFixture, LongCryptoTest]
//     public class HssKeyPairFactoryTests
//     {
//         [OneTimeSetUp]
//         public void Setup()
//         {
//             var shaFactory = new NativeShaFactory();
//             var lmOtsKeyPairFactory = new LmOtsKeyPairFactory(shaFactory);
//             var lmsKeyPairFactory = new LmsKeyPairFactory(lmOtsKeyPairFactory, shaFactory);
//             var lmOts = new LmOts(shaFactory);
//             var lms = new LMS.Native.Lms(lmOts, shaFactory);
//             var seedIdRotator = new SeedIdRotator();
//             _subject = new HssKeyPairFactory(lmsKeyPairFactory, lms, shaFactory, seedIdRotator);
//             _randomizerC = new LmOtsPseudoRandomizerC(shaFactory);
//         }
//
//         private HssKeyPairFactory _subject;
//         private ILmOtsRandomizerC _randomizerC;
//
//         [Test]
//         [TestCase(new[] { LmsMode.LMS_SHA256_M24_H5, LmsMode.LMS_SHA256_M24_H5 }, LmOtsMode.LMOTS_SHA256_N24_W1)]
//         public async Task ShouldCreateKeyPair(LmsMode[] lmsModes, LmOtsMode lmOtsMode)
//         {
//             var rootTreeAttribute = AttributesHelper.GetLmsAttribute(lmsModes[0]);
//
//             var hssLevelParameters = GetHssLevelParameters(lmsModes, lmOtsMode);
//
//             var keyPair = await _subject.GetKeyPair(hssLevelParameters, _randomizerC, new byte[16], new byte[rootTreeAttribute.M]);
//
//             Assert.IsNotNull(keyPair.PublicKey.Key, nameof(keyPair.PublicKey.Key));
//             Assert.IsNotNull(keyPair.PublicKey.Signatures, nameof(keyPair.PublicKey.Signatures));
//         }
//
//         [Test]
//         [TestCase(new[] { LmsMode.LMS_SHA256_M24_H5, LmsMode.LMS_SHA256_M32_H5 }, LmOtsMode.LMOTS_SHA256_N24_W1)]
//         [TestCase(new[] { LmsMode.LMS_SHA256_M24_H5, LmsMode.LMS_SHA256_M32_H5 }, LmOtsMode.LMOTS_SHA256_N32_W1)]
//         public void WhenGivenVaryingOutputLengthsToScheme_ShouldThrow(LmsMode[] lmsModes, LmOtsMode lmOtsMode)
//         {
//             var rootTreeAttribute = AttributesHelper.GetLmsAttribute(lmsModes[0]);
//             var hssLevelParameters = GetHssLevelParameters(lmsModes, lmOtsMode);
//
//             Assert.ThrowsAsync<ArgumentException>(async () =>
//                 await _subject.GetKeyPair(hssLevelParameters, _randomizerC, new byte[16], new byte[rootTreeAttribute.M]));
//         }
//
//         [Test]
//         [TestCase(new[] { LmsMode.LMS_SHAKE_M24_H5, LmsMode.LMS_SHA256_M24_H5 }, LmOtsMode.LMOTS_SHA256_N24_W1)]
//         [TestCase(new[] { LmsMode.LMS_SHA256_M24_H5, LmsMode.LMS_SHA256_M24_H5 }, LmOtsMode.LMOTS_SHAKE_N24_W1)]
//         public void WhenGivenVaryingOneWayFunctionsToScheme_ShouldThrow(LmsMode[] lmsModes, LmOtsMode lmOtsMode)
//         {
//             var rootTreeAttribute = AttributesHelper.GetLmsAttribute(lmsModes[0]);
//             var hssLevelParameters = GetHssLevelParameters(lmsModes, lmOtsMode);
//
//             Assert.ThrowsAsync<ArgumentException>(async () =>
//                 await _subject.GetKeyPair(hssLevelParameters, _randomizerC, new byte[16], new byte[rootTreeAttribute.M]));
//         }
//
//         private static HssLevelParameter[] GetHssLevelParameters(LmsMode[] lmsModes, LmOtsMode lmOtsMode)
//         {
//             var hssLevelParameters = new List<HssLevelParameter>();
//             foreach (var lmsMode in lmsModes)
//             {
//                 hssLevelParameters.Add(new HssLevelParameter(lmsMode, lmOtsMode));
//             }
//
//             return hssLevelParameters.ToArray();
//         }
//     }
// }
