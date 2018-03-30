using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TLS.Tests
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
            sourceTest.Add("masterSecret", new JValue("00AA"));
            var subject = new TestCase(sourceTest);
            Assert.IsNotNull(subject);
        }

        [Test]
        public void ShouldNotReconstituteTestCaseFromJObjectWithouttcId_ThrowsException()
        {
            var sourceTest = new JObject();
            sourceTest.Add("masterSecret", new JValue("00AA"));
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
        public void ShouldSetProperMasterSecretFromDynamicAnswerTest()
        {
            var sourceTest = GetSourceAnswerTest();
            var subject = new TestCase(sourceTest);
            Assume.That(subject != null);
            Assert.AreEqual(sourceTest.masterSecret, subject.MasterSecret);
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
        [TestCase("client_random")]
        [TestCase("clientRandom")]
        [TestCase("CLIENT_RANDOM")]
        public void ShouldSetClientRandom(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.ClientRandom.ToHex());
        }

        [Test]
        [TestCase("server_random")]
        [TestCase("serverRandom")]
        [TestCase("SERVER_RANDOM")]
        public void ShouldSetServerRandom(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.ServerRandom.ToHex());
        }

        private dynamic GetSourceAnswerTest()
        {
            var sourceVector = new TestVectorSet { TestGroups = _tdm.GetTestGroups().Select(g => (ITestGroup)g).ToList() };
            var sourceTest = sourceVector.AnswerProjection[0].tests[0];
            return sourceTest;
        }
    }
}
