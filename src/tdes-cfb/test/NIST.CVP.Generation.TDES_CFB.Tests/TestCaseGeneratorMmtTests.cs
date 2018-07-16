using Moq;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math;
using NUnit.Framework;


namespace NIST.CVP.Generation.TDES_CFB.Tests
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
                .Returns(new TdesResult()
                {
                    Key = new BitString(192),
                    Iv = new BitString(64),
                    PlainText = new BitString(64),
                    CipherText = new BitString(64)
                });
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1)]
        [TestCase(AlgoMode.TDES_CFB8)]
        [TestCase(AlgoMode.TDES_CFB64)]
        public void ShouldSuccessfullyGenerate(AlgoMode algo)
        {
            var group = new TestGroup { Function = "encrypt", KeyingOption = 1, AlgoMode = algo };
            _subject = new TestCaseGeneratorMmt(_oracle.Object, group);
            
            var result = _subject.Generate(group, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1)]
        [TestCase(AlgoMode.TDES_CFB8)]
        [TestCase(AlgoMode.TDES_CFB64)]
        public void ShouldHaveProperNumberOfTestCasesToGenerate(AlgoMode algo)
        {
            var group = new TestGroup { Function = "encrypt", KeyingOption = 1, AlgoMode = algo };
            _subject = new TestCaseGeneratorMmt(_oracle.Object, group);
            Assert.AreEqual(10, _subject.NumberOfTestCasesToGenerate);
        }
    }
}
