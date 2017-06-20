using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase(KeyGenModes.B32, typeof(TestCaseGeneratorAFT_B32))]
        [TestCase(KeyGenModes.B33, typeof(TestCaseGeneratorGDT_B33))]
        [TestCase(KeyGenModes.B34, typeof(TestCaseGeneratorAFT_B34))]
        public void ShouldReturnProperGenerator(KeyGenModes mode, Type expectedType)
        {
            var testGroup = new TestGroup
            {
                Mode = mode,
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }
    }
}
