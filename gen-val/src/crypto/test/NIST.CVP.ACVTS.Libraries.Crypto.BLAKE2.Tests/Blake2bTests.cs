using NIST.CVP.ACVTS.Libraries.Crypto.Blake2;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.Blake2;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.BLAKE2.Tests
{
    [TestFixture, FastCryptoTest]
    public class Blake2bTests
    {
        [Test]
        [TestCase("", "786a02f742015903c6c6fd852552d272912f4740e15847618a86e217f71f5419d25e1031afee585313896444934eb04b903a685b1448b755d56f701afe9be2ce")]
        [TestCase("00", "2fa3f686df876995167e7c2e5d74c4c7b6e48f8068fe0e44208344d480f7904c36963e44115fe3eb2a3ac8694c28bcb4f5a0f3276f2e79487d8219057a506e4b")]
        [TestCase("0001", "1c08798dc641aba9dee435e22519a4729a09b2bfe0ff00ef2dcd8ed6f8a07d15eaf4aee52bbf18ab5608a6190f70b90486c8a7d4873710b1115d3debbb4327b5")]
        [TestCase("000102", "40a374727302d9a4769c17b5f409ff32f58aa24ff122d7603e4fda1509e919d4107a52c57570a6d94e50967aea573b11f86f473f537565c66f7039830a85d186")]
        public void ShouldHashUnkeyedKatMessages(string messageHex, string expectedDigestHex)
        {
            var subject = new Blake2Factory().GetBlake2Instance(new Blake2HashFunction(Blake2Variant.Blake2b, 512));

            var result = subject.HashMessage(new BitString(messageHex));

            Assert.That(result.Success);
            Assert.That(result.Digest.ToHex(), Is.EqualTo(expectedDigestHex.ToUpperInvariant()));
        }

        [Test]
        [TestCase("", "10ebb67700b1868efb4417987acf4690ae9d972fb7a590c2f02871799aaa4786b5e996e8f0f4eb981fc214b005f42d2ff4233499391653df7aefcbc13fc51568")]
        [TestCase("00", "961f6dd1e4dd30f63901690c512e78e4b45e4742ed197c3c5e45c549fd25f2e4187b0bc9fe30492b16b0d0bc4ef9b0f34c7003fac09a5ef1532e69430234cebd")]
        [TestCase("0001", "da2cfbe2d8409a0f38026113884f84b50156371ae304c4430173d08a99d9fb1b983164a3770706d537f49e0c916d9f32b95cc37a95b99d857436f0232c88a965")]
        public void ShouldHashKeyedKatMessages(string messageHex, string expectedDigestHex)
        {
            var subject = new Blake2Factory().GetBlake2Instance(new Blake2HashFunction(Blake2Variant.Blake2b, 512));
            var key = new BitString("000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f");

            var result = subject.HashMessage(new BitString(messageHex), key);

            Assert.That(result.Success);
            Assert.That(result.Digest.ToHex(), Is.EqualTo(expectedDigestHex.ToUpperInvariant()));
        }

        [Test]
        public void ShouldSupportShorterDigestLength()
        {
            var subject = new Blake2Factory().GetBlake2Instance(new Blake2HashFunction(Blake2Variant.Blake2b, 256));

            var result = subject.HashMessage(new BitString(""));

            Assert.That(result.Success);
            Assert.That(result.Digest.BitLength, Is.EqualTo(256));
            Assert.That(result.Digest.ToHex(), Is.EqualTo("0E5751C026E543B2E8AB2EB06099DAA1D1E5DF47778F7787FAAB45CDF12FE3A8"));
        }
    }
}
