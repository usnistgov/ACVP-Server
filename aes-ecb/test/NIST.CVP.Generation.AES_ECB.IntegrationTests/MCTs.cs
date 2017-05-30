using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class MCTs
    {
        #region Encrypt
        [Test]
        public void ShouldMonteCarloTestEncrypt128BitKey()
        {
            AES_ECB_MCT subject = new AES_ECB_MCT(new Crypto.AES_ECB.AES_ECB(new RijndaelFactory(new RijndaelInternals())));

            BitString key = new BitString("71cdc0006a5de45e31a56ddab56e5595");
            BitString plainText = new BitString("0333cf639a8e98b4e5383d21c659d0c7");

            var result = subject.MCTEncrypt(key, plainText);

            var firstExpectedCipherText = new BitString("cec42be01aa7918cd7d563407324bcbb");
            var lastExpectedCipherText = new BitString("9da00f60c0724427108eff09f2888d7f");

            var firstCipherText = result.Response[0].CipherText.ToHex();
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText.ToHex();

            Assert.AreEqual(firstExpectedCipherText.ToHex(), firstCipherText);
            Assert.AreEqual(lastExpectedCipherText.ToHex(), lastCipherText);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldMonteCarloTestEncrypt192BitKey()
        {
            AES_ECB_MCT subject = new AES_ECB_MCT(new Crypto.AES_ECB.AES_ECB(new RijndaelFactory(new RijndaelInternals())));

            BitString key = new BitString("ebbc71ca8573fb5b81ab065a63ac4ee5da381a749510b675");
            BitString plainText = new BitString("2f12359490fe7ed3c7c2429e63383dcf");

            var result = subject.MCTEncrypt(key, plainText);

            var firstExpectedCipherText = new BitString("cb79bfeac7f53bad2e285b2ddff297d3");
            var lastExpectedCipherText = new BitString("ef2eeeacc2bf255d617a27498ace2213");

            var firstCipherText = result.Response[0].CipherText.ToHex();
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText.ToHex();

            Assert.AreEqual(firstExpectedCipherText.ToHex(), firstCipherText);
            Assert.AreEqual(lastExpectedCipherText.ToHex(), lastCipherText);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldMonteCarloTestEncrypt256BitKey()
        {
            AES_ECB_MCT subject = new AES_ECB_MCT(new Crypto.AES_ECB.AES_ECB(new RijndaelFactory(new RijndaelInternals())));

            BitString key = new BitString("a1b76968592787f921a4d7f899172b3a5697ca3eec31200d4ce9b36451fc5915");
            BitString plainText = new BitString("9d9cb8cb51ae38627c9780cac96825d0");

            var result = subject.MCTEncrypt(key, plainText);

            var firstExpectedCipherText = new BitString("33a0232d1581f1a170309ce46c3fb389");
            var lastExpectedCipherText = new BitString("73e73585774d4245cc91a741dc7134c9");

            var firstCipherText = result.Response[0].CipherText.ToHex();
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText.ToHex();

            Assert.AreEqual(firstExpectedCipherText.ToHex(), firstCipherText);
            Assert.AreEqual(lastExpectedCipherText.ToHex(), lastCipherText);

            Assert.IsTrue(result.Success);
        }
        #endregion Encrypt

        #region Decrypt
        [Test]
        public void ShouldMonteCarloTestDecrypt128BitKey()
        {
            AES_ECB_MCT subject = new AES_ECB_MCT(new Crypto.AES_ECB.AES_ECB(new RijndaelFactory(new RijndaelInternals())));

            BitString key = new BitString("da9c44768fa8ddb20aeb367d64ec4c34");
            BitString cipherText = new BitString("ac057c3f729274bb0ecdda820b223d79");

            var result = subject.MCTDecrypt(key, cipherText);

            var firstExpectedPlainText = new BitString("38b9f64a9845c248c4c170dfcf038a40");
            var lastExpectedPlainText = new BitString("0c0b9727509545b6df928e449a9c0cbf");

            var firstPlaintText = result.Response[0].PlainText.ToHex();
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText.ToHex();

            Assert.AreEqual(firstExpectedPlainText.ToHex(), firstPlaintText);
            Assert.AreEqual(lastExpectedPlainText.ToHex(), lastPlainText);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldMonteCarloTestDecrypt192BitKey()
        {
            AES_ECB_MCT subject = new AES_ECB_MCT(new Crypto.AES_ECB.AES_ECB(new RijndaelFactory(new RijndaelInternals())));

            BitString key = new BitString("f1b7f4fc55e8890c038dcce9bcb2af8b29ac3fa3cc4333da");
            BitString cipherText = new BitString("69f2245ac801b9956774b3f0bf6ff8c6");

            var result = subject.MCTDecrypt(key, cipherText);

            var firstExpectedPlainText = new BitString("09725a1d53dc761c0b9ac47f0e9855e2");
            var lastExpectedPlainText = new BitString("2fc61701960daec738425a8595df21e6");

            var firstPlaintText = result.Response[0].PlainText.ToHex();
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText.ToHex();

            Assert.AreEqual(firstExpectedPlainText.ToHex(), firstPlaintText);
            Assert.AreEqual(lastExpectedPlainText.ToHex(), lastPlainText);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldMonteCarloTestDecrypt256BitKey()
        {
            AES_ECB_MCT subject = new AES_ECB_MCT(new Crypto.AES_ECB.AES_ECB(new RijndaelFactory(new RijndaelInternals())));

            BitString key = new BitString("e60840ea6635f07a1c2624092b03b422c7bb25453ff6cd7e3a4c383c40b4cb61");
            BitString cipherText = new BitString("01b74579771b03c15987d06d5da33f13");

            var result = subject.MCTDecrypt(key, cipherText);

            var firstExpectedPlainText = new BitString("c70e80755dba4e5d1d1d53b4af6e6441");
            var lastExpectedPlainText = new BitString("c06d3221d10622bda50ca9bb2ca431cc");

            var firstPlaintText = result.Response[0].PlainText.ToHex();
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText.ToHex();

            Assert.AreEqual(firstExpectedPlainText.ToHex(), firstPlaintText);
            Assert.AreEqual(lastExpectedPlainText.ToHex(), lastPlainText);

            Assert.IsTrue(result.Success);
        }
        #endregion Decrypt
    }
}

