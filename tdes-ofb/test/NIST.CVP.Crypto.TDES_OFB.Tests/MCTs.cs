using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TDES_OFB.Tests
{
    public class MCTs
    {

        private class TestTDesOfbMct : TDES_OFB_MCT
        {
            public TestTDesOfbMct(ITDES_OFB algo, IMonteCarloKeyMaker keyMaker)
                : base(algo, keyMaker) { }

            private int _numberOfCases = 1;

            protected override int NumberOfCases { get { return _numberOfCases; } }

            public void SetNumberOfCases(int numberOfCases)
            {
                _numberOfCases = numberOfCases;
            }
        }

        TestTDesOfbMct _subject = new TestTDesOfbMct(new TdesOfb(), new MonteCarloKeyMaker());

        [Test]
        [TestCase("077f9707fb013dc8751379f80ef8dfb0b389a819457a1c16", "d1129fbb096bd692", "86b6c9d558918cd5",  1, "4a048f98d6b568c846e53e2c6804237a51bcec4c8c138f8c", "f1ad08a091ab678c", "13dd9d4147694872", "3cbbea5a5e1c7888")]
        [TestCase("077f9707fb013dc8751379f80ef8dfb0b389a819457a1c16", "d1129fbb096bd692", "86b6c9d558918cd5",  2, "58d913d991dc20ba85d0f7d0f2ea8076756b8f85a24c6b3d", "1f6abc3c6ea219fd", "85ddae1761ba0019", "fb7dd11fbd2f10cd")]
        [TestCase("077f9707fb013dc8751379f80ef8dfb0b389a819457a1c16", "d1129fbb096bd692", "86b6c9d558918cd5", 10, "d3a7dc94e5643befdfd3c84968a83ef4b56783d09e027f1f", "1744c8c0c9352b7a", "18154d3d1579bcc9", "a98c513dd987c441")]
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


             var result = _subject.MCTEncrypt(key, pt, iv);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedFinalKey.ToHex(), result.Response[result.Response.Count - 1].Keys.ToHex(), nameof(expectedFinalKey));
                Assert.AreEqual(expectedFinalPt.ToHex(), result.Response[result.Response.Count - 1].PlainText.ToHex(), nameof(expectedFinalPtHex));
                Assert.AreEqual(expectedFinalCt.ToHex(), result.Response[result.Response.Count - 1].CipherText.ToHex(), nameof(expectedFinalCtHex));
                Assert.AreEqual(expectedFinalIV.ToHex(), result.Response[result.Response.Count - 1].IV.ToHex(), nameof(expectedFinalIvHex));
            });

        }

        [Test]
        [TestCase("d602a1d0c83eec1fb90b8fba8002f2e9feb03bae51e9545d", "475f7c6b7d95923d", "0cafddc3b1362d63",  1, "dc9d5192c2d92f529491c86d94e5f1978f40f4702cd0dabf", "43ce97cd31f08958", "7709a02a8d00a715", "5d7cbfb422961ef1")]
        [TestCase("d602a1d0c83eec1fb90b8fba8002f2e9feb03bae51e9545d", "475f7c6b7d95923d", "0cafddc3b1362d63",  2, "9e52c75ef229a70b2594b970fbbc8c4ad3382fe53d3468cb", "a5b21d25035d3e60", "7a996e573495ce55", "851d496af10480f9")]
        [TestCase("d602a1d0c83eec1fb90b8fba8002f2e9feb03bae51e9545d", "475f7c6b7d95923d", "0cafddc3b1362d63", 10, "76400208d0796e7331cb581fab317c32d389230e4a20e35d", "12fa0c4cdaf1849d", "15f2278b3fec7673", "39bdb1350c979325")]

        public void ShouldMonteCarloTestDecrypt(string startingKeyHex, string startingCtHex, string startingIV, int numberOfRounds, string expectedFinalKeyHex, string expectedFinalPtHex, string expectedFinalCtHex, string expectedFinalIvHex)
        {
            _subject.SetNumberOfCases(numberOfRounds);

            BitString key = new BitString(startingKeyHex);
            BitString ct = new BitString(startingCtHex);
            BitString iv = new BitString(startingIV);

            BitString expectedFinalKey = new BitString(expectedFinalKeyHex);
            BitString expectedFinalPt = new BitString(expectedFinalPtHex);
            BitString expectedFinalIV = new BitString(expectedFinalIvHex);

            var result = _subject.MCTDecrypt(key, ct, iv);

            Assert.AreEqual(expectedFinalKey.ToHex(), result.Response[result.Response.Count - 1].Keys.ToHex(), nameof(expectedFinalKey));
            Assert.AreEqual(expectedFinalPt.ToHex(), result.Response[result.Response.Count - 1].PlainText.ToHex(), nameof(expectedFinalPtHex));
            Assert.AreEqual(expectedFinalIV.ToHex(), result.Response[result.Response.Count - 1].IV.ToHex(), nameof(expectedFinalIvHex));

        }
    }
}
