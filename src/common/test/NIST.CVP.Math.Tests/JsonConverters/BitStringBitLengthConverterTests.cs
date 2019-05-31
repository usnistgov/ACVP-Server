using Newtonsoft.Json;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Math.JsonConverters;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Collections.Generic;

namespace NIST.CVP.Math.Tests.JsonConverters
{
    [TestFixture, UnitTest]
    public class BitStringBitLengthConverterTests
    {
        [Test]
        [TestCase("FF", 8, "FF", 8)]
        [TestCase("FF", 7, "FE", 7)]
        [TestCase("FF", 4, "F0", 4)]
        [TestCase("", 0, "", 0)]
        public void ShouldDeSerializeProperly(string hex, int length, string expectedHex, int expectedLength)
        {
            var bs = new BitString(hex, length);

            var serialize = JsonConvert.SerializeObject(bs, new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>()
                {
                    new BitstringBitLengthConverter()
                }
            });
            var deserialize = JsonConvert.DeserializeObject<BitString>(serialize, new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>()
                {
                    new BitstringBitLengthConverter()
                }
            });

            Assert.AreEqual(expectedHex, deserialize.ToHex(), nameof(bs.ToHex));
            Assert.AreEqual(expectedLength, deserialize.BitLength, nameof(bs.BitLength));
        }

        [Test]
        [TestCase("FF", 8, "FF", 8)]
        [TestCase("FF", 7, "FE", 8)]
        [TestCase("FF", 4, "F0", 8)]
        [TestCase("", 0, "", 0)]
        public void ShouldReadJsonFromHexString(string hex, int length, string expectedHex, int expectedLength)
        {
            var bs = new BitString(hex, length);

            // Use the old Hex representation
            var serialize = JsonConvert.SerializeObject(bs, new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>()
                {
                    new BitstringConverter()
                }
            });
            // Use the new converter to ensure the old format can be parsed
            var deserialize = JsonConvert.DeserializeObject<BitString>(serialize, new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>()
                {
                    new BitstringBitLengthConverter()
                }
            });

            Assert.AreEqual(expectedHex, deserialize.ToHex(), nameof(bs.ToHex));
            Assert.AreEqual(expectedLength, deserialize.BitLength, nameof(bs.BitLength));
        }
    }
}
