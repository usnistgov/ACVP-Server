using Moq;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_CBCI;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CBCI.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMMTDecryptTests
    {
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), new TdesCbci());
            var result = subject.Generate(new TestGroup { Function = "decrypt", KeyingOption = 1 }, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldHaveProperNumberOfTestCasesToGenerate()
        {
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), new TdesCbci());
            Assert.AreEqual(10, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        public void ShouldGenerateProperlySizedCipherTextForEachGenerateCall()
        {
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), new TdesCbci());
            for (int caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(new TestGroup { Function = "decrypt", KeyingOption = 1 }, false);
                Assume.That(result != null);
                Assume.That(result.Success);
                var testCase = (TestCase)result.TestCase;
                Assert.AreEqual((caseIdx + 1) * 8 * 3, testCase.CipherText.ToBytes().Length);
            }

        }

        [Test]
        public void ShouldReturnAnErrorIfAnDecryptionFails()
        {
            var algo = new Mock<ITDES_CBCI>();
            algo.Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new EncryptionResultWithIv("I Failed to decrypt"));
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), algo.Object);
            var result = subject.Generate(new TestGroup { Function = "decrypt", KeyingOption = 1 }, false);
            Assert.IsFalse(result.Success);


        }


        [Test]
        public void GeneratedPlainTextShouldDecryptBackToPlainText()
        {
            var algo = new TdesCbci();
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), algo);
            var testGroup = new TestGroup()
            {
                Function = "decrypt",
                KeyingOption = 1
            };

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(testGroup, false);
                Assume.That(result.Success);
                testGroup.Tests.Add(result.TestCase);
            }

            Assume.That(testGroup.Tests.Count > 0);

            foreach (TestCase testCase in testGroup.Tests)
            {
                var result = algo.BlockDecrypt(testCase.Keys, testCase.IV1, testCase.CipherText);

                Assert.AreEqual(testCase.PlainText, result.PlainText);
            }
        }
    }
}
