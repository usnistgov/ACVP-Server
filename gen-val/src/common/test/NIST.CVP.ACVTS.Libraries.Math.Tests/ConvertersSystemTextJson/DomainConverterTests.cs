using System.Linq;
using System.Text.Json;
using NIST.CVP.ACVTS.Libraries.Math.ConvertersSystemTextJson;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Math.Tests.ConvertersSystemTextJson
{
    [TestFixture, UnitTest]
    public class DomainConverterTests
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            Converters = { new DomainConverter() },
            IgnoreNullValues = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private class TestObject
        {
            public MathDomain TestDomain { get; set; }
        }

        [Test]
        public void ShouldIntDomainOverflowValueSegments()
        {
            string json = @"
            {
                ""anInt"" : 5,
                ""testDomain"" : [ 1, 2, 3, 4, 3147483647 ]
            }
            ";

            Assert.Throws<JsonException>(() =>
            {
                JsonSerializer.Deserialize<TestObject>(json, _jsonSerializerOptions);
            });

        }

        [Test]
        public void ShouldIntDomainOverflowRangeSegments()
        {
            string json = @"
            {
                ""anInt"" : 5,
                ""testDomain"" : [ {""min"" : 1, ""max"": 3147483647}]
            }
            ";

            Assert.Throws<JsonException>(() =>
            {
                JsonSerializer.Deserialize<TestObject>(json, _jsonSerializerOptions);
            });
        }

        [Test]
        public void ShouldReturnAllExplicitValues()
        {
            string json = @"
            {
                ""anInt"" : 5,
                ""testDomain"" : [ 1, 2, 3, 4, 5 ]
            }
            ";

            var parsedObj = JsonSerializer.Deserialize<TestObject>(json, _jsonSerializerOptions);

            Assert.That(parsedObj, Is.Not.Null, $"{nameof(parsedObj)} was null.");
            Assert.That(parsedObj.TestDomain.DomainSegments.ToList().Count, Is.EqualTo(5), "count");
            Assert.That(
                parsedObj.TestDomain.DomainSegments.ToList().TrueForAll(a => a.GetType() == typeof(ValueDomainSegment)), Is.True,
                "types");
        }

        [Test]
        public void ShouldReturnAllRangeValues()
        {
            string json = @"
            {
                ""anInt"" : 5,
                ""testDomain"" : [ {""min"" : 1, ""max"" : 2}, {""min"" : 3, ""max"" : 4} ]
            }
            ";

            var parsedObj = JsonSerializer.Deserialize<TestObject>(json, _jsonSerializerOptions);

            Assert.That(parsedObj, Is.Not.Null, $"{nameof(parsedObj)} was null.");
            Assert.That(parsedObj.TestDomain.DomainSegments.ToList().Count, Is.EqualTo(2), "count");
            Assert.That(
                parsedObj.TestDomain.DomainSegments.ToList().TrueForAll(a => a.GetType() == typeof(RangeDomainSegment)), Is.True);
        }

        [Test]
        public void ShouldReturnDomainWithCombinationOfLiteralAndRanges()
        {
            string json = @"
            {
                ""testDomain"" : [ 0, {""min"" : 1, ""max"" : 2}, {""min"" : 3, ""max"" : 4}, 5, 6 ]
            }
            ";

            var parsedObj = JsonSerializer.Deserialize<TestObject>(json, _jsonSerializerOptions);

            Assert.That(parsedObj, Is.Not.Null, $"{nameof(parsedObj)} was null.");
            Assert.That(parsedObj.TestDomain.DomainSegments.ToList().Count, Is.EqualTo(5), "count");
            Assert.That(parsedObj.TestDomain.DomainSegments.Count(c => c.GetType() == typeof(ValueDomainSegment)), Is.EqualTo(3),
                "ValueDomainSegment count");
            Assert.That(parsedObj.TestDomain.DomainSegments.Count(c => c.GetType() == typeof(RangeDomainSegment)), Is.EqualTo(2),
                "RangeDomainSegment count");
        }

        [Test]
        public void ShouldWriteJsonAndDeserializeIntoSameObject()
        {
            MathDomain original = new MathDomain();
            ValueDomainSegment vds = new ValueDomainSegment(1024);
            RangeDomainSegment rds = new RangeDomainSegment(new Random800_90(), 0, 1024, 128);

            original.AddSegment(vds);
            original.AddSegment(rds);

            var serializedObject = JsonSerializer.Serialize(original, _jsonSerializerOptions);

            var deserializedObject = JsonSerializer.Deserialize<MathDomain>(serializedObject, _jsonSerializerOptions);

            Assert.That(deserializedObject, Is.Not.Null, $"{nameof(deserializedObject)} was null.");

            var vdsReObjectified = deserializedObject.DomainSegments.OfType<ValueDomainSegment>().First();
            var rdsReObjectified = deserializedObject.DomainSegments.OfType<RangeDomainSegment>().First();

            Assert.That(deserializedObject.DomainSegments.Count(), Is.EqualTo(original.DomainSegments.Count()), "count");
            Assert.That(vdsReObjectified.ToString(), Is.EqualTo(vds.ToString()), nameof(vds));
            Assert.That(rdsReObjectified.ToString(), Is.EqualTo(rds.ToString()), nameof(rds));
        }
    }
}
