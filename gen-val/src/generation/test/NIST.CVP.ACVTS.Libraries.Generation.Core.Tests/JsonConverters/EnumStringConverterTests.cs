using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.JsonConverters
{
    [TestFixture, UnitTest]
    public class EnumStringConverterTests
    {
        public enum TestEnum
        {
            [EnumMember(Value = "OPTIONONE")]
            OptionOne,
        
            [EnumMember(Value = "OptionTwo")]
            OptionTwo,
        
            [EnumMember(Value = "optionThree")]
            OptionThree,
        
            [EnumMember(Value = "optionfour")]
            OptionFour
        }

        private class FakeClass
        {
            public string TestString { get; set; }
            public int TestInt { get; set; }
            public TestEnum TestEnum { get; set; }
        }

        private EnumStringConverter _enumStringConverter;
        private List<JsonConverter> _converters;
        private JsonSerializerSettings _settings;
        
        [SetUp]
        public void SetUp()
        {
            _enumStringConverter = new EnumStringConverter();
            _converters = new List<JsonConverter> { _enumStringConverter };
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                Converters = _converters,
            };
        }

        [Test]
        [TestCase(TestEnum.OptionOne)]
        [TestCase(TestEnum.OptionTwo)]
        [TestCase(TestEnum.OptionThree)]
        [TestCase(TestEnum.OptionFour)]
        public void ShouldParseJsonCorrectly(TestEnum testEnum)
        {
            var fake = new FakeClass
            {
                TestString = "abc",
                TestInt = 4,
                TestEnum = testEnum
            };

            var json = JsonConvert.SerializeObject(fake, _settings);
            Assert.IsNotEmpty(json);

            var deserializedFake = JsonConvert.DeserializeObject<FakeClass>(json, _settings);
            Assert.IsNotNull(deserializedFake);

            Assert.AreEqual(deserializedFake.TestString, fake.TestString);
            Assert.AreEqual(deserializedFake.TestInt, fake.TestInt);
            Assert.AreEqual(deserializedFake.TestEnum, fake.TestEnum);
        }

        [Test]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""OPTIONONE""}", TestEnum.OptionOne)]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""OptionTwo""}", TestEnum.OptionTwo)]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""optionThree""}", TestEnum.OptionThree)]
        [TestCase(@"{""testString"": ""abc"", ""testInt"": 1, ""testEnum"": ""optionfour""}", TestEnum.OptionFour)]
        public void ShouldReadCorrectJson(string json, TestEnum expectedEnum)
        {
            var deserializedFake = JsonConvert.DeserializeObject<FakeClass>(json, _settings);
            Assert.IsNotNull(deserializedFake);
            
            Assert.AreEqual(deserializedFake.TestString, "abc");
            Assert.AreEqual(deserializedFake.TestInt, 1);
            Assert.AreEqual(deserializedFake.TestEnum, expectedEnum);
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
        public void ShouldNotReadIncorrectJson(string json)
        {
            Assert.Throws(typeof(JsonException), () => JsonConvert.DeserializeObject<FakeClass>(json, _settings));
        }
    }
}
