﻿using NIST.CVP.Crypto.Common.MAC.KMAC;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Crypto.CSHAKE;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KMAC.Tests
{
    [TestFixture, FastCryptoTest]
    public class KmacTests
    {
        private KmacFactory _kmacFactory;

        [SetUp]
        public void Setup()
        {
            _kmacFactory = new KmacFactory(new CSHAKEWrapper());
        }

        [Test]
        [TestCase(32, 256, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "e5780b0d3ea6f7d3a429c5706aa43a00fadbd7d49628839e3187243f456ee14e", "")]
        [TestCase(32, 256, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "3b1fba963cd8b0b59e8c1a6d71888b7143651af8ba0a7070c0979e2811324aa5", "My Tagged Application")]
        [TestCase(1600, 256, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7", "1f5b4e6cca02209e0dcb5ca635b89a15e271ecc760071dfd805faa38f9729230", "My Tagged Application")]
        [TestCase(256, 275, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f", "fb89a756507bb94e920943ea56a6ce202512e40cf7f6d79bbe380ba79f759dacd0d005", "My Function")]
        [TestCase(1607, 256, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c748", "83ca4cba3addd49f363bad03da260880fb99ede3926d06b0c20a3db253185b80", "My Function")]
        [TestCase(256, 256, 2407, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7c8c9cacbcccdcecfd0d1d2d3d4d5d6d7d8d9dadbdcdddedfe0e1e2e3e4e5e6e7e8e9eaebecedeeeff0f1f2f3f4f5f6f7f8f9fafbfcfdfeff000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f", "dea457f7eb7469f63f02b018da7400060d6164ef230bdeb7e73ccd8464f38725", "My Function")]
        public void ShouldKMAC128Correctly(int length, int macLength, int keyLen, string key, string inputHex, string outputHex, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex, macLength, false);
            
            var subject = _kmacFactory.GetKmacInstance(256, false);
            var result = subject.Generate(new BitString(key, keyLen, true), message, customization, macLength);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Mac);
        }

        [Test]
        [TestCase(32, 256, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "e5780b0d3ea6f7d3a429c5706aa43a00fadbd7d49628839e3187243f456ee14e", "")]
        [TestCase(32, 256, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "3b1fba963cd8b0b59e8c1a6d71888b7143651af8ba0a7070c0979e2811324aa5", "4d7920546167676564204170706c69636174696f6e")]
        public void ShouldKMACHexCustomizationCorrectly(int length, int macLength, int keyLen, string key, string inputHex, string outputHex, string customizationHex)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex, macLength, false);
            var customization = new BitString(customizationHex);

            var subject = _kmacFactory.GetKmacInstance(256, false);
            var result = subject.Generate(new BitString(key, keyLen, true), message, customization, macLength);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Mac);
        }

        [Test]
        [TestCase(32, 512, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "20c570c31346f703c9ac36c61c03cb64c3970d0cfc787e9b79599d273a68d2f7f69d4cc3de9d104a351689f27cf6f5951f0103f33f4f24871024d9c27773a8dd", "My Tagged Application")]
        [TestCase(1600, 512, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7", "75358cf39e41494e949707927cee0af20a3ff553904c86b08f21cc414bcfd691589d27cf5e15369cbbff8b9a4c2eb17800855d0235ff635da82533ec6b759b69", "")]
        [TestCase(1600, 512, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7", "b58618f71f92e1d56c1b8c55ddd7cd188b97b4ca4d99831eb2699a837da2e4d970fbacfde50033aea585f1a2708510c32d07880801bd182898fe476876fc8965", "My Tagged Application")]
        [TestCase(256, 275, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f", "fd54c330cb49287cc877c859a77153ce4026caae11a9d1fbc07eb922b9b5743b1d3804", "My Function")]
        [TestCase(1607, 256, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c748", "f248f8a2ee78cfdafb49761ce91c06ca5670b00d751eac7dc03c5350ba228faa", "My Function")]
        [TestCase(256, 256, 2407, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7c8c9cacbcccdcecfd0d1d2d3d4d5d6d7d8d9dadbdcdddedfe0e1e2e3e4e5e6e7e8e9eaebecedeeeff0f1f2f3f4f5f6f7f8f9fafbfcfdfeff000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f", "2515394698556660ca02ba53d2f1713b7492b28e8f161686ae982941ec889297", "My Function")]
        public void ShouldKMAC256Correctly(int length, int macLength, int keyLen, string key, string inputHex, string outputHex, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex, macLength, false);

            var subject = _kmacFactory.GetKmacInstance(512, false);
            var result = subject.Generate(new BitString(key, keyLen, true), message, customization, macLength);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Mac);
        }

        [Test]
        [TestCase(32, 256, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "cd83740bbd92ccc8cf032b1481a0f4460e7ca9dd12b08a0c4031178bacd6ec35", "")]
        [TestCase(32, 256, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "31a44527b4ed9f5c6101d11de6d26f0620aa5c341def41299657fe9df1a3b16c", "My Tagged Application")]
        [TestCase(1600, 256, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7", "47026c7cd793084aa0283c253ef658490c0db61438b8326fe9bddf281b83ae0f", "My Tagged Application")]
        [TestCase(32, 128, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "cd83740bbd92ccc8cf032b1481a0f446", "")]
        [TestCase(32, 128, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "31a44527b4ed9f5c6101d11de6d26f06", "My Tagged Application")]
        [TestCase(1600, 128, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7", "47026c7cd793084aa0283c253ef65849", "My Tagged Application")]
        [TestCase(256, 275, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f", "a989c1c81528a5a7be018962d89d9cbdd1ab82ad5bc35e1a3877cd7d819f5a18409407", "My Function")]
        [TestCase(1607, 256, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c748", "124df5776fff8edde733ce31af9d4010afb2d78782d385ee68130e77a50ae7bd", "My Function")]
        [TestCase(256, 256, 2407, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7c8c9cacbcccdcecfd0d1d2d3d4d5d6d7d8d9dadbdcdddedfe0e1e2e3e4e5e6e7e8e9eaebecedeeeff0f1f2f3f4f5f6f7f8f9fafbfcfdfeff000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f", "f7777da1d234b76441bb7297377be8c85214dd754e68a92beb83f7d397ae209e", "My Function")]
        public void ShouldKMACXOF128Correctly(int length, int macLength, int keyLen, string key, string inputHex, string outputHex, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex, macLength, false);

            var subject = _kmacFactory.GetKmacInstance(256, true);
            var result = subject.Generate(new BitString(key, keyLen, true), message, customization, macLength);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Mac);
        }

        [Test]
        [TestCase(32, 512, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "1755133f1534752aad0748f2c706fb5c784512cab835cd15676b16c0c6647fa96faa7af634a0bf8ff6df39374fa00fad9a39e322a7c92065a64eb1fb0801eb2b", "My Tagged Application")]
        [TestCase(1600, 512, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7", "ff7b171f1e8a2b24683eed37830ee797538ba8dc563f6da1e667391a75edc02ca633079f81ce12a25f45615ec89972031d18337331d24ceb8f8ca8e6a19fd98b", "")]
        [TestCase(1600, 512, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7", "d5be731c954ed7732846bb59dbe3a8e30f83e77a4bff4459f2f1c2b4ecebb8ce67ba01c62e8ab8578d2d499bd1bb276768781190020a306a97de281dcc30305d", "My Tagged Application")]
        [TestCase(32, 256, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "1755133f1534752aad0748f2c706fb5c784512cab835cd15676b16c0c6647fa9", "My Tagged Application")]
        [TestCase(1600, 256, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7", "ff7b171f1e8a2b24683eed37830ee797538ba8dc563f6da1e667391a75edc02c", "")]
        [TestCase(1600, 256, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7", "d5be731c954ed7732846bb59dbe3a8e30f83e77a4bff4459f2f1c2b4ecebb8ce", "My Tagged Application")]
        [TestCase(256, 275, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f", "b68f14ba54d1223a33d1f736b222bc4e38ead147538e7f4d238af16a8bca297f0c7f04", "My Function")]
        [TestCase(1607, 256, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c748", "1111810380347558199d14901fc268304440243262b5e7a6462ccc9f54ff4dc2", "My Function")]
        [TestCase(256, 256, 2407, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f808182838485868788898a8b8c8d8e8f909192939495969798999a9b9c9d9e9fa0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebfc0c1c2c3c4c5c6c7c8c9cacbcccdcecfd0d1d2d3d4d5d6d7d8d9dadbdcdddedfe0e1e2e3e4e5e6e7e8e9eaebecedeeeff0f1f2f3f4f5f6f7f8f9fafbfcfdfeff000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f", "df184001f61a4a86efc452cda2486a40a7715a9c866f8bd60d766c88560d51cb", "My Function")]
        public void ShouldKMACXOF256Correctly(int length, int macLength, int keyLen, string key, string inputHex, string outputHex, string customization)
        {
            var message = new BitString(inputHex, length, false);
            var expectedResult = new BitString(outputHex, macLength, false);
            
            var subject = _kmacFactory.GetKmacInstance(512, true);
            var result = subject.Generate(new BitString(key, keyLen, true), message, customization, macLength);

            Assume.That(result.Success);
            Assert.AreEqual(expectedResult, result.Mac);
        }
    }
}