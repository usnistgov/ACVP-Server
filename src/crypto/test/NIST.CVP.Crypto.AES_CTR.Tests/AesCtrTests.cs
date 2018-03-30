using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;
using NUnit.Framework;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Crypto.AES_CTR.Tests
{
    [TestFixture, FastCryptoTest]
    public class AesCtrTests
    {
        [Test]
        [TestCase("2b7e151628aed2a6abf7158809cf4f3c", 
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "6bc1bee22e409f96e93d7e117393172a",
            "874d6191b620e3261bef6864990db6ce",
            TestName = "AES_CTR - Encrypt - 128")]
        [TestCase("8e73b0f7da0e6452c810f32b809079e562f8ead2522c6b7b",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "6bc1bee22e409f96e93d7e117393172a",
            "1abc932417521ca24f2b0459fe7e6e0b",
            TestName = "AES_CTR - Encrypt - 192")]
        [TestCase("603deb1015ca71be2b73aef0857d77811f352c073b6108d72d9810a30914dff4",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "6bc1bee22e409f96e93d7e117393172a",
            "601ec313775789a5b7a7f504bbf3d228",
            TestName = "AES_CTR - Encrypt - 256")]
        public void ShouldEncryptCorrectly(string keyHex, string ivHex, string ptHex, string ctHex)
        {
            var key = new BitString(keyHex);
            var iv = new BitString(ivHex);
            var pt = new BitString(ptHex);
            var ct = new BitString(ctHex);

            var subject = new AesCtr();

            var result = subject.EncryptBlock(key, pt, iv);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(ct, result.Result);
        }

        [Test]
        [TestCase("2b7e151628aed2a6abf7158809cf4f3c",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "6bc1bee22e409f96e93d7e1173931720",
            "874d6191b620e3261bef6864990db6c0",
            124,
            TestName = "AES_CTR - Partial Encrypt - 128")]
        [TestCase("8e73b0f7da0e6452c810f32b809079e562f8ead2522c6b7b",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "6bc1bee22e40",
            "1abc93241752",
            43,
            TestName = "AES_CTR - Partial Encrypt - 192")]
        [TestCase("603deb1015ca71be2b73aef0857d77811f352c073b6108d72d9810a30914dff4",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "60",
            "60",
            3,
            TestName = "AES_CTR - Partial Encrypt - 256")]
        public void ShouldEncryptPartialBlockCorrectly(string keyHex, string ivHex, string ptHex, string ctHex,
            int length)
        {
            var key = new BitString(keyHex);
            var iv = new BitString(ivHex);
            var pt = new BitString(ptHex, length);
            var ct = new BitString(ctHex, length);

            var subject = new AesCtr();

            var result = subject.EncryptBlock(key, pt, iv);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(ct, result.Result);
        }

        [Test]
        [TestCase("2b7e151628aed2a6abf7158809cf4f3c",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "874d6191b620e3261bef6864990db6ce",
            "6bc1bee22e409f96e93d7e117393172a",
            TestName = "AES_CTR - Decrypt - 128")]
        [TestCase("8e73b0f7da0e6452c810f32b809079e562f8ead2522c6b7b",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "1abc932417521ca24f2b0459fe7e6e0b",
            "6bc1bee22e409f96e93d7e117393172a",
            TestName = "AES_CTR - Decrypt - 192")]
        [TestCase("603deb1015ca71be2b73aef0857d77811f352c073b6108d72d9810a30914dff4",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "601ec313775789a5b7a7f504bbf3d228",
            "6bc1bee22e409f96e93d7e117393172a",
            TestName = "AES_CTR - Decrypt - 256")]
        public void ShouldDecryptCorrectly(string keyHex, string ivHex, string ctHex, string ptHex)
        {
            var key = new BitString(keyHex);
            var iv = new BitString(ivHex);
            var ct = new BitString(ctHex);
            var pt = new BitString(ptHex);

            var subject = new AesCtr();

            var result = subject.DecryptBlock(key, ct, iv);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(pt, result.Result);
        }
    }
}
