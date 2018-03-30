using System;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests
{
    [TestFixture, UnitTest]
    public class Array3DTests
    {
        byte[,,] _working3DArray = new byte[2, 3, 4]
            {
                {
                    { 1, 2, 3, 4 },
                    { 5, 6, 7, 8 },
                    { 9, 10, 11, 12 }
                },
                {
                    { 13, 14, 15, 16 },
                    { 17, 18, 19, 20 },
                    { 21, 22, 23, 24 }
                }
            };

        [Test]
        public void ShouldHaveCorrect3DTestArray()
        {
            // Note this is mostly to ensure the _working3DArray stays the expected array, as tests rely on its specific makeup.
            var expectedArray = new byte[2, 3, 4]
            {
                {
                    { 1, 2, 3, 4 },
                    { 5, 6, 7, 8 },
                    { 9, 10, 11, 12 }
                },
                {
                    { 13, 14, 15, 16 },
                    { 17, 18, 19, 20 },
                    { 21, 22, 23, 24 }
                }
            };

            Assert.AreEqual(expectedArray, _working3DArray);
        }

        [Test]
        public void ShouldReturnInternalFromArrayWhenConstructedWithSupplyCtor()
        {
            var subject = new Array3D(_working3DArray);

            Assert.AreEqual(_working3DArray, subject.Array);
        }

        [Test]
        public void ShouldHaveProperDimension1Size()
        {
            var subject = new Array3D(1, 2, 3);
            Assert.AreEqual(1, subject.Dimension1Size);
        }

        [Test]
        public void ShouldHaveProperDimension2Size()
        {
            var subject = new Array3D(1, 2, 3);
            Assert.AreEqual(2, subject.Dimension2Size);
        }

        [Test]
        public void ShouldHaveProperDimension3Size()
        {
            var subject = new Array3D(1, 2, 3);
            Assert.AreEqual(3, subject.Dimension3Size);
        }

        [Test]
        public void ShouldThrowArgumentExceptionIfGetSubArrayDimension1IndexGtDimension1Index()
        {
            var subject = new Array3D(_working3DArray);
            Assert.Throws(
                typeof(ArgumentException),
                () => subject.GetSubArray(subject.Dimension1Size + 1)
            );
        }

        [Test]
        public void ShouldThrowArgumentExceptionIfGetSubArrayDimension1IndexLtZero()
        {
            var subject = new Array3D(_working3DArray);
            Assert.Throws(
                typeof(ArgumentException),
                () => subject.GetSubArray(-1)
            );
        }

        [Test]
        public void ShouldPullOutProperSubArray()
        {
            var subject = new Array3D(_working3DArray);
            var subArray = subject.GetSubArray(1);

            var expectedResults = new Array2D(new byte[3, 4]
            {
                { 13, 14, 15, 16 },
                { 17, 18, 19, 20 },
                { 21, 22, 23, 24 }
            });

            Assert.AreEqual(expectedResults.Array, subArray.Array);
        }

        [Test]
        public void ShouldReturnNullOnExceptionStaticSubArray()
        {
            var result = Array3D.GetSubArray(_working3DArray, -1);
            Assert.IsNull(result);
        }

        [Test]
        public void ShouldPullOutProperSubArrayStatic()
        {
            var subArray = Array3D.GetSubArray(_working3DArray, 1);

            var expectedResults = new Array2D(new byte[3, 4]
            {
                { 13, 14, 15, 16 },
                { 17, 18, 19, 20 },
                { 21, 22, 23, 24 }
            });

            Assert.AreEqual(expectedResults.Array, subArray);
        }
    }
}
