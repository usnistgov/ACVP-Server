using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.ParallelHash.Tests
{
    [TestFixture, FastCryptoTest]
    public class ParallelHashTests
    {
        [Test]
        [TestCase(192, "000102030405060710111213141516172021222324252627", "ba8dc1d1d979331d3f813603c67f72609ab5e44b94a0b8f9af46514454a2b4f5", 8, "")]
        [TestCase(192, "000102030405060710111213141516172021222324252627", "fc484dcb3f84dceedc353438151bee58157d6efed0445a81f165e495795b7206", 8, "Parallel Data")]
        [TestCase(576, "000102030405060708090a0b101112131415161718191a1b202122232425262728292a2b303132333435363738393a3b404142434445464748494a4b505152535455565758595a5b", "f7fd5312896c6685c828af7e2adb97e393e7f8d54e3c2ea4b95e5aca3796e8fc", 12, "Parallel Data")]
        public void ShouldParallelHash128HashCorrectly(int length, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetParallelHashHashFunction(256, 256, false, blockSize, customization);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        [Test]
        public void ShouldParallelHash128HashCorrectlyBitWise(int length, int digestSize, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetParallelHashHashFunction(digestSize, 256, false, blockSize, customization);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        [Test]
        [TestCase(192, 256, "000102030405060710111213141516172021222324252627", "fe47d661e49ffe5b7d999922c062356750caf552985b8e8ce6667f2727c3c8d3", 8, "")]
        [TestCase(192, 256, "000102030405060710111213141516172021222324252627", "ea2a793140820f7a128b8eb70a9439f93257c6e6e79b4a540d291d6dae7098d7", 8, "Parallel Data")]
        [TestCase(576, 256, "000102030405060708090a0b101112131415161718191a1b202122232425262728292a2b303132333435363738393a3b404142434445464748494a4b505152535455565758595a5b", "0127ad9772ab904691987fcc4a24888f341fa0db2145e872d4efd255376602f0", 12, "Parallel Data")]
        [TestCase(192, 128, "000102030405060710111213141516172021222324252627", "fe47d661e49ffe5b7d999922c0623567", 8, "")]
        [TestCase(192, 128, "000102030405060710111213141516172021222324252627", "ea2a793140820f7a128b8eb70a9439f9", 8, "Parallel Data")]
        [TestCase(576, 128, "000102030405060708090a0b101112131415161718191a1b202122232425262728292a2b303132333435363738393a3b404142434445464748494a4b505152535455565758595a5b", "0127ad9772ab904691987fcc4a24888f", 12, "Parallel Data")]
        public void ShouldParallelHashXOF128HashCorrectly(int length, int digestSize, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetParallelHashHashFunction(digestSize, 256, true, blockSize, customization);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        [Test]
        public void ShouldParallelHash128XOFHashCorrectlyBitWise(int length, int digestSize, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetParallelHashHashFunction(digestSize, 256, true, blockSize, customization);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        [Test]
        [TestCase(192, "000102030405060710111213141516172021222324252627", "bc1ef124da34495e948ead207dd9842235da432d2bbc54b4c110e64c451105531b7f2a3e0ce055c02805e7c2de1fb746af97a1dd01f43b824e31b87612410429", 8, "")]
        [TestCase(192, "000102030405060710111213141516172021222324252627", "cdf15289b54f6212b4bc270528b49526006dd9b54e2b6add1ef6900dda3963bb33a72491f236969ca8afaea29c682d47a393c065b38e29fae651a2091c833110", 8, "Parallel Data")]
        [TestCase(576, "000102030405060708090a0b101112131415161718191a1b202122232425262728292a2b303132333435363738393a3b404142434445464748494a4b505152535455565758595a5b", "69d0fcb764ea055dd09334bc6021cb7e4b61348dff375da262671cdec3effa8d1b4568a6cce16b1cad946ddde27f6ce2b8dee4cd1b24851ebf00eb90d43813e9", 12, "Parallel Data")]
        public void ShouldParallelHash256HashCorrectly(int length, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetParallelHashHashFunction(512, 512, false, blockSize, customization);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        [Test]
        public void ShouldParallelHash256HashCorrectlyBitWise(int length, int digestSize, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetParallelHashHashFunction(digestSize, 512, false, blockSize, customization);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        [Test]
        [TestCase(192, 512, "000102030405060710111213141516172021222324252627", "c10a052722614684144d28474850b410757e3cba87651ba167a5cbddff7f466675fbf84bcae7378ac444be681d729499afca667fb879348bfdda427863c82f1c", 8, "")]
        [TestCase(192, 512, "000102030405060710111213141516172021222324252627", "538e105f1a22f44ed2f5cc1674fbd40be803d9c99bf5f8d90a2c8193f3fe6ea768e5c1a20987e2c9c65febed03887a51d35624ed12377594b5585541dc377efc", 8, "Parallel Data")]
        [TestCase(576, 512, "000102030405060708090a0b101112131415161718191a1b202122232425262728292a2b303132333435363738393a3b404142434445464748494a4b505152535455565758595a5b", "6b3e790b330c889a204c2fbc728d809f19367328d852f4002dc829f73afd6bcefb7fe5b607b13a801c0be5c1170bdb794e339458fdb0e62a6af3d42558970249", 12, "Parallel Data")]
        [TestCase(192, 256, "000102030405060710111213141516172021222324252627", "c10a052722614684144d28474850b410757e3cba87651ba167a5cbddff7f4666", 8, "")]
        [TestCase(192, 256, "000102030405060710111213141516172021222324252627", "538e105f1a22f44ed2f5cc1674fbd40be803d9c99bf5f8d90a2c8193f3fe6ea7", 8, "Parallel Data")]
        [TestCase(576, 256, "000102030405060708090a0b101112131415161718191a1b202122232425262728292a2b303132333435363738393a3b404142434445464748494a4b505152535455565758595a5b", "6b3e790b330c889a204c2fbc728d809f19367328d852f4002dc829f73afd6bce", 12, "Parallel Data")]
        public void ShouldParallelHashXOF256HashCorrectly(int length, int digestSize, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetParallelHashHashFunction(digestSize, 512, true, blockSize, customization);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        [Test]
        public void ShouldParallelHash256XOFHashCorrectlyBitWise(int length, int digestSize, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetParallelHashHashFunction(digestSize, 512, true, blockSize, customization);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Digest);
        }

        private HashFunction GetParallelHashHashFunction(int digestSize, int capacity, bool xof, int blockSize, string customization)
        {
            return new HashFunction()
            {
                DigestSize = digestSize,
                Capacity = capacity,
                XOF = xof,
                BlockSize = blockSize,
                Customization = customization
            };
        }
    }
}
