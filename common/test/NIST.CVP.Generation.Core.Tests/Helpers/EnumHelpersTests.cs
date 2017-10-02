using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.ComponentModel;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.Core.Tests.Helpers
{
    [TestFixture, UnitTest]
    public class EnumHelpersTests
    {
        private const string TestDescription = "this is different";

        public enum FakeEnum
        {
            First,
            Second,
            [System.ComponentModel.Description(TestDescription)]
            Third
        }

        [Test]
        [TestCase(FakeEnum.First, "First")]
        [TestCase(FakeEnum.Second, "Second")]
        [TestCase(FakeEnum.Third, TestDescription)]
        public void ShouldReturnCorrectEnumString(FakeEnum value, string expectedValue)
        {
            Assert.AreEqual(expectedValue, EnumHelpers.GetEnumDescriptionFromEnum(value));
        }

        [Test]
        [TestCase(TestDescription, FakeEnum.Third)]
        public void ShouldReturnCorrectEnumFromDescription(string description, FakeEnum expectedEnum)
        {
            Assert.AreEqual(expectedEnum, EnumHelpers.GetEnumFromEnumDescription<FakeEnum>(description));
        }
    }
}