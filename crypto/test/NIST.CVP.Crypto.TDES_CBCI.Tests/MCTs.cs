using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TDES_CBCI.Tests
{
    public class MCTs
    {
        private class TestTDesCbciMct : TdesCbciMCT
        {
            public TestTDesCbciMct(IMonteCarloKeyMaker keyMaker) : base(keyMaker) { }

            private int _numberOfCases = 1;

            protected override int NumberOfCases => _numberOfCases;

            public void SetNumberOfCases(int numberOfCases)
            {
                _numberOfCases = numberOfCases;
            }
        }

        readonly TestTDesCbciMct _subject = new TestTDesCbciMct(new MonteCarloKeyMaker());

        [Test]
        [TestCase(
                "da29627667c70d3d310becdc76d9d6678c0279f78c945202", //startingKeyHex
                "de94b10c9a04b92d6848ce7c9deb5da9ebbb08958040ebd5", //startingPtHex
                "729ec1cff09ba0f1",                                 //startingIV
                1,                                                  //numberOfRounds
                "da29627667c70d3d310becdc76d9d6678c0279f78c945202", //expectedFinalKeyHex
                "de94b10c9a04b92d6848ce7c9deb5da9ebbb08958040ebd5", //expectedFinalPtHex
                "28aa4c8e263939898a8b3b0e07b014244c7d85f2c6c7d76c", //expectedFinalCtHex
                "729ec1cff09ba0f1",                                 //expectedIV1,
                "c7f4172545f0f646",                                 //expectedIV2
                "1d496c7a9b464b9b",                                 //expectedIV3
                 TestName = "MCT Encrypt 001 round"
            )]

        [TestCase(
            "da29627667c70d3d310becdc76d9d6678c0279f78c945202", //startingKeyHex
            "de94b10c9a04b92d6848ce7c9deb5da9ebbb08958040ebd5", //startingPtHex
            "729ec1cff09ba0f1",                                 //startingIV
            2,                                                  //numberOfRounds
            "f2832ff840fe34b5fb91d9837fe657e0d9e508ea2c76a837", //expectedFinalKeyHex
            "d577c74d8250be46cb9a355e093f8086a8479e705a1b5574", //expectedFinalPtHex
            "b77a9c2c4db50da5e07f1c0e7c31d9482597b50d04080ace", //expectedFinalCtHex
            "28aa4c8e26393989",                                 //expectedIV1,
            "8a8b3b0e07b01424",                                 //expectedIV2
            "4c7d85f2c6c7d76c",                                 //expectedIV3
            TestName = "MCT Encrypt 002 rounds"
        )]

        [TestCase(
            "da29627667c70d3d310becdc76d9d6678c0279f78c945202", //startingKeyHex
            "de94b10c9a04b92d6848ce7c9deb5da9ebbb08958040ebd5", //startingPtHex
            "729ec1cff09ba0f1",                                 //startingIV
            10,                                                 //numberOfRounds
            "adc2efd6ef7c2f5d98649d4552ea26b0fbc12901860d1054", //expectedFinalKeyHex
            "16e7d3786c368d398d4d1402f4de2ec5dfe26053689d5c78", //expectedFinalPtHex
            "13c516373f3eed1da86f95675265e780a0a6cc7d41f1c18d", //expectedFinalCtHex
            "c84b284d2ead40bd",                                 //expectedIV1,
            "e7e33d933c87d8b3",                                 //expectedIV2
            "dfe26bea2d5496ad",                                 //expectedIV3
            TestName = "MCT Encrypt 010 rounds"
        )]

        [TestCase(
            "da29627667c70d3d310becdc76d9d6678c0279f78c945202", //startingKeyHex
            "de94b10c9a04b92d6848ce7c9deb5da9ebbb08958040ebd5", //startingPtHex
            "729ec1cff09ba0f1",                                 //startingIV
            400,                                                //numberOfRounds
            "8a51d5733d0d4cf15dc4755b7940a25ed0ae085e73260897", //expectedFinalKeyHex
            "468d291a369299c696db4fadb5707a91babf4025c17a13c0", //expectedFinalPtHex
            "f59ccbd4f12cbc9444c307539f2c099e11deec3a2f75d83c", //expectedFinalCtHex
            "7dfa4ee940f16032",                                 //expectedIV1,
            "a9571b5cc0837d74",                                 //expectedIV2
            "c2580be797042163",                                 //expectedIV3
            TestName = "MCT Encrypt 400 rounds"
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
            "f79132292640a4a17a3864daf8d589cdf7f1d568d93791d0", //startingKeyHex
            "ec2437b92b61d2386fed3f57934337bfc809e623df818f36", //startingCtHex
            "2f30af1dc1fff479",                                 //startingIV
            1,                                                  //numberOfRounds
            "f79132292640a4a17a3864daf8d589cdf7f1d568d93791d0", //expectedFinalKeyHex
            "ec2437b92b61d2386fed3f57934337bfc809e623df818f36", //expectedFinalCtHex
            "19e01ac10228736e3cbb9db568044a521a032100f845afb5", //expectedFinalPtHex
            "2f30af1dc1fff479",                                 //expectedIV1,
            "84860473175549ce",                                 //expectedIV2
            "d9db59c86caa9f23",                                 //expectedIV3
            TestName = "MCT Decrypt 001 rounds"
        )]


        [TestCase(
            "f79132292640a4a17a3864daf8d589cdf7f1d568d93791d0", //startingKeyHex
            "ec2437b92b61d2386fed3f57934337bfc809e623df818f36", //startingCtHex
            "2f30af1dc1fff479",                                 //startingIV
            2,                                                  //numberOfRounds
            "ef7029e92568d6ce92761f835229cdf73740c24946493452", //expectedFinalKeyHex
            "19e01ac10228736e3cbb9db568044a521a032100f845afb5", //expectedFinalCtHex
            "ae6cf904cfb8b7cf79733f2ae80df644f008144d96ec6af3", //expectedFinalPtHex
            "6fa5477bbba23398",                                 //expectedIV1,
            "e94f7a59aafd453b",                                 //expectedIV2
            "380648d36a585736",                                 //expectedIV3
            TestName = "MCT Decrypt 002 rounds"
        )]


        [TestCase(
            "f79132292640a4a17a3864daf8d589cdf7f1d568d93791d0", //startingKeyHex
            "ec2437b92b61d2386fed3f57934337bfc809e623df818f36", //startingCtHex
            "2f30af1dc1fff479",                                 //startingIV
            10,                                                 //numberOfRounds
            "75e6768608a79db01ad6201ffb4a25e929e5c492463ead46", //expectedFinalKeyHex
            "1c906ec7bbbaf8927c6df542be33cfe9fa6285eca8919a9d", //expectedFinalCtHex
            "f36fb82653e3a293087c67352cb1b14f2f0c6f01f13f8568", //expectedFinalPtHex
            "3e824e2cd53d1e2d",                                 //expectedIV1,
            "16113ec3e9e6c8f0",                                 //expectedIV2
            "626c264b41f20bdd",                                 //expectedIV3
            TestName = "MCT Decrypt 010 rounds"
        )]

        [TestCase(
            "f79132292640a4a17a3864daf8d589cdf7f1d568d93791d0", //startingKeyHex
            "ec2437b92b61d2386fed3f57934337bfc809e623df818f36", //startingCtHex
            "2f30af1dc1fff479",                                 //startingIV
            400,                                                //numberOfRounds
            "7fe93bbcd0b0fb896d98a7ef8f64b0e6733297d92a9e57da", //expectedFinalKeyHex
            "2e238975011ef37583da0a3d2aa3a209a4ab75d540f02d3f", //expectedFinalCtHex
            "d79f5f1c7e6a8bebbc0e7ab43a13466e14771f2ecc14be18", //expectedFinalPtHex
            "b007ef3aa18886d5",                                 //expectedIV1,
            "5947f6ac5a6b9e09",                                 //expectedIV2
            "1c5c844fe5d34eee",                                 //expectedIV3
            TestName = "MCT Decrypt 400 rounds"
        )]

        public void ShouldMonteCarloTestDecrypt(
            string startingKeyHex,
            string startingCtHex,
            string startingIVHex,
            int numberOfRounds,
            string expectedFinalKeyHex,
            string expectedFinalCtHex,
            string expectedFinalPtHex,
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
