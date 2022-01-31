using System;
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
    public class TestCaseGeneratorMctTests
    {
        private Mock<IOracle> _mockOracle;
        private TestCaseGeneratorMct _subject;

        [SetUp]
        public void Setup()
        {
            var mctResult = new MctResult<TdesResult>();
            mctResult.Results.Add(new TdesResult()
            {
                Key = new BitString(192),
                Iv = new BitString(64),
                PlainText = new BitString(64),
                CipherText = new BitString(64)
            });

            _mockOracle = new Mock<IOracle>();
            _mockOracle
                .Setup(s => s.GetTdesMctCaseAsync(It.IsAny<TdesParameters>()))
                .Returns(Task.FromResult(mctResult));
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1_v1_0)]
        [TestCase(AlgoMode.TDES_CFB8_v1_0)]
        [TestCase(AlgoMode.TDES_CFB64_v1_0)]
        public async Task ShouldCallAlgoFromIsSampleMethod(AlgoMode algoMode)
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1,
                AlgoMode = algoMode
            };
            _subject = new TestCaseGeneratorMct(_mockOracle.Object, testGroup);
            await _subject.GenerateAsync(testGroup, false);

            _mockOracle.Verify(v => v.GetTdesMctCaseAsync(It.IsAny<TdesParameters>()));
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1_v1_0)]
        [TestCase(AlgoMode.TDES_CFB8_v1_0)]
        [TestCase(AlgoMode.TDES_CFB64_v1_0)]
        public async Task ShouldReturnErrorMessageIfAlgoFailsWithException(AlgoMode algoMode)
        {
            string errorMessage = "something bad happened! oh noes!";
            _mockOracle
                .Setup(s => s.GetTdesMctCaseAsync(It.IsAny<TdesParameters>()))
                .Throws(new Exception(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1,
                AlgoMode = algoMode
            };
            _subject = new TestCaseGeneratorMct(_mockOracle.Object, testGroup);

            var result = await _subject.GenerateAsync(testGroup, false);

            Assert.IsFalse(result.Success, nameof(result.Success));
        }
    }
}
