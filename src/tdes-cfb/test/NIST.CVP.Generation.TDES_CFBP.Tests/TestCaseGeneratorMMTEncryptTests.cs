using Moq;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES_CFBP;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFBP.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMMTEncryptTests
    {
        [Test]
        [TestCase(AlgoMode.TDES_CFBP1)]
        [TestCase(AlgoMode.TDES_CFBP8)]
        [TestCase(AlgoMode.TDES_CFBP64)]
        public void ShouldSuccessfullyGenerate(AlgoMode algo)
        {
            var modeOfOperation = ModeFactory.GetMode(algo);
            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), modeOfOperation);
            var result = subject.Generate(new TestGroup { Function = "encrypt", KeyingOption = 1 }, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFBP1)]
        [TestCase(AlgoMode.TDES_CFBP8)]
        [TestCase(AlgoMode.TDES_CFBP64)]
        public void ShouldHaveProperNumberOfTestCasesToGenerate(AlgoMode algo)
        {
            var modeOfOperation = ModeFactory.GetMode(algo);
            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), modeOfOperation);
            Assert.AreEqual(10, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFBP1, 1)]
        [TestCase(AlgoMode.TDES_CFBP8, 8)]
        [TestCase(AlgoMode.TDES_CFBP64, 64)]
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
        [TestCase(AlgoMode.TDES_CFBP1)]
        [TestCase(AlgoMode.TDES_CFBP8)]
        [TestCase(AlgoMode.TDES_CFBP64)]
        public void ShouldReturnAnErrorIfAnEncryptionFails(AlgoMode algo)
        {
            var modeOfOperation = ModeFactory.GetMode(algo);
            //var mockAlgo = new Mock<ITDES_CFB>();
            //mockAlgo.Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
            //    .Returns(new EncryptionResult("I Failed to encrypt"));

            var mockModeOfOperation = new Mock<ICFBPMode>();
            mockModeOfOperation.Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), false))
                .Returns(new SymmetricCipherWithIvResult("I Failed to encrypt"));
            mockModeOfOperation.SetupProperty(x => x.Algo, AlgoMode.TDES_CFBP1);

            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), mockModeOfOperation.Object);
            var result = subject.Generate(new TestGroup { Function = "encrypt", KeyingOption = 1 }, false);
            Assert.IsFalse(result.Success);


        }

        [Test]
        [TestCase(AlgoMode.TDES_CFBP1)]
        [TestCase(AlgoMode.TDES_CFBP8)]
        [TestCase(AlgoMode.TDES_CFBP64)]
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
                var result = modeOfOperation.BlockEncrypt(testCase.Keys, testCase.IV1, testCase.PlainText);
                Assert.AreEqual(testCase.CipherText, result.Result);
            }
        }
    }
}
