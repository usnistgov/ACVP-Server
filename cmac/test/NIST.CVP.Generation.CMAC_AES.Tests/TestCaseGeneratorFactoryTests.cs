using System;
using Moq;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.CMAC.Enums;
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
        private Mock<IRandom800_90> _random;
        private Mock<ICmac> _cmac;
        private Mock<ICmacFactory> _cmacFactory;
        private TestCaseGeneratorFactory<TestGroup, TestCase> _subject;

        [SetUp]
        public void Setup()
        {
            _random = new Mock<IRandom800_90>();
            _cmac = new Mock<ICmac>();
            _cmacFactory = new Mock<ICmacFactory>();
            _subject = new TestCaseGeneratorFactory<TestGroup, TestCase>(_random.Object, _cmacFactory.Object);

            _cmacFactory
                .Setup(s => s.GetCmacInstance(It.IsAny<CmacTypes>()))
                .Returns(_cmac.Object);
        }

        [Test]
        [TestCase("gen", typeof(TestCaseGeneratorGen<TestGroup, TestCase>))]
        [TestCase("GeN", typeof(TestCaseGeneratorGen<TestGroup, TestCase>))]
        [TestCase("Ver", typeof(TestCaseGeneratorVer<TestGroup, TestCase>))]
        [TestCase("veR", typeof(TestCaseGeneratorVer<TestGroup, TestCase>))]
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
