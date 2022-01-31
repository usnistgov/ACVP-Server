using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.AES_ECB.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.ECB
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorKnownAnswerTests
    {
        private TestCaseGeneratorKnownAnswer _subject;

        [TestCase(128, "gfsbox")]
        [TestCase(192, "gfsbox")]
        [TestCase(256, "gfsbox")]
        [TestCase(128, "keysbox")]
        [TestCase(192, "keysbox")]
        [TestCase(256, "keysbox")]
        [TestCase(128, "vartxt")]
        [TestCase(192, "vartxt")]
        [TestCase(256, "vartxt")]
        [TestCase(128, "varkey")]
        [TestCase(192, "varkey")]
        [TestCase(256, "varkey")]
        public async Task ShouldReturnResponseWithCollectionMatchingKeySize(int keyLength, string katType)
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyLength = keyLength
            };

            _subject = new TestCaseGeneratorKnownAnswer(keyLength, katType);
            var result = await _subject.GenerateAsync(testGroup, false);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(keyLength, result.TestCase.Key.BitLength, nameof(keyLength));
        }

        [Test]
        [TestCase(1, "gfsbox")]
        [TestCase(2, "keysbox")]
        [TestCase(3, "vartxt")]
        [TestCase(4, "varkey")]
        public void ShouldThrowOnInvalidKeySize(int keyLength, string katType)
        {
            Assert.Throws(
                typeof(ArgumentException),
                () => _subject = new TestCaseGeneratorKnownAnswer(keyLength, katType)
            );
        }

        [Test]
        [TestCase(128, "gfsbox")]
        [TestCase(192, "gfsbox")]
        [TestCase(256, "gfsbox")]
        [TestCase(128, "keysbox")]
        [TestCase(192, "keysbox")]
        [TestCase(256, "keysbox")]
        [TestCase(128, "vartxt")]
        [TestCase(192, "vartxt")]
        [TestCase(256, "vartxt")]
        [TestCase(128, "varkey")]
        [TestCase(192, "varkey")]
        [TestCase(256, "varkey")]
        public async Task ShouldReturnSuccessResponseWhenKatsGenerated(int keyLength, string katType)
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyLength = keyLength
            };

            _subject = new TestCaseGeneratorKnownAnswer(keyLength, katType);

            var results = new List<TestCaseGenerateResponse<TestGroup, TestCase>>();
            for (int i = 0; i < _subject.NumberOfTestCasesToGenerate; i++)
            {
                results.Add(await _subject.GenerateAsync(testGroup, false));
            }

            Assert.IsTrue(results.TrueForAll(tfa => tfa.Success));
        }

        [Test]
        [TestCase(128, "gfsbox")]
        [TestCase(192, "gfsbox")]
        [TestCase(256, "gfsbox")]
        [TestCase(128, "keysbox")]
        [TestCase(192, "keysbox")]
        [TestCase(256, "keysbox")]
        [TestCase(128, "vartxt")]
        [TestCase(192, "vartxt")]
        [TestCase(256, "vartxt")]
        [TestCase(128, "varkey")]
        [TestCase(192, "varkey")]
        [TestCase(256, "varkey")]
        public async Task ShouldReturnFailureResponseWhenKatsExhausted(int keyLength, string katType)
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyLength = keyLength
            };

            _subject = new TestCaseGeneratorKnownAnswer(keyLength, katType);

            var results = new List<TestCaseGenerateResponse<TestGroup, TestCase>>();
            for (int i = 0; i <= _subject.NumberOfTestCasesToGenerate; i++)
            {
                results.Add(await _subject.GenerateAsync(testGroup, false));
            }

            Assert.IsTrue(results.Any(a => !a.Success));
        }
    }
}
