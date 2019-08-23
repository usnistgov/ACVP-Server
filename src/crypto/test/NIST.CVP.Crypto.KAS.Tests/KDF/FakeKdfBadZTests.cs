using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS.Fakes;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests.KDF
{
    [TestFixture, FastCryptoTest]
    public class FakeKdfBadZTests
    {
        [Test]
        public void OriginalSharedSecretShouldBeModified()
        {
            var kdfFactory = new KdfFactory(new ShaFactory(), new HmacFactory(new ShaFactory()));
            var fakeKdfFactory = new FakeKdfFactory_BadZ(kdfFactory);

            var originalZ = new BitString("01");
            var copyZ = originalZ.GetDeepCopy();

            Assert.AreEqual(copyZ, originalZ, "sanity check");

            var badKdf = fakeKdfFactory.GetInstance(
                KdfHashMode.Sha, 
                new HashFunction(ModeValues.SHA2, DigestSizes.d224)
            );

            badKdf.DeriveKey(originalZ, 256, new BitString(256));

            Assert.AreNotEqual(copyZ, originalZ, nameof(copyZ));
        }
    }
}
