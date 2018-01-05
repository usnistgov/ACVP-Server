using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.SRTP.Tests
{
    [TestFixture, FastCryptoTest]
    public class SrtpTests
    {
        [Test]
        [TestCase(128, "0e45d3a360c315dc4de56b04e2dc4014",
            "085e831797b928be78dbeacec798", "000000000000",
            "6cc743bca42b", "01f94827",
            "101a9cfac073fbefe0b39e4bfb5652cc", "5241bae6a88585b78cdf15db195d20c37a08b9e1", "d8ca5e8f0e8f7989ce7e304ffa24",
            "90b8d1315709590fd03d9cc016eaf82e", "d691cf44b8772ad3bff2cd381f9574c65582a738", "969672175fd4b0510d6b82e7804c",
            TestName = "SRTP - AES-128 - 1")]
        [TestCase(128, "c3e424d5cee134ec8fe155dc6d28673b",
            "49f8c0ff87171ad2645f154d4f9c", "000000400000",
            "e2011c088a5e", "1cca8296",
            "82fdb30a2e75c0edf5164929283a434e", "459e4b29be369f444995edc34efa76ad9ff2ec28", "f2fa26852da72aab695e097ef4d2",
            "19d06fefef50f4b663622715d7a8759b", "2c079fefd89cb7faddd8a108f37a8ab27e1f4685", "c61e777e9603642d3c71c369e133",
            TestName = "SRTP - AES-128 - 2")]
        [TestCase(192, "2f1c2da9bf7f5a9c2bd7b48bedd8f6e8b910c9b0308b31a3",
            "278ae2ee48688fc1ca82fb695722", "000000000004",
            "9894b0ac8d78", "6bce9e03",
            "db793fa3fae81a702132913c4ea98eccc9c5c5e2971523dc", "6edd83781c132cc8742621db8abb6b89231addf4", "73ab5936d6da5156053452dd4388",
            "2c4a240a04c5c02d3b34f3d9d82883094d6026779eb5a8ae", "5ced4c155e0fcece6a087276dc0d125491a10a36", "e2fbd3dcbde9f142f65fb789a12d",
            TestName = "SRTP - AES-192 - 1")]
        [TestCase(192, "1ee654d25de13f5241da7e376a2cbed1313c7124b318f282",
            "4dbf2eae440a28a92c18824a2979", "000000000020",
            "c6e166b9118c", "2fb09f46",
            "2416d96bfcb71f1cf7c5aa68372dac39b78919796bad74a9", "4c6dc4e6314cc29c38dd34eb44a022c8c9b75675", "e019b6efac5e05e20a1d906ef46d",
            "af132ee91030cfd003b3ac0b41535115b35171910d35bff9", "382b96afda2fa6973bfb7f9739be6a72d10e535b", "0c582ea6fc3887494982754dbd17",
            TestName = "SRTP - AES-192 - 2")]
        [TestCase(256, "48a16be787283461699a232601d1e106d7fff8af073f4a5fb0ced415c3b16282",
            "0da708de000471291a5bdf3c1544", "000000002000",
            "7d3aa668bc9b", "5ba17c64",
            "c443154aa9185f1a10b2d329f46cff82fc498925865ab61615c8bff24b7b7300", "e00f82265e9f02f307f63a549e363ff0db369600", "c1bc2f9f970ed0268b4a7d13a8fa",
            "e991d9e5ce1dec31fd9349628e28f6a2a8c4370debefb15ed0f1078c8f852494", "cfd474602ccc4a15a24f13e54b975ede3901c544", "259b08254b76cb143157f6f4fda5",
            TestName = "SRTP - AES-256 - 1")]
        [TestCase(256, "24aef47321e770596b3c1797515d6d4baa617a14176700d4154fc4ecd3cb2213",
            "64cd971683194cc9cb00ea9130cc", "000000000008",
            "e999bbd5394e", "695dd1a5",
            "425c3cda02b5922c9018be02989ccd43335921e0f5638c2167929cee21a7926e", "4061f129da36136367f994ed421a1e4119e865eb", "5fa2d84210c8008f5d3a492e5428",
            "2c2ed36272338dd3916843597862420a8f2aa888345051c677404fc5001bcc42", "77b8650f2a8170c870d86d1f4944905c3c413042", "59b3b38a556a1df6b262d127304c",
            TestName = "SRTP - AES-256 - 2")]
        public void ShouldSrtpCorrectly(int keyLength, string keyHex, string saltHex, string kdrHex, string indexHex, string srtcpIndexHex, string srtpKe, string srtpKa, string srtpKs, string srtcpKe, string srtcpKa, string srtcpKs)
        {
            var key = new BitString(keyHex);
            var salt = new BitString(saltHex);
            var kdr = new BitString(kdrHex);
            var index = new BitString(indexHex);
            var srtcpIndex = new BitString(srtcpIndexHex);

            var subject = new Srtp();
            var result = subject.DeriveKey(keyLength, key, salt, kdr, index, srtcpIndex);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(srtpKe, result.SrtpResult.EncryptionKey.ToHex().ToLower(), "srtp ke");
            Assert.AreEqual(srtpKa, result.SrtpResult.AuthenticationKey.ToHex().ToLower(), "srtp ka");
            Assert.AreEqual(srtpKs, result.SrtpResult.SaltingKey.ToHex().ToLower(), "srtp ks");
            Assert.AreEqual(srtcpKe, result.SrtcpResult.EncryptionKey.ToHex().ToLower(), "srtcp ke");
            Assert.AreEqual(srtcpKa, result.SrtcpResult.AuthenticationKey.ToHex().ToLower(), "srtcp ka");
            Assert.AreEqual(srtcpKs, result.SrtcpResult.SaltingKey.ToHex().ToLower(), "srtcp ks");

        }
    }
}
