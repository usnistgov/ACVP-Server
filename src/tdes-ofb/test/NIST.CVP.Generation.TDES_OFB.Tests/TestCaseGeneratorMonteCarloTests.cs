using Moq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NUnit.Framework;
using System;

namespace NIST.CVP.Generation.TDES_OFB.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMonteCarloTests
    {
        private Mock<IOracle> _oracle;
        private TestCaseGeneratorMonteCarlo _subject;

        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();
            _oracle
                .Setup(s => s.GetTdesMctCase(It.IsAny<TdesParameters>()))
                .Returns(() => new MctResult<TdesResult>());
            _subject = new TestCaseGeneratorMonteCarlo(_oracle.Object);
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoFailsWithException()
        {
            string errorMessage = "something bad happened! oh noes!";
            _oracle.Setup(s => s.GetTdesMctCase(It.IsAny<TdesParameters>()))
                .Throws(new Exception(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                NumberOfKeys = 3
            };
            var result = _subject.Generate(testGroup, true);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.IsTrue(result.ErrorMessage.Contains(errorMessage));
        }
    }
}
