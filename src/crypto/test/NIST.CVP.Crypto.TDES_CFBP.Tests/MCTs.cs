using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.TDES_CFBP.Tests
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
            "ec39cc90fef6f539",  //expected iv2
            "418f21e6544c4a8e",  //expected iv3
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
            "212fc45d731ecc46",  //expected iv2
            "768519b2c874219b",  //expected iv3
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
            "f6e75c1e1869288c",  //expected iv2
            "4c3cb1736dbe7de1",  //expected iv3
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
            "1fd6deea51b23e42",  //expected iv2
            "752c343fa7079397",  //expected iv3
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
            "aa478cd97ee69e22",  //expected iv2
            "ff9ce22ed43bf377",  //expected iv3
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
            "a9781a5122e7f8a2",  //expected iv2
            "fecd6fa6783d4df7",  //expected iv3
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
            "aecc2a589f4def68",  //expected iv2
            "04217fadf4a344bd",  //expected iv3
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
            "1bc20ece3662e287",  //expected iv2
            "711764238bb837dc",  //expected iv3
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
            "0e1296bd739225bf",  //expected iv2
            "6367ec12c8e77b14",  //expected iv3
            "ddbfa0458125ade1",  //expected plainText
            "0fb6547e303690d9",  //expected cipherText
            BlockCipherModesOfOperation.CfbpBlock)]     //algo
        public void ShouldMonteCarloTestEncrypt(
            string startKey1, string startKey2, string startKey3,
            string startIv1, string startPlainText, int rounds,
            string expectedKey1, string expectedKey2, string expectedKey3,
            string expectedIv1, string expectedIv2, string expectedIv3,
            string expectedPlainText, string expectedCipherText,
            BlockCipherModesOfOperation mode, bool isPtAndCtHex = true)
        {
            _subject = new TestCFBPModeMCT(_engineFactory, _modeFactory, _keyMaker, mode);
            _subject.SetNumberOfCases(rounds);

            var key = new BitString(startKey1 + startKey2 + startKey3);
            var iv = new BitString(startIv1);

            var expectedFinalKey = new BitString(expectedKey1 + expectedKey2 + expectedKey3);
            var expectedFinalIV1 = new BitString(expectedIv1);
            var expectedFinalIV2 = new BitString(expectedIv2);
            var expectedFinalIV3 = new BitString(expectedIv3);

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
                Assert.AreEqual(expectedFinalKey.ToHex(), result.Response[result.Response.Count - 1].Keys.ToHex(), nameof(expectedFinalKey));
                Assert.AreEqual(expectedFinalIV1.ToHex(), result.Response[result.Response.Count - 1].IV1.ToHex(), nameof(expectedIv1));
                Assert.AreEqual(expectedFinalIV2.ToHex(), result.Response[result.Response.Count - 1].IV2.ToHex(), nameof(expectedIv2));
                Assert.AreEqual(expectedFinalIV3.ToHex(), result.Response[result.Response.Count - 1].IV3.ToHex(), nameof(expectedIv3));
                Assert.AreEqual(expectedFinalPt.ToHex(), result.Response[result.Response.Count - 1].PlainText.ToHex(), nameof(expectedPlainText));
                Assert.AreEqual(expectedFinalCt.ToHex(), result.Response[result.Response.Count - 1].CipherText.ToHex(), nameof(expectedCipherText));
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
            "8e6702b7336195b0",  //expected iv2
            "e3bc580c88b6eb05",  //expected iv3
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
            "1044c38f373d2328",  //expected iv2
            "659a18e48c92787d",  //expected iv3
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
            "28432f7b08842e69",  //expected iv2
            "7d9884d05dd983be",  //expected iv3
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
            "e552a41216cc52f9",  //expected iv2
            "3aa7f9676c21a84e",  //expected iv3
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
            "3bd6b61f29e00f2a",  //expected iv2
            "912c0b747f35647f",  //expected iv3
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
            "5878c2d72ca00836",  //expected iv2
            "adce182c81f55d8b",  //expected iv3
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
            "20ede805fae0cb62",  //expected iv2
            "76433d5b503620b7",  //expected iv3
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
            "245e23632a3f5fcd",  //expected iv2
            "79b378b87f94b522",  //expected iv3
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
            "fa7b3e92907c7629",  //expected iv2
            "4fd093e7e5d1cb7e",  //expected iv3
            "fe2a6fcc403326f0",  //expected cipherText
            "2d60a0682c95e7df",  //expected plainText
            BlockCipherModesOfOperation.CfbpBlock)]   //algo
        
        public void ShouldMonteCarloTestDecrypt(
            string startKey1, string startKey2, string startKey3,
            string startIv1, string startCipherText, int rounds,
            string expectedKey1, string expectedKey2, string expectedKey3,
            string expectedIv1, string expectedIv2, string expectedIv3,
            string expectedCipherText, string expectedPlainText,
            BlockCipherModesOfOperation mode, bool isPtAndCtHex = true)
        {
            _subject = new TestCFBPModeMCT(_engineFactory, _modeFactory, _keyMaker, mode);
            _subject.SetNumberOfCases(rounds);

            var key = new BitString(startKey1 + startKey2 + startKey3);
            var iv = new BitString(startIv1);

            var expectedFinalKey = new BitString(expectedKey1 + expectedKey2 + expectedKey3);
            var expectedFinalIV1 = new BitString(expectedIv1);
            var expectedFinalIV2 = new BitString(expectedIv2);
            var expectedFinalIV3 = new BitString(expectedIv3);

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
                Assert.AreEqual(expectedFinalKey.ToHex(), result.Response[result.Response.Count - 1].Keys.ToHex(), nameof(expectedFinalKey));
                Assert.AreEqual(expectedFinalIV1.ToHex(), result.Response[result.Response.Count - 1].IV1.ToHex(), nameof(expectedIv1));
                Assert.AreEqual(expectedFinalIV2.ToHex(), result.Response[result.Response.Count - 1].IV2.ToHex(), nameof(expectedIv2));
                Assert.AreEqual(expectedFinalIV3.ToHex(), result.Response[result.Response.Count - 1].IV3.ToHex(), nameof(expectedIv3));
                Assert.AreEqual(expectedFinalPt.ToHex(), result.Response[result.Response.Count - 1].PlainText.ToHex(), nameof(expectedPlainText));
                Assert.AreEqual(expectedFinalCt.ToHex(), result.Response[result.Response.Count - 1].CipherText.ToHex(), nameof(expectedCipherText));
            });

        }
    }
}