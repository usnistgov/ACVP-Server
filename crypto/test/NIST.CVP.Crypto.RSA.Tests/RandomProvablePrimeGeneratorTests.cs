using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Crypto.RSA.Tests
{
    [TestFixture]
    public class RandomProvablePrimeGeneratorTests
    {
        [Test]
        [TestCase(0, "010001", "ABCD")]
        public void ShouldFailWithBadParameters(int nlen, string e, string seed)
        {
            var subject = new RandomProvablePrimeGenerator();
            var result = subject.GeneratePrimes(nlen, new BitString(e).ToBigInteger(), new BitString(seed));
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase(0, "010001", "ABCD", "ABCD", "ABCD")]
        public void ShouldPassWithGoodParameters(int nlen, string e, string seed, string p, string q)
        {
            var subject = new RandomProvablePrimeGenerator();
            var result = subject.GeneratePrimes(nlen, new BitString(e).ToBigInteger(), new BitString(seed));
            Assert.IsTrue(result.Success);
            Assert.AreEqual(new BitString(p).ToBigInteger(), result.P, "p");
            Assert.AreEqual(new BitString(q).ToBigInteger(), result.Q, "q");
        }
    }
}
