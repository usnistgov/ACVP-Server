using Moq;
using NIST.CVP.Crypto.Core;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_CFB;
using NIST.CVP.Math;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Algo = NIST.CVP.Crypto.TDES_CFB.Algo;

namespace NIST.CVP.Generation.TDES_CFB.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMMTDecryptTests
    {


        [Test]
        [TestCase(Algo.TDES_CFB1)]
        [TestCase(Algo.TDES_CFB8)]
        [TestCase(Algo.TDES_CFB64)]
        public void ShouldSuccessfullyGenerate(Algo algo)
        {
            var modeOfOperation = ModeFactory.GetMode(algo);
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), modeOfOperation);
            var result = subject.Generate(new TestGroup { Function = "decrypt", KeyingOption = 1 }, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(Algo.TDES_CFB1)]
        [TestCase(Algo.TDES_CFB8)]
        [TestCase(Algo.TDES_CFB64)]
        public void ShouldHaveProperNumberOfTestCasesToGenerate(Algo algo)
        {
            var modeOfOperation = ModeFactory.GetMode(algo);
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), modeOfOperation);
            Assert.AreEqual(10, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        [TestCase(Algo.TDES_CFB1, 1)]
        [TestCase(Algo.TDES_CFB8, 8)]
        [TestCase(Algo.TDES_CFB64, 64)]
        public void ShouldGenerateProperlySizedCipherTextForEachGenerateCall(Algo algo, int shift)
        {
            var modeOfOperation = ModeFactory.GetMode(algo);
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), modeOfOperation);
            for (int caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(new TestGroup { Function = "decrypt", KeyingOption = 1 }, false);
                Assume.That(result != null);
                Assume.That(result.Success);
                var testCase = (TestCase)result.TestCase;
                Assert.AreEqual((caseIdx + 1) * shift, testCase.CipherText.BitLength);
            }

        }

        [Test]
        [TestCase(Algo.TDES_CFB1)]
        [TestCase(Algo.TDES_CFB8)]
        [TestCase(Algo.TDES_CFB64)]
        public void ShouldReturnAnErrorIfAnDecryptionFails(Algo algo)
        {
            //var mockAlgo = new Mock<ITDES_CFB>();
            //mockAlgo.Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
            //    .Returns(new EncryptionResult("I Failed to decrypt"));

            var mockModeOfOperation = new Mock<IModeOfOperation>();
            mockModeOfOperation.Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                            .Returns(new EncryptionResult("I Failed to decrypt"));

            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), mockModeOfOperation.Object);
            var result = subject.Generate(new TestGroup { Function = "decrypt", KeyingOption = 1 }, false);
            Assert.IsFalse(result.Success);


        }


        [Test]
        [TestCase(Algo.TDES_CFB1)]
        [TestCase(Algo.TDES_CFB8)]
        [TestCase(Algo.TDES_CFB64)]
        public void GeneratedPlainTextShouldDecryptBackToPlainText(Algo algo)
        {
            var modeOfOperation = ModeFactory.GetMode(algo);

            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), modeOfOperation);
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
                var result = modeOfOperation.BlockDecrypt(testCase.Key, testCase.Iv, testCase.CipherText);

                Assert.AreEqual(testCase.PlainText, result.PlainText);
            }
        }
    }
}
