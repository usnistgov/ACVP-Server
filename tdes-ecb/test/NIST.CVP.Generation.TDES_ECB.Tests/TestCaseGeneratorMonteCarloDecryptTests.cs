using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NLog;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMonteCarloDecryptTests
    {

        [OneTimeSetUp]
        public void Setup()
        {
            Utilities.ConfigureLogging("TDES_ECB", true);
        }
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var subject = new TestCaseGeneratorMonteCarloDecrypt(new Random800_90(), new TdesEcb(), new MonteCarloKeyMaker(), false);
            var result = subject.Generate(new TestGroup {Function = "decrypt", NumberOfKeys = 3}, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(false, 400)]
        [TestCase(true, 3)]
        public void ShouldHaveProperNumberOfTestCasesToGenerate(bool isSample, int expected)
        {
           
            var subject = new TestCaseGeneratorMonteCarloDecrypt(new Random800_90(), new TdesEcb(), new MonteCarloKeyMaker(), isSample);
            Assert.AreEqual(expected, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        [TestCase("15fe6b3e6d37e6d9046123f1439d4f8a15fe6b3e6d37e6d9", "28f6c5f0a2b12f54", "ff363f1f14478890", 2)]
        [TestCase("d3bf80986b15618676e0e30e2a736ee9babc1032dc5b5d1c", "43a9cbb4c4284b85", "133b4b6f3aa99cf1", 3)]
        [TestCase("9dd36eadf280c898d015c4ba578f7625021fc20e791538f8", "2f95fbd4421031b8", "0dc425805dbde39b", 3)]
        public void ShouldGenerateProperPlainTextForSuppliedCase(string key, string plainText, string cipherText, int numberOfKeys)
        {
            var subject = new TestCaseGeneratorMonteCarloDecrypt(new Random800_90(), new TdesEcb(), new MonteCarloKeyMaker(), false);
            var tCase = new TestCase(key, null, cipherText);
            var result = subject.Generate(new TestGroup { Function = "decrypt", NumberOfKeys = numberOfKeys},tCase);
            Assume.That(result != null);
            Assume.That(result.Success);
            var resultTestCase = (TestCase) result.TestCase;
            ThisLogger.Debug(resultTestCase.PlainText.ToHex());
            Assert.AreEqual(new BitString(plainText), resultTestCase.PlainText);
        }

        [Test]
        [TestCase("d3bf80986b15618676e0e30e2a736ee9babc1032dc5b5d1c", "43a9cbb4c4284b85", "133b4b6f3aa99cf1", 0)]
        [TestCase("d3bf80986b15618676e0e30e2a736ee9babc1032dc5b5d1c", "0dc425805dbde39b", "133b4b6f3aa99cf1", 1)]
        [TestCase("d3bf80986b15618676e0e30e2a736ee9babc1032dc5b5d1c", "6d7575db06deab95", "133b4b6f3aa99cf1", 10)]
        //[TestCase("d3bf80986b15618676e0e30e2a736ee9babc1032dc5b5d1c", "e720eff0c4443175", "133b4b6f3aa99cf1", 399)]
        public void ShouldHaveTheProperPlainTextForTheGivenCase(string key, string plainText, string cipherText, int targetCaseId)
        {
            var seedCase = new TestCase(key, null, cipherText);
            var subject = new TestCaseGeneratorMonteCarloDecrypt(new Random800_90(), new TdesEcb(), new MonteCarloKeyMaker(),seedCase);
            TestCaseGenerateResponse result = null;
            for (int iCaseId = 0; iCaseId <= targetCaseId; iCaseId++)
            {
                result = subject.Generate(new TestGroup {Function = "decrypt", NumberOfKeys = 3}, false);
            }
            Assert.IsNotNull(result);
            var resultTestCase = (TestCase)result.TestCase;
            Assert.AreEqual(new BitString(plainText).ToHex(), resultTestCase.PlainText.ToHex());
        }

        [Test]
        [TestCase("d3bf80986b15618676e0e30e2a736ee9babc1032dc5b5d1c", "43a9cbb4c4284b85", "133b4b6f3aa99cf1", 10)]
        [TestCase("d3bf80986b15618676e0e30e2a736ee9babc1032dc5b5d1c", "0dc425805dbde39b", "43a9cbb4c4284b85", 0)]
        [TestCase("d3bf80986b15618676e0e30e2a736ee9babc1032dc5b5d1c", "6d7575db06deab95", "17edee2156a8ca87", 1)]
        public void ShouldNotHaveTheProperPlainextForADifferentCase(string key, string plainText, string cipherText, int targetCaseId)
        {
            var seedCase = new TestCase(key, null, cipherText);
            var subject = new TestCaseGeneratorMonteCarloDecrypt(new Random800_90(), new TdesEcb(), new MonteCarloKeyMaker(), seedCase);
            TestCaseGenerateResponse result = null;
            for (int iCaseId = 0; iCaseId <= targetCaseId; iCaseId++)
            {
                result = subject.Generate(new TestGroup { Function = "decrypt", NumberOfKeys = 3 }, false);

            }
            Assert.IsNotNull(result);
            var resultTestCase = (TestCase)result.TestCase;
            Assert.AreNotEqual(new BitString(cipherText), resultTestCase.CipherText);
        }

        [Test]
        public void ShouldReturnAnErrorIfADecryptionFails()
        {
            var seedCase = new TestCase("435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", "435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", null);
            var algo = new Mock<ITDES_ECB>();
             algo.Setup(s => s.BlockDecrypt(It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new DecryptionResult("I Failed to decrypt"));
            var subject = new TestCaseGeneratorMonteCarloDecrypt(new Random800_90(), algo.Object, new MonteCarloKeyMaker(), seedCase);
            var result = subject.Generate(new TestGroup {Function = "decrypt", NumberOfKeys = 3}, false);
            Assert.IsFalse(result.Success);


        }



        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
