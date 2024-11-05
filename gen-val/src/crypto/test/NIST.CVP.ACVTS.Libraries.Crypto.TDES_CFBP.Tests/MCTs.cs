using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TDES_CFBP.Tests
{
    [TestFixture, LongCryptoTest]
    public class MCTs
    {
        private TestCFBPModeMCT _subject;
        private readonly BlockCipherEngineFactory _engineFactory = new BlockCipherEngineFactory();
        private readonly ModeBlockCipherFactory _modeFactory = new ModeBlockCipherFactory();
        private readonly MonteCarloKeyMaker _keyMaker = new MonteCarloKeyMaker();

        private class TestCFBPModeMCT : MonteCarloTdesCfbp
        {
            public TestCFBPModeMCT(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory, IMonteCarloKeyMakerTdes keyMaker, BlockCipherModesOfOperation mode)
                : base(engineFactory, modeFactory, keyMaker, mode)
            {
            }

            private int _numberOfCases = 1;

            protected override int NumberOfCases => _numberOfCases;

            public void SetNumberOfCases(int numberOfCases)
            {
                _numberOfCases = numberOfCases;
            }
        }

        [Test]
        [TestCase(
            "8f7013cd6d13bc52",  //key1 
            "57158c403bb9ec6e",  //key2
            "89bf19cd61cb8a02",  //key3
            "96e4773ba9a19fe4",  //iv1
            "0",                 //plaintext
            1,                   //number of rounds
            "8f7013cd6d13bc52",  //expected key1
            "57158c403bb9ec6e",  //expected key2 
            "89bf19cd61cb8a02",  //expected key3
            "96e4773ba9a19fe4",  //expected iv1
            "0",                 //expected plainText
            "1",                 //expected cipherText
            BlockCipherModesOfOperation.CfbpBit,     //algo
            false)]               //isPtAndCtHex

        [TestCase(
            "8f7013cd6d13bc52",  //key1 
            "57158c403bb9ec6e",  //key2
            "89bf19cd61cb8a02",  //key3
            "96e4773ba9a19fe4",  //iv1
            "0",                 //plaintext
            2,                   //number of rounds
            "a119aeec1a376792",  //expected key1
            "9b4534d5bf6b9d7c",  //expected key2 
            "ea0e5b4c3262fe5b",  //expected key3
            "cbda6f081dc976f1",  //expected iv1
            "0",                 //expected plainText
            "1",                 //expected cipherText
            BlockCipherModesOfOperation.CfbpBit,     //algo
            false)]               //isPtAndCtHex

        [TestCase(
            "8f7013cd6d13bc52",  //key1 
            "57158c403bb9ec6e",  //key2
            "89bf19cd61cb8a02",  //key3
            "96e4773ba9a19fe4",  //iv1
            "0",                 //plaintext
            10,                  //number of rounds
            "8f15daec9734ea29",  //expected key1
            "cbd54029c462d91c",  //expected key2 
            "9b4670a1ecd3f783",  //expected key3
            "a19206c8c313d337",  //expected iv1
            "0",                 //expected plainText
            "1",                 //expected cipherText
            BlockCipherModesOfOperation.CfbpBit,     //algo
            false)]               //isPtAndCtHex

        [TestCase(
            "089725404620fd94",  //key1 
            "b0ba371cc764ada4",  //key2
            "79c2bc837a5e4cdc",  //key3
            "ca818994fc5ce8ed",  //iv1
            "88",                //plaintext
            1,                   //number of rounds
            "089725404620fd94",  //expected key1
            "b0ba371cc764ada4",  //expected key2 
            "79c2bc837a5e4cdc",  //expected key3
            "ca818994fc5ce8ed",  //expected iv1
            "88",                //expected plainText
            "cd",                //expected cipherText
            BlockCipherModesOfOperation.CfbpByte)]     //algo

        [TestCase(
            "089725404620fd94",  //key1 
            "b0ba371cc764ada4",  //key2
            "79c2bc837a5e4cdc",  //key3
            "ca818994fc5ce8ed",  //iv1
            "88",                //plaintext
            2,                   //number of rounds
            "3e130dd00eef3258",  //expected key1
            "bc5797aeb6e9f857",  //expected key2 
            "58c48c19c28f85ef",  //expected key3
            "54f23784299148cd",  //expected iv1
            "8c",                //expected plainText
            "30",                //expected cipherText
            BlockCipherModesOfOperation.CfbpByte)]     //algo

        [TestCase(
            "089725404620fd94",  //key1 
            "b0ba371cc764ada4",  //key2
            "79c2bc837a5e4cdc",  //key3
            "ca818994fc5ce8ed",  //iv1
            "88",                //plaintext
            10,                  //number of rounds
            "b57fcb7c9b10cbda",  //expected key1
            "7cba082fc894d932",  //expected key2 
            "f1baf291c4026797",  //expected key3
            "5422c4fbcd92a34d",  //expected iv1
            "5b",                //expected plainText
            "e8",                //expected cipherText
            BlockCipherModesOfOperation.CfbpByte)]     //algo

        [TestCase(
            "a8b62ae0a804853b",  //key1 
            "4f3ecb49d9bc3d70",  //key2
            "1ff19b31e9bff129",  //key3
            "5976d50349f89a13",  //iv1
            "bc205af012edc861",  //plaintext
            1,                   //number of rounds
            "a8b62ae0a804853b",  //expected key1
            "4f3ecb49d9bc3d70",  //expected key2 
            "1ff19b31e9bff129",  //expected key3
            "5976d50349f89a13",  //expected iv1
            "bc205af012edc861",  //expected plainText
            "c66cb978e10d8d32",  //expected cipherText
            BlockCipherModesOfOperation.CfbpBlock)]     //algo

        [TestCase(
            "a8b62ae0a804853b",  //key1 
            "4f3ecb49d9bc3d70",  //key2
            "1ff19b31e9bff129",  //key3
            "5976d50349f89a13",  //iv1
            "bc205af012edc861",  //plaintext
            2,                   //number of rounds
            "6eda929849080808",  //expected key1
            "13b59ddf89e5cd25",  //expected key2 
            "cd5be516012a8fd3",  //expected key3
            "c66cb978e10d8d32",  //expected iv1
            "6f369bfb78804490",  //expected plainText
            "12d1d4ec330f4103",  //expected cipherText
            BlockCipherModesOfOperation.CfbpBlock)]     //algo

        [TestCase(
            "a8b62ae0a804853b",  //key1 
            "4f3ecb49d9bc3d70",  //key2
            "1ff19b31e9bff129",  //key3
            "5976d50349f89a13",  //iv1
            "bc205af012edc861",  //plaintext
            10,                  //number of rounds
            "d95b67f2578cb3f4",  //expected key1
            "01ae6415d6451645",  //expected key2 
            "1fa2292f0b15da2f",  //expected key3
            "b8bd41681e3cd06a",  //expected iv1
            "ddbfa0458125ade1",  //expected plainText
            "0fb6547e303690d9",  //expected cipherText
            BlockCipherModesOfOperation.CfbpBlock)]     //algo
        public void ShouldMonteCarloTestEncrypt(
            string startKey1, string startKey2, string startKey3,
            string startIv1, string startPlainText, int rounds,
            string expectedKey1, string expectedKey2, string expectedKey3,
            string expectedIv,
            string expectedPlainText, string expectedCipherText,
            BlockCipherModesOfOperation mode, bool isPtAndCtHex = true)
        {
            _subject = new TestCFBPModeMCT(_engineFactory, _modeFactory, _keyMaker, mode);
            _subject.SetNumberOfCases(rounds);

            var key = new BitString(startKey1 + startKey2 + startKey3);
            var iv = new BitString(startIv1);

            var expectedFinalKey = new BitString(expectedKey1 + expectedKey2 + expectedKey3);
            var expectedFinalIV = new BitString(expectedIv);

            BitString pt, expectedFinalPt, expectedFinalCt;
            if (isPtAndCtHex)
            {
                pt = new BitString(startPlainText);
                expectedFinalPt = new BitString(expectedPlainText);
                expectedFinalCt = new BitString(expectedCipherText);
            }
            else
            {
                pt = new BitString(
                    MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(startPlainText)
                );
                expectedFinalPt = new BitString(
                    MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(expectedPlainText)
                );
                expectedFinalCt = new BitString(
                    MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(expectedCipherText)
                );
            }
            var result = _subject.ProcessMonteCarloTest(new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt, iv.GetDeepCopy(), key.GetDeepCopy(), pt.GetDeepCopy()
            ));

            Assert.Multiple(() =>
            {
                Assert.That(result.Response[result.Response.Count - 1].Keys.ToHex(), Is.EqualTo(expectedFinalKey.ToHex()), nameof(expectedFinalKey));
                Assert.That(result.Response[result.Response.Count - 1].IV.ToHex(), Is.EqualTo(expectedFinalIV.ToHex()), nameof(expectedIv));
                Assert.That(result.Response[result.Response.Count - 1].PlainText.ToHex(), Is.EqualTo(expectedFinalPt.ToHex()), nameof(expectedPlainText));
                Assert.That(result.Response[result.Response.Count - 1].CipherText.ToHex(), Is.EqualTo(expectedFinalCt.ToHex()), nameof(expectedCipherText));
            });

        }

        [Test]
        [TestCase(
            "9b89f4fd2c38fbe9",  //key1 
            "0207c46183e65176",  //key2
            "51a40158c126456d",  //key3
            "3911ad61de0c405b",  //iv1
            "1",                 //ciphertext
            1,                   //number of rounds
            "9b89f4fd2c38fbe9",  //expected key1
            "0207c46183e65176",  //expected key2 
            "51a40158c126456d",  //expected key3
            "3911ad61de0c405b",  //expected iv1
            "1",                 //expected cipherText
            "1",                 //expected plainText
            BlockCipherModesOfOperation.CfbpBit,     //algo
            false)]

        [TestCase(
            "9b89f4fd2c38fbe9",  //key1 
            "0207c46183e65176",  //key2
            "51a40158c126456d",  //key3
            "3911ad61de0c405b",  //iv1
            "1",                 //ciphertext
            2,                   //number of rounds
            "a74f3dd5a498a23e",  //expected key1
            "7a3280fdb58a1349",  //expected key2 
            "6e022094f15b01c8",  //expected key3
            "baef6e39e1e7cdd3",  //expected iv1
            "0",                 //expected cipherText
            "0",                 //expected plainText
            BlockCipherModesOfOperation.CfbpBit,     //algo
            false)]

        [TestCase(
            "9b89f4fd2c38fbe9",  //key1 
            "0207c46183e65176",  //key2
            "51a40158c126456d",  //key3
            "3911ad61de0c405b",  //iv1
            "1",                 //ciphertext
            10,                  //number of rounds
            "52c7c4c8198380ad",  //expected key1
            "853d34c7c47aa88a",  //expected key2 
            "e6c22a027cfe70ea",  //expected key3
            "d2edda25b32ed914",  //expected iv1
            "1",                 //expected cipherText
            "0",                 //expected plainText
            BlockCipherModesOfOperation.CfbpBit,     //algo
            false)]

        [TestCase(
            "fb267a6b895d897c",  //key1 
            "25403d807fdcb386",  //key2
            "527958b6da1fef9b",  //key3
            "8ffd4ebcc176fda4",  //iv1
            "8a",                //ciphertext
            1,                   //number of rounds
            "fb267a6b895d897c",  //expected key1
            "25403d807fdcb386",  //expected key2 
            "527958b6da1fef9b",  //expected key3
            "8ffd4ebcc176fda4",  //expected iv1
            "8a",                //expected cipherText
            "ec",                //expected plainText
            BlockCipherModesOfOperation.CfbpByte)]   //algo

        [TestCase(
            "fb267a6b895d897c",  //key1 
            "25403d807fdcb386",  //key2
            "527958b6da1fef9b",  //key3
            "8ffd4ebcc176fda4",  //iv1
            "8a",                //ciphertext
            2,                   //number of rounds
            "523b2558f270b391",  //expected key1
            "899bcba43ba1d567",  //expected key2 
            "9d61a2c2e973b94f",  //expected key3
            "e68160c9d48ab9d5",  //expected iv1
            "39",                //expected cipherText
            "e2",                //expected plainText
            BlockCipherModesOfOperation.CfbpByte)]    //algo

        [TestCase(
            "fb267a6b895d897c",  //key1 
            "25403d807fdcb386",  //key2
            "527958b6da1fef9b",  //key3
            "8ffd4ebcc176fda4",  //iv1
            "8a",                //ciphertext
            10,                  //number of rounds
            "4caeb6f716c4f23b",  //expected key1
            "feea31670779d5da",  //expected key2 
            "ef2adfc2d0f498d3",  //expected key3
            "03236d81d74ab2e1",  //expected iv1
            "53",                //expected cipherText
            "4f",                //expected plainText
            BlockCipherModesOfOperation.CfbpByte)]    //algo

        [TestCase(
            "15525b6b6bc7cbea",  //key1 
            "3732d5dfd9c7dfa8",  //key2
            "5eae258a32159297",  //key3
            "cb9892b0a58b760d",  //iv1
            "977b0343a9f1dd40",  //ciphertext
            1,                   //number of rounds
            "15525b6b6bc7cbea",  //expected key1
            "3732d5dfd9c7dfa8",  //expected key2 
            "5eae258a32159297",  //expected key3
            "cb9892b0a58b760d",  //expected iv1
            "977b0343a9f1dd40",  //expected cipherText
            "b820a65e8143011d",  //expected plainText
            BlockCipherModesOfOperation.CfbpBlock)]   //algo

        [TestCase(
            "15525b6b6bc7cbea",  //key1 
            "3732d5dfd9c7dfa8",  //key2
            "5eae258a32159297",  //key3
            "cb9892b0a58b760d",  //iv1
            "977b0343a9f1dd40",  //ciphertext
            2,                   //number of rounds
            "ad73fd34ea85cbf7",  //expected key1
            "cd31f1e6511a67cd",  //expected key2 
            "df6e575429d6ce54",  //expected key3
            "cf08ce0dd4ea0a78",  //expected iv1
            "7728685355a90b65",  //expected cipherText
            "d970178d24891f75",  //expected plainText
            BlockCipherModesOfOperation.CfbpBlock)]   //algo

        [TestCase(
            "15525b6b6bc7cbea",  //key1 
            "3732d5dfd9c7dfa8",  //key2
            "5eae258a32159297",  //key3
            "cb9892b0a58b760d",  //iv1
            "977b0343a9f1dd40",  //ciphertext
            10,                   //number of rounds
            "6d16622f04981532",  //expected key1
            "ab8f97d3b3d3fb02",  //expected key2 
            "f867c1dc01e6e9f8",  //expected key3
            "a525e93d3b2720d4",  //expected iv1
            "fe2a6fcc403326f0",  //expected cipherText
            "2d60a0682c95e7df",  //expected plainText
            BlockCipherModesOfOperation.CfbpBlock)]   //algo

        public void ShouldMonteCarloTestDecrypt(
            string startKey1, string startKey2, string startKey3,
            string startIv1, string startCipherText, int rounds,
            string expectedKey1, string expectedKey2, string expectedKey3,
            string expectedIv,
            string expectedCipherText, string expectedPlainText,
            BlockCipherModesOfOperation mode, bool isPtAndCtHex = true)
        {
            _subject = new TestCFBPModeMCT(_engineFactory, _modeFactory, _keyMaker, mode);
            _subject.SetNumberOfCases(rounds);

            var key = new BitString(startKey1 + startKey2 + startKey3);
            var iv = new BitString(startIv1);

            var expectedFinalKey = new BitString(expectedKey1 + expectedKey2 + expectedKey3);
            var expectedFinalIV = new BitString(expectedIv);

            BitString ct, expectedFinalPt, expectedFinalCt;
            if (isPtAndCtHex)
            {
                ct = new BitString(startCipherText);
                expectedFinalPt = new BitString(expectedPlainText);
                expectedFinalCt = new BitString(expectedCipherText);
            }
            else
            {
                ct = new BitString(
                    MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(startCipherText)
                );
                expectedFinalPt = new BitString(
                    MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(expectedPlainText)
                );
                expectedFinalCt = new BitString(
                    MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(expectedCipherText)
                );
            }
            var result = _subject.ProcessMonteCarloTest(new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt, iv.GetDeepCopy(), key.GetDeepCopy(), ct.GetDeepCopy()
            ));

            Assert.Multiple(() =>
            {
                Assert.That(result.Response[result.Response.Count - 1].Keys.ToHex(), Is.EqualTo(expectedFinalKey.ToHex()), nameof(expectedFinalKey));
                Assert.That(result.Response[result.Response.Count - 1].IV.ToHex(), Is.EqualTo(expectedFinalIV.ToHex()), nameof(expectedIv));
                Assert.That(result.Response[result.Response.Count - 1].PlainText.ToHex(), Is.EqualTo(expectedFinalPt.ToHex()), nameof(expectedPlainText));
                Assert.That(result.Response[result.Response.Count - 1].CipherText.ToHex(), Is.EqualTo(expectedFinalCt.ToHex()), nameof(expectedCipherText));
            });

        }
    }
}
