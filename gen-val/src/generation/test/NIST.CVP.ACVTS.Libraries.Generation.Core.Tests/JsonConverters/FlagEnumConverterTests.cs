using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.JsonConverters
{
    [TestFixture, UnitTest]
    public class FlagEnumConverterTests
    {
        [Flags]
        public enum TestEnum
        {
            [EnumMember(Value = "none")]
            None = 0,

            [EnumMember(Value = "OPTIONONE")]
            OptionOne = 1 << 0,

            [EnumMember(Value = "OptionTwo")]
            OptionTwo = 1 << 1,

            [EnumMember(Value = "optionThree")]
            OptionThree = 1 << 2,

            [EnumMember(Value = "optionfour")]
            OptionFour = 1 << 3
        }
        
        private class FakeClass
        {
            public string TestString { get; set; }
            public int TestInt { get; set; }
            public TestEnum TestEnum { get; set; }
        }

        private FlagEnumConverter _flagEnumConverter;
        private List<JsonConverter> _converters;
        private JsonSerializerSettings _settings;
        
        [SetUp]
        public void SetUp()
        {
            _flagEnumConverter = new FlagEnumConverter();
            _converters = new List<JsonConverter> { _flagEnumConverter };
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                Converters = _converters,
            };
        }
        
        [Test]
        [TestCase(TestEnum.None)]
        [TestCase(TestEnum.OptionOne)]
        [TestCase(TestEnum.OptionTwo)]
        [TestCase(TestEnum.OptionThree)]
        [TestCase(TestEnum.OptionFour)]
        
        [TestCase(TestEnum.OptionOne | TestEnum.OptionFour)]
        [TestCase(TestEnum.OptionThree | TestEnum.OptionTwo | TestEnum.OptionFour | TestEnum.OptionOne)]
        public void ShouldParseJsonCorrectly(TestEnum testEnum)
        {
            var fake = new FakeClass
            {
                TestString = "abc",
                TestInt = 4,
                TestEnum = testEnum
            };

            var json = JsonConvert.SerializeObject(fake, _settings);
            Assert.That(json, Is.Not.Empty);

            var deserializedFake = JsonConvert.DeserializeObject<FakeClass>(json, _settings);
            Assert.That(deserializedFake, Is.Not.Null);

            Assert.That(fake.TestString, Is.EqualTo(deserializedFake.TestString));
            Assert.That(fake.TestInt, Is.EqualTo(deserializedFake.TestInt));
            Assert.That(fake.TestEnum, Is.EqualTo(deserializedFake.TestEnum));
        }

        [Test]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""none""}", TestEnum.None)]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""OPTIONONE""}", TestEnum.OptionOne)]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""OptionTwo""}", TestEnum.OptionTwo)]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""optionThree""}", TestEnum.OptionThree)]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""optionfour""}", TestEnum.OptionFour)]

        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""OPTIONONE,optionfour""}", (TestEnum.OptionOne | TestEnum.OptionFour))]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": "" OPTIONONE, optionfour""}", (TestEnum.OptionOne | TestEnum.OptionFour))]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""optionfour, OPTIONONE ""}", TestEnum.OptionOne | TestEnum.OptionFour)]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""optionThree, OptionTwo, optionfour, OPTIONONE""}", TestEnum.OptionThree | TestEnum.OptionTwo | TestEnum.OptionFour | TestEnum.OptionOne)]
        public void ShouldDeserializeCorrectJson(string json, TestEnum expectedEnum)
        {
            var deserializedFake = JsonConvert.DeserializeObject<FakeClass>(json, _settings);
            Assert.That(deserializedFake, Is.Not.Null);

            Assert.That(deserializedFake.TestString, Is.EqualTo("abc"));
            Assert.That(deserializedFake.TestInt, Is.EqualTo(1));
            Assert.That(expectedEnum, Is.EqualTo(deserializedFake.TestEnum));
        }

        [Test]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""OPTIONTWO""}")]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""OptionTwo_""}")]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""option three""}")]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""OpTiOnFoUr""}")]

        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": """"}")]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": "" ""}")]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""None""}")]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""default""}")]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": 1}")]
        
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""OPTIONONE, OPTIONTWO""}")]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""OPTIONTWO, OPTIONONE""}")]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""OPTIONONE, optionThree, OPTIONTWO""}")]
        public void ShouldNotDeserializeIncorrectJson(string json)
        {
            Assert.Throws(typeof(JsonException), () => JsonConvert.DeserializeObject<FakeClass>(json, _settings));
        }
    }
}
