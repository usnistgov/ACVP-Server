using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseTests
    {
        private TestDataMother _tdm = new TestDataMother();

        [Test]
        public void ShouldReconstituteTestCaseFromDyanmicAnswerTest()
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
            sourceTest.Add("e", new JValue("00AA"));
            sourceTest.Add("n", new JValue("00AA"));
            sourceTest.Add("p", new JValue("00AA"));
            sourceTest.Add("q", new JValue("00AA"));
            sourceTest.Add("d", new JValue("00AA"));
            sourceTest.Add("seed", new JValue("00AA"));
            var subject = new TestCase(sourceTest);
            Assert.IsNotNull(subject);
        }

        [Test]
        public void ShouldNotReconstituteTestCaseFromJObjectWithout_tcId_ThrowsException()
        {
            var sourceTest = new JObject();
            sourceTest.Add("e", new JValue("00AA"));
            Assert.That(() => new TestCase(sourceTest), Throws.InstanceOf<RuntimeBinderException>());
        }

        [Test]
        public void ShouldSetProperTestIdFromDynamicAnswerTest()
        {
            var sourceTest = GetSourceAnswerTest();
            var subject = new TestCase(sourceTest);
            Assume.That(subject != null);
            Assert.AreEqual(sourceTest.tcId, subject.TestCaseId);
        }

        [Test]
        public void ShouldSetProperDFromDynamicAnswerTest()
        {
            var sourceTest = GetSourceAnswerTest();
            var subject = new TestCase(sourceTest);
            Assume.That(subject != null);
            Assert.AreEqual(sourceTest.d, new BitString(subject.Key.PrivKey.D));
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
        [TestCase("seed")]
        [TestCase("SeEd")]
        public void ShouldSetSeed(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual(new BitString("00AA"), subject.Seed);
        }

        [Test]
        public void ShouldNotMergeTestWithMismatchedIds()
        {
            var testCase = new TestCase {TestCaseId = 1};
            var otherTestCase = new TestCase {TestCaseId = 2};
            var mergeResult = testCase.Merge(otherTestCase);
            Assert.IsFalse(mergeResult);
        }

        private dynamic GetSourceAnswerTest()
        {
            var sourceVector = new TestVectorSet { TestGroups = _tdm.GetTestGroups().Select(g => (ITestGroup)g).ToList()};
            var sourceTest = sourceVector.AnswerProjection[0].tests[0];
            return sourceTest;
        }
    }
}
