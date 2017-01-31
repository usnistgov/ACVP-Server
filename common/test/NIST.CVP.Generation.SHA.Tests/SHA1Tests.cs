using NIST.CVP.Math;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA.Tests
{
    [TestFixture]
    public class SHA1Tests
    {
        private static object[] hashTests = new object[]
        {
            new object[]
            {
                new HashFunction
                {
                    DigestSize = DigestSizes.d160,
                    Mode = ModeValues.SHA1
                },
                "",
                "da39a3ee5e6b4b0d3255bfef95601890afd80709"
            },
            new object[]
            {
                new HashFunction
                {
                    DigestSize = DigestSizes.d160,
                    Mode = ModeValues.SHA1
                },
                "c0fabb1f2dc66055",
                "b137b2b7a60e8d2f0552d0ddc3dc960245fffe6a"
            },
            new object[]
            {
                new HashFunction
                {
                    DigestSize = DigestSizes.d160,
                    Mode = ModeValues.SHA1
                },
                "638985ba4eed2b8ad7a546b620a1105b6578d86278090c4a6d62982796e16eebb29866e561f64987dba4286ce2aef39af5e34704c77e8653ef062de5e17262161d91cdbfa6a9a9fdb65f1b34b0d6c253561b8f593cc1d7187cc8a638acc457800d3a6151054e7473d09bc5157263a60ef0e85969bf1926217d71ab29df1d74afeb5dcba2672cd1729123ce17109bc6542b124d3d39d09bf758c9e3bf62c6e12d1dc0b3",
                "fd7f2c5502a682eaf2977d7d12cc1616feb97c3e"
            },
        };

        [Test]
        [TestCaseSource(nameof(hashTests))]
        public void ShouldSHA1HashCorrectly(HashFunction hashFunction, string messageHex, string digestHex)
        {
            var sha1 = new SHA1(new SHAInternals(hashFunction));
            var message = new BitString(messageHex);
            var expectedDigest = new BitString(digestHex);

            var result = sha1.HashMessage(message);

            Assert.AreEqual(expectedDigest.ToHex(), result.ToHex());
        }
    }
}
