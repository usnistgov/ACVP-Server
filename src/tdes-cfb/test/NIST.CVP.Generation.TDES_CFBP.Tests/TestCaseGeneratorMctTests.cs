using System;
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
            var mctResult = new MctResult<TdesResultWithIvs>();
            mctResult.Results.Add(new TdesResultWithIvs()
            {
                Key = new BitString(192),
                Iv = new BitString(64),
                Iv1 = new BitString(64),
                Iv2 = new BitString(64),
                Iv3 = new BitString(64),
                PlainText = new BitString(64),
                CipherText = new BitString(64)
            });

            _mockOracle = new Mock<IOracle>();
            _mockOracle
                .Setup(s => s.GetTdesMctWithIvsCase(It.IsAny<TdesParameters>()))
                .Returns(mctResult);
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFBP1)]
        [TestCase(AlgoMode.TDES_CFBP8)]
        [TestCase(AlgoMode.TDES_CFBP64)]
        public void ShouldCallAlgoFromIsSampleMethod(AlgoMode algoMode)
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1,
                AlgoMode = algoMode
            };
            _subject = new TestCaseGeneratorMct(_mockOracle.Object, testGroup);
            _subject.Generate(testGroup, false);

            _mockOracle.Verify(v => v.GetTdesMctWithIvsCase(It.IsAny<TdesParameters>()));
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFBP1)]
        [TestCase(AlgoMode.TDES_CFBP8)]
        [TestCase(AlgoMode.TDES_CFBP64)]
        public void ShouldReturnErrorMessageIfAlgoFailsWithException(AlgoMode algoMode)
        {
            string errorMessage = "something bad happened! oh noes!";
            _mockOracle
                .Setup(s => s.GetTdesMctWithIvsCase(It.IsAny<TdesParameters>()))
                .Throws(new Exception(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1,
                AlgoMode = algoMode
            };
            _subject = new TestCaseGeneratorMct(_mockOracle.Object, testGroup);
            
            var result = _subject.Generate(testGroup, false);

            Assert.IsFalse(result.Success, nameof(result.Success));
        }
    }
}
