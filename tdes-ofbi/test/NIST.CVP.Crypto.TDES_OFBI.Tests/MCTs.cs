using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TDES_OFBI.Tests
{
    public class MCTs
    {

        private class TestOFBIModeMCT : TdesOfbiMCT
        {
            public TestOFBIModeMCT(IMonteCarloKeyMaker keyMaker)
                : base(keyMaker)
            {
            }

            private int _numberOfCases = 1;

            protected override int NumberOfCases => _numberOfCases;

            public void SetNumberOfCases(int numberOfCases)
            {
                _numberOfCases = numberOfCases;
            }
        }

        readonly TestOFBIModeMCT _subject = new TestOFBIModeMCT(new MonteCarloKeyMaker());

        [Test]
        [TestCase(
                "c10d3e9b1f79ab80a75e5e62fd25ef3e9ef702892346bab5", //startingKeyHex
                "09f2d92e433e4d0d",                                 //startingPtHex
                "724d95e667bba8d5",                                 //startingIV
                1,                                                  //numberOfRounds
                "c10d3e9b1f79ab80a75e5e62fd25ef3e9ef702892346bab5", //expectedFinalKeyHex
                "09f2d92e433e4d0d",                                 //expectedFinalPtHex
                "faacbfb9159fb25b",                                 //expectedFinalCtHex
                "724d95e667bba8d5",                                 //expectedIV1,
                "c7a2eb3bbd10fe2a",                                 //expectedIV2
                "1cf840911266537f",                                 //expectedIV3
                 TestName = "MCT Encrypt 001 round"
            )]


        [TestCase(
            "c10d3e9b1f79ab80a75e5e62fd25ef3e9ef702892346bab5", //startingKeyHex
            "09f2d92e433e4d0d",                                 //startingPtHex
            "724d95e667bba8d5",                                 //startingIV
            2,                                                  //numberOfRounds
            "3ba180230be619dab5b96eb0074c868992b53e07206d9b49", //expectedFinalKeyHex
            "300c51d3fb4cea9d",                                 //expectedFinalPtHex
            "82a14bd4cea5dea5",                                 //expectedFinalCtHex
            "1f26f916fcddfcd3",                                 //expectedIV1,
            "747c4e6c52335228",                                 //expectedIV2
            "c9d1a3c1a788a77d",                                 //expectedIV3
            TestName = "MCT Encrypt 002 round"
        )]

        [TestCase(
            "c10d3e9b1f79ab80a75e5e62fd25ef3e9ef702892346bab5", //startingKeyHex
            "09f2d92e433e4d0d",                                 //startingPtHex
            "724d95e667bba8d5",                                 //startingIV
            10,                                                 //numberOfRounds
            "b5ab5167a82c61aef8c73701f729a7642a54731cd3ecfd7f", //expectedFinalKeyHex
            "a2de53c3a546b16e",                                 //expectedFinalPtHex
            "25c6fc86fd1979be",                                 //expectedFinalCtHex
            "2d0db607bbc6c8e4",                                 //expectedIV1,
            "82630b5d111c1e39",                                 //expectedIV2
            "d7b860b26671738e",                                 //expectedIV3
            TestName = "MCT Encrypt 010 round"
        )]

        [TestCase(
            "c10d3e9b1f79ab80a75e5e62fd25ef3e9ef702892346bab5", //startingKeyHex
            "09f2d92e433e4d0d",                                 //startingPtHex
            "724d95e667bba8d5",                                 //startingIV
            400,                                                 //numberOfRounds
            "0df1ae640408e03d518f5252084a31fec2fde631c70dade6", //expectedFinalKeyHex
            "dabe9c783bac0393",                                 //expectedFinalPtHex
            "ac70d6c3f60c322b",                                 //expectedFinalCtHex
            "c9cafc9b91503232",                                 //expectedIV1,
            "1f2051f0e6a58787",                                 //expectedIV2
            "7475a7463bfadcdc",                                 //expectedIV3
            TestName = "MCT Encrypt 400 round"
        )]
        public void ShouldMonteCarloTestEncrypt(
            string startingKeyHex,
            string startingPtHex,
            string startingIVHex,
            int numberOfRounds,
            string expectedFinalKeyHex,
            string expectedFinalPtHex,
            string expectedFinalCtHex,
            string expectedIv1Hex,
            string expectedIV2Hex,
            string expectedIV3Hex)
        {
            _subject.SetNumberOfCases(numberOfRounds);

            var startingKey = new BitString(startingKeyHex);
            var startingPt = new BitString(startingPtHex);
            var startingIV = new BitString(startingIVHex);
            var expectedFinalKey = new BitString(expectedFinalKeyHex);
            var expectedFinalPt = new BitString(expectedFinalPtHex);
            var expectedFinalCt = new BitString(expectedFinalCtHex);
            var expectedIv1 = new BitString(expectedIv1Hex);
            var expectedIV2 = new BitString(expectedIV2Hex);
            var expectedIV3 = new BitString(expectedIV3Hex);

            var result = _subject.MCTEncrypt(keyBits: startingKey, iv: startingIV, data: startingPt);

            Assert.Multiple(() =>
            {
                var lastResult = result.Response[result.Response.Count - 1];
                Assert.AreEqual(expectedFinalKey.ToHex(), lastResult.Keys.ToHex(), nameof(expectedFinalKey));
                Assert.AreEqual(expectedFinalPt.ToHex(), lastResult.PlainText.ToHex(), nameof(expectedFinalPt));
                Assert.AreEqual(expectedFinalCt.ToHex(), lastResult.CipherText.ToHex(), nameof(expectedFinalCt));
                Assert.AreEqual(expectedIv1.ToHex(), lastResult.IV1.ToHex(), nameof(expectedIv1));
                Assert.AreEqual(expectedIV2.ToHex(), lastResult.IV2.ToHex(), nameof(expectedIV2));
                Assert.AreEqual(expectedIV3.ToHex(), lastResult.IV3.ToHex(), nameof(expectedIV3));

            });
        }


        [Test]
        [TestCase(
            "e0fbec02e50ba2618a29400ba84f8f2f0b7aea0b49e5793e", //startingKeyHex
            "e8799ba46aeaa1c4",                                 //startingCtHex
            "443df12ce9184c9b",                                 //startingIV
            1,                                                  //numberOfRounds
            "e0fbec02e50ba2618a29400ba84f8f2f0b7aea0b49e5793e", //expectedFinalKeyHex
            "c0e04fa57896e53d",                                 //expectedFinalPtHex
            "e8799ba46aeaa1c4",                                 //expectedFinalCtHex
            "443df12ce9184c9b",                                 //expectedIV1,
            "999346823e6da1f0",                                 //expectedIV2
            "eee89bd793c2f745",                                 //expectedIV3
            TestName = "MCT Decrypt 001 round"
        )]


        [TestCase(
            "e0fbec02e50ba2618a29400ba84f8f2f0b7aea0b49e5793e", //startingKeyHex
            "e8799ba46aeaa1c4",                                 //startingCtHex
            "443df12ce9184c9b",                                 //startingIV
            2,                                                  //numberOfRounds
            "201aa2a79d9d465d7c2662ab9ba7bcc2ce239483540df404", //expectedFinalKeyHex
            "33773eaf0554c5be",                                 //expectedFinalPtHex
            "76ccf97649827129",                                 //expectedFinalCtHex
            "30458ab24526f952",                                 //expectedIV1,
            "859ae0079a7c4ea7",                                 //expectedIV2
            "daf0355cefd1a3fc",                                 //expectedIV3
            TestName = "MCT Decrypt 002 round"
        )]


        [TestCase(
            "e0fbec02e50ba2618a29400ba84f8f2f0b7aea0b49e5793e", //startingKeyHex
            "e8799ba46aeaa1c4",                                 //startingCtHex
            "443df12ce9184c9b",                                 //startingIV
            10,                                                 //numberOfRounds
            "e62fae4c7502ef076bbcc11a0dc7b6d329a8917a834fa416", //expectedFinalKeyHex
            "bb1e98b123d7fec3",                                 //expectedFinalPtHex
            "b66405227fef87e3",                                 //expectedFinalCtHex
            "f9245486eb7acff4",                                 //expectedIV1,
            "4e79a9dc40d02549",                                 //expectedIV2
            "a3ceff3196257a9e",                                 //expectedIV3
            TestName = "MCT Decrypt 010 round"
        )]


        [TestCase(
            "e0fbec02e50ba2618a29400ba84f8f2f0b7aea0b49e5793e", //startingKeyHex
            "e8799ba46aeaa1c4",                                 //startingCtHex
            "443df12ce9184c9b",                                 //startingIV
            400,                                                //numberOfRounds
            "a2fbefd9b96bf29e9289c86db9ba6bec6e37d983d38525df", //expectedFinalKeyHex
            "b9c24c2a87b465a5",                                 //expectedFinalPtHex
            "fc632be08d259e02",                                 //expectedFinalCtHex
            "932cda1b9622af36",                                 //expectedIV1,
            "e8822f70eb78048b",                                 //expectedIV2
            "3dd784c640cd59e0",                                 //expectedIV3
            TestName = "MCT Decrypt 400 round"
        )]
        public void ShouldMonteCarloTestDecrypt(
            string startingKeyHex,
            string startingCtHex,
            string startingIVHex,
            int numberOfRounds,
            string expectedFinalKeyHex,
            string expectedFinalPtHex,
            string expectedFinalCtHex,
            string expectedIv1Hex,
            string expectedIV2Hex,
            string expectedIV3Hex)
        {
            _subject.SetNumberOfCases(numberOfRounds);

            var startingKey = new BitString(startingKeyHex);
            var startingCt = new BitString(startingCtHex);
            var startingIV = new BitString(startingIVHex);
            var expectedFinalKey = new BitString(expectedFinalKeyHex);
            var expectedFinalPt = new BitString(expectedFinalPtHex);
            var expectedFinalCt = new BitString(expectedFinalCtHex);
            var expectedIv1 = new BitString(expectedIv1Hex);
            var expectedIV2 = new BitString(expectedIV2Hex);
            var expectedIV3 = new BitString(expectedIV3Hex);

            var result = _subject.MCTDecrypt(keyBits: startingKey, iv: startingIV, data: startingCt);

            Assert.Multiple(() =>
            {
                var lastResult = result.Response[result.Response.Count - 1];
                Assert.AreEqual(expectedFinalKey.ToHex(), lastResult.Keys.ToHex(), nameof(expectedFinalKey));
                Assert.AreEqual(expectedFinalPt.ToHex(), lastResult.PlainText.ToHex(), nameof(expectedFinalPt));
                Assert.AreEqual(expectedFinalCt.ToHex(), lastResult.CipherText.ToHex(), nameof(expectedFinalCt));
                Assert.AreEqual(expectedIv1.ToHex(), lastResult.IV1.ToHex(), nameof(expectedIv1));
                Assert.AreEqual(expectedIV2.ToHex(), lastResult.IV2.ToHex(), nameof(expectedIV2));
                Assert.AreEqual(expectedIV3.ToHex(), lastResult.IV3.ToHex(), nameof(expectedIV3));

            });
        }
    }
}
