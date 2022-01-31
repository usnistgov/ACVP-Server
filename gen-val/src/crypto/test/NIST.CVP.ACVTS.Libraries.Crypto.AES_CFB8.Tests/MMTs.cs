using NIST.CVP.ACVTS.Libraries.Crypto.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.ShiftRegister;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_CFB8.Tests
{
    [TestFixture, FastCryptoTest]
    public class MMTs
    {
        private CfbBlockCipher _subject;

        [OneTimeSetUp]
        public void Setup()
        {
            var engine = new AesEngine();
            _subject = new CfbBlockCipher(engine, new ShiftRegisterStrategyByte(engine));
        }

        #region Encrypt 
        [Test]
        public void ShouldCorrectlyMMTEncrypt128()
        {
            BitString key = new BitString("811747ce1fea043a39c342625ec52b1a");
            BitString iv = new BitString("e09c8d3be53be9bdd63f99b70f630149");
            BitString plainText = new BitString("eb0a89a9e21abb266eaf");
            BitString expectedCipherText = new BitString("751c5e72da27a3a616f4");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                plainText
            );
            var result = _subject.ProcessPayload(param);

            Assert.AreEqual(expectedCipherText, result.Result);
        }

        [Test]
        public void ShouldCorrectlyMMTEncrypt192()
        {
            BitString key = new BitString("0a3d6df4cbedc20898ab590e64ede2c6810d5ffd14ee3356");
            BitString iv = new BitString("f37bbc1994a365fe86231f65d5eaeadb");
            BitString plainText = new BitString("7ef7b0c9a0e23b8690e1");
            BitString expectedCipherText = new BitString("12d5726fd26bb1ec01a9");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                plainText
            );
            var result = _subject.ProcessPayload(param);

            Assert.AreEqual(expectedCipherText, result.Result);
        }

        [Test]
        public void ShouldCorrectlyMMTEncrypt256()
        {
            BitString key = new BitString("49d76c1b3621a091e32d7c39712ebafcf966e54114970603a1bb8026c2f4390b");
            BitString iv = new BitString("91a58f90be53223fbe70bae9a44db291");
            BitString plainText = new BitString("957b955599316d1d919c");
            BitString expectedCipherText = new BitString("f991b412bf221b6e878c");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                plainText
            );
            var result = _subject.ProcessPayload(param);

            Assert.AreEqual(expectedCipherText, result.Result);
        }
        #endregion Encrypt 

        #region Decrypt 
        [Test]
        public void ShouldCorrectlyMMTDecrypt128()
        {
            BitString key = new BitString("81d78365438bbb00e7807546f6ee99d1");
            BitString iv = new BitString("d27d153413f24ffba2db18589ee6319c");
            BitString cipherText = new BitString("643644bbe279795c7c73");
            BitString expectedPlainText = new BitString("4f34dba6219cc94d86a8");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                iv,
                key,
                cipherText
            );
            var result = _subject.ProcessPayload(param);

            Assert.AreEqual(expectedPlainText, result.Result);
        }

        [Test]
        public void ShouldCorrectlyMMTDecrypt192()
        {
            BitString key = new BitString("f28f1db3de8933a89fa6fd29c1918ad9eba6d59711569b00");
            BitString iv = new BitString("f406c129d611cdbe32eca63117c1e199");
            BitString cipherText = new BitString("2a5169adacf924a3922b");
            BitString expectedPlainText = new BitString("d1385049b0ed2be53528");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                iv,
                key,
                cipherText
            );
            var result = _subject.ProcessPayload(param);

            Assert.AreEqual(expectedPlainText, result.Result);
        }

        [Test]
        public void ShouldCorrectlyMMTDecrypt256()
        {
            BitString key = new BitString("9c45cf50619e071ed5e97238b8f575fe335328bbbeb05a904162d5ae7940e2c8");
            BitString iv = new BitString("2f53d242194fea139f17c91209dfa13d");
            BitString cipherText = new BitString("b2520a85db76ffc6fe9d");
            BitString expectedPlainText = new BitString("2747e1ab03aae6fcc9c6");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                iv,
                key,
                cipherText
            );
            var result = _subject.ProcessPayload(param);

            Assert.AreEqual(expectedPlainText, result.Result);
        }
        #endregion Decrypt 
    }
}
