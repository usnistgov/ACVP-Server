using System;
using System.Linq;
using Moq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.KAS_IFC.Sp800_56Br2;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.IFC.Tests
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
            var tvs = TestDataMother.GetVectorSet(testType, KasKdf.OneStep);
            var result = _subject.GetValidators(tvs).ToList();
            
            Assume.That(result.Count == 1);
            Assert.IsInstanceOf(expectedType, result[0]);
        }
    }
}