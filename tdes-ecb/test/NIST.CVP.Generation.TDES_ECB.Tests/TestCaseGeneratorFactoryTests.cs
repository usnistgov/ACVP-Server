using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", "MultiBlockMessage", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Encrypt", "MultiBlockMessage", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("ENcrypt", "MultiBlockMessage", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Junk", "", typeof(TestCaseGeneratorNull))]
        [TestCase("", "", typeof(TestCaseGeneratorNull))]
        [TestCase("Encrypt", "", typeof(TestCaseGeneratorNull))]
        [TestCase("encrypt", "MonteCarlo", typeof(TestCaseGeneratorMonteCarloEncrypt))]
        [TestCase("Decrypt", "MultiBlockMessage", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("Decrypt", "MonteCarlo", typeof(TestCaseGeneratorMonteCarloDecrypt))]
        public void ShouldReturnProperGenerator(string direction, string testType, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = direction,
                TestType = testType
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup, false);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReturSampleMonteCarloGeneratorIfRequested(bool isSample)
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = "encrypt",
                TestType = "MonteCarlo"
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup, isSample);
            Assume.That(generator != null);
            var typedGen = generator as TestCaseGeneratorMonteCarloEncrypt;
            Assume.That(typedGen != null);
            Assert.AreEqual(isSample, typedGen.IsSample);
        }


        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(new TestGroup {Function = "", TestType = ""}, false);
            Assert.IsNotNull(generator);
        }

        private TestCaseGeneratorFactory GetSubject()
        {
            var randy = new Mock<IRandom800_90>().Object;
            var algo = new Mock<ITDES_ECB>().Object;
            var keyMaker = new Mock<IMonteCarloKeyMaker>().Object;
            return new TestCaseGeneratorFactory(randy, algo, keyMaker);
        }

      
    }
}
