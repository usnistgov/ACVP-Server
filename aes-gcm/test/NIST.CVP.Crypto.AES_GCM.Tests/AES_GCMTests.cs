using System;
using System.IO;
using Moq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_GCM.Tests
{
    [TestFixture, UnitTest]
    public class AES_GCMTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            ConfigureLogging();
        }

        [Test]
        public void ShouldEncryptSuccessfully()
        {
            ThisLogger.Debug("");
            var key = new BitString("FEFFE992 8665731C 6D6A8F94 67308308");
            var iv = new BitString("CAFEBABE FACEDBAD DECAF888");
            var aad = new BitString(0);
            var plainText = new BitString(0);

            var subject = new AES_GCM(
                new AES_GCMInternals(
                    new RijndaelFactory(
                        new RijndaelInternals()
                    )
                ),
                new RijndaelFactory(
                    new RijndaelInternals()
                )
            );
            var results = subject.BlockEncrypt(key, plainText, iv, aad, 128);
            Assert.IsTrue(results.Success);

            var dResults = subject.BlockDecrypt(key, results.CipherText, iv, aad, results.Tag);
            Assert.IsTrue(dResults.Success);

            Assert.AreEqual(new BitString("3247184B 3C4F69A4 4DBCD228 87BBB418"), results.Tag);
        }

        [Test]
        public void ShouldEncryptWithPlainTextSuccessfully()
        {
            ThisLogger.Debug("");
            var key = new BitString("FEFFE992 8665731C 6D6A8F94 67308308");
            var iv = new BitString("CAFEBABE FACEDBAD DECAF888");
            var aad = new BitString(0);
            var plainText = new BitString("D9313225 F88406E5 A55909C5 AFF5269A 86A7A953 1534F7DA 2E4C303D 8A318A72 1C3C0C95 95680953 2FCF0E24 49A6B525 B16AEDF5 AA0DE657 BA637B39 1AAFD255");

            var subject = new AES_GCM(
                new AES_GCMInternals(
                    new RijndaelFactory(
                        new RijndaelInternals()
                    )
                ),
                new RijndaelFactory(
                    new RijndaelInternals()
                )
            );
            var results = subject.BlockEncrypt(key, plainText, iv, aad, 128);
            Assert.IsTrue(results.Success);
            ThisLogger.Debug(results.Tag.ToHex());

            var dResults = subject.BlockDecrypt(key, results.CipherText, iv, aad, results.Tag);
            Assert.IsTrue(dResults.Success);
            Assert.AreEqual(plainText, dResults.PlainText);
            Assert.AreEqual(new BitString("4D5C2AF3 27CD64A6 2CF35ABD 2BA6FAB4"), results.Tag);
        }

        [Test]
        public void ShouldReturnDecryptionResultWithErrorOnException()
        {
            Mock<IAES_GCMInternals> iAes_gcmInternals = new Mock<IAES_GCMInternals>();
            Mock<IRijndaelFactory> iRijndaelFactory = new Mock<IRijndaelFactory>();
            AES_GCM subject = new AES_GCM(iAes_gcmInternals.Object, iRijndaelFactory.Object);
            string exceptionMessage = "Something bad happened.";

            iRijndaelFactory
                .Setup(s => s.GetRijndael(It.IsAny<ModeValues>()))
                .Throws(new Exception(exceptionMessage));

            var results = subject.BlockDecrypt(
                new BitString(0),
                new BitString(0),
                new BitString(0),
                new BitString(0),
                new BitString(0)
            );

            Assert.IsFalse(results.Success, nameof(results));
            Assert.IsInstanceOf<DecryptionResult>(results, $"{nameof(results)} type");
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
            Assert.IsInstanceOf<EncryptionResult>(results, $"{nameof(results)} type");
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
