using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests
{
    [TestFixture]
    public class Random800_90Tests
    {
        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void ShouldReturnZeroLngthBitStringForZeroOrLessLengths(int length)
        {
            var subject = new Random800_90();
            var result = subject.GetRandomBitString(length);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(16)]
        public void ShouldReturnSelectedLength(int length)
        {
            var subject = new Random800_90();
            var result = subject.GetRandomBitString(length);
            Assume.That(result != null);
            Assert.AreEqual(length, result.Length);
        }

      
    }
}
