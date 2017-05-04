using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_CBC;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CBC.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMMTDecryptTests
    {
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), new TdesCbc());
            var result = subject.Generate(new TestGroup { Function = "decrypt", NumberOfKeys = 3 }, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldHaveProperNumberOfTestCasesToGenerate()
        {
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), new TdesCbc());
            Assert.AreEqual(10, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        public void ShouldGenerateProperlySizedCipherTextForEachGenerateCall()
        {
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), new TdesCbc());
            for (int caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(new TestGroup { Function = "decrypt", NumberOfKeys = 3 }, false);
                Assume.That(result != null);
                Assume.That(result.Success);
                var testCase = (TestCase)result.TestCase;
                Assert.AreEqual((caseIdx + 1) * 8, testCase.CipherText.ToBytes().Length);
            }

        }

        [Test]
        public void ShouldReturnAnErrorIfAnDecryptionFails()
        {
            var algo = new Mock<ITDES_CBC>();
            algo.Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
               .Returns(new EncryptionResult("I Failed to decrypt"));
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), algo.Object);
            var result = subject.Generate(new TestGroup { Function = "decrypt", NumberOfKeys = 3 }, false);
            Assert.IsFalse(result.Success);


        }


        [Test]
        public void GeneratedPlainTextShouldDecryptBackToPlainText()
        {
            var algo = new TdesCbc();
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), algo);
            var testGroup = new TestGroup()
            {
                Function = "decrypt",
                NumberOfKeys = 3
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
                var result = algo.BlockDecrypt(testCase.Key, testCase.CipherText, testCase.Iv);

                Assert.AreEqual(testCase.PlainText, result.PlainText);
            }
        }
    }
}
