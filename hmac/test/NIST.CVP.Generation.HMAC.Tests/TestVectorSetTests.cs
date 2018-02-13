using System;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.HMAC.Tests
{
    [TestFixture, UnitTest]
    public class TestVectorSetTests
    {
        private TestDataMother _tdm = new TestDataMother();

        [Test]
        public void ShouldHaveTheExpectedAnswerProjection()
        {
            var subject = GetSubject();
            var results = subject.AnswerProjection;
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(15, results[0].tests.Count);
        }

        [Test]
        public void ShouldHaveTheExpectedPromptProjection()
        {
            var subject = GetSubject();
            var results = subject.PromptProjection;
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(15, results[0].tests.Count);
        }

        [Test]
        public void ShouldHaveTheExpectedResultProjection()
        {
            var subject = GetSubject(2);
            var results = subject.ResultProjection;
            Assert.IsNotNull(results);
            Assert.AreEqual(30, results.Count);
        }

        [Test]
        public void ShouldReconstituteTestVectorFromAnswerAndPrompt()
        {
            var source = GetSubject(2).ToDynamic();
            var subject = new TestVectorSet(source);
            Assert.AreEqual(2, subject.TestGroups.Count);
        }

        [Test]
        public void ShouldContainElementsWithinAnswerProjection()
        {
            var subject = GetSubject(1);
            var results = subject.AnswerProjection;
            var group = results[0];
            Assert.IsTrue(!string.IsNullOrEmpty(group.testType.ToString()), nameof(group.testType));
            Assert.IsTrue(!string.IsNullOrEmpty(group.keyLen.ToString()), nameof(group.keyLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.msgLen.ToString()), nameof(group.msgLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.macLen.ToString()), nameof(group.macLen));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.key.ToString()), nameof(test.key));
                Assert.IsTrue(!string.IsNullOrEmpty(test.msg.ToString()), nameof(test.msg));
                Assert.IsTrue(!string.IsNullOrEmpty(test.mac.ToString()), nameof(test.mac));
                Assert.IsTrue(!string.IsNullOrEmpty(test.deferred.ToString()), nameof(test.deferred));
                Assert.IsTrue(!string.IsNullOrEmpty(test.failureTest.ToString()), nameof(test.failureTest));
            }
        }

        [Test]
        public void ShouldContainElementsWithinPromptProjection()
        {
            var subject = GetSubject(1);
            var results = subject.PromptProjection;
            var group = results[0];
            Assert.IsTrue(!string.IsNullOrEmpty(group.testType.ToString()), nameof(group.testType));
            Assert.IsTrue(!string.IsNullOrEmpty(group.keyLen.ToString()), nameof(group.keyLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.msgLen.ToString()), nameof(group.msgLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.macLen.ToString()), nameof(group.macLen));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.key.ToString()), nameof(test.key));
                Assert.IsTrue(!string.IsNullOrEmpty(test.msg.ToString()), nameof(test.msg));
            }
        }

        [Test]
        public void ShouldContainElementsWithinResultProjection()
        {
            var subject = GetSubject(1);
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(item.tcId.ToString()), nameof(item.tcId));
            }
        }

        [Test]
        public void ShouldIncludeMsgInAnswerProjection()
        {
            var subject = GetSubject(1);
            var results = subject.AnswerProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue((bool) !string.IsNullOrEmpty(test.msg.ToString()));
            }
        }

        [Test]
        public void ShouldIncludeMsgInPromptProjection()
        {
            var subject = GetSubject(1);
            var results = subject.PromptProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue((bool) !string.IsNullOrEmpty(test.msg.ToString()));
            }
        }

        [Test]
        public void ShouldIncludeMacInResultProjection()
        {
            var subject = GetSubject(1);
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                Assert.IsTrue((bool) !string.IsNullOrEmpty(item.mac.ToString()));
            }
        }
        
        private TestVectorSet GetSubject(int groups = 1)
        {
            var subject = new TestVectorSet {Algorithm = "HMAC-SHA-1"};
            var testGroups = _tdm.GetTestGroups(groups);
            subject.TestGroups = testGroups.Select(g => (ITestGroup)g).ToList();
            return subject;
        }
    }
}
