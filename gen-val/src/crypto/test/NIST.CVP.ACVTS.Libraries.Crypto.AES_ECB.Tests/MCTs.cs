using NIST.CVP.ACVTS.Libraries.Crypto.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_ECB.Tests
{
    [TestFixture, LongCryptoTest]
    public class MCTs
    {
        private readonly MonteCarloAesEcb _subject =
                new MonteCarloAesEcb(
                    new BlockCipherEngineFactory(),
                    new ModeBlockCipherFactory(),
                    new AesMonteCarloKeyMaker())
            ;

        #region Encrypt
        [Test]
        public void ShouldMonteCarloTestEncrypt128BitKey()
        {
            BitString key = new BitString("71cdc0006a5de45e31a56ddab56e5595");
            BitString plainText = new BitString("0333cf639a8e98b4e5383d21c659d0c7");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                null,
                key,
                plainText);

            var result = _subject.ProcessMonteCarloTest(param);

            var firstExpectedCipherText = new BitString("cec42be01aa7918cd7d563407324bcbb");
            var lastExpectedCipherText = new BitString("9da00f60c0724427108eff09f2888d7f");

            var firstCipherText = result.Response[0].CipherText.ToHex();
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText.ToHex();

            Assert.That(firstCipherText, Is.EqualTo(firstExpectedCipherText.ToHex()));
            Assert.That(lastCipherText, Is.EqualTo(lastExpectedCipherText.ToHex()));

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ShouldMonteCarloTestEncrypt192BitKey()
        {
            BitString key = new BitString("748789fca94484c182898394d660bee136acdcc02a5d5cc7");
            BitString plainText = new BitString("da50abcf79301a9c0104967ca87d2137");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                null,
                key,
                plainText);

            var result = _subject.ProcessMonteCarloTest(param);

            var firstExpectedCipherText = new BitString("c0bbf4e6725c482a23941294950f0b6a");
            var lastExpectedCipherText = new BitString("aa08459ffe87dd0ce15d29a669b5d02f");

            var firstCipherText = result.Response[0].CipherText.ToHex();
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText.ToHex();

            Assert.That(firstCipherText, Is.EqualTo(firstExpectedCipherText.ToHex()), "first");
            Assert.That(lastCipherText, Is.EqualTo(lastExpectedCipherText.ToHex()), "last");

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ShouldMonteCarloTestEncrypt256BitKey()
        {
            BitString key = new BitString("a1b76968592787f921a4d7f899172b3a5697ca3eec31200d4ce9b36451fc5915");
            BitString plainText = new BitString("9d9cb8cb51ae38627c9780cac96825d0");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                null,
                key,
                plainText);

            var result = _subject.ProcessMonteCarloTest(param);

            var firstExpectedCipherText = new BitString("33a0232d1581f1a170309ce46c3fb389");
            var lastExpectedCipherText = new BitString("73e73585774d4245cc91a741dc7134c9");

            var firstCipherText = result.Response[0].CipherText.ToHex();
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText.ToHex();

            Assert.That(firstCipherText, Is.EqualTo(firstExpectedCipherText.ToHex()), "first");
            Assert.That(lastCipherText, Is.EqualTo(lastExpectedCipherText.ToHex()), "last");

            Assert.That(result.Success, Is.True);
        }
        #endregion Encrypt

        #region Decrypt
        [Test]
        public void ShouldMonteCarloTestDecrypt128BitKey()
        {
            BitString key = new BitString("da9c44768fa8ddb20aeb367d64ec4c34");
            BitString cipherText = new BitString("ac057c3f729274bb0ecdda820b223d79");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                null,
                key,
                cipherText);

            var result = _subject.ProcessMonteCarloTest(param);
            var firstExpectedPlainText = new BitString("38b9f64a9845c248c4c170dfcf038a40");
            var lastExpectedPlainText = new BitString("0c0b9727509545b6df928e449a9c0cbf");

            var firstPlaintText = result.Response[0].PlainText.ToHex();
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText.ToHex();

            Assert.That(firstPlaintText, Is.EqualTo(firstExpectedPlainText.ToHex()));
            Assert.That(lastPlainText, Is.EqualTo(lastExpectedPlainText.ToHex()));

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ShouldMonteCarloTestDecrypt192BitKey()
        {
            BitString key = new BitString("f1b7f4fc55e8890c038dcce9bcb2af8b29ac3fa3cc4333da");
            BitString cipherText = new BitString("69f2245ac801b9956774b3f0bf6ff8c6");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                null,
                key,
                cipherText);

            var result = _subject.ProcessMonteCarloTest(param);

            var firstExpectedPlainText = new BitString("09725a1d53dc761c0b9ac47f0e9855e2");
            var lastExpectedPlainText = new BitString("2fc61701960daec738425a8595df21e6");

            var firstPlaintText = result.Response[0].PlainText.ToHex();
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText.ToHex();

            Assert.That(firstPlaintText, Is.EqualTo(firstExpectedPlainText.ToHex()));
            Assert.That(lastPlainText, Is.EqualTo(lastExpectedPlainText.ToHex()));

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ShouldMonteCarloTestDecrypt256BitKey()
        {
            BitString key = new BitString("e60840ea6635f07a1c2624092b03b422c7bb25453ff6cd7e3a4c383c40b4cb61");
            BitString cipherText = new BitString("01b74579771b03c15987d06d5da33f13");

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                null,
                key,
                cipherText);

            var result = _subject.ProcessMonteCarloTest(param);

            var firstExpectedPlainText = new BitString("c70e80755dba4e5d1d1d53b4af6e6441");
            var lastExpectedPlainText = new BitString("c06d3221d10622bda50ca9bb2ca431cc");

            var firstPlaintText = result.Response[0].PlainText.ToHex();
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText.ToHex();

            Assert.That(firstPlaintText, Is.EqualTo(firstExpectedPlainText.ToHex()));
            Assert.That(lastPlainText, Is.EqualTo(lastExpectedPlainText.ToHex()));

            Assert.That(result.Success, Is.True);
        }
        #endregion Decrypt
    }
}

