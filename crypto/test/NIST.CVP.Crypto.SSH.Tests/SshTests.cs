using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SHAWrapper.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.SSH.Tests
{
    [TestFixture, FastCryptoTest]
    public class SshTests
    {
        [Test]
        [TestCase("sha-1", 64, 192,
            "0000010100d22d2d8fe769165c538910f772ce1beafa6b688a1089700febcbc48d0516dd08faf16805759625414e0893f1affae484a6e8c90240f054020bbc092e3e52367316a3b6a52c483dbeff9057820d94a75316e04351e3073574e49973c19d74e02d8c27b3e55ceb6ce6c322ddf332aedba8cd0a8f461b6c17478cedfd35e34eb7e76a03b243cb04494f2bfcfcb42803a4ad305e972b80a5cfc9f723123c4125e463c3135c509d1b880410c722f0d2cb3df0047d6e01410b2f17365bd3a9e68e25333acc1e71cfd7b7431a7a73578e6003180285d156e834427fc2be4648e28c9980db9656812ea267e8ef543c7d2c9e0c34349115c8dea2d1c20c6f16a31e233fca",
            "9edafcbdea26489509a5c023f645d957e9e34cc9",
            "9edafcbdea26489509a5c023f645d957e9e34cc9",
            "39f823d42e79e6d2",
            "3bf8570ee3223fd3",
            "4715bd0625fe68a23af753895c01294de3cbed40de450bd9",
            "08626087476c26e86753eb036d605774fe11a4b970ee7bd7",
            "807eead13926da4ab3ce924706107e6cee1b8ca1",
            "9f92451a02f09212bd96cb2ab4f381377816e141",
            TestName = "Ssh - SHA-1"
            )]
        [TestCase("sha2-256", 128, 192,
            "0000010100b9829061a7dcc3246eb2b51ff08afcc5a5d505f096173c4367f1a540b57c1cd2a686e5db0bd4a665467fb3ea976aec228f70733030b0ed46d4e064033e6980be754195e7d7bfca1d97ff5fd2df6955b376e59d83b4a49879cd19104a7618df19b1190670bb5e08ab5447ef2aed0a0ba7ad00f7ad47603c16e645cb4d407ec90e20135f91a72faceae3c45952b5bc50ff6ddd5d2defe8cc4e9dcbfeaf2a3a7f671fd558c021f280c54517f2caebfd68490199ac83baaa6078502bf5448ea6f61870758b43c95a86e01cb86ad1885d34020194742a83485fd52eae7d10b265bf1e08e99aea733425f525e57525af367de6e239ac40cdf91085e8764278324a689c",
            "f68c0e0c23adaeecbc3264a8bec365ca144a15ca3caec535a4d48d759b501462",
            "f68c0e0c23adaeecbc3264a8bec365ca144a15ca3caec535a4d48d759b501462",
            "c5ce936965f601708be8b5659c616240",
            "8e7a5c9d00863850884f8c1ee414dcc8",
            "3ae770bbcf66ba5494c56f5de16d65b4e4c79c3d6a950250",
            "91e145cf7e381d56bf455ad2e42bf9be0fe637cf3377f4a5",
            "da14d007eb63a1e9936a0be9e1b5c5d5959765549885785ea812e4eb3cec5d71",
            "97eb9b124cd8796e5669df067c5eafabe231c8c53f4831b662390ed39508a6bc",
            TestName = "Ssh - SHA2-256"
        )]
        [TestCase("sha2-512", 128, 256,
            "000001004c52670cc6d4bd01dfd18e9ca87defb1cb03f5bc06cd949c5024fcb7ea6f78d947c33151d03e98bd3309ca9a4142ade8e4eab78c23bd39edee33e5d37a0a44f237f409e0a7fb44eab28df0cd97a7c0832251a395b6155176422975c29aedd2b2f57e977e5a9ecff5a00b00d54eff24f598bcdadee6b14a75ac50a2d39f9642af6c5c959ed38d0677612c35cf2d58c3a718b4c057100065ff0ec7174c36ccd9f391821373279b55ed6b52dd20c6d74f8dbe4b72a0e07b74ee8b73fb070d9d3fc5813aac28caa4e1f061fa76a29d7ddba62f9e184cfc21ebd1ac052c46fa7749d4cd004c434ea954986457d3ef6cb2584f76ffaeac8d3790567ab0ef36e7f2b334",
            "79850cb981fd21862dfd5318dfea94f7e409e28b04457fe6bc936fd4788f9a2e84f36f300aa45826ceb91935b6e9b69beb595310c3d6a7465fdc9af876e101c6",
            "79850cb981fd21862dfd5318dfea94f7e409e28b04457fe6bc936fd4788f9a2e84f36f300aa45826ceb91935b6e9b69beb595310c3d6a7465fdc9af876e101c6",
            "3426c90364d275e209d259c7b0c9282b",
            "c6e7a67d04991c481417d4db1a6e1ebb",
            "b43bf36b875b985282653364bbcfc97bd9e6af92069bfebbbedc8c88398db3e2",
            "ad52ec2b6579814f0cb8fbaca8fae97a7acabd02c2ad09cde2d064b62b6ffddb",
            "a720f573d05b0591443d553404cf0e7e6ce0320f536c48e4170ef781802bafaebc73faafba577f0d2c035746bed5a3841cef72e92dc781084eb7748bb8569a06",
            "c36fa823d58629499e13430cd1f823d61df210d43de6a8d8755c4e69da72d574f97df9b38b2d2652467182720fcabb59ed900b4a95dfe9c1516d9ee5d07a2c3a",
            TestName = "Ssh - SHA2-512"
        )]
        public void ShouldSshCorrectly(string hashName, int ivLength, int keyLength, string kHex, string hHex, string sessionIdHex, string ivCS, string ivSC, string keyCS, string keySC, string iKeyCS, string iKeySC)
        {
            var hash = ShaAttributes.GetHashFunctionFromName(hashName);
            var sha = new ShaFactory().GetShaInstance(hash);
            var k = new BitString(kHex);
            var h = new BitString(hHex);
            var sessionId = new BitString(sessionIdHex);

            var subject = new Ssh(sha, ivLength, keyLength);
            var result = subject.DeriveKey(k, h, sessionId);

            Assert.AreEqual(ivCS, result.ClientToServer.InitialIv.ToHex().ToLower(), "C to S, Initial IV");
            Assert.AreEqual(keyCS, result.ClientToServer.EncryptionKey.ToHex().ToLower(), "C to S, Encryption Key");
            Assert.AreEqual(iKeyCS, result.ClientToServer.IntegrityKey.ToHex().ToLower(), "C to S, Integrity Key");

            Assert.AreEqual(ivSC, result.ServerToClient.InitialIv.ToHex().ToLower(), "S to C, Initial IV");
            Assert.AreEqual(keySC, result.ServerToClient.EncryptionKey.ToHex().ToLower(), "S to C, Encryption Key");
            Assert.AreEqual(iKeySC, result.ServerToClient.IntegrityKey.ToHex().ToLower(), "S to C, Integrity Key");
        }
    }
}
