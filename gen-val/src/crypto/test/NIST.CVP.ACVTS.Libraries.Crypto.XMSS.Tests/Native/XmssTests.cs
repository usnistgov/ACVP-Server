using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Tests.Native
{
    [TestFixture, FastCryptoTest]
    public class XmssTests
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
        public void WhenGivenReferenceVectors_ShouldVerifySuccessfully(string file)
        {
            var vectors = ReferenceVectors.LoadXmss(file);
            Assert.That(vectors, Is.Not.Empty);

            foreach (var vector in vectors)
            {
                Assert.That(vector.Mode, Is.Not.EqualTo(XmssMode.Invalid), $"unknown oid {vector.Oid}");
                var publicKey = ReferenceVectors.PublicKeyFromVector(vector);

                foreach (var sig in vector.Sigs)
                {
                    var (signature, message) = ReferenceVectors.SignatureAndMessageFromVector(vector, sig);
                    Assert.That(new BitString(message).ToHex(), Is.EqualTo(sig.Msg).IgnoreCase);

                    var result = _subject.Verify(publicKey, signature, message);
                    Assert.That(result.Success, Is.True,
                        $"verify for {vector.Mode} idx {sig.Idx}: {result.ErrorMessage}");
                }
            }
        }

        [Test]
        public void WhenGivenTamperedInput_ShouldFailToVerify()
        {
            var vector = ReferenceVectors.LoadXmss("xmss-h10.jsonl").First();
            var attribute = AttributesHelper.GetXmssAttribute(vector.Mode);
            var publicKey = ReferenceVectors.PublicKeyFromVector(vector);
            var (signature, message) = ReferenceVectors.SignatureAndMessageFromVector(vector, vector.Sigs.First());

            Assert.Multiple(() =>
            {
                // baseline sanity
                Assert.That(_subject.Verify(publicKey, signature, message).Success, Is.True, "untampered");

                // tampered message
                var badMessage = (byte[])message.Clone();
                badMessage[0] ^= 1;
                Assert.That(_subject.Verify(publicKey, signature, badMessage).Success, Is.False,
                    "tampered message");

                // tampered randomizer r
                var badSignature = (byte[])signature.Clone();
                badSignature[4] ^= 1;
                Assert.That(_subject.Verify(publicKey, badSignature, message).Success, Is.False,
                    "tampered r");

                // tampered WOTS+ signature
                badSignature = (byte[])signature.Clone();
                badSignature[4 + attribute.N] ^= 1;
                Assert.That(_subject.Verify(publicKey, badSignature, message).Success, Is.False,
                    "tampered sig_ots");

                // tampered auth path
                badSignature = (byte[])signature.Clone();
                badSignature[badSignature.Length - 1] ^= 1;
                Assert.That(_subject.Verify(publicKey, badSignature, message).Success, Is.False,
                    "tampered auth");

                // tampered idx_sig
                badSignature = (byte[])signature.Clone();
                badSignature[3] ^= 1;
                Assert.That(_subject.Verify(publicKey, badSignature, message).Success, Is.False,
                    "tampered idx_sig");

                // idx_sig beyond the height of the tree
                badSignature = (byte[])signature.Clone();
                badSignature[0] ^= 0x80;
                Assert.That(_subject.Verify(publicKey, badSignature, message).Success, Is.False,
                    "idx_sig out of range");

                // truncated signature
                Assert.That(_subject.Verify(publicKey, signature.Take(signature.Length - 1).ToArray(), message)
                    .Success, Is.False, "truncated signature");

                // tampered root in the public key
                var badPublicKey = (byte[])publicKey.Clone();
                badPublicKey[4] ^= 1;
                Assert.That(_subject.Verify(badPublicKey, signature, message).Success, Is.False,
                    "tampered root");

                // unknown OID in the public key
                badPublicKey = (byte[])publicKey.Clone();
                badPublicKey[3] = 0xFF;
                Assert.That(_subject.Verify(badPublicKey, signature, message).Success, Is.False,
                    "unknown OID");
            });
        }

        [Test]
        public void WhenGivenWrongSeedLength_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                _keyPairFactory.GetKeyPair(XmssMode.XMSS_SHA2_10_256, new byte[95]));
        }
    }
}
