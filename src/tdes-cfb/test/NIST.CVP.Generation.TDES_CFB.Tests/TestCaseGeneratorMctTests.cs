using Moq;
using NIST.CVP.Math;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_CFB.Tests
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
                .Setup(s => s.GetTdesMctCase(It.IsAny<TdesParameters>()))
                .Returns(mctResult);
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1)]
        [TestCase(AlgoMode.TDES_CFB8)]
        [TestCase(AlgoMode.TDES_CFB64)]
        public void ShouldCallAlgoFromIsSampleMethod(AlgoMode algoMode)
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1,
                AlgoMode = algoMode
            };
            _subject = new TestCaseGeneratorMct(_mockOracle.Object, testGroup);
            _subject.Generate(testGroup, false);

            _mockOracle.Verify(v => v.GetTdesMctCase(It.IsAny<TdesParameters>()));
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1)]
        [TestCase(AlgoMode.TDES_CFB8)]
        [TestCase(AlgoMode.TDES_CFB64)]
        public void ShouldReturnErrorMessageIfAlgoFailsWithException(AlgoMode algoMode)
        {
            string errorMessage = "something bad happened! oh noes!";
            _mockOracle
                .Setup(s => s.GetTdesMctCase(It.IsAny<TdesParameters>()))
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
