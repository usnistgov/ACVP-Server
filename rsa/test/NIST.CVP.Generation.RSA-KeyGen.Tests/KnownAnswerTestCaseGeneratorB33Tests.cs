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
    public class KnownAnswerTestCaseGeneratorB33Tests
    {
        [Test]
        [TestCase(2048, PrimeTestModes.C2)]
        [TestCase(2048, PrimeTestModes.C3)]
        [TestCase(3072, PrimeTestModes.C2)]
        [TestCase(3072, PrimeTestModes.C3)]
        public void ShouldReturnNonNullResponseWithGoodData(int modulo, PrimeTestModes ptMode)
        {
            var testGroup = new TestGroup
            {
                Modulo = modulo,
                PrimeTest = ptMode
            };

            var subject = new KnownAnswerTestCaseGeneratorB33(testGroup);
            var result = subject.Generate(testGroup, false);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.TestCase);
        }

        [Test]
        [TestCase(4096, PrimeTestModes.C2)]
        [TestCase(4096, PrimeTestModes.C3)]
        [TestCase(1234, PrimeTestModes.C2)]
        [TestCase(4321, PrimeTestModes.C3)]
        public void ShouldThrowWithBadData(int modulo, PrimeTestModes ptMode)
        {
            var testGroup = new TestGroup
            {
                Modulo = modulo,
                PrimeTest = ptMode
            };

            Assert.Throws(typeof(ArgumentException), () => new KnownAnswerTestCaseGeneratorB33(testGroup));
        }
    }
}
