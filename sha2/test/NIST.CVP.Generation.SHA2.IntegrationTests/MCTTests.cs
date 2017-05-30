using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.InternalAbstractions;
using Moq;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NLog;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class MCTTests
    {
        // Sample test, so this is the 3rd output (COUNT = 2) in the .fax file
        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, "faefffb2e59bf80e38a9f5e68a6ad23240bbc7d9", "e24e388ab9e6a95463817593821b5689bfa440f0")]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, "99f957f272b0acc5bfaf38c03078c88c9722671af04f7e399f5e4068", "9a67e8195a12d7cb41e7b94d952f3d1f353fe1f1d76a1025948b33d3")]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, "9c8a961df3e1b77fbdf46ae9389b6e2cbe559cd08ef5c960cf98ee09", "2c18c5304c725d8e62950c4b05ffa8000afa4ae2d737b0bfc3a3c561")]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, "2b2b2303fc764da5f33849264dd0cdf70a4d7cb99481aff5a156750d5a66d972", "5c2e3646a4b8469bce57d573702ad474a9766cbb856e70a0d5d35c62bc837cae")]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, "c6f92c9fd18c9b1b42cb617a4c735afbc9415fe37458160958e30e9888fb0b11", "9ba12c8d98ec4257dff85837f255aabce19cc1efc1158282712596f344662af1")]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, "dcab0ea121a6fc7ce794b11d018a405cbb36ee7c1b15950167e546c1e75e323eec1dcb3312e1266dc1f9a4dcfafdd1ea", "14fac6c33dba911ebe1ea25c9033956b79d7aa3499c65aee57de991fe7147cc5cae2058947c9fb71033e5a9c7fb72feb")]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, "6de26c41d68bfe20b4bd46eca6278ae3c11c1b1b8f8b80185731d09e9f0d60c4f0149e4ad9273d2c36429377b3f63f5c", "3a534e5fd50552e9eeb2f3fb2e4ede3bb1ca385e5f8205f38f87e9dead1e3b7f9e91cba4fab0a69aed84f32798907002")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, "4ae271af6853ffa1fc99641016593f6c73286391954d88a419c8403e46942ccde22a6f37494663c0c0ed8b2d06afc9292c2ad1e9a7cfde0ca4e9a4c59bda853b", "e2abda4dac460b00439ad1b666ab433b50501329115cee0d82c26674b0bd567bd139e35a9c51bc9536978af4ab6014f1513b634628846b3816ec7f010a213e36")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, "31c6a5609b55d67b8627ecd1f6bac0ed1f0badc9e10739e38f42f894f4ebae9d9800b4033fbe9d1eb35c18ecc3f818ab8cd233a6f7b8484f83c1658e453ee859", "98665db1fd37d0fe3b9f82da92d07c3e9dfd51f4e6565f6ffac7e28bf061b0af68d30f53f409d410ceeaac5cc369d44dd1245f1650dc0c1c627ff0a36990582f")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, "31d9679a1b363e8f0f293e4318da00b8c4f6909f8f537b1ba0d329c8", "0da91ad11678281e00ee79044277c309d9dd5bd76f48b4ee2ed3923f")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, "9d80bd871b384231b0009963b55b86e0b340766f27cb662d82330127", "94252bff98fdd3808f9dd372607602b648c3d38ab902a8d3f4c16709")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, "e3d9369970980731bea72e079e288c869b04bbedc799f4a5bbac2fd2d9a58b28", "f1677ccdba7da23a9ef085428a38c37c41f5b9cb93d474d4a723c2961970f1f0")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, "762c3e174e9666b190e0963503c56e57dfac88d8317dfef980b42d35b1b4b92f", "211b88a066c5f7ba21c6ff4ff17996517281266c70b24696e7d6c05aa2ed833e")]
        public void ShouldMonteCarloTestHashForSampleSuppliedCase(ModeValues mode, DigestSizes size, string message, string digest)
        {
            var subject = new SHA_MCT(new SHA());
            var messageBitString = new BitString(message);
            var hashFunction = new HashFunction
            {
                Mode = mode,
                DigestSize = size
            };

            var result = subject.MCTHash(hashFunction, messageBitString, true);

            Assume.That(result != null);
            Assume.That(result.Success);

            var resultDigest = result.Response[result.Response.Count - 1].Digest.ToHex();
            Assert.AreEqual(new BitString(digest).ToHex(), resultDigest);
        }

        [Test]
        [TestCase("9c8a961df3e1b77fbdf46ae9389b6e2cbe559cd08ef5c960cf98ee09", "3842db2b33a26db980d6f62def34efc4e0b802bf3d5b65ae8f12fc04")]
        [TestCase("99f957f272b0acc5bfaf38c03078c88c9722671af04f7e399f5e4068", "35bf53ae63fd979cc8cbbb05980d238edb05b0687158f600ed853ed4")]
        public void ShouldMonteCarloTestHashForFullCase(string message, string digest)
        {
            var subject = new SHA_MCT(new SHA());
            var messageBitString = new BitString(message);
            var hashFunction = new HashFunction
            {
                Mode = ModeValues.SHA2,
                DigestSize = DigestSizes.d224
            };

            var result = subject.MCTHash(hashFunction, messageBitString, false);

            Assume.That(result != null);
            Assume.That(result.Success);

            var resultDigest = result.Response[result.Response.Count - 1].Digest.ToHex();
            Assert.AreEqual(new BitString(digest).ToHex(), resultDigest);
        }
    }
}
