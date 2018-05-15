using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CBCI.Tests
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
        public void ShouldSetNullValues(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, null);
            Assert.IsTrue(result);

        }



        [Test]
        [TestCase("key")]
        [TestCase("keys")]
        public void ShouldSetKey(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA00AA00AA00AA00AA00AA00AA00AA00AA00AA00AA00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA00AA00AA00AA00AA00AA00AA00AA00AA00AA00AA00AA", subject.Keys.ToHex());
        }



        [Test]
        [TestCase("CipherText")]
        [TestCase("ciphertext")]
        [TestCase("ct")]
        [TestCase("CT")]
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
        public void ShouldSetPlainText(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.PlainText.ToHex());
        }

        [Test]
        [TestCase("Initialization vector")]
        [TestCase("INITIalization VECTOR")]
        [TestCase("iv")]
        [TestCase("IV")]
        public void ShouldSetIv(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.IV1.ToHex());
        }

        private dynamic GetSourceAnswerTest()
        {
            var sourceVector = new TestVectorSet() { TestGroups = _tdm.GetTestGroups().Select(g => (ITestGroup)g).ToList() };
            var sourceTest = sourceVector.AnswerProjection[0].tests[0];
            return sourceTest;
        }
    }
}
