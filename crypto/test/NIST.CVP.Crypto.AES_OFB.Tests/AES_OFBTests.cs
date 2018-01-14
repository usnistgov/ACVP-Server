using System;
using Moq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_OFB.Tests
{
    [TestFixture,  FastCryptoTest]
    public class AES_OFBTests
    {
        [Test]
        public void ShouldReturnDecryptionResultWithErrorOnException()
        {
            Mock<IRijndaelFactory> iRijndaelFactory = new Mock<IRijndaelFactory>();
            AES_OFB subject = new AES_OFB(iRijndaelFactory.Object);
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
            Assert.IsInstanceOf<SymmetricCipherResult>(results, $"{nameof(results)} type");
            Assert.AreEqual(exceptionMessage, results.ErrorMessage, nameof(exceptionMessage));
        }

        [Test]
        public void ShouldInvokeRijndaelBlockEncryptForDecrypt()
        {
            Mock<IRijndaelFactory> iRijndaelFactory = new Mock<IRijndaelFactory>();
            Mock<IRijndaelInternals> iRijndaelInternals = new Mock<IRijndaelInternals>();
            AES_OFB subject = new AES_OFB(iRijndaelFactory.Object);

            iRijndaelFactory
                .Setup(s => s.GetRijndael(It.IsAny<ModeValues>()))
                .Returns(new RijndaelOFB(iRijndaelInternals.Object));

            var results = subject.BlockDecrypt(
                new BitString(128),
                new BitString(128),
                new BitString(128)
            );

            Assert.IsTrue(results.Success, nameof(results));
            Assert.IsInstanceOf<SymmetricCipherResult>(results, $"{nameof(results)} type");
            iRijndaelInternals.Verify(v => v.EncryptSingleBlock(It.IsAny<byte[,]>(), It.IsAny<Key>()),
                Times.AtLeastOnce(),
                nameof(iRijndaelInternals.Object.EncryptSingleBlock)
            );
        }

        [Test]
        public void ShouldReturnEncryptionResultWithErrorOnException()
        {
            Mock<IRijndaelFactory> iRijndaelFactory = new Mock<IRijndaelFactory>();
            AES_OFB subject = new AES_OFB(iRijndaelFactory.Object);
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
            Assert.IsInstanceOf<SymmetricCipherResult>(results, $"{nameof(results)} type");
            Assert.AreEqual(exceptionMessage, results.ErrorMessage, nameof(exceptionMessage));
        }

        [Test]
        public void ShouldInvokeRijndaelBlockEncryptForEncrypt()
        {
            Mock<IRijndaelFactory> iRijndaelFactory = new Mock<IRijndaelFactory>();
            Mock<IRijndaelInternals> iRijndaelInternals = new Mock<IRijndaelInternals>();
            AES_OFB subject = new AES_OFB(iRijndaelFactory.Object);

            iRijndaelFactory
                .Setup(s => s.GetRijndael(It.IsAny<ModeValues>()))
                .Returns(new RijndaelOFB(iRijndaelInternals.Object));

            var results = subject.BlockEncrypt(
                new BitString(128),
                new BitString(128),
                new BitString(128)
            );

            Assert.IsTrue(results.Success, nameof(results));
            Assert.IsInstanceOf<SymmetricCipherResult>(results, $"{nameof(results)} type");
            iRijndaelInternals.Verify(v => v.EncryptSingleBlock(It.IsAny<byte[,]>(), It.IsAny<Key>()),
                Times.AtLeastOnce(),
                nameof(iRijndaelInternals.Object.EncryptSingleBlock)
            );
        }
    }
}
