using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Crypto.CSHAKE;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TupleHash.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("junk", typeof(TestCaseGeneratorNull))]
        [TestCase("aFt", typeof(TestCaseGeneratorAFTHash))]
        [TestCase("Mct", typeof(TestCaseGeneratorMCTHash))]
        [TestCase("lTT", typeof(TestCaseGeneratorLTTHash))]
        public void ShouldReturnProperGenerator(string testType, Type expectedType)
        {
            var testGroup = new TestGroup
            {
                Function = "TupleHash",
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
        public void ShouldReturnSampleMonteCarloGeneratorIfRequested(bool isSample)
        {
            var testGroup = new TestGroup
            {
                Function = "TupleHash",
                DigestSize = 128,
                TestType = "MCT"
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);

            var typedGen = generator as TestCaseGeneratorMCTHash;
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
                    Function = "TupleHash",
                    DigestSize = 0,
                    TestType = ""
                }
                );
            Assert.IsNotNull(generator);
        }

        private TestCaseGeneratorFactory GetSubject()
        {
            var random = new Mock<IRandom800_90>().Object;
            var algo = new Mock<ITupleHash>().Object;
            var mctAlgo = new Mock<ITupleHash_MCT>().Object;

            return new TestCaseGeneratorFactory(random, algo, mctAlgo);
        }
    }
}
