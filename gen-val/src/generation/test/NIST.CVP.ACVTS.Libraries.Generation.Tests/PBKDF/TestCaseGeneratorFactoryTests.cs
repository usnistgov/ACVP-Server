using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Generation.PBKDF;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.PBKDF
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        public void ShouldReturnProperGenerator()
        {
            var testGroup = new TestGroup
            {
                HashAlg = new HashFunction(ModeValues.SHA1, DigestSizes.d160),
                TestType = "aft"
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.That(generator != null);
            Assert.That(generator, Is.InstanceOf(typeof(TestCaseGenerator)));
        }
    }
}
