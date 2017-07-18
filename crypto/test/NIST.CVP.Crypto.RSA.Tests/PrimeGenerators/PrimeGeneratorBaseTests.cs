using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.RSA.Tests.PrimeGenerators
{
    [TestFixture, FastIntegrationTest]
    public class PrimeGeneratorBaseTests
    {
        // From ProvableProbablePrimesWithConditions (B35)
        [Test]
        [TestCase("00c43e35054d734394b4448eeaeb8724173aa0955357af8545ce24411f62cf7e", 392,
            "de6453afdc2a7bad4c4199d20b928b4898db10716480fedf1e994425949fa3d7ca0fcee5064597b119c8faf50858cf33fb",
            "c43e35054d734394b4448eeaeb8724173aa0955357af8545ce24411f62d0b5",
            ModeValues.SHA2, DigestSizes.d256)]
        [TestCase("00c43e35054d734394b4448eeaeb8724173aa0955357af8545ce24411f62cf7e", 27,
            "06a77c79",
            "c43e35054d734394b4448eeaeb8724173aa0955357af8545ce24411f62cfb8",
            ModeValues.SHA2, DigestSizes.d256)]
        [TestCase("cad20639f564334910aed056da3b56f4253c5150632627f8d4f5b8803e941f4d", 440,
            "00b6fe6c2a9cc304ee8fed70909b7d999b8b5d9966997d9b6db637fb0cdf876454d773264e455af3de97b9d058e718876d87808d6b7057dd",
            "cad20639f564334910aed056da3b56f4253c5150632627f8d4f5b8803e941fd0",
            ModeValues.SHA2, DigestSizes.d512)]
        public void ShouldSTCorrectly(string seedHex, int len, string primeHex, string primeSeedHex, ModeValues mode, DigestSizes dig)
        {
            var subject = new FakePrimeGenerator();
            subject.SetHashFunction(new HashFunction { Mode = mode, DigestSize = dig });

            var seed = new BitString(seedHex).ToPositiveBigInteger();
            var result = subject.STTest(len, seed);

            var prime = new BitString(primeHex).ToPositiveBigInteger();
            var primeseed = new BitString(primeSeedHex).ToPositiveBigInteger();

            Assert.AreEqual(prime, result.Prime, "prime");
            Assert.AreEqual(primeseed, result.PrimeSeed, "primeseed");
        }

        // B32
        // Only short tests here because anything larger triggers a call from CAVS where 'outlen = 1' for SHA1 when it should be 'outlen = 160'
        [Test]
        [TestCase(18, "b24d92a1d4a8c5cefd8e93e2db24936262259c8cac122325e28f0850", "0002ee35")]
        [TestCase(18, "4341382476e1feba95d4e8e4d63bd9adda918a761a169efe27c6217f", "00035413")]
        [TestCase(18, "32d0f92233675392519e0b9650e42721a3dc740caacc0d1fb6a6b39d", "00038525")]
        [TestCase(18, "6fcaf1e5e02dff1b08480b0483e32902ed4fcbd90414113b1e90353d", "0003db3b")]
        [TestCase(18, "6fcaf1e5e02dff1b08480b0483e32902ed4fcbd90414113b1e937750", "00033ceb")]
        [TestCase(18, "4ab3e23a488eae6d0fe5f2c284a7ec6ca5a9366f815527e6e2e28fb8", "00036757")]
        [TestCase(18, "4ab3e23a488eae6d0fe5f2c284a7ec6ca5a9366f815527e6e2e0f5ad", "00038e9d")]
        [TestCase(18, "32d0f92233675392519e0b9650e42721a3dc740caacc0d1fb6a83429", "0003c5a1")]
        public void ShouldShortShaweTaylorCorrectly(int len, string seed, string prime)
        {
            var subject = new FakePrimeGenerator();
            subject.SetHashFunction(new HashFunction { Mode = ModeValues.SHA1, DigestSize = DigestSizes.d160 });

            var result = subject.STTest(len, new BitString(seed).ToPositiveBigInteger());
            Assert.AreEqual(new BitString(prime).ToPositiveBigInteger(), result.Prime);
        }

        // B32
        [Test]
        // C10-0.txt
        [TestCase(1024, 1, 1, "be075a0bcdf2f2785d85acb63ff37638bb3bdb87c4ad1476d21eb12d", "5373c7", "fdd7fee40b6b3832e02edf01f5da5c8f0a5be18b6e06211ef6324c57081685813914e1bd19c3b5007219853710be02ae3f8c7b3f800a3cc6098db692518e1c58f2f899a98a48ff3a36e0596b98e427171ca69e4ee13ae9fcb7156ebbaf413b390b91c4682cc15438d0ba13872b929cb7089fe8f7445c48fa681c60e5fb38a24b")]
        // C10-8.txt
        [TestCase(1024, 1, 1, "c6c4a2a68995815536423c31aade68add7fad655921a601bfff35b97", "7dc47b", "cf6c3a760e20b3d4c3e269b9a59c99bb795578ea3a0d4662e50ae9b79955e9fee8b265bc942f7c1ac4b8c6b7f92caa5b4728bbfeb234de6275e01c2328a4016f51b44ddd2a99d8f9e1d0ddfa2ab9a495f11f6becff614a9cc68a00893f67b2e891cc2a246f2e6087b8d268975cde81fedcbcb3e3d1a79de6ed988e318ca9c8f5")]
        public void ShouldProvablePrimeConstructionCorrectly(int L, int N1, int N2, string seed, string e, string prime)
        {
            var subject = new FakePrimeGenerator();
            subject.SetHashFunction(new HashFunction { Mode = ModeValues.SHA1, DigestSize = DigestSizes.d160 });

            var result = subject.PPCTest(L, N1, N2, new BitString(seed).ToPositiveBigInteger(), new BitString(e).ToPositiveBigInteger());
            Assert.AreEqual(result.P, new BitString(prime).ToPositiveBigInteger());
        }

        [Test]
        [TestCase("b24d92a1d4a8c5cefd8e93e2db24936262259c8cac122325e28f0850", "cdb2685d10b535aead5b8f29f2b13ef59ea9f1e5",
            ModeValues.SHA1, DigestSizes.d160)]
        [TestCase("00c43e35054d734394b4448eeaeb8724173aa0955357af8545ce24411f62cf7e", "3C51747150C2B0C33CD4C1B63F9E5608F0B8A2CCC88A0051BF6BA914C9B2D8B8", ModeValues.SHA2, DigestSizes.d256)]
        public void SmallHashTest(string primeSeedHex, string expectedHex, ModeValues mode, DigestSizes dig)
        {
            var subject = new FakePrimeGenerator();
            subject.SetHashFunction(new HashFunction { Mode = mode, DigestSize = dig });

            var primeSeed = new BitString(primeSeedHex).ToPositiveBigInteger();
            var bytearr = primeSeed.ToByteArray();
            var result = subject.Hash(primeSeed) ^ subject.Hash(primeSeed + 1);

            var expectedResult = new BitString(expectedHex).ToPositiveBigInteger();
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void ShouldSOmething()
        {
            var input_seedWithZero = new BitString("00c43e35054d734394b4448eeaeb8724173aa0955357af8545ce24411f62cf7e").ToPositiveBigInteger();
            var a = input_seedWithZero.ToByteArray();

            var input_seedWithoutZero = new BitString("cad20639f564334910aed056da3b56f4253c5150632627f8d4f5b8803e941f4d").ToPositiveBigInteger();
            var b = input_seedWithoutZero.ToByteArray();

            var inputTesting = new BitString("7e").ToPositiveBigInteger();
            var c = inputTesting.ToByteArray();

            var inputTesting1MostSignificant = new BitString("8e").ToPositiveBigInteger();
            var d = inputTesting1MostSignificant.ToByteArray();

            int i = 5;
        }
    }
}
