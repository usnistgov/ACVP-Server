using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.TDES;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Tests.Tdes
{
    [TestFixture, FastCryptoTest]
    public class TDESIVTests
    {
        [Test]
        public void ShouldCreateIVs()
        {
            var subject = new TDESIVs(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            Assert.That(subject.IVs, Is.Not.Null);
        }

        [Test]
        public void ShouldCreateThreeIVs()
        {
            var subject = new TDESIVs(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            Assert.That((bool)(subject.IVs != null));
            Assert.That(subject.IVs.Count, Is.EqualTo(3));
        }

        [Test]
        public void ShouldCreateIV2AsR1AddedToIV1()
        {
            var subject = new TDESIVs(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            Assert.That((bool)(subject.IVs != null));
            var iv2 = subject.IVs[1];
            Assert.That(iv2, Is.EqualTo(subject.R1));
        }

        [Test]
        public void ShouldCreateIV3AsR2AddedToIV1()
        {
            var subject = new TDESIVs(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            Assert.That((bool)(subject.IVs != null));
            var iv3 = subject.IVs[2];
            Assert.That(iv3, Is.EqualTo(subject.R2));
        }

        [Test]
        [TestCase("0000000000000000", "5555555555555555", "AAAAAAAAAAAAAAAA")]
        [TestCase("1111111111111111", "6666666666666666", "BBBBBBBBBBBBBBBB")]
        [TestCase("1234567890ABCDEF", "6789ABCDE6012344", "BCDF01233B567899")]
        [TestCase("BEEFCAFEBEEFCAFE", "1445205414452053", "699A75A9699A75A8")]
        public void ShouldSetupInterleavePipelineIvs(string hexIv1, string expectedHexIv2, string expectedHexIv3)
        {
            var ivs = TdesPartitionHelpers.SetupIvs(new BitString(hexIv1));

            Assert.Multiple(() =>
            {
                Assert.That(ivs[0].ToHex(), Is.EqualTo(new BitString(hexIv1).ToHex()), "iv0");
                Assert.That(ivs[1].ToHex(), Is.EqualTo(new BitString(expectedHexIv2).ToHex()), "iv1");
                Assert.That(ivs[2].ToHex(), Is.EqualTo(new BitString(expectedHexIv3).ToHex()), "iv2");
            });
        }
    }
}
