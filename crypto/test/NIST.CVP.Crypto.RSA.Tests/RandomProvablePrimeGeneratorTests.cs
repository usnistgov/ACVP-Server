using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Crypto.RSA.Tests
{
    [TestFixture]
    public class RandomProvablePrimeGeneratorTests
    {
        [Test]
        [TestCase(0, "010001", "ABCD")]
        [TestCase(2048, "03", "ABCD")]
        public void ShouldFailWithBadParameters(int nlen, string e, string seed)
        {
            var subject = new RandomProvablePrimeGenerator();
            var result = subject.GeneratePrimes(nlen, new BitString(e).ToPositiveBigInteger(), new BitString(seed));
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase(2048, "0100000001", "e5f707e49c4e7cc8fb202b5cd957963713f1c4726677c09b6a7f5dfe", "ff03b1a74827c746db83d2eaff00067622f545b62584321256e62b01509f10962f9c5c8fd0b7f5184a9ce8e81f439df47dda14563dd55a221799d2aa57ed2713271678a5a0b8b40a84ad13d5b6e6599e6467c670109cf1f45ccfed8f75ea3b814548ab294626fe4d14ff764dd8b091f11a0943a2dd2b983b0df02f4c4d00b413", "dacaabc1dc57faa9fd6a4274c4d588765a1d3311c22e57d8101431b07eb3ddcb05d77d9a742ac2322fe6a063bd1e05acb13b0fe91c70115c2b1eee1155e072527011a5f849de7072a1ce8e6b71db525fbcda7a89aaed46d27aca5eaeaf35a26270a4a833c5cda681ffd49baa0f610bad100cdf47cc86e5034e2a0b2179e04ec7")]
        public void ShouldPassWithGoodParameters(int nlen, string e, string seed, string p, string q)
        {
            var subject = new RandomProvablePrimeGenerator(new HashFunction {Mode = ModeValues.SHA1, DigestSize = DigestSizes.d160});
            var result = subject.GeneratePrimes(nlen, new BitString(e).ToPositiveBigInteger(), new BitString(seed));
            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(new BitString(p).ToPositiveBigInteger(), result.P, "p");
            Assert.AreEqual(new BitString(q).ToPositiveBigInteger(), result.Q, "q");
        }
    }
}
