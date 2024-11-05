using System;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.CMAC
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        private Mock<IOracle> _oracle;
        private TestCaseGeneratorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();
            _subject = new TestCaseGeneratorFactory(_oracle.Object);
        }

        [Test]
        [TestCase(CmacTypes.AES128, "gen", typeof(TestCaseGeneratorGenAes))]
        [TestCase(CmacTypes.AES128, "GeN", typeof(TestCaseGeneratorGenAes))]
        [TestCase(CmacTypes.AES128, "Ver", typeof(TestCaseGeneratorVerAes))]
        [TestCase(CmacTypes.AES128, "veR", typeof(TestCaseGeneratorVerAes))]
        [TestCase(CmacTypes.AES192, "gen", typeof(TestCaseGeneratorGenAes))]
        [TestCase(CmacTypes.AES192, "GeN", typeof(TestCaseGeneratorGenAes))]
        [TestCase(CmacTypes.AES192, "Ver", typeof(TestCaseGeneratorVerAes))]
        [TestCase(CmacTypes.AES192, "veR", typeof(TestCaseGeneratorVerAes))]
        [TestCase(CmacTypes.AES256, "gen", typeof(TestCaseGeneratorGenAes))]
        [TestCase(CmacTypes.AES256, "GeN", typeof(TestCaseGeneratorGenAes))]
        [TestCase(CmacTypes.AES256, "Ver", typeof(TestCaseGeneratorVerAes))]
        [TestCase(CmacTypes.AES256, "veR", typeof(TestCaseGeneratorVerAes))]
        [TestCase(CmacTypes.TDES, "gen", typeof(TestCaseGeneratorGenTdes))]
        [TestCase(CmacTypes.TDES, "GeN", typeof(TestCaseGeneratorGenTdes))]
        [TestCase(CmacTypes.TDES, "Ver", typeof(TestCaseGeneratorVerTdes))]
        [TestCase(CmacTypes.TDES, "veR", typeof(TestCaseGeneratorVerTdes))]

        [TestCase(CmacTypes.AES128, "Junk", typeof(TestCaseGeneratorNull))]
        [TestCase(CmacTypes.AES128, "", typeof(TestCaseGeneratorNull))]
        [TestCase(CmacTypes.AES192, "Junk", typeof(TestCaseGeneratorNull))]
        [TestCase(CmacTypes.AES192, "", typeof(TestCaseGeneratorNull))]
        [TestCase(CmacTypes.AES256, "Junk", typeof(TestCaseGeneratorNull))]
        [TestCase(CmacTypes.AES256, "", typeof(TestCaseGeneratorNull))]
        [TestCase(CmacTypes.TDES, "Junk", typeof(TestCaseGeneratorNull))]
        [TestCase(CmacTypes.TDES, "", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(CmacTypes cmacType, string direction, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                CmacType = cmacType,
                Function = direction
            };

            var generator = _subject.GetCaseGenerator(testGroup);
            Assert.That(generator != null);
            Assert.That(generator, Is.InstanceOf(expectedType));
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = string.Empty
            };

            var generator = _subject.GetCaseGenerator(testGroup);
            Assert.That(generator, Is.Not.Null);
        }
    }
}
