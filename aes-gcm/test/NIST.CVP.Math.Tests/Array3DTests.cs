using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests
{
    [TestFixture]
    public class Array3DTests
    {
        [Test]
        public void ShouldHaveProperDimension1Size()
        {
            var subject = new Array3D(1,2,3);
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
        public void ShouldPullOutProperSubArray()
        {
            var supply = new byte[,,] { { { 1, 2, 3 }, { 4, 5, 6 } },
                                 { { 7, 8, 9 }, { 10, 11, 12 } } };

            var subject = new Array3D(supply);
            var subArray = subject.GetSubArray(1);


        }

    }
}
