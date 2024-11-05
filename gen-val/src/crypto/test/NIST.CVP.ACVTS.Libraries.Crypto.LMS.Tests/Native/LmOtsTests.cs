using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Tests.Native
{
    [TestFixture, FastCryptoTest]
    public class LmOtsTests
    {
        private readonly IShaFactory _shaFactory = new NativeShaFactory();
        private LmOts _subject;
        private ILmOtsKeyPairFactory _keyPairFactory;
        private ILmOtsRandomizerC _randomizerC;

        [OneTimeSetUp]
        public void Setup()
        {
            _subject = new LmOts(_shaFactory);
            _keyPairFactory = new LmOtsKeyPairFactory(_shaFactory);
            _randomizerC = new LmOtsPseudoRandomizerC(_shaFactory);
        }

        [Test]
        public void UsingSeedShouldReturnUniquePerIndexX()
        {
            var key = _keyPairFactory.GetKeyPair(LmOtsMode.LMOTS_SHA256_N24_W1, new byte[16], new byte[4], new byte[24]);
            var x = key.PrivateKey.X;

            var hexes = new List<string>();
            foreach (var item in x)
            {
                hexes.Add(new BitString(item).ToHex());
            }

            Assert.That(hexes.Distinct().Count() == hexes.Count, Is.True);
        }

        [Test]
        [TestCase(LmOtsMode.LMOTS_SHA256_N24_W1)]
        [TestCase(LmOtsMode.LMOTS_SHA256_N24_W2)]
        [TestCase(LmOtsMode.LMOTS_SHA256_N24_W4)]
        [TestCase(LmOtsMode.LMOTS_SHA256_N24_W8)]
        [TestCase(LmOtsMode.LMOTS_SHA256_N32_W1)]
        [TestCase(LmOtsMode.LMOTS_SHA256_N32_W2)]
        [TestCase(LmOtsMode.LMOTS_SHA256_N32_W4)]
        [TestCase(LmOtsMode.LMOTS_SHA256_N32_W8)]
        [TestCase(LmOtsMode.LMOTS_SHAKE_N24_W1)]
        [TestCase(LmOtsMode.LMOTS_SHAKE_N24_W2)]
        [TestCase(LmOtsMode.LMOTS_SHAKE_N24_W4)]
        [TestCase(LmOtsMode.LMOTS_SHAKE_N24_W8)]
        [TestCase(LmOtsMode.LMOTS_SHAKE_N32_W1)]
        [TestCase(LmOtsMode.LMOTS_SHAKE_N32_W2)]
        [TestCase(LmOtsMode.LMOTS_SHAKE_N32_W4)]
        [TestCase(LmOtsMode.LMOTS_SHAKE_N32_W8)]
        public void WhenGivenSeedConstructedKeyPairAndMessage_ShouldSignAndVerifySuccessfully(LmOtsMode mode)
        {
            var attribute = AttributesHelper.GetLmOtsAttribute(mode);
            var key = _keyPairFactory.GetKeyPair(mode, new byte[16], new byte[4], new byte[attribute.N]);

            var message = new byte[16];

            var signature = _subject.Sign(key.PrivateKey, _randomizerC, message);
            var verify = _subject.Verify(key.PublicKey, signature, message);

            Assert.That(verify, Is.True);
        }
    }
}
