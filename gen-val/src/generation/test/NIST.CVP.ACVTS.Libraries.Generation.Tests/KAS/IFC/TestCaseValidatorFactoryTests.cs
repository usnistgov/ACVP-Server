using System;
using System.Linq;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KAS_IFC.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.IFC
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        TestCaseValidatorFactory _subject = new TestCaseValidatorFactory(new Mock<IOracle>().Object);

        [Test]
        [TestCase("aft", typeof(TestCaseValidatorAft))]
        [TestCase("val", typeof(TestCaseValidatorVal))]
        public void ShouldReturnCorrectValidator(string testType, Type expectedType)
        {
            var tvs = TestDataMother.GetVectorSet(testType, Kda.OneStep);
            var result = _subject.GetValidators(tvs).ToList();

            Assert.That(result.Count == 1);
            Assert.IsInstanceOf(expectedType, result[0]);
        }
    }
}
