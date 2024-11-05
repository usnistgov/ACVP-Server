using System;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Math.Tests.Entropy
{
    [TestFixture, UnitTest]
    public class EntropyProviderFactoryTests
    {
        [SetUp]
        public void Setup()
        {
            _subject = new EntropyProviderFactory();
        }

        EntropyProviderFactory _subject;

        [Test]
        [TestCase(EntropyProviderTypes.Testable, typeof(TestableEntropyProvider))]
        [TestCase(EntropyProviderTypes.Random, typeof(EntropyProvider))]
        public void ShouldReturnRandomCorrectProvider(EntropyProviderTypes providerType, Type expectedType)
        {
            var result = _subject.GetEntropyProvider(providerType);

            Assert.That(result, Is.InstanceOf(expectedType));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenEnumInvalid()
        {
            int i = -1;
            var invalidEnum = (EntropyProviderTypes)i;

            Assert.Throws(typeof(ArgumentException), () => _subject.GetEntropyProvider(invalidEnum));
        }
    }
}
