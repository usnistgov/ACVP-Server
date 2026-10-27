using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Tests.Native
{
    [TestFixture, LongCryptoTest]
    public class XmssSigningTests
    {
        private Xmss _subject;
        private XmssKeyPairFactory _keyPairFactory;

        [OneTimeSetUp]
        public void Setup()
        {
            var shaFactory = new NativeShaFactory();
            var wots = new Wots(shaFactory);
            _subject = new Xmss(wots, shaFactory);
            _keyPairFactory = new XmssKeyPairFactory(wots, shaFactory);
        }

        [Test]
        [TestCase("xmss-h10.jsonl")]
        [TestCase("xmss-h16.jsonl")]
        public void WhenGivenReferenceSeed_ShouldProduceMatchingKeyPairAndSignatures(string file)
        {
            var vectors = ReferenceVectors.LoadXmss(file);
            Assert.That(vectors, Is.Not.Empty);

            foreach (var vector in vectors)
            {
                var attribute = AttributesHelper.GetXmssAttribute(vector.Mode);
                var seed = new BitString(vector.Seed).ToBytes();

                // store the full tree so signing at arbitrary indices is cheap
                var keyPair = _keyPairFactory.GetKeyPair(vector.Mode, seed, attribute.H);

                Assert.That(new BitString(keyPair.PublicKey.Key).ToHex(),
                    Is.EqualTo(new BitString(ReferenceVectors.PublicKeyFromVector(vector)).ToHex()).IgnoreCase,
                    $"public key for {vector.Mode}");

                foreach (var sig in vector.Sigs)
                {
                    var (expectedSignature, message) = ReferenceVectors.SignatureAndMessageFromVector(vector, sig);

                    keyPair.PrivateKey.SetIdx((int)sig.Idx);
                    var result = _subject.Sign(keyPair.PrivateKey, message);

                    Assert.That(result.Exhausted, Is.False, $"exhausted for {vector.Mode} idx {sig.Idx}");
                    Assert.That(new BitString(result.Signature).ToHex(),
                        Is.EqualTo(new BitString(expectedSignature).ToHex()).IgnoreCase,
                        $"signature for {vector.Mode} idx {sig.Idx}");
                }
            }
        }

        [Test]
        public void WhenSigningPastFinalLeafIndex_ShouldReportExhausted()
        {
            var vector = ReferenceVectors.LoadXmss("xmss-h10.jsonl")[0];
            var attribute = AttributesHelper.GetXmssAttribute(vector.Mode);
            var seed = new BitString(vector.Seed).ToBytes();
            var keyPair = _keyPairFactory.GetKeyPair(vector.Mode, seed, attribute.H);
            var message = new byte[] { 37 };

            keyPair.PrivateKey.SetIdx((1 << attribute.H) - 1);

            var lastSignature = _subject.Sign(keyPair.PrivateKey, message);
            Assert.That(lastSignature.Exhausted, Is.False, "final leaf index should sign");
            Assert.That(_subject.Verify(keyPair.PublicKey.Key, lastSignature.Signature, message).Success,
                Is.True, "final leaf signature should verify");

            var exhausted = _subject.Sign(keyPair.PrivateKey, message);
            Assert.That(exhausted.Exhausted, Is.True, "index past the final leaf should be exhausted");
            Assert.That(exhausted.Signature, Is.Null);
            Assert.That(keyPair.PrivateKey.IsExhausted, Is.True);
        }
    }
}
