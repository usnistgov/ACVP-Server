using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_ECB;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", "MultiBlockMessage", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Encrypt", "MultiBlockMessage", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("ENcrypt", "MultiBlockMessage", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Junk", "", typeof(TestCaseGeneratorNull))]
        [TestCase("", "", typeof(TestCaseGeneratorNull))]
        [TestCase("Encrypt", "", typeof(TestCaseGeneratorNull))]
        [TestCase("encrypt", "MCT", typeof(TestCaseGeneratorMonteCarloEncrypt))]
        [TestCase("Decrypt", "MultiBlockMessage", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("Decrypt", "MCT", typeof(TestCaseGeneratorMonteCarloDecrypt))]
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
            var algoMct = new Mock<ITDES_ECB_MCT>().Object;
            return new TestCaseGeneratorFactory(randy, algo, algoMct);
        }

      
    }
}
