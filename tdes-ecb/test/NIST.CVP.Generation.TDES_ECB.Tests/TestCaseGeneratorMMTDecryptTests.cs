using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMMTDecryptTests
    {
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), new TdesEcb());
            var result = subject.Generate(new TestGroup {Function = "decrypt", NumberOfKeys = 3}, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldHaveProperNumberOfTestCasesToGenerate()
        {
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), new TdesEcb());
            Assert.AreEqual(10, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        public void ShouldGenerateProperlySizedCipherTextForEachGenerateCall()
        {
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), new TdesEcb());
            for (int caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(new TestGroup { Function = "decrypt", NumberOfKeys = 3 }, false);
                Assume.That(result != null);
                Assume.That(result.Success);
                var testCase = (TestCase) result.TestCase;
                Assert.AreEqual((caseIdx + 1) * 8, testCase.CipherText.ToBytes().Length );
            }
            
        }

        [Test]
        public void ShouldReturnAnErrorIfAnDecryptionFails()
        { 
            var algo = new Mock<ITDES_ECB>();
            algo.Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>()))
               .Returns(new EncryptionResult("I Failed to decrypt"));
            var subject = new TestCaseGeneratorMMTDecrypt(new Random800_90(), algo.Object);
            var result = subject.Generate(new TestGroup { Function = "decrypt", NumberOfKeys = 3 }, false);
            Assert.IsFalse(result.Success);


        }
    }
}
