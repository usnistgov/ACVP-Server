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
    public class KnownAnswerTestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase(KeyGenModes.B33, typeof(KnownAnswerTestCaseGeneratorB33))]
        [TestCase(KeyGenModes.B32, typeof(KnownAnswerTestCaseGeneratorNull))]
        [TestCase(KeyGenModes.B34, typeof(KnownAnswerTestCaseGeneratorNull))]
        [TestCase(KeyGenModes.B35, typeof(KnownAnswerTestCaseGeneratorNull))]
        [TestCase(KeyGenModes.B36, typeof(KnownAnswerTestCaseGeneratorNull))]
        public void ShouldReturnCorrectType(KeyGenModes mode, Type type)
        {
            var testGroup = new TestGroup
            {
                Mode = mode,
                TestType = "kat"
            };

            var subject = new KnownAnswerTestCaseGeneratorFactory();
            var result = subject.GetStaticCaseGenerator(testGroup);

            Assert.IsInstanceOf(type, result);
        }
    }
}
