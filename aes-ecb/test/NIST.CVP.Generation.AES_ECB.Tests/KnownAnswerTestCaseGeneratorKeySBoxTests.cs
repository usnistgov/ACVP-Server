using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.Tests
{
    public class KnownAnswerTestCaseGeneratorKeySBoxTests
    {
        [Test]
        [TestCase(128)]
        [TestCase(192)]
        [TestCase(256)]
        public void ShouldReturnResponseWithCollectionMatchingKeySize(int keyLength)
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyLength = keyLength
            };

            KnownAnswerTestCaseGeneratorKeySBox subject = new KnownAnswerTestCaseGeneratorKeySBox();
            var result = subject.Generate(testGroup);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.IsTrue(result.TestCases.All(a => a.Key.BitLength == keyLength), nameof(keyLength));
        }

        [Test]
        [TestCase(1)]
        [TestCase(100)]
        public void ShouldReturnErrorMessageOnInvalidKeySize(int keyLength)
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyLength = keyLength
            };

            KnownAnswerTestCaseGeneratorKeySBox subject = new KnownAnswerTestCaseGeneratorKeySBox();
            var result = subject.Generate(testGroup);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.IsTrue(!string.IsNullOrEmpty(result.ErrorMessage));
        }
    }
}
