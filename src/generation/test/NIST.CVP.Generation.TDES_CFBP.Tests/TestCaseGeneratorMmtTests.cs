using Moq;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.TDES_CFBP.v1_0;
using NIST.CVP.Math;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.TDES_CFBP.Tests
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
                .Setup(s => s.GetTdesCaseAsync(It.IsAny<TdesParameters>()))
                .Returns(Task.FromResult(new TdesResult()
                {
                    Key = new BitString(192),
                    Iv = new BitString(64),
                    PlainText = new BitString(64),
                    CipherText = new BitString(64)
                }));
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFBP1_v1_0)]
        [TestCase(AlgoMode.TDES_CFBP8_v1_0)]
        [TestCase(AlgoMode.TDES_CFBP64_v1_0)]
        public async Task ShouldSuccessfullyGenerate(AlgoMode algo)
        {
            var group = new TestGroup { Function = "encrypt", KeyingOption = 1, AlgoMode = algo };
            _subject = new TestCaseGeneratorMmt(_oracle.Object, group);
            
            var result = await _subject.GenerateAsync(group, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFBP1_v1_0)]
        [TestCase(AlgoMode.TDES_CFBP8_v1_0)]
        [TestCase(AlgoMode.TDES_CFBP64_v1_0)]
        public void ShouldHaveProperNumberOfTestCasesToGenerate(AlgoMode algo)
        {
            var group = new TestGroup { Function = "encrypt", KeyingOption = 1, AlgoMode = algo };
            _subject = new TestCaseGeneratorMmt(_oracle.Object, group);
            Assert.AreEqual(10, _subject.NumberOfTestCasesToGenerate);
        }
    }
}
