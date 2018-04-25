using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_OFB;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_OFB.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMMTEncryptTests
    {
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), new TdesOfb());
            var result = subject.Generate(new TestGroup { Function = "encrypt", NumberOfKeys = 3 }, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldHaveProperNumberOfTestCasesToGenerate()
        {
            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), new TdesOfb());
            Assert.AreEqual(10, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        public void ShouldGenerateProperlySizedPlainTextForEachGenerateCall()
        {
            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), new TdesOfb());
            for (int caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(new TestGroup { Function = "encrypt", NumberOfKeys = 3 }, false);
                Assume.That(result != null);
                Assume.That(result.Success);
                var testCase = result.TestCase;
                Assert.AreEqual((caseIdx + 1) * 8, testCase.PlainText.ToBytes().Length);
            }
        }

        [Test]
        public void ShouldReturnAnErrorIfAnEncryptionFails()
        {
            var algo = new Mock<ITDES_OFB>();
            algo.Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new SymmetricCipherResult("I Failed to encrypt"));
            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), algo.Object);
            var result = subject.Generate(new TestGroup { Function = "encrypt", NumberOfKeys = 3 }, false);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void GeneratedCipherTextShouldEncryptBackToCipherText()
        {
            var algo = new TdesOfb();
            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), algo);
            var testGroup = new TestGroup
            {
                Function = "encrypt",
                NumberOfKeys = 3
            };

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(testGroup, false);
                Assume.That(result.Success);
                testGroup.Tests.Add(result.TestCase);
            }

            Assume.That(testGroup.Tests.Count > 0);

            foreach (var testCase in testGroup.Tests)
            {
                var result = algo.BlockEncrypt(testCase.Key, testCase.PlainText, testCase.Iv);
                Assert.AreEqual(testCase.CipherText, result.Result);
            }
        }
    }
}
