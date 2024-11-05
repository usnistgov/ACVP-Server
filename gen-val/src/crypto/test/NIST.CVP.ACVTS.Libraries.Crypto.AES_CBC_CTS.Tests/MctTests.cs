using System.IO;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_CBC_CTS.Tests
{
    [TestFixture, FastCryptoTest]
    public class MctTests
    {
        [SetUp]
        public void Setup()
        {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget("File");

            fileTarget.FileName = Path.Combine(Directory.GetCurrentDirectory(), "log.log");
            fileTarget.Layout = "${longdate} ${level} ${logger} ${message}";
            fileTarget.DeleteOldFileOnStartup = true;

            config.AddTarget(fileTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

            LogManager.Configuration = config;
        }

        private readonly MonteCarloAesCbcCts _subject = new MonteCarloAesCbcCts(
            new BlockCipherEngineFactory(),
            new ModeBlockCipherFactory(),
            new AesMonteCarloKeyMaker(),
            BlockCipherModesOfOperation.CbcCs3);

        [Test]
        [TestCase(
            // Key
            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
            // IV
            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
            // Payload
            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
            // Check index
            0,
            // expected CT at index
            "75 4B A8 5A D4 3E 85 B2 74 16 61 B5 C4 C5 40 DE E7 38 A3 8F 3A BB 14 25")]
        public void ShouldEncrypt(string keyString, string ivString, string payloadString, int indexCt, string expectedCtString)
        {
            var key = new BitString(keyString);
            var iv = new BitString(ivString);
            var payload = new BitString(payloadString);
            var expectedCt = new BitString(expectedCtString);

            var result =
                _subject.ProcessMonteCarloTest(new ModeBlockCipherParameters(BlockCipherDirections.Encrypt, iv, key, payload));

            Assert.That(result.Response[indexCt].CipherText.ToHex(), Is.EqualTo(expectedCt.ToHex()));
        }
    }
}
