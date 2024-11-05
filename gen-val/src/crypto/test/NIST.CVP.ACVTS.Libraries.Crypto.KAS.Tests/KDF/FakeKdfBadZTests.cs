using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Fakes;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF.OneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests.KDF
{
    [TestFixture, FastCryptoTest]
    public class FakeKdfBadZTests
    {
        [Test]
        public void OriginalSharedSecretShouldBeModified()
        {
            var kdfFactory = new KdfOneStepFactory(new NativeShaFactory(), new HmacFactory(new NativeShaFactory()), new KmacFactory(new cSHAKEWrapper()));
            var fakeKdfFactory = new FakeKdfFactory_BadZ(kdfFactory);

            var originalZ = new BitString("01");
            var copyZ = originalZ.GetDeepCopy();

            Assert.That(originalZ, Is.EqualTo(copyZ), "sanity check");

            var badKdf = fakeKdfFactory.GetInstance(KdaOneStepAuxFunction.SHA2_D256, true);

            badKdf.DeriveKey(originalZ, 256, new BitString(256), null);

            Assert.That(originalZ, Is.Not.EqualTo(copyZ), nameof(copyZ));
        }
    }
}
