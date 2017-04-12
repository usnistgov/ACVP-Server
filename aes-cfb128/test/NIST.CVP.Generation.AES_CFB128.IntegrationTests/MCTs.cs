using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CFB128;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB128.IntegrationTests
{
    [TestFixture]
    public class MCTs
    {
        AES_CFB128_MCT _subject = new AES_CFB128_MCT(
            new Crypto.AES_CFB128.AES_CFB128(
                new RijndaelFactory(
                    new RijndaelInternals()
                )
            )
        );

        #region Encrypt
        [Test]
        public void ShouldMonteCarloTestEncrypt128BitKey()
        {
            string keyString = "ef22e67279d033c22053886c67ebb15a";
            string ivString = "0dfb0fe904ea07f356e58470423e5f69";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var plainText = new BitString("f9f662a43a988a3f7116ead7c60886ef");
            var firstExpectedCipherText = new BitString("909e9a16af24281a1f4771ba360d8733");

            var secondExpectedKey = new BitString("7fbc7c64d6f41bd83f14f9d651e63669");
            var secondExpectedIv = new BitString("909e9a16af24281a1f4771ba360d8733");

            var lastExpectedKey = new BitString("01c78e54c67009f34e3a9cb740744111");
            var lastExpectedIv = new BitString("4e5ffce071752c3cb513e3a6f7347da5");
            var lastExpectedPlainText = new BitString("3bff30f019dbf7457c6c188d16e7b0e3");
            var lastExpectedCipherText = new BitString("c51817da5cb5a2ffb9f71b9cbbb0b087");


            var result = _subject.MCTEncrypt(iv, key, plainText);


            Assert.AreEqual(key, sanityCheckFirstKey, nameof(sanityCheckFirstKey));
            Assert.AreEqual(iv, sanityCheckFirstIv, nameof(sanityCheckFirstIv));
            var firstCipherText = result.Response[0].CipherText;
            Assert.AreEqual(firstExpectedCipherText, firstCipherText, nameof(firstCipherText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;

            Assert.AreEqual(secondExpectedKey, secondKey, nameof(secondExpectedKey));
            Assert.AreEqual(secondExpectedIv, secondIv, nameof(secondExpectedIv));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;

            Assert.AreEqual(lastExpectedKey, lastKey, nameof(lastExpectedKey));
            Assert.AreEqual(lastExpectedIv, lastIv, nameof(lastExpectedIv));
            Assert.AreEqual(lastExpectedPlainText, lastPlainText, nameof(lastPlainText));
            Assert.AreEqual(lastExpectedCipherText, lastCipherText, nameof(lastCipherText));

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldMonteCarloTestEncrypt192BitKey()
        {
            string keyString = "4c0231647a59c70f6ddf5a2ae3683c4c646dd3d4265f9cda";
            string ivString = "f53e467c1ddd6567c2fc19541fc0852a";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var plainText = new BitString("7fd187154d2c64efa99ccdca12b2dee4");
            var firstExpectedCipherText = new BitString("ad0456662b40eae2afef3783ab208e8e");

            var secondExpectedKey = new BitString("8905ef0e76dfac9bc0db0c4cc828d6aecb82e4578d7f1254");
            var secondExpectedIv = new BitString("ad0456662b40eae2afef3783ab208e8e");

            var lastExpectedKey = new BitString("62408183c9316a5501fce6f420e5d797600128763dbeeaba");
            var lastExpectedIv = new BitString("224b94d3b54f836346b3ac8673e0594a");
            var lastExpectedPlainText = new BitString("e5ad48b4e3d389918a719814bbfcc763");
            var lastExpectedCipherText = new BitString("a62280e0e5538243eea688ef2f02a81b");


            var result = _subject.MCTEncrypt(iv, key, plainText);


            Assert.AreEqual(key, sanityCheckFirstKey, nameof(sanityCheckFirstKey));
            Assert.AreEqual(iv, sanityCheckFirstIv, nameof(sanityCheckFirstIv));
            var firstCipherText = result.Response[0].CipherText;
            Assert.AreEqual(firstExpectedCipherText, firstCipherText, nameof(firstCipherText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;

            Assert.AreEqual(secondExpectedKey, secondKey, nameof(secondExpectedKey));
            Assert.AreEqual(secondExpectedIv, secondIv, nameof(secondExpectedIv));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;

            Assert.AreEqual(lastExpectedKey, lastKey, nameof(lastExpectedKey));
            Assert.AreEqual(lastExpectedIv, lastIv, nameof(lastExpectedIv));
            Assert.AreEqual(lastExpectedPlainText, lastPlainText, nameof(lastPlainText));
            Assert.AreEqual(lastExpectedCipherText, lastCipherText, nameof(lastCipherText));

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldMonteCarloTestEncrypt256BitKey()
        {
            string keyString = "757106d825a4e61225dbd9b317c12ed9757cc8edd1dd7f7861dac338ba3f8274";
            string ivString = "c1f64c5a2d2e6fd16dc3f1ca4048a61b";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var plainText = new BitString("642675b49e604dd7755b1a7e9cd039dc");
            var firstExpectedCipherText = new BitString("dbe155d45ccbb202ac10581abe9573d1");

            var secondExpectedKey = new BitString("09e3ae7b108f89a2241b7558265015daae9d9d398d16cd7acdca9b2204aaf1a5");
            var secondExpectedIv = new BitString("dbe155d45ccbb202ac10581abe9573d1");

            var lastExpectedKey = new BitString("750cff078d3681e917afeac20280413c89d0b8d95b29f031531d9031dfaf35b7");
            var lastExpectedIv = new BitString("e7d8b95999d442f2e68fe2f39e3a14ce");
            var lastExpectedPlainText = new BitString("716da78ac71ab64d38f32efb54fe2852");
            var lastExpectedCipherText = new BitString("12f8927c9465d67cd433ca4bcf2bf062");


            var result = _subject.MCTEncrypt(iv, key, plainText);


            Assert.AreEqual(key, sanityCheckFirstKey, nameof(sanityCheckFirstKey));
            Assert.AreEqual(iv, sanityCheckFirstIv, nameof(sanityCheckFirstIv));
            var firstCipherText = result.Response[0].CipherText;
            Assert.AreEqual(firstExpectedCipherText, firstCipherText, nameof(firstCipherText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;

            Assert.AreEqual(secondExpectedKey, secondKey, nameof(secondExpectedKey));
            Assert.AreEqual(secondExpectedIv, secondIv, nameof(secondExpectedIv));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;

            Assert.AreEqual(lastExpectedKey, lastKey, nameof(lastExpectedKey));
            Assert.AreEqual(lastExpectedIv, lastIv, nameof(lastExpectedIv));
            Assert.AreEqual(lastExpectedPlainText, lastPlainText, nameof(lastPlainText));
            Assert.AreEqual(lastExpectedCipherText, lastCipherText, nameof(lastCipherText));

            Assert.IsTrue(result.Success);
        }
        #endregion Encrypt

        #region Decrypt
        [Test]
        public void ShouldMonteCarloTestDecrypt128BitKey()
        {
            string keyString = "6103eb7be3904894498efcaba3e02fdd";
            string ivString = "aeb8755b46e166880416bee30bfd5477";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var cipherText = new BitString("80cd9ac774010998e53dea78fd105c9a");
            var firstExpectedPlainText = new BitString("9417ad4ccb4513a253305f427ee6aa7a");

            var secondExpectedKey = new BitString("f514463728d55b361abea3e9dd0685a7");
            var secondExpectedIv = new BitString("9417ad4ccb4513a253305f427ee6aa7a");

            var lastExpectedKey = new BitString("6dc42535c621783b6761e115a5f99cff");
            var lastExpectedIv = new BitString("b95fe77204ff7782f7643afbfa10015e");
            var lastExpectedCipherText = new BitString("b4e3345c9bd181680cbbe34d4ab3718d");
            var lastExpectedPlainText = new BitString("3187ca5d4555d69c187d16533f4f4a5b");


            var result = _subject.MCTDecrypt(iv, key, cipherText);


            Assert.AreEqual(key, sanityCheckFirstKey, nameof(sanityCheckFirstKey));
            Assert.AreEqual(iv, sanityCheckFirstIv, nameof(sanityCheckFirstIv));
            var firstPlainText = result.Response[0].PlainText;
            Assert.AreEqual(firstExpectedPlainText, firstPlainText, nameof(firstPlainText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;

            Assert.AreEqual(secondExpectedKey, secondKey, nameof(secondExpectedKey));
            Assert.AreEqual(secondExpectedIv, secondIv, nameof(secondExpectedIv));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;

            Assert.AreEqual(lastExpectedKey, lastKey, nameof(lastExpectedKey));
            Assert.AreEqual(lastExpectedIv, lastIv, nameof(lastExpectedIv));
            Assert.AreEqual(lastExpectedCipherText, lastCipherText, nameof(lastCipherText));
            Assert.AreEqual(lastExpectedPlainText, lastPlainText, nameof(lastPlainText));

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldMonteCarloTestDecrypt192BitKey()
        {
            string keyString = "5b3c9ed4cffc4b48436bd85446306078e3776de719970e42";
            string ivString = "a9b41f6c4c04c2044035477432d353db";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var cipherText = new BitString("277422c5882da57509372078e72addbe");
            var firstExpectedPlainText = new BitString("184829ca198cd7c51ec77a417dbc83ac");

            var secondExpectedKey = new BitString("ccb7bd972283210c5b23f19e5fbcb7bdfdb017a6642b8dee");
            var secondExpectedIv = new BitString("184829ca198cd7c51ec77a417dbc83ac");

            var lastExpectedKey = new BitString("7f375ef4a38b979808aaa6be85366c33792bea090e7891cb");
            var lastExpectedIv = new BitString("3e69adb2079e95af3c18ef05f2645d70");
            var lastExpectedCipherText = new BitString("9c9a9f904870a15a0551d6d13f1c48e2");
            var lastExpectedPlainText = new BitString("cb143d1a6491d978c98bcf254094fbff");


            var result = _subject.MCTDecrypt(iv, key, cipherText);


            Assert.AreEqual(key, sanityCheckFirstKey, nameof(sanityCheckFirstKey));
            Assert.AreEqual(iv, sanityCheckFirstIv, nameof(sanityCheckFirstIv));
            var firstPlainText = result.Response[0].PlainText;
            Assert.AreEqual(firstExpectedPlainText, firstPlainText, nameof(firstPlainText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;

            Assert.AreEqual(secondExpectedKey, secondKey, nameof(secondExpectedKey));
            Assert.AreEqual(secondExpectedIv, secondIv, nameof(secondExpectedIv));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;

            Assert.AreEqual(lastExpectedKey, lastKey, nameof(lastExpectedKey));
            Assert.AreEqual(lastExpectedIv, lastIv, nameof(lastExpectedIv));
            Assert.AreEqual(lastExpectedCipherText, lastCipherText, nameof(lastCipherText));
            Assert.AreEqual(lastExpectedPlainText, lastPlainText, nameof(lastPlainText));

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldMonteCarloTestDecrypt256BitKey()
        {
            string keyString = "9add39abf993e6775c51304c023451ce0ca0ac231c84035522dfeaf773837b0c";
            string ivString = "0ac07ca05546f54f3fe63762b7a8b9c7";

            var key = new BitString(keyString);
            var iv = new BitString(ivString);

            var sanityCheckFirstKey = new BitString(keyString);
            var sanityCheckFirstIv = new BitString(ivString);
            var cipherText = new BitString("22c0f8cbcc8b7ccc623c470cfca1c2e2");
            var firstExpectedPlainText = new BitString("814ed7216d1e0989e8d8de37bef090ea");

            var secondExpectedKey = new BitString("140e5e684ccc66c380ac0ab61e13fd398dee7b02719a0adcca0734c0cd73ebe6");
            var secondExpectedIv = new BitString("814ed7216d1e0989e8d8de37bef090ea");

            var lastExpectedKey = new BitString("905b2009ee1fe31186ad52b9262f129062720775b881d009dbf355db51489268");
            var lastExpectedIv = new BitString("3ef317fb9daa1747ef43d87303cea88e");
            var lastExpectedCipherText = new BitString("ee635e6975494fa3dfadbff676ad001c");
            var lastExpectedPlainText = new BitString("d1349786763d446b4171aceee283bfda");


            var result = _subject.MCTDecrypt(iv, key, cipherText);


            Assert.AreEqual(key, sanityCheckFirstKey, nameof(sanityCheckFirstKey));
            Assert.AreEqual(iv, sanityCheckFirstIv, nameof(sanityCheckFirstIv));
            var firstPlainText = result.Response[0].PlainText;
            Assert.AreEqual(firstExpectedPlainText, firstPlainText, nameof(firstPlainText));

            var secondKey = result.Response[1].Key;
            var secondIv = result.Response[1].IV;

            Assert.AreEqual(secondExpectedKey, secondKey, nameof(secondExpectedKey));
            Assert.AreEqual(secondExpectedIv, secondIv, nameof(secondExpectedIv));

            var lastKey = result.Response[result.Response.Count - 1].Key;
            var lastIv = result.Response[result.Response.Count - 1].IV;
            var lastCipherText = result.Response[result.Response.Count - 1].CipherText;
            var lastPlainText = result.Response[result.Response.Count - 1].PlainText;

            Assert.AreEqual(lastExpectedKey, lastKey, nameof(lastExpectedKey));
            Assert.AreEqual(lastExpectedIv, lastIv, nameof(lastExpectedIv));
            Assert.AreEqual(lastExpectedCipherText, lastCipherText, nameof(lastCipherText));
            Assert.AreEqual(lastExpectedPlainText, lastPlainText, nameof(lastPlainText));

            Assert.IsTrue(result.Success);
        }
        #endregion Decrypt
    }
}
