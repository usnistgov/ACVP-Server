using Moq;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.SP800_106.Tests
{

    [TestFixture, FastCryptoTest]
    public class PreSigVerMessageRandomizerTests
    {
        private readonly Mock<IEntropyProvider> _mockEntropyProvider = new Mock<IEntropyProvider>();
        private PreSigVerMessageRandomizer _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new PreSigVerMessageRandomizer(_mockEntropyProvider.Object);
        }

        private static IEnumerable<object> _testData = new List<object>()
        {
            new object[]
            {
                // label
                "test 1",
                // message
                new BitString(80),
                // random value
                new BitString(80),
                // transformed message

                /*
                 *
                 * message          00 00 00 00 00 00 00 00 00 00
                 * random value     00 00 00 00 00 00 00 00 00 00
                 *
                 * 1. if message.BitLength >= randomValue.BitLength - 1
                 *      80 >= 80 - 1
                 *      80 >= 79
                 *      true
                 *
                 *      padding = 1
                 *
                 * 2. m = message || padding
                 *      m = 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 || 1
                 *      m = 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 1
                 * 
                 *
                 * 3. n = rv.BitLength
                 *      n = 80
                 *
                 * 4. if (n > 1024)
                 *      false
                 *
                 *
                 * 5. count = Floor (m.BitLength / n)
                 *      count = Floor (81 / 80)
                 *      count = 1
                 *
                 * 6. remainder = m.BitLength mod n
                 *      remainder = 81 mod 80
                 *      remainder = 1
                 *
                 * 7. Concatenate [counter] copies of the [rv] to the [remainder] left most bits of the [rv] to get [Rv], such that |Rv| = |m|
                 *      Rv = counter[1] = 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 || 0
                 *      Rv = 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 0
                 *
                 * 8. Convert [n] to a 16-bit binary string [rv_length_indicator]
                 *      ‭01010000‬
                 *
                 * 9. M = rv || (m XOR Rv) || rv_length_indicator
                 *      M = 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 || (00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 000000001 XOR 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 0) || 00000000 01010000
                 *      M = 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 || 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 1 || 00000000 01010000
                 *      M = 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 10000000 00101000 0 
                 *      M = 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 28 00
                 */

                new BitString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 28 00"),
                // expected bit count
                177
            },

            new object[]
            {
                // label
                "test 2 - random value larger than message logic",
                // message
                new BitString("00 00 00 00 00 00 00 00 00 00"),
                // random value
                new BitString("00 00 00 00 00 00 00 00 00 00 FF FF FF FF"),
                // transformed message

                /*
                 *
                 * message          00 00 00 00 00 00 00 00 00 00
                 * random value     00 00 00 00 00 00 00 00 00 00 FF FF FF FF
                 *
                 * 1. if message.BitLength >= randomValue.BitLength - 1
                 *      80 >= 112 - 1
                 *      80 >= 111
                 *      false
                 *
                 *      padding = 1 || Zeroes(rv.BitLength - message.BitLength - 1)
                 *      padding = 1 || Zeroes(112 - 80 - 1)
                 *      padding = 1 || Zeroes(31)
                 *      padding = 10000000 00000000 00000000 00000000
                 *      
                 *
                 * 2. m = message || padding
                 *      m = 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 || 10000000 00000000 00000000 00000000
                 *      m = 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 10000000 00000000 00000000 00000000
                 * 
                 *
                 * 3. n = rv.BitLength
                 *      n = 112
                 *
                 * 4. if (n > 1024)
                 *      false
                 *
                 *
                 * 5. count = Floor (m.BitLength / n)
                 *      Floor (112 / 112)
                 *      Floor (1)
                 *      count = 1
                 *
                 *
                 * 6. remainder = m.BitLength mod n
                 *      remainder = 112 mod 112
                 *      remainder = 0
                 *
                 *
                 * 7. Concatenate [counter] copies of the [rv] to the [remainder] left most bits of the [rv] to get [Rv], such that |Rv| = |m|
                 *      Rv = counter[1] = 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 11111111 11111111 11111111 11111111
                 *
                 * 8. Convert [n] to a 16-bit binary string [rv_length_indicator]
                 *      n = 112
                 *      n = 00000000 ‭‭01110000‬‬
                 *
                 * 9. M = rv || (m XOR Rv) || rv_length_indicator
                 *      M = rv || m XOR Rv || rv_length_indicator
                 *      M = 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 11111111 11111111 11111111 11111111 || (00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 10000000 00000000 00000000 00000000 XOR 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 11111111 11111111 11111111 11111111) || 00000000 ‭01110000‬
                 *      M = 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 11111111 11111111 11111111 11111111 || 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 01111111 11111111 11111111 11111111 || 00000000 ‭01110000‬
                 *      M = 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 11111111 11111111 11111111 11111111 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000 01111111 11111111 11111111 11111111 00000000 ‭01110000‬
                 *      M = 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 7F FF FF FF 00 70
                 *
                 */

                new BitString("00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 7F FF FF FF 00 70"),
                // expected bit count
                240
            }
        };

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void ShouldApplyRandomizeFunctionProperly(string testLabel, BitString message, BitString randomValue, BitString transformedMessage, int expectedBitCount)
        {
            _mockEntropyProvider
                .Setup(s => s.GetEntropy(randomValue.BitLength))
                .Returns(() => randomValue);

            var result = _subject.RandomizeMessage(message, randomValue.BitLength);

            Assert.AreEqual(transformedMessage.ToHex(), result.ToHex(), nameof(transformedMessage));
            Assert.AreEqual(expectedBitCount, result.BitLength, nameof(expectedBitCount));
        }

        [Test]
        [TestCase(42, false)]
        [TestCase(80, true)]
        [TestCase(128, true)]
        [TestCase(1024, true)]
        [TestCase(1025, false)]
        public void ShouldThrowWithInvalidRandomValueLength(int randomValueLength, bool shouldPass)
        {
            _mockEntropyProvider
                .Setup(s => s.GetEntropy(randomValueLength))
                .Returns(() => new BitString(randomValueLength));

            if (shouldPass)
            {
                var result = _subject.RandomizeMessage(new BitString(0), randomValueLength);
                Assert.IsTrue(result != null);
            }
            else
            {
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                    _subject.RandomizeMessage(new BitString(0), randomValueLength));
            }
        }
    }
}