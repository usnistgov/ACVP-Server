using System;
using Moq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.Generation.CMAC;
using NIST.CVP.Generation.CMAC.AES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC_AES.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        private Mock<IOracle> _oracle;
        private TestCaseGeneratorFactory<TestCaseGeneratorGen, TestCaseGeneratorVer, TestGroup, TestCase> _subject;

        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();
            _subject = new TestCaseGeneratorFactory<TestCaseGeneratorGen, TestCaseGeneratorVer, TestGroup, TestCase>(_oracle.Object);
        }

        [Test]
        [TestCase("gen", typeof(TestCaseGeneratorGen))]
        [TestCase("GeN", typeof(TestCaseGeneratorGen))]
        [TestCase("Ver", typeof(TestCaseGeneratorVer))]
        [TestCase("veR", typeof(TestCaseGeneratorVer))]
        [TestCase("Junk", typeof(TestCaseGeneratorNull<TestGroup, TestCase>))]
        [TestCase("", typeof(TestCaseGeneratorNull<TestGroup, TestCase>))]
        public void ShouldReturnProperGenerator(string direction, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = direction
            };

            var generator = _subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = string.Empty
            };

            var generator = _subject.GetCaseGenerator(testGroup);
            Assert.IsNotNull(generator);
        }
    }
}
