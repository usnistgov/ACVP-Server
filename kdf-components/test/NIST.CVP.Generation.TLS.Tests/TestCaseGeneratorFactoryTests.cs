using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.Crypto.TLS;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TLS.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        public void ShouldReturnProperGenerator()
        {
            var testGroup = new TestGroup
            {
                TlsMode = TlsModes.v10v11,
                HashAlg =  new HashFunction(ModeValues.SHA1, DigestSizes.d160),
                TestType = "aft"
            };

            var subject = new TestCaseGeneratorFactory(null, new TlsKdfFactory());
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(typeof(TestCaseGenerator), generator);
        }
    }
}
