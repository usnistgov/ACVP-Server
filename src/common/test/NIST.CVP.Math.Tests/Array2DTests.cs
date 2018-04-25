using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests
{
    [TestFixture, UnitTest]
    public class Array2DTests
    {
        [Test]
        public void ShouldReturnInternalFromArrayWhenConstructedWithSupplyCtor()
        {
            var expected = new byte[,] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } };
            var subject = new Array2D(expected);

            Assert.AreEqual(expected, subject.Array);
        }

        [Test]
        public void ShouldHaveProperDimension1Size()
        {
            var subject = new Array2D(1, 2);
            Assert.AreEqual(1, subject.Dimension1Size);
        }

        [Test]
        public void ShouldHaveProperDimension2Size()
        {
            var subject = new Array2D(1, 2);
            Assert.AreEqual(2, subject.Dimension2Size);
        }
    }
}