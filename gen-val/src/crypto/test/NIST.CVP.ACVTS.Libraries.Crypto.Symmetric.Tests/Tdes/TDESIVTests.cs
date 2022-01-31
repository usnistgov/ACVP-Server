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
            Assert.IsNotNull(subject.IVs);
        }

        [Test]
        public void ShouldCreateThreeIVs()
        {
            var subject = new TDESIVs(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            Assert.That((bool)(subject.IVs != null));
            Assert.AreEqual(3, subject.IVs.Count);
        }

        [Test]
        public void ShouldCreateIV2AsR1AddedToIV1()
        {
            var subject = new TDESIVs(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            Assert.That((bool)(subject.IVs != null));
            var iv2 = subject.IVs[1];
            Assert.AreEqual(subject.R1, iv2);
        }

        [Test]
        public void ShouldCreateIV3AsR2AddedToIV1()
        {
            var subject = new TDESIVs(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            Assert.That((bool)(subject.IVs != null));
            var iv3 = subject.IVs[2];
            Assert.AreEqual(subject.R2, iv3);
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
                Assert.AreEqual(new BitString(hexIv1).ToHex(), ivs[0].ToHex(), "iv0");
                Assert.AreEqual(new BitString(expectedHexIv2).ToHex(), ivs[1].ToHex(), "iv1");
                Assert.AreEqual(new BitString(expectedHexIv3).ToHex(), ivs[2].ToHex(), "iv2");
            });
        }
    }
}
