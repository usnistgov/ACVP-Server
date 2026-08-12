using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Tests.Native
{
    [TestFixture, FastCryptoTest]
    public class WotsTests
    {
        private Wots _subject;

        [OneTimeSetUp]
        public void Setup()
        {
            _subject = new Wots(new NativeShaFactory());
        }

        [Test]
        public void WhenGivenReferenceVectors_ShouldProduceMatchingKeySignatureAndLeaf()
        {
            var vectors = ReferenceVectors.LoadWots("wots.jsonl");
            Assert.That(vectors, Is.Not.Empty);

            foreach (var vector in vectors)
            {
                var attribute = AttributesHelper.GetXmssAttribute(vector.Mode);
                var skSeed = new BitString(vector.SkSeed).ToBytes();
                var pubSeed = new BitString(vector.PubSeed).ToBytes();
                var message = new BitString(vector.M).ToBytes();

                var pk = _subject.PkGen(attribute, skSeed, pubSeed, (uint[])vector.Addr.Clone());
                Assert.That(new BitString(pk).ToHex(), Is.EqualTo(vector.Pk).IgnoreCase,
                    $"pkGen for {vector.Mode}");

                var signature = _subject.Sign(attribute, message, skSeed, pubSeed, (uint[])vector.Addr.Clone());
                Assert.That(new BitString(signature).ToHex(), Is.EqualTo(vector.Sig).IgnoreCase,
                    $"sign for {vector.Mode}");

                var pkCandidate = _subject.PkFromSig(attribute, signature, message, pubSeed,
                    (uint[])vector.Addr.Clone());
                Assert.That(new BitString(pkCandidate).ToHex(), Is.EqualTo(vector.Pk).IgnoreCase,
                    $"pkFromSig for {vector.Mode}");

                /* The leaf vector compresses the WOTS+ public key at hash address Addr2 with an
                   L-tree at hash address Addr. */
                var sha = XmssHelpers.GetSha(new NativeShaFactory(), vector.Mode);
                var buffer = new byte[AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(vector.Mode)];
                var leafWotsPk = _subject.PkGen(attribute, skSeed, pubSeed, (uint[])vector.Addr2.Clone());
                var leaf = XmssHelpers.LTree(sha, attribute, XmssHelpers.Unflatten(attribute, leafWotsPk), pubSeed,
                    (uint[])vector.Addr.Clone(), buffer);
                Assert.That(new BitString(leaf).ToHex(), Is.EqualTo(vector.Leaf).IgnoreCase,
                    $"leaf for {vector.Mode}");
            }
        }

        [Test]
        public void WhenGivenTamperedSignature_ShouldProduceDifferentPublicKeyCandidate()
        {
            var vectors = ReferenceVectors.LoadWots("wots.jsonl");
            var vector = vectors[0];

            var attribute = AttributesHelper.GetXmssAttribute(vector.Mode);
            var pubSeed = new BitString(vector.PubSeed).ToBytes();
            var message = new BitString(vector.M).ToBytes();

            var signature = new BitString(vector.Sig).ToBytes();
            signature[0] ^= 1;

            var pkCandidate = _subject.PkFromSig(attribute, signature, message, pubSeed,
                (uint[])vector.Addr.Clone());
            Assert.That(new BitString(pkCandidate).ToHex(), Is.Not.EqualTo(vector.Pk).IgnoreCase);
        }
    }
}
