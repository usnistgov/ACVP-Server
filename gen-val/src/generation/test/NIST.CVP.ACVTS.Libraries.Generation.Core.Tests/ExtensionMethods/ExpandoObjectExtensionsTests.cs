using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.JsonConverters;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.ExtensionMethods
{
    [TestFixture, UnitTest]
    public class ExpandoObjectExtensionsTests
    {
        protected readonly IList<JsonConverter> _jsonConverters = new List<JsonConverter>
        {
            new BitstringConverter(),
            new BigIntegerConverter(),
            new StringEnumConverter()
        };

        public enum TestEnum
        {
            None,
            Option1,
            Option2
        }

        private class TestClass
        {
            public BitString TestBitString { get; set; }
            public BigInteger TestBigInteger { get; set; }
            public int TestInt { get; set; }
            public TestEnum TestEnum { get; set; }
        }

        private static object[] _testShouldGetBitStringFromProperty = {
            new object[]
            {
                new BitString("CAFECAFE"),
                new BitString("CAFECAFE")
            },
            new object[]
            {
                new BitString(0),
                new BitString(0)
            }
        };

        [Test]
        [TestCaseSource(nameof(_testShouldGetBitStringFromProperty))]
        public void ShouldGetBitStringFromProperty(BitString input, BitString expectedOutput)
        {
            TestClass obj = new TestClass
            {
                TestBitString = input
            };

            var json = ObjectToJson(obj);
            var dyn = JsonToDynamic(json).ParsedObject;
            var expando = (ExpandoObject)dyn.ToObject<ExpandoObject>();

            var result = expando.GetBitStringFromProperty("TestBitString");

            Assert.That(result, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void ShouldGetNullWhenBitStringNull()
        {
            TestClass obj = new TestClass
            {
                TestBitString = null
            };

            var json = ObjectToJson(obj);
            var dyn = JsonToDynamic(json).ParsedObject;
            var expando = (ExpandoObject)dyn.ToObject<ExpandoObject>();

            var result = expando.GetBitStringFromProperty("TestBitString");

            Assert.That(result, Is.Null);
        }

        private static object[] _testShouldGetBigIntegerFromProperty = {
            new object[]
            {
                new BigInteger(42),
                new BigInteger(42)
            },
            new object[]
            {
                new BigInteger(0),
                new BigInteger(0)
            },
            new object[]
            {
                new BitString("123456789012345678901234567890123456789012345678901234567890").ToPositiveBigInteger(),
                new BitString("123456789012345678901234567890123456789012345678901234567890").ToPositiveBigInteger()
            },
        };

        [Test]
        [TestCaseSource(nameof(_testShouldGetBigIntegerFromProperty))]
        public void ShouldGetBigIntegerFromProperty(BigInteger input, BigInteger expectedOutput)
        {
            TestClass obj = new TestClass
            {
                TestBigInteger = input
            };

            var json = ObjectToJson(obj);
            var dyn = JsonToDynamic(json).ParsedObject;
            var expando = (ExpandoObject)dyn.ToObject<ExpandoObject>();

            var result = expando.GetBigIntegerFromProperty("TestBigInteger");

            Assert.That(result, Is.EqualTo(expectedOutput));
        }

        private static object[] _testShouldGetIntTypeFromProperty = {
            new object[]
            {
                1,
                1
            },
            new object[]
            {
                0,
                0
            },
            new object[]
            {
                42,
                42
            }
        };

        [Test]
        [TestCaseSource(nameof(_testShouldGetIntTypeFromProperty))]
        public void ShouldGetIntTypeFromProperty(int input, int expectedOutput)
        {
            TestClass obj = new TestClass
            {
                TestInt = input
            };

            var json = ObjectToJson(obj);
            var dyn = JsonToDynamic(json).ParsedObject;
            var expando = (ExpandoObject)dyn.ToObject<ExpandoObject>();

            var result = expando.GetTypeFromProperty<int>("TestInt");

            Assert.That(result, Is.EqualTo(expectedOutput));
        }

        private static object[] _testShouldGetEnumTypeFromProperty = {
            new object[]
            {
                TestEnum.None,
                TestEnum.None
            },
            new object[]
            {
                TestEnum.Option1,
                TestEnum.Option1
            },
            new object[]
            {
                TestEnum.Option2,
                TestEnum.Option2
            }
        };

        [Test]
        [TestCaseSource(nameof(_testShouldGetEnumTypeFromProperty))]
        public void ShouldGetEnumTypeFromProperty(TestEnum input, TestEnum expectedOutput)
        {
            TestClass obj = new TestClass
            {
                TestEnum = input
            };

            var json = ObjectToJson(obj);
            var dyn = JsonToDynamic(json).ParsedObject;
            var expando = (ExpandoObject)dyn.ToObject<ExpandoObject>();

            var result = expando.GetTypeFromProperty<TestEnum>("TestEnum");

            Assert.That(result, Is.EqualTo(expectedOutput));
        }

        [Test]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(42, true)]
        public void ShouldAddBigIntegerWhenNotZero(int value, bool expectedToAdd)
        {
            const string label = "testLabel";
            BigInteger bi = new BigInteger(value);

            ExpandoObject dyn = new ExpandoObject();
            dyn.AddBigIntegerWhenNotZero(label, bi);

            Assert.That(dyn.ContainsProperty(label), Is.EqualTo(expectedToAdd));
        }

        [Test]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(42, true)]
        public void ShouldAddIntegerWhenNotZero(int value, bool expectedToAdd)
        {
            const string label = "testLabel";

            ExpandoObject dyn = new ExpandoObject();
            dyn.AddIntegerWhenNotZero(label, value);

            Assert.That(dyn.ContainsProperty(label), Is.EqualTo(expectedToAdd));
        }

        private string ObjectToJson(object obj)
        {
            var json = JsonConvert.SerializeObject(
                obj, Formatting.Indented, new JsonSerializerSettings
                {
                    Converters = _jsonConverters,
                    NullValueHandling = NullValueHandling.Ignore
                }
            );

            return json;
        }

        private ParseResponse<dynamic> JsonToDynamic(string json)
        {
            var dyn = JsonConvert.DeserializeObject<dynamic>(
                json,
                new JsonSerializerSettings
                {
                    Converters = _jsonConverters
                });
            return new ParseResponse<dynamic>(dyn);
        }
    }
}
