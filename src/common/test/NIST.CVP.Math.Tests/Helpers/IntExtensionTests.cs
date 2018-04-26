using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests.Helpers
{
    [TestFixture, UnitTest]
    public class IntExtensionTests
    {
        [Test]
        [TestCase(1, 1, 1)]
        [TestCase(1, 100, 1)]
        [TestCase(200, 4, 50)]
        [TestCase(300, 7, 43)]
        [TestCase(104714, 198, 529)]
        [TestCase(981724, 8176, 121)]
        public void ShouldCeilingDivideProperly(int numerator, int denominator, int expectedResult)
        {
            Assert.AreEqual(expectedResult, numerator.CeilingDivide(denominator));
        }
    }
}
