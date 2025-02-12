﻿using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.ParallelHash.Tests
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
            var hashFunction = GetParallelHashHashFunction(256, 256, false);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message, blockSize, customization);

            Assert.That(result.Success);
            Assert.That(result.Digest, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(192, "000102030405060710111213141516172021222324252627", "ba8dc1d1d979331d3f813603c67f72609ab5e44b94a0b8f9af46514454a2b4f5", 8, "")]
        [TestCase(192, "000102030405060710111213141516172021222324252627", "fc484dcb3f84dceedc353438151bee58157d6efed0445a81f165e495795b7206", 8, "506172616c6c656c2044617461")]
        public void ShouldParallelHashHexCustomizationCorrectly(int length, string inputHex, string outputHex, int blockSize, string customizationHex)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var customization = new BitString(customizationHex);
            var hashFunction = GetParallelHashHashFunction(256, 256, false);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message, blockSize, customization);

            Assert.That(result.Success);
            Assert.That(result.Digest, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(1607, "000102030405060708090a0b0c0d0e0f1011121314151617", "5ecce5ca1708265cdce11cb73f5f59d4d1d20789a887b96adad3675fcdda9d7dbe8f121cd9886207ca8a585c2373952519228a202aa7841effebcef8dd68b110c62d66ed1d43969065669e39cc6290ea3371a0dd4c08b0a4f776b2fec7f01a56f957c2c596b66d1723176a85999c2a3e3fdcc785cc3eed3ef9335e1d5690ee5e54bf7178c44ce12440bb253f56aa5a61611df67f3ad4110c70e6e78a7e3300a9ae33a51e547e09da021a3cab20fa15e7e8a4c1a90d62aaeb44356c93db3c9d5385e22ff65d05437a71", 8, "My Function")]
        [TestCase(35, "000102030405060708090a0b0c0d0e0f1011121314151617", "261171c004", 8, "My Function")]
        [TestCase(16, "", "ef5e", 126, ")7HY7q< 'V!Rpq_l4:k(Ju$Q@:Qt]d1R:jRIQ~'CWSQ*ADPNMZx%e5t23v9{?FCBFN8&")]
        public void ShouldParallelHash128HashCorrectlyWithVariableOutput(int outputLength, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex);
            var expectedResult = new BitString(outputHex, outputLength, false);
            var hashFunction = GetParallelHashHashFunction(outputLength, 256, false);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message, blockSize, customization);

            Assert.That(result.Success);
            Assert.That(result.Digest.ToHex(), Is.EqualTo(expectedResult.ToHex()));
        }

        [Test]
        [TestCase(2407, 256, "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7c8c9cacbcccdcecfd0d1d2d3d4d5d6d7d8d9dadbdcdddedfe0e1e2e3e4e5e6e7e8e9eaebecedeeeff0f1f2f3f4f5f6f7f8f9fafbfcfdfeff000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c", "77b58c572f012d33d152a25a6cf352a3efc344a34c932ff071fd670cc9335d88", 8, "My Function")]
        [TestCase(279, 256, "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122", "85332d798f0d49d97c722b81b353c0113c289907f1cfadb7d6485c756612575a", 8, "My Function")]
        public void ShouldParallelHash128HashCorrectlyBitWise(int length, int digestSize, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetParallelHashHashFunction(digestSize, 256, false);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message, blockSize, customization);

            Assert.That(result.Success);
            Assert.That(result.Digest, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(192, 256, "000102030405060710111213141516172021222324252627", "fe47d661e49ffe5b7d999922c062356750caf552985b8e8ce6667f2727c3c8d3", 8, "")]
        [TestCase(192, 256, "000102030405060710111213141516172021222324252627", "ea2a793140820f7a128b8eb70a9439f93257c6e6e79b4a540d291d6dae7098d7", 8, "Parallel Data")]
        [TestCase(576, 256, "000102030405060708090a0b101112131415161718191a1b202122232425262728292a2b303132333435363738393a3b404142434445464748494a4b505152535455565758595a5b", "0127ad9772ab904691987fcc4a24888f341fa0db2145e872d4efd255376602f0", 12, "Parallel Data")]
        [TestCase(192, 128, "000102030405060710111213141516172021222324252627", "fe47d661e49ffe5b7d999922c0623567", 8, "")]
        [TestCase(192, 128, "000102030405060710111213141516172021222324252627", "ea2a793140820f7a128b8eb70a9439f9", 8, "Parallel Data")]
        [TestCase(576, 128, "000102030405060708090a0b101112131415161718191a1b202122232425262728292a2b303132333435363738393a3b404142434445464748494a4b505152535455565758595a5b", "0127ad9772ab904691987fcc4a24888f", 12, "Parallel Data")]
        [TestCase(192, 1607, "000102030405060708090a0b0c0d0e0f1011121314151617", "148afd6b4554041b42b072e5d7b4377098c38d6bf8d3192bc0dc1d2360662e1ad536be70c8d2004a86d99d09d476d34f64d051662513d30d41a02d62bf3aeaedcd25069490b572d6e295f8d6d75d4a5e127727a71c270ca332518c6c7c160518b724b6116fd403f8e211178f44b729deb4b6ee7063df8cfd63c0efc0c56bf5116c5d63e7437b176b35ce84a1c1d79eae73f747e6435b21a4c73f872e9b2892a257ae0c96703c67c1b85d1824cfb83c0e6a65654cf95d22be3240f36baf7a0a3f098a81bfa510f68c66", 8, "My Function")]
        [TestCase(192, 35, "000102030405060708090a0b0c0d0e0f1011121314151617", "148afd6b05", 8, "My Function")]
        public void ShouldParallelHash128XOFHashCorrectly(int length, int outLength, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex, outLength, false);
            var hashFunction = GetParallelHashHashFunction(outLength, 256, true);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message, blockSize, customization);

            Assert.That(result.Success);
            Assert.That(result.Digest, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(2407, 256, "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7c8c9cacbcccdcecfd0d1d2d3d4d5d6d7d8d9dadbdcdddedfe0e1e2e3e4e5e6e7e8e9eaebecedeeeff0f1f2f3f4f5f6f7f8f9fafbfcfdfeff000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c", "7ac7478866d146bcb0dab6db4a4fe76e27317817b040ccf53f90c93dffdc6a6a", 8, "My Function")]
        [TestCase(279, 256, "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122", "a65a74ee70181fddff87deebc72a3f2c97fc2dc914111eafe39ed7b891ca1703", 8, "My Function")]
        public void ShouldParallelHash128XOFHashCorrectlyBitWise(int length, int digestSize, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetParallelHashHashFunction(digestSize, 256, true);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message, blockSize, customization);

            Assert.That(result.Success);
            Assert.That(result.Digest, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(192, "000102030405060710111213141516172021222324252627", "bc1ef124da34495e948ead207dd9842235da432d2bbc54b4c110e64c451105531b7f2a3e0ce055c02805e7c2de1fb746af97a1dd01f43b824e31b87612410429", 8, "")]
        [TestCase(192, "000102030405060710111213141516172021222324252627", "cdf15289b54f6212b4bc270528b49526006dd9b54e2b6add1ef6900dda3963bb33a72491f236969ca8afaea29c682d47a393c065b38e29fae651a2091c833110", 8, "Parallel Data")]
        [TestCase(576, "000102030405060708090a0b101112131415161718191a1b202122232425262728292a2b303132333435363738393a3b404142434445464748494a4b505152535455565758595a5b", "69d0fcb764ea055dd09334bc6021cb7e4b61348dff375da262671cdec3effa8d1b4568a6cce16b1cad946ddde27f6ce2b8dee4cd1b24851ebf00eb90d43813e9", 12, "Parallel Data")]
        public void ShouldParallelHash256HashCorrectly(int length, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetParallelHashHashFunction(512, 512, false);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message, blockSize, customization);

            Assert.That(result.Success);
            Assert.That(result.Digest, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(1607, "000102030405060708090a0b0c0d0e0f1011121314151617", "4b67687720aa8244f2707775ca4eca9e1493fb25933069bf4bcff904c8c14651d977ad4dc9c339592fdbd82802af54d9242c2c81b8ef921a696b66b02ca47bd7614dd274ca2f58f17634af930afdbab98be29f33a0ead877da26576f6bac7b243bce5f59a1033292991fb31031b6fb8eb5921a233ffe5ae014a38efa1112a0c0f189508c0847fc4b193c839314f0a7cf6769bd848e6f73fccc47f8445bfc65401b832351a380a006840fced19cbc7c533300430fed03cba7078f44cac740f8e959c4786b128846ec23", 8, "My Function")]
        [TestCase(35, "000102030405060708090a0b0c0d0e0f1011121314151617", "615d36de02", 8, "My Function")]
        public void ShouldParallelHash256HashCorrectlyWithVariableOutput(int outputLength, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex);
            var expectedResult = new BitString(outputHex, outputLength, false);
            var hashFunction = GetParallelHashHashFunction(outputLength, 512, false);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message, blockSize, customization);

            Assert.That(result.Success);
            Assert.That(result.Digest, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(2407, 256, "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7c8c9cacbcccdcecfd0d1d2d3d4d5d6d7d8d9dadbdcdddedfe0e1e2e3e4e5e6e7e8e9eaebecedeeeff0f1f2f3f4f5f6f7f8f9fafbfcfdfeff000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c", "92542b304f383c1114ea0420a9102488978fda9ca9bbac2988fa269e23b91430", 8, "My Function")]
        [TestCase(279, 256, "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122", "e49596a6da5e9c75dde39456539fe76c711aba40868637fdf2a6969f5586bffa", 8, "My Function")]
        public void ShouldParallelHash256HashCorrectlyBitWise(int length, int digestSize, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetParallelHashHashFunction(digestSize, 512, false);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message, blockSize, customization);

            Assert.That(result.Success);
            Assert.That(result.Digest, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(192, 512, "000102030405060710111213141516172021222324252627", "c10a052722614684144d28474850b410757e3cba87651ba167a5cbddff7f466675fbf84bcae7378ac444be681d729499afca667fb879348bfdda427863c82f1c", 8, "")]
        [TestCase(192, 512, "000102030405060710111213141516172021222324252627", "538e105f1a22f44ed2f5cc1674fbd40be803d9c99bf5f8d90a2c8193f3fe6ea768e5c1a20987e2c9c65febed03887a51d35624ed12377594b5585541dc377efc", 8, "Parallel Data")]
        [TestCase(576, 512, "000102030405060708090a0b101112131415161718191a1b202122232425262728292a2b303132333435363738393a3b404142434445464748494a4b505152535455565758595a5b", "6b3e790b330c889a204c2fbc728d809f19367328d852f4002dc829f73afd6bcefb7fe5b607b13a801c0be5c1170bdb794e339458fdb0e62a6af3d42558970249", 12, "Parallel Data")]
        [TestCase(192, 256, "000102030405060710111213141516172021222324252627", "c10a052722614684144d28474850b410757e3cba87651ba167a5cbddff7f4666", 8, "")]
        [TestCase(192, 256, "000102030405060710111213141516172021222324252627", "538e105f1a22f44ed2f5cc1674fbd40be803d9c99bf5f8d90a2c8193f3fe6ea7", 8, "Parallel Data")]
        [TestCase(576, 256, "000102030405060708090a0b101112131415161718191a1b202122232425262728292a2b303132333435363738393a3b404142434445464748494a4b505152535455565758595a5b", "6b3e790b330c889a204c2fbc728d809f19367328d852f4002dc829f73afd6bce", 12, "Parallel Data")]
        [TestCase(192, 1607, "000102030405060708090a0b0c0d0e0f1011121314151617", "967539d68bb797972c2255f58dc921739565094b3ae4a98d49585d6b96e08255aee8611225a6483a8b331772ba9f78e9b98812f38fcf681016264e595c415b84eca20912c1243cf35141951c2293a3bbe8dac2f8f8dc6a02277c15b5e98da80573cf842d533ebdf6a6e15f1f2219af004661fe2638e552018f01b868e36ac1805a9f5240438f24dc5849f33db8dd98f87d0834631657808ada7d7fede692925c762c90b572796f642268b8a611a03354308c22da747b4c6723536b29d058cfee320133bcad62058838", 8, "My Function")]
        [TestCase(192, 35, "000102030405060708090a0b0c0d0e0f1011121314151617", "967539d603", 8, "My Function")]
        public void ShouldParallelHash256XOFHashCorrectly(int length, int outLength, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex, outLength, false);
            var hashFunction = GetParallelHashHashFunction(outLength, 512, true);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message, blockSize, customization);

            Assert.That(result.Success);
            Assert.That(result.Digest, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(2407, 256, "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7c8c9cacbcccdcecfd0d1d2d3d4d5d6d7d8d9dadbdcdddedfe0e1e2e3e4e5e6e7e8e9eaebecedeeeff0f1f2f3f4f5f6f7f8f9fafbfcfdfeff000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c", "9622ba7085ad82dbf1fe2f39f8d432b4e71de566ba7f298a446267ea213d6762", 8, "My Function")]
        [TestCase(279, 256, "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122", "56abc9d7c2f68f24b5d0278edea4b8e3dece1ebd5ca08dc56ddfaaa7cdb5a466", 8, "My Function")]
        public void ShouldParallelHash256XOFHashCorrectlyBitWise(int length, int digestSize, string inputHex, string outputHex, int blockSize, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetParallelHashHashFunction(digestSize, 512, true);

            var subject = new ParallelHash();
            var result = subject.HashMessage(hashFunction, message, blockSize, customization);

            Assert.That(result.Success);
            Assert.That(result.Digest, Is.EqualTo(expectedResult));
        }

        private HashFunction GetParallelHashHashFunction(int digestLength, int capacity, bool xof)
        {
            return new HashFunction(digestLength, capacity, xof);
        }

        [Test]
        public void MctInitalInnerLoop()
        {
            var mct = new ParallelHash_MCT(new ParallelHash(new ParallelHashFactory()));
            var random = new Random800_90();

            var hashFunction = GetParallelHashHashFunction(256, 256, true);

            var result = mct.MCTHash(
                hashFunction,
                new BitString("703B1C24E0119940AB77774F0BA0A38D"),
                new MathDomain().AddSegment(new RangeDomainSegment(random, 16, 65536, 1)),
                new MathDomain().AddSegment(new RangeDomainSegment(random, 32, 64, 1)),
                false, true);
        }
    }
}
