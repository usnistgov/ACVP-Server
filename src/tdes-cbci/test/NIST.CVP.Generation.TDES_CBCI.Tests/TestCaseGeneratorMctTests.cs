using Moq;
using NUnit.Framework;
using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Generation.TDES_CBCI.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMctTests
    {
        private Mock<IOracle> _oracle;
        private TestCaseGeneratorMct _subject;

        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();
            _oracle
                .Setup(s => s.GetTdesMctWithIvsCase(It.IsAny<TdesParameters>()))
                .Returns(() => new MctResult<TdesResultWithIvs>());
            _subject = new TestCaseGeneratorMct(_oracle.Object);
        }
        
        [Test]
        public void ShouldReturnErrorMessageIfAlgoFailsWithException()
        {
            string errorMessage = "something bad happened! oh noes!";
            _oracle.Setup(s => s.GetTdesMctWithIvsCase(It.IsAny<TdesParameters>()))
                .Throws(new Exception(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1
            };
            var result = _subject.Generate(testGroup, true);

            Assert.IsFalse(result.Success, nameof(result.Success));
        }
    }
}
