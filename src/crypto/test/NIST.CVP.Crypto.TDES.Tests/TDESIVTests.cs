using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TDES.Tests
{
    [TestFixture,  FastCryptoTest]
    public class TDESIVTests
    {
        [Test]
        public void ShouldCreateIVs()
        {
            var subject = new TDESIVs(new byte[] {0, 0, 0, 0, 0, 0, 0, 0});
            Assert.IsNotNull(subject.IVs);
        }

        [Test]
        public void ShouldCreateThreeIVs()
        {
            var subject = new TDESIVs(new byte[] {0, 0, 0, 0, 0, 0, 0, 0});
            Assume.That((bool) (subject.IVs != null));
            Assert.AreEqual(3, subject.IVs.Count);
        }

        [Test]
        public void ShouldCreateIV2AsR1AddedToIV1()
        {
            var subject = new TDESIVs(new byte[] {0, 0, 0, 0, 0, 0, 0, 0});
            Assume.That((bool) (subject.IVs != null));
            var iv2 = subject.IVs[1];
            Assert.AreEqual(subject.R1, iv2);
        }

        [Test]
        public void ShouldCreateIV3AsR2AddedToIV1()
        {
            var subject = new TDESIVs(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            Assume.That((bool) (subject.IVs != null));
            var iv3 = subject.IVs[2];
            Assert.AreEqual(subject.R2, iv3);
        }
    }
}
