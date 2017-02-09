using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.TDES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NLog;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMonteCarloEncryptTests
    {

        [OneTimeSetUp]
        public void Setup()
        {
            Utilities.ConfigureLogging("TDES_ECB", true);
        }
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var subject = new TestCaseGeneratorMonteCarloEncrypt(new Random800_90(), new TdesEcb(), new MonteCarloKeyMaker(), false);
            var result = subject.Generate(new TestGroup {Function = "encrypt", NumberOfKeys = 3}, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(false, 400)]
        [TestCase(true, 3)]
        public void ShouldHaveProperNumberOfTestCasesToGenerate(bool isSample, int expected)
        {
           
            var subject = new TestCaseGeneratorMonteCarloEncrypt(new Random800_90(), new TdesEcb(), new MonteCarloKeyMaker(), isSample);
            Assert.AreEqual(expected, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        [TestCase("435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", "e2bed01a66f61b9c", "644fca5969d6a540")]
        [TestCase("26134aae37a7fd2f97f75723833e91eac8cea49280792554", "644fca5969d6a540", "0d75b6a7e7c6ae96")]
        public void ShouldGenerateProperCipherTextForSuppliedCase(string key, string plainText, string cipherText)
        {
            var subject = new TestCaseGeneratorMonteCarloEncrypt(new Random800_90(), new TdesEcb(), new MonteCarloKeyMaker(), false);
            var tCase = new TestCase(key, plainText, null);
            var result = subject.Generate(new TestGroup { Function = "encrypt", NumberOfKeys = 3 },tCase);
            Assume.That(result != null);
            Assume.That(result.Success);
            var resultTestCase = (TestCase) result.TestCase;
            ThisLogger.Debug(resultTestCase.CipherText.ToHex());
            Assert.AreEqual(new BitString(cipherText), resultTestCase.CipherText);
        }

        [Test]
        [TestCase("435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", "e2bed01a66f61b9c", "644fca5969d6a540", 0)]
        [TestCase("435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", "e2bed01a66f61b9c", "0d75b6a7e7c6ae96", 1)]
        [TestCase("435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", "e2bed01a66f61b9c", "c800f437b70c0bc6", 10)]
        //[TestCase("435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", "e2bed01a66f61b9c", "b8c99c4b302a5151", 20)]
        //[TestCase("435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", "e2bed01a66f61b9c", "3f7d3f3757d85ff1", 99)]
        //[TestCase("435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", "e2bed01a66f61b9c", "112fef67a4970972", 100)]
        //[TestCase("435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", "e2bed01a66f61b9c", "313bc17f647259d9", 200)]
        //[TestCase("435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", "e2bed01a66f61b9c", "2a869bd2757f3697", 300)]
        //[TestCase("435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", "e2bed01a66f61b9c", "e60bff62c37ea3f9", 399)]
        public void ShouldHaveTheProperCipherTextForTheGivenCase(string key, string plainText, string cipherText, int targetCaseId)
        {
            var seedCase = new TestCase(key, plainText, null);
            var subject = new TestCaseGeneratorMonteCarloEncrypt(new Random800_90(), new TdesEcb(), new MonteCarloKeyMaker(),seedCase);
            TestCaseGenerateResponse result = null;
            for (int iCaseId = 0; iCaseId <= targetCaseId; iCaseId++)
            {
                result = subject.Generate(new TestGroup {Function = "encrypt", NumberOfKeys = 3}, false);

            }
            Assert.IsNotNull(result);
            var resultTestCase = (TestCase)result.TestCase;
            Assert.AreEqual(new BitString(cipherText), resultTestCase.CipherText);
        }

        [Test]
        [TestCase("435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", "e2bed01a66f61b9c", "c800f437b70c0bc6", 0)]
        [TestCase("435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", "e2bed01a66f61b9c", "644fca5969d6a540", 1)]
        [TestCase("435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", "e2bed01a66f61b9c", "0d75b6a7e7c6ae96", 10)]
        public void ShouldNotHaveTheProperCipherTextForADifferentCase(string key, string plainText, string cipherText, int targetCaseId)
        {
            var seedCase = new TestCase(key, plainText, null);
            var subject = new TestCaseGeneratorMonteCarloEncrypt(new Random800_90(), new TdesEcb(), new MonteCarloKeyMaker(), seedCase);
            TestCaseGenerateResponse result = null;
            for (int iCaseId = 0; iCaseId <= targetCaseId; iCaseId++)
            {
                result = subject.Generate(new TestGroup { Function = "encrypt", NumberOfKeys = 3 }, false);

            }
            Assert.IsNotNull(result);
            var resultTestCase = (TestCase)result.TestCase;
            Assert.AreNotEqual(new BitString(cipherText), resultTestCase.CipherText);
        }

        [Test]
        public void ShouldReturnAnErrorIfAnEncryptionFails()
        {
            var seedCase = new TestCase("435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", "435d80f75e70586eb579f4da07206e5d3ed54aa70d68dc57", null);
            var algo = new Mock<ITDES_ECB>();
             algo.Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new EncryptionResult("I Failed to encrypt"));
            var subject = new TestCaseGeneratorMonteCarloEncrypt(new Random800_90(), algo.Object, new MonteCarloKeyMaker(), seedCase);
            var result = subject.Generate(new TestGroup {Function = "encrypt", NumberOfKeys = 3}, false);
            Assert.IsFalse(result.Success);


        }



        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
