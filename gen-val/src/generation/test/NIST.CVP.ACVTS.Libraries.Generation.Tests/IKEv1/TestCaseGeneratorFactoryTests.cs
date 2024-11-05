using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv1;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.IKEv1
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        public void ShouldReturnProperGenerator()
        {
            var testGroup = new TestGroup
            {
                AuthenticationMethod = AuthenticationMethods.Dsa,
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
