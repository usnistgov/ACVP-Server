using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Math.JsonConverters;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Math.Tests.JsonConverters
{
    [TestFixture, UnitTest]
    public class DomainConverterTests
    {
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

            Assert.Throws<JsonSerializationException>(() =>
            {
                _ = JsonConvert.DeserializeObject<TestObject>(
                    json,
                    new JsonSerializerSettings
                    {
                        Converters = new List<JsonConverter> { new DomainConverter() },
                        NullValueHandling = NullValueHandling.Ignore
                    });
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

            Assert.Throws<JsonReaderException>(() =>
            {
                _ = JsonConvert.DeserializeObject<TestObject>(
                    json,
                    new JsonSerializerSettings
                    {
                        Converters = new List<JsonConverter> { new DomainConverter() },
                        NullValueHandling = NullValueHandling.Ignore
                    });
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

            //var parsedObj = JsonConvert.DeserializeObject<TestObject>(json);
            var parsedObj = JsonConvert.DeserializeObject<TestObject>(
                json,
                new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter> { new DomainConverter() },
                    NullValueHandling = NullValueHandling.Ignore
                });

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

            //var parsedObj = JsonConvert.DeserializeObject<TestObject>(json);
            var parsedObj = JsonConvert.DeserializeObject<TestObject>(
                json,
                new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter> { new DomainConverter() },
                    NullValueHandling = NullValueHandling.Ignore
                });

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

            var parsedObj = JsonConvert.DeserializeObject<TestObject>(
                json,
                new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter> { new DomainConverter() },
                    NullValueHandling = NullValueHandling.Ignore
                });

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

            var serializedObject = JsonConvert.SerializeObject(
                original,
                new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter> { new DomainConverter() },
                    NullValueHandling = NullValueHandling.Ignore
                }
            );

            var deserializedObject =
                JsonConvert.DeserializeObject<MathDomain>(
                    serializedObject,
                    new DomainConverter()
                );

            var vdsReObjectified = deserializedObject.DomainSegments.OfType<ValueDomainSegment>().First();
            var rdsReObjectified = deserializedObject.DomainSegments.OfType<RangeDomainSegment>().First();

            Assert.That(deserializedObject.DomainSegments.Count(), Is.EqualTo(original.DomainSegments.Count()), "count");
            Assert.That(vdsReObjectified.ToString(), Is.EqualTo(vds.ToString()), nameof(vds));
            Assert.That(rdsReObjectified.ToString(), Is.EqualTo(rds.ToString()), nameof(rds));
        }
    }
}
