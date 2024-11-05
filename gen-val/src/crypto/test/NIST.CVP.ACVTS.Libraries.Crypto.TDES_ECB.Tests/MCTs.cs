using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Crypto.TDES_ECB;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TDES_ECB.Tests
{
    [TestFixture, LongCryptoTest]
    public class MCTs
    {
        private class TestTDesEcbMct : MonteCarloTdesEcb
        {
            public TestTDesEcbMct(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory, IMonteCarloKeyMakerTdes keyMaker)
                : base(engineFactory, modeFactory, keyMaker) { }

            private int _numberOfCases = 1;

            protected override int NumberOfCases => _numberOfCases;

            public void SetNumberOfCases(int numberOfCases)
            {
                _numberOfCases = numberOfCases;
            }
        }

        private readonly TestTDesEcbMct _subject = new TestTDesEcbMct(
            new BlockCipherEngineFactory(),
            new ModeBlockCipherFactory(),
            new MonteCarloKeyMaker()
        );

        [Test]
        [TestCase("6d087a8c917fbc163786e63d8f91d3a24c4a9132643e4f75", "df1ebc023e0f0225", 1, "6d087a8c917fbc163786e63d8f91d3a24c4a9132643e4f75", "cac1f53d6a69e2af")]
        [TestCase("6d087a8c917fbc163786e63d8f91d3a24c4a9132643e4f75", "df1ebc023e0f0225", 2, "a7c88fb0fb165eb9bfbfb9d983751c1ce346fb10c831a1ec", "6ed6af72b56f2970")]
        [TestCase("6d087a8c917fbc163786e63d8f91d3a24c4a9132643e4f75", "df1ebc023e0f0225", 10, "433223a483135b620d6ba875c7325720b6790e7037f723dc", "af152dc1a7be72da")]
        public void ShouldMonteCarloTestEncrypt(string startingKeyHex, string startingPtHex, int numberOfRounds, string expectedFinalKeyHex, string expectedFinalCtHex)
        {
            _subject.SetNumberOfCases(numberOfRounds);

            BitString key = new BitString(startingKeyHex);
            BitString pt = new BitString(startingPtHex);

            BitString expectedFinalKey = new BitString(expectedFinalKeyHex);
            BitString expectedFinalCt = new BitString(expectedFinalCtHex);

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                null,
                key,
                pt
            );
            var result = _subject.ProcessMonteCarloTest(param);

            Assert.That(result.Response[result.Response.Count - 1].Keys.ToHex(), Is.EqualTo(expectedFinalKey.ToHex()), nameof(expectedFinalKey));
            Assert.That(result.Response[result.Response.Count - 1].CipherText.ToHex(), Is.EqualTo(expectedFinalCt.ToHex()), nameof(expectedFinalCtHex));
        }

        [Test]
        [TestCase("c4c873468ce0238f6e51cde61c64804568bccb70372a3749", "2c01a4ccd03cb972", 1, "c4c873468ce0238f6e51cde61c64804568bccb70372a3749", "e43c592a10cc6e84")]
        [TestCase("c4c873468ce0238f6e51cde61c64804568bccb70372a3749", "2c01a4ccd03cb972", 2, "20f42a6d9d2c4c0be55826dc4fbf0e83bc9e642cd9920bc4", "bb6441a197dd8a66")]
        [TestCase("c4c873468ce0238f6e51cde61c64804568bccb70372a3749", "2c01a4ccd03cb972", 10, "ba0bd6612cb55d836b83ae8c85dc51a72cb604ad232a4c0d", "fb82f0d3ca4035f1")]
        public void ShouldMonteCarloTestDecrypt(string startingKeyHex, string startingCtHex, int numberOfRounds, string expectedFinalKeyHex, string expectedFinalPtHex)
        {
            _subject.SetNumberOfCases(numberOfRounds);

            BitString key = new BitString(startingKeyHex);
            BitString ct = new BitString(startingCtHex);

            BitString expectedFinalKey = new BitString(expectedFinalKeyHex);
            BitString expectedFinalPt = new BitString(expectedFinalPtHex);

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                null,
                key,
                ct
            );
            var result = _subject.ProcessMonteCarloTest(param);

            Assert.That(result.Response[result.Response.Count - 1].Keys.ToHex(), Is.EqualTo(expectedFinalKey.ToHex()), nameof(expectedFinalKey));
            Assert.That(result.Response[result.Response.Count - 1].PlainText.ToHex(), Is.EqualTo(expectedFinalPt.ToHex()), nameof(expectedFinalPtHex));
        }
    }
}
