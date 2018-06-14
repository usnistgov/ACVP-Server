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
        [TestCase(32, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "e5780b0d3ea6f7d3a429c5706aa43a00fadbd7d49628839e3187243f456ee14e", "")]
        [TestCase(32, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "3b1fba963cd8b0b59e8c1a6d71888b7143651af8ba0a7070c0979e2811324aa5", "My Tagged Application")]
        [TestCase(1600, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7", "1f5b4e6cca02209e0dcb5ca635b89a15e271ecc760071dfd805faa38f9729230", "My Tagged Application")]
        public void ShouldKMAC128Correctly(int length, string key, string inputHex, string outputHex, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetCSHAKEHashFunction(256, 256);

            var CSHAKEWrapped = _cSHAKEFactory.GetCSHAKE(hashFunction);

            var subject = new Kmac(CSHAKEWrapped, 256, false);
            var result = subject.Generate(new BitString(key), new BitString(inputHex), customization, 256);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Mac);
        }

        [Test]
        [TestCase(32, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "20c570c31346f703c9ac36c61c03cb64c3970d0cfc787e9b79599d273a68d2f7f69d4cc3de9d104a351689f27cf6f5951f0103f33f4f24871024d9c27773a8dd", "My Tagged Application")]
        [TestCase(1600, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7", "75358cf39e41494e949707927cee0af20a3ff553904c86b08f21cc414bcfd691589d27cf5e15369cbbff8b9a4c2eb17800855d0235ff635da82533ec6b759b69", "")]
        [TestCase(1600, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7", "b58618f71f92e1d56c1b8c55ddd7cd188b97b4ca4d99831eb2699a837da2e4d970fbacfde50033aea585f1a2708510c32d07880801bd182898fe476876fc8965", "My Tagged Application")]
        public void ShouldKMAC256Correctly(int length, string key, string inputHex, string outputHex, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetCSHAKEHashFunction(256, 512);

            var CSHAKEWrapped = _cSHAKEFactory.GetCSHAKE(hashFunction);

            var subject = new Kmac(CSHAKEWrapped, 512, false);
            var result = subject.Generate(new BitString(key), new BitString(inputHex), customization, 512);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Mac);
        }

        [Test]
        [TestCase(32, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "cd83740bbd92ccc8cf032b1481a0f4460e7ca9dd12b08a0c4031178bacd6ec35", "")]
        [TestCase(32, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "31a44527b4ed9f5c6101d11de6d26f0620aa5c341def41299657fe9df1a3b16c", "My Tagged Application")]
        [TestCase(1600, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7", "47026c7cd793084aa0283c253ef658490c0db61438b8326fe9bddf281b83ae0f", "My Tagged Application")]
        public void ShouldKMACXOF128Correctly(int length, string key, string inputHex, string outputHex, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetCSHAKEHashFunction(256, 256);

            var CSHAKEWrapped = _cSHAKEFactory.GetCSHAKE(hashFunction);

            var subject = new Kmac(CSHAKEWrapped, 256, true);
            var result = subject.Generate(new BitString(key), new BitString(inputHex), customization, 256);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Mac);
        }

        [Test]
        [TestCase(32, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "1755133f1534752aad0748f2c706fb5c784512cab835cd15676b16c0c6647fa96faa7af634a0bf8ff6df39374fa00fad9a39e322a7c92065a64eb1fb0801eb2b", "My Tagged Application")]
        [TestCase(1600, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7", "ff7b171f1e8a2b24683eed37830ee797538ba8dc563f6da1e667391a75edc02ca633079f81ce12a25f45615ec89972031d18337331d24ceb8f8ca8e6a19fd98b", "")]
        [TestCase(1600, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7", "d5be731c954ed7732846bb59dbe3a8e30f83e77a4bff4459f2f1c2b4ecebb8ce67ba01c62e8ab8578d2d499bd1bb276768781190020a306a97de281dcc30305d", "My Tagged Application")]
        public void ShouldKMACXOF256Correctly(int length, string key, string inputHex, string outputHex, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex);
            var hashFunction = GetCSHAKEHashFunction(256, 512);

            var CSHAKEWrapped = _cSHAKEFactory.GetCSHAKE(hashFunction);

            var subject = new Kmac(CSHAKEWrapped, 512, true);
            var result = subject.Generate(new BitString(key), new BitString(inputHex), customization, 512);

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
