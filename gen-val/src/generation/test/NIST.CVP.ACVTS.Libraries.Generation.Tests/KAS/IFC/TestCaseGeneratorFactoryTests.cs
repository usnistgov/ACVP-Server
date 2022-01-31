using System;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KAS_IFC.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.IFC
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        TestCaseGeneratorFactory _subject = new TestCaseGeneratorFactory(new Mock<IOracle>().Object);

        [Test]
        [TestCase(true, "aft", typeof(TestCaseGeneratorVal))]
        [TestCase(true, "val", typeof(TestCaseGeneratorVal))]
        [TestCase(false, "aft", typeof(TestCaseGeneratorAft))]
        [TestCase(false, "val", typeof(TestCaseGeneratorVal))]
        public void ShouldReturnCorrectGenerator(bool isSample, string testType, Type expectedType)
        {
            var tvs = TestDataMother.GetVectorSet(testType, Kda.OneStep);
            var tg = tvs.TestGroups[0];
            tg.IsSample = isSample;

            var result = _subject.GetCaseGenerator(tg);

            Assert.IsInstanceOf(expectedType, result);
        }
    }
}
