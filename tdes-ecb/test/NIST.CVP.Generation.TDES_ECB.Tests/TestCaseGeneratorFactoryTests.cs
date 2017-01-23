using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        //[TestCase("encrypt", "MMT", typeof(TestCaseGeneratorMMTEncrypt))]
        //[TestCase("Encrypt", "MmT", typeof(TestCaseGeneratorMMTEncrypt))]
        //[TestCase("ENcrypt", "MMT", typeof(TestCaseGeneratorMMTEncrypt))]
        //[TestCase("Decrypt", "mMT", typeof(TestCaseGeneratorMMTDecrypt))]
        //[TestCase("decrypt", "MMt", typeof(TestCaseGeneratorMMTDecrypt))]
        //[TestCase("encrypt", "mCt", typeof(TestCaseGeneratorMCTEncrypt))]
        //[TestCase("Encrypt", "MCT", typeof(TestCaseGeneratorMCTEncrypt))]
        //[TestCase("ENcrypt", "mct", typeof(TestCaseGeneratorMCTEncrypt))]
        //[TestCase("Decrypt", "mct", typeof(TestCaseGeneratorMCTDecrypt))]
        //[TestCase("decrypt", "McT", typeof(TestCaseGeneratorMCTDecrypt))]
        [TestCase("Junk", "", typeof(TestCaseGeneratorNull))]
        [TestCase("", "", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string direction, string testType, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = direction,
                TestType = testType
            };

            var subject = new TestCaseGeneratorFactory(null, null);
            var generator = subject.GetCaseGenerator(new TestGroup());
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = new TestCaseGeneratorFactory(null, null);
            var generator = subject.GetCaseGenerator(new TestGroup());
            Assert.IsNotNull(generator);
        }
    }
}
