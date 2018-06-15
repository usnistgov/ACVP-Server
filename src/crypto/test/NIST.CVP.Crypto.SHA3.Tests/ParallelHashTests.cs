using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.SHA3.Tests
{
    [TestFixture, FastCryptoTest]
    public class ParallelHashTests
    {
        [Test]
        [TestCase(192, "000102030405060710111213141516172021222324252627", "ba8dc1d1d979331d3f813603c67f72609ab5e44b94a0b8f9af46514454a2b4f5", 8, "")]
        [TestCase(192, "000102030405060710111213141516172021222324252627", "fc484dcb3f84dceedc353438151bee58157d6efed0445a81f165e495795b7206", 8, "Parallel Data")]
        public void ShouldParallelHash128HashCorrectly(int length, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetParallelHashHashFunction(256, 256, false);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message, blockSize, customization);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        [Test]
        [TestCase(32, "00010203", "d008828e2b80ac9d2218ffee1d070c48b8e4c87bff32c9699d5b6896eee0edd164020e2be0560858d9c00c037e34a96937c561a74c412bb4c746469527281c8c", "", "Email Signature")]
        [TestCase(1600, "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7", "07dc27b11e51fbac75bc7b3c1d983e8b4b85fb1defaf218912ac86430273091727f42b17ed1df63e8ec118f04b23633c1dfb1574c8fb55cb45da8e25afb092bb", "", "Email Signature")]
        public void ShouldParallelHash256HashCorrectly(int length, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetParallelHashHashFunction(512, 512, false);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message, blockSize, customization);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        private HashFunction GetParallelHashHashFunction(int digestSize, int capacity, bool xof)
        {
            return new HashFunction()
            {
                DigestSize = digestSize,
                Capacity = capacity,
                XOF = xof
            };
        }
    }
}
