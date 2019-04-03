using System;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFBP.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMctTests
    {
        private Mock<IOracle> _mockOracle;
        private TestCaseGeneratorMct _subject;
        
        [SetUp]
        public void Setup()
        {
            var mctResult = Task.FromResult(new MctResult<TdesResult>());
            mctResult.Result.Results.Add(new TdesResult()
            {
                Key = new BitString(192),
                Iv = new BitString(64),
                PlainText = new BitString(64),
                CipherText = new BitString(64)
            });

            _mockOracle = new Mock<IOracle>();
            _mockOracle
                .Setup(s => s.GetTdesMctCaseAsync(It.IsAny<TdesParameters>()))
                .Returns(mctResult);
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFBP1_v1_0)]
        [TestCase(AlgoMode.TDES_CFBP8_v1_0)]
        [TestCase(AlgoMode.TDES_CFBP64_v1_0)]
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
        [TestCase(AlgoMode.TDES_CFBP1_v1_0)]
        [TestCase(AlgoMode.TDES_CFBP8_v1_0)]
        [TestCase(AlgoMode.TDES_CFBP64_v1_0)]
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
