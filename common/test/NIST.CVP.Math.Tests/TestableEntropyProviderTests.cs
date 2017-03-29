using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests
{
    [TestFixture]
    public class TestableEntropyProviderTests
    {
        private TestableEntropyProvider _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestableEntropyProvider();
        }

        [Test]
        public void ShouldThrowExceptionWhenNoEntropy()
        {
            Assert.Throws(typeof(Exception), () => _subject.GetEntropy(0));
        }

        [Test]
        public void ShouldThrowExceptionWhenExhaustedEntropy()
        {
            _subject.AddEntropy(new BitString(1));
            _subject.AddEntropy(new BitString(2));

            _subject.GetEntropy(1);
            _subject.GetEntropy(2);
            Assert.Throws(typeof(Exception), () => _subject.GetEntropy(0));
        }

        [Test]
        public void ShouldThrowExceptionWhenEntropyLengthNotMatched()
        {
            _subject.AddEntropy(new BitString(1));
            Assert.Throws(typeof(ArgumentException), () => _subject.GetEntropy(0));
        }

        [Test]
        public void ShouldReturnEntropyFirstInFirstOut()
        {
            _subject.AddEntropy(new BitString(1));
            _subject.AddEntropy(new BitString(2));

            var first = _subject.GetEntropy(1);
            _subject.AddEntropy(new BitString(3));
            var second = _subject.GetEntropy(2);
            var third = _subject.GetEntropy(3);

            Assert.AreEqual(1, first.BitLength, nameof(first));
            Assert.AreEqual(2, second.BitLength, nameof(second));
            Assert.AreEqual(3, third.BitLength, nameof(third));
        }
    }
}
