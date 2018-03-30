using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_ECB;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorMMTEncryptTests
    {
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), new Crypto.TDES_ECB.TDES_ECB());
            var result = subject.Generate(new TestGroup {Function = "encrypt", KeyingOption = 1}, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldHaveProperNumberOfTestCasesToGenerate()
        {
            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), new Crypto.TDES_ECB.TDES_ECB());
            Assert.AreEqual(10, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        public void ShouldGenerateProperlySizedPlainTextForEachGenerateCall()
        {
            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), new Crypto.TDES_ECB.TDES_ECB());
            for (int caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(new TestGroup { Function = "encrypt", KeyingOption = 1 }, false);
                Assume.That(result != null);
                Assume.That(result.Success);
                var testCase = (TestCase) result.TestCase;
                Assert.AreEqual((caseIdx + 1) * 8, testCase.PlainText.ToBytes().Length );
            }
            
        }

        [Test]
        public void ShouldReturnAnErrorIfAnEncryptionFails()
        { 
            var algo = new Mock<ITDES_ECB>();
            algo.Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false))
               .Returns(new SymmetricCipherResult("I Failed to encrypt"));
            var subject = new TestCaseGeneratorMMTEncrypt(new Random800_90(), algo.Object);
            var result = subject.Generate(new TestGroup { Function = "encrypt", KeyingOption = 1 }, false);
            Assert.IsFalse(result.Success);


        }
    }
}
