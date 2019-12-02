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

        [Test]
        [TestCase(1, 2, 2)]
        [TestCase(3, 2, 4)]
        [TestCase(4, 4, 4)]
        [TestCase(4, 10, 10)]
        [TestCase(1, 1024, 1024)]
        [TestCase(1024, 32, 1024)]
        [TestCase(1018, 32, 1024)]
        [TestCase(1024, 1024, 1024)]
        [TestCase(1025, 1024, 2048)]
        public void ShouldValueToModCorrectly(int value, int modulo, int expectedValue)
        {
            var result = value.ValueToMod(modulo);
            
            Assert.AreEqual(expectedValue, result);
        }
    }
}
