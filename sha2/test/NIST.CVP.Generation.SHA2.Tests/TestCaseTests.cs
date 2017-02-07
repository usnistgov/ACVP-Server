using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.SHA;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture]
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
            sourceTest.Add("digest", new JValue("00AA"));
            var subject = new TestCase(sourceTest);
            Assert.IsNotNull(subject);
        }

        [Test]
        public void ShouldNotReconstituteTestCaseFromJObjectWithouttcId_ThrowsException()
        {
            var sourceTest = new JObject();
            sourceTest.Add("digest", new JValue("00AA"));
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
        public void ShouldSetProperDeferredFromDynamicAnswerTest()
        {
            var sourceTest = GetSourceAnswerTest();
            var subject = new TestCase(sourceTest);
            Assume.That(subject != null);
            Assert.AreEqual(sourceTest.deferred, subject.Deferred);
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
        public void ShouldSetProperDigestFromDynamicAnswerTest()
        {
            var sourceTest = GetSourceAnswerTest();
            var subject = new TestCase(sourceTest);
            Assume.That(subject != null);
            Assert.AreEqual(sourceTest.digest, subject.Digest);
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
        [TestCase("message")]
        [TestCase("MESSAGE")]
        [TestCase("msg")]
        [TestCase("MSG")]
        public void ShouldSetMessage(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.Message.ToHex());
        }

        [Test]
        [TestCase("Digest")]
        [TestCase("digest")]
        [TestCase("dig")]
        [TestCase("DIG")]
        public void ShouldSetDigest(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.Digest.ToHex());
        }

        [Test]
        public void ShouldNotMergeTestsWithMismatchedIds()
        {
            var testCase = new TestCase { TestCaseId = 1 };
            var otherTestCase = new TestCase { TestCaseId = 2 };
            var mergeResult = testCase.Merge(otherTestCase);
            Assert.IsFalse(mergeResult);
        }

        //[Test]
        //[TestCase(null, null, null, null, false)]
        //[TestCase(null, "00BB", null, null, true)]
        //[TestCase("00BB", "00BB", null, null, false)]
        //[TestCase(null, null, "00BB", "00BB", false)]
        //[TestCase("00BB", "00BB", "00BB", "00BB", false)]
        //[TestCase(null, null, null, "00BB", true)]
        //[TestCase(null, "00BB", null, "00BB", true)]
        //[TestCase("00BB", null, "00BB", null, false)]
        //public void ShouldOnlyMergeWhenOriginalCipherTextOrPlaintextIsNullAndIsSuppliedByOther(string originalPlain, string suppliedPlain, string originalCipher, string suppliedCipher, bool expectedResult)
        //{
        //    var testCase = new TestCase { TestCaseId = 1 };
        //    SetBitString(testCase, "ct", originalCipher);
        //    SetBitString(testCase, "pt", originalPlain);
        //    var suppliedTestCase = new TestCase { TestCaseId = 1 };
        //    SetBitString(suppliedTestCase, "ct", suppliedCipher);
        //    SetBitString(suppliedTestCase, "pt", suppliedPlain);
        //    var mergeResult = testCase.Merge(suppliedTestCase);
        //    Assert.AreEqual(expectedResult, mergeResult);
        //}

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
            var hashFunction = new HashFunction()
            {
                Mode = ModeValues.SHA2,
                DigestSize = DigestSizes.d224
            };

            var sourceVector = new TestVectorSet() { TestGroups = _tdm.GetTestGroups(hashFunction).Select(g => (ITestGroup)g).ToList() };
            var sourceTest = sourceVector.AnswerProjection[0].tests[0];
            return sourceTest;
        }
    }
}
