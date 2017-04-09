using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests
{
    [TestFixture]
    public class DomainConverterTests
    {

        private Mock<IRandom800_90> _mockRandom;

        [SetUp]
        public void Setup()
        {
            _mockRandom = new Mock<IRandom800_90>();
        }

        private class TestObject
        {
            public MathDomain TestDomain { get; set; }
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
                    Converters = new List<JsonConverter>() { new DomainConverter(_mockRandom.Object) },
                    NullValueHandling = NullValueHandling.Ignore
                });

            Assert.AreEqual(5, parsedObj.TestDomain.DomainSegments.ToList().Count, "count");
            Assert.IsTrue(parsedObj.TestDomain.DomainSegments.ToList().TrueForAll(a => a.GetType() == typeof(ValueDomainSegment)), "types");
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
                    Converters = new List<JsonConverter>() { new DomainConverter(_mockRandom.Object) },
                    NullValueHandling = NullValueHandling.Ignore
                });

            Assert.AreEqual(2, parsedObj.TestDomain.DomainSegments.ToList().Count, "count");
            Assert.IsTrue(parsedObj.TestDomain.DomainSegments.ToList().TrueForAll(a => a.GetType() == typeof(RangeDomainSegment)));
        }

        [Test]
        public void ShouldReturnDomainWithCombinationOfLiteralAndRanges()
        {
            string json = @"
            {
                ""testDomain"" : [ 0, {""min"" : 1, ""max"" : 2}, {""min"" : 3, ""max"" : 4}, 5, 6 ]
            }
            ";

            //var parsedObj = JsonConvert.DeserializeObject<TestObject>(json);
            var parsedObj = JsonConvert.DeserializeObject<TestObject>(
                json,
                new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>() { new DomainConverter(_mockRandom.Object) },
                    NullValueHandling = NullValueHandling.Ignore
                });

            Assert.AreEqual(5, parsedObj.TestDomain.DomainSegments.ToList().Count, "count");
            Assert.AreEqual(3, parsedObj.TestDomain.DomainSegments.Count(c => c.GetType() == typeof(ValueDomainSegment)), "ValueDomainSegment count");
            Assert.AreEqual(2, parsedObj.TestDomain.DomainSegments.Count(c => c.GetType() == typeof(RangeDomainSegment)), "RangeDomainSegment count");
        }
    }
}
