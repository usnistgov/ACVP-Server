using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TDES_CBC.Tests
{
    [TestFixture, FastCryptoTest]
    public class TdesCbcTests
    {
        private CbcBlockCipher _newSubject;

        [OneTimeSetUp]
        public void Setup()
        {
            var engine = new TdesEngine();
            _newSubject = new CbcBlockCipher(engine);
        }

        [Test]
        public void ShouldEncryptNewEngine()
        {
            var keyBytes = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef };
            var plainTextBytes = new byte[] { 0x4e, 0x6f, 0x77, 0x20, 0x69, 0x73, 0x20, 0x74 };
            var expectedBytes = new byte[] { 0x3f, 0xa4, 0x0e, 0x8a, 0x98, 0x4d, 0x48, 0x15 };
            var iv = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                new BitString(iv),
                new BitString(keyBytes),
                new BitString(plainTextBytes)
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.IsTrue((bool)result.Success);
            var actual = result.Result.ToBytes();
            Assert.AreEqual(expectedBytes, actual);
        }

        [Test]
        public void ShouldDecryptNewEngine()
        {
            var keyBytes = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef };
            var expectedBytes = new byte[] { 0x4e, 0x6f, 0x77, 0x20, 0x69, 0x73, 0x20, 0x74 };
            var CipherTextBytes = new byte[] { 0x3f, 0xa4, 0x0e, 0x8a, 0x98, 0x4d, 0x48, 0x15 };
            var iv = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                new BitString(iv),
                new BitString(keyBytes),
                new BitString(CipherTextBytes)
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.IsTrue((bool)result.Success);
            var actual = result.Result.ToBytes();
            Assert.AreEqual(expectedBytes, actual);
        }

        [Test]
        [TestCase("198f973b5462df64946245e0b6ec0d98a4e5136ba75294b3", "60dd7b8f00554bb6003161ba62fd66adc2757a40dbf5d6d84d30b0e49e255f6d7e4fc0ee4f1cd867e4a1bbad898cac04445a85ef5bca5a471691598bc64ff47706c243d84139a39e", "b141cecac08330b579ad8554dbaec7c1a7d411e4ab4697dbd78ee0bdb54271d20d64b3b0b0f0b451182e858012eb7bb5c94dfce73b42b803ded904534fbe3c52b65f8501ccbfe5f7")]
        [TestCase("c1da626de38389f11cc28fe0f8fdb623374ccb495d20c81f", "5fe074a3c30b281ec7db62b76ddbcb7d51c784242ffbc410a42ac2b03953d50d9df1d9a33273d66fcbebdc49b50a3174f44caf74ce70671f8e2b8af7821d8ab746047c2c4430c1467c37e56f81e9c71b", "d7f5edf35f207dfcc2004d7172b984ad37626519a3b9c51e85ed84f56a6b2856d9ba220380fadad19175ce56557fdbb7f8fa691ccc8232f24a4b4a034d9ce966ce1177118d078e27d8c4b622e3a05d86")]
        public void ShouldPassMultiBlockEncryptTestsNewEngine(string key, string plaintext, string cipherText)
        {
            var iv = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                new BitString(iv),
                new BitString(key),
                new BitString(plaintext)
            );
            var result = _newSubject.ProcessPayload(param);
            
            Assert.IsTrue((bool)result.Success);

            Assert.AreEqual(new BitString(cipherText).ToHex(), result.Result.ToHex());
        }

        [Test]
        [TestCase("198f973b5462df64946245e0b6ec0d98a4e5136ba75294b3", "60dd7b8f00554bb6003161ba62fd66adc2757a40dbf5d6d84d30b0e49e255f6d7e4fc0ee4f1cd867e4a1bbad898cac04445a85ef5bca5a471691598bc64ff47706c243d84139a39e", "b141cecac08330b579ad8554dbaec7c1a7d411e4ab4697dbd78ee0bdb54271d20d64b3b0b0f0b451182e858012eb7bb5c94dfce73b42b803ded904534fbe3c52b65f8501ccbfe5f7")]
        [TestCase("c1da626de38389f11cc28fe0f8fdb623374ccb495d20c81f", "5fe074a3c30b281ec7db62b76ddbcb7d51c784242ffbc410a42ac2b03953d50d9df1d9a33273d66fcbebdc49b50a3174f44caf74ce70671f8e2b8af7821d8ab746047c2c4430c1467c37e56f81e9c71b", "d7f5edf35f207dfcc2004d7172b984ad37626519a3b9c51e85ed84f56a6b2856d9ba220380fadad19175ce56557fdbb7f8fa691ccc8232f24a4b4a034d9ce966ce1177118d078e27d8c4b622e3a05d86")]
        public void ShouldPassMultiBlockDecryptTestsNewEngine(string key, string plaintext, string cipherText)
        {
            var iv = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                new BitString(iv),
                new BitString(key),
                new BitString(cipherText)
            );
            var result = _newSubject.ProcessPayload(param);

            Assert.IsTrue((bool)result.Success);

            Assert.AreEqual(new BitString(plaintext).ToHex(), result.Result.ToHex());
        }
    }
}
