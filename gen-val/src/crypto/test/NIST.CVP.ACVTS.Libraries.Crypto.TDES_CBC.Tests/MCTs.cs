﻿using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TDES_CBC.Tests
{
    [TestFixture, LongCryptoTest]
    public class MCTs
    {

        private class TestTDesCbcMct : MonteCarloTdesCbc
        {
            public TestTDesCbcMct(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory, IMonteCarloKeyMakerTdes keyMaker)
                : base(engineFactory, modeFactory, keyMaker) { }

            private int _numberOfCases = 1;

            protected override int NumberOfCases => _numberOfCases;

            public void SetNumberOfCases(int numberOfCases)
            {
                _numberOfCases = numberOfCases;
            }
        }

        private readonly TestTDesCbcMct _subject = new TestTDesCbcMct(
            new BlockCipherEngineFactory(),
            new ModeBlockCipherFactory(),
            new MonteCarloKeyMaker()
        );

        [Test]
        [TestCase("29469264c25ed54ab90e3737049e7301e3198aecc4ae4c4c", "bed1e248f734d16d", "06c3cedd527b1206", 1, "29469264c25ed54ab90e3737049e7301e3198aecc4ae4c4c", "f61d6ca68a7928a6", "06c3cedd527b1206")]
        [TestCase("29469264c25ed54ab90e3737049e7301e3198aecc4ae4c4c", "bed1e248f734d16d", "06c3cedd527b1206", 2, "df5bfec24926fdec6876e958d057b5ade668fee970a7131a", "34370c4d299faa86", "f61d6ca68a7928a6")]
        [TestCase("29469264c25ed54ab90e3737049e7301e3198aecc4ae4c4c", "bed1e248f734d16d", "06c3cedd527b1206", 10, "524fdc9ee6c4a21c4097fea446d313a794da6e9d67761fd3", "4f5e951b0f43997a", "e5a5d8b7f01aca0a")]
        public void ShouldMonteCarloTestEncrypt(string startingKeyHex, string startingPtHex, string startingIV, int numberOfRounds, string expectedFinalKeyHex, string expectedFinalCtHex, string expectedFinalIvHex)
        {
            _subject.SetNumberOfCases(numberOfRounds);

            BitString key = new BitString(startingKeyHex);
            BitString pt = new BitString(startingPtHex);
            BitString iv = new BitString(startingIV);

            BitString expectedFinalKey = new BitString(expectedFinalKeyHex);
            BitString expectedFinalCt = new BitString(expectedFinalCtHex);
            BitString expectedFinalIV = new BitString(expectedFinalIvHex);

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                pt
            );
            var result = _subject.ProcessMonteCarloTest(param);

            Assert.That(result.Response[result.Response.Count - 1].Keys.ToHex(), Is.EqualTo(expectedFinalKey.ToHex()), nameof(expectedFinalKey));
            Assert.That(result.Response[result.Response.Count - 1].IV.ToHex(), Is.EqualTo(expectedFinalIV.ToHex()), nameof(expectedFinalIvHex));
            Assert.That(result.Response[result.Response.Count - 1].CipherText.ToHex(), Is.EqualTo(expectedFinalCt.ToHex()), nameof(expectedFinalCtHex));
        }

        [Test]
        [TestCase("2fe56894d501341a6179f2e5d9d0e568c7e95404c215a8ba", "fa7c94a2eb67dbf0", "f23ea484df4a458f", 1, "2fe56894d501341a6179f2e5d9d0e568c7e95404c215a8ba", "369bef371d3d6357", "f23ea484df4a458f")]
        [TestCase("2fe56894d501341a6179f2e5d9d0e568c7e95404c215a8ba", "fa7c94a2eb67dbf0", "f23ea484df4a458f", 2, "197f86a2c83d574c868391f1f7bf49c8e5b994574c9d7652", "1106a4ca257f7e34", "e6fb63152f6eada1")]
        [TestCase("2fe56894d501341a6179f2e5d9d0e568c7e95404c215a8ba", "fa7c94a2eb67dbf0", "f23ea484df4a458f", 10, "51ab8a834391da266bd65ee51ca7dfdce9d543e0e3df295b", "5d439239e6fd47ba", "f950ba02be073b15")]
        public void ShouldMonteCarloTestDecrypt(string startingKeyHex, string startingCtHex, string startingIV, int numberOfRounds, string expectedFinalKeyHex, string expectedFinalPtHex, string expectedFinalIvHex)
        {
            _subject.SetNumberOfCases(numberOfRounds);

            BitString key = new BitString(startingKeyHex);
            BitString ct = new BitString(startingCtHex);
            BitString iv = new BitString(startingIV);

            BitString expectedFinalKey = new BitString(expectedFinalKeyHex);
            BitString expectedFinalPt = new BitString(expectedFinalPtHex);
            BitString expectedFinalIV = new BitString(expectedFinalIvHex);

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                iv,
                key,
                ct
            );
            var result = _subject.ProcessMonteCarloTest(param);

            Assert.That(result.Response[result.Response.Count - 1].Keys.ToHex(), Is.EqualTo(expectedFinalKey.ToHex()), nameof(expectedFinalKey));
            Assert.That(result.Response[result.Response.Count - 1].IV.ToHex(), Is.EqualTo(expectedFinalIV.ToHex()), nameof(expectedFinalIvHex));
            Assert.That(result.Response[result.Response.Count - 1].PlainText.ToHex(), Is.EqualTo(expectedFinalPt.ToHex()), nameof(expectedFinalPtHex));
        }
    }
}
