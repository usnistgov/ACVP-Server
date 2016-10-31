using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests
{
    [TestFixture]
    public class Array2DTests
    {
        [Test]
        public void ShouldHaveProperDimension1Size()
        {
            var subject = new Array2D(1,2);
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
