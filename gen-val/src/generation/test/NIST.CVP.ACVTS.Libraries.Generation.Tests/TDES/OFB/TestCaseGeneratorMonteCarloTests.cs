using System;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_OFB.v1_0;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.OFB
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
                .Setup(s => s.GetTdesMctCaseAsync(It.IsAny<TdesParameters>()))
                .Returns(() => Task.FromResult(new MctResult<TdesResult>()));
            _subject = new TestCaseGeneratorMonteCarlo(_oracle.Object);
        }

        [Test]
        public async Task ShouldReturnErrorMessageIfAlgoFailsWithException()
        {
            string errorMessage = "something bad happened! oh noes!";
            _oracle.Setup(s => s.GetTdesMctCaseAsync(It.IsAny<TdesParameters>()))
                .Throws(new Exception(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 3
            };
            var result = await _subject.GenerateAsync(testGroup, true);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.IsTrue(result.ErrorMessage.Contains(errorMessage));
        }
    }
}
