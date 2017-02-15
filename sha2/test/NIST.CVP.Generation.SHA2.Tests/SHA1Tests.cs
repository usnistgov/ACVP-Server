using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture]
    public class SHA1Tests
    {
        [Test]
        [TestCase("", "da39a3ee5e6b4b0d3255bfef95601890afd80709")]
        [TestCase("c0fabb1f2dc66055", "b137b2b7a60e8d2f0552d0ddc3dc960245fffe6a")]
        [TestCase("638985ba4eed2b8ad7a546b620a1105b6578d86278090c4a6d62982796e16eebb29866e561f64987dba4286ce2aef39af5e34704c77e8653ef062de5e17262161d91cdbfa6a9a9fdb65f1b34b0d6c253561b8f593cc1d7187cc8a638acc457800d3a6151054e7473d09bc5157263a60ef0e85969bf1926217d71ab29df1d74afeb5dcba2672cd1729123ce17109bc6542b124d3d39d09bf758c9e3bf62c6e12d1dc0b3", "fd7f2c5502a682eaf2977d7d12cc1616feb97c3e")]
        public void ShouldSHA1HashCorrectly(string messageHex, string digestHex)
        {
            var hashFunction = new HashFunction
            {
                Mode = ModeValues.SHA1,
                DigestSize = DigestSizes.d160
            };

            var sha1 = new SHA1(new SHAInternals(hashFunction));
            var message = new BitString(messageHex);
            var expectedDigest = new BitString(digestHex);

            var result = sha1.HashMessage(message);

            Assert.AreEqual(expectedDigest, result);
        }

        [Test]
        [TestCase(1, "00", "bb6b3e18f0115b57925241676f5b1ae88747b08a")]
        [TestCase(2, "c0", "d90631a32faf316a87b9582bfa4e05a2773005ca")]
        [TestCase(9, "ff80", "7bd5813934a8a67115358b1a5f3c5b97192b7b3b")]
        [TestCase(611, "513b414e77fba286ff1b612d9cc9038618fdafe8015c87bf9f5d39d328aad0cd37c9d1f77bfb8343ac648e8fc46dc7276b674b8bb371cb73059b26115c4b3d96b0e003f7b696d3f1c6a9074b00", "261730f3143cc63add4ec324e3fe2f9ab37d347e")]
        [TestCase(1007, "d271aa31b8b92606a10a52612dd1fab495b82f9a98cade18b9d8a723a71ceb63fd1d27372bd281f9b40aa1839b0cc2f2177a09aa8e7b159ac118d7c145e7a4f032e788d21facde2b4dbc1d5d2238f530d9bf9bd2798f611d03ed8919f0c85bc2da99750b7a8d6322d2e66ff6ab9ebaf7424e8c1c3f4fe92be61f65359106", "0ca6c7fa9aa75bd0d9420b7e4bcf017579d95b70")]
        public void ShouldSHA1HashCorrectlyWithBits(int len, string messageHex, string digestHex)
        {
            var hashFunction = new HashFunction
            {
                Mode = ModeValues.SHA1,
                DigestSize = DigestSizes.d160
            };

            var sha1 = new SHA1(new SHAInternals(hashFunction));
            var message = new BitString(messageHex, len);
            var expectedDigest = new BitString(digestHex);

            var result = sha1.HashMessage(message);

            Assert.AreEqual(expectedDigest, result);
        }
    }
}
