using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.Fakes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.Tests
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
        public void ShouldSetProperDigestFromDynamicAnswerTest()
        {
            var sourceTest = GetSourceAnswerTest();
            var subject = new TestCase(sourceTest);
            Assume.That(subject != null);
            Assert.AreEqual(sourceTest.digest, subject.Digest.ToLittleEndianHex());
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

        // Note: These hex strings are little endian
        [Test]
        [TestCase("01", 2)]
        [TestCase("AB0C", 12)]
        [TestCase("AFF56703", 25)]
        public void ShouldSetMessageWithLengthAsLittleEndian(string hex, int length)
        {
            var expectedResult = new BitString(hex, length, false);
            var subject = new TestCase();
            var result = subject.SetString("msg", hex, length);
            Assert.IsTrue(result);
            Assert.AreEqual(expectedResult, subject.Message);
        }

        [Test]
        [TestCase("Digest")]
        [TestCase("digest")]
        [TestCase("dig")]
        [TestCase("DIG")]
        [TestCase("md")]
        public void ShouldSetDigest(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.Digest.ToHex());
        }

        // Note: These hex strings are little endian
        [Test]
        [TestCase("01", 2)]
        [TestCase("AB0C", 12)]
        [TestCase("AFF56703", 25)]
        public void ShouldSetDigestWithLengthAsLittleEndian(string hex, int length)
        {
            var expectedResult = new BitString(hex, length, false);
            var subject = new TestCase();
            var result = subject.SetString("md", hex, length);
            Assert.IsTrue(result);
            Assert.AreEqual(expectedResult, subject.Digest);
        }

        [Test]
        public void ShouldNotMergeTestsWithMismatchedIds()
        {
            var testCase = new TestCase { TestCaseId = 1 };
            var otherTestCase = new TestCase { TestCaseId = 2 };
            var mergeResult = testCase.Merge(otherTestCase);
            Assert.IsFalse(mergeResult);
        }

        private dynamic GetSourceAnswerTest()
        {
            var hashFunction = new HashFunction
            {
                Capacity = 448,
                DigestSize = 224,
                XOF = false
            };

            var sourceVector = new TestVectorSet() { TestGroups = _tdm.GetTestGroups(hashFunction).Select(g => (ITestGroup)g).ToList() };
            var sourceTest = sourceVector.AnswerProjection[0].tests[0];
            return sourceTest;
        }
    }
}
