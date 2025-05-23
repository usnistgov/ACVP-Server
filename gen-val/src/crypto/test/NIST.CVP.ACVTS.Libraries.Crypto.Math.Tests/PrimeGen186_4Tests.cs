﻿using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Math.Tests
{
    [TestFixture, LongCryptoTest]
    public class PrimeGen186_4Tests
    {
        private readonly IShaFactory _shaFactory = new NativeShaFactory();

        // From ProvableProbablePrimesWithConditions (B35)
        [Test]
        [TestCase("00c43e35054d734394b4448eeaeb8724173aa0955357af8545ce24411f62cf7e", 392,
            "de6453afdc2a7bad4c4199d20b928b4898db10716480fedf1e994425949fa3d7ca0fcee5064597b119c8faf50858cf33fb",
            "c43e35054d734394b4448eeaeb8724173aa0955357af8545ce24411f62d0b5",
            ModeValues.SHA2, DigestSizes.d256
        )]
        [TestCase("00c43e35054d734394b4448eeaeb8724173aa0955357af8545ce24411f62cf7e", 27,
            "06a77c79",
            "c43e35054d734394b4448eeaeb8724173aa0955357af8545ce24411f62cfb8",
            ModeValues.SHA2, DigestSizes.d256
        )]
        [TestCase("cad20639f564334910aed056da3b56f4253c5150632627f8d4f5b8803e941f4d", 440,
            "00b6fe6c2a9cc304ee8fed70909b7d999b8b5d9966997d9b6db637fb0cdf876454d773264e455af3de97b9d058e718876d87808d6b7057dd",
            "cad20639f564334910aed056da3b56f4253c5150632627f8d4f5b8803e941fd0",
            ModeValues.SHA2, DigestSizes.d512
        )]
        public void ShouldFullShaweTaylorCorrectly(string seedHex, int len, string primeHex, string primeSeedHex, ModeValues mode, DigestSizes dig)
        {
            var seed = new BitString(seedHex).ToPositiveBigInteger();

            var hashFunction = new HashFunction(mode, dig);
            var sha = _shaFactory.GetShaInstance(hashFunction);

            var result = PrimeGen186_4.ShaweTaylorRandomPrime(len, seed, sha);

            var prime = new BitString(primeHex).ToPositiveBigInteger();
            var primeseed = new BitString(primeSeedHex).ToPositiveBigInteger();

            Assert.That(result.Prime, Is.EqualTo(prime), "prime");
            Assert.That(result.PrimeSeed, Is.EqualTo(primeseed), "primeseed");
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
        public void ShouldShortShaweTaylorCorrectly(int len, string seedHex, string primeHex)
        {
            var seed = new BitString(seedHex).ToPositiveBigInteger();
            var sha = _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160));

            var result = PrimeGen186_4.ShaweTaylorRandomPrime(len, seed, sha);

            var prime = new BitString(primeHex).ToPositiveBigInteger();

            Assert.That(result.Prime, Is.EqualTo(prime));
        }
    }
}
