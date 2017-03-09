using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Math;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;
using NIST.CVP.Generation.AES;

namespace NIST.CVP.Generation.AES_CCM.Tests
{
    [TestFixture]
    public class AES_CCMTests
    {
        [Test]
        public void ShouldReturnDecryptionResultWithErrorOnException()
        {
            Mock<IAES_CCMInternals> iAES_CCMInternals = new Mock<IAES_CCMInternals>();
            Mock<IRijndaelFactory> iRijndaelFactory = new Mock<IRijndaelFactory>();
            AES_CCM subject = new AES_CCM(iAES_CCMInternals.Object, iRijndaelFactory.Object);
            string exceptionMessage = "Something bad happened.";

            iRijndaelFactory
                .Setup(s => s.GetRijndael(It.IsAny<ModeValues>()))
                .Throws(new Exception(exceptionMessage));

            var results = subject.Decrypt(
                new BitString(0),
                new BitString(0),
                new BitString(0),
                new BitString(0),
                8
            );

            Assert.IsFalse(results.Success, nameof(results));
            Assert.IsInstanceOf<DecryptionResult>(results, $"{nameof(results)} type");
            Assert.AreEqual(exceptionMessage, results.ErrorMessage, nameof(exceptionMessage));
        }

        [Test]
        public void ShouldReturnEncryptionResultWithErrorOnException()
        {
            Mock<IAES_CCMInternals> iAES_CCMInternals = new Mock<IAES_CCMInternals>();
            Mock<IRijndaelFactory> iRijndaelFactory = new Mock<IRijndaelFactory>();
            AES_CCM subject = new AES_CCM(iAES_CCMInternals.Object, iRijndaelFactory.Object);
            string exceptionMessage = "Something bad happened, sorry about that.";

            iRijndaelFactory
                .Setup(s => s.GetRijndael(It.IsAny<ModeValues>()))
                .Throws(new Exception(exceptionMessage));

            var results = subject.Encrypt(
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
            get { return LogManager.GetLogger("AES_CCM"); }
        }
    }
}
