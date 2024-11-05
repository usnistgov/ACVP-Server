using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA.PrimeGenerators;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.Tests.PrimeGenerators
{
    // About 5 seconds
    [TestFixture, LongCryptoTest]
    public class PrimeGeneratorHelperTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            _random = new Random800_90();
            _entropyProvider = new EntropyProvider(_random);
        }

        private IEntropyProvider _entropyProvider;
        private IRandom800_90 _random;

        // B32
        [Test]
        [TestCase(1024, 1, 1, "be075a0bcdf2f2785d85acb63ff37638bb3bdb87c4ad1476d21eb12d", "5373c7", "fdd7fee40b6b3832e02edf01f5da5c8f0a5be18b6e06211ef6324c57081685813914e1bd19c3b5007219853710be02ae3f8c7b3f800a3cc6098db692518e1c58f2f899a98a48ff3a36e0596b98e427171ca69e4ee13ae9fcb7156ebbaf413b390b91c4682cc15438d0ba13872b929cb7089fe8f7445c48fa681c60e5fb38a24b")]
        [TestCase(1024, 1, 1, "c6c4a2a68995815536423c31aade68add7fad655921a601bfff35b97", "7dc47b", "cf6c3a760e20b3d4c3e269b9a59c99bb795578ea3a0d4662e50ae9b79955e9fee8b265bc942f7c1ac4b8c6b7f92caa5b4728bbfeb234de6275e01c2328a4016f51b44ddd2a99d8f9e1d0ddfa2ab9a495f11f6becff614a9cc68a00893f67b2e891cc2a246f2e6087b8d268975cde81fedcbcb3e3d1a79de6ed988e318ca9c8f5")]
        public void ShouldProvablePrimeConstructionCorrectly(int L, int N1, int N2, string seed, string e, string prime)
        {
            var sha = new NativeShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160));
            var result = PrimeGeneratorHelper.ProvablePrimeConstruction(sha, L, N1, N2, new BitString(seed).ToPositiveBigInteger(), new BitString(e).ToPositiveBigInteger());
            Assert.That(new BitString(prime).ToPositiveBigInteger(), Is.EqualTo(result.Prime));
        }

        [Test]
        [TestCase(2048, true)]
        [TestCase(4096, true)]
        [TestCase(6144, true)]
        [TestCase(8192, true)]
        public void ShouldProduceKeyForTiming(int modulo, bool ourCrypto)
        {
            var sha = new NativeShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));

            RunRsaKeyGen(sha, modulo, ourCrypto);

            Assert.That(true, Is.True, $"Modulo: {modulo}, ourCrypto: {ourCrypto}");
        }

        private void RunRsaKeyGen(ISha sha, int modulo, bool ourCrypto)
        {
            if (ourCrypto)
            {
                var keyComposer = new KeyComposerFactory().GetKeyComposer(PrivateKeyModes.Standard);
                var keyBuilder = new KeyBuilder(new PrimeGeneratorFactory())
                    .WithBitlens(KeyGenHelper.GetBitlens(_random, modulo, PrimeGenModes.RandomProbablePrimesWithAuxiliaryProbablePrimes))
                    .WithNlen(modulo)
                    .WithStandard(Fips186Standard.Fips186_5)
                    .WithHashFunction(sha)
                    .WithKeyComposer(keyComposer)
                    .WithPrimeGenMode(PrimeGenModes.RandomProbablePrimesWithAuxiliaryProbablePrimes)
                    .WithPrimeTestMode(PrimeTestModes.TwoPowSecurityStrengthErrorBound)
                    .WithEntropyProvider(_entropyProvider)
                    .WithPublicExponent(65537);

                var key = keyBuilder.Build();

                Assert.That(key.Success, Is.True, "Failed to generate key pair");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        [TestCase(4096)]
        [TestCase(6144)]
        [TestCase(8192)]
        public void ShouldGetRoot2Mult2PowValForModulo(int modulo)
        {
            var bs = new BitString(PrimeGeneratorHelper.Root2Mult2Pow7680Minus1).GetMostSignificantBits(modulo / 2);

            Assert.That(bs.ToHex(), Is.EqualTo(GetAppropriateBitStringBasedOnModulo(modulo).ToHex()));
        }

        private BitString GetAppropriateBitStringBasedOnModulo(int modulo)
        {
            switch (modulo)
            {
                case 4096:
                    return new BitString(PrimeGeneratorHelper.Root2Mult2Pow2048Minus1);
                case 6144:
                    return new BitString(PrimeGeneratorHelper.Root2Mult2Pow3072Minus1);
                case 8192:
                    return new BitString(PrimeGeneratorHelper.Root2Mult2Pow4096Minus1);
            }

            throw new ArgumentOutOfRangeException(nameof(modulo));
        }
    }
}
