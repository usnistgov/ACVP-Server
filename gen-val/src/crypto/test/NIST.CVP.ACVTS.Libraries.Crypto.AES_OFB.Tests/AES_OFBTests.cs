using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_OFB.Tests
{
    [TestFixture, FastCryptoTest]
    public class AES_OFBTests
    {
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

            Assert.That(result.Result.ToHex(), Is.EqualTo(ct.ToHex()));
        }

        [Test]
        [TestCase(128, "0777ef9c046d80cefe0f2b5bedc3cc82", "1f88d353dd8f39eced080debba862a13", "a9adff5fec2ed8f30b7b2e7d0fce61a688c0b085c6b30a1e4c7120288211bf14aa1b5d4a15ecf2b6975b0983389c16262c650a9f065025fb28810b1e7e881bbf6c88fcb4049978707d6c8ecae02ec20005aeb8fbbec101d452e635982d264248d53e032420001529a5f7e6d9704a0e3afae2e32158cf47b0b2e8c51ef7b2ddb6", "6161513687577c3b284778adbaafdf15372868f4651211d0f8733bded466a37662e9836318a5d9856ffddc8d99e7243882bd6f9e2ec5284c378d045b163ad65ed6b1c01276a8f94ca028dc3777d116955d788fba40518cbf63e98e96d30c8ff3f1828db4a8a7f1bce5c3885eea082ac6a90baf9c3bb8311dc04e19a4a790c53b")]
        [TestCase(3, "3F381F7769E59142FCCAC5455CA84611", "D8DD576BC0B59558F2D3BFE254FECD84", "80", "C0")]
        [TestCase(30, "01120E66ECBF4FDC3668582D5CCBFF79", "21EDE718B00047D69C0656C27BC49574", "97AF00E8", "7CF149DC")]
        [TestCase(80, "996AAA67A9772FE2590D28F781FC9593", "E3CC96E37F1C121DA097C8377F413998", "708B913D0F75602C7448", "DE181A1CD1EEEB37C5A2")]
        public void ShouldEncryptPartialBlockCorrectlyNewEngine(int len, string keyHex, string ivHex, string ptHex, string ctHex)
        {
            var key = new BitString(keyHex);
            var iv = new BitString(ivHex);
            var pt = new BitString(ptHex, len);
            var ct = new BitString(ctHex, len);

            var subject = new OfbBlockCipher(new AesEngine());

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                pt
            );
            var result = subject.ProcessPayload(param);

            Assert.That(result.Result.ToHex(), Is.EqualTo(ct.ToHex()));
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

            Assert.That(result.Result.ToHex(), Is.EqualTo(pt.ToHex()));
        }
    }
}
