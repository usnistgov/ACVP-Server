using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA2.Tests
{
    // About 5 seconds
    [TestFixture, LongCryptoTest]
    public class PrimeGeneratorBaseTests
    {
        // B32
        [Test]
        // C10-0.txt
        [TestCase(1024, 1, 1, "be075a0bcdf2f2785d85acb63ff37638bb3bdb87c4ad1476d21eb12d", "5373c7", "fdd7fee40b6b3832e02edf01f5da5c8f0a5be18b6e06211ef6324c57081685813914e1bd19c3b5007219853710be02ae3f8c7b3f800a3cc6098db692518e1c58f2f899a98a48ff3a36e0596b98e427171ca69e4ee13ae9fcb7156ebbaf413b390b91c4682cc15438d0ba13872b929cb7089fe8f7445c48fa681c60e5fb38a24b")]
        // C10-8.txt
        [TestCase(1024, 1, 1, "c6c4a2a68995815536423c31aade68add7fad655921a601bfff35b97", "7dc47b", "cf6c3a760e20b3d4c3e269b9a59c99bb795578ea3a0d4662e50ae9b79955e9fee8b265bc942f7c1ac4b8c6b7f92caa5b4728bbfeb234de6275e01c2328a4016f51b44ddd2a99d8f9e1d0ddfa2ab9a495f11f6becff614a9cc68a00893f67b2e891cc2a246f2e6087b8d268975cde81fedcbcb3e3d1a79de6ed988e318ca9c8f5")]
        public void ShouldProvablePrimeConstructionCorrectly(int L, int N1, int N2, string seed, string e, string prime)
        {
            var sha = new ShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160));
            var subject = new PrimeGeneratorBase(sha);

            var result = subject.ProvablePrimeConstruction(L, N1, N2, new BitString(seed).ToPositiveBigInteger(), new BitString(e).ToPositiveBigInteger());
            Assert.AreEqual(result.Prime, new BitString(prime).ToPositiveBigInteger());
        }
    }
}
