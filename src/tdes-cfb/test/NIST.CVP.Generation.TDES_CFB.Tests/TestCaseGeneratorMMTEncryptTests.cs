using Moq;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_CFB;
using NIST.CVP.Math;
using NUnit.Framework;


namespace NIST.CVP.Generation.TDES_CFB.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMMTEncryptTests
    {
        [Test]
        [TestCase(AlgoMode.TDES_CFB1)]
        [TestCase(AlgoMode.TDES_CFB8)]
        [TestCase(AlgoMode.TDES_CFB64)]
        public void ShouldSuccessfullyGenerate(AlgoMode algo)
        {
            var modeOfOperation = ModeFactory.GetMode(algo);
            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), modeOfOperation);
            var result = subject.Generate(new TestGroup { Function = "encrypt", KeyingOption = 1 }, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1)]
        [TestCase(AlgoMode.TDES_CFB8)]
        [TestCase(AlgoMode.TDES_CFB64)]
        public void ShouldHaveProperNumberOfTestCasesToGenerate(AlgoMode algo)
        {
            var modeOfOperation = ModeFactory.GetMode(algo);
            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), modeOfOperation);
            Assert.AreEqual(10, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1, 1)]
        [TestCase(AlgoMode.TDES_CFB8, 8)]
        [TestCase(AlgoMode.TDES_CFB64, 64)]
        public void ShouldGenerateProperlySizedPlainTextForEachGenerateCall(AlgoMode algo, int shift)
        {
            var modeOfOperation = ModeFactory.GetMode(algo);
            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), modeOfOperation);
            for (int caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(new TestGroup { Function = "encrypt", KeyingOption = 1 }, false);
                Assume.That(result != null);
                Assume.That(result.Success);
                var testCase = (TestCase)result.TestCase;
                Assert.AreEqual((caseIdx + 1) * shift, testCase.PlainText.BitLength);
            }

        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1)]
        [TestCase(AlgoMode.TDES_CFB8)]
        [TestCase(AlgoMode.TDES_CFB64)]
        public void ShouldReturnAnErrorIfAnEncryptionFails(AlgoMode algo)
        {
            var mockModeOfOperation = new Mock<ICFBMode>();
            mockModeOfOperation.Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new SymmetricCipherResult("I Failed to encrypt"));
            mockModeOfOperation.SetupGet(s => s.Algo).Returns(algo);

            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), mockModeOfOperation.Object);
            var result = subject.Generate(new TestGroup { Function = "encrypt", KeyingOption = 1 }, false);
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1)]
        [TestCase(AlgoMode.TDES_CFB8)]
        [TestCase(AlgoMode.TDES_CFB64)]
        public void GeneratedCipherTextShouldEncryptBackToCipherText(AlgoMode algo)
        {
            var modeOfOperation = ModeFactory.GetMode(algo);
            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), modeOfOperation);
            var testGroup = new TestGroup()
            {
                Function = "encrypt",
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
                var result = modeOfOperation.BlockEncrypt(testCase.Keys, testCase.Iv, testCase.PlainText);
                Assert.AreEqual(testCase.CipherText, result.Result);
            }
        }
    }
}
