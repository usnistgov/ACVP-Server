using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Math.Tests
{
    [TestFixture, UnitTest]
    public class Array2DTests
    {
        [Test]
        public void ShouldReturnInternalFromArrayWhenConstructedWithSupplyCtor()
        {
            var expected = new byte[,] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } };
            var subject = new Array2D(expected);

            Assert.That(subject.Array, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldHaveProperDimension1Size()
        {
            var subject = new Array2D(1, 2);
            Assert.That(subject.Dimension1Size, Is.EqualTo(1));
        }

        [Test]
        public void ShouldHaveProperDimension2Size()
        {
            var subject = new Array2D(1, 2);
            Assert.That(subject.Dimension2Size, Is.EqualTo(2));
        }
    }
}
