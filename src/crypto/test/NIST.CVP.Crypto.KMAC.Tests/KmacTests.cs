using NIST.CVP.Crypto.Common.MAC.KMAC;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KMAC.Tests
{
    [TestFixture, FastCryptoTest]
    public class KmacTests
    {
        private CSHAKEFactory _cSHAKEFactory;

        [SetUp]
        public void Setup()
        {
            _cSHAKEFactory = new CSHAKEFactory();
        }

        [Test]
        [TestCase(32, "40 41 42 43 44 45 46 47 48 49 4A 4B 4C 4D 4E 4F 50 51 52 53 54 55 56 57 58 59 5A 5B 5C 5D 5E 5F", "00 01 02 03", "E5 78 0B 0D 3E A6 F7 D3 A4 29 C5 70 6A A4 3A 00 FA DB D7 D4 96 28 83 9E 31 87 24 3F 45 6E E1 4E")]
        public void ShouldKMAC128Correctly(int length, string key, string inputHex, string outputHex)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetCSHAKEHashFunction(256, 256);

            var CSHAKEWrapped = _cSHAKEFactory.GetCSHAKE(hashFunction);

            var subject = new Kmac(CSHAKEWrapped, 256, 256);
            var result = subject.Generate(new BitString(key), new BitString(inputHex), 256);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Mac);
        }

        private HashFunction GetCSHAKEHashFunction(int digestSize, int capacity)
        {
            return new HashFunction()
            {
                DigestSize = digestSize,
                Capacity = capacity,
                XOF = true
            };
        }
    }
}
