using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SRTP;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SRTP
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        public void ShouldReturnProperGenerator()
        {
            var testGroup = new TestGroup
            {
                TestType = "aft"
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.That(generator != null);
            Assert.That(generator, Is.InstanceOf(typeof(TestCaseGenerator)));
        }
    }
}
