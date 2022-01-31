using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Crypto.TDES;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TDES_OFB.Tests
{
    [TestFixture, LongCryptoTest]
    public class MCTs
    {

        private class TestTDesOfbMct : MonteCarloTdesOfb
        {
            public TestTDesOfbMct(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory, IMonteCarloKeyMakerTdes keyMaker)
                : base(engineFactory, modeFactory, keyMaker) { }

            private int _numberOfCases = 1;

            protected override int NumberOfCases => _numberOfCases;

            public void SetNumberOfCases(int numberOfCases)
            {
                _numberOfCases = numberOfCases;
            }
        }

        private readonly TestTDesOfbMct _subject = new TestTDesOfbMct(
            new BlockCipherEngineFactory(),
            new ModeBlockCipherFactory(),
            new MonteCarloKeyMaker()
        );

        [Test]
        [TestCase("F1EFDA23F48A6194674AFB1A86860D407361683798D37C68", "42F761DB81650D7F", "7C2523C15334C0BB", 1, "F1EFDA23F48A6194674AFB1A86860D407361683798D37C68", "42F761DB81650D7F", "2B9C18EEC7A90FF4", "7C2523C15334C0BB")]
        [TestCase("F1EFDA23F48A6194674AFB1A86860D407361683798D37C68", "42F761DB81650D7F", "7C2523C15334C0BB", 2, "DA73C2CD32236E617FF83D866D43C4E5463152F1AE38F704", "EA47A7787BBB958A", "028462203251EFF2", "E2C6E141F1E707BC")]
        [TestCase("F1EFDA23F48A6194674AFB1A86860D407361683798D37C68", "42F761DB81650D7F", "7C2523C15334C0BB", 10, "6E16CBF2209BBC6226AD5898582C6EAD9E0BF702382A62D0", "FE7BCB76CA03857B", "088FBBEE2C518D4C", "3D30C0A626B2190B")]
        public void ShouldMonteCarloTestEncrypt(string startingKeyHex, string startingPtHex, string startingIV, int numberOfRounds, string expectedFinalKeyHex, string expectedFinalPtHex, string expectedFinalCtHex, string expectedFinalIvHex)
        {
            _subject.SetNumberOfCases(numberOfRounds);

            BitString key = new BitString(startingKeyHex);
            BitString pt = new BitString(startingPtHex);
            BitString iv = new BitString(startingIV);

            BitString expectedFinalKey = new BitString(expectedFinalKeyHex);
            BitString expectedFinalPt = new BitString(expectedFinalPtHex);
            BitString expectedFinalCt = new BitString(expectedFinalCtHex);
            BitString expectedFinalIV = new BitString(expectedFinalIvHex);


            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv.GetDeepCopy(),
                key.GetDeepCopy(),
                pt.GetDeepCopy()
            );
            var result = _subject.ProcessMonteCarloTest(param);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedFinalKey.ToHex(), result.Response[result.Response.Count - 1].Keys.ToHex(), nameof(expectedFinalKey));
                Assert.AreEqual(expectedFinalPt.ToHex(), result.Response[result.Response.Count - 1].PlainText.ToHex(), nameof(expectedFinalPtHex));
                Assert.AreEqual(expectedFinalCt.ToHex(), result.Response[result.Response.Count - 1].CipherText.ToHex(), nameof(expectedFinalCtHex));
                Assert.AreEqual(expectedFinalIV.ToHex(), result.Response[result.Response.Count - 1].IV.ToHex(), nameof(expectedFinalIvHex));
            });

        }

        [Test]
        [TestCase("DF97E0022919E38F8C750BC4498683B9B63B52C458B06764", "71A4F7C65ACA4415", "2C2B952BBC8F0E3B", 1, "DF97E0022919E38F8C750BC4498683B9B63B52C458B06764", "7278B1D5D95CA56A", "71A4F7C65ACA4415", "2C2B952BBC8F0E3B")]
        [TestCase("DF97E0022919E38F8C750BC4498683B9B63B52C458B06764", "71A4F7C65ACA4415", "2C2B952BBC8F0E3B", 2, "ADEF51D6F14546E5928CCDB5AD5DDC3D494580E0541AD680", "DF7C8172DC2B5DF7", "4D7DD637397FD09D", "F838F10D69F65BA3")]
        [TestCase("DF97E0022919E38F8C750BC4498683B9B63B52C458B06764", "71A4F7C65ACA4415", "2C2B952BBC8F0E3B", 10, "B0DA8FB0FE46C45BB61015D95BE0AD0EDA027637AB2C6129", "D17E2156FF60B338", "5F60A98539497770", "F03A7DBCC84EB605")]

        public void ShouldMonteCarloTestDecrypt(string startingKeyHex, string startingCtHex, string startingIV, int numberOfRounds, string expectedFinalKeyHex, string expectedFinalPtHex, string expectedFinalCtHex, string expectedFinalIvHex)
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
                iv.GetDeepCopy(),
                key.GetDeepCopy(),
                ct.GetDeepCopy()
            );
            var result = _subject.ProcessMonteCarloTest(param);

            Assert.Multiple(() =>
            {

                Assert.AreEqual(expectedFinalKey.ToHex(), result.Response[result.Response.Count - 1].Keys.ToHex(), nameof(expectedFinalKey));
                Assert.AreEqual(expectedFinalPt.ToHex(), result.Response[result.Response.Count - 1].PlainText.ToHex(), nameof(expectedFinalPtHex));
                Assert.AreEqual(expectedFinalIV.ToHex(), result.Response[result.Response.Count - 1].IV.ToHex(), nameof(expectedFinalIvHex));
            });
        }
    }
}
