using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests.ExtensionMethods
{
    [TestFixture, UnitTest]
    public class ExpandoObjectExtensionsTests
    {
        protected readonly IList<JsonConverter> _jsonConverters = new List<JsonConverter>()
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

        #region GetBitStringFromProperty
        private static object[] _testShouldGetBitStringFromProperty = new object[]
        {
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
            TestClass obj = new TestClass()
            {
                TestBitString = input
            };

            var json = ObjectToJson(obj);
            var dyn = JsonToDynamic(json).ParsedObject;
            var expando = (ExpandoObject)dyn.ToObject<ExpandoObject>();

            var result = expando.GetBitStringFromProperty("TestBitString");

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void ShouldGetNullWhenBitStringNull()
        {
            TestClass obj = new TestClass()
            {
                TestBitString = null
            };

            var json = ObjectToJson(obj);
            var dyn = JsonToDynamic(json).ParsedObject;
            var expando = (ExpandoObject)dyn.ToObject<ExpandoObject>();

            var result = expando.GetBitStringFromProperty("TestBitString");

            Assert.IsNull(result);
        }
        #endregion GetBitStringFromProperty

        #region GetBigIntegerFromProperty
        private static object[] _testShouldGetBigIntegerFromProperty = new object[]
        {
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
            TestClass obj = new TestClass()
            {
                TestBigInteger = input
            };

            var json = ObjectToJson(obj);
            var dyn = JsonToDynamic(json).ParsedObject;
            var expando = (ExpandoObject)dyn.ToObject<ExpandoObject>();

            var result = expando.GetBigIntegerFromProperty("TestBigInteger");

            Assert.AreEqual(expectedOutput, result);
        }
        #endregion GetBigIntegerFromProperty

        #region GetTypeFromProperty
        private static object[] _testShouldGetIntTypeFromProperty = new object[]
        {
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
            TestClass obj = new TestClass()
            {
                TestInt = input
            };

            var json = ObjectToJson(obj);
            var dyn = JsonToDynamic(json).ParsedObject;
            var expando = (ExpandoObject)dyn.ToObject<ExpandoObject>();

            var result = expando.GetTypeFromProperty<int>("TestInt");

            Assert.AreEqual(expectedOutput, result);
        }

        private static object[] _testShouldGetEnumTypeFromProperty = new object[]
        {
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
            TestClass obj = new TestClass()
            {
                TestEnum = input
            };

            var json = ObjectToJson(obj);
            var dyn = JsonToDynamic(json).ParsedObject;
            var expando = (ExpandoObject)dyn.ToObject<ExpandoObject>();

            var result = expando.GetTypeFromProperty<TestEnum>("TestEnum");

            Assert.AreEqual(expectedOutput, result);
        }
        #endregion GetTypeFromProperty

        #region AddBigIntegerWhenNotZero
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

            Assert.AreEqual(expectedToAdd, dyn.ContainsProperty(label));
        }
        #endregion AddBigIntegerWhenNotZero

        #region AddIntegerWhenNotZero
        [Test]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(42, true)]
        public void ShouldAddIntegerWhenNotZero(int value, bool expectedToAdd)
        {
            const string label = "testLabel";

            ExpandoObject dyn = new ExpandoObject();
            dyn.AddIntegerWhenNotZero(label, value);

            Assert.AreEqual(expectedToAdd, dyn.ContainsProperty(label));
        }
        #endregion AddIntegerWhenNotZero

        private string ObjectToJson(object obj)
        {
            var json = JsonConvert.SerializeObject(
                obj, Formatting.Indented, new JsonSerializerSettings()
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