using System;
using Moq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
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

        [Test]
        public void ShouldEncryptMultiblockCorrectlyNewEngine()
        {
            var key = new BitString("0777ef9c046d80cefe0f2b5bedc3cc82");
            var iv = new BitString("1f88d353dd8f39eced080debba862a13");
            var pt = new BitString(
                "a9adff5fec2ed8f30b7b2e7d0fce61a688c0b085c6b30a1e4c7120288211bf14aa1b5d4a15ecf2b6975b0983389c16262c650a9f065025fb28810b1e7e881bbf6c88fcb4049978707d6c8ecae02ec20005aeb8fbbec101d452e635982d264248d53e032420001529a5f7e6d9704a0e3afae2e32158cf47b0b2e8c51ef7b2ddb6");
            var ct = new BitString(
                "6161513687577c3b284778adbaafdf15372868f4651211d0f8733bded466a37662e9836318a5d9856ffddc8d99e7243882bd6f9e2ec5284c378d045b163ad65ed6b1c01276a8f94ca028dc3777d116955d788fba40518cbf63e98e96d30c8ff3f1828db4a8a7f1bce5c3885eea082ac6a90baf9c3bb8311dc04e19a4a790c53b");

            var subject = new OfbBlockCipher(new AesEngine());

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                pt
            );
            var result = subject.ProcessPayload(param);

            Assert.AreEqual(ct.ToHex(), result.Result.ToHex());
        }

        [Test]
        public void ShouldDecryptMultiblockCorrectlyNewEngine()
        {
            var key = new BitString("0777ef9c046d80cefe0f2b5bedc3cc82");
            var iv = new BitString("1f88d353dd8f39eced080debba862a13");
            var pt = new BitString(
                "a9adff5fec2ed8f30b7b2e7d0fce61a688c0b085c6b30a1e4c7120288211bf14aa1b5d4a15ecf2b6975b0983389c16262c650a9f065025fb28810b1e7e881bbf6c88fcb4049978707d6c8ecae02ec20005aeb8fbbec101d452e635982d264248d53e032420001529a5f7e6d9704a0e3afae2e32158cf47b0b2e8c51ef7b2ddb6");
            var ct = new BitString(
                "6161513687577c3b284778adbaafdf15372868f4651211d0f8733bded466a37662e9836318a5d9856ffddc8d99e7243882bd6f9e2ec5284c378d045b163ad65ed6b1c01276a8f94ca028dc3777d116955d788fba40518cbf63e98e96d30c8ff3f1828db4a8a7f1bce5c3885eea082ac6a90baf9c3bb8311dc04e19a4a790c53b");

            var subject = new OfbBlockCipher(new AesEngine());

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                iv,
                key,
                ct
            );
            var result = subject.ProcessPayload(param);

            Assert.AreEqual(pt.ToHex(), result.Result.ToHex());
        }
    }
}
