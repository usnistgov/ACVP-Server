using System.Text;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SHA.Tests
{
    [TestFixture]
    [FastCryptoTest]
    public class Sha3DerivedHelpersTests
    {
        [Test]
        [TestCase(1, "01")]
        [TestCase(128, "80")]
        [TestCase(129, "81")]
        [TestCase(255, "FF")]
        [TestCase(256, "0100")]
        [TestCase(168, "A8")]
        [TestCase(136, "88")]
        public void ShouldIntToBitString(int n, string expectedHex)
        {
            var result = Sha3DerivedHelpers.IntToBitString(n);

            Assert.That(result.ToHex(), Is.EqualTo(expectedHex));
        }

        [Test]
        // Shake 128
        [TestCase(168, "", 161)]
        [TestCase(168, "TupleHash", 152)]
        [TestCase(168, "ParallelHash", 149)]
        [TestCase(168, "", 161 + 168 * 1)]
        [TestCase(168, "", 161 + 168 * 2)]
        [TestCase(168, "", 161 + 168 * 3)]
        [TestCase(168, "TupleHash", 152 + 168 * 1)]
        [TestCase(168, "TupleHash", 152 + 168 * 2)]
        [TestCase(168, "TupleHash", 152 + 168 * 3)]
        [TestCase(168, "ParallelHash", 149 + 168 * 1)]
        [TestCase(168, "ParallelHash", 149 + 168 * 2)]
        [TestCase(168, "ParallelHash", 149 + 168 * 3)]

        // Shake 256
        [TestCase(136, "", 129)]
        [TestCase(136, "TupleHash", 120)]
        [TestCase(136, "ParallelHash", 117)]
        [TestCase(136, "", 129 + 136 * 1)]
        [TestCase(136, "", 129 + 136 * 2)]
        [TestCase(136, "", 129 + 136 * 3)]
        [TestCase(136, "TupleHash", 120 + 136 * 1)]
        [TestCase(136, "TupleHash", 120 + 136 * 2)]
        [TestCase(136, "TupleHash", 120 + 136 * 3)]
        [TestCase(136, "ParallelHash", 117 + 136 * 1)]
        [TestCase(136, "ParallelHash", 117 + 136 * 2)]
        [TestCase(136, "ParallelHash", 117 + 136 * 3)]
        public void ShouldGetBlockAlignedDataWithSpecificLengthForCshake(int rateBytes, string functionName,
            int customizationStringLengthBytes)
        {
            var blockAlignedToRate = new BitString(new byte[rateBytes]);

            var functionNameBitString = new BitString(Encoding.ASCII.GetBytes(functionName));
            var customizationString = new BitString(new byte[customizationStringLengthBytes]);

            var encodedFunctionName = Sha3DerivedHelpers.EncodeString(functionNameBitString);
            var encodedCustomizationString = Sha3DerivedHelpers.EncodeString(customizationString);
            var leftEncodeRate = Sha3DerivedHelpers.LeftEncode(Sha3DerivedHelpers.IntToBitString(rateBytes));

            var result = Sha3DerivedHelpers.Bytepad(
                BitString.ConcatenateBits(
                    encodedFunctionName,
                    encodedCustomizationString),
                Sha3DerivedHelpers.IntToBitString(rateBytes));

            Assert.That((encodedFunctionName.BitLength + encodedCustomizationString.BitLength + leftEncodeRate.BitLength) %
                blockAlignedToRate.BitLength, Is.EqualTo(0),
                "individual pieces mod rate bits should equal 0");
            Assert.That(result.BitLength % blockAlignedToRate.BitLength, Is.EqualTo(0),
                "result bit length mod rate bits should equal 0");
        }

        [Test]
        [TestCase(136, 131)]
        [TestCase(136, 131 + 136 * 1)]
        [TestCase(136, 131 + 136 * 2)]
        [TestCase(136, 131 + 136 * 3)]
        [TestCase(168, 163)]
        [TestCase(168, 163 + 168 * 1)]
        [TestCase(168, 163 + 168 * 2)]
        [TestCase(168, 163 + 168 * 3)]
        public void ShouldGetBlockAlignedDataWithSpecificKeyLengthForKmac(int rateBytes, int keyLength)
        {
            var blockAlignedToRate = new BitString(new byte[rateBytes]);

            var key = new BitString(new byte[keyLength]);

            var encodedKey = Sha3DerivedHelpers.EncodeString(key);
            var leftEncodeRate = Sha3DerivedHelpers.LeftEncode(Sha3DerivedHelpers.IntToBitString(rateBytes));

            var result = Sha3DerivedHelpers.Bytepad(
                encodedKey,
                Sha3DerivedHelpers.IntToBitString(rateBytes));

            Assert.That((encodedKey.BitLength + leftEncodeRate.BitLength) % blockAlignedToRate.BitLength, Is.EqualTo(0),
                "individual pieces mod rate bits should equal 0");
            Assert.That(result.BitLength % blockAlignedToRate.BitLength, Is.EqualTo(0),
                "result bit length mod rate bits should equal 0");
        }

        [Test]
        [TestCase(0, "0001")]
        [TestCase(1, "0101")]
        public void ShouldRightEncode(int n, string expectedHex)
        {
            var result = Sha3DerivedHelpers.RightEncode(Sha3DerivedHelpers.IntToBitString(n));

            Assert.That(result.ToHex(), Is.EqualTo(expectedHex));
        }

        [Test]
        [TestCase(0, "0100")]
        [TestCase(1, "0101")]
        public void ShouldLeftEncode(int n, string expectedHex)
        {
            var result = Sha3DerivedHelpers.LeftEncode(Sha3DerivedHelpers.IntToBitString(n));

            Assert.That(result.ToHex(), Is.EqualTo(expectedHex));
        }
    }
}
