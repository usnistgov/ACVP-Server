using System;
using Moq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_CFB1.Tests
{
    [TestFixture]
    public class AES_CFB1Tests
    {
        [Test]
        public void ShouldReturnDecryptionResultWithErrorOnException()
        {
            Mock<IRijndaelFactory> iRijndaelFactory = new Mock<IRijndaelFactory>();
            AES_CFB1 subject = new AES_CFB1(iRijndaelFactory.Object);
            string exceptionMessage = "Something bad happened.";

            iRijndaelFactory
                .Setup(s => s.GetRijndael(It.IsAny<ModeValues>()))
                .Throws(new Exception(exceptionMessage));

            var results = subject.BlockDecrypt(
                new BitString(0), 
                new BitString(0),
                new BitString(0)
            );

            Assert.IsFalse(results.Success, nameof(results));
            Assert.IsInstanceOf<DecryptionResult>(results, $"{nameof(results)} type");
            Assert.AreEqual(exceptionMessage, results.ErrorMessage, nameof(exceptionMessage));
        }

        [Test]
        public void ShouldInvokeRijndaelBlockEncryptForDecrypt()
        {
            Mock<IRijndaelFactory> iRijndaelFactory = new Mock<IRijndaelFactory>();
            Mock<IRijndaelInternals> iRijndaelInternals = new Mock<IRijndaelInternals>();
            AES_CFB1 subject = new AES_CFB1(iRijndaelFactory.Object);

            iRijndaelFactory
                .Setup(s => s.GetRijndael(It.IsAny<ModeValues>()))
                .Returns(new RijndaelCFB1(iRijndaelInternals.Object));

            var results = subject.BlockDecrypt(
                new BitString(128),
                new BitString(128),
                new BitString(1)
            );

            Assert.IsTrue(results.Success, nameof(results));
            Assert.IsInstanceOf<DecryptionResult>(results, $"{nameof(results)} type");
            iRijndaelInternals.Verify(v => v.EncryptSingleBlock(It.IsAny<byte[,]>(), It.IsAny<Key>()),
                Times.AtLeastOnce(),
                nameof(iRijndaelInternals.Object.EncryptSingleBlock)
            );
        }

        [Test]
        public void ShouldReturnEncryptionResultWithErrorOnException()
        {
            Mock<IRijndaelFactory> iRijndaelFactory = new Mock<IRijndaelFactory>();
            AES_CFB1 subject = new AES_CFB1(iRijndaelFactory.Object);
            string exceptionMessage = "Something bad happened, sorry about that.";

            iRijndaelFactory
                .Setup(s => s.GetRijndael(It.IsAny<ModeValues>()))
                .Throws(new Exception(exceptionMessage));

            var results = subject.BlockEncrypt(
                new BitString(0),
                new BitString(0),
                new BitString(0)
            );

            Assert.IsFalse(results.Success, nameof(results));
            Assert.IsInstanceOf<EncryptionResult>(results, $"{nameof(results)} type");
            Assert.AreEqual(exceptionMessage, results.ErrorMessage, nameof(exceptionMessage));
        }

        [Test]
        public void ShouldInvokeRijndaelBlockEncryptForEncrypt()
        {
            Mock<IRijndaelFactory> iRijndaelFactory = new Mock<IRijndaelFactory>();
            Mock<IRijndaelInternals> iRijndaelInternals = new Mock<IRijndaelInternals>();
            AES_CFB1 subject = new AES_CFB1(iRijndaelFactory.Object);

            iRijndaelFactory
                .Setup(s => s.GetRijndael(It.IsAny<ModeValues>()))
                .Returns(new RijndaelCFB1(iRijndaelInternals.Object));

            var results = subject.BlockEncrypt(
                new BitString(128),
                new BitString(128),
                new BitString(1)
            );

            Assert.IsTrue(results.Success, nameof(results));
            Assert.IsInstanceOf<EncryptionResult>(results, $"{nameof(results)} type");
            iRijndaelInternals.Verify(v => v.EncryptSingleBlock(It.IsAny<byte[,]>(), It.IsAny<Key>()),
                Times.AtLeastOnce(),
                nameof(iRijndaelInternals.Object.EncryptSingleBlock)
            );
        }
    }
}
