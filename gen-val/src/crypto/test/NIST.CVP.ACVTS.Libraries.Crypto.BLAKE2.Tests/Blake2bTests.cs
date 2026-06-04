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
        /// <summary>
        /// Checks official unkeyed BLAKE2b known answer vectors for small messages that are aligned to whole bytes.
        /// This covers the basic hash path without a key, which is used by normal hash style registrations.
        /// </summary>
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

        /// <summary>
        /// Checks messages around the 128 byte BLAKE2b compression block boundary.
        /// These KATs exercise final-block handling for one byte short, exactly one block, and one byte over.
        /// </summary>
        [Test]
        [TestCase("000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e", "b6292669ccd38d5f01caae96ba272c76a879a45743afa0725d83b9ebb26665b731f1848c52f11972b6644f554c064fa90780dbbbf3a89d4fc31f67df3e5857ef")]
        [TestCase("000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f", "2319e3789c47e2daa5fe807f61bec2a1a6537fa03f19ff32e87eecbfd64b7e0e8ccff439ac333b040f19b0c4ddd11a61e24ac1fe0f10a039806c5dcc0da3d115")]
        [TestCase("000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f606162636465666768696a6b6c6d6e6f707172737475767778797a7b7c7d7e7f80", "f59711d44a031d5f97a9413c065d1e614c417ede998590325f49bad2fd444d3e4418be19aec4e11449ac1a57207898bc57d76a1bcf3566292c20c683a5c4648f")]
        public void ShouldHashBlockBoundaryKatMessages(string messageHex, string expectedDigestHex)
        {
            var subject = new Blake2Factory().GetBlake2Instance(new Blake2HashFunction(Blake2Variant.Blake2b, 512));

            var result = subject.HashMessage(new BitString(messageHex));

            Assert.That(result.Success);
            Assert.That(result.Digest.ToHex(), Is.EqualTo(expectedDigestHex.ToUpperInvariant()));
        }

        /// <summary>
        /// Checks official keyed BLAKE2b known answer vectors with a 64 byte key.
        /// This covers keyed hashing and configures the native implementation with BLAKE2's key length field.
        /// </summary>
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

        /// <summary>
        /// Checks that BLAKE2b supports digest lengths shorter than the default 512 bit digest.
        /// This covers the configurable output size path that ACVP registrations may need to model.
        /// </summary>
        [Test]
        public void ShouldSupportShorterDigestLength()
        {
            var subject = new Blake2Factory().GetBlake2Instance(new Blake2HashFunction(Blake2Variant.Blake2b, 256));

            var result = subject.HashMessage(new BitString(""));

            Assert.That(result.Success);
            Assert.That(result.Digest.BitLength, Is.EqualTo(256));
            Assert.That(result.Digest.ToHex(), Is.EqualTo("0E5751C026E543B2E8AB2EB06099DAA1D1E5DF47778F7787FAAB45CDF12FE3A8"));
        }

        /// <summary>
        /// Checks that the factory preserves the requested variant and digest metadata on the returned implementation.
        /// This is a small framework shape test: callers can inspect the selected BLAKE2 function after construction.
        /// </summary>
        [Test]
        public void ShouldExposeHashFunctionMetadata()
        {
            var subject = new Blake2Factory().GetBlake2Instance(new Blake2HashFunction(Blake2Variant.Blake2b, 384));

            Assert.That(subject.HashFunction.Variant, Is.EqualTo(Blake2Variant.Blake2b));
            Assert.That(subject.HashFunction.DigestLength, Is.EqualTo(384));
        }

        /// <summary>
        /// Checks that messages with partial bytes are rejected instead of silently hashed as padded bytes.
        /// This protects the demo implementation until a bit level BLAKE2 message policy is intentionally added.
        /// </summary>
        [Test]
        public void ShouldRejectNonByteAlignedMessages()
        {
            var subject = new Blake2Factory().GetBlake2Instance(new Blake2HashFunction(Blake2Variant.Blake2b, 512));

            var result = subject.HashMessage(new BitString("0F", 7));

            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("BLAKE2b currently supports byte-aligned messages only."));
        }

        /// <summary>
        /// Checks that keys with partial bytes are rejected instead of silently hashed as padded bytes.
        /// This keeps keyed hashing behavior explicit and aligned to whole bytes while the ACVP parameter shape is still pending.
        /// </summary>
        [Test]
        public void ShouldRejectNonByteAlignedKeys()
        {
            var subject = new Blake2Factory().GetBlake2Instance(new Blake2HashFunction(Blake2Variant.Blake2b, 512));

            var result = subject.HashMessage(new BitString("00"), new BitString("0F", 7));

            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("BLAKE2b currently supports byte-aligned keys only."));
        }

        /// <summary>
        /// Checks that BLAKE2b construction enforces the algorithm's 512 bit maximum digest size.
        /// This checks the factory/implementation boundary before invalid configuration reaches the native code.
        /// </summary>
        [Test]
        public void ShouldRejectDigestLengthAboveBlake2bMaximum()
        {
            Assert.Throws<System.ArgumentException>(() =>
                new Blake2Factory().GetBlake2Instance(new Blake2HashFunction(Blake2Variant.Blake2b, 520)));
        }

        /// <summary>
        /// Documents that BLAKE2s is part of the intended BLAKE2 family but is not implemented in this first slice.
        /// This prevents accidental success for an unsupported variant and gives the next implementation step a clear marker.
        /// </summary>
        [Test]
        public void ShouldRejectBlake2sUntilImplemented()
        {
            Assert.Throws<System.ArgumentException>(() =>
                new Blake2Factory().GetBlake2Instance(new Blake2HashFunction(Blake2Variant.Blake2s, 256)));
        }
    }
}
