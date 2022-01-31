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

namespace NIST.CVP.ACVTS.Libraries.Crypto.TDES_CFB.Tests
{
    [TestFixture, LongCryptoTest]
    public class MCTs
    {

        private TestCFBModeMCT _subject;
        private readonly BlockCipherEngineFactory _engineFactory = new BlockCipherEngineFactory();
        private readonly ModeBlockCipherFactory _modeFactory = new ModeBlockCipherFactory();
        private readonly MonteCarloKeyMaker _keyMaker = new MonteCarloKeyMaker();

        private class TestCFBModeMCT : MonteCarloTdesCfb
        {
            public TestCFBModeMCT(
                IBlockCipherEngineFactory engineFactory,
                IModeBlockCipherFactory modeFactory,
                IMonteCarloKeyMakerTdes keyMaker,
                int shiftSize,
                BlockCipherModesOfOperation mode
            )
                : base(engineFactory, modeFactory, keyMaker, shiftSize, mode) { }

            private int _numberOfCases = 1;

            protected override int NumberOfCases => _numberOfCases;

            public void SetNumberOfCases(int numberOfCases)
            {
                _numberOfCases = numberOfCases;
            }
        }

        [Test]
        [TestCase("b515730d92b615cb83547a2ce6b6fdf897effe384331b6e0", "1", "ed8a73aa9bba7b0d", 1,
                  "b515730d92b615cb83547a2ce6b6fdf897effe384331b6e0", "1", "0", "ed8a73aa9bba7b0d",
                  1, BlockCipherModesOfOperation.CfbBit, false)]

        [TestCase("b515730d92b615cb83547a2ce6b6fdf897effe384331b6e0", "1", "ed8a73aa9bba7b0d", 2,
                  "737380e02343ae3df7c168fd4c86b9da45f7b56468b0c24c", "1", "0", "c667f3edb1f5bbf6",
                  1, BlockCipherModesOfOperation.CfbBit, false)]


        [TestCase("b515730d92b615cb83547a2ce6b6fdf897effe384331b6e0", "1", "ed8a73aa9bba7b0d", 10,
                  "4a23ba51527fc7583d6b85f861da43eca183e9575815a2a8", "1", "0", "c2e8a4d38c414a53",
                  1, BlockCipherModesOfOperation.CfbBit, false)]

        [TestCase("bc5220b9d9eac2c4a775b33183d3fb762680bc1fe05861e0", "8e", "1dfc6de15ecc853f", 1,
                  "bc5220b9d9eac2c4a775b33183d3fb762680bc1fe05861e0", "8e", "70", "1dfc6de15ecc853f",
                  8, BlockCipherModesOfOperation.CfbByte, true)]


        [TestCase("bc5220b9d9eac2c4a775b33183d3fb762680bc1fe05861e0", "8e", "1dfc6de15ecc853f", 2,
                  "abc4d0c82a751ab5e00de346ae46ae0bea083ea8a7293b54", "7d", "23", "1796f070f39fd870",
                   8, BlockCipherModesOfOperation.CfbByte, true)]

        [TestCase("bc5220b9d9eac2c4a775b33183d3fb762680bc1fe05861e0", "8e", "1dfc6de15ecc853f", 10,
                  "f49edcec7052734fc8ae13c2ad23ab6e8938ad89bfdc8a8a", "82", "aa", "9cb6cbe21a671e3e",
                   8, BlockCipherModesOfOperation.CfbByte, true)]

        [TestCase("bac1fe80b686b673fef7e67ac829d6578f7637fb5832f13b", "e77f7c2c598b7a09", "1c51861f9a851911", 1,
                  "bac1fe80b686b673fef7e67ac829d6578f7637fb5832f13b", "e77f7c2c598b7a09", "e2a9f8e9a1f831cb", "1c51861f9a851911",
                   64, BlockCipherModesOfOperation.CfbBlock, true)]


        [TestCase("bac1fe80b686b673fef7e67ac829d6578f7637fb5832f13b", "e77f7c2c598b7a09", "1c51861f9a851911", 2,
                  "58680768167f86b970a4b9f88ccd4962e9f1856d13e04fb0", "8e535f8244e49f34", "4cc3075b1806f415", "e2a9f8e9a1f831cb",
                   64, BlockCipherModesOfOperation.CfbBlock, true)]

