using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NUnit.Framework;
using KdfResult = NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.KdfResult;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF.IntegrationTests
{
    [TestFixture]
    public class DebugTests
    {
        [SetUp]
        public void Setup()
        {
            _oracle = new OracleBuilder().Build().GetAwaiter().GetResult();
        }

        private IOracle _oracle;

        // [Test]
        // public async Task MultipleIterationSerialCrypto()
        // {
        // 	var interations = 100;
        // 	var param = new KdfParameters
        // 	{
        // 		Mode = KdfModes.Counter,
        // 		MacMode = MacModes.HMAC_SHA3_224,
        // 		CounterLocation = CounterLocations.AfterFixedData,
        // 		CounterLength = 8,
        // 		KeyOutLength = 4096,
        // 		ZeroLengthIv = true
        // 	};
        //
        // 	// Get the deferred tasks (random values)
        // 	var deferredTasks = new Dictionary<int, Task<KdfResult>>();
        // 	for (var i = 0; i < interations; i++)
        // 	{
        // 		deferredTasks.Add(i, _oracle.GetDeferredKdfCaseAsync(param));
        // 	}
        //
        // 	await Task.WhenAll(deferredTasks.Values);
        //
        // 	// Run the random values through complete deferred to the get the "answers"
        //
        // 	Assert.Multiple(async () =>
        // 	{
        // 		for (var i = 0; i < interations; i++)
        // 		{
        // 			var result = await _oracle.CompleteDeferredKdfCaseAsync(param, deferredTasks[i].Result);
        // 			var secondResult = await _oracle.CompleteDeferredKdfCaseAsync(param, deferredTasks[i].Result);
        // 			
        // 			Assert.AreEqual(result.KeyOut.ToHex(), secondResult.KeyOut.ToHex(), $"{i}");
        // 		}				
        // 	});
        // }

        [Test]
        public async Task MultipleIterationConcurrentCrypto()
        {
            var interations = 100;
            var param = new KdfParameters
            {
                Mode = KdfModes.Counter,
                MacMode = MacModes.HMAC_SHA3_224,
                CounterLocation = CounterLocations.AfterFixedData,
                CounterLength = 8,
                KeyOutLength = 4096,
                ZeroLengthIv = true
            };

            // Get the deferred tasks (random values)
            var deferredTasks = new Dictionary<int, Task<KdfResult>>();
            for (var i = 0; i < interations; i++)
            {
                deferredTasks.Add(i, _oracle.GetDeferredKdfCaseAsync(param));
            }

            await Task.WhenAll(deferredTasks.Values);

            // Run the random values through complete deferred to the get the "answers"
            var resultTasks = new Dictionary<int, Task<KdfResult>>();
            for (var i = 0; i < interations; i++)
            {
                resultTasks.Add(i, _oracle.CompleteDeferredKdfCaseAsync(param, deferredTasks[i].Result));
            }

            await Task.WhenAll(resultTasks.Values);

            // Do it again to make sure we get the same answers
            var secondRun = new Dictionary<int, Task<KdfResult>>();
            for (var i = 0; i < interations; i++)
            {
                secondRun.Add(i, _oracle.CompleteDeferredKdfCaseAsync(param, deferredTasks[i].Result));
            }

            await Task.WhenAll(secondRun.Values);

            // Compare the first run and second run values
            for (var i = 0; i < interations; i++)
            {
                Assert.That(secondRun[i].Result.KeyOut.ToHex(), Is.EqualTo(resultTasks[i].Result.KeyOut.ToHex()), $"{i}");
            }
        }

        [Test]
        public async Task FailingTestThroughOracle()
        {
            var kdfMode = KdfModes.Counter;
            var macMode = MacModes.HMAC_SHA3_224;
            var counterLocation = CounterLocations.AfterFixedData;

            var keyOutLength = 699;
            var counterLength = 8;

            var keyIn = new BitString("136D08625A3AE8C0A819144486836722");
            var fixedData = new BitString("CED33D446B19826517DD99D844AB5060");
            var iv = new BitString("CD26B4517542DA0195A4C3C00FB3D80E3ADA4670493EFB08E93FFFC7");
            var breakLocation = 43;
            var expectedKeyOut = new BitString("8EC5E4CF42E5CBB09620E5AE4B9F3276EC24DCB80B573E1A00BACB37BFEF145789560A0A54A9DFF6FC5430347A2D48E016452F793E259345F7367423D2476182657ABB67A18D0BA910212AF53AD6761C24BE872AC3EA07A0");

            var result = await _oracle.CompleteDeferredKdfCaseAsync(new KdfParameters
            {
                Mode = kdfMode,
                CounterLength = counterLength,
                CounterLocation = counterLocation,
                MacMode = macMode,
                KeyOutLength = keyOutLength,
                ZeroLengthIv = false
            }, new KdfResult
            {
                Iv = iv,
                BreakLocation = breakLocation,
                FixedData = fixedData,
                KeyIn = keyIn
            });

            Assert.That(result.KeyOut.ToHex(), Is.EqualTo(expectedKeyOut.ToHex()));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Passes(bool cImpl)
        {
            var kdfMode = KdfModes.Counter;
            var macMode = MacModes.HMAC_SHA3_224;
            var counterLocation = CounterLocations.AfterFixedData;

            var keyOutLength = 699;
            var counterLength = 8;

            var keyIn = new BitString("136D08625A3AE8C0A819144486836722");
            var fixedData = new BitString("CED33D446B19826517DD99D844AB5060");
            var iv = new BitString("CD26B4517542DA0195A4C3C00FB3D80E3ADA4670493EFB08E93FFFC7");
            var breakLocation = 43;
            var expectedKeyOut = new BitString("8EC5E4CF42E5CBB09620E5AE4B9F3276EC24DCB80B573E1A00BACB37BFEF145789560A0A54A9DFF6FC5430347A2D48E016452F793E259345F7367423D2476182657ABB67A18D0BA910212AF53AD6761C24BE872AC3EA07A0");

            var kdf = GetKdf(cImpl, kdfMode, macMode, counterLocation, counterLength);

            var result = kdf.DeriveKey(keyIn, fixedData, keyOutLength, iv, breakLocation);

            Assert.That(result.DerivedKey.ToHex(), Is.EqualTo(expectedKeyOut.ToHex()));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Fails(bool cImpl)
        {
            var kdfMode = KdfModes.Counter;
            var macMode = MacModes.HMAC_SHA3_224;
            var counterLocation = CounterLocations.AfterFixedData;

            var keyOutLength = 699;
            var counterLength = 8;

            var keyIn = new BitString("098036054F1A6EFC26685699CF7BAB0F");
            var fixedData = new BitString("923B56CCD4E7B38959CC01AC181402B5");
            var iv = new BitString("98E1514715DC7E84A7FB52F3828B2AB51779069425DE3A6437E78C5A");
            var breakLocation = 109;
            var computedKeyOut = new BitString(
                "D51EE9AFFC3F003E1307CC01DF6377148378029ACCD3F624E9C404E80EF85A544F9D6B0909E07057DC534453B7A8CF7117F02147D7F586624DE4FA5E6F3437161F16816D1216EEC1225C5709CBD42398C1363930202A4560");

            // var expectedKeyOutFromInternalProjection = new BitString(
            // 	"D51EE9AFFC3F003E1307CC01DF6377148378029ACCD3F624E9C404E80EF85A544F9D6B0909E07057DC534453B7A8CF7117F02147D7F58662DC308271D4650EEE9201EEFB2F1A00104CD94000D5CDC7CCEBAD951B202A4560");

            var kdf = GetKdf(cImpl, kdfMode, macMode, counterLocation, counterLength);

            var result = kdf.DeriveKey(keyIn, fixedData, keyOutLength, iv, breakLocation);

            Assert.Multiple(() =>
            {
                Assert.That(result.DerivedKey.ToHex(), Is.EqualTo(computedKeyOut.ToHex()), nameof(computedKeyOut));
                //Assert.AreEqual(expectedKeyOutFromInternalProjection.ToHex(), result.DerivedKey.ToHex(), nameof(expectedKeyOutFromInternalProjection));
            });
        }

        private IKdf GetKdf(bool cImpl, KdfModes kdfMode, MacModes macMode, CounterLocations counterLocation, int counterLength)
        {
            IKdfFactory kdfFactory = null;
            if (cImpl)
            {
                kdfFactory = new KdfFactory(new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()), new HmacFactory(new NativeShaFactory()), new KmacFactory(new cSHAKEWrapper()));

            }
            else
            {
                kdfFactory = new KdfFactory(new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()), new HmacFactory(new NativeShaFactory()), new KmacFactory(new cSHAKEWrapper()));
            }

            return kdfFactory.GetKdfInstance(kdfMode, macMode, counterLocation, counterLength);
        }
    }
}
