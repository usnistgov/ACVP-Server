using System;
using System.Runtime.Serialization;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Common.Tests.Helpers
{
    [TestFixture, UnitTest]
    public class EnumHelpersTests
    {
        private const string TestDescription = "this is different";

        public enum FakeEnum
        {
            First,
            Second,
            [EnumMember(Value = TestDescription)]
            Third
        }

        [Test]
        [TestCase(FakeEnum.First, "First")]
        [TestCase(FakeEnum.Second, "Second")]
        [TestCase(FakeEnum.Third, TestDescription)]
        public void ShouldReturnCorrectEnumString(FakeEnum value, string expectedValue)
        {
            Assert.That(EnumHelpers.GetEnumDescriptionFromEnum(value), Is.EqualTo(expectedValue));
        }

        [Test]
        public void ShouldThrowWhenTypeIsNotEnum()
        {
            Assert.Throws(typeof(InvalidOperationException), () => EnumHelpers.GetEnumFromEnumDescription<int>(""));
        }

        [Test]
        [TestCase(TestDescription, FakeEnum.Third)]
        public void ShouldReturnCorrectEnumFromDescription(string description, FakeEnum expectedEnum)
        {
            Assert.That(EnumHelpers.GetEnumFromEnumDescription<FakeEnum>(description), Is.EqualTo(expectedEnum));
        }

        [Test]
        public void ShouldReturnDefaultWhenThrowIsFalse()
        {
            Assert.That(EnumHelpers.GetEnumFromEnumDescription<FakeEnum>("bad description", false), Is.EqualTo(default(FakeEnum)));
        }

        [Test]
        public void ShouldThrowByDefaultWhenNotFound()
        {
            Assert.Throws(typeof(InvalidOperationException), () => EnumHelpers.GetEnumFromEnumDescription<FakeEnum>("bad description"));
        }

        [Test]
        public void ShouldReturnEnumDescriptions()
        {
            var descriptions = EnumHelpers.GetEnumDescriptions<FakeEnum>();

            Assert.That(descriptions, Does.Contain(FakeEnum.First.ToString()), "first, no description");
            Assert.That(descriptions, Does.Contain(FakeEnum.Second.ToString()), "second, no description");
            Assert.That(descriptions, Does.Contain(TestDescription), "third, description");
        }
    }
}
