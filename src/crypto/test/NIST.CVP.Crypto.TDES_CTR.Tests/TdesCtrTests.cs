using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NUnit.Framework;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Crypto.TDES_CTR.Tests
{
    [TestFixture, FastCryptoTest]
    public class TdesCtrTests
    {
        private Mock<ICounter> _mockCounter;
        private CtrBlockCipher _newSubject;

        [SetUp]
        public void Setup()
        {
            _mockCounter = new Mock<ICounter>();
            _newSubject = new CtrBlockCipher(new TdesEngine(), _mockCounter.Object);
        }

        [Test]
        [TestCase("0101010101010101",
            "8000000000000000",
            "0000000000000000",
            "95f8a5e5dd31d900",
            TestName = "TDES_CTR - Encrypt NewEngine")]
        public void ShouldEncryptCorrectlyNewEngine(string keyHex, string ivHex, string ptHex, string ctHex)
        {
            var key = new BitString(keyHex);
            var iv = new BitString(ivHex);
            var pt = new BitString(ptHex);
            var ct = new BitString(ctHex);

            _mockCounter.Setup(s => s.GetNextIV()).Returns(iv);

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                key,
                pt
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(ct, result.Result);
        }

        [Test]
        [TestCase("0101010101010101",
            "8000000000000000",
            "000000000000",
            "95f8a5e5dd20",
            43,
            TestName = "TDES_CTR - Encrypt Partial NewEngine")]
        public void ShouldEncryptPartialBlockCorrectlyNewEngine(string keyHex, string ivHex, string ptHex, string ctHex,
            int length)
        {
            var key = new BitString(keyHex);
            var iv = new BitString(ivHex);
            var pt = new BitString(ptHex, length);
            var ct = new BitString(ctHex, length);

            _mockCounter.Setup(s => s.GetNextIV()).Returns(iv);

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                key,
                pt
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(ct, result.Result);
        }

        [Test]
        [TestCase("0101010101010101",
            "8000000000000000",
            "95f8a5e5dd31d900",
            "0000000000000000",
            TestName = "TDES_CTR - Decrypt NewEngine")]
        public void ShouldDecryptCorrectlyNewEngine(string keyHex, string ivHex, string ctHex, string ptHex)
        {
            var key = new BitString(keyHex);
            var iv = new BitString(ivHex);
            var ct = new BitString(ctHex);
            var pt = new BitString(ptHex);

            _mockCounter.Setup(s => s.GetNextIV()).Returns(iv);

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                key,
                ct
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(pt, result.Result);
        }
    }
}
