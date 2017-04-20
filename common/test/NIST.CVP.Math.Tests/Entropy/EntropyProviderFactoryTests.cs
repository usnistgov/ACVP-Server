using System;
using NUnit.Framework;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Math.Tests.Entropy
{
    [TestFixture]
    public class EntropyProviderFactoryTests
    {

        EntropyProviderFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new EntropyProviderFactory();
        }

        [Test]
        [TestCase(EntropyProviderTypes.Testable, typeof(TestableEntropyProvider))]
        [TestCase(EntropyProviderTypes.Random, typeof(EntropyProvider))]
        public void ShouldReturnRandomCorrectProvider(EntropyProviderTypes providerType, Type expectedType)
        {
            var result = _subject.GetEntropyProvider(providerType);

            Assert.IsInstanceOf(expectedType, result);
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenEnumInvalid()
        {
            int i = -1;
            var invalidEnum = (EntropyProviderTypes) i;

            Assert.Throws(typeof(ArgumentException), () => _subject.GetEntropyProvider(invalidEnum));
        }
    }
}
