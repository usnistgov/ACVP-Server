// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Numerics;
// using System.Threading.Tasks;
// using Microsoft.Extensions.Logging;
// using Moq;
// using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
// using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
// using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
// using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;
// using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
// using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native;
// using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys;
// using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
// using NIST.CVP.ACVTS.Libraries.Math;
// using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
// using NUnit.Framework;
//
// namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Tests.Native
// {
//     [TestFixture, LongCryptoTest]
//     public class HssTests
//     {
//         [OneTimeSetUp]
//         public void Setup()
//         {
//             var logger = new Mock<ILogger<LMS.Native.Hss>>();
//             var shaFactory = new NativeShaFactory();
//             var lmOtsKeyPairFactory = new LmOtsKeyPairFactory(shaFactory);
//             var lmsKeyPairFactory = new LmsKeyPairFactory(lmOtsKeyPairFactory, shaFactory);
//             var lmOts = new LmOts(shaFactory);
//             var lms = new LMS.Native.Lms(lmOts, shaFactory);
//             var seedIdRotator = new SeedIdRotator();
//             _hssKeyPairFactory = new HssKeyPairFactory(lmsKeyPairFactory, lms, shaFactory, seedIdRotator);
//             _randomizerC = new LmOtsPseudoRandomizerC(shaFactory);
//
//             _subject = new LMS.Native.Hss(logger.Object, _hssKeyPairFactory, lms, lms);
//         }
//
//         private LMS.Native.Hss _subject;
//         private HssKeyPairFactory _hssKeyPairFactory;
//         private ILmOtsRandomizerC _randomizerC;
//
//         [Test]
//         [TestCase(new byte[] { 1, 2, 3 }, new byte[] { 1, 2, 3 })]
//         public void WhenGivenSameByteArrays_ShouldEvaluateEqual(byte[] a1, byte[] a2)
//         {
//             Assert.AreEqual(a1, a2);
//         }
//
//         [Test]
//         [TestCase(new byte[] { 1, 2, 3 }, new byte[] { 1, 2, 4 })]
//         [TestCase(new byte[] { 1, 2 }, new byte[] { 1, 2, 4 })]
//         public void WhenGivenDifferingByteArrays_ShouldEvaluateNotEqual(byte[] a1, byte[] a2)
//         {
//             Assert.AreNotEqual(a1, a2);
//         }
//
//         [Test]
//         public async Task WhenGivenSameSeed_ShouldCreateConsistentKeys()
//         {
//             var lmsMode = LmsMode.LMS_SHA256_M24_H5;
//
//             var rootTreeAttribute = AttributesHelper.GetLmsAttribute(lmsMode);
//
//             var hssLevelParameters = GetHssLevelParameters(
//                 new[] { lmsMode, lmsMode },
//                 LmOtsMode.LMOTS_SHA256_N24_W1);
//
//             var keyPair1 = await _hssKeyPairFactory.GetKeyPair(
//                 hssLevelParameters,
//                 _randomizerC,
//                 new byte[16],
//                 new byte[rootTreeAttribute.M]);
//
//             var keyPair2 = await _hssKeyPairFactory.GetKeyPair(
//                 hssLevelParameters,
//                 _randomizerC,
//                 new byte[16],
//                 new byte[rootTreeAttribute.M]);
//
//             Assert.Multiple(async () =>
//             {
//                 var pubKey1 = await keyPair1.PublicKey.Key;
//                 var pubKey2 = await keyPair2.PublicKey.Key;
//
//                 var sig1 = await keyPair1.PublicKey.Signatures;
//                 var sig2 = await keyPair2.PublicKey.Signatures;
//
//                 Assert.AreEqual(pubKey1, pubKey2, nameof(pubKey1));
//                 Assert.AreEqual(sig1, sig2, nameof(sig1));
//             });
//         }
//
//         [Test]
//         [TestCase(new[] { LmsMode.LMS_SHA256_M24_H5 }, LmOtsMode.LMOTS_SHA256_N24_W1)]
//         [TestCase(new[] { LmsMode.LMS_SHA256_M24_H5, LmsMode.LMS_SHA256_M24_H5 }, LmOtsMode.LMOTS_SHA256_N24_W1)]
//         [TestCase(new[] { LmsMode.LMS_SHA256_M24_H10, LmsMode.LMS_SHA256_M24_H5, LmsMode.LMS_SHA256_M24_H10 }, LmOtsMode.LMOTS_SHA256_N24_W1)]
//         public async Task WhenGivenMsg_ShouldSignAndVerify(LmsMode[] lmsModes, LmOtsMode lmOtsMode)
//         {
//             var rootTreeAttribute = AttributesHelper.GetLmsAttribute(lmsModes[0]);
//
//             var hssLevelParameters = GetHssLevelParameters(lmsModes, lmOtsMode);
//
//             var keyPair = await _hssKeyPairFactory.GetKeyPair(hssLevelParameters, _randomizerC, new byte[16], new byte[rootTreeAttribute.M]);
//
//             var message = new byte[10];
//             var signature = await _subject.Sign(keyPair, _randomizerC, message, 0);
//             var verify = await _subject.Verify(keyPair.PublicKey, signature.Signature, message);
//
//             Assert.IsTrue(verify);
//         }
//
//         [Test]
//         [TestCase(new[] { LmsMode.LMS_SHA256_M24_H5 }, LmOtsMode.LMOTS_SHA256_N24_W1, 1, false)]
//         [TestCase(new[] { LmsMode.LMS_SHA256_M24_H5 }, LmOtsMode.LMOTS_SHA256_N24_W1, 5, false)]
//         [TestCase(new[] { LmsMode.LMS_SHA256_M24_H5 }, LmOtsMode.LMOTS_SHA256_N24_W1, 32, false)]
//         [TestCase(new[] { LmsMode.LMS_SHA256_M24_H5 }, LmOtsMode.LMOTS_SHA256_N24_W1, 33, true)]
//         public async Task WhenOneLayerSignsRequests_ShouldBecomeExhaustedAfterTwoPowHeight(LmsMode[] lmsModes, LmOtsMode lmOtsMode, int messagesToSign, bool isExhausted)
//         {
//             var rootTreeAttribute = AttributesHelper.GetLmsAttribute(lmsModes[0]);
//
//             var hssLevelParameters = GetHssLevelParameters(lmsModes, lmOtsMode);
//
//             var keyPair = await _hssKeyPairFactory.GetKeyPair(hssLevelParameters, _randomizerC, new byte[16], new byte[rootTreeAttribute.M]);
//
//             var message = new byte[10];
//
//             for (var i = 0; i < messagesToSign - 1; i++)
//             {
//                 var sig = await _subject.Sign(keyPair, _randomizerC, message, 0);
//                 var verify = await _subject.Verify(keyPair.PublicKey, sig.Signature, message);
//                 Assert.IsTrue(verify);
//             }
//
//             // Last signature, check if exhausted or not based on parameter to method
//             if (isExhausted)
//             {
//                 var sig = await _subject.Sign(keyPair, _randomizerC, message, 0);
//                 Assert.IsTrue(sig.IsExhausted, nameof(sig));
//                 Assert.IsTrue(keyPair.IsExhausted, nameof(keyPair));
//             }
//             else
//             {
//                 var sig = await _subject.Sign(keyPair, _randomizerC, message, 0);
//                 Assert.IsFalse(sig.IsExhausted);
//             }
//         }
//
//         [Test]
//         public async Task WhenSigningMultipleTrees_ShouldHaveTwoPowSumTreeHeightKeysBeforeExhaustion()
//         {
//             var lmsMode = LmsMode.LMS_SHA256_M24_H5;
//
//             var rootTreeAttribute = AttributesHelper.GetLmsAttribute(lmsMode);
//
//             var hssLevelParameters = GetHssLevelParameters(
//                 new[] { lmsMode, lmsMode },
//                 LmOtsMode.LMOTS_SHA256_N24_W1);
//
//             var expectedSignOperationsPriorToExhaustion = (int)BigInteger.Pow(2, 5 * 2);
//             var signedMessages = 0;
//
//             var keyPair = await _hssKeyPairFactory.GetKeyPair(
//                 hssLevelParameters,
//                 _randomizerC,
//                 new byte[16],
//                 new byte[rootTreeAttribute.M]);
//
//             var message = new byte[16];
//
//             while (!keyPair.IsExhausted)
//             {
//                 try
//                 {
//                     await _subject.Sign(keyPair, _randomizerC, message, 0);
//                     signedMessages++;
//                 }
//                 catch (Exception e)
//                 {
//                     Console.WriteLine(e);
//                     throw;
//                 }
//
//             }
//
//             Assert.AreEqual(expectedSignOperationsPriorToExhaustion, signedMessages);
//         }
//
//         [Test]
//         [TestCase(1)]
//         [TestCase(2)]
//         [TestCase(3)]
//         [TestCase(4)]
//         [TestCase(10)]
//         [TestCase(25)]
//         [TestCase(30)]
//         [TestCase(31)]
//         public async Task WhenUsingPresignIncrementOnOneLayerHss_ShouldAdvanceThroughTreeConsistentlyAsWithoutIt(int incrementPriorToTest)
//         {
//             var lmsMode = LmsMode.LMS_SHA256_M24_H5;
//
//             var rootTreeAttribute = AttributesHelper.GetLmsAttribute(lmsMode);
//
//             var hssLevelParameters = GetHssLevelParameters(
//                 new[] { lmsMode },
//                 LmOtsMode.LMOTS_SHA256_N24_W1);
//
//             var keyPairPresignIncrement = await _hssKeyPairFactory.GetKeyPair(
//                 hssLevelParameters,
//                 _randomizerC,
//                 new byte[16],
//                 new byte[rootTreeAttribute.M]);
//
//             var keyPairManualAdvance = await _hssKeyPairFactory.GetKeyPair(
//                 hssLevelParameters,
//                 _randomizerC,
//                 new byte[16],
//                 new byte[rootTreeAttribute.M]);
//
//             var message = new byte[10];
//
//             for (var i = 0; i < incrementPriorToTest; i++)
//             {
//                 await _subject.Sign(keyPairManualAdvance, _randomizerC, message, 0);
//             }
//
//             var sig1 = await _subject.Sign(keyPairManualAdvance, _randomizerC, message, 0);
//             var sig2 = await _subject.Sign(keyPairPresignIncrement, _randomizerC, message, incrementPriorToTest);
//
//             var sigHex1 = new BitString(sig1.Signature).ToHex();
//             var sigHex2 = new BitString(sig2.Signature).ToHex();
//
//             Console.WriteLine(sigHex1);
//             Console.WriteLine(sigHex2);
//
//             Assert.AreEqual(sigHex1, sigHex2);
//         }
//
//         [Test]
//         [TestCase(1)]
//         [TestCase(2)]
//         [TestCase(4)]
//         [TestCase(10)]
//         [TestCase(25)]
//         [TestCase(30)]
//         [TestCase(31)]
//         [TestCase(32)]
//         [TestCase(33)]
//         [TestCase(35)]
//         [TestCase(60)]
//         [TestCase(63)]
//         [TestCase(64)]
//         [TestCase(1022)]
//         [TestCase(1023)]
//         public async Task WhenUsingPresignIncrementOnTwoLayerHss_ShouldAdvanceThroughTreeConsistentlyAsWithoutIt(int incrementPriorToTest)
//         {
//             var lmsMode = LmsMode.LMS_SHA256_M24_H5;
//
//             var rootTreeAttribute = AttributesHelper.GetLmsAttribute(lmsMode);
//
//             var hssLevelParameters = GetHssLevelParameters(
//                 new[] { lmsMode, lmsMode },
//                 LmOtsMode.LMOTS_SHA256_N24_W1);
//
//             var keyPairPresignIncrement = await _hssKeyPairFactory.GetKeyPair(
//                 hssLevelParameters,
//                 _randomizerC,
//                 new byte[16],
//                 new byte[rootTreeAttribute.M]);
//
//             var keyPairManualAdvance = await _hssKeyPairFactory.GetKeyPair(
//                 hssLevelParameters,
//                 _randomizerC,
//                 new byte[16],
//                 new byte[rootTreeAttribute.M]);
//
//             var message = new byte[10];
//
//             for (var i = 0; i < incrementPriorToTest; i++)
//             {
//                 await _subject.Sign(keyPairManualAdvance, _randomizerC, message, 0);
//             }
//
//             var sig2 = await _subject.Sign(keyPairPresignIncrement, _randomizerC, message, incrementPriorToTest);
//             var sig1 = await _subject.Sign(keyPairManualAdvance, _randomizerC, message, 0);
//
//
//             var sigHex1 = new BitString(sig1.Signature).ToHex();
//             var sigHex2 = new BitString(sig2.Signature).ToHex();
//
//             Console.WriteLine(sigHex1);
//             Console.WriteLine(sigHex2);
//
//             Assert.AreEqual(sigHex1, sigHex2);
//         }
//
//         [Test]
//         [TestCase(1)]
//         [TestCase(2)]
//         [TestCase(4)]
//         [TestCase(31)]
//         [TestCase(32)]
//         [TestCase(33)]
//         [TestCase(64)]
//         [TestCase(1024)]
//         [TestCase(1026)]
//         [TestCase(2048)]
//         [TestCase(32767)]
//         public async Task WhenUsingPresignIncrementOnThreeLayerHss_ShouldAdvanceThroughTreeConsistentlyAsWithoutIt(int incrementPriorToTest)
//         {
//             var lmsMode = LmsMode.LMS_SHA256_M24_H5;
//
//             var rootTreeAttribute = AttributesHelper.GetLmsAttribute(lmsMode);
//
//             var hssLevelParameters = GetHssLevelParameters(
//                 new[] { lmsMode, lmsMode, lmsMode },
//                 LmOtsMode.LMOTS_SHA256_N24_W1);
//
//             var keyPairPresignIncrement = await _hssKeyPairFactory.GetKeyPair(
//                 hssLevelParameters,
//                 _randomizerC,
//                 new byte[16],
//                 new byte[rootTreeAttribute.M]);
//
//             var keyPairManualAdvance = await _hssKeyPairFactory.GetKeyPair(
//                 hssLevelParameters,
//                 _randomizerC,
//                 new byte[16],
//                 new byte[rootTreeAttribute.M]);
//
//             var message = new byte[10];
//
//             for (var i = 0; i < incrementPriorToTest; i++)
//             {
//                 await _subject.Sign(keyPairManualAdvance, _randomizerC, message, 0);
//             }
//
//             var sig1 = await _subject.Sign(keyPairManualAdvance, _randomizerC, message, 0);
//             var sig2 = await _subject.Sign(keyPairPresignIncrement, _randomizerC, message, incrementPriorToTest);
//
//             var sigHex1 = new BitString(sig1.Signature).ToHex();
//             var sigHex2 = new BitString(sig2.Signature).ToHex();
//
//             Console.WriteLine(sigHex1);
//             Console.WriteLine(sigHex2);
//
//             Assert.AreEqual(sigHex1, sigHex2);
//         }
//
//         /// <summary>
//         /// Test cases from https://github.com/cisco/hash-sigs/blob/master/test_testvector.c
//         /// which were modeled after the RFC tests https://datatracker.ietf.org/doc/html/rfc8554,
//         /// though the proceeding tests are just signature verification, rather than deterministic creation,
//         /// <see cref="WhenGivenRfcTestCaseInputs_ShouldDeterministicallyGenerateRfcTestCaseOutputs"/> for those tests.. 
//         /// </summary>
//         /// <param name="publicKey">The hex string public key</param>
//         /// <param name="message">The hex string message</param>
//         /// <param name="signature">The hex string signature</param>
//         [Test]
//         [TestCase(
//             "00000002000000050000000461a5d57d37f5e46bfb7520806b07a1b850650e3b31fe4a773ea29a07f09cf2ea30e579f0df58ef8e298da0434cb2b878",
//             "54686520706f77657273206e6f742064656c65676174656420746f2074686520556e69746564205374617465732062792074686520436f6e737469747574696f6e2c206e6f722070726f6869626974656420627920697420746f20746865205374617465732c2061726520726573657276656420746f207468652053746174657320726573706563746976656c792c206f7220746f207468652070656f706c652e0a",
//             "000000010000000500000004d32b56671d7eb98833c49b433c272586bc4a1c8a8970528ffa04b966f9426eb9965a25bfd37f196b9073f3d4a232feb69128ec45146f86292f9dff9610a7bf95a64c7f60f6261a62043f86c70324b7707f5b4a8a6e19c114c7be866d488778a0e05fd5c6509a6e61d559cf1a77a970de927d60c70d3de31a7fa0100994e162a2582e8ff1b10cd99d4e8e413ef469559f7d7ed12c838342f9b9c96b83a4943d1681d84b15357ff48ca579f19f5e71f18466f2bbef4bf660c2518eb20de2f66e3b14784269d7d876f5d35d3fbfc7039a462c716bb9f6891a7f41ad133e9e1f6d9560b960e7777c52f060492f2d7c660e1471e07e72655562035abc9a701b473ecbc3943c6b9c4f2405a3cb8bf8a691ca51d3f6ad2f428bab6f3a30f55dd9625563f0a75ee390e385e3ae0b906961ecf41ae073a0590c2eb6204f44831c26dd768c35b167b28ce8dc988a3748255230cef99ebf14e730632f27414489808afab1d1e783ed04516de012498682212b07810579b250365941bcc98142da13609e9768aaf65de7620dabec29eb82a17fde35af15ad238c73f81bdb8dec2fc0e7f932701099762b37f43c4a3c20010a3d72e2f606be108d310e639f09ce7286800d9ef8a1a40281cc5a7ea98d2adc7c7400c2fe5a101552df4e3cccfd0cbf2ddf5dc6779cbbc68fee0c3efe4ec22b83a2caa3e48e0809a0a750b73ccdcf3c79e6580c154f8a58f7f24335eec5c5eb5e0cf01dcf4439424095fceb077f66ded5bec73b27c5b9f64a2a9af2f07c05e99e5cf80f00252e39db32f6c19674f190c9fbc506d826857713afd2ca6bb85cd8c107347552f30575a5417816ab4db3f603f2df56fbc413e7d0acd8bdd81352b2471fc1bc4f1ef296fea1220403466b1afe78b94f7ecf7cc62fb92be14f18c2192384ebceaf8801afdf947f698ce9c6ceb696ed70e9e87b0144417e8d7baf25eb5f70f09f016fc925b4db048ab8d8cb2a661ce3b57ada67571f5dd546fc22cb1f97e0ebd1a65926b1234fd04f171cf469c76b884cf3115cce6f792cc84e36da58960c5f1d760f32c12faef477e94c92eb75625b6a371efc72d60ca5e908b3a7dd69fef0249150e3eebdfed39cbdc3ce9704882a2072c75e13527b7a581a556168783dc1e97545e31865ddc46b3c957835da252bb7328d3ee2062445dfb85ef8c35f8e1f3371af34023cef626e0af1e0bc017351aae2ab8f5c612ead0b729a1d059d02bfe18efa971b7300e882360a93b025ff97e9e0eec0f3f3f13039a17f88b0cf808f488431606cb13f9241f40f44e537d302c64a4f1f4ab949b9feefadcb71ab50ef27d6d6ca8510f150c85fb525bf25703df7209b6066f09c37280d59128d2f0f637c7d7d7fad4ed1c1ea04e628d221e3d8db77b7c878c9411cafc5071a34a00f4cf07738912753dfce48f07576f0d4f94f42c6d76f7ce973e9367095ba7e9a3649b7f461d9f9ac1332a4d1044c96aefee67676401b64457c54d65fef6500c59cdfb69af7b6dddfcb0f086278dd8ad0686078dfb0f3f79cd893d314168648499898fbc0ced5f95b74e8ff14d735cdea968bee7400000005d8b8112f9200a5e50c4a262165bd342cd800b8496810bc716277435ac376728d129ac6eda839a6f357b5a04387c5ce97382a78f2a4372917eefcbf93f63bb59112f5dbe400bd49e4501e859f885bf0736e90a509b30a26bfac8c17b5991c157eb5971115aa39efd8d564a6b90282c3168af2d30ef89d51bf14654510a12b8a144cca1848cf7da59cc2b3d9d0692dd2a20ba3863480e25b1b85ee860c62bf51360000000500000004d2f14ff6346af964569f7d6cb880a1b66c5004917da6eafe4d9ef6c6407b3db0e5485b122d9ebe15cda93cfec582d7ab0000000a000000040703c491e7558b35011ece3592eaa5da4d918786771233e8353bc4f62323185c95cae05b899e35dffd717054706209988ebfdf6e37960bb5c38d7657e8bffeef9bc042da4b4525650485c66d0ce19b317587c6ba4bffcc428e25d08931e72dfb6a120c5612344258b85efdb7db1db9e1865a73caf96557eb39ed3e3f426933ac9eeddb03a1d2374af7bf77185577456237f9de2d60113c23f846df26fa942008a698994c0827d90e86d43e0df7f4bfcdb09b86a373b98288b7094ad81a0185ac100e4f2c5fc38c003c1ab6fea479eb2f5ebe48f584d7159b8ada03586e65ad9c969f6aecbfe44cf356888a7b15a3ff074f771760b26f9c04884ee1faa329fbf4e61af23aee7fa5d4d9a5dfcf43c4c26ce8aea2ce8a2990d7ba7b57108b47dabfbeadb2b25b3cacc1ac0cef346cbb90fb044beee4fac2603a442bdf7e507243b7319c9944b1586e899d431c7f91bcccc8690dbf59b28386b2315f3d36ef2eaa3cf30b2b51f48b71b003dfb08249484201043f65f5a3ef6bbd61ddfee81aca9ce60081262a00000480dcbc9a3da6fbef5c1c0a55e48a0e729f9184fcb1407c31529db268f6fe50032a363c9801306837fafabdf957fd97eafc80dbd165e435d0e2dfd836a28b354023924b6fb7e48bc0b3ed95eea64c2d402f4d734c8dc26f3ac591825daef01eae3c38e3328d00a77dc657034f287ccb0f0e1c9a7cbdc828f627205e4737b84b58376551d44c12c3c215c812a0970789c83de51d6ad787271963327f0a5fbb6b5907dec02c9a90934af5a1c63b72c82653605d1dcce51596b3c2b45696689f2eb382007497557692caac4d57b5de9f5569bc2ad0137fd47fb47e664fcb6db4971f5b3e07aceda9ac130e9f38182de994cff192ec0e82fd6d4cb7f3fe00812589b7a7ce515440456433016b84a59bec6619a1c6c0b37dd1450ed4f2d8b584410ceda8025f5d2d8dd0d2176fc1cf2cc06fa8c82bed4d944e71339ece780fd025bd41ec34ebff9d4270a3224e019fcb444474d482fd2dbe75efb20389cc10cd600abb54c47ede93e08c114edb04117d714dc1d525e11bed8756192f929d15462b939ff3f52f2252da2ed64d8fae88818b1efa2c7b08c8794fb1b214aa233db3162833141ea4383f1a6f120be1db82ce3630b3429114463157a64e91234d475e2f79cbf05e4db6a9407d72c6bff7d1198b5c4d6aad2831db61274993715a0182c7dc8089e32c8531deed4f7431c07c02195eba2ef91efb5613c37af7ae0c066babc69369700e1dd26eddc0d216c781d56e4ce47e3303fa73007ff7b949ef23be2aa4dbf25206fe45c20dd888395b2526391a724996a44156beac808212858792bf8e74cba49dee5e8812e019da87454bff9e847ed83db07af313743082f880a278f682c2bd0ad6887cb59f652e155987d61bbf6a88d36ee93b6072e6656d9ccbaae3d655852e38deb3a2dcf8058dc9fb6f2ab3d3b3539eb77b248a661091d05eb6e2f297774fe6053598457cc61908318de4b826f0fc86d4bb117d33e865aa805009cc2918d9c2f840c4da43a703ad9f5b5806163d7161696b5a0adc00000005d5c0d1bebb06048ed6fe2ef2c6cef305b3ed633941ebc8b3bec9738754cddd60e1920ada52f43d055b5031cee6192520d6a5115514851ce7fd448d4a39fae2ab2335b525f484e9b40d6a4a969394843bdcf6d14c48e8015e08ab92662c05c6e9f90b65a7a6201689999f32bfd368e5e3ec9cb70ac7b8399003f175c40885081a09ab3034911fe125631051df0408b3946b0bde790911e8978ba07dd56c73e7ee")]
//         [TestCase(
//             "000000020000000600000003d08fabd4a2091ff0a8cb4ed834e7453432a58885cd9ba0431235466bff9651c6c92124404d45fa53cf161c28f1ad5a8e",
//             "54686520656e756d65726174696f6e20696e2074686520436f6e737469747574696f6e2c206f66206365727461696e207269676874732c207368616c6c206e6f7420626520636f6e73747275656420746f2064656e79206f7220646973706172616765206f74686572732072657461696e6564206279207468652070656f706c652e0a",
//             "0000000100000003000000033d46bee8660f8f215d3f96408a7a64cf1c4da02b63a55f62c666ef5707a914ce0674e8cb7a55f0c48d484f31f3aa4af9719a74f22cf823b94431d01c926e2a76bb71226d279700ec81c9e95fb11a0d10d065279a5796e265ae17737c44eb8c594508e126a9a7870bf4360820bdeb9a01d9693779e416828e75bddd7d8c70d50a0ac8ba39810909d445f44cb5bb58de737e60cb4345302786ef2c6b14af212ca19edeaa3bfcfe8baa6621ce88480df2371dd37add732c9de4ea2ce0dffa53c92649a18d39a50788f4652987f226a1d48168205df6ae7c58e049a25d4907edc1aa90da8aa5e5f7671773e941d8055360215c6b60dd35463cf2240a9c06d694e9cb54e7b1e1bf494d0d1a28c0d31acc75161f4f485dfd3cb9578e836ec2dc722f37ed30872e07f2b8bd0374eb57d22c614e09150f6c0d8774a39a6e168211035dc52988ab46eaca9ec597fb18b4936e66ef2f0df26e8d1e34da28cbb3af752313720c7b345434f72d65314328bbb030d0f0f6d5e47b28ea91008fb11b05017705a8be3b2adb83c60a54f9d1d1b2f476f9e393eb5695203d2ba6ad815e6a111ea293dcc21033f9453d49c8e5a6387f588b1ea4f706217c151e05f55a6eb7997be09d56a326a32f9cba1fbe1c07bb49fa04cecf9df1a1b815483c75d7a27cc88ad1b1238e5ea986b53e087045723ce16187eda22e33b2c70709e53251025abde8939645fc8c0693e97763928f00b2e3c75af3942d8ddaee81b59a6f1f67efda0ef81d11873b59137f67800b35e81b01563d187c4a1575a1acb92d087b517a8833383f05d357ef4678de0c57ff9f1b2da61dfde5d88318bcdde4d9061cc75c2de3cd4740dd7739ca3ef66f1930026f47d9ebaa713b07176f76f953e1c2e7f8f271a6ca375dbfb83d719b1635a7d8a13891957944b1c29bb101913e166e11bd5f34186fa6c0a555c9026b256a6860f4866bd6d0b5bf90627086c6149133f8282ce6c9b3622442443d5eca959d6c14ca8389d12c4068b503e4e3c39b635bea245d9d05a2558f249c9661c0427d2e489ca5b5dde220a90333f4862aec793223c781997da98266c12c50ea28b2c438e7a379eb106eca0c7fd6006e9bf612f3ea0a454ba3bdb76e8027992e60de01e9094fddeb3349883914fb17a9621ab929d970d101e45f8278c14b032bcab02bd15692d21b6c5c204abbf077d465553bd6eda645e6c3065d33b10d518a61e15ed0f092c32226281a29c8a0f50cde0a8c66236e29c2f310a375cebda1dc6bb9a1a01dae6c7aba8ebedc6371a7d52aacb955f83bd6e4f84d2949dcc198fb77c7e5cdf6040b0f84faf82808bf985577f0a2acf2ec7ed7c0b0ae8a270e951743ff23e0b2dd12e9c3c828fb5598a22461af94d568f29240ba2820c4591f71c088f96e095dd98beae456579ebbba36f6d9ca2613d1c26eee4d8c73217ac5962b5f3147b492e8831597fd89b64aa7fde82e1974d2f6779504dc21435eb3109350756b9fdabe1c6f368081bd40b27ebcb9819a75d7df8bb07bb05db1bab705a4b7e37125186339464ad8faaa4f052cc1272919fde3e025bb64aa8e0eb1fcbfcc25acb5f718ce4f7c2182fb393a1814b0e942490e52d3bca817b2b26e90d4c9b0cc38608a6cef5eb153af0858acc867c9922aed43bb67d7b33acc519313d28d41a5c6fe6cf3595dd5ee63f0a4c4065a083590b275788bee7ad875a7f88dd73720708c6c6c0ecf1f43bbaadae6f208557fdc07bd4ed91f88ce4c0de842761c70c186bfdafafc444834bd3418be4253a71eaf41d718753ad07754ca3effd5960b0336981795721426803599ed5b2b7516920efcbe32ada4bcf6c73bd29e3fa152d9adeca36020fdeeee1b739521d3ea8c0da497003df1513897b0f54794a873670b8d93bcca2ae47e64424b7423e1f078d9554bb5232cc6de8aae9b83fa5b9510beb39ccf4b4e1d9c0f19d5e17f58e5b8705d9a6837a7d9bf99cd13387af256a8491671f1f2f22af253bcff54b673199bdb7d05d81064ef05f80f0153d0be7919684b23da8d42ff3effdb7ca0985033f389181f47659138003d712b5ec0a614d31cc7487f52de8664916af79c98456b2c94a8038083db55391e3475862250274a1de2584fec975fb09536792cfbfcf6192856cc76eb5b13dc4709e2f7301ddff26ec1b23de2d188c999166c74e1e14bbc15f457cf4e471ae13dcbdd9c50f4d646fc6278e8fe7eb6cb5c94100fa870187380b777ed19d7868fd8ca7ceb7fa7d5cc861c5bdac98e7495eb0a2ceec1924ae979f44c5390ebedddc65d6ec11287d978b8df064219bc5679f7d7b264a76ff272b2ac9f2f7cfc9fdcfb6a51428240027afd9d52a79b647c90c2709e060ed70f87299dd798d68f4fadd3da6c51d839f851f98f67840b964ebe73f8cec41572538ec6bc131034ca2894eb736b3bda93d9f5f6fa6f6c0f03ce43362b8414940355fb54d3dfdd03633ae108f3de3ebc85a3ff51efeea3bc2cf27e1658f1789ee612c83d0f5fd56f7cd071930e2946beeecaa04dccea9f97786001475e0294bc2852f62eb5d39bb9fbeef75916efe44a662ecae37ede27e9d6eadfdeb8f8b2b2dbccbf96fa6dbaf7321fb0e701f4d429c2f4dcd153a2742574126e5eaccc77686acf6e3ee48f423766e0fc466810a905ff5453ec99897b56bc55dd49b991142f65043f2d744eeb935ba7f4ef23cf80cc5a8a335d3619d781e7454826df720eec82e06034c44699b5f0c44a8787752e057fa3419b5bb0e25d30981e41cb1361322dba8f69931cf42fad3f3bce6ded5b8bfc3d20a2148861b2afc14562ddd27f12897abf0685288dcc5c4982f826026846a24bf77e383c7aacab1ab692b29ed8c018a65f3dc2b87ff619a633c41b4fadb1c78725c1f8f922f6009787b1964247df0136b1bc614ab575c59a16d089917bd4a8b6f04d95c581279a139be09fcf6e98a470a0bceca191fce476f9370021cbc05518a7efd35d89d8577c990a5e19961ba16203c959c91829ba7497cffcbb4b294546454fa5388a23a22e805a5ca35f956598848bda678615fec28afd5da61a00000006b326493313053ced3876db9d237148181b7173bc7d042cefb4dbe94d2e58cd21a769db4657a103279ba8ef3a629ca84ee836172a9c50e51f45581741cf8083150b491cb4ecbbabec128e7c81a46e62a67b57640a0a78be1cbf7dd9d419a10cd8686d16621a80816bfdb5bdc56211d72ca70b81f1117d129529a7570cf79cf52a7028a48538ecdd3b38d3d5d62d26246595c4fb73a525a5ed2c30524ebb1d8cc82e0c19bc4977c6898ff95fd3d310b0bae71696cef93c6a552456bf96e9d075e383bb7543c675842bafbfc7cdb88483b3276c29d4f0a341c2d406e40d4653b7e4d045851acf6a0a0ea9c710b805cced4635ee8c107362f0fc8d80c14d0ac49c516703d26d14752f34c1c0d2c4247581c18c2cf4de48e9ce949be7c888e9caebe4a415e291fd107d21dc1f084b1158208249f28f4f7c7e931ba7b3bd0d824a45700000000500000004215f83b7ccb9acbcd08db97b0d04dc2ba1cd035833e0e90059603f26e07ad2aad152338e7a5e5984bcd5f7bb4eba40b700000004000000040eb1ed54a2460d512388cad533138d240534e97b1e82d33bd927d201dfc24ebb11b3649023696f85150b189e50c00e98850ac343a77b3638319c347d7310269d3b7714fa406b8c35b021d54d4fdada7b9ce5d4ba5b06719e72aaf58c5aae7aca057aa0e2e74e7dcfd17a0823429db62965b7d563c57b4cec942cc865e29c1dad83cac8b4d61aacc457f336e6a10b66323f5887bf3523dfcadee158503bfaa89dc6bf59daa82afd2b5ebb2a9ca6572a6067cee7c327e9039b3b6ea6a1edc7fdc3df927aade10c1c9f2d5ff446450d2a3998d0f9f6202b5e07c3f97d2458c69d3c8190643978d7a7f4d64e97e3f1c4a08a7c5bc03fd55682c017e2907eab07e5bb2f190143475a6043d5e6d5263471f4eecf6e2575fbc6ff37edfa249d6cda1a09f797fd5a3cd53a066700f45863f04b6c8a58cfd341241e002d0d2c0217472bf18b636ae547c1771368d9f317835c9b0ef430b3df4034f6af00d0da44f4af7800bc7a5cf8a5abdb12dc718b559b74cab9090e33cc58a955300981c420c4da8ffd67df540890a062fe40dba8b2c1c548ced22473219c534911d48ccaabfb71bc71862f4a24ebd376d288fd4e6fb06ed8705787c5fedc813cd2697e5b1aac1ced45767b14ce88409eaebb601a93559aae893e143d1c395bc326da821d79a9ed41dcfbe549147f71c092f4f3ac522b5cc57290706650487bae9bb5671ecc9ccc2ce51ead87ac01985268521222fb9057df7ed41810b5ef0d4f7cc67368c90f573b1ac2ce956c365ed38e893ce7b2fae15d3685a3df2fa3d4cc098fa57dd60d2c9754a8ade980ad0f93f6787075c3f680a2ba1936a8c61d1af52ab7e21f416be09d2a8d64c3d3d8582968c2839902229f85aee297e717c094c8df4a23bb5db658dd377bf0f4ff3ffd8fba5e383a48574802ed545bbe7a6b4753533353d73706067640135a7ce517279cd683039747d218647c86e097b0daa2872d54b8f3e5085987629547b830d8118161b65079fe7bc59a99e9c3c7380e3e70b7138fe5d9be2551502b698d09ae193972f27d40f38dea264a0126e637d74ae4c92a6249fa103436d3eb0d4029ac712bfc7a5eacbdd7518d6d4fe903a5ae65527cd65bb0d4e9925ca24fd7214dc617c150544e423f450c99ce51ac8005d33acd74f1bed3b17b7266a4a3bb86da7eba80b101e15cb79de9a207852cf91249ef480619ff2af8cabca83125d1faa94cbb0a03a906f683b3f47a97c871fd513e510a7a25f283b196075778496152a91c2bf9da76ebe089f4654877f2d586ae7149c406e663eadeb2b5c7e82429b9e8cb4834c83464f079995332e4b3c8f5a72bb4b8c6f74b0d45dc6c1f79952c0b7420df525e37c15377b5f0984319c3993921e5ccd97e097592064530d33de3afad5733cbe7703c5296263f77342efbf5a04755b0b3c997c4328463e84caa2de3ffdcd297baaaacd7ae646e44b5c0f16044df38fabd296a47b3a838a913982fb2e370c078edb042c84db34ce36b46ccb76460a690cc86c302457dd1cde197ec8075e82b393d542075134e2a17ee70a5e187075d03ae3c853cff60729ba4000000054de1f6965bdabc676c5a4dc7c35f97f82cb0e31c68d04f1dad96314ff09e6b3de96aeee300d1f68bf1bca9fc58e4032336cd819aaf578744e50d1357a0e4286704d341aa0a337b19fe4bc43c2e79964d4f351089f2e0e41c7c43ae0d49e7f404b0f75be80ea3af098c9752420a8ac0ea2bbb1f4eeba05238aef0d8ce63f0c6e5e4041d95398a6f7f3e0ee97cc1591849d4ed236338b147abde9f51ef9fd4e1c1")]
//         public async Task WhenGivenSignatureFromRfcTestCases_ShouldVerify(string publicKey, string message, string signature)
//         {
//             var publicKeyBytes = new BitString(publicKey).ToBytes();
//             var messageBytes = new BitString(message).ToBytes();
//             var signatureBytes = new BitString(signature).ToBytes();
//
//             var hssPublicKey = new Mock<IHssPublicKey>();
//             hssPublicKey
//                 .Setup(s => s.Key)
//                 .Returns(Task.FromResult(publicKeyBytes));
//             hssPublicKey
//                 .Setup(s => s.Levels)
//                 .Returns(2);
//
//             var result = await _subject.Verify(hssPublicKey.Object, signatureBytes, messageBytes);
//
//             Assert.IsTrue(result);
//         }
//
//         /// <summary>
//         /// https://datatracker.ietf.org/doc/html/rfc8554#page-49
//         ///
//         /// Test case 2
//         /// </summary>
//         [Test]
//         public async Task WhenGivenRfcTestCaseInputs_ShouldDeterministicallyGenerateRfcTestCaseOutputs()
//         {
//             var seedIdRotator = new Mock<ISeedIdRotator>();
//             var logger = new Mock<ILogger<LMS.Native.Hss>>();
//             var shaFactory = new NativeShaFactory();
//             var lmOtsKeyPairFactory = new LmOtsKeyPairFactory(shaFactory);
//             var lmsKeyPairFactory = new LmsKeyPairFactory(lmOtsKeyPairFactory, shaFactory);
//             var lmOts = new LmOts(shaFactory);
//             var lms = new LMS.Native.Lms(lmOts, shaFactory);
//             var hssKeyPairFactory = new HssKeyPairFactory(lmsKeyPairFactory, lms, shaFactory, seedIdRotator.Object);
//             var randomizerC = new Mock<ILmOtsRandomizerC>();
//
//             // This will be the new seed/id value for the level 1 tree, level 0 (root) tree will have its seed/id directly passed in
//             seedIdRotator
//                 .Setup(s => s.GetNewSeedId(
//                     It.IsAny<ISha>(),
//                     It.IsAny<LmsAttribute>(),
//                     It.IsAny<byte[]>(),
//                     It.IsAny<byte[]>(),
//                     It.IsAny<int>()))
//                 .Returns(new IdSeedResult(
//                     new BitString("215f83b7ccb9acbcd08db97b0d04dc2b").ToBytes(),
//                     new BitString("a1c4696e2608035a886100d05cd99945eb3370731884a8235e2fb3d4d71f2547").ToBytes()));
//
//             // this is the C value that is used during the LMS sign operation for the signature created as a part of HSS construction
//             randomizerC
//                 .Setup(s => s.GetRandomizerValueC(It.IsAny<ILmOtsPrivateKey>()))
//                 .Returns(new BitString("3d46bee8660f8f215d3f96408a7a64cf1c4da02b63a55f62c666ef5707a914ce").ToBytes());
//
//             var subject = new LMS.Native.Hss(logger.Object, hssKeyPairFactory, lms, lms);
//
//             // Generate the key pair, using the specified i and seed values,
//             // providing the mocked randomizerC to get the expected C value during the sign operation during
//             // HSS scheme construction
//             var keyPair = await hssKeyPairFactory.GetKeyPair(
//                 new[]
//                 {
//                     new HssLevelParameter(LmsMode.LMS_SHA256_M32_H10, LmOtsMode.LMOTS_SHA256_N32_W4),
//                     new HssLevelParameter(LmsMode.LMS_SHA256_M32_H5, LmOtsMode.LMOTS_SHA256_N32_W8)
//                 },
//                 randomizerC.Object,
//                 new BitString("d08fabd4a2091ff0a8cb4ed834e74534").ToBytes(),
//                 new BitString("558b8966c48ae9cb898b423c83443aae014a72f1b1ab5cc85cf1d892903b5439").ToBytes());
//
//             // The RFC specifies
//             // first level tree should be at q 3
//             // second level tree should be at q 4
//             // we need to preSignIncrement the bottom tree several cycles such that sig[0] is at q 3
//             // That should be a pre-sign increment of (((2^h)*3) + 3) - I think.
//             var qIncrementNeededToProgressToAppropriateValueForAllTrees = (1 << 5); // total nodes for bottom level tree
//             qIncrementNeededToProgressToAppropriateValueForAllTrees *= 3; // total progressions through the bottom level tree to get the root level at the appropriate q value
//             qIncrementNeededToProgressToAppropriateValueForAllTrees += 3; // total progressions on the bottom level tree such that the root tree is as the correct q, and the bottom tree is at the correct q for the next signature.
//
//             // sign a test message for the tree progression, using the specific randomizer C value intended for the "top level" signed public key.
//             await subject.Sign(keyPair, randomizerC.Object, new byte[1], qIncrementNeededToProgressToAppropriateValueForAllTrees);
//
//             // At this point the q values of both levels of the tree should be as expected.
//             // At this point, we're signing a new message with a different randomizerC value, as specified in the RFC,
//             // set that C value up.
//             randomizerC
//                 .Setup(s => s.GetRandomizerValueC(It.IsAny<ILmOtsPrivateKey>()))
//                 .Returns(new BitString("0eb1ed54a2460d512388cad533138d240534e97b1e82d33bd927d201dfc24ebb").ToBytes());
//
//             // The message to sign
//             var message = new BitString("54686520656e756d65726174696f6e20696e2074686520436f6e737469747574696f6e2c206f66206365727461696e207269676874732c207368616c6c206e6f7420626520636f6e73747275656420746f2064656e79206f7220646973706172616765206f74686572732072657461696e6564206279207468652070656f706c652e0a").ToBytes();
//
//             // Sign using the HSS scheme
//             var signature = await subject.Sign(keyPair, randomizerC.Object, message, 0);
//
//             // The expected signature as per the RFC
//             var expectedSignature = new BitString(
//                     "0000000100000003 000000033d46bee8 660f8f215d3f9640 8a7a64cf1c4da02b 63a55f62c666ef57 07a914ce0674e8cb 7a55f0c48d484f31 f3aa4af9719a74f2 2cf823b94431d01c 926e2a76bb71226d 279700ec81c9e95f b11a0d10d065279a 5796e265ae17737c 44eb8c594508e126 a9a7870bf4360820 bdeb9a01d9693779 e416828e75bddd7d 8c70d50a0ac8ba39 810909d445f44cb5 bb58de737e60cb43 45302786ef2c6b14 af212ca19edeaa3b fcfe8baa6621ce88 480df2371dd37add 732c9de4ea2ce0df fa53c92649a18d39 a50788f4652987f2 26a1d48168205df6 ae7c58e049a25d49 07edc1aa90da8aa5 e5f7671773e941d8 055360215c6b60dd 35463cf2240a9c06 d694e9cb54e7b1e1 bf494d0d1a28c0d3 1acc75161f4f485d fd3cb9578e836ec2 dc722f37ed30872e 07f2b8bd0374eb57 d22c614e09150f6c 0d8774a39a6e1682 11035dc52988ab46 eaca9ec597fb18b4 936e66ef2f0df26e 8d1e34da28cbb3af 752313720c7b3454 34f72d65314328bb b030d0f0f6d5e47b 28ea91008fb11b05 017705a8be3b2adb 83c60a54f9d1d1b2 f476f9e393eb5695 203d2ba6ad815e6a 111ea293dcc21033 f9453d49c8e5a638 7f588b1ea4f70621 7c151e05f55a6eb7 997be09d56a326a3 2f9cba1fbe1c07bb 49fa04cecf9df1a1 b815483c75d7a27c c88ad1b1238e5ea9 86b53e087045723c e16187eda22e33b2 c70709e53251025a bde8939645fc8c06 93e97763928f00b2 e3c75af3942d8dda ee81b59a6f1f67ef da0ef81d11873b59 137f67800b35e81b 01563d187c4a1575 a1acb92d087b517a 8833383f05d357ef 4678de0c57ff9f1b 2da61dfde5d88318 bcdde4d9061cc75c 2de3cd4740dd7739 ca3ef66f1930026f 47d9ebaa713b0717 6f76f953e1c2e7f8 f271a6ca375dbfb8 3d719b1635a7d8a1 3891957944b1c29b b101913e166e11bd 5f34186fa6c0a555 c9026b256a6860f4 866bd6d0b5bf9062 7086c6149133f828 2ce6c9b362244244 3d5eca959d6c14ca 8389d12c4068b503 e4e3c39b635bea24 5d9d05a2558f249c 9661c0427d2e489c a5b5dde220a90333 f4862aec793223c7 81997da98266c12c 50ea28b2c438e7a3 79eb106eca0c7fd6 006e9bf612f3ea0a 454ba3bdb76e8027 992e60de01e9094f ddeb3349883914fb 17a9621ab929d970 d101e45f8278c14b 032bcab02bd15692 d21b6c5c204abbf0 77d465553bd6eda6 45e6c3065d33b10d 518a61e15ed0f092 c32226281a29c8a0 f50cde0a8c66236e 29c2f310a375cebd a1dc6bb9a1a01dae 6c7aba8ebedc6371 a7d52aacb955f83b d6e4f84d2949dcc1 98fb77c7e5cdf604 0b0f84faf82808bf 985577f0a2acf2ec 7ed7c0b0ae8a270e 951743ff23e0b2dd 12e9c3c828fb5598 a22461af94d568f2 9240ba2820c4591f 71c088f96e095dd9 8beae456579ebbba 36f6d9ca2613d1c2 6eee4d8c73217ac5 962b5f3147b492e8 831597fd89b64aa7 fde82e1974d2f677 9504dc21435eb310 9350756b9fdabe1c 6f368081bd40b27e bcb9819a75d7df8b b07bb05db1bab705 a4b7e37125186339 464ad8faaa4f052c c1272919fde3e025 bb64aa8e0eb1fcbf cc25acb5f718ce4f 7c2182fb393a1814 b0e942490e52d3bc a817b2b26e90d4c9 b0cc38608a6cef5e b153af0858acc867 c9922aed43bb67d7 b33acc519313d28d 41a5c6fe6cf3595d d5ee63f0a4c4065a 083590b275788bee 7ad875a7f88dd737 20708c6c6c0ecf1f 43bbaadae6f20855 7fdc07bd4ed91f88 ce4c0de842761c70 c186bfdafafc4448 34bd3418be4253a7 1eaf41d718753ad0 7754ca3effd5960b 0336981795721426 803599ed5b2b7516 920efcbe32ada4bc f6c73bd29e3fa152 d9adeca36020fdee ee1b739521d3ea8c 0da497003df15138 97b0f54794a87367 0b8d93bcca2ae47e 64424b7423e1f078 d9554bb5232cc6de 8aae9b83fa5b9510 beb39ccf4b4e1d9c 0f19d5e17f58e5b8 705d9a6837a7d9bf 99cd13387af256a8 491671f1f2f22af2 53bcff54b673199b db7d05d81064ef05 f80f0153d0be7919 684b23da8d42ff3e ffdb7ca0985033f3 89181f4765913800 3d712b5ec0a614d3 1cc7487f52de8664 916af79c98456b2c 94a8038083db5539 1e3475862250274a 1de2584fec975fb0 9536792cfbfcf619 2856cc76eb5b13dc 4709e2f7301ddff2 6ec1b23de2d188c9 99166c74e1e14bbc 15f457cf4e471ae1 3dcbdd9c50f4d646 fc6278e8fe7eb6cb 5c94100fa8701873 80b777ed19d7868f d8ca7ceb7fa7d5cc 861c5bdac98e7495 eb0a2ceec1924ae9 79f44c5390ebeddd c65d6ec11287d978 b8df064219bc5679 f7d7b264a76ff272 b2ac9f2f7cfc9fdc fb6a51428240027a fd9d52a79b647c90 c2709e060ed70f87 299dd798d68f4fad d3da6c51d839f851 f98f67840b964ebe 73f8cec41572538e c6bc131034ca2894 eb736b3bda93d9f5 f6fa6f6c0f03ce43 362b8414940355fb 54d3dfdd03633ae1 08f3de3ebc85a3ff 51efeea3bc2cf27e 1658f1789ee612c8 3d0f5fd56f7cd071 930e2946beeecaa0 4dccea9f97786001 475e0294bc2852f6 2eb5d39bb9fbeef7 5916efe44a662eca e37ede27e9d6eadf deb8f8b2b2dbccbf 96fa6dbaf7321fb0 e701f4d429c2f4dc d153a2742574126e 5eaccc77686acf6e 3ee48f423766e0fc 466810a905ff5453 ec99897b56bc55dd 49b991142f65043f 2d744eeb935ba7f4 ef23cf80cc5a8a33 5d3619d781e74548 26df720eec82e060 34c44699b5f0c44a 8787752e057fa341 9b5bb0e25d30981e 41cb1361322dba8f 69931cf42fad3f3b ce6ded5b8bfc3d20 a2148861b2afc145 62ddd27f12897abf 0685288dcc5c4982 f826026846a24bf7 7e383c7aacab1ab6 92b29ed8c018a65f 3dc2b87ff619a633 c41b4fadb1c78725 c1f8f922f6009787 b1964247df0136b1 bc614ab575c59a16 d089917bd4a8b6f0 4d95c581279a139b e09fcf6e98a470a0 bceca191fce476f9 370021cbc05518a7 efd35d89d8577c99 0a5e19961ba16203 c959c91829ba7497 cffcbb4b29454645 4fa5388a23a22e80 5a5ca35f95659884 8bda678615fec28a fd5da61a00000006 b326493313053ced 3876db9d23714818 1b7173bc7d042cef b4dbe94d2e58cd21 a769db4657a10327 9ba8ef3a629ca84e e836172a9c50e51f 45581741cf808315 0b491cb4ecbbabec 128e7c81a46e62a6 7b57640a0a78be1c bf7dd9d419a10cd8 686d16621a80816b fdb5bdc56211d72c a70b81f1117d1295 29a7570cf79cf52a 7028a48538ecdd3b 38d3d5d62d262465 95c4fb73a525a5ed 2c30524ebb1d8cc8 2e0c19bc4977c689 8ff95fd3d310b0ba e71696cef93c6a55 2456bf96e9d075e3 83bb7543c675842b afbfc7cdb88483b3 276c29d4f0a341c2 d406e40d4653b7e4 d045851acf6a0a0e a9c710b805cced46 35ee8c107362f0fc 8d80c14d0ac49c51 6703d26d14752f34 c1c0d2c4247581c1 8c2cf4de48e9ce94 9be7c888e9caebe4 a415e291fd107d21 dc1f084b11582082 49f28f4f7c7e931b a7b3bd0d824a4570 0000000500000004 215f83b7ccb9acbc d08db97b0d04dc2b a1cd035833e0e900 59603f26e07ad2aa d152338e7a5e5984 bcd5f7bb4eba40b7 0000000400000004 0eb1ed54a2460d51 2388cad533138d24 0534e97b1e82d33b d927d201dfc24ebb 11b3649023696f85 150b189e50c00e98 850ac343a77b3638 319c347d7310269d 3b7714fa406b8c35 b021d54d4fdada7b 9ce5d4ba5b06719e 72aaf58c5aae7aca 057aa0e2e74e7dcf d17a0823429db629 65b7d563c57b4cec 942cc865e29c1dad 83cac8b4d61aacc4 57f336e6a10b6632 3f5887bf3523dfca dee158503bfaa89d c6bf59daa82afd2b 5ebb2a9ca6572a60 67cee7c327e9039b 3b6ea6a1edc7fdc3 df927aade10c1c9f 2d5ff446450d2a39 98d0f9f6202b5e07 c3f97d2458c69d3c 8190643978d7a7f4 d64e97e3f1c4a08a 7c5bc03fd55682c0 17e2907eab07e5bb 2f190143475a6043 d5e6d5263471f4ee cf6e2575fbc6ff37 edfa249d6cda1a09 f797fd5a3cd53a06 6700f45863f04b6c 8a58cfd341241e00 2d0d2c0217472bf1 8b636ae547c17713 68d9f317835c9b0e f430b3df4034f6af 00d0da44f4af7800 bc7a5cf8a5abdb12 dc718b559b74cab9 090e33cc58a95530 0981c420c4da8ffd 67df540890a062fe 40dba8b2c1c548ce d22473219c534911 d48ccaabfb71bc71 862f4a24ebd376d2 88fd4e6fb06ed870 5787c5fedc813cd2 697e5b1aac1ced45 767b14ce88409eae bb601a93559aae89 3e143d1c395bc326 da821d79a9ed41dc fbe549147f71c092 f4f3ac522b5cc572 90706650487bae9b b5671ecc9ccc2ce5 1ead87ac01985268 521222fb9057df7e d41810b5ef0d4f7c c67368c90f573b1a c2ce956c365ed38e 893ce7b2fae15d36 85a3df2fa3d4cc09 8fa57dd60d2c9754 a8ade980ad0f93f6 787075c3f680a2ba 1936a8c61d1af52a b7e21f416be09d2a 8d64c3d3d8582968 c2839902229f85ae e297e717c094c8df 4a23bb5db658dd37 7bf0f4ff3ffd8fba 5e383a48574802ed 545bbe7a6b475353 3353d73706067640 135a7ce517279cd6 83039747d218647c 86e097b0daa2872d 54b8f3e508598762 9547b830d8118161 b65079fe7bc59a99 e9c3c7380e3e70b7 138fe5d9be255150 2b698d09ae193972 f27d40f38dea264a 0126e637d74ae4c9 2a6249fa103436d3 eb0d4029ac712bfc 7a5eacbdd7518d6d 4fe903a5ae65527c d65bb0d4e9925ca2 4fd7214dc617c150 544e423f450c99ce 51ac8005d33acd74 f1bed3b17b7266a4 a3bb86da7eba80b1 01e15cb79de9a207 852cf91249ef4806 19ff2af8cabca831 25d1faa94cbb0a03 a906f683b3f47a97 c871fd513e510a7a 25f283b196075778 496152a91c2bf9da 76ebe089f4654877 f2d586ae7149c406 e663eadeb2b5c7e8 2429b9e8cb4834c8 3464f079995332e4 b3c8f5a72bb4b8c6 f74b0d45dc6c1f79 952c0b7420df525e 37c15377b5f09843 19c3993921e5ccd9 7e097592064530d3 3de3afad5733cbe7 703c5296263f7734 2efbf5a04755b0b3 c997c4328463e84c aa2de3ffdcd297ba aaacd7ae646e44b5 c0f16044df38fabd 296a47b3a838a913 982fb2e370c078ed b042c84db34ce36b 46ccb76460a690cc 86c302457dd1cde1 97ec8075e82b393d 542075134e2a17ee 70a5e187075d03ae 3c853cff60729ba4 000000054de1f696 5bdabc676c5a4dc7 c35f97f82cb0e31c 68d04f1dad96314f f09e6b3de96aeee3 00d1f68bf1bca9fc 58e4032336cd819a af578744e50d1357 a0e4286704d341aa 0a337b19fe4bc43c 2e79964d4f351089 f2e0e41c7c43ae0d 49e7f404b0f75be8 0ea3af098c975242 0a8ac0ea2bbb1f4e eba05238aef0d8ce 63f0c6e5e4041d95 398a6f7f3e0ee97c c1591849d4ed2363 38b147abde9f51ef 9fd4e1c1");
//
//             var expectedSignatureHex = expectedSignature.ToHex();
//             var actualSignatureHex = new BitString(signature.Signature).ToHex();
//
//             Console.WriteLine($"{nameof(expectedSignatureHex)}: {expectedSignatureHex}");
//             Console.WriteLine($"{nameof(actualSignatureHex)}: {actualSignatureHex}");
//
//             Assert.AreEqual(expectedSignatureHex, actualSignatureHex, nameof(expectedSignatureHex));
//         }
//
//         private class SeedIdRotatorWithTracking : SeedIdRotator
//         {
//             private readonly HashSet<string> _idSeedResults = new();
//
//             public override IdSeedResult GetNewSeedId(ISha sha, LmsAttribute lmsAttribute, byte[] seed, byte[] i, int level)
//             {
//                 var newSeedId = base.GetNewSeedId(sha, lmsAttribute, seed, i, level);
//
//                 var concat = $"{new BitString(newSeedId.Seed).ToHex()}|{new BitString(newSeedId.I).ToHex()}";
//                 if (_idSeedResults.Contains(concat))
//                     throw new Exception(
//                         $"The seed/i combination has already been used.\nSeed: {newSeedId.Seed}\nI:{newSeedId.I}");
//
//                 _idSeedResults.Add(concat);
//
//                 return newSeedId;
//             }
//         }
//
//         /// <summary>
//         /// Utilizing the <see cref="SeedIdRotator"/>, we want to make sure a HSS scheme has unique seed/ids for the full
//         /// lifetime of the scheme.
//         /// </summary>
//         [Test]
//         public async Task WhenGeneratingMultiLevelHss_AllSeedIdsShouldBeUnique()
//         {
//             var logger = new Mock<ILogger<LMS.Native.Hss>>();
//             var shaFactory = new NativeShaFactory();
//             var lmOtsKeyPairFactory = new LmOtsKeyPairFactory(shaFactory);
//             var lmsKeyPairFactory = new LmsKeyPairFactory(lmOtsKeyPairFactory, shaFactory);
//             var lmOts = new LmOts(shaFactory);
//             var lms = new LMS.Native.Lms(lmOts, shaFactory);
//             var seedIdRotator = new SeedIdRotatorWithTracking();
//             var hssKeyPairFactory = new HssKeyPairFactory(lmsKeyPairFactory, lms, shaFactory, seedIdRotator);
//             var subject = new LMS.Native.Hss(logger.Object, hssKeyPairFactory, lms, lms);
//
//             var lmsMode = LmsMode.LMS_SHA256_M24_H5;
//
//             var rootTreeAttribute = AttributesHelper.GetLmsAttribute(lmsMode);
//
//             var hssLevelParameters = GetHssLevelParameters(
//                 new[] { lmsMode, lmsMode, lmsMode },
//                 LmOtsMode.LMOTS_SHA256_N24_W1);
//
//             var keyPair = await hssKeyPairFactory.GetKeyPair(
//                 hssLevelParameters,
//                 _randomizerC,
//                 new byte[16],
//                 new byte[rootTreeAttribute.M]);
//
//             var message = new byte[16];
//             var totalSignOperations = 0;
//             while (!keyPair.IsExhausted)
//             {
//                 try
//                 {
//                     await subject.Sign(keyPair, _randomizerC, message, 0);
//                     totalSignOperations++;
//                 }
//                 catch (Exception)
//                 {
//                     Assert.Fail();
//                 }
//             }
//
//             Assert.AreEqual(1 << (5 * 3), totalSignOperations, "sign operations");
//             Assert.Pass();
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
