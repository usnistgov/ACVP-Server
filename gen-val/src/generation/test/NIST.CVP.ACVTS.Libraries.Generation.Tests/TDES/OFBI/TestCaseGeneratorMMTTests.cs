using System;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_OFBI.v1_0;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.OFBI
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorMMTTests
    {
        private Mock<IOracle> _oracle;
        private TestCaseGeneratorMmt _subject;

        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();
            _oracle
                .Setup(s => s.GetTdesCaseAsync(It.IsAny<TdesParameters>()))
                .Returns(() => Task.FromResult(new TdesResult()));
            _subject = new TestCaseGeneratorMmt(
                _oracle.Object
            );
        }

        [Test]
        public async Task ShouldSuccessfullyGenerate()
        {
            var result = await _subject.GenerateAsync(new TestGroup { Function = "encrypt", KeyingOption = 1 }, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldHaveProperNumberOfTestCasesToGenerate()
        {
            Assert.AreEqual(10, _subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        public async Task ShouldReturnAnErrorIfAnEncryptionFails()
        {
            _oracle
                .Setup(s => s.GetTdesCaseAsync(It.IsAny<TdesParameters>()))
                .Throws(new Exception());
            var result = await _subject.GenerateAsync(new TestGroup { Function = "encrypt", KeyingOption = 1 }, false);
            Assert.IsFalse(result.Success);
        }
    }
}
