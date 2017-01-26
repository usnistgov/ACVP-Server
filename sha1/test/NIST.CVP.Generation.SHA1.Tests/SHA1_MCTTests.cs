using Moq;
using NIST.CVP.Generation.SHA;
using NIST.CVP.Math;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;
using System;

namespace NIST.CVP.Generation.SHA1.Tests
{
    [TestFixture]
    public class SHA1_MCTTests
    {
        private Mock<ISHA1> _sha1;
        private SHA1_MCT _subject;

        //[OneTimeSetUp]
        //public void OneTimeSetUp()
        //{
        //    var config = new LoggingConfiguration();
        //
        //    var fileTarget = new FileTarget();
        //    config.AddTarget("file", fileTarget);
        //
        //    fileTarget.FileName = @"C:\Users\ctc\Documents\testLog.log"; //put your log file where you want it
        //    fileTarget.Layout = "${callsite:includeNamespace=false}:  ${message}";
        //    config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));
        //
        //    LogManager.Configuration = config;
        //}

        [SetUp]
        public void SetUp()
        {
            _sha1 = new Mock<ISHA1>();
            _subject = new SHA1_MCT(_sha1.Object);
        }

        [Test]
        public void ShouldRunHashOperation100000TimesForTestCase()
        {
            var message = new BitString("ABCD");
            _sha1
                .Setup(s => s.HashMessage(It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString(160)));

            var result = _subject.MCTHash(message);

            Assert.IsTrue(result.Success, nameof(result.Success));
            _sha1.Verify(v => v.HashMessage(It.IsAny<BitString>()), Times.Exactly(100000), nameof(_sha1.Object.HashMessage));
        }

        [Test]
        public void ShouldReturnHashResponseWith100Count()
        {
            var message = new BitString("ABCD");
            _sha1
                .Setup(s => s.HashMessage(It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString(160)));

            var result = _subject.MCTHash(message);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(100, result.Response.Count);
        }

        [Test]
        public void ShouldReturnErrorMessageOnErrorHash()
        {
            string error = "Algo failure!";

            var message = new BitString("ABCD");
            _sha1
                .Setup(s => s.HashMessage(It.IsAny<BitString>()))
                .Throws(new Exception(error));

            var result = _subject.MCTHash(message);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(error, result.ErrorMessage, nameof(result.ErrorMessage));
        }

        [Test]
        [TestCase(
            "0D93 736A F479 3413 72D2 EED3 A909 F5FE ACB6 0E92",
            "ADCB 8A75 9D08 759C E0E1 920B 0E42 6C68 C7ED 424C"
        )]
        [Ignore("Takes ~3 minutes to run. Only run when needed.")]
        public void ShouldRunFullMCTHashWithCorrectResponse(string seedHex, string expectedResultHex)
        {
            var seed = new BitString(seedHex);
            var expectedResult = new BitString(expectedResultHex);

            var shaSubject = new SHA1_MCT(new SHA1());

            var result = shaSubject.MCTHash(seed);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Response[result.Response.Count-1].Digest);
        }
    }
}
