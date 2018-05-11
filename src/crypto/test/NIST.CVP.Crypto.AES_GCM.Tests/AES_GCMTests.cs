using System;
using System.IO;
using Moq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_GCM.Tests
{
    [TestFixture,  FastCryptoTest]
    public class AES_GCMTests
    {
        private IAES_GCM _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new AES_GCM(
                new AES_GCMInternals(new ModeBlockCipherFactory(), new BlockCipherEngineFactory()),
                new RijndaelFactory(new RijndaelInternals())
            );
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            ConfigureLogging();
        }

        [Test]
        public void ShouldReturnDecryptionResultWithErrorOnException()
        {
            Mock<IAES_GCMInternals> iAes_gcmInternals = new Mock<IAES_GCMInternals>();
            Mock<IRijndaelFactory> iRijndaelFactory = new Mock<IRijndaelFactory>();
            _subject = new AES_GCM(iAes_gcmInternals.Object, iRijndaelFactory.Object);
            string exceptionMessage = "Something bad happened.";

            iRijndaelFactory
                .Setup(s => s.GetRijndael(It.IsAny<ModeValues>()))
                .Throws(new Exception(exceptionMessage));

            var results = _subject.BlockDecrypt(
                new BitString(0),
                new BitString(0),
                new BitString(0),
                new BitString(0),
                new BitString(0)
            );

            Assert.IsFalse(results.Success, nameof(results));
            Assert.IsInstanceOf<SymmetricCipherResult>(results, $"{nameof(results)} type");
            Assert.AreEqual(exceptionMessage, results.ErrorMessage, nameof(exceptionMessage));
        }

        [Test]
        public void ShouldReturnEncryptionResultWithErrorOnException()
        {
            Mock<IAES_GCMInternals> iAes_gcmInternals = new Mock<IAES_GCMInternals>();
            Mock<IRijndaelFactory> iRijndaelFactory = new Mock<IRijndaelFactory>();
            AES_GCM subject = new AES_GCM(iAes_gcmInternals.Object, iRijndaelFactory.Object);
            string exceptionMessage = "Something bad happened, sorry about that.";

            iRijndaelFactory
                .Setup(s => s.GetRijndael(It.IsAny<ModeValues>()))
                .Throws(new Exception(exceptionMessage));

            var results = subject.BlockEncrypt(
                new BitString(0),
                new BitString(0),
                new BitString(0),
                new BitString(0),
                It.IsAny<int>()
            );

            Assert.IsFalse(results.Success, nameof(results));
            Assert.IsInstanceOf<SymmetricCipherAeadResult>(results, $"{nameof(results)} type");
            Assert.AreEqual(exceptionMessage, results.ErrorMessage, nameof(exceptionMessage));
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetLogger("AES_GCM"); }
        }

        private void ConfigureLogging()
        {
            var config = new LoggingConfiguration();



            var debugTarget = new DebugTarget("AES_GCMD");
            debugTarget.Layout = "${message}";
            config.AddTarget(debugTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, debugTarget));

            var consoleTarget = new ConsoleTarget("AES_GCM1");
            consoleTarget.Layout = "${message}";
            config.AddTarget(consoleTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));

            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);
            string baseDir = @"C:\Users\def2\Documents\UnitTests\ACAVP";
            fileTarget.FileName = Path.Combine(baseDir, "test_aes-gcm.log");
            fileTarget.Layout = "${callsite}:  ${message}";
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

            LogManager.Configuration = config;
        }
    }
}
