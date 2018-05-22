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

        [Test]
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

        [Test]
        public void SpecificTestNewEngineEncrypt()
        {
            var subject = new CbcBlockCipher(new AesEngine());

            var key = new BitString("70912e89521a485ac35a10ad06187eac");
            var iv = new BitString("603336e51e55003d3bcc69d050616bc9");
            var pt = new BitString("09424ff1ed3fb8ad593f82619a7e1794bbddc1fc1013ae78c3d3f34632215100ba95d395d6ecea98c3013efdf6194cb71680fff50c8882c14bd21f6be5e380c61e51c89ecce34a540a2c474ad17de2a6285fc677917777d93b60c466bab4dd26b2241d033ae3488d803e17db4963ec94fdd88ef84056a19f6647ca1bd08f3c2f92ea193b116db92c1ec3b3b0fe9a70b7ee15b0c88f902f44a9770c9720019b53");
            var ct = new BitString("37771cf060b9623c5e5410f140edcb72ec9ed152b73f2fbb33e4341048a625d187a732bfba50a941c0dd6a07981c9bedbb719c22af1e3d89d9c12f5f931eb91f7e1faca21919998b935141e2debf4dff051240aabff4a980448e9d995f4f6467c5e973e1a59d9ae180b97fcb3b5f4b707387a3321047da681d536504d7c08f73030d6ec48563150d2f1c0b0ad783fc08acdaaf01caa238090b166fe820f9a009");

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
        public void SpecificTestNewEngineDecrypt()
        {
            var subject = new CbcBlockCipher(new AesEngine());

            var key = new BitString("70912e89521a485ac35a10ad06187eac");
            var iv = new BitString("603336e51e55003d3bcc69d050616bc9");
            var pt = new BitString("09424ff1ed3fb8ad593f82619a7e1794bbddc1fc1013ae78c3d3f34632215100ba95d395d6ecea98c3013efdf6194cb71680fff50c8882c14bd21f6be5e380c61e51c89ecce34a540a2c474ad17de2a6285fc677917777d93b60c466bab4dd26b2241d033ae3488d803e17db4963ec94fdd88ef84056a19f6647ca1bd08f3c2f92ea193b116db92c1ec3b3b0fe9a70b7ee15b0c88f902f44a9770c9720019b53");
            var ct = new BitString("37771cf060b9623c5e5410f140edcb72ec9ed152b73f2fbb33e4341048a625d187a732bfba50a941c0dd6a07981c9bedbb719c22af1e3d89d9c12f5f931eb91f7e1faca21919998b935141e2debf4dff051240aabff4a980448e9d995f4f6467c5e973e1a59d9ae180b97fcb3b5f4b707387a3321047da681d536504d7c08f73030d6ec48563150d2f1c0b0ad783fc08acdaaf01caa238090b166fe820f9a009");

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
