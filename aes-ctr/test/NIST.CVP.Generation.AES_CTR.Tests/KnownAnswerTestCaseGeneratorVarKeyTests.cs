using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class KnownAnswerTestCaseGeneratorVarKeyTests
    {
        [Test]
        [TestCase(128)]
        [TestCase(192)]
        [TestCase(256)]
        public void ShouldReturnResponseWithCollectionMatchingKeySize(int keyLength)
        {
            var testGroup = new TestGroup
            {
                KeyLength = keyLength
            };

            var subject = new KnownAnswerTestCaseGeneratorVarKey();
            var result = subject.Generate(testGroup);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.IsTrue(result.TestCases.All(a => a.Key.BitLength == keyLength), nameof(keyLength));
        }

        [Test]
        [TestCase(1)]
        [TestCase(100)]
        public void ShouldThrowExceptionOnInvalidKeySize(int keyLength)
        {
            var testGroup = new TestGroup
            {
                KeyLength = keyLength
            };

            var subject = new KnownAnswerTestCaseGeneratorVarKey();

            Assert.Throws(Is.TypeOf<ArgumentException>(), () => subject.Generate(testGroup));
        }
    }
}
