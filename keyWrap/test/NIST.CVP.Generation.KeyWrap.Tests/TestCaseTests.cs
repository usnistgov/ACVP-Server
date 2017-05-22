using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KeyWrap.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseTests
    {
        private TestDataMother _tdm = new TestDataMother();

        [Test]
        public void ShouldReconstituteTestCaseFromDynamicAnswerTest()
        {
            var sourceTest = GetSourceAnswerTest();
            var subject = new TestCase(sourceTest);
            Assert.IsNotNull(subject);
        }

        [Test]
        public void ShouldReconstituteTestCaseFromProperJObject()
        {
            var sourceTest = new JObject();
            sourceTest.Add("tcId", new JValue(1));
            sourceTest.Add("cipherText", new JValue("00AA"));
            var subject = new TestCase(sourceTest);
            Assert.IsNotNull(subject);
        }

        [Test]
        public void ShouldNotReconstituteTestCaseFromJObjectWithouttcId_ThrowsException()
        {
            var sourceTest = new JObject();
            sourceTest.Add("cipherText", new JValue("00AA"));
            Assert.That(() => new TestCase(sourceTest), Throws.InstanceOf<RuntimeBinderException>());
        }

        [Test]
        public void ShouldSetProperTestCaseIdFromDynamicAnswerTest()
        {
            var sourceTest = GetSourceAnswerTest();
            var subject = new TestCase(sourceTest);
            Assume.That(subject != null);
            Assert.AreEqual(sourceTest.tcId, subject.TestCaseId);
        }

        [Test]
        public void ShouldSetProperFailureTestFromDynamicAnswerTest()
        {
            var sourceTest = GetSourceAnswerTest();
            var subject = new TestCase(sourceTest);
            Assume.That(subject != null);
            Assert.AreEqual(sourceTest.failureTest, subject.FailureTest);
        }

        [Test]
        public void ShouldSetProperCipherTextFromDynamicAnswerTest()
        {
            var sourceTest = GetSourceAnswerTest();
            var subject = new TestCase(sourceTest);
            Assume.That(subject != null);
            Assert.AreEqual(sourceTest.cipherText, subject.CipherText);
        }

        [Test]
        [TestCase("Fredo")]
        [TestCase("")]
        [TestCase("NULL")]
        [TestCase(null)]
        public void ShouldReturnFalseIfUnknownSetStringName(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsFalse(result);
        }
        
        [Test]
        [TestCase("key")]
        [TestCase("KEY")]
        [TestCase("K")]
        public void ShouldSetKey(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.Key.ToHex());
        }

        [Test]
        [TestCase("CipherText")]
        [TestCase("ciphertext")]
        [TestCase("ct")]
        [TestCase("CT")]
        [TestCase("C")]
        public void ShouldSetCipherText(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.CipherText.ToHex());
        }

        [Test]
        [TestCase("PlainText")]
        [TestCase("PLAINtext")]
        [TestCase("pt")]
        [TestCase("PT")]
        [TestCase("P")]
        public void ShouldSetPlainText(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.PlainText.ToHex());
        }

        [Test]
        public void ShouldNotMergeTestsWithMismatchedIds()
        {
            var testCase = new TestCase { TestCaseId = 1 };
            var otherTestCase = new TestCase { TestCaseId = 2 };
            var mergeResult = testCase.Merge(otherTestCase);
            Assert.IsFalse(mergeResult);
        }

        [Test]
        [TestCase(null, null, null, null, false)]
        [TestCase(null, "00BB", null, null, true)]
        [TestCase("00BB", "00BB", null, null, false)]
        [TestCase(null, null, "00BB", "00BB", false)]
        [TestCase("00BB", "00BB", "00BB", "00BB", false)]
        [TestCase(null, null, null, "00BB", true)]
        [TestCase(null, "00BB", null, "00BB", true)]
        [TestCase("00BB", null, "00BB", null, false)]
        public void ShouldOnlyMergeWhenOriginalCipherTextOrPlaintextIsNullAndIsSuppliedByOther(string originalPlain, string suppliedPlain, string originalCipher, string suppliedCipher, bool expectedResult)
        {
            var testCase = new TestCase { TestCaseId = 1 };
            SetBitString(testCase, "ct", originalCipher);
            SetBitString(testCase, "pt", originalPlain);
            var suppliedTestCase = new TestCase { TestCaseId = 1 };
            SetBitString(suppliedTestCase, "ct", suppliedCipher);
            SetBitString(suppliedTestCase, "pt", suppliedPlain);
            var mergeResult = testCase.Merge(suppliedTestCase);
            Assert.AreEqual(expectedResult, mergeResult);
        }

        private void SetBitString(TestCase testCase, string name, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            testCase.SetString(name, value);
        }

        private dynamic GetSourceAnswerTest()
        {
            var sourceVector = new TestVectorSet() { TestGroups = _tdm.GetTestGroups().Select(g => (ITestGroup)g).ToList() };
            var sourceTest = sourceVector.AnswerProjection[0].tests[0];
            return sourceTest;
        }
    }
}