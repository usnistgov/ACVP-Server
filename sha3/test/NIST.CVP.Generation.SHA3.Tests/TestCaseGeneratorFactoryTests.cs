using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("junk", typeof(TestCaseGeneratorNull))]
        [TestCase("aFt", typeof(TestCaseGeneratorSHA3AFTHash))]
        [TestCase("Mct", typeof(TestCaseGeneratorSHA3MCTHash))]
        public void ShouldReturnProperSHA3Generator(string testType, Type expectedType)
        {
            var testGroup = new TestGroup
            {
                Function = "SHA3",
                DigestSize = 224,
                TestType = testType
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        [TestCase("junk", typeof(TestCaseGeneratorNull))]
        [TestCase("aFt", typeof(TestCaseGeneratorSHAKEAFTHash))]
        [TestCase("Mct", typeof(TestCaseGeneratorSHAKEMCTHash))]
        [TestCase("VOT", typeof(TestCaseGeneratorSHAKEVOTHash))]
        public void ShouldReturnProperSHAKEGenerator(string testType, Type expectedType)
        {
            var testGroup = new TestGroup
            {
                Function = "SHAKE",
                DigestSize = 128,
                TestType = testType
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReturnSampleSHA3MonteCarloGeneratorIfRequested(bool isSample)
        {
            var testGroup = new TestGroup
            {
                Function = "sha3",
                DigestSize = 224,
                TestType = "MCT"
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);

            var typedGen = generator as TestCaseGeneratorSHA3MCTHash;
            Assume.That(typedGen != null);

            var result = typedGen.Generate(testGroup, isSample);

            Assert.AreEqual(isSample, typedGen.IsSample);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReturnSampleSHAKEMonteCarloGeneratorIfRequested(bool isSample)
        {
            var testGroup = new TestGroup
            {
                Function = "shake",
                DigestSize = 128,
                TestType = "MCT"
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);

            var typedGen = generator as TestCaseGeneratorSHAKEMCTHash;
            Assume.That(typedGen != null);

            var result = typedGen.Generate(testGroup, isSample);

            Assert.AreEqual(isSample, typedGen.IsSample);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(
                new TestGroup
                {
                    Function = "SHA3",
                    DigestSize = 0,
                    TestType = ""}
                );
            Assert.IsNotNull(generator);
        }

        private TestCaseGeneratorFactory GetSubject()
        {
            var random = new Mock<IRandom800_90>().Object;
            var algo = new Mock<ISHA3>().Object;
            var mctAlgo = new Mock<ISHA3_MCT>().Object;

            return new TestCaseGeneratorFactory(random, algo, mctAlgo);
        }
    }
}
