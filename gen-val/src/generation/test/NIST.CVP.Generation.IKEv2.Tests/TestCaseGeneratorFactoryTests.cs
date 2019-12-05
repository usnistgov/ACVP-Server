using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.KDF_Components.v1_0.IKEv2;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.IKEv2.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        public void ShouldReturnProperGenerator()
        {
            var testGroup = new TestGroup
            {
                HashAlg =  new HashFunction(ModeValues.SHA1, DigestSizes.d160),
                TestType = "aft"
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(typeof(TestCaseGenerator), generator);
        }
    }
}
