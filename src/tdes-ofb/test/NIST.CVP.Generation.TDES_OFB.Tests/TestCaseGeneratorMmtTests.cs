using Moq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NUnit.Framework;
using System;

namespace NIST.CVP.Generation.TDES_OFB.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMmtTests
    {
        private Mock<IOracle> _oracle;
        private TestCaseGeneratorMmt _subject;

        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();
            _oracle
                .Setup(s => s.GetTdesCase(It.IsAny<TdesParameters>()))
                .Returns(() => new TdesResult());
            _subject = new TestCaseGeneratorMmt(
                _oracle.Object
            );
        }

        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var result = _subject.Generate(new TestGroup { Function = "encrypt", NumberOfKeys = 3 }, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldHaveProperNumberOfTestCasesToGenerate()
        {
            Assert.AreEqual(10, _subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        public void ShouldReturnAnErrorIfAnEncryptionFails()
        {
            _oracle
                .Setup(s => s.GetTdesCase(It.IsAny<TdesParameters>()))
                .Throws(new Exception());
            var result = _subject.Generate(new TestGroup { Function = "encrypt", NumberOfKeys = 3 }, false);
            Assert.IsFalse(result.Success);
        }
    }
}
