using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.IKEv1.Tests
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
            sourceTest.Add("sKeyId", new JValue("00AA"));
            var subject = new TestCase(sourceTest);
            Assert.IsNotNull(subject);
        }

        [Test]
        public void ShouldNotReconstituteTestCaseFromJObjectWithouttcId_ThrowsException()
        {
            var sourceTest = new JObject();
            sourceTest.Add("sKeyId", new JValue("00AA"));
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
        public void ShouldSetProperSKeyIdFromDynamicAnswerTest()
        {
            var sourceTest = GetSourceAnswerTest();
            var subject = new TestCase(sourceTest);
            Assume.That(subject != null);
            Assert.AreEqual(sourceTest.sKeyId, subject.SKeyId);
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
        [TestCase("nResp")]
        [TestCase("nr")]
        [TestCase("NR")]
        public void ShouldSetNResp(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.NResp.ToHex());
        }

        [Test]
        [TestCase("nInit")]
        [TestCase("ni")]
        [TestCase("NI")]
        public void ShouldSetNInit(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.NInit.ToHex());
        }

        private dynamic GetSourceAnswerTest()
        {
            var sourceVector = new TestVectorSet { TestGroups = _tdm.GetTestGroups().Select(g => (ITestGroup)g).ToList() };
            var sourceTest = sourceVector.AnswerProjection[0].tests[0];
            return sourceTest;
        }
    }
}
