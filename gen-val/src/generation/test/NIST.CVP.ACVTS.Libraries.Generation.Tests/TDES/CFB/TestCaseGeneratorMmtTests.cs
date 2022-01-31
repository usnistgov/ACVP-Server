using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CFB.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CFB
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
        [TestCase(AlgoMode.TDES_CFB1_v1_0)]
        [TestCase(AlgoMode.TDES_CFB8_v1_0)]
        [TestCase(AlgoMode.TDES_CFB64_v1_0)]
        public async Task ShouldSuccessfullyGenerate(AlgoMode algo)
        {
            var group = new TestGroup { Function = "encrypt", KeyingOption = 1, AlgoMode = algo };
            _subject = new TestCaseGeneratorMmt(_oracle.Object, group);

            var result = await _subject.GenerateAsync(group, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1_v1_0)]
        [TestCase(AlgoMode.TDES_CFB8_v1_0)]
        [TestCase(AlgoMode.TDES_CFB64_v1_0)]
        public void ShouldHaveProperNumberOfTestCasesToGenerate(AlgoMode algo)
        {
            var group = new TestGroup { Function = "encrypt", KeyingOption = 1, AlgoMode = algo };
            _subject = new TestCaseGeneratorMmt(_oracle.Object, group);
            Assert.AreEqual(10, _subject.NumberOfTestCasesToGenerate);
        }
    }
}