        [TestCase("bac1fe80b686b673fef7e67ac829d6578f7637fb5832f13b", "e77f7c2c598b7a09", "1c51861f9a851911", 10,
                  "a8ce150e49f8610e971f97a7d0ef08a4624a23a898e6ae98", "7f250a96cd39d07e", "1721c5eebd87b0fd", "47f9b8176f2a3131",
                  64, BlockCipherModesOfOperation.CfbBlock, true)]

        public void ShouldMonteCarloTestEncrypt(string startingKeyHex, string startingPtHex, string startingIV, int numberOfRounds,
            string expectedFinalKeyHex, string expectedFinalPtHex, string expectedFinalCtHex, string expectedFinalIvHex, int shiftSize, BlockCipherModesOfOperation mode, bool isPtAndCtHex)
        {
            _subject = new TestCFBModeMCT(_engineFactory, _modeFactory, _keyMaker, shiftSize, mode);
            _subject.SetNumberOfCases(numberOfRounds);

            BitString key = new BitString(startingKeyHex);

            BitString iv = new BitString(startingIV);

            BitString expectedFinalKey = new BitString(expectedFinalKeyHex);
            BitString expectedFinalIV = new BitString(expectedFinalIvHex);

            BitString pt, expectedFinalPt, expectedFinalCt;
            if (isPtAndCtHex)
            {
                pt = new BitString(startingPtHex);
                expectedFinalPt = new BitString(expectedFinalPtHex);
                expectedFinalCt = new BitString(expectedFinalCtHex);
            }
            else
            {
                pt = new BitString(
                    MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(startingPtHex)
                );
                expectedFinalPt = new BitString(
                    MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(expectedFinalPtHex)
                );
                expectedFinalCt = new BitString(
                    MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(expectedFinalCtHex)
                );
            }

            var result = _subject.ProcessMonteCarloTest(new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt, iv, key, pt
            ));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedFinalKey.ToHex(), result.Response[result.Response.Count - 1].Keys.ToHex(), nameof(expectedFinalKey));
                Assert.AreEqual(expectedFinalPt.ToHex(), result.Response[result.Response.Count - 1].PlainText.ToHex(), nameof(expectedFinalPtHex));
                Assert.AreEqual(expectedFinalCt.ToHex(), result.Response[result.Response.Count - 1].CipherText.ToHex(), nameof(expectedFinalCtHex));
                Assert.AreEqual(expectedFinalIV.ToHex(), result.Response[result.Response.Count - 1].IV.ToHex(), nameof(expectedFinalIvHex));
            });
        }

        [Test]
        [TestCase("6883a1b6192fabb5c8156d207610a7321fefa7fdadf8b59b", "0", "68fa95c1752560bb", 1,
                  "6883a1b6192fabb5c8156d207610a7321fefa7fdadf8b59b", "1", "0", "68fa95c1752560bb",
                  1, BlockCipherModesOfOperation.CfbBit, false)]

        [TestCase("6883a1b6192fabb5c8156d207610a7321fefa7fdadf8b59b", "0", "68fa95c1752560bb", 2,
                  "31b62f2a0dbfcbdc8af11a8ad61f83b3e5cdd61a7c9415f4", "0", "1", "37137a8bf38fdfd8",
                  1, BlockCipherModesOfOperation.CfbBit, false)]

        [TestCase("6883a1b6192fabb5c8156d207610a7321fefa7fdadf8b59b", "0", "68fa95c1752560bb", 10,
                  "5d58310b70b323bf197ad6e9c226fe588cf461ea79d5c7f7", "0", "1", "77c9da73dfdd2a50",
                  1, BlockCipherModesOfOperation.CfbBit, false)]

        [TestCase("68c26420f78ff2ef01e5324c1ad6ecba1697ba8ae337102a", "c8", "dde8d02f395a2f38", 1,
                  "68c26420f78ff2ef01e5324c1ad6ecba1697ba8ae337102a", "68", "c8", "dde8d02f395a2f38",
                  8, BlockCipherModesOfOperation.CfbByte, true)]

        [TestCase("68c26420f78ff2ef01e5324c1ad6ecba1697ba8ae337102a", "c8", "dde8d02f395a2f38", 2,
                  "1a9e5879310db086c176bc3e75d980fe97293efda1084957", "80", "84", "a1d28fb2eb2cafec",
                  8, BlockCipherModesOfOperation.CfbByte, true)]

        [TestCase("68c26420f78ff2ef01e5324c1ad6ecba1697ba8ae337102a", "c8", "dde8d02f395a2f38", 10,
                  "1a31f8949d640767b0347c9116bfe65dda898a04705eb0b9", "1f", "f1", "f95f1a68b8079517",
                  8, BlockCipherModesOfOperation.CfbByte, true)]

        [TestCase("1c8a5e4079674954d0970816b0c8404cc1d976192fa8ec3b", "f9d0b3c58abd52fb", "9609405b7bb65dcf", 1,
                  "1c8a5e4079674954d0970816b0c8404cc1d976192fa8ec3b", "04e57788a38868b6", "f9d0b3c58abd52fb", "9609405b7bb65dcf",
                  64, BlockCipherModesOfOperation.CfbBlock, true)]

        [TestCase("1c8a5e4079674954d0970816b0c8404cc1d976192fa8ec3b", "f9d0b3c58abd52fb", "9609405b7bb65dcf", 2,
                  "196e29c8daef20e3f216239ba4d692eca898efcbc7014c02", "23c7edff0e389a1d", "296869e08810f487", "2d8d1e682b989c31",
                  64, BlockCipherModesOfOperation.CfbBlock, true)]

        [TestCase("1c8a5e4079674954d0970816b0c8404cc1d976192fa8ec3b", "f9d0b3c58abd52fb", "9609405b7bb65dcf", 10,
                  "eaa16ef189f101efaedf2c34f7ae4034e5573bbaa2452a89", "f2fab935571ffb2c", "01d9ac868d19c90c", "247d1733687747eb",
                  64, BlockCipherModesOfOperation.CfbBlock, true)]
        public void ShouldMonteCarloTestDecrypt(string startingKeyHex, string startingCtHex, string startingIV, int numberOfRounds,
            string expectedFinalKeyHex, string expectedFinalPtHex, string expectedFinalCtHex, string expectedFinalIvHex, int shiftSize, BlockCipherModesOfOperation mode, bool isPtAndCtHex)
        {
            _subject = new TestCFBModeMCT(_engineFactory, _modeFactory, _keyMaker, shiftSize, mode);
            _subject.SetNumberOfCases(numberOfRounds);

            BitString key = new BitString(startingKeyHex);

            BitString iv = new BitString(startingIV);

            BitString expectedFinalKey = new BitString(expectedFinalKeyHex);
            BitString expectedFinalIV = new BitString(expectedFinalIvHex);

            BitString ct, expectedFinalPt, expectedFinalCt;
            if (isPtAndCtHex)
            {
                ct = new BitString(startingCtHex);
                expectedFinalPt = new BitString(expectedFinalPtHex);
                expectedFinalCt = new BitString(expectedFinalCtHex);
            }
            else
            {
                ct = new BitString(
                    MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(startingCtHex)
                );
                expectedFinalPt = new BitString(
                    MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(expectedFinalPtHex)
                );
                expectedFinalCt = new BitString(
                    MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(expectedFinalCtHex)
                );
            }

            var result = _subject.ProcessMonteCarloTest(new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt, iv, key, ct
            ));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedFinalKey.ToHex(), result.Response[result.Response.Count - 1].Keys.ToHex(), nameof(expectedFinalKey));
                Assert.AreEqual(expectedFinalPt.ToHex(), result.Response[result.Response.Count - 1].PlainText.ToHex(), nameof(expectedFinalPtHex));
                Assert.AreEqual(expectedFinalCt.ToHex(), result.Response[result.Response.Count - 1].CipherText.ToHex(), nameof(expectedFinalCtHex));
                Assert.AreEqual(expectedFinalIV.ToHex(), result.Response[result.Response.Count - 1].IV.ToHex(), nameof(expectedFinalIvHex));
            });
        }
    }
}
