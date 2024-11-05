using NIST.CVP.ACVTS.Libraries.Crypto.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_CBC.Tests
{
    [TestFixture, LongCryptoTest]
    public class MCTs
    {
        private readonly MonteCarloAesCbc _subject =
            new MonteCarloAesCbc(
                new BlockCipherEngineFactory(),
                new ModeBlockCipherFactory(),
                new AesMonteCarloKeyMaker()
            );

        #region Encrypt
        [Test]
        public void ShouldMonteCarloTestEncrypt128BitKey()
        {
            BitString key = new BitString("894b05c8d80eeeb74e2483b26dd87202");
            BitString iv = new BitString("598ec60a28fd252762da2792067cd8fe");
            BitString plainText = new BitString("9a57d7620be2df90f1218a9029103954");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                plainText
            );
            var result = _subject.ProcessMonteCarloTest(param);
            var firstExpectedCipherText = new BitString("7cd54955fbb72f7da46775f4abbc71d0");
            var lastExpectedCipherText = new BitString("a549ce4c52dbbbb9be407f1be1f4a61a");

            var firstCipherText = result.Response[0].CipherText.ToHex();
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText.ToHex();

            Assert.That(firstCipherText, Is.EqualTo(firstExpectedCipherText.ToHex()), nameof(firstCipherText));
            Assert.That(lastCipherText, Is.EqualTo(lastExpectedCipherText.ToHex()), nameof(lastCipherText));

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ShouldMonteCarloTestEncrypt192BitKey()
        {
            BitString key = new BitString("4d647b53e1a7f64fe00cde9362df9febd071905dec101db8");
            BitString iv = new BitString("ba5b5ecf7a33ced8957e56457d789158");
            BitString plainText = new BitString("62662f85e743969bfd317830d3371395");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                plainText
            );
            var result = _subject.ProcessMonteCarloTest(param);

            var firstExpectedCipherText = new BitString("a87a94f4791bef0a345ebe92d935531e");
            var lastExpectedCipherText = new BitString("7cec0fea1800b246db28832620f7d5ad");

            var firstCipherText = result.Response[0].CipherText.ToHex();
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText.ToHex();

            Assert.That(firstCipherText, Is.EqualTo(firstExpectedCipherText.ToHex()), nameof(firstCipherText));
            Assert.That(lastCipherText, Is.EqualTo(lastExpectedCipherText.ToHex()), nameof(lastCipherText));

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ShouldMonteCarloTestEncrypt256BitKey()
        {
            BitString key = new BitString("99be5720f51d234530a9fe6ec015a5fbd7da795588cfc8232bf94ec102a4fced");
            BitString iv = new BitString("7981e5f1101f242eb55e17807a1cf6d4");
            BitString plainText = new BitString("9ac7e60acaf55dd9fbc31491789bdb94");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                plainText
            );
            var result = _subject.ProcessMonteCarloTest(param);

            var firstExpectedCipherText = new BitString("17de51ae26223e52f85e898916ea6e60");
            var lastExpectedCipherText = new BitString("35991f5535b05a332ed66ec17f4dd9e6");

            var firstCipherText = result.Response[0].CipherText.ToHex();
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText.ToHex();

            Assert.That(firstCipherText, Is.EqualTo(firstExpectedCipherText.ToHex()), nameof(firstCipherText));
            Assert.That(lastCipherText, Is.EqualTo(lastExpectedCipherText.ToHex()), nameof(lastCipherText));

            Assert.That(result.Success, Is.True);
        }
        #endregion Encrypt

        #region Decrypt
        [Test]
        public void ShouldMonteCarloTestDecrypt128BitKey()
        {
            BitString key = new BitString("40212e6752be1b903e0185002eb7128c");
            BitString iv = new BitString("c1bd1dede054c4fede03f3f6a5e4f547");
            BitString cipherText = new BitString("945860fcffa51451ed55d8ca6d4ad67d");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                iv,
                key,
                cipherText
            );
            var result = _subject.ProcessMonteCarloTest(param);

            var firstExpectedPlainText = new BitString("e0183c620a21991788afaa3fb915de9d");
            var lastExpectedPlainText = new BitString("0d097f9b5641f5264dfb1143f15fe10f");

            var firstPlaintText = result.Response[0].PlainText.ToHex();
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText.ToHex();

            Assert.That(firstPlaintText, Is.EqualTo(firstExpectedPlainText.ToHex()), nameof(firstExpectedPlainText));
            Assert.That(lastPlainText, Is.EqualTo(lastExpectedPlainText.ToHex()), nameof(lastPlainText));

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ShouldMonteCarloTestDecrypt192BitKey()
        {
            BitString key = new BitString("3832711e0dcb9fe4073d6a68129536c5ee64e3b482c2b7ae");
            BitString iv = new BitString("a4493c2caf1e1bb3ed2be2de37364851");
            BitString cipherText = new BitString("33b27eb00d0b07a0dc0132332461af2a");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                iv,
                key,
                cipherText
            );
            var result = _subject.ProcessMonteCarloTest(param);

            var firstExpectedPlainText = new BitString("d09df30f71e25d9b9611152407d9f1aa");
            var lastExpectedPlainText = new BitString("6f616ceec053d3cfb029d3d3f7ececb9");

            var firstPlaintText = result.Response[0].PlainText.ToHex();
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText.ToHex();

            Assert.That(firstPlaintText, Is.EqualTo(firstExpectedPlainText.ToHex()), nameof(firstExpectedPlainText));
            Assert.That(lastPlainText, Is.EqualTo(lastExpectedPlainText.ToHex()), nameof(lastPlainText));

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ShouldMonteCarloTestDecrypt256BitKey()
        {
            BitString key = new BitString("c6d1ef08d4da7baf3dc77a9bfe307836ade554f4ba24a12bf60f4d26515c8075");
            BitString iv = new BitString("e0c59e6838ae504fd03fcc885e2897d3");
            BitString cipherText = new BitString("7577a2ba16c466b2a7e9b1df32538e36");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                iv,
                key,
                cipherText
            );
            var result = _subject.ProcessMonteCarloTest(param);

            var firstExpectedPlainText = new BitString("10024966f0e39a07473cd7ec85adad73");
            var lastExpectedPlainText = new BitString("8b175090b4d6cd40e4e1ab04a7fc0098");

            var firstPlaintText = result.Response[0].PlainText.ToHex();
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText.ToHex();

            Assert.That(firstPlaintText, Is.EqualTo(firstExpectedPlainText.ToHex()), nameof(firstExpectedPlainText));
            Assert.That(lastPlainText, Is.EqualTo(lastExpectedPlainText.ToHex()), nameof(lastPlainText));

            Assert.That(result.Success, Is.True);
        }
        #endregion Decrypt
    }
}

