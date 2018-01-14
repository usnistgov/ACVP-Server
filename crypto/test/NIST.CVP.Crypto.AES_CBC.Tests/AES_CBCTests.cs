using System;
using Moq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_CBC.Tests
{
    [TestFixture,  FastCryptoTest]
    public class AES_CBCTests
    {
        [Test]
        public void ShouldReturnDecryptionResultWithErrorOnException()
        {
            Mock<IRijndaelFactory> iRijndaelFactory = new Mock<IRijndaelFactory>();
            Crypto.AES_CBC.AES_CBC subject = new Crypto.AES_CBC.AES_CBC(iRijndaelFactory.Object);
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
            Crypto.AES_CBC.AES_CBC subject = new Crypto.AES_CBC.AES_CBC(iRijndaelFactory.Object);

            iRijndaelFactory
                .Setup(s => s.GetRijndael(It.IsAny<ModeValues>()))
                .Returns(new RijndaelCBC(iRijndaelInternals.Object));

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
            Crypto.AES_CBC.AES_CBC subject = new Crypto.AES_CBC.AES_CBC(iRijndaelFactory.Object);
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
            Crypto.AES_CBC.AES_CBC subject = new Crypto.AES_CBC.AES_CBC(iRijndaelFactory.Object);

            iRijndaelFactory
                .Setup(s => s.GetRijndael(It.IsAny<ModeValues>()))
                .Returns(new RijndaelCBC(iRijndaelInternals.Object));

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

        //[Test]
        public void SpecificTest()
        {
            var rijndaelInternals = new RijndaelInternals();
            var rijndaelFactory = new RijndaelFactory(rijndaelInternals);
            var subject = new AES_CBC(rijndaelFactory);

            var key = new BitString("72D6E1A903180DF8889E4112FE2090C1");
            var iv = new BitString("9B2B6F3AF5F4A1651797EF34676B3719");
            var pt = new BitString("F56B38666B2EF044B70A7BDD8054FED33F2E5D5D2F061D097E2AAACA0CDB4DA8");
            var ct = new BitString("69644FE62ED7F8023B6DDF6A4FFFC0F4C4028B96AE265A5B8F9AC5F2756281D3");

            var result = subject.BlockEncrypt(iv, key, pt);

            Assert.AreEqual(ct.ToHex(), result.Result.ToHex());
        }
    }
}
