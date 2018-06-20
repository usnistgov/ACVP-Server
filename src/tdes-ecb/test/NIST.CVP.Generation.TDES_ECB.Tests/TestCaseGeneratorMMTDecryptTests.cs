using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorMMTDecryptTests
    {
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var algo = GetTdesMock();
            algo.Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherResult(new BitString("ABCD")));

            var subject = new TestCaseGeneratorMMTDecrypt(GetRandomMock().Object, algo.Object);
            var result = subject.Generate(new TestGroup {Function = "decrypt", KeyingOption = 1}, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldHaveProperNumberOfTestCasesToGenerate()
        {
            var subject = new TestCaseGeneratorMMTDecrypt(GetRandomMock().Object, GetTdesMock().Object);
            Assert.AreEqual(10, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        public void ShouldReturnAnErrorIfAnDecryptionFails()
        { 
            var algo = GetTdesMock();
            algo.Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
               .Returns(new SymmetricCipherResult("I Failed to decrypt"));

            var subject = new TestCaseGeneratorMMTDecrypt(GetRandomMock().Object, algo.Object);
            var result = subject.Generate(new TestGroup { Function = "decrypt", KeyingOption = 1 }, false);
            Assert.IsFalse(result.Success);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var mock = new Mock<IRandom800_90>();
            mock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("123412341234"));
            return mock;
        }

        private Mock<IModeBlockCipher<SymmetricCipherResult>> GetTdesMock()
        {
            return new Mock<IModeBlockCipher<SymmetricCipherResult>>();
        }
    }
}
