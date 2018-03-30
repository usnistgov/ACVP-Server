using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.HMAC.Tests
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
            sourceTest.Add("msg", new JValue("00AA"));
            var subject = new TestCase(sourceTest);
            Assert.IsNotNull(subject);
        }

        [Test]
        
        public void ShouldNotReconstituteTestCaseFromJObjectWithouttcId_ThrowsException()
        {
            var sourceTest = new JObject();
            sourceTest.Add("msg", new JValue("00AA"));
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
        public void ShouldSetProperKeyFromDynamicAnswerTest()
        {
            var sourceTest = GetSourceAnswerTest();
            var subject = new TestCase(sourceTest);
            Assume.That(subject != null);
            Assert.AreEqual(sourceTest.key, subject.Key);
        }

        [Test]
        public void ShouldSetProperMsgFromDynamicAnswerTest()
        {
            var sourceTest = GetSourceAnswerTest();
            var subject = new TestCase(sourceTest);
            Assume.That(subject != null);
            Assert.AreEqual(sourceTest.msg, subject.Message);
        }

        [Test]
        public void ShouldSetProperMacFromDynamicAnswerTest()
        {
            var sourceTest = GetSourceAnswerTest();
            var subject = new TestCase(sourceTest);
            Assume.That(subject != null);
            Assert.AreEqual(sourceTest.mac, subject.Mac);
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
        [TestCase("msg")]
        [TestCase("mac")]
        public void ShouldSetNullValues(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, null);
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("key")]
        [TestCase("KEY")]
        public void ShouldSetKey(string name)
        {
            const string value = "00AA";

            var subject = new TestCase();
            var result = subject.SetString(name, value);
            Assert.IsTrue(result);
            Assert.AreEqual(value, subject.Key.ToHex());
        }
        
        [Test]
        [TestCase("msg")]
        [TestCase("MsG")]
        public void ShouldSetMessage(string name)
        {
            const string value = "00AA";

            var subject = new TestCase();
            var result = subject.SetString(name, value);
            Assert.IsTrue(result);
            Assert.AreEqual(value, subject.Message.ToHex());
        }

        [Test]
        [TestCase("mac")]
        [TestCase("MaC")]
        public void ShouldSetMac(string name)
        {
            const string value = "00AA";

            var subject = new TestCase();
            var result = subject.SetString(name, value);
            Assert.IsTrue(result);
            Assert.AreEqual(value, subject.Mac.ToHex());
        }
        
        private dynamic GetSourceAnswerTest()
        {
            var sourceVector = new TestVectorSet() {TestGroups = _tdm.GetTestGroups().Select(g => (ITestGroup) g).ToList()};
            var sourceTest = sourceVector.AnswerProjection[0].tests[0];
            return sourceTest;
        }
    }
}
