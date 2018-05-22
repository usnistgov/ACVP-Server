using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Runtime.Serialization;
using NIST.CVP.Common.Helpers;

namespace NIST.CVP.Common.Tests.Helpers
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
            Assert.AreEqual(expectedValue, EnumHelpers.GetEnumDescriptionFromEnum(value));
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
            Assert.AreEqual(expectedEnum, EnumHelpers.GetEnumFromEnumDescription<FakeEnum>(description));
        }

        [Test]
        public void ShouldReturnDefaultWhenThrowIsFalse()
        {
            Assert.AreEqual(default(FakeEnum), EnumHelpers.GetEnumFromEnumDescription<FakeEnum>("bad description", false));
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

            Assert.Contains(FakeEnum.First.ToString(), descriptions, "first, no description");
            Assert.Contains(FakeEnum.Second.ToString(), descriptions, "second, no description");
            Assert.Contains(TestDescription, descriptions, "third, description");
        }
    }
}